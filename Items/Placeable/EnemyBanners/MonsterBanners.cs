using RijamsMod.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RijamsMod.Items.Placeable.EnemyBanners
{
	public class DarkSoldierBanner : ModItem
	{
		// The tooltip for this item is automatically assigned from .lang files
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<MonsterBanners>();
			Item.placeStyle = 0;        //Place style means which frame(Horizontally, starting from 0) of the tile should be placed
		}
	}
	public class SkeletonCrossbowerBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<MonsterBanners>();
			Item.placeStyle = 1;
		}
	}
	public class SkeletonGunnerBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<MonsterBanners>();
			Item.placeStyle = 2;
		}
	}
	public class DungeonBatBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<MonsterBanners>();
			Item.placeStyle = 3;
		}
	}

	public class SnowmanMuscleBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<MonsterBanners>();
			Item.placeStyle = 4;
		}
	}
	public class SirSlushBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<MonsterBanners>();
			Item.placeStyle = 5;
		}
	}

	public class FirmamentHarpyBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<MonsterBanners>();
			Item.placeStyle = 6;
		}
	}
}
