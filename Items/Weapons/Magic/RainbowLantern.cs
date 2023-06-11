using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Items.Materials;
using RijamsMod.Projectiles.Magic;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Magic
{
	public class RainbowLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
			GlobalItems.isLanternWeapon.Add(Item.type);
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 40;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.shoot = ModContent.ProjectileType<LanternLightRainbow>();
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.Pink;
			Item.value = 35000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 40;
			Item.knockBack = 2.1f;
			Item.noMelee = true;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.autoReuse = true;
			Item.mana = 22;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.3f };
			Item.scale = 0.75f;
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash").Value;
				flash.posOffsetXLeft = 24;
				flash.posOffsetXRight = -70;
				flash.posOffsetY = -46;
				flash.posOffsetYGravity = 53;
				flash.frameCount = 1;
				flash.frameRate = 18;
				flash.colorNoAlpha = Main.DiscoColor;
				flash.alpha = 120;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;
				flash.discoColor = true;

				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
				glowMask.flameFlicker = true;
				glowMask.drawColor = new(Math.Clamp(Main.DiscoR, 0, 100), Math.Clamp(Main.DiscoG, 0, 100), Math.Clamp(Main.DiscoB, 0, 100), 0);
				glowMask.discoColor = true;
			}
			//Item.flame = true; // Doesn't create the flame when it is thrown on the ground.
			Item.useLimitPerAnimation = 3; // Added by TML.
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
				Vector2 randomCircular = Main.rand.NextVector2Circular(3f, 3f) + Main.rand.NextVector2CircularEdge(2f, 2f);
			
				// This will make it always go up instead of sometimes going down.
				if (randomCircular.Y > 0f)
				{
					randomCircular.Y *= -1f;
				}

				// This will make it go in the direction the player is facing instead of randomly any direction.
				//if ((randomCircular.X > 0).ToDirectionInt() != player.direction)
				//{
				// 	randomCircular.X *= -1f;
				//}

				Projectile projectile = Projectile.NewProjectileDirect(source, playerHandPos, randomCircular, type, damage, knockback, player.whoAmI, -1f, Main.rand.NextFloat(0f, 1f));
				/*projectile.tileCollide = true;
				projectile.ignoreWater = false;
				projectile.penetrate = 3;
				projectile.timeLeft = 500;
				if (projectile.ModProjectile is LanternLight modProjectile)
				{
					modProjectile.timeLeftMax = projectile.timeLeft;
					modProjectile.vecolityMultiplier = 15f;
					modProjectile.timeBeforeItCanStartHoming = 440;
					modProjectile.timeLeftBeforeItStopsHoming = 60;
					modProjectile.trailLength = 20;
					modProjectile.shineScale = 1f;
					modProjectile.bounceOnTiles = true;
					modProjectile.homingRange = 35 * 16; // 35 tiles
					modProjectile.overrideColor = Main.DiscoColor * 0.5f;
					modProjectile.homingNeedsLineOfSight = true;

					modProjectile.orgTileCollide = true;
					modProjectile.orgIgnoreWater = false;
					modProjectile.orgPenetrate = 3;
				}*/
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI);
				}
			}

			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.RainbowRodHit, new ParticleOrchestraSettings
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

			Vector2 itemPos = player.itemLocation + new Vector2(8 * player.direction, -10f * player.gravDir);
			Vector2 playerPos = player.RotatedRelativePoint(itemPos);
			Lighting.AddLight(playerPos, Main.DiscoColor.ToVector3());
			if (Main.rand.NextBool(40))
			{
				Vector2 randomCirclular = Main.rand.NextVector2Circular(4f, 4f);
				Dust dust = Dust.NewDustPerfect(playerPos + randomCirclular, DustID.TintableDustLighted, Vector2.Zero, 254, Main.DiscoColor, 0.3f);
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
			Lighting.AddLight(Item.Center, Main.DiscoColor.ToVector3());
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SoulofMight, 2)
				.AddIngredient(ItemID.HallowedBar, 3)
				.AddIngredient(ItemID.RainbowTorch, 3)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}