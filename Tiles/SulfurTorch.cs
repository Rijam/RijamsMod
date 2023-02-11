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
	public class SulfurTorch : ModTile
	{
		private Asset<Texture2D> flameTexture;
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileWaterDeath[Type] = false;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileID.Sets.Torch[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.WoodenBeam };
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.WoodenBeam };
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.addAlternate(0);

			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Torch");
			AddMapEntry(new Color(250, 250, 0), name);
			DustType = ModContent.DustType<SulfurDust>();
			ItemDrop = ModContent.ItemType<Items.Placeable.SulfurTorch>();
			AdjTiles = new int[] { TileID.Torches };
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			// Assets
			if (!Main.dedServ)
			{
				flameTexture = ModContent.Request<Texture2D>("RijamsMod/Tiles/SulfurTorch_Flame");
			}
		}

		public override float GetTorchLuck(Player player)
		{
			bool inUndergroundCaverns = Main.LocalPlayer.ZoneRockLayerHeight || Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneUnderworldHeight;
			return inUndergroundCaverns ? 0.1f : -0.1f; //gives luck when in biome, otherwise a small negative luck
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = Main.rand.Next(1, 3);
		}
		
		public override bool RightClick(int i, int j) // Torch gets destroyed when right clicked.
		{
			WorldGen.KillTile(i, j);
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(new Vector2(i, j), i, j, DustType);
			}
			return true;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX < 66)
			{
				r = 1.2f;
				g = 1.2f;
				b = 0.3f;
			}
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 0;
			if (WorldGen.SolidTile(i, j - 1)) {
				offsetY = 2;
				if (WorldGen.SolidTile(i - 1, j + 1) || WorldGen.SolidTile(i + 1, j + 1))
				{
					offsetY = 4;
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
			Color color = new(100, 100, 100, 0);
			int frameX = Main.tile[i, j].TileFrameX;
			int frameY = Main.tile[i, j].TileFrameY;
			int width = 20;
			int offsetY = 0;
			int height = 20;
			if (WorldGen.SolidTile(i, j - 1))
			{
				offsetY = 2;
				if (WorldGen.SolidTile(i - 1, j + 1) || WorldGen.SolidTile(i + 1, j + 1))
				{
					offsetY = 4;
				}
			}
			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 7; k++)
			{
				float x = (float)Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
				float y = (float)Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(flameTexture.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) - (width - 16f) / 2f + x, (float)(j * 16 - (int)Main.screenPosition.Y + offsetY) + y) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default, 1f, SpriteEffects.None, 0f);
			}
		}
	}
}