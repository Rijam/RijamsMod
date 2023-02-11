using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Enemies
{
	public class FirmamentHarpyFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Firmament Harpy Feather");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.DamageType = DamageClass.Generic;
			AIType = ProjectileID.Bullet;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
		}
		public override void AI()
		{
			if (!Main.getGoodWorld)
			{
				return;
			}
			// In For the Worthy worlds:
			Projectile.ai[1]++;
			float speed = 10f;
			float inertia = 14f;
			if (Projectile.ai[1] <= 60) // Home for 2 seconds, then stop homing
			{
				int newTarget = FindTargetWithLineOfSight();
				if (newTarget != -1) //fly to the target
				{
					Player target = Main.player[newTarget];
					float distanceFromTarget = Projectile.Distance(target.Center);
					if (distanceFromTarget < 1000f)
					{
						Vector2 direction = target.Center - Projectile.Center;
						direction.Normalize();
						direction *= speed;
						Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
						Projectile.netUpdate = true;
					}
				}
			}
		}
		//Copied from vanilla (1.4) Projectiles.cs
		public int FindTargetWithLineOfSight(float maxRange = 800f)
		{
			float newMaxRange = maxRange;
			int result = -1;
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player target = Main.player[i];
				float projDist = Projectile.Distance(Main.player[i].Center);
				if (projDist < newMaxRange && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height) && target.active && !target.dead)
				{
					newMaxRange = projDist;
					result = i;
				}
			}
			return result;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDust, 0, 0, 0, Color.Red, 0.5f);
			}
		}
	}
}