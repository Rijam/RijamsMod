using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Items.Placeable;
using RijamsMod.Projectiles.Magic;

namespace RijamsMod.Items.Weapons.Magic.Lanterns
{
	public class AvoliteLantern : MagicLanternBase
	{
		public int colorMode = 0;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 24;
			Item.height = 40;
			Item.shoot = ModContent.ProjectileType<LanternLightAvoliteRed>();
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.Yellow;
			Item.value = 50000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 55;
			Item.knockBack = 3f;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.autoReuse = true;
			Item.mana = 25;
			Item.UseSound = SoundID.Item82 with { Pitch = -0.9f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 4;
				flash.posOffsetXRight = -48;
				flash.posOffsetY = -32;
				flash.posOffsetYGravity = 53;
				flash.frameCount = 6;
				flash.frameRate = 25;
				flash.alpha = 120;
				flash.forceFirstFrame = false;
				flash.animationLoop = false;

				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow");
			}
			
			Item.useLimitPerAnimation = 4; // Added by TML.
		}

		public override Color LightColor()
		{
			return new Color(1f, 0.02f, 0.24f, 1f);
		}
		public override float ProjColorFloat()
		{
			return 0f;
		}
		public override float ProjSpawnDistance()
		{
			return 2f;
		}
		public override float ProjSpawnVelocity()
		{
			return 3f;
		}
		public override bool ShootInPlayerDirection()
		{
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			colorMode = Main.rand.Next(0, 3);
			var flashPlayer = ModContent.GetInstance<WeaponAttackFlashLayer>();
			flashPlayer.frame = colorMode * 2;
			flashPlayer.Timer = 0;

			float spawnColor = 0f;
			switch (colorMode)
			{
				case 0: // Red
					spawnColor = 0.5f;
					break;
				case 1: // Yellow
					spawnColor = 0.7f;
					break;
				case 2: // Blue
					spawnColor = 0.1f;
					break;
				default:
					break;
			}

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
				Vector2 randomCircular = Main.rand.NextVector2Circular(ProjSpawnDistance() + colorMode, ProjSpawnDistance() + colorMode) + Main.rand.NextVector2CircularEdge(ProjSpawnVelocity(), ProjSpawnVelocity());

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

				int projType = type;

				switch (colorMode)
				{
					case 0: // Red
						projType = type;
						break;
					case 1: // Yellow
						projType = ModContent.ProjectileType<LanternLightAvoliteYellow>();
						break;
					case 2: // Blue
						projType = ModContent.ProjectileType<LanternLightAvoliteBlue>();
						break;
					default:
						break;
				}

				Projectile projectile = Projectile.NewProjectileDirect(source, playerHandPos, randomCircular, projType, damage, knockback, player.whoAmI, -1f, spawnColor);
				/*projectile.tileCollide = true;
				projectile.ignoreWater = false;
				projectile.penetrate = 3;
				projectile.timeLeft = 500;
				if (projectile.ModProjectile is LanternLight modProjectile)
				{
					modProjectile.timeLeftMax = projectile.timeLeft;

					modProjectile.orgTileCollide = true;
					modProjectile.orgIgnoreWater = false;
					modProjectile.orgPenetrate = 3;

					switch (colorMode)
					{
						case 0: // Red
							modProjectile.vecolityMultiplier = 25f;
							modProjectile.timeBeforeItCanStartHoming = 450;
							modProjectile.timeLeftBeforeItStopsHoming = 50;
							modProjectile.bounceOnTiles = true;
							modProjectile.overrideColor = new Color(1f, 0.47f, 0.59f, 1f) * 0.5f;
							modProjectile.homingNeedsLineOfSight = true;
							break;
						case 1: // Yellow
							modProjectile.vecolityMultiplier = 10f;
							modProjectile.timeBeforeItCanStartHoming = 400;
							modProjectile.timeLeftBeforeItStopsHoming = 70;
							modProjectile.orgTileCollide = false;
							modProjectile.bounceOnTiles = false;
							modProjectile.overrideColor = new Color(1f, 0.99f, 0.47f, 1f) * 0.5f;
							modProjectile.homingNeedsLineOfSight = false;
							break;
						case 2: // Blue
							modProjectile.vecolityMultiplier = 45f;
							modProjectile.timeBeforeItCanStartHoming = 490;
							modProjectile.timeLeftBeforeItStopsHoming = 10;
							modProjectile.bounceOnTiles = false;
							modProjectile.overrideColor = new Color(0.47f, 0.82f, 1f, 1f) * 0.5f;
							modProjectile.homingNeedsLineOfSight = true;
							break;
						default:
							break;
					}
					modProjectile.trailLength = 18;
					modProjectile.shineScale = 1.2f;
					modProjectile.homingRange = 50 * 16; // 50 tiles
				}*/
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI);
				}

				switch (colorMode)
				{
					case 0:
						Lighting.AddLight(player.Center, new Vector3(1f, 0.47f, 0.59f));
						break;
					case 1:
						Lighting.AddLight(player.Center, new Vector3(1f, 0.99f, 0.47f));
						break;
					case 2:
						Lighting.AddLight(player.Center, new Vector3(0.47f, 0.82f, 1f));
						break;
					default:
						break;
				}
			}

			int particleColor = 511;

			switch (colorMode)
			{
				case 0: // Red
					particleColor = 0;
					break;
				case 1: // Yellow
					particleColor = 46; //65
					break;
				case 2: // Blue
					particleColor = 170; //240
					break;
				default:
					break;
			}

			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings
			{
				PositionInWorld = playerHandPos + new Vector2(player.width / 4 * player.direction, 0),
				MovementVector = velocity,
				UniqueInfoPiece = particleColor
			});

			colorMode = 4;

			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CrystalShard, 2)
				.AddIngredient(ModContent.ItemType<SunplatePillarBlock>(), 3)
				.AddIngredient(ModContent.ItemType<AvoliteCandle>())
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}