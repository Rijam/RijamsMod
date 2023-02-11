using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Enemies
{
	public class SpikedGreenSlimeSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Green Slime Spike");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.SpikedSlimeSpike);
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
		}
        public override void AI()
        {
			Projectile.ai[0]++;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.alpha == 0 && Main.rand.NextBool(3))
			{
				int num70 = Dust.NewDust(Projectile.position - Projectile.velocity * 3f, Projectile.width, Projectile.height, DustID.t_Slime, 0f, 0f, 50, new Color(69, 129, 67, 150), 1.2f);
				Main.dust[num70].velocity *= 0.3f;
				Main.dust[num70].velocity += Projectile.velocity * 0.3f;
				Main.dust[num70].noGravity = true;
			}
			Projectile.alpha -= 50;
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			if (Projectile.ai[1] == 0f)
			{
				Projectile.ai[1] = 1f;
				SoundEngine.PlaySound(SoundID.Item17, Projectile.position);
			}
			if (Projectile.ai[0] >= 5f)
			{
				Projectile.ai[0] = 5f;
				Projectile.velocity.Y += 0.15f;
			}
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			return true;
		}
	}
}