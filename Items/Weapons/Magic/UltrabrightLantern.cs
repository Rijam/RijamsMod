using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Magic
{
	public class UltrabrightLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
			GlobalItems.isLanternWeapon.Add(Item.type);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 40;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.shoot = ModContent.ProjectileType<LanternLightUltrabright>();
			Item.shootSpeed = 1;
			Item.rare = ItemRarityID.Blue;
			Item.value = 7000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 25;
			Item.knockBack = 1.3f;
			Item.noMelee = true;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.autoReuse = true;
			Item.mana = 15;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.8f };
			Item.scale = 0.75f;
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash").Value;
				flash.posOffsetXLeft = 14;
				flash.posOffsetXRight = -52;
				flash.posOffsetY = -34;
				flash.posOffsetYGravity = 53;
				flash.frameCount = 1;
				flash.frameRate = 20;
				flash.alpha = 120;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;
				
				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
				glowMask.flameFlicker = true;
				glowMask.drawColor = new(100, 100, 100, 0);
			}
			//Item.flame = true; // Doesn't create the flame when it is thrown on the ground.
			Item.useLimitPerAnimation = 1; // Added by TML.
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// NextVector2Circular is like how far it can randomly choose to target. It'll spread out more with bigger numbers. Nightglow uses 1f, 1f
			// NextVector2CircularEdge determines it's "velocity" or how fast out it'll travel. Nightglow uses 3f, 3f
			Vector2 randomCircular = Main.rand.NextVector2Circular(3f, 3f) + Main.rand.NextVector2CircularEdge(4f, 4f);
			
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

			Vector2 playerHandPos = player.MountedCenter + new Vector2(player.direction * 15, player.gravDir * 3f);
			
			// This check sees if the hands are located in a solid block. If so, spawn the projectile at the center of the player instead of inside of the block.
			Point playerTileCoords = playerHandPos.ToTileCoordinates();
			Tile tile = Main.tile[playerTileCoords.X, playerTileCoords.Y];
			if (tile != null && tile.HasUnactuatedTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType] && !TileID.Sets.Platforms[tile.TileType])
			{
				playerHandPos = player.MountedCenter;
			}

			Projectile projectile = Projectile.NewProjectileDirect(source, playerHandPos, randomCircular, type, damage, knockback, player.whoAmI, -1f, 0.9f);
			/*projectile.tileCollide = true;
			projectile.ignoreWater = false;
			projectile.penetrate = 2;
			projectile.timeLeft = 500;
			if (projectile.ModProjectile is LanternLight modProjectile)
			{
				modProjectile.timeLeftMax = 500;
				modProjectile.vecolityMultiplier = 7f;
				modProjectile.timeBeforeItCanStartHoming = 420;
				modProjectile.timeLeftBeforeItStopsHoming = 30;
				modProjectile.trailLength = 9;
				modProjectile.shineScale = 0.6f;
				modProjectile.bounceOnTiles = true;
				modProjectile.homingRange = 25 * 16; // 25 tiles
				TorchID.TorchColor(TorchID.UltraBright, out float r, out float g, out float b);
				modProjectile.overrideColor = new Color(r, g, b, 1f) * 0.5f;

				modProjectile.orgTileCollide = true;
				modProjectile.orgIgnoreWater = false;
				modProjectile.orgPenetrate = 2;
			}*/

			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI);
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

			Vector2 itemPos = player.itemLocation + new Vector2(8 * player.direction, -10f * player.gravDir);
			Vector2 playerPos = player.RotatedRelativePoint(itemPos);
			Lighting.AddLight(playerPos, TorchID.UltraBright);
			if (Main.rand.NextBool(40))
			{
				Vector2 randomCirclular = Main.rand.NextVector2Circular(4f, 4f);
				TorchID.TorchColor(TorchID.UltraBright, out float r, out float g, out float b);
				Dust dust = Dust.NewDustPerfect(playerPos + randomCirclular, DustID.TintableDustLighted, Vector2.Zero, 254, new Color(r, b, g, 1f), 0.3f);
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
			Lighting.AddLight(Item.Center, TorchID.UltraBright);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Chain)
				.AddRecipeGroup(RijamsModRecipes.GoldBars, 3)
				.AddIngredient(ItemID.UltrabrightTorch, 3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}