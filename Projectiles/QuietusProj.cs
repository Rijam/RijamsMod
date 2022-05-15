using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class QuietusProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 14;
			projectile.height = 14;
			projectile.alpha = 1;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.penetrate = 1;
			aiType = ProjectileID.Bullet;
			projectile.timeLeft = 180;
			//projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lime * (projectile.timeLeft / 100f);
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			projectile.Kill();
        }
        public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			projectile.alpha += 25;
			int selectRand = Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.CursedTorch, DustID.GreenFairy);
			Dust killDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, selectRand)];
			killDust.noGravity = true;
			killDust.fadeIn = 0.25f;
			Dust killDust3 = killDust;
			killDust3.velocity *= 2f;
			killDust.noLight = true;
			killDust3.alpha /= 2;
		}
		public override bool PreKill(int timeLeft)
		{
			if (projectile.owner == Main.myPlayer)
			{
				projectile.position = projectile.Center;
				projectile.width = 32;
				projectile.height = 32;
				projectile.Center = projectile.position;
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
            {
				int selectRand = Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.CursedTorch, DustID.GreenFairy);
				Dust killDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, selectRand)];
				killDust.noGravity = true;
				killDust.scale = 1.25f + Main.rand.NextFloat();
				killDust.fadeIn = 0.25f;
				Dust killDust3 = killDust;
				killDust3.velocity *= 2f;
				killDust.noLight = true;
			}
			Main.PlaySound(SoundID.Item66, projectile.position);
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
		}
	}
}