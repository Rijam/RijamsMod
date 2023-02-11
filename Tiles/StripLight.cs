using RijamsMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using ReLogic.Content;

namespace RijamsMod.Tiles
{
	public class StripLight : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileWaterDeath[Type] = false;
			TileID.Sets.FramesOnKillWall[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.WaterDeath = false;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);

			TileObjectData.newAlternate.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newAlternate.StyleHorizontal = true;
			TileObjectData.newAlternate.WaterDeath = false;
			TileObjectData.newAlternate.LavaDeath = false;
			TileObjectData.newAlternate.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newAlternate.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(1);

			TileObjectData.newAlternate.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newAlternate.StyleHorizontal = true;
			TileObjectData.newAlternate.WaterDeath = false;
			TileObjectData.newAlternate.LavaDeath = false;
			TileObjectData.newAlternate.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newAlternate.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.WoodenBeam };
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(2);

			TileObjectData.newAlternate.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newAlternate.StyleHorizontal = true;
			TileObjectData.newAlternate.WaterDeath = false;
			TileObjectData.newAlternate.LavaDeath = false;
			TileObjectData.newAlternate.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newAlternate.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.WoodenBeam };
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(3);

			/*TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.WaterDeath = false;
			TileObjectData.newAlternate.LavaDeath = false;
			TileObjectData.newAlternate.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newAlternate.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.addAlternate(0);*/
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Strip Light");
			AddMapEntry(new Color(250, 250, 250), name);
			DustType = DustID.WhiteTorch;
			ItemDrop = ModContent.ItemType<Items.Placeable.StripLight>();
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX < 72)
			{
				// Change the color of the lighting based on the paint used!
				Vector3? color = PaintHelper.NormalizeColors(PaintHelper.GetColorFromPaintForLighting(tile.TileColor));
				Vector3 newColor;
				if (color == null)
				{
					newColor = new(1, 1, 1);
				}
				else
				{
					newColor = (Vector3)color;
				}
				if (tile.HasActuator) // Has an Actuator on it (can't activate it because the tile isn't solid)
				{
					newColor *= 0.5f;
				}
				if (tile.IsTileFullbright) // Has Illuminant Coating
				{
					newColor *= 2f;
				}
				/*if (tile.IsTileInvisible) // Has Echo Coating
				{
					newColor *= 0f;
				}*/
				r = newColor.X;
				g = newColor.Y;
				b = newColor.Z;
			}
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int topY = j - tile.TileFrameY / 18 % 3;
			short frameAdjustment = (short)(tile.TileFrameX >= 72 ? -72 : 72);
			Main.tile[i, topY].TileFrameX += frameAdjustment;
			Wiring.SkipWire(i, topY);
			NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
		}

		/*public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 0;
			if (WorldGen.SolidTile(i, j - 1)) {
				offsetY = 2;
				if (WorldGen.SolidTile(i - 1, j + 1) || WorldGen.SolidTile(i + 1, j + 1))
				{
					offsetY = 4;
				}
			}
		}*/
	}
}