using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace RijamsMod.Projectiles.Pets
{
	public class DwarfStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dwarf Star");
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			AIType = -1;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			if (player.dead)
			{
				modPlayer.dwarfStarPet = false;
			}
			if (modPlayer.dwarfStarPet)
			{
				Projectile.timeLeft = 2;
			}

			Color dayTime = Color.White;
			Color duskTime = Color.Orange;
			Color nightTime = Color.SeaGreen;
			Color lerping = Color.White;

			if (TimeMode() == 0)
			{
				lerping = Color.Lerp(dayTime, duskTime, (float)(Main.time / 54000.0));
			}
			else if (TimeMode() == 1)
			{
				lerping = Color.Lerp(duskTime, nightTime, (float)(Main.time / 9000.0));
			}
			else if (TimeMode() == 2)
			{
				lerping = Color.Lerp(nightTime, dayTime, (float)((Main.time - 9000) / 23400.0));
			}
			Lighting.AddLight(Projectile.Center, lerping.ToVector3());

			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (Projectile.Hitbox.Intersects(npc.Hitbox) && (!npc.friendly || player.killGuide || player.killClothier) && npc.active && !npc.dontCountMe && !(npc.CountsAsACritter && player.dontHurtCritters))
				{
					Main.npc[i].AddBuff(BuffID.OnFire, Main.rand.Next(60, 301));
				}
			}
			// adapted from the Wisp light pet
			Projectile.ai[2]++;

			float num49 = 0.2f;
			float num50 = 5f;
			Projectile.tileCollide = false;
			Vector2 vector5 = new(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
			float posX = player.position.X + (float)(player.width / 2) - vector5.X;
			float posY = player.position.Y + player.gfxOffY + (float)(player.height / 2) - vector5.Y;
			if (player.controlLeft)
			{
				posX -= 120f;

			}
			else if (player.controlRight)
			{
				posX += 120f;

			}

			if (player.controlDown)
			{
				posY += 120f;
			}
			else
			{
				if (player.controlUp)
				{
					posY -= 120f;
				}

				posY -= 60f;
			}

			float distance = (float)Math.Sqrt(posX * posX + posY * posY);

			if (distance > 1000f)
			{
				Projectile.position.X += posX;
				Projectile.position.Y += posY;
			}

			if (Projectile.localAI[0] == 1f)
			{
				if (distance < 10f)
				{
					Player player2 = player;
					if (Math.Abs(player2.velocity.X) + Math.Abs(player2.velocity.Y) < num50 && (player2.velocity.Y == 0f || (player2.mount.Active && player2.mount.CanFly())))
						Projectile.localAI[0] = 0f;
				}

				num50 = 12f;
				if (distance < num50)
				{
					Projectile.velocity.X = posX;
					Projectile.velocity.Y = posY;
				}
				else
				{
					distance = num50 / distance;
					Projectile.velocity.X = posX * distance;
					Projectile.velocity.Y = posY * distance;
				}

				if ((double)Projectile.velocity.X > 0.5)
					Projectile.direction = -1;
				else if ((double)Projectile.velocity.X < -0.5)
					Projectile.direction = 1;

				Projectile.spriteDirection = Projectile.direction;
				Projectile.rotation -= (0.2f + Math.Abs(Projectile.velocity.X) * 0.025f) * (float)Projectile.direction;
				Projectile.frameCounter++;

				for (int l = 0; l < 2; l++)
				{
					int num54 = Dust.NewDust(new Vector2(Projectile.position.X + 3f, Projectile.position.Y + 4f), 14, 14, DustID.Torch);
					Main.dust[num54].velocity *= 0.2f;
					Main.dust[num54].noGravity = true;
					Main.dust[num54].scale = 0.5f;
					Main.dust[num54].noLight = true;
					Main.dust[num54].shader = GameShaders.Armor.GetSecondaryShader(player.cLight, player);
				}
				return;
			}

			if (distance > 200f)
				Projectile.localAI[0] = 1f;

			if ((double)Projectile.velocity.X > 0.5)
				Projectile.direction = -1;
			else if ((double)Projectile.velocity.X < -0.5)
				Projectile.direction = 1;

			Projectile.spriteDirection = Projectile.direction;
			if (distance < 10f)
			{
				Projectile.velocity.X = posX;
				Projectile.velocity.Y = posY;
				//Projectile.rotation = Projectile.velocity.X * 0.05f;
				if (distance < num50)
				{
					Projectile.position += Projectile.velocity;
					Projectile.velocity *= 0f;
					num49 = 0f;
				}

				Projectile.direction = -player.direction;
			}

			distance = num50 / distance;
			posX *= distance;
			posY *= distance;
			if (Projectile.velocity.X < posX)
			{
				Projectile.velocity.X += num49;
				if (Projectile.velocity.X < 0f)
					Projectile.velocity.X *= 0.99f;
			}

			if (Projectile.velocity.X > posX)
			{
				Projectile.velocity.X -= num49;
				if (Projectile.velocity.X > 0f)
					Projectile.velocity.X *= 0.99f;
			}

			if (Projectile.velocity.Y < posY)
			{
				Projectile.velocity.Y += num49;
				if (Projectile.velocity.Y < 0f)
					Projectile.velocity.Y *= 0.99f;
			}

			if (Projectile.velocity.Y > posY)
			{
				Projectile.velocity.Y -= num49;
				if (Projectile.velocity.Y > 0f)
					Projectile.velocity.Y *= 0.99f;
			}

			if (Projectile.velocity.X != 0f || Projectile.velocity.Y != 0f)
			{
				Projectile.rotation += Projectile.velocity.X * 0.05f;
			}
			Projectile.rotation -= 0.01f * Projectile.direction;
		}

		private static int TimeMode()
		{
			if (Main.time >= 0 && Main.time <= 54000 && Main.dayTime) // 4:30 AM to 7:30 PM (Day time)
			{
				return 0;
			}
			if (Main.time >= 0 && Main.time < 9000 && !Main.dayTime) // 7:30 PM to 10:00 PM (Dusk)
			{
				return 1;
			}
			if (Main.time >= 9000 && Main.time < 32400 && !Main.dayTime) // 10:00 PM to 4:30 AM (Night time)
			{
				return 2;
			}
			return -1;
		}


		private readonly Asset<Texture2D> top = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Pets/DwarfStar");
		private readonly Asset<Texture2D> middle = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Pets/DwarfStar2");
		private readonly Asset<Texture2D> bottom = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Pets/DwarfStar3");
		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = top.Frame(1, Main.projFrames[Type], frameY: 0);

			// The origin in this case is (0, 0) of our projectile because Projectile.Center is the tip of our Jousting Lance.
			Vector2 origin = sourceRectangle.Size() / 2f;

			// The rotation of the projectile.
			float rotation = Projectile.rotation;

			// The position of the sprite. Not subtracting Main.player[Projectile.owner].gfxOffY will cause the sprite to bounce when walking up blocks.
			Vector2 position = new(Projectile.Center.X, Projectile.Center.Y - Main.player[Projectile.owner].gfxOffY);

			Color dayTime = new(255, 255, 255, 0); // White
			Color duskTime = new(255, 165, 0, 0); // Orange
			Color nightTime = new(46, 139, 87, 0); // Seagreen
			Color lerping = new(255, 255, 255, 0); // White

			if (TimeMode() == 0)
			{
				lerping = Color.Lerp(dayTime, duskTime, (float)(Main.time / 54000.0));
			}
			else if (TimeMode() == 1)
			{
				lerping = Color.Lerp(duskTime, nightTime, (float)(Main.time / 9000.0));
			}
			else if (TimeMode() == 2)
			{
				lerping = Color.Lerp(nightTime, dayTime, (float)((Main.time - 9000) / 23400.0));
			}

			Color bottomColor = lerping;// * 0.5f;
			Color middleColor = lerping;// * 0.75f;
			Color topColor = lerping;

			Main.EntitySpriteDraw(bottom.Value,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, bottomColor, rotation, origin, (float)(0.125f * Math.Sin(Projectile.scale + 0.1f * Projectile.ai[2]) + 1), spriteEffects, 0);

			Main.EntitySpriteDraw(middle.Value,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, middleColor, rotation, origin, (float)(0.125f * Math.Sin(Projectile.scale + 0.1f * Projectile.ai[2]) + 1), spriteEffects, 0);

			Main.EntitySpriteDraw(top.Value,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, topColor, rotation, origin, (float)(0.125f * Math.Sin(Projectile.scale + 0.1f * Projectile.ai[2]) + 1), spriteEffects, 0);

			return false;
		}
	}
}