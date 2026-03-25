using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Magic;

namespace RijamsMod.Items.Weapons.Magic.Lanterns
{
	public class UltrabrightLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.shoot = ModContent.ProjectileType<LanternLightUltrabright>();
			Item.shootSpeed = 1;
			Item.rare = ItemRarityID.Blue;
			Item.value = 7000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 25;
			Item.knockBack = 1.3f;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.autoReuse = true;
			Item.mana = 15;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.8f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
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
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow");
				glowMask.flameFlicker = true;
				glowMask.drawColor = new(100, 100, 100, 0);
			}
		}

		public override Color LightColor()
		{
			TorchID.TorchColor(TorchID.UltraBright, out float r, out float g, out float b);
			return new Color(r, g, b, 1f);
		}
		public override float ProjColorFloat()
		{
			return 0.9f;
		}
		public override float ProjSpawnDistance()
		{
			return 3f;
		}
		public override float ProjSpawnVelocity()
		{
			return 4f;
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