using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Misc
{
	public class SkywareArmorHarpyFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Harpy Feather");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Generic;
			AIType = ProjectileID.Bullet;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 120;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDust, 0, 0, 0, Color.LightBlue, 0.5f);
			}
		}
	}
	public class RedSkywareArmorHarpyFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Harpy Feather");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.extraUpdates = 1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			Projectile.ai[0]++;
			if (Projectile.ai[0] > 30) //Update every 2 ticks and homes for half a second
			{
				int newTarget = Projectile.FindTargetWithLineOfSight(16 * 20); // 20 tiles
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
				Projectile.netUpdate = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDust, 0, 0, 0, Color.PaleVioletRed, 0.5f);
			}
		}
	}
}