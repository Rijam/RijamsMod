using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Placeable
{
	public class SunplatePillarBlock : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.SunplatePillarBlock>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SunplateBlock, 1)
				.Register();
		}
	}
}
