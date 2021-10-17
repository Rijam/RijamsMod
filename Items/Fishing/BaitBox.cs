using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Fishing
{
	public class BaitBox : ModItem
	{
		public override void SetDefaults()
		{

			item.maxStack = 999;
			item.consumable = true;
			item.width = 24;
			item.height = 24;
			item.value = 50000;
			item.rare = ItemRarityID.Orange; //Orange
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bait Box");
			Tooltip.SetDefault("Contains a bunch of bait!\nRight click to open");
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public virtual void CrateLoot(Player player)
		{

			int random;
			for (int i = 0; i < 10; i++) //roll 10 times
			{
				random = Main.rand.Next(9);
				if (random == 8)
				{
					player.QuickSpawnItem(mod.ItemType("RedWorm"), Main.rand.Next(0, 3));
				}
				if (random == 7)
				{
					player.QuickSpawnItem(mod.ItemType("Mealworm"), Main.rand.Next(0, 3));
				}
				if (random == 6)
                {
					player.QuickSpawnItem(mod.ItemType("PlasticWormLure"), Main.rand.Next(0, 3));
				}
				if (random == 5)
				{
					player.QuickSpawnItem(ItemID.Snail, Main.rand.Next(0, 3));
				}
				if (random == 4)
				{
					player.QuickSpawnItem(ItemID.EnchantedNightcrawler, Main.rand.Next(0, 3));
				}
				if (random == 3)
				{
					player.QuickSpawnItem(ItemID.Worm, Main.rand.Next(0, 3));
				}
				if (random == 2)
				{
					player.QuickSpawnItem(ItemID.MasterBait, Main.rand.Next(0, 3));
				}
				if (random == 1)
				{
					player.QuickSpawnItem(ItemID.JourneymanBait, Main.rand.Next(0, 3));
				}
				if (random == 0)
				{
					player.QuickSpawnItem(ItemID.ApprenticeBait, Main.rand.Next(0, 3));
				}
			}
		}
		public override void RightClick(Player player)
		{
			CrateLoot(player);
		}
	}
}