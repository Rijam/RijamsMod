using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using System.IO;
using Terraria.DataStructures;
using ReLogic.Peripherals.RGB;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.RGB;
using FullSerializer.Internal;
using static Humanizer.In;

namespace RijamsMod.Projectiles.Magic
{
	public class AetherLanternProj : ModProjectile
	{
		private readonly int sheetFrames = 5;
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.netImportant = true; // Sync this projectile if a player joins mid game.
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 0.75f;
			Projectile.timeLeft = 30; // This value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
			Projectile.hide = false; // Important when used alongside player.heldProj. "Hidden" projectiles have special draw conditions
		}
		public override void AI()
		{
			Player owner = Main.player[Projectile.owner]; // Get the owner of the projectile.
			Projectile.direction = owner.direction; // Set the direction of the projectile to the same direction as the owner.
			owner.heldProj = Projectile.whoAmI; // Set the owner's held projectile to this projectile.

			if (owner.channel && owner.itemAnimation < owner.itemAnimationMax)
			{
				//owner.SetDummyItemTime(owner.itemAnimationMax); // This makes it so the projectile never dies while we are holding it (except when we take damage, see ExampleJoustingLancePlayer).
			}

			// If the Jousting Lance is no longer being used, kill the projectile.
			if (owner.ItemAnimationEndingOrEnded)
			{
				Projectile.Kill();
				return;
			}

			

			/*Vector2 pos = owner.GetBackHandPosition(Player.CompositeArmStretchAmount.Full, MathHelper.PiOver4);
			if (owner.direction > 0)
			{
				pos.X += owner.width;
			}
			else
			{
				pos.X -= owner.width / 4f;
			}
			Projectile.Center = pos;*/
		}
		public override bool PreAI()
		{
			Player owner = Main.player[Projectile.owner]; // Get the owner of the projectile.

			//Main.NewText(owner.heldProj);

			Projectile.direction = owner.direction; // Set the direction of the projectile to the same direction as the owner.
			Projectile.spriteDirection = Projectile.direction;
			owner.heldProj = Projectile.whoAmI; // Set the owner's held projectile to this projectile.

			Vector2 pos = owner.GetBackHandPosition(Player.CompositeArmStretchAmount.Full, MathHelper.PiOver4);
			if (owner.ItemAnimationActive)
			{
				pos = owner.GetBackHandPosition(Player.CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 + 0.5f);
			}
			if (owner.direction > 0)
			{
				pos.X += owner.width;
			}
			else
			{
				pos.X -= owner.width / 4f;
			}
			Projectile.Center = pos;

			if (owner.channel && owner.itemAnimation < owner.itemAnimationMax)
			{
				//owner.SetDummyItemTime(owner.itemAnimationMax); // This makes it so the projectile never dies while we are holding it (except when we take damage, see ExampleJoustingLancePlayer).
			}

			// If the Jousting Lance is no longer being used, kill the projectile.
			if (owner.ItemAnimationEndingOrEnded)
			{
				Projectile.Kill();
			}

			/*if (Projectile.ai[2] == 1f)
			{
				owner.GetModPlayer<RijamsModPlayer>().holdingAetherLantern = true;
			}*/

			// frame is the flash frame
			// Projectile.ai[1] is the faeling frame
			Projectile.frame = (int)Projectile.ai[0];
			if (Projectile.frameCounter % 30 == 0)
			{
				Projectile.ai[1]++;
				if (Projectile.ai[1] >= 5)
				{
					Projectile.ai[1] = 0;
				}
			}
			Projectile.frameCounter++;
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player owner = Main.player[Projectile.owner];

			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = ((Projectile.spriteDirection <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			if (owner.gravDir == -1f)
			{
				spriteEffects |= SpriteEffects.FlipVertically;
			}

			Texture2D textureLantern = TextureAssets.Projectile[Type].Value;
			//Texture2D textureFaeling = ModContent.Request<Texture2D>(Mod.Name + "/Projectiles/Magic/" + Name + "_Faeling").Value;
			Texture2D textureGlass = ModContent.Request<Texture2D>(Mod.Name + "/Projectiles/Magic/" + Name + "_Glass").Value;
			
			Texture2D textureFlashBack = ModContent.Request<Texture2D>(Mod.Name + "/Projectiles/Magic/" + Name + "_FlashBack").Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangleSingle = textureLantern.Frame(1, Main.projFrames[Type]);
			
			//Rectangle sourceRectangleFaeling = textureFaeling.Frame(1, sheetFrames, frameY: (int)Projectile.ai[1]);

			// The origin in this case is (0, 0) of our projectile because Projectile.Center is the tip of our Jousting Lance.
			Vector2 origin = sourceRectangleSingle.Size() / 2f;

			// The rotation of the projectile.
			float rotation = Projectile.rotation;

			// The position of the sprite. Not subtracting Main.player[Projectile.owner].gfxOffY will cause the sprite to bounce when walking up blocks.
			Vector2 position = new(Projectile.Center.X, Projectile.Center.Y);

			// Apply lighting and draw our projectile
			Color drawColor = lightColor;

			for (int i = 0; i < Projectile.timeLeft; i++)
			{
				Main.EntitySpriteDraw(textureFlashBack,
					position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
					sourceRectangleSingle, new(10, 10, 10, 0), rotation, origin, Projectile.scale, spriteEffects, 0);
			}

			Main.EntitySpriteDraw(textureLantern,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangleSingle, drawColor, rotation, origin, Projectile.scale, spriteEffects, 0);

			/*Main.EntitySpriteDraw(textureFaeling,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangleFaeling, Color.White, rotation, origin, Projectile.scale, spriteEffects, 0);*/
			DrawNPCDirect_Faeling(Projectile, ref Main.screenPosition, TextureAssets.Npc[NPCID.Shimmerfly].Value, spriteEffects);

			Main.EntitySpriteDraw(textureGlass,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangleSingle, new(lightColor.R, lightColor.G, lightColor.B, 100), rotation, origin, Projectile.scale, spriteEffects, 0);

			return false;
		}

		private static void DrawNPCDirect_Faeling(Projectile rCurrentProj, ref Vector2 screenPos, Texture2D texture, SpriteEffects npcSpriteEffect)
		{
			Vector2 vector = rCurrentProj.Center - screenPos;
			vector.Y += Main.player[rCurrentProj.owner].gfxOffY;
			int verticalFrames = 5;
			int horizontalFrames = 4;
			//int currentFrame = (int)rCurrentProj.ai[1];
			int currentFrame = (int)Main.GameUpdateCount % 30 / 6;
			//Main.NewText(Main.GameUpdateCount % 30 / 6);
			float num2 = ((float)rCurrentProj.whoAmI * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f;
			Color color = Main.hslToRgb(num2, 1f, 0.65f);
			color.A /= 2;
			float rotation = rCurrentProj.rotation;
			Rectangle rectangle = texture.Frame(horizontalFrames, verticalFrames, 0, currentFrame);
			Vector2 origin = rectangle.Size() / 2f;
			float scale = rCurrentProj.scale;
			Rectangle value2 = texture.Frame(horizontalFrames, verticalFrames, 2);
			Color color2 = new Color(255, 255, 255, 0) * 1f;
			int num3 = rCurrentProj.oldPos.Length;
			int num4 = num3 - 1 - 5;
			int num5 = 5;
			int num6 = 3;
			float num7 = 32f;
			float num8 = 16f;
			float fromMax = new Vector2(num7, num8).Length();
			float num9 = Utils.Remap(Vector2.Distance(rCurrentProj.oldPos[num4], rCurrentProj.position), 0f, fromMax, 0f, 100f);
			num9 = (int)num9 / 5;
			num9 *= 5f;
			num9 /= 100f;
			num8 *= num9;
			num7 *= num9;
			float num10 = 9f;
			float num11 = 0.5f;
			float num12 = (float)Math.PI;
			for (int i = num4; i >= num5; i -= num6)
			{
				Vector2 vector2 = rCurrentProj.oldPos[i] - rCurrentProj.position;
				float num14 = Utils.Remap(i, 0f, num3, 1f, 0f);
				float num15 = 1f - num14;
				Vector2 spinningpoint = new Vector2((float)Math.Sin((double)((float)rCurrentProj.whoAmI / 17f) + Main.timeForVisualEffects / (double)num10 + (double)(num14 * 2f * ((float)Math.PI * 2f))) * num8, 0f - num7) * num15;
				vector2 += spinningpoint.RotatedBy(num12);
				Color color3 = Main.hslToRgb((num2 + num15 * num11) % 1f, 1f, 0.5f);
				color3.A = 0;
				Main.EntitySpriteDraw(texture, vector + vector2, value2, color3 * num14 * 0.16f, rotation, origin, scale * Utils.Remap(num14 * num14, 0f, 1f, 0f, 2.5f), npcSpriteEffect, 0f);
			}

			Main.EntitySpriteDraw(texture, vector, value2, color2, rotation, origin, scale, npcSpriteEffect, 0f);
			Rectangle value3 = texture.Frame(horizontalFrames, verticalFrames, 1, currentFrame);
			Color white = Color.White;
			white.A /= 2;
			Main.EntitySpriteDraw(texture, vector, value3, white, rotation, origin, scale, npcSpriteEffect, 0f);
			Main.EntitySpriteDraw(texture, vector, rectangle, color, rotation, origin, scale, npcSpriteEffect, 0f);
			float num16 = MathHelper.Clamp((float)Math.Sin(Main.timeForVisualEffects / 60.0) * 0.3f + 0.3f, 0f, 1f);
			float num17 = 0.8f + (float)Math.Sin(Main.timeForVisualEffects / 15.0 * 6.2831854820251465) * 0.3f;
			Rectangle value4 = texture.Frame(horizontalFrames, verticalFrames, 3, rCurrentProj.whoAmI % verticalFrames);
			Color color4 = Color.Lerp(color, new Color(255, 255, 255, 0), 0.5f) * num16;
			Main.EntitySpriteDraw(texture, vector, value4, color4, rotation, origin, scale * num17, SpriteEffects.None, 0f);
			Rectangle value5 = texture.Frame(horizontalFrames, verticalFrames, 3, 1);
			Color color5 = Color.Lerp(color, new Color(255, 255, 255, 0), 0.5f) * num16;
			Main.EntitySpriteDraw(texture, vector, value5, color5, rotation, origin, scale * num17, SpriteEffects.None, 0f);
		}


		public override void PostDraw(Color lightColor)
		{
			Player owner = Main.player[Projectile.owner];

			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = ((Projectile.spriteDirection <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			if (owner.gravDir == -1f)
			{
				spriteEffects |= SpriteEffects.FlipVertically;
			}

			Texture2D textureFlash = ModContent.Request<Texture2D>(Mod.Name + "/Projectiles/Magic/" + Name + "_Flash").Value;

			Rectangle sourceRectangleFlash = textureFlash.Frame(1, sheetFrames, frameY: Projectile.frame);

			// The origin in this case is (0, 0) of our projectile because Projectile.Center is the tip of our Jousting Lance.
			Vector2 origin = sourceRectangleFlash.Size() / 2f;

			// The rotation of the projectile.
			float rotation = Projectile.rotation;

			// The position of the sprite. Not subtracting Main.player[Projectile.owner].gfxOffY will cause the sprite to bounce when walking up blocks.
			Vector2 position = new(Projectile.Center.X, Projectile.Center.Y - Main.player[Projectile.owner].gfxOffY);

			if (Projectile.timeLeft > 10)
			{
				DrawData flash = new(textureFlash,
					position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
					sourceRectangleFlash, Color.White, rotation, origin, Projectile.scale, spriteEffects, 0);
				GameShaders.Misc["RainbowTownSlime"].Apply(flash);
				Main.EntitySpriteDraw(flash);
			}
		}
	}
}