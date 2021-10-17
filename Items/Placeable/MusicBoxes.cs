using Terraria.ModLoader;
using Terraria.ID;

namespace RijamsMod.Items.Placeable
{
	public class TestMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Outer Storage Warehouse)");
			Tooltip.SetDefault("Freedoom Phase 2 - MAP07 - Outer Storage Warehouse");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.TestMusicBox>();
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.accessory = true;
		}
	}
}
