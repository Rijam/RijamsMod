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
	public class JackOLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 24;
			Item.height = 40;
			Item.shoot = ModContent.ProjectileType<LanternLightJackO>();
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.Yellow;
			Item.value = 40000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 45;
			Item.knockBack = 2.3f;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.autoReuse = true;
			Item.mana = 22;
			Item.UseSound = SoundID.Item82 with { Pitch = -0.4f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 9;
				flash.posOffsetXRight = -50;
				flash.posOffsetY = -44;
				flash.posOffsetYGravity = 53;
				flash.frameCount = 1;
				flash.frameRate = 18;
				flash.alpha = 120;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;

				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow");
				glowMask.flameFlicker = true;
				glowMask.drawColor = new(50, 50, 50, 0);
			}
			
			Item.useLimitPerAnimation = 3; // Added by TML.
		}

		public override Color LightColor()
		{
			return new Color(1f, 0.72f, 0.47f, 1f);
		}
		public override float ProjColorFloat()
		{
			return 0.525f;
		}
		public override float ProjSpawnDistance()
		{
			return 3f;
		}
		public override float ProjSpawnVelocity()
		{
			return 3f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			base.Shoot(player, source, position, velocity, type, damage, knockback);

			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningSmall, new ParticleOrchestraSettings
			{
				PositionInWorld = PlayerHandPos + new Vector2(player.width / 4 * player.direction, 0),
				MovementVector = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f))
			});

			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.ExplosiveJackOLantern, 2)
				.AddIngredient(ItemID.SpookyWood, 3)
				.AddIngredient(ItemID.JackOLantern)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}