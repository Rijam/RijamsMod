using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Enemies
{
	public class SirSlushSnowball : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sir Slush Snowball");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.coldDamage = true;
			AIType = -1;
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			target.AddBuff(BuffID.Frozen, 60);
			if (Main.netMode != NetmodeID.SinglePlayer)
            {
				NetMessage.SendData(MessageID.NPCBuffs, number: BuffID.Frozen, number2: 60);
			}
			if (Main.expertMode)
            {
				target.AddBuff(BuffID.Chilled, 600);
				if (Main.netMode != NetmodeID.SinglePlayer)
                {
					NetMessage.SendData(MessageID.NPCBuffs, number: BuffID.Chilled, number2: 600);
				}
			}
		}

        public override void AI()
        {
			Projectile.rotation += 0.4f * (float)Projectile.direction;
			Projectile.ai[0]++;
			if (Projectile.ai[0] >= 20f)
			{
				Projectile.ai[0] = 120f;
				Projectile.velocity.Y += 0.1f;
				Projectile.velocity.X *= 0.99f;
			}
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
			if (Projectile.ai[0] % 2 == 0)
            {
				Dust.NewDust(Projectile.position, 2, 2, DustID.Snow, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 200, default, 0.5f);
			}
		}

        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
            {
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, DustID.SnowBlock);
			}
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/SirSlushSnowballSplat") { MaxInstances = 5 }, Projectile.position);
		}
	}
}