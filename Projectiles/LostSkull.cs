using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class LostSkull : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 24;
			projectile.height = 24;
			//projectile.alpha = 255;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.minion = true;
			projectile.penetrate = 3;
			aiType = -1;
			projectile.timeLeft = 600;
		}

		public override void AI()
		{
			projectile.ai[0]++;
			if (projectile.ai[0] == 30) //Wait 0.5 seconds
            {
				int newTarget = FindTargetWithLineOfSight();
				if (newTarget != -1) //fly to the target
				{
					NPC nPC2 = Main.npc[newTarget];
					projectile.Distance(nPC2.Center);
					projectile.velocity = projectile.DirectionTo(nPC2.Center).SafeNormalize(-Vector2.UnitY) * projectile.velocity.Length();
					projectile.netUpdate = true;
					if (!Main.dedServ)
					{
						Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/MeatballDemonShoot").WithVolume(0.5f), (int)projectile.position.X, (int)projectile.position.Y);
					}
				}
				projectile.ai[0] = 0;
				projectile.netUpdate = true;
			}

			#region Animation and visuals
			// So it will lean slightly towards the direction it's moving
			//projectile.rotation = projectile.velocity.X * 0.05f;

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
			int frameSpeed = 3;
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
			Lighting.AddLight(projectile.Center, Color.Orange.ToVector3() * 0.85f);
			#endregion
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			else //bounce off of tiles
			{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = oldVelocity.X * -1f;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = oldVelocity.Y * -1f;
				}
			}
			return false;
		}
		//Copied from vanilla (1.4) Projectiles.cs
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
			for (int i = 0; i < 30; i++)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, DustID.Fire);
			}
			Main.PlaySound(SoundID.Item14, projectile.position);
		}

		public override Color? GetAlpha(Color lightColor) => Color.White; //Fullbright
	}
}