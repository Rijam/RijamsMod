using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Pets
{
	public class Lumoth : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lumoth");
			Main.projFrames[projectile.type] = 4;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.LightPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ZephyrFish);
			projectile.width = 18;
			projectile.height = 18;
			aiType = ProjectileID.ZephyrFish;
			drawOffsetX = -9;
			drawOriginOffsetY -= 8;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.zephyrfish = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			if (player.dead)
			{
				modPlayer.lumothPet = false;
			}
			if (modPlayer.lumothPet)
			{
				projectile.timeLeft = 2;
			}

			//Teleport if too far away
			Vector2 PlayPosProjPos = player.position - projectile.position;
			float distance = PlayPosProjPos.Length();
			if (Main.myPlayer == player.whoAmI && distance > 1000f)
			{
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
				// and then set netUpdate to true
				projectile.position = player.position;
				projectile.velocity *= 0.1f;
				projectile.netUpdate = true;
			}
			Lighting.AddLight(projectile.Center, 1f, 1f, 0.5f);
		}
	}
}