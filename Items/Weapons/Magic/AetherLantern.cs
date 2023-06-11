using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using RijamsMod.Items.Materials;
using RijamsMod.Items.Placeable;
using RijamsMod.Projectiles.Magic;
using RijamsMod.Projectiles.Summon.Whips;
using System;
using System.Drawing.Imaging;
using System.Reflection.Metadata;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace RijamsMod.Items.Weapons.Magic
{
	public class AetherLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
			GlobalItems.isLanternWeapon.Add(Item.type);
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 44;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.shoot = ModContent.ProjectileType<LanternLightAether>();
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.Red;
			Item.value = 100000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 75;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.autoReuse = true;
			Item.mana = 25;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.4f };
			Item.scale = 0.75f;
			if (!Main.dedServ)
			{
				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
				glowMask.drawOnPlayer =	false;
				glowMask.drawColor = new(100, 100, 100, 0);
			}
			Item.useLimitPerAnimation = 4; // Added by TML.
			Item.noUseGraphic = true;
			Item.channel = true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
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

			Vector2 playerHandPos = player.MountedCenter + new Vector2(player.direction * 15, player.gravDir * 3f);

			// This check sees if the hands are located in a solid block. If so, spawn the projectile at the center of the player instead of inside of the block.
			Point playerTileCoords = playerHandPos.ToTileCoordinates();
			Tile tile = Main.tile[playerTileCoords.X, playerTileCoords.Y];
			if (tile != null && tile.HasUnactuatedTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType] && !TileID.Sets.Platforms[tile.TileType])
			{
				playerHandPos = player.MountedCenter;
			}

			for (int i = 0; i < Item.useLimitPerAnimation; i++)
			{
				// NextVector2Circular is like how far it can randomly choose to target. It'll spread out more with bigger numbers. Nightglow uses 1f, 1f
				// NextVector2CircularEdge determines it's "velocity" or how fast out it'll travel. Nightglow uses 3f, 3f
				Vector2 randomCircular = Main.rand.NextVector2Circular(8f, 8f) + Main.rand.NextVector2CircularEdge(2f, 2f);

				// This will make it always go up instead of sometimes going down.
				if (randomCircular.Y > 0f)
				{
					randomCircular.Y *= -1f;
				}

				// This will make it go in the direction the player is facing instead of randomly any direction.
				if ((randomCircular.X > 0).ToDirectionInt() != player.direction)
				{
				 	randomCircular.X *= -1f;
				}

				Projectile projectile = Projectile.NewProjectileDirect(source, playerHandPos, randomCircular, type, damage, knockback, player.whoAmI, -1f, Main.rand.NextFloat(0.2f, 0.4f));

				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI);
				}

				Lighting.AddLight(player.Center, TorchID.Shimmer);
			}

			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerArrow, new ParticleOrchestraSettings
			{
				PositionInWorld = playerHandPos + new Vector2(player.width / 4 * player.direction, 0),
				MovementVector = velocity
			});

			return false;
		}

		public override void HoldItem(Player player)
		{
			// Don't add the light or dust if the player is on a rope or is petting a town pet. This is because the item is hidden when doing those actions.
			if (player.pulley || player.isPettingAnimal)
			{
				return;
			}

			TorchID.TorchColor(TorchID.Shimmer, out float r, out float g, out float b);
			Color lightColor = new(r, g, b, 1f);

			Vector2 itemPos = player.itemLocation + new Vector2(8 * player.direction, -10f * player.gravDir);
			Vector2 playerPos = player.RotatedRelativePoint(itemPos);
			Lighting.AddLight(playerPos, lightColor.ToVector3());
			if (Main.rand.NextBool(40))
			{
				Vector2 randomCirclular = Main.rand.NextVector2Circular(4f, 4f);
				Dust dust = Dust.NewDustPerfect(playerPos + randomCirclular, DustID.TintableDustLighted, Vector2.Zero, 254, lightColor, 0.3f);
				if (randomCirclular != Vector2.Zero)
				{
					dust.velocity = playerPos.DirectionTo(dust.position) * 0.2f;
				}
				dust.fadeIn = 0.3f;
				dust.noLightEmittence = true;
				dust.customData = this;
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerBlock, new ParticleOrchestraSettings
				{
					PositionInWorld = playerPos + randomCirclular,
					MovementVector = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(0.1f, 1f))
				});
			}
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, TorchID.Shimmer);
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
	}
}