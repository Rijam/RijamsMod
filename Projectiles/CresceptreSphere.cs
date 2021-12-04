using Microsoft.Xna.Framework;
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
			}

			#region Animation and visuals

			// Set both direction and spriteDirection to 1 or -1 (right and left respectively)
			// projectile.direction is automatically set correctly in Projectile.Update, but we need to set it here or the textures will draw incorrectly on the 1st frame.
			projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
			// Adding Pi to rotation if facing left corrects the drawing
			projectile.rotation = projectile.velocity.ToRotation() + (projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
			if (projectile.spriteDirection == 1) // facing right
			{
				drawOffsetX = -14; // These values match the values in SetDefaults
				drawOriginOffsetY = 0;
				drawOriginOffsetX = 7;
			}
			else
			{
				// Facing left.
				// You can figure these values out if you flip the sprite in your drawing program.
				drawOffsetX = 0; // 0 since now the top left corner of the hitbox is on the far left pixel.
				drawOriginOffsetY = 0; // doesn't change
				drawOriginOffsetX = -7; // Math works out that this is negative of the other value.
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