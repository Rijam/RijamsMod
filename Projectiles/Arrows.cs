using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class BloodyArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Arrow");     //The English name of the projectile
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.UnholyArrow);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

        public override void PostAI()
        {
			if (Main.rand.Next(5) == 0)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CrimtaneWeapons, 0f, 0f, 150, default, 1.1f);
			}
		}

        public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, projectile.position);
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CrimtaneWeapons, 0f, 0f, 150, default, 1.1f);
			}
		}
	}
}