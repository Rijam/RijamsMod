using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Summoner.Whip
{
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
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.accWickedArmlet = true;
			player.GetModPlayer<RijamsModPlayer>().Accessory_SummonersGlove = true;
			player.GetKnockback(DamageClass.Summon).Base += 2f;
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.12f;
			player.whipRangeMultiplier += 0.1f;
			player.GetModPlayer<RijamsModPlayer>().Stat_BonusWhipTagDamage += 5;
			player.GetModPlayer<RijamsModPlayer>().Stat_BonusWhipTagCritChance += 10;
			//player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionRadiusIncrease += 4;
			//player.GetArmorPenetration(DamageClass.Summon) += 4;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.WickedArmlet)
				.AddIngredient(ModContent.ItemType<VirtuousArmlet>())
				.AddIngredient(ItemID.PowerGlove)
				.AddIngredient(ItemID.HallowedBar)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.WickedClaws)
				.AddIngredient(ModContent.ItemType<VirtuousArmlet>())
				.AddIngredient(ItemID.TitanGlove)
				.AddIngredient(ItemID.HallowedBar)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<HarmonyArmlets>())
				.AddIngredient(ItemID.PowerGlove)
				.AddIngredient(ItemID.HallowedBar)
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
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 8, 0, 0);
			Item.accessory = true;
			Item.defense = 4;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.accWickedArmlet = true;
			player.GetModPlayer<RijamsModPlayer>().Accessory_SummonersGlove = true;
			player.GetKnockback(DamageClass.Summon).Base += 2f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.12f;
			player.whipRangeMultiplier += 0.1f;
			player.GetDamage(DamageClass.SummonMeleeSpeed) += 0.05f;
			//player.statDefense += 4;
			player.GetModPlayer<RijamsModPlayer>().Stat_BonusWhipTagDamage += 5;
			player.GetModPlayer<RijamsModPlayer>().Stat_BonusWhipTagCritChance += 10;
			//player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionRadiusIncrease += 4;
			//player.GetArmorPenetration(DamageClass.Summon) += 4;
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

	[AutoloadEquip(EquipType.HandsOff)]
	public class VirtuousArmlet : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Dropped by Enchanted Swords]"]);
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.WickedArmlet;
			ItemID.Sets.ShimmerTransformToItem[ItemID.WickedArmlet] = Type;
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().Stat_BonusWhipTagDamage += 5;
			player.GetModPlayer<RijamsModPlayer>().Stat_BonusWhipTagCritChance += 10;
		}
	}

	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class HarmonyArmlets : ModItem
	{
		public override void SetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 2, 10, 0);
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().Stat_BonusWhipTagDamage += 5;
			player.GetModPlayer<RijamsModPlayer>().Stat_BonusWhipTagCritChance += 10;
			player.accWickedArmlet = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.WickedArmlet)
				.AddIngredient(ModContent.ItemType<VirtuousArmlet>())
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
