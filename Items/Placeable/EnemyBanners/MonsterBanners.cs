using RijamsMod.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RijamsMod.Items.Placeable.EnemyBanners
{
	public class DarkSoldierBanner : ModItem
	{
		// The tooltip for this item is automatically assigned from .hjson files
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<MonsterBanners>(), 0); // Place style means which frame (Horizontally, starting from 0) of the tile should be placed
			Item.width = 10;
			Item.height = 24;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 0, 10, 0);
		}
	}
	public class SkeletonCrossbowerBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.placeStyle = 1;
		}
	}
	public class SkeletonGunnerBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.placeStyle = 2;
		}
	}
	public class DungeonBatBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.placeStyle = 3;
		}
	}

	public class SnowmanMuscleBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.placeStyle = 4;
		}
	}
	public class SirSlushBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.placeStyle = 5;
		}
	}

	public class FirmamentHarpyBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.placeStyle = 6;
		}
	}

	public class AngryGusterBanner : DarkSoldierBanner
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.placeStyle = 7;
		}
	}
}
