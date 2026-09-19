using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Items.Dyes;
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
			/* Drawn in a different way below.
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>($"{Mod.Name}/Projectiles/Magic/{Name}Proj_Flash");
				flash.posOffsetXLeft = 34;
				flash.posOffsetXRight = -84;
				flash.posOffsetY = -60;
				flash.posOffsetYGravity = 40;
				flash.frameCount = 5;
				flash.frameRate = Item.useAnimation;
				flash.animationLoop = false;
				flash.useRandomFrame = true;
				flash.shader = "RainbowTownSlime";

				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow");
				glowMask.drawOnPlayer =	false;
				glowMask.drawColor = new(100, 100, 100, 0);
			}
			*/
			Item.useLimitPerAnimation = 4; // Added by TML.
			Item.noUseGraphic = false;
			Item.channel = false;
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

			/* Old held projectile
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
			*/

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
			if (player.pulley || player.petting.isPetting)
			{
				return;
			}
			Vector2 itemPos = player.itemLocation + new Vector2(8 * player.direction, -10f * player.gravDir);
			Vector2 playerPos = player.RotatedRelativePoint(itemPos);
			if (!Main.gamePaused && Main.rand.NextBool(40))
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
			if (player.pulley || player.petting.isPetting)
			{
				return;
			}
			
			// Old held projectile:

			// A little jank. player.heldProj is always -1 in HeldItem()? So this spawn a projectile every frame that lives for 2 frames.

			//Main.NewText("Pre HoldStyle " + player.heldProj);
			//Main.NewText("Rect " + heldItemFrame);

			// ai[0] = Flash frame
			/*
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
			*/

			//Main.NewText("Post HoldStyle " + player.heldProj);
		}

		internal readonly Asset<Texture2D> TextureGlass = ModContent.Request<Texture2D>("RijamsMod/Items/Weapons/Magic/Lanterns/AetherLantern_Glass");

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			// Draw the faeling and glass in the inventory.
			SpriteEffects spriteEffects = SpriteEffects.None;

			DrawNPCDirect_Faeling(null, ref Main.screenPosition, TextureAssets.Npc[NPCID.Shimmerfly].Value, spriteEffects, 0f, position, false);

			spriteBatch.Draw(TextureGlass.Value,
				position,
				frame, new(drawColor.R, drawColor.G, drawColor.B, 100), 0f, origin, scale, spriteEffects, 0);

			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}

		public override bool PreDrawInWorld(WorldItem item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			// Draw the faeling and glass in the world.

			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = ((item.direction <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

			Texture2D textureLantern = TextureAssets.Item[Type].Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangleSingle = textureLantern.Bounds;

			Vector2 origin = sourceRectangleSingle.Size() / 2f;

			DrawNPCDirect_Faeling(item, ref Main.screenPosition, TextureAssets.Npc[NPCID.Shimmerfly].Value, spriteEffects, rotation, Vector2.Zero, true);

			spriteBatch.Draw(TextureGlass.Value,
				item.Center - new Vector2(0, 14) - Main.screenPosition,
				sourceRectangleSingle, new(lightColor.R, lightColor.G, lightColor.B, 100), rotation, origin, scale, spriteEffects, 0);

			return base.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}

		internal readonly Asset<Texture2D> FlashBack = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Magic/AetherLanternProj_FlashBack");
		internal readonly Asset<Texture2D> Flash = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Magic/AetherLanternProj_Flash");

		private int frame = 0; // The frame of the texture.

		public override bool ModifyItemDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, ref DrawData? coloredDrawData, ref DrawData? glowMaskDrawData)
		{
			// Draw the faeling and glass in the hand.
			Player player = drawInfo.drawPlayer;

			Vector2 flashBackHandPos = drawInfo.ItemLocation + new Vector2(12f, -16.5f /* + player.gfxOffY Already taken into account */) * player.Directions;

			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = ((player.direction <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			if (player.gravDir == -1f)
			{
				spriteEffects |= SpriteEffects.FlipVertically;
			}

			Texture2D textureLantern = TextureAssets.Item[ModContent.ItemType<AetherLantern>()].Value;

			// Get the currently selected frame on the texture.
			Rectangle lanternBounds = textureLantern.Bounds;

			Vector2 lanternOrigin = lanternBounds.Size() / 2f;

			if (player.ItemAnimationActive)
			{
				float animationPercent = player.itemAnimation / (float)player.itemAnimationMax;
				Color flashBackColor = new(255, 255, 255, 0);

				DrawData flashBack = new(FlashBack.Value,
					flashBackHandPos - new Vector2(lanternBounds.Width / 2f, lanternBounds.Height / 4f) - Main.screenPosition,
					FlashBack.Value.Bounds, flashBackColor * animationPercent, player.itemRotation, lanternOrigin, player.HeldItem.scale, spriteEffects, 0);
				drawInfo.DrawDataCache.Add(flashBack);
			}

			DrawNPCDirect_Faeling(ref drawInfo, ref Main.screenPosition, TextureAssets.Npc[NPCID.Shimmerfly].Value, spriteEffects, player.itemRotation, flashBackHandPos);

			Color lightingColor = Lighting.GetColor(player.Center.ToTileCoordinates());

			DrawData glassDrawData = new(this.TextureGlass.Value,
				flashBackHandPos - Main.screenPosition,
				lanternBounds, new(lightingColor.R, lightingColor.G, lightingColor.B, 100), player.itemRotation, lanternOrigin, player.HeldItem.scale, spriteEffects, 0);
			drawInfo.DrawDataCache.Add(glassDrawData);

			drawInfo.DrawDataCache.Add(drawData); // Draw the item

			// Draw the flash.

			if (player.ItemAnimationActive)
			{
				Vector2 frontFlashHandPos = drawInfo.ItemLocation + new Vector2(12, -13f /* + player.gfxOffY Already taken into account */) * player.Directions;

				if (player.ItemAnimationJustStarted)
				{
					frame = Main.rand.Next(0, 5);
				}
				Rectangle sourceRectangleFlash = Flash.Frame(1, 5, frameY: frame);

				// SpriteEffects change which direction the sprite is drawn.
				/*
				SpriteEffects spriteEffects = ((player.direction <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				if (player.gravDir == -1f)
				{
					spriteEffects |= SpriteEffects.FlipVertically;
				}
				*/

				Vector2 flashOrigin = sourceRectangleFlash.Size() / 2f;
				float animationPercent = player.itemAnimation / (float)player.itemAnimationMax;
				animationPercent = (float)Utils.EaseOutCirc(animationPercent);

				DrawData flash = new(Flash.Value,
					frontFlashHandPos - Main.screenPosition,
					sourceRectangleFlash,
					Color.White * animationPercent,
					player.itemRotation,
					flashOrigin,
					player.HeldItem.scale,
					spriteEffects,
					0)
				{
					shader = GameShaders.Armor.GetShaderIdFromItemId(ModContent.ItemType<AetherDye>())
				};
				// GameShaders.Misc["RainbowTownSlime"].Apply(flash); // Doesn't apply the shader with drawInfo.DrawDataCache :(
				drawInfo.DrawDataCache.Add(flash);
			}

			return false;
		}

		// Copied from vanilla. Modified for items.
		private static void DrawNPCDirect_Faeling(ref PlayerDrawSet drawInfo, ref Vector2 screenPos, Texture2D texture, SpriteEffects itemSpriteEffect, float rotation, Vector2 handPos)
		{
			Vector2 itemCenter = handPos;
			int rWorldItemWhoAmI = 1;
			float scale = 1f;

			itemCenter.X -= 0.5f;
			if (drawInfo.drawPlayer.gravDir == 1)
			{
				itemCenter.Y += 3f;
			}
			else
			{
				itemCenter.Y -= 3f;
			}

			int verticalFrames = 5;
			int horizontalFrames = 4;
			int currentFrame = (int)Main.GameUpdateCount % 30 / 6;
			float colorPulseWings = (rWorldItemWhoAmI * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f;
			Color colorWings = Main.hslToRgb(colorPulseWings, 1f, 0.65f);
			colorWings.A /= 2;
			Rectangle sourceRectBody = texture.Frame(horizontalFrames, verticalFrames, 0, currentFrame);
			Vector2 origin = sourceRectBody.Size() / 2f;

			Rectangle sourceRectGlow = texture.Frame(horizontalFrames, verticalFrames, 2);
			Color color2 = new Color(255, 255, 255, 0) * 1f;

			// Remove all of the after image trail code.

			DrawData drawData = new(texture, itemCenter - screenPos, sourceRectGlow, color2, rotation, origin, scale, itemSpriteEffect, 0f);
			drawInfo.DrawDataCache.Add(drawData);

			Rectangle sourceRectWings = texture.Frame(horizontalFrames, verticalFrames, 1, currentFrame);
			Color white = Color.White;
			white.A /= 2;
			drawData = new(texture, itemCenter - screenPos, sourceRectWings, white, rotation, origin, scale, itemSpriteEffect, 0f);
			drawInfo.DrawDataCache.Add(drawData);
			drawData = new(texture, itemCenter - screenPos, sourceRectBody, colorWings, rotation, origin, scale, itemSpriteEffect, 0f);
			drawInfo.DrawDataCache.Add(drawData);
			float colorPulse = MathHelper.Clamp((float)Math.Sin(Main.timeForVisualEffects / 60.0) * 0.3f + 0.3f, 0f, 1f);
			float scaleMulti = 0.8f + (float)Math.Sin(Main.timeForVisualEffects / 15.0 * MathHelper.TwoPi) * 0.3f;
			Color colorFlash = Color.Lerp(colorWings, new Color(255, 255, 255, 0), 0.5f) * colorPulse;
			Rectangle sourceRectFlash = texture.Frame(horizontalFrames, verticalFrames, 3, rWorldItemWhoAmI % verticalFrames);
			Rectangle sourceRectFlash0 = texture.Frame(horizontalFrames, verticalFrames, 3, 1);
			drawData = new(texture, itemCenter - screenPos, sourceRectFlash, colorFlash, rotation, origin, scale * scaleMulti, SpriteEffects.None, 0f);
			drawInfo.DrawDataCache.Add(drawData);
			drawData = new(texture, itemCenter - screenPos, sourceRectFlash0, colorFlash, rotation, origin, scale * scaleMulti, SpriteEffects.None, 0f);
			drawInfo.DrawDataCache.Add(drawData);
		}

		// Copied from vanilla. Modified for items.ModifyItemDraw
		private static void DrawNPCDirect_Faeling(WorldItem rCurrentItem, ref Vector2 screenPos, Texture2D texture, SpriteEffects itemSpriteEffect, float rotation, Vector2 inventoryPos, bool inWorld)
		{
			Vector2 itemCenter;
			int rWorldItemWhoAmI = 1;
			float rWorldItemScale = 1f;
			if (inWorld)
			{
				itemCenter = rCurrentItem.Center - screenPos;
				itemCenter.Y -= 10f;
				rWorldItemWhoAmI = rCurrentItem.whoAmI;
				rWorldItemScale= rCurrentItem.scale;
			}
			else
			{
				itemCenter = inventoryPos;
				itemCenter.Y += 3f;
			}
			int verticalFrames = 5;
			int horizontalFrames = 4;
			int currentFrame = (int)Main.GameUpdateCount % 30 / 6;
			float colorPulseWings = (rWorldItemWhoAmI * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f;
			Color colorWings = Main.hslToRgb(colorPulseWings, 1f, 0.65f);
			colorWings.A /= 2;
			Rectangle sourceRectBody = texture.Frame(horizontalFrames, verticalFrames, 0, currentFrame);
			Vector2 origin = sourceRectBody.Size() / 2f;
			float scale = rWorldItemScale;
			if (inWorld)
			{
				scale *= 1.5f; // Slightly scaled up in the world.
			}
			else
			{
				scale *= 0.75f;
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
			Rectangle sourceRectFlash = texture.Frame(horizontalFrames, verticalFrames, 3, rWorldItemWhoAmI % verticalFrames);
			Rectangle sourceRectFlash0 = texture.Frame(horizontalFrames, verticalFrames, 3, 1);
			Main.EntitySpriteDraw(texture, itemCenter, sourceRectFlash, colorFlash, rotation, origin, scale * scaleMulti, SpriteEffects.None, 0f);
			Main.EntitySpriteDraw(texture, itemCenter, sourceRectFlash0, colorFlash, rotation, origin, scale * scaleMulti, SpriteEffects.None, 0f);
		}
	}

	// Superseded by ModItem.
	/*
	public class AetherLanternPreDrawLayer : PlayerDrawLayer
	{
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.HeldItem.type != ModContent.ItemType<AetherLantern>())
			{
				return false;
			}
			if (drawInfo.drawPlayer.dead || drawInfo.drawPlayer.shimmering || drawInfo.drawPlayer.stoned || drawInfo.drawPlayer.frozen)
			{
				return false;
			}
			if (drawInfo.drawPlayer.mount.Active && MountID.Sets.DontHoldItems[drawInfo.drawPlayer.mount.Type])
			{
				return false;
			}
			return true;
			// The item is still shown while webbed.
		}
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeldItem);

		internal readonly Asset<Texture2D> FlashBack = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Magic/AetherLanternProj_FlashBack");

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			// Draw the faeling and glass in the hand.
			Player player = drawInfo.drawPlayer;

			if (player.HeldItem.ModItem is AetherLantern aetherLantern)
			{
				Vector2 handPos = drawInfo.ItemLocation + new Vector2(12f, -16.5f + player.gfxOffY) * player.Directions;

				// SpriteEffects change which direction the sprite is drawn.
				SpriteEffects spriteEffects = ((player.direction <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				if (player.gravDir == -1f)
				{
					spriteEffects |= SpriteEffects.FlipVertically;
				}

				Texture2D textureLantern = TextureAssets.Item[ModContent.ItemType<AetherLantern>()].Value;

				// Get the currently selected frame on the texture.
				Rectangle lanternBounds = textureLantern.Bounds;

				Vector2 origin = lanternBounds.Size() / 2f;

				if (player.ItemAnimationActive)
				{
					float animationPercent = player.itemAnimation / (float)player.itemAnimationMax;
					Color flashBackColor = new(255, 255, 255, 0);

					DrawData flashBack = new(FlashBack.Value,
						handPos - new Vector2(lanternBounds.Width / 2f, lanternBounds.Height / 4f) - Main.screenPosition,
						FlashBack.Value.Bounds, flashBackColor * animationPercent, player.itemRotation, origin, player.HeldItem.scale, spriteEffects, 0);
					drawInfo.DrawDataCache.Add(flashBack);
				}

				DrawNPCDirect_Faeling(ref drawInfo, ref Main.screenPosition, TextureAssets.Npc[NPCID.Shimmerfly].Value, spriteEffects, player.itemRotation, handPos);

				Color lightingColor = Lighting.GetColor(player.Center.ToTileCoordinates());

				DrawData drawData = new(aetherLantern.TextureGlass.Value,
					handPos - Main.screenPosition,
					lanternBounds, new(lightingColor.R, lightingColor.G, lightingColor.B, 100), player.itemRotation, origin, player.HeldItem.scale, spriteEffects, 0);
				drawInfo.DrawDataCache.Add(drawData);
			}
		}

		private static void DrawNPCDirect_Faeling(ref PlayerDrawSet drawInfo, ref Vector2 screenPos, Texture2D texture, SpriteEffects itemSpriteEffect, float rotation, Vector2 handPos)
		{
			Vector2 itemCenter = handPos;
			int rWorldItemWhoAmI = 1;
			float scale = 1f;

			itemCenter.X -= 0.5f;
			if (drawInfo.drawPlayer.gravDir == 1)
			{
				itemCenter.Y += 3f;
			}
			else
			{
				itemCenter.Y -= 3f;
			}

			int verticalFrames = 5;
			int horizontalFrames = 4;
			int currentFrame = (int)Main.GameUpdateCount % 30 / 6;
			float colorPulseWings = (rWorldItemWhoAmI * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f;
			Color colorWings = Main.hslToRgb(colorPulseWings, 1f, 0.65f);
			colorWings.A /= 2;
			Rectangle sourceRectBody = texture.Frame(horizontalFrames, verticalFrames, 0, currentFrame);
			Vector2 origin = sourceRectBody.Size() / 2f;

			Rectangle sourceRectGlow = texture.Frame(horizontalFrames, verticalFrames, 2);
			Color color2 = new Color(255, 255, 255, 0) * 1f;

			// Remove all of the after image trail code.

			DrawData drawData = new (texture, itemCenter - screenPos, sourceRectGlow, color2, rotation, origin, scale, itemSpriteEffect, 0f);
			drawInfo.DrawDataCache.Add(drawData);

			Rectangle sourceRectWings = texture.Frame(horizontalFrames, verticalFrames, 1, currentFrame);
			Color white = Color.White;
			white.A /= 2;
			drawData = new(texture, itemCenter - screenPos, sourceRectWings, white, rotation, origin, scale, itemSpriteEffect, 0f);
			drawInfo.DrawDataCache.Add(drawData);
			drawData = new(texture, itemCenter - screenPos, sourceRectBody, colorWings, rotation, origin, scale, itemSpriteEffect, 0f);
			drawInfo.DrawDataCache.Add(drawData);
			float colorPulse = MathHelper.Clamp((float)Math.Sin(Main.timeForVisualEffects / 60.0) * 0.3f + 0.3f, 0f, 1f);
			float scaleMulti = 0.8f + (float)Math.Sin(Main.timeForVisualEffects / 15.0 * MathHelper.TwoPi) * 0.3f;
			Color colorFlash = Color.Lerp(colorWings, new Color(255, 255, 255, 0), 0.5f) * colorPulse;
			Rectangle sourceRectFlash = texture.Frame(horizontalFrames, verticalFrames, 3, rWorldItemWhoAmI % verticalFrames);
			Rectangle sourceRectFlash0 = texture.Frame(horizontalFrames, verticalFrames, 3, 1);
			drawData = new(texture, itemCenter - screenPos, sourceRectFlash, colorFlash, rotation, origin, scale * scaleMulti, SpriteEffects.None, 0f);
			drawInfo.DrawDataCache.Add(drawData);
			drawData = new(texture, itemCenter - screenPos, sourceRectFlash0, colorFlash, rotation, origin, scale * scaleMulti, SpriteEffects.None, 0f);
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
	public class AetherLanternPostDrawLayer : PlayerDrawLayer
	{
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.HeldItem.type != ModContent.ItemType<AetherLantern>())
			{
				return false;
			}
			if (drawInfo.drawPlayer.dead || drawInfo.drawPlayer.shimmering || drawInfo.drawPlayer.stoned || drawInfo.drawPlayer.frozen)
			{
				return false;
			}
			if (drawInfo.drawPlayer.mount.Active && MountID.Sets.DontHoldItems[drawInfo.drawPlayer.mount.Type])
			{
				return false;
			}
			return true;
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

		internal readonly Asset<Texture2D> Flash = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Magic/AetherLanternProj_Flash");

		private int frame = 0; // The frame of the texture.

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			// Draw the flash.

			Player player = drawInfo.drawPlayer;

			if (player.ItemAnimationActive)
			{
				Vector2 handPos = drawInfo.ItemLocation + new Vector2(12, -13f + player.gfxOffY) * player.Directions;

				if (player.ItemAnimationJustStarted)
				{
					frame = Main.rand.Next(0, 5);
				}
				Rectangle sourceRectangleFlash = Flash.Frame(1, 5, frameY: frame);

				// SpriteEffects change which direction the sprite is drawn.
				SpriteEffects spriteEffects = ((player.direction <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				if (player.gravDir == -1f)
				{
					spriteEffects |= SpriteEffects.FlipVertically;
				}

				Vector2 origin = sourceRectangleFlash.Size() / 2f;
				float animationPercent = player.itemAnimation / (float)player.itemAnimationMax;
				animationPercent = (float)Utils.EaseOutCirc(animationPercent);

				DrawData flash = new(Flash.Value,
					handPos - Main.screenPosition,
					sourceRectangleFlash,
					Color.White * animationPercent,
					player.itemRotation,
					origin,
					player.HeldItem.scale,
					spriteEffects,
					0)
				{
					shader = GameShaders.Armor.GetShaderIdFromItemId(ModContent.ItemType<AetherDye>())
				};
				// GameShaders.Misc["RainbowTownSlime"].Apply(flash); // Doesn't apply the shader with drawInfo.DrawDataCache :(
				drawInfo.DrawDataCache.Add(flash);
			}
		}
	}
	*/
}
