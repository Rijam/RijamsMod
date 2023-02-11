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
			// DisplayName.SetDefault("Lumoth");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.width = 18;
			Projectile.height = 18;
			AIType = ProjectileID.ZephyrFish;
			DrawOffsetX = -9;
			DrawOriginOffsetY -= 16;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.zephyrfish = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			if (player.dead)
			{
				modPlayer.lumothPet = false;
			}
			if (modPlayer.lumothPet)
			{
				Projectile.timeLeft = 2;
			}

			//Teleport if too far away
			Vector2 PlayPosProjPos = player.position - Projectile.position;
			float distance = PlayPosProjPos.Length();
			if (Main.myPlayer == player.whoAmI && distance > 1000f)
			{
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
				// and then set netUpdate to true
				Projectile.position = player.position;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}
			Lighting.AddLight(Projectile.Center, 1f, 1f, 0.5f);
		}
	}
	public class LEDLumoth : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("LED Lumoth");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.width = 18;
			Projectile.height = 18;
			AIType = -1;
			Projectile.aiStyle = -1;
			DrawOffsetX = -9;
			DrawOriginOffsetY -= 16;
			Projectile.extraUpdates = 1;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.zephyrfish = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			if (player.dead)
			{
				modPlayer.lEDLumothPet = false;
			}
			if (modPlayer.lEDLumothPet)
			{
				Projectile.timeLeft = 2;
			}

			float smallFloat = 0.3f;
			Projectile.tileCollide = false;
			Vector2 vectorPos = new(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float posNewX = Main.player[Projectile.owner].position.X + (Main.player[Projectile.owner].width / 2) - vectorPos.X;
			float posNewY = Main.player[Projectile.owner].position.Y + (Main.player[Projectile.owner].height / 2) - vectorPos.Y;
			posNewY += Main.rand.Next(-5, 15);
			posNewX += Main.rand.Next(-5, 15);
			posNewX += (30 * -Main.player[Projectile.owner].direction);
			posNewY -= 30f;
			float posNewXY = (float)Math.Sqrt(posNewX * posNewX + posNewY * posNewY);
			if (posNewXY < 100f && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.position.Y + Projectile.height <= Main.player[Projectile.owner].position.Y + Main.player[Projectile.owner].height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
			{
				Projectile.ai[0] = 0f;
				if (Projectile.velocity.Y < -16f)
				{
					Projectile.velocity.Y = -16f;
				}
			}
			if (posNewXY < 50f)
			{
				if (Math.Abs(Projectile.velocity.X) > 1f || Math.Abs(Projectile.velocity.Y) > 1f)
				{
					Projectile.velocity *= 0.9f;
				}
				smallFloat = 0.01f;
			}
			else
			{
				if (posNewXY < 100f)
				{
					smallFloat = 0.2f;
				}
				if (posNewXY > 300f)
				{
					smallFloat = 0.4f;
				}
				posNewXY = 12f / posNewXY;
				posNewX *= posNewXY;
				posNewY *= posNewXY;
			}
			if (Projectile.velocity.X < posNewX)
			{
				Projectile.velocity.X += smallFloat;
			}
			if (Projectile.velocity.X > posNewX)
			{
				Projectile.velocity.X -= smallFloat;
			}
			if (Projectile.velocity.Y < posNewY)
			{
				Projectile.velocity.Y += smallFloat;
			}
			if (Projectile.velocity.Y > posNewY)
			{
				Projectile.velocity.Y -= smallFloat;
			}
			if (Projectile.velocity.X > 0.25)
			{
				Projectile.direction = -1;
			}
			else if (Projectile.velocity.X < -0.25)
			{
				Projectile.direction = 1;
			}
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.X * 0.05f;
			Projectile.frameCounter++;
			int frameAmount = 10;
			if (Projectile.frameCounter > frameAmount)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}

			//Teleport if too far away
			Vector2 PlayPosProjPos = player.position - Projectile.position;
			float distance = PlayPosProjPos.Length();
			if (Main.myPlayer == player.whoAmI && distance > 750f)
			{
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
				// and then set netUpdate to true
				Projectile.position = player.position;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}
			Lighting.AddLight(Projectile.Center, 2f, 2f, 2f);
		}
	}
}