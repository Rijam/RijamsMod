using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Items.Materials;
using RijamsMod.Items.Placeable;
using RijamsMod.Projectiles.Magic;
using System;
using System.Drawing.Imaging;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Magic
{
	public class NebulaLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
			GlobalItems.isLanternWeapon.Add(Item.type);
			GlobalItems.fixItemUseStyleIDRaiseLampFrontArmAnimation.Add(Item.type);
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 44;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.shoot = ModContent.ProjectileType<LanternLightNebula>();
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.Cyan;
			Item.value = 80000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 65;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.autoReuse = true;
			Item.mana = 25;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.2f };
			Item.scale = 0.75f;
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash").Value;
				flash.posOffsetXLeft = 24;
				flash.posOffsetXRight = -74;
				flash.posOffsetY = -38;
				flash.posOffsetYGravity = 60;
				flash.frameCount = 3;
				flash.frameRate = 12;
				flash.alpha = 120;
				flash.forceFirstFrame = false;
				flash.animationLoop = true;

				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
				glowMask.flameFlicker = true;
				glowMask.drawColor = new(100, 100, 100, 0);
			}
			//Item.flame = true; // Doesn't create the flame when it is thrown on the ground.
			Item.useLimitPerAnimation = 4; // Added by TML.
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
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
				Vector2 randomCircular = Main.rand.NextVector2Circular(4f, 4f) + Main.rand.NextVector2CircularEdge(4f, 4f);

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

				Projectile projectile = Projectile.NewProjectileDirect(source, playerHandPos, randomCircular, type, damage, knockback, player.whoAmI, -1f, 0.41f);
				/*projectile.tileCollide = true;
				projectile.ignoreWater = true;
				projectile.penetrate = 3;
				projectile.timeLeft = 500;
				if (projectile.ModProjectile is LanternLight modProjectile)
				{
					modProjectile.timeLeftMax = projectile.timeLeft;

					modProjectile.orgTileCollide = true;
					modProjectile.orgIgnoreWater = true;
					modProjectile.orgPenetrate = 3;

					modProjectile.vecolityMultiplier = 30f;
					modProjectile.timeBeforeItCanStartHoming = 470;
					modProjectile.timeLeftBeforeItStopsHoming = 30;
					modProjectile.bounceOnTiles = true;
					modProjectile.overrideColor = new Color(1f, 0.5f, 0.9f, 1f) * 0.5f;
					modProjectile.homingNeedsLineOfSight = true;

					modProjectile.trailLength = 19;
					modProjectile.shineScale = 1.5f;
					modProjectile.homingRange = 55 * 16; // 55 tiles
				}*/
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI);
				}

				Lighting.AddLight(player.Center, new Vector3(1f, 0.47f, 0.59f));

				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.PrincessWeapon, new ParticleOrchestraSettings
				{
					//PositionInWorld = playerHandPos + new Vector2(player.width / 4 * player.direction, 0),
					PositionInWorld = playerHandPos + new Vector2(player.width / 4 * player.direction, 0),
					MovementVector = velocity
				});
			}


			return false;
		}

		public override void HoldItem(Player player)
		{
			// Don't add the light or dust if the player is on a rope or is petting a town pet. This is because the item is hidden when doing those actions.
			if (player.pulley || player.isPettingAnimal)
			{
				return;
			}

			Color lightColor = new(1f, 0.5f, 0.9f, 1f);

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
			}
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, new Vector3(1f, 0.5f, 0.9f));
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FragmentNebula, 6)
				.AddIngredient(ItemID.LunarBlockNebula, 10)
				.AddIngredient(ItemID.NebulaCandle, 2)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}