using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
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
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Orange; //3
			item.value = Item.sellPrice(0, 3, 50, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions++;
			player.fireWalk = true;
			player.statDefense++;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PygmyNecklace, 1);
			recipe.AddIngredient(ItemID.ObsidianSkull, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
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
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.LightRed; //4
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.accessory = true;
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ObsidianPygmyNecklace>(), 1);
			recipe.AddIngredient(ModContent.ItemType<FrostyRose>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PygmyNecklace, 1);
			recipe.AddIngredient(ModContent.ItemType<ObsidianFrostySkullRose>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class NaughtyList : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Naughty List");
			Tooltip.SetDefault("+2 Minion capacity");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Dropped by Santa-NK1]");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 36;
			item.rare = ItemRarityID.Yellow; //8
			item.value = Item.sellPrice(0, 4, 50, 0);
			item.accessory = true;
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
			item.width = 36;
			item.height = 36;
			item.rare = ItemRarityID.Yellow; //8
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions += 3;
			player.minionDamage += 0.15f;
			player.minionKB += 2f;
			player.fireWalk = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PapyrusScarab, 1);
			recipe.AddIngredient(ModContent.ItemType<NaughtyList>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class SummonersGlove : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Summoner's Glove");
			Tooltip.SetDefault("Makes all summon and sentry weapons autoswing\n[c/FFFF00:Currently a little buggy:]\n[c/FFFF00:Will make all summon and sentry weapons autoswing]\n[c/FFFF00:even after it has been unequipped.]\n[c/FFFF00:Weapons are corrected upon world reload.]");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Sold by Interstellar Traveler]");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 32;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().summonersGlove = true;
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
			item.width = 24;
			item.height = 28;
			item.rare = ItemRarityID.Lime; //7
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions += 4;
			player.maxTurrets += 1;
			player.minionDamage += 0.30f;
			player.statDefense++;
			player.fireWalk = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HailfirePygmyNecklace>(), 1);
			recipe.AddIngredient(ModContent.ItemType<LegionScarf>(), 1);
			recipe.AddRecipeGroup("RijamsMod:Defender's Gear", 1);
			recipe.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Back)]
	public class AssembleAssemble : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assemble Assemble");
			Tooltip.SetDefault("+4 Minion capacity\n+1 Sentry capacity\n+30% Minion damage\n+2 Minion knockback\n+1 Defense\nGrants immunity to fire blocks\nGrants immunity to Frostburn, Frozen, and Chilled\nEnemies are less likely to target you\n'FlavorText BYTE \"It all comes together\", 0'");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.rare = ItemRarityID.Cyan; //9
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions += 4;
			player.maxTurrets += 1;
			player.minionDamage += 0.30f;
			player.minionKB += 2f;
			player.statDefense++;
			player.fireWalk = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
			player.aggro -= 30 * 16; //30 tiles
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NaughtyScarab>(), 1);
			recipe.AddIngredient(ModContent.ItemType<HailfirePygmyScarf>(), 1);
			recipe.AddIngredient(ItemID.PutridScent, 1);
			recipe.AddIngredient(ItemID.LunarBlockStardust, 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}