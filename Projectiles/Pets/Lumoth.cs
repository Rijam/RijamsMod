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
	public class LEDLumoth : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("LED Lumoth");
			Main.projFrames[projectile.type] = 4;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.LightPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ZephyrFish);
			projectile.width = 18;
			projectile.height = 18;
			aiType = -1;
			projectile.aiStyle = -1;
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
				modPlayer.lEDLumothPet = false;
			}
			if (modPlayer.lEDLumothPet)
			{
				projectile.timeLeft = 2;
			}

			float smallFloat = 0.3f;
			projectile.tileCollide = false;
			Vector2 vectorPos = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
			float posNewX = Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2) - vectorPos.X;
			float posNewY = Main.player[projectile.owner].position.Y + (Main.player[projectile.owner].height / 2) - vectorPos.Y;
			posNewY += Main.rand.Next(-5, 15);
			posNewX += Main.rand.Next(-5, 15);
			posNewX += (30 * -Main.player[projectile.owner].direction);
			posNewY -= 30f;
			float posNewXY = (float)Math.Sqrt(posNewX * posNewX + posNewY * posNewY);
			if (posNewXY < 100f && Main.player[projectile.owner].velocity.Y == 0f && projectile.position.Y + projectile.height <= Main.player[projectile.owner].position.Y + Main.player[projectile.owner].height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
			{
				projectile.ai[0] = 0f;
				if (projectile.velocity.Y < -12f)
				{
					projectile.velocity.Y = -12f;
				}
			}
			if (posNewXY < 50f)
			{
				if (Math.Abs(projectile.velocity.X) > 1f || Math.Abs(projectile.velocity.Y) > 1f)
				{
					projectile.velocity *= 0.9f;
				}
				smallFloat = 0.01f;
			}
			else
			{
				if (posNewXY < 100f)
				{
					smallFloat = 0.1f;
				}
				if (posNewXY > 300f)
				{
					smallFloat = 0.4f;
				}
				posNewXY = 12f / posNewXY;
				posNewX *= posNewXY;
				posNewY *= posNewXY;
			}
			if (projectile.velocity.X < posNewX)
			{
				projectile.velocity.X += smallFloat;
			}
			if (projectile.velocity.X > posNewX)
			{
				projectile.velocity.X -= smallFloat;
			}
			if (projectile.velocity.Y < posNewY)
			{
				projectile.velocity.Y += smallFloat;
			}
			if (projectile.velocity.Y > posNewY)
			{
				projectile.velocity.Y -= smallFloat;
			}
			if (projectile.velocity.X > 0.25)
			{
				projectile.direction = -1;
			}
			else if (projectile.velocity.X < -0.25)
			{
				projectile.direction = 1;
			}
			projectile.spriteDirection = projectile.direction;
			projectile.rotation = projectile.velocity.X * 0.05f;
			projectile.frameCounter++;
			int frameAmount = 5;
			if (projectile.frameCounter > frameAmount)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame > 3)
			{
				projectile.frame = 0;
			}

			//Teleport if too far away
			Vector2 PlayPosProjPos = player.position - projectile.position;
			float distance = PlayPosProjPos.Length();
			if (Main.myPlayer == player.whoAmI && distance > 750f)
			{
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
				// and then set netUpdate to true
				projectile.position = player.position;
				projectile.velocity *= 0.1f;
				projectile.netUpdate = true;
			}
			Lighting.AddLight(projectile.Center, 2f, 2f, 2f);
		}
	}
}