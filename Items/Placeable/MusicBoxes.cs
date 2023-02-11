using Terraria.ModLoader;
using Terraria.ID;
using RijamsMod.Items.Information;

namespace RijamsMod.Items.Placeable
{
	public class MusicBoxOSW : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Music Box (Outer Storage Warehouse)");
			// Tooltip.SetDefault("Freedoom Phase 2 - MAP07 - Outer Storage Warehouse");
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/FreedoomPhase2_MAP07_OuterStorageWarehouse"), Item.type, ModContent.TileType<Tiles.MusicBoxOSW>());
			ItemID.Sets.CanGetPrefixes[Type] = false; // music boxes can't get prefixes in vanilla
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox; // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.MusicBoxOSW>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}
	}
}
