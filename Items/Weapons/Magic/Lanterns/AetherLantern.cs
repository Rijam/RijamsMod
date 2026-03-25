using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Magic;

namespace RijamsMod.Items.Weapons.Magic.Lanterns
{
	public class AetherLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 32;
			Item.height = 44;
			Item.shoot = ModContent.ProjectileType<LanternLightAether>();
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.Red;
			Item.value = 100000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 75;
			Item.knockBack = 3f;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.autoReuse = true;
			Item.mana = 25;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.4f };
			/*if (!Main.dedServ)
			{
				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
				glowMask.drawOnPlayer =	false;
				glowMask.drawColor = new(100, 100, 100, 0);
			}*/
			Item.useLimitPerAnimation = 4; // Added by TML.
			Item.noUseGraphic = true;
			Item.channel = true;
		}

		public override Color LightColor()
		{
			TorchID.TorchColor(TorchID.Shimmer, out float r, out float g, out float b);
			return new Color(r, g, b, 1f);
		}
		public override float ProjColorFloat()
		{
			return Main.rand.NextFloat(0.2f, 0.4f);
		}
		public override float ProjSpawnDistance()
		{
			return 8f;
		}
		public override float ProjSpawnVelocity()
		{
			return 2f;
		}
		public override bool ShootInPlayerDirection()
		{
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			base.Shoot(player, source, position, velocity, type, damage, knockback);

			//Main.NewText("Pre Shoot " + player.heldProj);
			// ai[0] = Flash frame
			if (player.heldProj < 0)
			{
				Projectile heldProj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.RotatedRelativePoint(player.MountedCenter), Vector2.Zero, ModContent.ProjectileType<AetherLanternProj>(), -1, -1, player.whoAmI, Main.rand.Next(0, 5));
				player.heldProj = heldProj.whoAmI;
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, heldProj.whoAmI);
				}
			}

			//Main.NewText("Post Shoot " + player.heldProj);

			for (int i = 0; i < Item.useLimitPerAnimation; i++)
			{
				Lighting.AddLight(player.Center, TorchID.Shimmer);
			}

			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerArrow, new ParticleOrchestraSettings
			{
				PositionInWorld = PlayerHandPos + new Vector2(player.width / 4 * player.direction, 0),
				MovementVector = velocity
			});

			return false;
		}

		public override void HoldItem(Player player)
		{
			base.HoldItem(player);
			// Don't add the light or dust if the player is on a rope or is petting a town pet. This is because the item is hidden when doing those actions.
			if (player.pulley || player.isPettingAnimal)
			{
				return;
			}
			Vector2 itemPos = player.itemLocation + new Vector2(8 * player.direction, -10f * player.gravDir);
			Vector2 playerPos = player.RotatedRelativePoint(itemPos);
			if (Main.rand.NextBool(40))
			{
				Vector2 randomCirclular = Main.rand.NextVector2Circular(4f, 4f);
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerBlock, new ParticleOrchestraSettings
				{
					PositionInWorld = playerPos + randomCirclular,
					MovementVector = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(0.1f, 1f))
				});
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.ShimmerBlock, 12)
				.AddIngredient(ItemID.LunarBar, 6)
				.AddIngredient(ItemID.ShimmerflyinaBottle, 1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}

		public override void HoldStyle(Player player, Rectangle heldItemFrame)
		{
			// Don't add the light or dust if the player is on a rope or is petting a town pet. This is because the item is hidden when doing those actions.
			if (player.pulley || player.isPettingAnimal)
			{
				return;
			}

			// A little jank. player.heldProj is always -1 in HeldItem()? So this spawn a projectile every frame that lives for 2 frames.

			//Main.NewText("Pre HoldStyle " + player.heldProj);
			//Main.NewText("Rect " + heldItemFrame);

			// ai[0] = Flash frame
			if (player.heldProj < 0 && player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<AetherLanternProj>()] < 1)
			{
				//player.GetModPlayer<RijamsModPlayer>().holdingAetherLantern = true;
				Projectile heldProj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.RotatedRelativePoint(player.MountedCenter), Vector2.Zero, ModContent.ProjectileType<AetherLanternProj>(), -1, -1, player.whoAmI, 0, 0, 1f);
				heldProj.timeLeft = 999;
				player.heldProj = heldProj.whoAmI;
				//Main.NewText(heldProj.whoAmI);
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, heldProj.whoAmI);
				}
			}

			//Main.NewText("Post HoldStyle " + player.heldProj);
		}

		private readonly Asset<Texture2D> TextureGlass = ModContent.Request<Texture2D>("RijamsMod/Items/Weapons/Magic/Lanterns/AetherLantern_Glass");

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = ((Item.direction <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

			DrawNPCDirect_Faeling(Item, ref Main.screenPosition, TextureAssets.Npc[NPCID.Shimmerfly].Value, spriteEffects, 0f, position, false);

			spriteBatch.Draw(TextureGlass.Value,
				position,
				frame, new(drawColor.R, drawColor.G, drawColor.B, 100), 0f, origin, scale, spriteEffects, 0);

			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = ((Item.direction <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

			Texture2D textureLantern = TextureAssets.Item[Type].Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangleSingle = textureLantern.Bounds;

			Vector2 origin = sourceRectangleSingle.Size() / 2f;

			DrawNPCDirect_Faeling(Item, ref Main.screenPosition, TextureAssets.Npc[NPCID.Shimmerfly].Value, spriteEffects, rotation, Vector2.Zero, true);

			spriteBatch.Draw(TextureGlass.Value,
				Item.Center - Main.screenPosition,
				sourceRectangleSingle, new(lightColor.R, lightColor.G, lightColor.B, 100), rotation, origin, scale, spriteEffects, 0);

			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}

		// Copied from vanilla. Modified for items.
		private static void DrawNPCDirect_Faeling(Item rCurrentItem, ref Vector2 screenPos, Texture2D texture, SpriteEffects itemSpriteEffect, float rotation, Vector2 inventoryPos, bool inWorld)
		{
			Vector2 itemCenter;
			if (inWorld)
			{
				itemCenter = rCurrentItem.Center - screenPos;
				itemCenter.Y += 5f;
			}
			else
			{
				itemCenter = inventoryPos;
				itemCenter.Y += 3f;
			}
			int verticalFrames = 5;
			int horizontalFrames = 4;
			int currentFrame = (int)Main.GameUpdateCount % 30 / 6;
			float colorPulseWings = (rCurrentItem.whoAmI * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f;
			Color colorWings = Main.hslToRgb(colorPulseWings, 1f, 0.65f);
			colorWings.A /= 2;
			Rectangle sourceRectBody = texture.Frame(horizontalFrames, verticalFrames, 0, currentFrame);
			Vector2 origin = sourceRectBody.Size() / 2f;
			float scale = rCurrentItem.scale;
			if (inWorld)
			{
				scale *= 1.5f; // Slightly scaled up in the world.
			}
			Rectangle sourceRectGlow = texture.Frame(horizontalFrames, verticalFrames, 2);
			Color color2 = new Color(255, 255, 255, 0) * 1f;

			// Remove all of the after image trail code.

			Main.EntitySpriteDraw(texture, itemCenter, sourceRectGlow, color2, rotation, origin, scale, itemSpriteEffect, 0f);
			Rectangle sourceRectWings = texture.Frame(horizontalFrames, verticalFrames, 1, currentFrame);
			Color white = Color.White;
			white.A /= 2;
			Main.EntitySpriteDraw(texture, itemCenter, sourceRectWings, white, rotation, origin, scale, itemSpriteEffect, 0f);
			Main.EntitySpriteDraw(texture, itemCenter, sourceRectBody, colorWings, rotation, origin, scale, itemSpriteEffect, 0f);
			float colorPulse = MathHelper.Clamp((float)Math.Sin(Main.timeForVisualEffects / 60.0) * 0.3f + 0.3f, 0f, 1f);
			float scaleMulti = 0.8f + (float)Math.Sin(Main.timeForVisualEffects / 15.0 * MathHelper.TwoPi) * 0.3f;
			Color colorFlash = Color.Lerp(colorWings, new Color(255, 255, 255, 0), 0.5f) * colorPulse;
			Rectangle sourceRectFlash = texture.Frame(horizontalFrames, verticalFrames, 3, rCurrentItem.whoAmI % verticalFrames);
			Rectangle sourceRectFlash0 = texture.Frame(horizontalFrames, verticalFrames, 3, 1);
			Main.EntitySpriteDraw(texture, itemCenter, sourceRectFlash, colorFlash, rotation, origin, scale * scaleMulti, SpriteEffects.None, 0f);
			Main.EntitySpriteDraw(texture, itemCenter, sourceRectFlash0, colorFlash, rotation, origin, scale * scaleMulti, SpriteEffects.None, 0f);
		}
	}
}