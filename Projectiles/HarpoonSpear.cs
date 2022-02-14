using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class HarpoonSpear : ModProjectile
	{
		public override string Texture => "Terraria/Projectile_" + ProjectileID.Harpoon;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpoon Spear");
		}

		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			aiType = ProjectileID.WoodenArrowFriendly;
			if (!Main.hardMode)
			{
				projectile.penetrate = 1;
			}
			if (Main.hardMode)
			{
				projectile.penetrate = 2;
			}
		}

		// Additional hooks/methods here.
	}
}