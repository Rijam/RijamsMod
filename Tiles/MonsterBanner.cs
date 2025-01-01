using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.NPCs.Enemies;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace RijamsMod.Tiles
{
	public class MonsterBanners : ModTile
	{
		public override void SetStaticDefaults()
		{
			// This allows the tile to sway in the wind and from player interaction when used together with the code in PreDraw below.
			TileID.Sets.MultiTileSway[Type] = true;
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

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				int style = Main.tile[i, j].TileFrameX / 18;
				int type = style switch
				{
					0 => ModContent.NPCType<DarkSoldier>(),
					1 => ModContent.NPCType<SkeletonCrossbower>(),
					2 => ModContent.NPCType<SkeletonGunner>(),
					3 => ModContent.NPCType<DungeonBat>(),
					4 => ModContent.NPCType<SnowmanMuscle>(),
					5 => ModContent.NPCType<SirSlush>(),
					6 => ModContent.NPCType<FirmamentHarpy>(),
					7 => ModContent.NPCType<AngryGuster>(),
					_ => ItemID.None
				};

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

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];

			if (TileObjectData.IsTopLeft(tile))
			{
				// Makes this tile sway in the wind and with player interaction when used with TileID.Sets.MultiTileSway
				Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileDrawing.TileCounterType.MultiTileVine);
			}

			// We must return false here to prevent the normal tile drawing code from drawing the default static tile. Without this a duplicate tile will be drawn.
			return false;
		}
	}
}