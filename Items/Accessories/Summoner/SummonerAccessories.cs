using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Summoner
{
	[AutoloadEquip(EquipType.Neck)]
	public class ObsidianPygmyNecklace : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obsidian Pygmy Necklace");
			Tooltip.SetDefault("+1 Minion capacity\n+1 Defense\nGrants immunity to fire blocks");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Orange; //3
			Item.value = Item.sellPrice(0, 3, 50, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions++;
			player.fireWalk = true;
			player.statDefense++;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PygmyNecklace, 1)
				.AddIngredient(ItemID.ObsidianSkull, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Neck)]
	public class HailfirePygmyNecklace : ObsidianPygmyNecklace
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hailfire Pygmy Necklace");
			Tooltip.SetDefault("+1 Minion capacity\n+1 Defense\nGrants immunity to fire blocks\nGrants immunity to Frostburn, Frozen, and Chilled");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.LightRed; //4
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions++;
			player.fireWalk = true;
			player.statDefense++;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<ObsidianPygmyNecklace>(), 1)
				.AddIngredient(ModContent.ItemType<Defense.FrostyRose>(), 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.PygmyNecklace, 1)
				.AddIngredient(ModContent.ItemType<Defense.ObsidianFrostySkullRose>(), 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	public class NaughtyList : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Naughty List");
			Tooltip.SetDefault("+2 Minion capacity");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Santa-NK1]", null, null });
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.rare = ItemRarityID.Yellow; //8
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions += 2;
		}
	}
	public class NaughtyScarab : NaughtyList
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Naughty Scarab");
			Tooltip.SetDefault("+3 Minion capacity\n+15% Minion damage\n+2 Minion knockback");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.rare = ItemRarityID.Yellow; //8
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions += 3;
			player.GetDamage(DamageClass.Summon) += 0.15f;
			player.GetKnockback(DamageClass.Summon) += 2f;
			player.fireWalk = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PapyrusScarab, 1)
				.AddIngredient(ModContent.ItemType<NaughtyList>(), 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Neck)]
	public class HailfirePygmyScarf : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hailfire Pygmy Scarf");
			Tooltip.SetDefault("+1 Minion capacity\n+1 Sentry capacity\n+15% Minion damage\n+1 Defense\nGrants immunity to fire blocks\nGrants immunity to Frostburn, Frozen, and Chilled\n'Don't question it.'");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.rare = ItemRarityID.Lime; //7
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions += 4;
			player.maxTurrets += 1;
			player.GetDamage(DamageClass.Summon) += 0.30f;
			player.statDefense++;
			player.fireWalk = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<HailfirePygmyNecklace>(), 1)
				.AddIngredient(ModContent.ItemType<Misc.LegionScarf>(), 1)
				.AddRecipeGroup("RijamsMod:Defender's Gear", 1)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Back)]
	public class AssembleAssemble : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assemble Assemble");
			Tooltip.SetDefault("+4 Minion capacity\n+1 Sentry capacity\n+30% Minion damage\n+200% Summon knockback\n+1 Defense\nGrants immunity to fire blocks\nGrants immunity to Frostburn, Frozen, and Chilled\nEnemies are less likely to target you\n'FlavorText BYTE \"It all comes together\", 0'");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.rare = ItemRarityID.Cyan; //9
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions += 4;
			player.maxTurrets += 1;
			player.GetDamage(DamageClass.Summon) += 0.30f;
			player.GetKnockback(DamageClass.Summon) += 2f;
			player.statDefense++;
			player.fireWalk = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
			player.aggro -= 30 * 16; //30 tiles
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<NaughtyScarab>(), 1)
				.AddIngredient(ModContent.ItemType<HailfirePygmyScarf>(), 1)
				.AddIngredient(ItemID.PutridScent, 1)
				.AddIngredient(ItemID.LunarBlockStardust, 5)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.HandsOff)]
	public class ControlGlove : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Control Glove");
			Tooltip.SetDefault("Enables auto swing for all minion and sentry weapons");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating Deerclops]", "[c/474747:Or in Hardmode]" });
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
		}
	}

	[AutoloadEquip(EquipType.HandsOff)]
	public class SummonersGlove : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Summoner's Glove");
			Tooltip.SetDefault("+12% Whip speed\n+10% Whip size\n+100% Summon knockback\nEnables auto swing for all summon weapons");
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
			player.GetKnockback(DamageClass.Summon) += 1f;
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.12f;
			player.whipRangeMultiplier += 0.1f;
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
			DisplayName.SetDefault("Reinforced Summoner's Glove");
			Tooltip.SetDefault("+12% All Summon speed\n+10% Whip size\n+5% whip damage\n+100% Summon knockback\n+4 Defense\nEnables auto swing for all summon weapons");
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
			player.GetKnockback(DamageClass.Summon) += 1f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.12f;
			player.whipRangeMultiplier += 0.1f;
			player.GetDamage(DamageClass.SummonMeleeSpeed) += 0.05f;
			player.statDefense += 4;
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