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
	public class SpectreLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 24;
			Item.height = 40;
			Item.shoot = ModContent.ProjectileType<LanternLightSpectre>();
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.Yellow;
			Item.value = 40000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 35;
			Item.knockBack = 2f;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.autoReuse = true;
			Item.mana = 21;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.6f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 14;
				flash.posOffsetXRight = -58;
				flash.posOffsetY = -39;
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
			return new Color(0.5f, 0.9f, 1f, 1f);
		}
		public override float ProjColorFloat()
		{
			return 0.05f;
		}
		public override float ProjSpawnDistance()
		{
			return 2f;
		}
		public override float ProjSpawnVelocity()
		{
			return 6f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			base.Shoot(player, source, position, velocity, type, damage, knockback);

			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
			{
				PositionInWorld = PlayerHandPos + new Vector2(player.width / 4 * player.direction, 0),
				MovementVector = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f))
			});

			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.EchoBlock, 2)
				.AddIngredient(ItemID.SpectreBar, 3)
				.AddIngredient(ItemID.WispinaBottle)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}