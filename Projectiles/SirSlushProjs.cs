using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class SirSlushSnowball : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sir Slush Snowball");
		}

		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.coldDamage = true;
			aiType = -1;
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			target.AddBuff(BuffID.Frozen, 60);
			if (Main.netMode != NetmodeID.SinglePlayer)
            {
				NetMessage.SendData(MessageID.SendNPCBuffs, number: BuffID.Frozen, number2: 60);
			}
			if (Main.expertMode)
            {
				target.AddBuff(BuffID.Chilled, 600);
				if (Main.netMode != NetmodeID.SinglePlayer)
                {
					NetMessage.SendData(MessageID.SendNPCBuffs, number: BuffID.Chilled, number2: 600);
				}
			}
		}

        public override void AI()
        {
			projectile.rotation += 0.4f * (float)projectile.direction;
			projectile.ai[0]++;
			if (projectile.ai[0] >= 20f)
			{
				projectile.ai[0] = 120f;
				projectile.velocity.Y += 0.1f;
				projectile.velocity.X *= 0.99f;
			}
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
			if (projectile.ai[0] % 2 == 0)
            {
				Dust.NewDust(projectile.position, 2, 2, DustID.Snow, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 200, default, 0.5f);
			}
		}

        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
            {
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, DustID.SnowBlock);
			}
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/SirSlushSnowballSplat"));
		}
	}
}