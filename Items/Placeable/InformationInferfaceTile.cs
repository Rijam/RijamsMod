using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Placeable
{
	public class InformationInterfaceTile : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Interface Anchor");
			Tooltip.SetDefault("Purely for decoration");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:when Information Interface]", "[c/474747:is in the player's inventory]" });
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<Tiles.InformationInterfaceTile>();
		}
	}
}