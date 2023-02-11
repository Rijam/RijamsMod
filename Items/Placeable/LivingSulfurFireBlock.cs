using RijamsMod.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.PlayerDrawLayer;

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
			Item.width = 18;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.LivingSulfurFireBlock>();
		}
		public override void PostUpdate()
		{
			Lighting.AddLight((int)((Item.position.X + (Item.width / 2)) / 16f), (int)((Item.position.Y + (Item.height / 2)) / 16f), 0.75f, 0.75f, 0f);
		}
		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.LivingFireBlock, 20)
				.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 1)
				.AddTile(TileID.CrystalBall)
				.Register();
		}
	}
}