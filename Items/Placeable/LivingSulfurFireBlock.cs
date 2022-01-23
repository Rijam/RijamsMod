using RijamsMod.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RijamsMod.Items.Placeable
{
	public class LivingSulfurFireBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Living Sulfur Fire Block");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 20;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.LivingSulfurFireBlock>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LivingFireBlock, 20);
			recipe.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();
		}
	}
}