using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Magic;

namespace RijamsMod.Items.Weapons.Magic.Lanterns
{
	public class FrostburnLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.shoot = ModContent.ProjectileType<LanternLightFrostburn>();
			Item.shootSpeed = 4;
			Item.rare = ItemRarityID.Blue;
			Item.value = 5000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 20;
			Item.knockBack = 1.25f;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.autoReuse = true;
			Item.mana = 12;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.9f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 10;
				flash.posOffsetXRight = -48;
				flash.posOffsetY = -35;
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
			TorchID.TorchColor(TorchID.Ice, out float r, out float g, out float b);
			return new Color(r, g, b, 1f);
		}
		public override float ProjColorFloat()
		{
			return 0.05f;
		}
		public override float ProjSpawnDistance()
		{
			return 3f;
		}
		public override float ProjSpawnVelocity()
		{
			return 3f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Chain)
				.AddRecipeGroup(RijamsModRecipes.SilverBars, 3)
				.AddIngredient(ItemID.IceTorch, 3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}