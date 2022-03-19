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
			DisplayName.SetDefault("Fluffalo");
			Main.projFrames[projectile.type] = 11;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.BlackCat);
			projectile.width = 32;
			projectile.height = 32;
			aiType = ProjectileID.BlackCat;
			drawOffsetX = -10;
			drawOriginOffsetY -= 12;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.blackCat = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			if (player.dead)
			{
				modPlayer.fluffaloPet = false;
			}
			if (modPlayer.fluffaloPet)
			{
				projectile.timeLeft = 2;
			}
		}
	}
}