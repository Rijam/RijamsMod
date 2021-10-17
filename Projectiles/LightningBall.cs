using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class LightningBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 14;
			projectile.height = 14;
			//projectile.alpha = 255;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.minion = true;
			projectile.penetrate = 1;
			aiType = ProjectileID.Bullet;
			projectile.timeLeft = 600;
		}

		/*public override Color? GetAlpha(Color lightColor)
		{
			return Color.Blue;
			//return new Color(0, 0, 120, 0) * (1f - (float)projectile.alpha / 255f);
		}*/

		public override void AI()
		{
			int frameSpeed = 2;
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
			Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.85f);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, DustID.Fire);
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundID.Item10, projectile.position);
		}
	}
}