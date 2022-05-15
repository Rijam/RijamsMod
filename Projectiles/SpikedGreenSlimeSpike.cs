using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class SpikedGreenSlimeSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Slime Spike");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.SpikedSlimeSpike);
			projectile.aiStyle = -1;
			projectile.timeLeft = 600;
		}
        public override void AI()
        {
			projectile.ai[0]++;
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (projectile.alpha == 0 && Main.rand.Next(3) == 0)
			{
				int num70 = Dust.NewDust(projectile.position - projectile.velocity * 3f, projectile.width, projectile.height, DustID.t_Slime, 0f, 0f, 50, new Color(69, 129, 67, 150), 1.2f);
				Main.dust[num70].velocity *= 0.3f;
				Main.dust[num70].velocity += projectile.velocity * 0.3f;
				Main.dust[num70].noGravity = true;
			}
			projectile.alpha -= 50;
			if (projectile.alpha < 0)
			{
				projectile.alpha = 0;
			}
			if (projectile.ai[1] == 0f)
			{
				projectile.ai[1] = 1f;
				Main.PlaySound(SoundID.Item17, projectile.position);
			}
			if (projectile.ai[0] >= 5f)
			{
				projectile.ai[0] = 5f;
				projectile.velocity.Y += 0.15f;
			}
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			return true;
		}
	}
}