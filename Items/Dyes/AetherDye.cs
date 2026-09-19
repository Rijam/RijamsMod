using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Creative;

namespace RijamsMod.Items.Dyes
{
	public class AetherDye : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Avoid loading assets on dedicated servers. They don't use graphics cards.
			if (!Main.dedServ)
			{
				// The following code creates an effect (shader) reference and associates it with this item's type Id.
				GameShaders.Armor.BindShader
				(
					Item.type,
					new ArmorShaderData(Main.PixelShaderRef, "RainbowTownSlime") // Vanilla shader
				);
			}
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}
		public override void SetDefaults()
		{
			int dye = Item.dye;
			Item.CloneDefaults(ItemID.ShiftingSandsDye);
			Item.dye = dye;
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater)
				.AddIngredient(ItemID.ShimmerBlock, 5)
				.AddTile(TileID.DyeVat)
				.Register();
		}
	}
}