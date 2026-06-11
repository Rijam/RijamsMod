using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Magic;
using Terraria.GameContent.Drawing;

namespace RijamsMod.Items.Weapons.Magic.Lanterns
{
	public class MagicLanternBase : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(MagicLanternBase);
		public override string Texture => Item.type == ModContent.ItemType<MagicLanternBase>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');

		public override void SetStaticDefaults()
		{
			CustomItemIDSets.IsLanternWeapon[Type] = true;
		}

		/// <summary>
		/// The color of the light emitted.
		/// </summary>
		/// <returns>The color of the normal torch.</returns>
		public virtual Color LightColor()
		{
			TorchID.TorchColor(TorchID.Torch, out float r, out float g, out float b);
			return new(r, g, b, 1);
		}

		/// <summary>
		/// The color of the projectile.
		/// </summary>
		/// <returns>1f</returns>
		public virtual float ProjColorFloat()
		{
			return 1f;
		}

		/// <summary>
		/// NextVector2Circular is like how far it can randomly choose to target. It'll spread out more with bigger numbers. Nightglow uses 1f, 1f
		/// </summary>
		/// <returns>1f</returns>
		public virtual float ProjSpawnDistance()
		{
			return 1f;
		}

		/// <summary>
		/// NextVector2CircularEdge determines it's "velocity" or how fast out it'll travel. Nightglow uses 3f, 3f
		/// </summary>
		/// <returns>3f</returns>
		public virtual float ProjSpawnVelocity()
		{
			return 3f;
		}

		/// <summary>
		/// If true, the projectile will go in the direction the player is facing instead of randomly any direction.
		/// </summary>
		/// <returns>false by default</returns>
		public virtual bool ShootInPlayerDirection()
		{
			return false;
		}

		/// <summary>
		/// The position of the shooter's hand.
		/// </summary>
		internal Vector2 PlayerHandPos = Vector2.Zero;

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 40;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.shoot = ModContent.ProjectileType<LanternLightShining>();
			Item.shootSpeed = 4;
			Item.rare = ItemRarityID.White;
			Item.value = 2000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 17;
			Item.knockBack = 1.2f;
			Item.noMelee = true;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.autoReuse = true;
			Item.mana = 10;
			Item.UseSound = SoundID.Item82 with { Pitch = 1f };
			Item.scale = 0.75f;
			//Item.flame = true; // Doesn't create the flame when it is thrown on the ground.
			Item.useLimitPerAnimation = 1; // Added by TML. The number of projectiles.
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
			PlayerHandPos = playerHandPos;

			for (int i = 0; i < Item.useLimitPerAnimation; i++)
			{
				// NextVector2Circular is like how far it can randomly choose to target. It'll spread out more with bigger numbers. Nightglow uses 1f, 1f
				// NextVector2CircularEdge determines it's "velocity" or how fast out it'll travel. Nightglow uses 3f, 3f
				Vector2 randomCircular = Main.rand.NextVector2Circular(ProjSpawnDistance(), ProjSpawnDistance()) + Main.rand.NextVector2CircularEdge(ProjSpawnVelocity(), ProjSpawnVelocity());

				// This will make it always go up instead of sometimes going down.
				if (randomCircular.Y > 0f)
				{
					randomCircular.Y *= -1f;
				}

				// This will make it go in the direction the player is facing instead of randomly any direction.
				if (ShootInPlayerDirection())
				{
					if ((randomCircular.X > 0).ToDirectionInt() != player.direction)
					{
				 		randomCircular.X *= -1f;
					}
				}

				Projectile projectile = Projectile.NewProjectileDirect(source, playerHandPos, randomCircular, type, damage, knockback, player.whoAmI, -1f, ProjColorFloat());
				/*projectile.tileCollide = true;
				projectile.ignoreWater = false;
				projectile.penetrate = 1;
				projectile.timeLeft = 500;
				if (projectile.ModProjectile is LanternLightShining modProjectile)
				{
					modProjectile.timeLeftMax = projectile.timeLeft;
					modProjectile.vecolityMultiplier = 5f;
					modProjectile.timeBeforeItCanStartHoming = 380;
					modProjectile.timeLeftBeforeItStopsHoming = 60;
					modProjectile.trailLength = 7;
					modProjectile.shineScale = 0.5f;
					modProjectile.bounceOnTiles = false;
					modProjectile.homingRange = 20 * 16; // 20 tiles
					TorchID.TorchColor(TorchID.Torch, out float r, out float g, out float b);
					modProjectile.overrideColor = new Color(r, g, b, 1f) * 0.5f;

					modProjectile.orgTileCollide = true;
					modProjectile.orgIgnoreWater = false;
					modProjectile.orgPenetrate = 1;
				}*/
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI);
				}
			}

			return false;
		}

		public override void HoldItem(Player player)
		{
			// Don't add the light or dust if the player is on a rope or is petting a town pet. This is because the item is hidden when doing those actions.
			if (player.pulley || player.petting.isPetting)
			{
				return;
			}

			Vector2 itemPos = player.itemLocation + new Vector2(8 * player.direction, -10f * player.gravDir);
			Vector2 playerPos = player.RotatedRelativePoint(itemPos);
			Lighting.AddLight(playerPos, LightColor().ToVector3());
			if (Main.rand.NextBool(40))
			{
				Vector2 randomCirclular = Main.rand.NextVector2Circular(4f, 4f);
				Dust dust = Dust.NewDustPerfect(playerPos + randomCirclular, DustID.TintableDustLighted, Vector2.Zero, 254, LightColor(), 0.3f);
				if (randomCirclular != Vector2.Zero)
				{
					dust.velocity = playerPos.DirectionTo(dust.position) * 0.2f;
				}
				dust.fadeIn = 0.3f;
				dust.noLightEmittance = true;
				dust.customData = this;
			}
		}
		public override void PostUpdate(WorldItem item)
		{
			Lighting.AddLight(item.Center, LightColor().ToVector3());
		}
	}
}