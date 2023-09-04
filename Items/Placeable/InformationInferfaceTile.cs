using System.Collections.Generic;
using RijamsMod.Items.Information;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Placeable
{
	public class InformationInterfaceTile : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Interface Anchor");
			// Tooltip.SetDefault("Purely for decoration");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Interstellar Traveler when the]", "[c/474747:Information Interface is in the player's inventory]" });
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<InformationInterface>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = Item.CommonMaxStack;
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