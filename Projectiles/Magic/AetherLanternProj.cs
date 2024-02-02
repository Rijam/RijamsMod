using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

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

			// If the Lantern is no longer being used, kill the projectile.
			if (owner.ItemAnimationEndingOrEnded)
			{
				Projectile.Kill();
				return;
			}
		}
		public override bool PreAI()
		{
			Player owner = Main.player[Projectile.owner]; // Get the owner of the projectile.

			//Main.NewText(owner.heldProj);

			Projectile.direction = owner.direction; // Set the direction of the projectile to the same direction as the owner.
			Projectile.spriteDirection = Projectile.direction;
			owner.heldProj = Projectile.whoAmI; // Set the owner's held projectile to this projectile.
			Projectile.rotation = owner.fullRotation; // Rotate based on the owner's rotation (when riding mounts and stuff).

			Vector2 pos = owner.GetBackHandPosition(Player.CompositeArmStretchAmount.Full, MathHelper.PiOver4);
			if (owner.gravDir == -1f)
			{
				pos.Y -= owner.height / 3.5f;
			}
			if (owner.ItemAnimationActive)
			{
				pos = owner.GetBackHandPosition(Player.CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 + 0.5f);
			}
			if (owner.direction > 0)
			{
				pos.X += owner.width;
				pos.Y += Projectile.rotation * 10f; // Move the position based on the rotation.
				pos.X += Projectile.rotation * 10f; // These values are just guesswork and aren't perfect, but they are good enough.
			}
			else
			{
				pos.X -= owner.width / 4f;
				pos.Y -= Projectile.rotation * 20f;
				pos.X += Projectile.rotation * 20f;
			}
			
			Projectile.Center = pos;

			// If the Jousting Lance is no longer being used, kill the projectile.
			if (owner.ItemAnimationEndingOrEnded)
			{
				Projectile.Kill();
			}

			// frame is the flash frame
			Projectile.frame = (int)Projectile.ai[0];
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

		// Copied from vanilla. Modified for projectiles.
		private static void DrawNPCDirect_Faeling(Projectile rCurrentProj, ref Vector2 screenPos, Texture2D texture, SpriteEffects projSpriteEffect)
		{
			Vector2 projCenter = rCurrentProj.Center - screenPos;
			projCenter.Y += Main.player[rCurrentProj.owner].gfxOffY;
			int verticalFrames = 5;
			int horizontalFrames = 4;
			//int currentFrame = (int)rCurrentProj.ai[1];
			int currentFrame = (int)Main.GameUpdateCount % 30 / 6;
			//Main.NewText(Main.GameUpdateCount % 30 / 6);
			float colorPulseWings = (rCurrentProj.whoAmI * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f;
			Color colorWings = Main.hslToRgb(colorPulseWings, 1f, 0.65f);
			colorWings.A /= 2;
			float rotation = rCurrentProj.rotation;
			Rectangle sourceRectBody = texture.Frame(horizontalFrames, verticalFrames, 0, currentFrame);
			Vector2 origin = sourceRectBody.Size() / 2f;
			float scale = rCurrentProj.scale;
			Rectangle sourceRectGlow = texture.Frame(horizontalFrames, verticalFrames, 2);
			Color color2 = new Color(255, 255, 255, 0) * 1f;
			int oldPosLength = rCurrentProj.oldPos.Length;
			int currentOldPos = oldPosLength - 6;
			float spinningPointY = 32f;
			float spinningPointX = 16f;
			float fromMax = new Vector2(spinningPointY, spinningPointX).Length();
			float lerpValue = Utils.Remap(Vector2.Distance(rCurrentProj.oldPos[currentOldPos], rCurrentProj.position), 0f, fromMax, 0f, 100f);
			lerpValue = (int)lerpValue / 5;
			lerpValue *= 5f;
			lerpValue /= 100f;
			spinningPointX *= lerpValue;
			spinningPointY *= lerpValue;
			float numIs9f = 9f;
			float numIs05f = 0.5f;
			for (int i = currentOldPos; i >= 5; i -= 3)
			{
				Vector2 projOldPos = rCurrentProj.oldPos[i] - rCurrentProj.position;
				float oldPosLerp = Utils.Remap(i, 0f, oldPosLength, 1f, 0f);
				float oldPosLerpInverse = 1f - oldPosLerp;
				Vector2 spinningpoint = new Vector2((float)Math.Sin((double)(rCurrentProj.whoAmI / 17f) + Main.timeForVisualEffects / (double)numIs9f + (double)(oldPosLerp * 2f * ((float)Math.PI * 2f))) * spinningPointX, 0f - spinningPointY) * oldPosLerpInverse;
				projOldPos += spinningpoint.RotatedBy(Math.PI);
				Color colorGlow = Main.hslToRgb((colorPulseWings + oldPosLerpInverse * numIs05f) % 1f, 1f, 0.5f);
				colorGlow.A = 0;
				Main.EntitySpriteDraw(texture, projCenter + projOldPos, sourceRectGlow, colorGlow * oldPosLerp * 0.16f, rotation, origin, scale * Utils.Remap(oldPosLerp * oldPosLerp, 0f, 1f, 0f, 2.5f), projSpriteEffect, 0f);
			}

			Main.EntitySpriteDraw(texture, projCenter, sourceRectGlow, color2, rotation, origin, scale, projSpriteEffect, 0f);
			Rectangle sourceRectWings = texture.Frame(horizontalFrames, verticalFrames, 1, currentFrame);
			Color white = Color.White;
			white.A /= 2;
			Main.EntitySpriteDraw(texture, projCenter, sourceRectWings, white, rotation, origin, scale, projSpriteEffect, 0f);
			Main.EntitySpriteDraw(texture, projCenter, sourceRectBody, colorWings, rotation, origin, scale, projSpriteEffect, 0f);
			float colorPulse = MathHelper.Clamp((float)Math.Sin(Main.timeForVisualEffects / 60.0) * 0.3f + 0.3f, 0f, 1f);
			float scaleMulti = 0.8f + (float)Math.Sin(Main.timeForVisualEffects / 15.0 * MathHelper.TwoPi) * 0.3f;
			Color colorFlash = Color.Lerp(colorWings, new Color(255, 255, 255, 0), 0.5f) * colorPulse;
			Rectangle sourceRectFlash = texture.Frame(horizontalFrames, verticalFrames, 3, rCurrentProj.whoAmI % verticalFrames);
			Rectangle sourceRectFlash0 = texture.Frame(horizontalFrames, verticalFrames, 3, 1);
			Main.EntitySpriteDraw(texture, projCenter, sourceRectFlash, colorFlash, rotation, origin, scale * scaleMulti, SpriteEffects.None, 0f);
			Main.EntitySpriteDraw(texture, projCenter, sourceRectFlash0, colorFlash, rotation, origin, scale * scaleMulti, SpriteEffects.None, 0f);
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