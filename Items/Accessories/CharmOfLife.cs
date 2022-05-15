using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class CharmOfLife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charm of Life");
			Tooltip.SetDefault("\n+25 Max Life\nProvides life regeneration\nReduces the cooldown of healing potions by 25%");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(0, 5, 50, 0);
			item.lifeRegen = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.pStone = true;
			player.statLifeMax2 += 25;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CharmofMyths, 1);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.LifeFruit, 1);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}