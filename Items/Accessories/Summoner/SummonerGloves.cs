using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Summoner
{
	[AutoloadEquip(EquipType.HandsOff)]
	public class ControlGlove : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Control Glove");
			// Tooltip.SetDefault("Enables auto swing for all minion and sentry weapons");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating Deerclops]" });
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 32;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().controlGlove = true;
			player.GetModPlayer<RijamsModPlayer>().supportMinionRadiusIncrease += 2;
		}
	}

	[AutoloadEquip(EquipType.HandsOff)]
	public class SummonersGlove : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Summoner's Glove");
			// Tooltip.SetDefault("+12% Whip speed\n+10% Whip size\n+1 Summon knockback\nEnables auto swing for all summon weapons");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 36;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().summonersGlove = true;
			player.GetKnockback(DamageClass.Summon).Base += 1f;
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.12f;
			player.whipRangeMultiplier += 0.1f;
			player.GetModPlayer<RijamsModPlayer>().supportMinionRadiusIncrease += 2;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<ControlGlove>(), 1)
				.AddIngredient(ItemID.PowerGlove, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.HandsOff)]
	public class ReinforcedSummonersGlove : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Reinforced Summoner's Glove");
			// Tooltip.SetDefault("+12% All Summon speed\n+10% Whip size\n+5% Whip damage\n+1 Summon knockback\n+4 Defense\nEnables auto swing for all summon weapons");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 36;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 8, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().summonersGlove = true;
			player.GetKnockback(DamageClass.Summon).Base += 1f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.12f;
			player.whipRangeMultiplier += 0.1f;
			player.GetDamage(DamageClass.SummonMeleeSpeed) += 0.05f;
			player.statDefense += 4;
			player.GetModPlayer<RijamsModPlayer>().supportMinionRadiusIncrease += 2;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SummonersGlove>(), 1)
				.AddIngredient(ModContent.ItemType<Materials.FestivePlating>(), 15)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}