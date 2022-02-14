using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class HammerOfRetributionProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 32;
			projectile.height = 32;
			projectile.alpha = 1;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.penetrate = -1;
			aiType = ProjectileID.Bullet;
			projectile.timeLeft = 300;
			//projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Red * (projectile.timeLeft / 100f);
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			projectile.Kill();
        }
        public override void AI()
		{
			projectile.rotation += 0.4f * projectile.direction;
			projectile.alpha += 25;
		}
		public override bool PreKill(int timeLeft)
		{
			if (projectile.owner == Main.myPlayer)
			{
				projectile.position = projectile.Center;
				projectile.width = 128;
				projectile.height = 128;
				projectile.Center = projectile.position;
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
            {
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
				Gore.NewGore(new Vector2(projectile.position.X + (projectile.width / 2), projectile.position.Y + (projectile.height / 2)), default, Main.rand.Next(61, 64), 1f);//Smoke gore
			}
			Main.PlaySound(SoundID.Item14, projectile.position);
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
		}
	}
}