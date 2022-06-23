using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class PlasmaBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 14;
			Projectile.height = 14;
			//projectile.alpha = 255;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			AIType = ProjectileID.Bullet;
			Projectile.timeLeft = 1200;
		}

		/*public override Color? GetAlpha(Color lightColor)
		{
			return Color.Blue;
			//return new Color(0, 0, 120, 0) * (1f - (float)projectile.alpha / 255f);
		}*/

		public override void AI()
		{
			int frameSpeed = 2;
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
			Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.78f);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, DustID.WaterCandle);// DustID.DungeonWater_Old
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item118, Projectile.position);
		}
	}
}