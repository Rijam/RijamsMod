using Microsoft.Xna.Framework;
using RijamsMod.Buffs.Debuffs;
using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class SulfuricWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sulfuric Whip");
			// Tooltip.SetDefault("10 summon tag damage\nInflicts Sulfuric Acid\nCan hit enemies through tiles\n{$CommonItemTooltip.Whips}");
			CustomItemIDSets.IsWhip[Type] = true;
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect_SulfuricWhip() { TagDamage = 10 };
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<SulfuricWhipProj>(), 30, 2f, 4f, 30);

			Item.width = 44;
			Item.height = 36;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 40000;
			Item.channel = false;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// This gives some visual variance on how fast the whip swinging animation plays out.
			// This has no effect on the actual collision.
			float swingDirection = 0.6f + (0.4f * Main.rand.NextFloat());
			// 1/3 of the time, swing the whip from the bottom to top instead of from top to bottom.
			// The Dark Harvest is the only whip that doesn't have the chance of swinging from the button up.
			if (Main.rand.NextBool(3))
			{
				swingDirection *= -2.5f;
			}
			// Set swingDirection to 1f for the pre-1.4.5 behavior.

			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, swingDirection);
			return false; // Return false because we've already spawned the projectile.
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 10)
				.AddIngredient(ModContent.ItemType<Items.Materials.Sulfur>(), 20)
				.AddIngredient(ModContent.ItemType<Items.Materials.InfernicFabric>(), 3)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override bool MeleePrefix() => true;
	}

	public class WhipTagEffect_SulfuricWhip : WhipTagEffect
	{
		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, float calcDamage)
		{
			Dust.NewDust(npcHit.Center, npcHit.width, npcHit.height, ModContent.DustType<Dusts.SulfurDust>(), npcHit.direction, npcHit.direction, 150, default, 1f);
			CustomParticleOrchestra.RequestParticleSpawn(clientOnly: true, CustomParticleOrchestraType.AshTreeShake, new ParticleOrchestraSettings
			{
				PositionInWorld = npcHit.Center
			});
			npcHit.AddBuff(ModContent.BuffType<SulfuricAcid>(), 480);
		}
	}
}
