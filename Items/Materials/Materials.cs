using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Materials
{
	public class GiantRedHarpyFeather: ModItem
	{
		public override void SetStaticDefaults() 
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Harpy or]", "[c/474747:dropped by Firmament Harpies]" } );
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.GiantHarpyFeather; // Shimmer transforms the item.
		}

		public override void SetDefaults() 
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 14;
			Item.height = 34;
			Item.value = 125000;
			Item.rare = ItemRarityID.Yellow;
		}
	}
	public class SunEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Harpy or]", "[c/474747:dropped by Firmament Harpies]" } );
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 20;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
		}
	}
	public class CrawlerChelicera : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by Blood Crawlers]" } );
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.WormTooth; // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 20;
			Item.height = 10;
			Item.value = 25;
			Item.rare = ItemRarityID.White;
		}
	}
	public class InfernicFabric : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Hell Trader]", "[c/474747:After defeating Eye of Cthulhu]" } );
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Silk; // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 24;
			Item.height = 24;
			Item.value = 5000;
			Item.rare = ItemRarityID.Orange;
		}
	}
	public class Sulfur : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Hell Trader]", "[c/474747:In Hardmode]" } );
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 20;
			Item.height = 20;
			Item.value = 7500;
			Item.rare = ItemRarityID.LightRed;
		}
	}
	public class FestivePlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by Santa NK1]" } );
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 22;
			Item.height = 22;
			Item.value = 8000;
			Item.rare = ItemRarityID.Yellow;
		}
	}
}