using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Minions
{
	public class LostSkull : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.MinionShot[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 24;
			Projectile.height = 24;
			//projectile.alpha = 255;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			//Projectile.minion = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = 3;
			AIType = -1;
			Projectile.timeLeft = 600;

			// Using local NPC immunity allows each to strike independently from one another.
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			Projectile.ai[0]++;
			if (Projectile.ai[0] == 30) //Wait 0.5 seconds
            {
				int newTarget = Projectile.FindTargetWithLineOfSight();
				if (newTarget != -1) //fly to the target
				{
					NPC nPC2 = Main.npc[newTarget];
					Projectile.Distance(nPC2.Center);
					Projectile.velocity = Projectile.DirectionTo(nPC2.Center).SafeNormalize(-Vector2.UnitY) * Projectile.velocity.Length();
					Projectile.netUpdate = true;
					if (!Main.dedServ)
					{
						SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/MeatballDemonShoot") { Volume = 0.25f, MaxInstances = 5 }, Projectile.position);
					}
				}
				Projectile.ai[0] = 0;
				Projectile.netUpdate = true;
			}

			#region Animation and visuals
			// So it will lean slightly towards the direction it's moving
			//projectile.rotation = projectile.velocity.X * 0.05f;

			// Set both direction and spriteDirection to 1 or -1 (right and left respectively)
			// projectile.direction is automatically set correctly in Projectile.Update, but we need to set it here or the textures will draw incorrectly on the 1st frame.
			Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
			// Adding Pi to rotation if facing left corrects the drawing
			Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
			if (Projectile.spriteDirection == 1) // facing right
			{
				DrawOffsetX = -14; // These values match the values in SetDefaults
				DrawOriginOffsetY = 0;
				DrawOriginOffsetX = 7;
			}
			else
			{
				// Facing left.
				// You can figure these values out if you flip the sprite in your drawing program.
				DrawOffsetX = 0; // 0 since now the top left corner of the hitbox is on the far left pixel.
				DrawOriginOffsetY = 0; // doesn't change
				DrawOriginOffsetX = -7; // Math works out that this is negative of the other value.
			}

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 3;
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
			Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.85f);
			#endregion
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else //bounce off of tiles
			{
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = oldVelocity.X * -1f;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = oldVelocity.Y * -1f;
				}
			}
			return false;
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, DustID.Torch);
			}
			SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.5f }, Projectile.position);
		}

		public override Color? GetAlpha(Color lightColor) => Color.White; //Fullbright
	}
}