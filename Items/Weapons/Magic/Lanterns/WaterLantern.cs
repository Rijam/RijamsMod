using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Magic;

namespace RijamsMod.Items.Weapons.Magic.Lanterns
{
	public class WaterLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 20;
			Item.height = 40;
			Item.shoot = ModContent.ProjectileType<LanternLightWater>();
			Item.shootSpeed = 4;
			Item.rare = ItemRarityID.Green;
			Item.value = 15000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 26;
			Item.knockBack = 1.5f;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.autoReuse = true;
			Item.mana = 20;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.75f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 17;
				flash.posOffsetXRight = -57;
				flash.posOffsetY = -38;
				flash.posOffsetYGravity = 53;
				flash.frameCount = 1;
				flash.frameRate = 20;
				flash.alpha = 120;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;

				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow");
				glowMask.flameFlicker = true;
				glowMask.drawColor = new(100, 100, 100, 0);
			}

			Item.useLimitPerAnimation = 2; // Added by TML.
		}

		public override Color LightColor()
		{
			return new Color(0f, 0.35f, 0.8f, 1f);
		}
		public override float ProjColorFloat()
		{
			return 0.15f;
		}
		public override float ProjSpawnDistance()
		{
			return 5f;
		}
		public override float ProjSpawnVelocity()
		{
			return 4f;
		}

		public override void HoldItem(Player player)
		{
			base.HoldItem(player);
			// Don't add the light or dust if the player is on a rope or is petting a town pet. This is because the item is hidden when doing those actions.
			if (player.pulley || player.isPettingAnimal)
			{
				return;
			}

			player.AddBuff(BuffID.WaterCandle, 4);
			player.aggro += 15 * 16; // 15 tiles (240)
			player.ZoneWaterCandle = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Bone)
				.AddRecipeGroup(RijamsModRecipes.EvilBars, 3)
				.AddIngredient(ItemID.WaterCandle, 3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}