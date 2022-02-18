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
			DisplayName.SetDefault("Snugget");
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
				modPlayer.snuggetPet = false;
			}
			if (modPlayer.snuggetPet)
			{
				projectile.timeLeft = 2;
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}