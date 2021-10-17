using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Materials
{
	public class GiantRedHarpyFeather: ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("\n[c/403638:Sold by Harpy]");
		}

		public override void SetDefaults() 
		{
			item.maxStack = 99;
			item.width = 14;
			item.height = 34;
			item.value = 125000;
			item.rare = ItemRarityID.Yellow;
		}
	}
	public class SunEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("\n[c/403638:Sold by Harpy]");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 20;
			item.height = 20;
			item.value = 7500;
			item.rare = ItemRarityID.Yellow;
		}
	}
}