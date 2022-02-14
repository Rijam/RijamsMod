using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class CresceptreSphere : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 2;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.magic = true;
			projectile.penetrate = 1;
			aiType = -1;
			projectile.timeLeft = 600;
			drawOffsetX = -12;
			projectile.netImportant = true;
		}

		public override void AI()
		{
			projectile.ai[0]++;
			if (projectile.ai[0] == 2) //Update every 2 ticks
            {
				int newTarget = FindTargetWithLineOfSight();
				if (newTarget != -1) //fly to the target
				{
					float speed = 14f;
					float inertia = 14f;
					NPC nPC2 = Main.npc[newTarget];
					float distanceFromTarget = projectile.Distance(nPC2.Center);
					if (distanceFromTarget < 1000f)
					{
						Vector2 direction = nPC2.Center - projectile.Center;
						direction.Normalize();
						direction *= speed;
						projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
						projectile.netUpdate = true;
					}
				}
				projectile.ai[0] = 0;
				projectile.netUpdate = true;
			}

			#region Animation and visuals

			// Set both direction and spriteDirection to 1 or -1 (right and left respectively)
			// projectile.direction is automatically set correctly in Projectile.Update, but we need to set it here or the textures will draw incorrectly on the 1st frame.
			projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
			// Adding Pi to rotation if facing left corrects the drawing
			projectile.rotation = projectile.velocity.ToRotation() + (projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
			if (projectile.spriteDirection == 1) // facing right
			{
				drawOffsetX = -12; // These values match the values in SetDefaults
				drawOriginOffsetY = 0;
				drawOriginOffsetX = 6;
			}
			else
			{
				// Facing left.
				// You can figure these values out if you flip the sprite in your drawing program.
				drawOffsetX = 0; // 0 since now the top left corner of the hitbox is on the far left pixel.
				drawOriginOffsetY = 0; // doesn't change
				drawOriginOffsetX = -6; // Math works out that this is negative of the other value.
			}

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 4;
			projectile.frameCounter++;
			if (projectile.frameCounter >= frameSpeed)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
			}

			// Some visuals here
			Lighting.AddLight(projectile.Center, Color.Purple.ToVector3() * 0.85f);
			#endregion
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.Kill();
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			projectile.Kill();
			return false;
		}
		//Copied from vanila (1.4) Projectiles.cs
		public int FindTargetWithLineOfSight(float maxRange = 800f)
		{
			float newMaxRange = maxRange;
			int result = -1;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				bool nPCCanBeChased = nPC.CanBeChasedBy(this);
				if (projectile.localNPCImmunity[i] != 0)
				{
					nPCCanBeChased = false;
				}
				if (nPCCanBeChased)
				{
					float projDist = projectile.Distance(Main.npc[i].Center);
					if (projDist < newMaxRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, nPC.position, nPC.width, nPC.height))
					{
						newMaxRange = projDist;
						result = i;
					}
				}
			}
			return result;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//This isn't perfect (especially when the projectile is going any way but straight right or left), but I can't figure out how to fix it.
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(projectile.direction >= 0 ? -12f : 0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length)* 0.5f;
				Texture2D texture = Main.projectileTexture[projectile.type];
				Rectangle frame = Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
				SpriteEffects spriteEffects = projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				spriteBatch.Draw(texture, drawPos, frame, color, projectile.rotation, drawOrigin, projectile.scale, spriteEffects, 0f);
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/CresceptreSphereExplode").WithPitchVariance(.05f), projectile.position);
			for (int i = 0; i < 30; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.PurpleTorch);
				Main.dust[dustIndex].velocity *= 1.4f;
			}
		}

		public override Color? GetAlpha(Color lightColor) => Color.White; //Fullbright
	}
}