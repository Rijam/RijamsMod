using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Placeable
{
	public class StripLight : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Paints will affect the color of the light\n Deep paints are brighter\n Actuators halve the light\n Illuminant Coating doubles the light");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Goblin Tinkerer]", "[c/474747:when the Interstellar Traveler is present]" });
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 6;
			Item.maxStack = Item.CommonMaxStack;
			Item.noWet = false;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.StripLight>();
			Item.value = 100;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.White.ToVector3());
		}
	}
}