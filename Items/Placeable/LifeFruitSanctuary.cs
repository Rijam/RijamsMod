using RijamsMod.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RijamsMod.Items.Placeable
{
	public class LifeFruitSanctuary : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Life Fruit Sanctuary");
			Tooltip.SetDefault("Increases life by 20 when placed nearby");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 29;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.createTile = ModContent.TileType<Tiles.LifeFruitSanctuary>();
			//item.placeStyle = 0;        //Place style means which frame(Horizontally, starting from 0) of the tile should be placed
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.LifeFruit, 4)
				.AddIngredient(ItemID.RichMahogany, 5)
				.AddIngredient(ItemID.JungleSpores, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}