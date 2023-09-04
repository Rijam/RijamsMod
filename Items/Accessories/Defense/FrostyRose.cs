using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Defense
{
	[AutoloadEquip(EquipType.Face)]
	public class FrostyRose : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Frosty Rose");
			// Tooltip.SetDefault("Grants immunity to Frostburn, Frozen, and Chilled");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by The Frost Legion]" });
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 38;
			Item.rare = ItemRarityID.LightRed; //4
			Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
		}
	}

	[AutoloadEquip(EquipType.Face)]
	public class ObsidianFrostySkullRose : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Obsidian Frosty Skull Rose");
			// Tooltip.SetDefault("+1 Defense\nGrants immunity to fire blocks\nGrants immunity to Frostburn, Frozen, and Chilled");
			ArmorIDs.Face.Sets.PreventHairDraw[Item.faceSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.rare = ItemRarityID.LightRed; //4
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
			player.fireWalk = true;
			player.statDefense++;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.ObsidianSkull, 1)
				.AddIngredient(ModContent.ItemType<FrostyRose>(), 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Face)]
	public class GelidSkullRose : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gelid Skull Rose");
			// Tooltip.SetDefault("+1 Defense\nMelee attacks inflict Frostburn\nGrants immunity to fire blocks\nGrants immunity to Frostburn, Frozen, and Chilled");
			ArmorIDs.Face.Sets.PreventHairDraw[Item.faceSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = ItemRarityID.LightPurple; //6
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().frostburnStone = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
			player.fireWalk = true;
			player.statDefense++;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<ObsidianFrostySkullRose>(), 1)
				.AddIngredient(ItemID.MagmaStone, 1)
				.AddIngredient(ItemID.LivingFrostFireBlock, 20)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Face)]
	public class RoseOfFireAndIce : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rose of Fire and Ice");
			// Tooltip.SetDefault("Reduces damage from touching lava\nGrants immunity to Frostburn, Frozen, and Chilled");
		}

		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 44;
			Item.rare = ItemRarityID.LightRed; //4
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.lavaRose = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<FrostyRose>(), 1)
				.AddIngredient(ItemID.ObsidianRose, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}