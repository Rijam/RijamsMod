using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Fishing
{
	public class PlasticWormLure : ModItem
	{
		public override void SetDefaults()
		{

			item.maxStack = 999;
			item.consumable = true;
			item.width = 10;
			item.height = 22;
			item.bait = 5;
			item.value = 50;
			item.rare = ItemRarityID.White;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plastic Worm Lure");
			Tooltip.SetDefault("'Not the best bait'");
		}
	}

	public class Mealworm : ModItem
	{
		public override void SetDefaults()
		{

			item.maxStack = 999;
			item.consumable = true;
			item.width = 14;
			item.height = 10;
			item.bait = 20;
			item.value = 250;
			item.rare = ItemRarityID.Blue;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mealworm");
		}
	}
	public class RedWorm : ModItem
	{
		public override void SetDefaults()
		{

			item.maxStack = 999;
			item.consumable = true;
			item.width = 18;
			item.height = 24;
			item.bait = 32;
			item.value = 500;
			item.rare = ItemRarityID.Green;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Worm");
		}
	}
	public class GlowingMushroomChunk : ModItem
	{
		public override void SetDefaults()
		{

			item.maxStack = 999;
			item.consumable = true;
			item.width = 16;
			item.height = 14;
			item.bait = 66;
			item.value = 1000;
			item.rare = ItemRarityID.LightRed;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowing Mushroom Chunk");
			Tooltip.SetDefault("'Not tasty enough to attract large beasts'");
		}
	}
}