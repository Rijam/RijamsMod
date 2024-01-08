using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RijamsMod.Items.Placeable
{
	public class LivingSulfurFireBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Living Sulfur Fire Block");
			ItemID.Sets.IsLavaImmuneRegardlessOfRarity[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LivingSulfurFireBlock>());
			Item.width = 18;
			Item.height = 20;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, 0.75f, 0.75f, 0f);
		}
		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.LivingFireBlock, 20)
				.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 1)
				.AddTile(TileID.CrystalBall)
				.SortAfterFirstRecipesOf(ItemID.LivingUltrabrightFireBlock) // places the recipe right after vanilla fire block recipes
				.Register();
		}
	}
}