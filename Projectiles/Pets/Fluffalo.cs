using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Pets
{
	public class Fluffalo : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fluffalo");
			Main.projFrames[Projectile.type] = 11;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BlackCat);
			Projectile.width = 32;
			Projectile.height = 32;
			AIType = ProjectileID.BlackCat;
			DrawOffsetX = -10;
			DrawOriginOffsetY -= 24;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.blackCat = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			if (player.dead)
			{
				modPlayer.fluffaloPet = false;
			}
			if (modPlayer.fluffaloPet)
			{
				Projectile.timeLeft = 2;
			}
		}
	}
}