using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Placeable
{
	public class SulfurCampfire : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Can be placed in water");
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.ShimmerCampfire; // Shimmer transforms the item.
		}
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.SulfurCampfire>();
			Item.width = 12;
			Item.height = 12;
			Item.value = 5;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(RecipeGroups.Wood, 10)
				.AddIngredient(ModContent.ItemType<SulfurTorch>(), 5)
				.Register();
		}
	}
}