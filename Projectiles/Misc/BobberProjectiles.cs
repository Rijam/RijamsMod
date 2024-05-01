using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Misc
{
	public class CuriosityLureProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = ProjAIStyleID.Bobber;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.bobber = true;
			AIType = ProjectileID.FishingBobber;
			DrawOriginOffsetY = -2;
		}
	}

	public class TrapBobberProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = ProjAIStyleID.Bobber;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.bobber = true;
			AIType = ProjectileID.FishingBobber;
			DrawOriginOffsetY = -8;
		}
	}

	public class SpinnerBobberProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = ProjAIStyleID.Bobber;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.bobber = true;
			AIType = ProjectileID.FishingBobber;
			DrawOriginOffsetY = -8;
		}
		public override void AI()
		{
			if (Projectile.wet) // Only animate while in the water.
			{
				if (Projectile.frameCounter++ >= 4)
				{
					Projectile.frameCounter = 0;
					Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
				}
			}
			else
			{
				Projectile.frame = 0;
			}
		}
	}
}