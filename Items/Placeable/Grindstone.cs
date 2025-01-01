using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Placeable
{
	public class Grindstone : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Sold by Goblin Tinkerer]"]);
		}
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 28;
			Item.height = 32;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<Tiles.Grindstone>();
		}
	}
}