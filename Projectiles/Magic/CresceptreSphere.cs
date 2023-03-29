using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Magic
{
	public class CresceptreSphere : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			AIType = -1;
			Projectile.timeLeft = 600;
			DrawOffsetX = -12;
			Projectile.netImportant = true;
		}

		public override void AI()
		{
			Projectile.ai[0]++;
			if (Projectile.ai[0] == 2) //Update every 2 ticks
			{
				int newTarget = FindTargetWithLineOfSight();
				Projectile.netUpdate = true;
				if (newTarget != -1) //fly to the target
				{
					float speed = 14f;
					float inertia = 14f;
					NPC nPC2 = Main.npc[newTarget];
					float distanceFromTarget = Projectile.Distance(nPC2.Center);
					if (distanceFromTarget < 1000f)
					{
						Vector2 direction = nPC2.Center - Projectile.Center;
						direction.Normalize();
						direction *= speed;
						Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
						Projectile.netUpdate = true;
					}
				}
				Projectile.ai[0] = 0;
				Projectile.netUpdate = true;
			}

			#region Animation and visuals

			// Set both direction and spriteDirection to 1 or -1 (right and left respectively)
			// projectile.direction is automatically set correctly in Projectile.Update, but we need to set it here or the textures will draw incorrectly on the 1st frame.
			Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
			// Adding Pi to rotation if facing left corrects the drawing
			Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
			if (Projectile.spriteDirection == 1) // facing right
			{
				DrawOffsetX = -12; // These values match the values in SetDefaults
				DrawOriginOffsetY = 0;
				DrawOriginOffsetX = 6;
			}
			else
			{
				// Facing left.
				// You can figure these values out if you flip the sprite in your drawing program.
				DrawOffsetX = 0; // 0 since now the top left corner of the hitbox is on the far left pixel.
				DrawOriginOffsetY = 0; // doesn't change
				DrawOriginOffsetX = -6; // Math works out that this is negative of the other value.
			}

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 4;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}

			// Some visuals here
			Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() * 0.85f);
			#endregion
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.Kill();
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			Projectile.Kill();
			return false;
		}
		//Copied from vanila (1.4) Projectiles.cs
		public int FindTargetWithLineOfSight(float maxRange = 800f)
		{
			float newMaxRange = maxRange;
			int result = -1;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC nPC = Main.npc[i];
				bool nPCCanBeChased = nPC.CanBeChasedBy(this);
				if (Projectile.localNPCImmunity[i] != 0)
				{
					nPCCanBeChased = false;
				}
				if (nPCCanBeChased)
				{
					float projDist = Projectile.Distance(Main.npc[i].Center);
					if (projDist < newMaxRange && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, nPC.position, nPC.width, nPC.height))
					{
						newMaxRange = projDist;
						result = i;
					}
				}
			}
			return result;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			//This isn't perfect (especially when the projectile is going any way but straight right or left), but I can't figure out how to fix it.
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(Projectile.direction >= 0 ? -12f : 0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length)* 0.5f;
				Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
				SpriteEffects spriteEffects = Projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Main.EntitySpriteDraw(texture, drawPos, frame, color, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0);
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			if (!Main.dedServ)
            {
				SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/CresceptreSphereExplode") { PitchVariance = 0.05f, MaxInstances = 10 }, Projectile.position);
			}
			for (int i = 0; i < 30; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.PurpleTorch);
				Main.dust[dustIndex].velocity *= 1.4f;
			}
		}

		public override Color? GetAlpha(Color lightColor) => Color.White; //Fullbright
	}
}