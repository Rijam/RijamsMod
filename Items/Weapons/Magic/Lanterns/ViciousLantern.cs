using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Items.Materials;
using RijamsMod.Projectiles.Magic;

namespace RijamsMod.Items.Weapons.Magic.Lanterns
{
	public class ViciousLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 26;
			Item.height = 40;
			Item.shoot = ModContent.ProjectileType<LanternLightVicious>();
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 30000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 32;
			Item.knockBack = 2f;
			Item.useTime = 27;
			Item.useAnimation = 27;
			Item.autoReuse = true;
			Item.mana = 21;
			Item.UseSound = SoundID.Item82 with { Pitch = -0.3f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 14;
				flash.posOffsetXRight = -59;
				flash.posOffsetY = -34;
				flash.posOffsetYGravity = 53;
				flash.frameCount = 1;
				flash.frameRate = 18;
				flash.alpha = 120;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;

				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow");
				glowMask.flameFlicker = true;
				glowMask.drawColor = new(100, 100, 100, 0);
			}

			Item.useLimitPerAnimation = 3; // Added by TML.
		}

		public override Color LightColor()
		{
			TorchID.TorchColor(TorchID.Ichor, out float r, out float g, out float b);
			return new Color(r, g, b, 1f);
		}
		public override float ProjColorFloat()
		{
			return 0.65f;
		}
		public override float ProjSpawnDistance()
		{
			return 2f;
		}
		public override float ProjSpawnVelocity()
		{
			return 4f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<CrawlerChelicera>(), 2)
				.AddIngredient(ItemID.CrimtaneBar, 3)
				.AddIngredient(ItemID.LivingIchorBlock, 3)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}