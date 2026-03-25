using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Magic;

namespace RijamsMod.Items.Weapons.Magic.Lanterns
{
	public class ShadowLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 20;
			Item.height = 40;
			Item.shoot = ModContent.ProjectileType<LanternLightShadow>();
			Item.shootSpeed = 4;
			Item.rare = ItemRarityID.Green;
			Item.value = 15000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 36;
			Item.knockBack = 1.8f;
			Item.useTime = 46;
			Item.useAnimation = 46;
			Item.autoReuse = true;
			Item.mana = 20;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.75f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 19;
				flash.posOffsetXRight = -60;
				flash.posOffsetY = -42;
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
			return new Color(0.2f, 0.3f, 0.32f, 1f);
		}
		public override float ProjColorFloat()
		{
			return 0f;
		}
		public override float ProjSpawnDistance()
		{
			return 1f;
		}
		public override float ProjSpawnVelocity()
		{
			return 5f;
		}

		public override void HoldItem(Player player)
		{
			base.HoldItem(player);
			// Don't add the light or dust if the player is on a rope or is petting a town pet. This is because the item is hidden when doing those actions.
			if (player.pulley || player.isPettingAnimal)
			{
				return;
			}

			player.AddBuff(BuffID.ShadowCandle, 4);
			player.aggro += 5 * 16; // 5 tiles (80)
			player.ZoneShadowCandle = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Spike)
				.AddRecipeGroup(RijamsModRecipes.EvilBars, 3)
				.AddIngredient(ItemID.ShadowCandle, 3)
				.AddTile(TileID.Anvils)
				.AddCondition(Condition.InGraveyard)
				.Register();
		}
	}
}