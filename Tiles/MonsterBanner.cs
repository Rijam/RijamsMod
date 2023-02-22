using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Items.Placeable.EnemyBanners;
using RijamsMod.NPCs.Enemies;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace RijamsMod.Tiles
{
	public class MonsterBanners : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(Type);
			DustType = -1;
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int style = frameX / 18;
			int item;
			switch (style)
			{
				case 0:
					item = ItemType<DarkSoldierBanner>();
					break;
				case 1:
					item = ItemType<SkeletonCrossbowerBanner>();
					break;
				case 2:
					item = ItemType<SkeletonGunnerBanner>();
					break;
				case 3:
					item = ItemType<DungeonBatBanner>();
					break;
				case 4:
					item = ItemType<SnowmanMuscleBanner>();
					break;
				case 5:
					item = ItemType<SirSlushBanner>();
					break;
				case 6:
					item = ItemType<FirmamentHarpyBanner>();
					break;
				default:
					return;
			}
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, item);
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				int style = Main.tile[i, j].TileFrameX / 18;
				int type;
				switch (style) {
					case 0:
						type = NPCType<DarkSoldier>();
						break;
					case 1:
						type = NPCType<SkeletonCrossbower>();
						break;
					case 2:
						type = NPCType<SkeletonGunner>();
						break;
					case 3:
						type = NPCType<DungeonBat>();
						break;
					case 4:
						type = NPCType<SnowmanMuscle>();
						break;
					case 5:
						type = NPCType<SirSlush>();
						break;
					case 6:
						type = NPCType<FirmamentHarpy>();
						break;
					default:
						return;
				}
				Main.SceneMetrics.NPCBannerBuff[type] = true;
				Main.SceneMetrics.hasBanner = true;
			}
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			Tile tile = Main.tile[i, j];
			int topLeftX = i - tile.TileFrameX / 18 % 1;
			int topLeftY = j - tile.TileFrameY / 18 % 3;
			if (WorldGen.IsBelowANonHammeredPlatform(topLeftX, topLeftY))
			{
				offsetY -= 8;
			}
		}
	}
}