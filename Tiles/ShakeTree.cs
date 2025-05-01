using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Tiles
{
	public class ShakeTree : ModSystem
	{
		// These arrays keep track of which trees have been shaken.
		// This is a copy of the real arrays because they get mutated first when calling orig.
		// The max size is set to maxTreeShakes, which is just 500.
		// The real arrays don't seem to be reset at the beginning of each day which seems odd to me.
		private static int[] copyTreeShakeX = new int[500];
		private static int[] copyTreeShakeY = new int[500];

		public override void Load()
		{
			Terraria.On_WorldGen.ShakeTree += Detour_WorldGen_ShakeTree;
		}

		public override void Unload()
		{
			copyTreeShakeX = null;
			copyTreeShakeY = null;
		}

		private static readonly FieldInfo Field_WorldGen_numTreeShakes = typeof(Terraria.WorldGen).GetField("numTreeShakes", BindingFlags.NonPublic | BindingFlags.Static);

		internal static int GetSet_WorldGen_numTreeShakes(int? increment = null)
		{
			FieldInfo field = Field_WorldGen_numTreeShakes;
			if (increment != null)
			{
				int oldValue = (int)field.GetValue(null);
				field.SetValue(Main.instance, oldValue + increment);
			}
			return (int)field.GetValue(null);
		}

		private static readonly FieldInfo Field_WorldGen_maxTreeShakes = typeof(Terraria.WorldGen).GetField("maxTreeShakes", BindingFlags.NonPublic | BindingFlags.Static);
		
		internal static int Get_WorldGen_maxTreeShakes()
		{
			return (int)Field_WorldGen_maxTreeShakes.GetValue(Main.instance);
		}

		/*
		private static readonly FieldInfo Field_WorldGen_treeShakeX = typeof(Terraria.WorldGen).GetField("treeShakeX", BindingFlags.NonPublic | BindingFlags.Static);

		internal static int GetSet_WorldGen_treeShakeX(int index, int? newX = null)
		{
			FieldInfo field = Field_WorldGen_treeShakeX;
			Array array = (Array)field.GetValue(null);
			if (newX != null)
			{
				array.SetValue(newX, index);
			}
			return (int)array.GetValue(index);
		}

		private static readonly FieldInfo Field_WorldGen_treeShakeY = typeof(Terraria.WorldGen).GetField("treeShakeY", BindingFlags.NonPublic | BindingFlags.Static);

		internal static int GetSet_WorldGen_treeShakeY(int index, int? newY = null)
		{
			FieldInfo field = Field_WorldGen_treeShakeY;
			Array array = (Array)field.GetValue(null);
			if (newY != null)
			{
				array.SetValue(newY, index);
			}
			return (int)array.GetValue(index);
		}
		*/

		private static void Detour_WorldGen_ShakeTree(On_WorldGen.orig_ShakeTree orig, int i, int j)
		{
			orig(i, j);

			if (GetSet_WorldGen_numTreeShakes() == Get_WorldGen_maxTreeShakes())
			{
				return;
			}

			WorldGen.GetTreeBottom(i, j, out var x, out var y); // Finds the block that the tree is planted on, not the actual bottom of the tree.
			int originalY = y;
			TreeTypes treeType = WorldGen.GetTreeType(Main.tile[x, y].TileType); // Get the tree type based on the tile the tree is planted on.
			int gemTree = -1;
			if (IsTileAGemTree(Main.tile[x, y - 1].TileType)) // Move the y up 1 to find the actual tree tile (not the tile it is placed on).
			{
				gemTree = Main.tile[x, y - 1].TileType;
			}

			// If the tile isn't a tree nor a gem tree, return.
			if (treeType == TreeTypes.None && gemTree == -1)
			{
				return;
			}

			// Check if the tree has already been shook. If so, don't shake it.
			for (int k = 0; k < GetSet_WorldGen_numTreeShakes(); k++)
			{
				// if (GetSet_WorldGen_treeShakeX(index: k) == x && GetSet_WorldGen_treeShakeY(index: k) == y)
				if (copyTreeShakeX[k] == x && copyTreeShakeY[k] == y)
				{
					// Main.NewText($"GetSet_WorldGen_treeShakeX(index: k) {GetSet_WorldGen_treeShakeX(index: k)} copyTreeShakeX[k] {copyTreeShakeX[k]} x {x}");
					// Main.NewText($"GetSet_WorldGen_treeShakeY(index: k) {GetSet_WorldGen_treeShakeY(index: k)} copyTreeShakeY[k] {copyTreeShakeY[k]} y {y}");
					return;
				}
			}

			// GetSet_WorldGen_treeShakeX(index: GetSet_WorldGen_numTreeShakes(), newX: x);
			copyTreeShakeX[GetSet_WorldGen_numTreeShakes()] = x;
			// GetSet_WorldGen_treeShakeY(index: GetSet_WorldGen_numTreeShakes(), newY: y);
			copyTreeShakeY[GetSet_WorldGen_numTreeShakes()] = y;
			GetSet_WorldGen_numTreeShakes(increment: 1);
			y--;
			while (y > 10 && Main.tile[x, y].HasTile && TileID.Sets.IsShakeable[Main.tile[x, y].TileType])
			{
				y--;
			}

			y++;
			if (!WorldGen.IsTileALeafyTreeTop(x, y) || Collision.SolidTiles(x - 2, x + 2, y - 2, y + 2))
			{
				return;
			}

			bool createLeaves = true;
			if (PlantLoader.ShakeTree(x, y, Main.tile[x, originalY].TileType, ref createLeaves))
			{
				// 1 in 600 chance for Dark Soldiers to drop from Ash Trees in For the Worthy worlds.
				if (Main.getGoodWorld && WorldGen.genRand.NextBool(600) && treeType == TreeTypes.Ash)
				{
					NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, ModContent.NPCType<NPCs.Enemies.DarkSoldier>());
				}
				// Gem Trees: 33% chance to get 1-3 gems.
				if (gemTree == TileID.TreeTopaz && WorldGen.genRand.NextBool(3))
				{
					Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Topaz, WorldGen.genRand.Next(1, 4));
				}
				if (gemTree == TileID.TreeAmethyst && WorldGen.genRand.NextBool(3))
				{
					Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Amethyst, WorldGen.genRand.Next(1, 4));
				}
				if (gemTree == TileID.TreeSapphire && WorldGen.genRand.NextBool(3))
				{
					Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Sapphire, WorldGen.genRand.Next(1, 4));
				}
				if (gemTree == TileID.TreeEmerald && WorldGen.genRand.NextBool(3))
				{
					Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Emerald, WorldGen.genRand.Next(1, 4));
				}
				if (gemTree == TileID.TreeRuby && WorldGen.genRand.NextBool(3))
				{
					Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Ruby, WorldGen.genRand.Next(1, 4));
				}
				if (gemTree == TileID.TreeDiamond && WorldGen.genRand.NextBool(3))
				{
					Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Diamond, WorldGen.genRand.Next(1, 4));
				}
				if (gemTree == TileID.TreeAmber && WorldGen.genRand.NextBool(3))
				{
					Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Amber, WorldGen.genRand.Next(1, 4));
				}
				// Any gem tree has a chance of dropping a Geode.
				if (gemTree > -1 && WorldGen.genRand.NextBool(10))
				{
					Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Geode);
				}
			}

			if (!createLeaves)
			{
				return;
			}

			int treeHeight = 0;
			WorldGen.GetTreeLeaf(x, Main.tile[x, y], Main.tile[x, originalY], ref treeHeight, out int _, out int passStyle);

			if (passStyle > 0)
			{
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 1, x, y, 1f, passStyle);
				}

				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					WorldGen.TreeGrowFX(x, y, 1, passStyle, hitTree: true);
				}
			}
		}

		public static bool IsTileAGemTree(int tileType)
		{
			// Main.NewText($"tileType {tileType}");
			return tileType switch
			{
				TileID.TreeTopaz => true,
				TileID.TreeAmethyst => true,
				TileID.TreeSapphire => true,
				TileID.TreeEmerald => true,
				TileID.TreeRuby => true,
				TileID.TreeDiamond => true,
				TileID.TreeAmber => true,
				_ => false,
			};
		}
	}
}
