using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee
{
	public class TimonsAxeProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.alpha = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			AIType = ProjectileID.Bullet;
			Projectile.timeLeft = 10;
			Projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Blue * Projectile.timeLeft;
		}

		public override void AI()
		{
			Projectile.rotation += 0.4f * Projectile.direction;
			Projectile.alpha += 25;
		}
		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		}
	}
}