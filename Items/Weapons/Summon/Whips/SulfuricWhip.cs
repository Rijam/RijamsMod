using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Buffs.Debuffs;
using RijamsMod.Projectiles.Summon.Whips;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class SulfuricWhip : WhipItemBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Sulfuric Whip");
			// Tooltip.SetDefault("10 summon tag damage\nInflicts Sulfuric Acid\nCan hit enemies through tiles\n{$CommonItemTooltip.Whips}");
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect_SulfuricWhip() { TagDamage = 10 };
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<SulfuricWhipProj>(), 30, 2f, 4f, 30);

			Item.rare = ItemRarityID.LightRed;
			Item.value = 40000;
			Item.channel = false;
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
	}

	public class WhipTagEffect_SulfuricWhip : WhipTagEffect
	{
		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, NPC.HitInfo hit)
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
