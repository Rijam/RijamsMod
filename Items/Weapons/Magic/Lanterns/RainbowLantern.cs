using System;
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
	public class RainbowLantern : MagicLanternBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 26;
			Item.height = 40;
			Item.shoot = ModContent.ProjectileType<LanternLightRainbow>();
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.Pink;
			Item.value = 35000;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 40;
			Item.knockBack = 2.1f;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.autoReuse = true;
			Item.mana = 22;
			Item.UseSound = SoundID.Item82 with { Pitch = 0.3f };
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 24;
				flash.posOffsetXRight = -70;
				flash.posOffsetY = -46;
				flash.posOffsetYGravity = 53;
				flash.frameCount = 1;
				flash.frameRate = 18;
				flash.colorNoAlpha = Main.DiscoColor;
				flash.alpha = 120;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;
				flash.discoColor = true;

				var glowMask = Item.GetGlobalItem<ItemUseGlow>();
				glowMask.glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow");
				glowMask.flameFlicker = true;
				glowMask.drawColor = new(Math.Clamp(Main.DiscoR, 0, 100), Math.Clamp(Main.DiscoG, 0, 100), Math.Clamp(Main.DiscoB, 0, 100), 0);
				glowMask.discoColor = true;
			}
			
			Item.useLimitPerAnimation = 3; // Added by TML.
		}

		public override Color LightColor()
		{
			return Main.DiscoColor;
		}
		public override float ProjColorFloat()
		{
			return Main.rand.NextFloat(0f, 1f);
		}
		public override float ProjSpawnDistance()
		{
			return 3f;
		}
		public override float ProjSpawnVelocity()
		{
			return 2f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			base.Shoot(player, source, position, velocity, type, damage, knockback);

			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.RainbowRodHit, new ParticleOrchestraSettings
			{
				PositionInWorld = PlayerHandPos + new Vector2(player.width / 4 * player.direction, 0),
				MovementVector = velocity
			});

			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SoulofMight, 2)
				.AddIngredient(ItemID.HallowedBar, 3)
				.AddIngredient(ItemID.RainbowTorch, 3)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}