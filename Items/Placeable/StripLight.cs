using RijamsMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Placeable
{
	public class StripLight : ModItem
	{
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Paints will affect the color of the light\nDeep paints are brighter");
		}
        public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 6;
			Item.maxStack = 999;
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
	}
}