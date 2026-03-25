using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Magic;

namespace RijamsMod.Items.Weapons.Magic.Lanterns
{
	public class NebulaLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 30;
			Item.height = 44;
			Item.shoot = ModContent.ProjectileType<LanternLightNebula>();
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.Cyan;
			Item.value = 80000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 65;
			Item.knockBack = 3f;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.autoReuse = true;
			Item.mana = 25;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.2f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
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
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow");
				glowMask.flameFlicker = true;
				glowMask.drawColor = new(100, 100, 100, 0);
			}
			
			Item.useLimitPerAnimation = 4; // Added by TML.
		}

		public override Color LightColor()
		{
			return new Color(1f, 0.5f, 0.9f, 1f);
		}
		public override float ProjColorFloat()
		{
			return 0.41f;
		}
		public override float ProjSpawnDistance()
		{
			return 4f;
		}
		public override float ProjSpawnVelocity()
		{
			return 4f;
		}
		public override bool ShootInPlayerDirection()
		{
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			base.Shoot(player, source, position, velocity, type, damage, knockback);

			Lighting.AddLight(player.Center, LightColor().ToVector3());

			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.PrincessWeapon, new ParticleOrchestraSettings
			{
				PositionInWorld = PlayerHandPos + new Vector2(player.width / 4 * player.direction, 0),
				MovementVector = velocity
			});

			return false;
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