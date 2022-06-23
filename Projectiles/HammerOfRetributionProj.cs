using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class HammerOfRetributionProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.alpha = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			AIType = ProjectileID.Bullet;
			Projectile.timeLeft = 300;
			//projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Red * (Projectile.timeLeft / 100f);
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.Kill();
        }
        public override void AI()
		{
			Projectile.rotation += 0.4f * Projectile.direction;
			Projectile.alpha += 25;
		}
		public override bool PreKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.position = Projectile.Center;
				Projectile.width = 128;
				Projectile.height = 128;
				Projectile.Center = Projectile.position;
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
            {
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
				Gore.NewGore(Entity.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), default, Main.rand.Next(61, 64), 1f);//Smoke gore
			}
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		}
	}
}