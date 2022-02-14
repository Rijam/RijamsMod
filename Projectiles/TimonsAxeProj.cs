using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class TimonsAxeProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 6;
			projectile.height = 6;
			projectile.alpha = 1;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.penetrate = -1;
			aiType = ProjectileID.Bullet;
			projectile.timeLeft = 10;
			projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Blue * projectile.timeLeft;
		}

		public override void AI()
		{
			projectile.rotation += 0.4f * projectile.direction;
			projectile.alpha += 25;
		}
		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
		}
	}
}