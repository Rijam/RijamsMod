using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Items.Materials;
using RijamsMod.Items.Placeable;
using RijamsMod.Projectiles.Magic;

namespace RijamsMod.Items.Weapons.Magic.Lanterns
{
	public class SulfuricLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 18;
			Item.height = 40;
			Item.shoot = ModContent.ProjectileType<LanternLightSulfuric>();
			Item.shootSpeed = 1;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 20000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 30;
			Item.knockBack = 2f;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.autoReuse = true;
			Item.mana = 20;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.5f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 12;
				flash.posOffsetXRight = -50;
				flash.posOffsetY = -36;
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
			return new Color(1f, 1f, 0.1f, 1f);
		}
		public override float ProjColorFloat()
		{
			return 0.575f;
		}
		public override float ProjSpawnDistance()
		{
			return 2f;
		}
		public override float ProjSpawnVelocity()
		{
			return 3f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<InfernicFabric>(), 2)
				.AddIngredient(ItemID.HellstoneBar, 3)
				.AddIngredient(ModContent.ItemType<LivingSulfurFireBlock>(), 3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}