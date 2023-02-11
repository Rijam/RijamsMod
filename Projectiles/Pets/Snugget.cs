using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Pets
{
	public class Snugget : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Snugget");
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
				modPlayer.snuggetPet = false;
			}
			if (modPlayer.snuggetPet)
			{
				Projectile.timeLeft = 2;
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}