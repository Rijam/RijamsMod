using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	[AutoloadEquip(EquipType.Face)]
	public class FrostyRose : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frosty Rose");
			Tooltip.SetDefault("Grants immunity to Frostburn, Frozen, and Chilled");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Dropped by The Frost Legion]");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 38;
			item.rare = ItemRarityID.LightRed; //4
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[BuffID.Frostburn] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.buffImmune[BuffID.Chilled] = true;
		}
	}
	[AutoloadEquip(EquipType.Face)]
	public class ObsidianFrostySkullRose : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obsidian Frosty Skull Rose");
			Tooltip.SetDefault("+1 Defense\nGrants immunity to fire blocks\nGrants immunity to Frostburn, Frozen, and Chilled");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 32;
			item.rare = ItemRarityID.LightRed; //4
			item.value = Item.sellPrice(0, 4, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[BuffID.Frostburn] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.fireWalk = true;
			player.statDefense++;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
			drawHair = false;
			drawAltHair = false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ObsidianSkull, 1);
			recipe.AddIngredient(ModContent.ItemType<FrostyRose>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Face)]
	public class GelidSkullRose : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gelid Skull Rose");
			Tooltip.SetDefault("+1 Defense\nMelee attacks inflict Frostburn\nGrants immunity to fire blocks\nGrants immunity to Frostburn, Frozen, and Chilled");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 34;
			item.rare = ItemRarityID.LightPurple; //6
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().frostburnStone = true;
			player.buffImmune[BuffID.Frostburn] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.fireWalk = true;
			player.statDefense++;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = false;
			drawAltHair = false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ObsidianFrostySkullRose>(), 1);
			recipe.AddIngredient(ItemID.MagmaStone, 1);
			recipe.AddIngredient(ItemID.IceBlock, 20);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}