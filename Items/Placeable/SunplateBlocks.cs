using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Placeable
{
	public class SunplatePillarBlock : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SunplatePillarBlock>());
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SunplateBlock, 1)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<RedSunplateBlock>(), 1)
				.Register();
		}
	}

	public class RedSunplateBlock : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.RedSunplateBlock>());
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.StoneBlock, 25)
				.AddIngredient(ItemID.FallenStar, 1)
				.AddTile(TileID.SkyMill)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SunplatePillarBlock>(), 1)
				.Register();
		}
	}

	public class BlueSkywareBlock : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.BlueSkywareBlock>());
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.StoneBlock, 25)
				.AddIngredient(ItemID.Feather, 1)
				.AddTile(TileID.SkyMill)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<RedSkywareBlock>())
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<BlueSkywareWall>(), 4)
				.Register();
		}
	}
	public class RedSkywareBlock : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.RedSkywareBlock>());
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.StoneBlock, 25)
				.AddIngredient(ItemID.Feather, 1)
				.AddTile(TileID.SkyMill)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<BlueSkywareBlock>())
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<RedSkywareWall>(), 4)
				.Register();
		}
	}

	public class SunplateColumnBlock : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SunplateColumnBlock>());
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.StoneBlock, 25)
				.AddIngredient(ItemID.FallenStar, 1)
				.AddTile(TileID.SkyMill)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SunplateColumnWall>(), 4)
				.Register();
		}
	}

	public class SunplateBeamBlock : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SunplateBeamBlock>());
		}
		public override void AddRecipes()
		{
			CreateRecipe(2)
				.AddIngredient(ModContent.ItemType<SunplatePillarBlock>())
				.AddTile(TileID.SkyMill)
				.Register();
		}
	}

	public class BlueSkywareWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Walls.BlueSkywareWall>());
		}
		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<BlueSkywareBlock>())
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class RedSkywareWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Walls.RedSkywareWall>());
		}
		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<RedSkywareBlock>())
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class SunplateColumnWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Walls.SunplateColumnWall>());
		}
		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<SunplateColumnBlock>())
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
