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
			item.width = 30;
			item.height = 29;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.createTile = ModContent.TileType<Tiles.LifeFruitSanctuary>();
			//item.placeStyle = 0;        //Place style means which frame(Horizontally, starting from 0) of the tile should be placed
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeFruit, 4);
			recipe.AddIngredient(ItemID.RichMahogany, 5);
			recipe.AddIngredient(ItemID.JungleSpores, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}