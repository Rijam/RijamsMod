using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace RijamsMod.Tiles
{
	public class RecyclingMachine : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Recylcing Machine");
			AddMapEntry(new Color(171, 87, 0), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Placeable.RecyclingMachine>());
		}

		public override bool NewRightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;

			// Main.mouseItem.type ==
			// player.inventory[58].type ==


			if (player.HeldItem.type == ItemID.OldShoe && player.HasItem(ItemID.OldShoe))
			{
				Main.PlaySound(SoundID.Item, i * 16, j * 16, 65);
				player.ConsumeItem(ItemID.OldShoe);
				if (Main.rand.Next(4) <= 2) //75% chance: 0, 1, 2, but not 3
				{
					player.QuickSpawnItem(ItemID.Silk, Main.rand.Next(1, 3));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after the 75% (25%) chance
				{
					player.QuickSpawnItem(ItemID.Leather, Main.rand.Next(1, 3));
				}
				else
				{
					player.QuickSpawnItem(ItemID.GreenThread, Main.rand.Next(1, 3));
				}
			}
			if (player.HeldItem.type == ItemID.FishingSeaweed && player.HasItem(ItemID.FishingSeaweed))
			{
				Main.PlaySound(SoundID.Item, i * 16, j * 16, 111);
				player.ConsumeItem(ItemID.FishingSeaweed);
				if (Main.rand.Next(4) <= 2) //75% chance
				{
					player.QuickSpawnItem(ItemID.Acorn, Main.rand.Next(1, 2));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after the 75% (25%) chance
				{
					player.QuickSpawnItem(ItemID.DaybloomSeeds, Main.rand.Next(1, 2));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after
				{
					player.QuickSpawnItem(ItemID.BlinkrootSeeds, Main.rand.Next(1, 2));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after
				{
					player.QuickSpawnItem(ItemID.ShiverthornSeeds, Main.rand.Next(1, 2));
				}
				else
				{
					player.QuickSpawnItem(ItemID.WaterleafSeeds, Main.rand.Next(1, 2));
				}
			}
			if (player.HeldItem.type == ItemID.TinCan && player.HasItem(ItemID.TinCan))
			{
				Main.PlaySound(SoundID.Item, i * 16, j * 16, 52);
				player.ConsumeItem(ItemID.TinCan);
				if (Main.rand.Next(2) == 0) //50% chance
				{
					player.QuickSpawnItem(ItemID.TinOre, Main.rand.Next(1, 2));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after the 50% chance
				{
					player.QuickSpawnItem(ItemID.CopperOre, Main.rand.Next(1, 2));
				}
				else
				{
					player.QuickSpawnItem(ItemID.Bottle, Main.rand.Next(1, 2));
				}
			}
			if (player.HeldItem.type == ItemID.Coal && player.HasItem(ItemID.Coal))
			{
				Main.PlaySound(SoundID.Item, i * 16, j * 16, 23);
				player.ConsumeItem(ItemID.Coal);
				if (Main.rand.Next(4) <= 2) //75% chance
				{
					player.QuickSpawnItem(ItemID.StoneBlock, Main.rand.Next(1, 3));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after the 75% (25%) chance
				{
					player.QuickSpawnItem(ItemID.Hook, 1);
				}
				else
				{
					player.QuickSpawnItem(ItemID.Diamond, 1);//Geode in 1.4
				}
			}
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			if (player.HeldItem.type == ItemID.OldShoe && player.HasItem(ItemID.OldShoe)) 
			{
				player.showItemIcon2 = ItemID.OldShoe;
			}
			else if (player.HeldItem.type == ItemID.FishingSeaweed && player.HasItem(ItemID.FishingSeaweed))
			{
				player.showItemIcon2 = ItemID.FishingSeaweed;
			}
			else if (player.HeldItem.type == ItemID.TinCan && player.HasItem(ItemID.TinCan))
			{
				player.showItemIcon2 = ItemID.TinCan;
			}
			else if (player.HeldItem.type == ItemID.Coal && player.HasItem(ItemID.Coal))
			{
				player.showItemIcon2 = ItemID.Coal;
			}
			else
			{
				player.showItemIcon2 = ModContent.ItemType<Items.Placeable.RecyclingMachine>();
			}
		}
	}
}