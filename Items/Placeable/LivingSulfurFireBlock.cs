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
			Item.width = 18;
			Item.height = 20;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.LivingSulfurFireBlock>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.LivingFireBlock, 20)
				.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 1)
				.AddTile(TileID.CrystalBall)
				.Register();
		}
	}
}