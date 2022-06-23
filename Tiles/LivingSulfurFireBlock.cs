using RijamsMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using System;

namespace RijamsMod.Tiles
{
	public class LivingSulfurFireBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.placementPreview = false;

			//Placeable in mid air to get around the fact that you can't place tiles on top of tiles if the tile is non-solid (which this tile is)
			//Adapted from Magic Storage StorageConnector.cs
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(CanPlace, -1, 0, true);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.addTile(Type);

			DustType = ModContent.DustType<Dusts.SulfurDust>();
			ItemDrop = ModContent.ItemType<Items.Placeable.LivingSulfurFireBlock>();
			AddMapEntry(new Color(255, 255, 0));
			AnimationFrameHeight = 90;
		}

		//Adapted from Magic Storage StorageConnector.cs
		public int CanPlace(int i, int j, int type, int style, int direction, int alternative) => 0;

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 12;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.75f;
			g = 0.75f;
			b = 0.0f;
		}

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
			frame = Main.tileFrame[TileID.LivingFire];
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = tile.TileFrameY == 36 ? 18 : 16;
			int animate = Main.tileFrame[Type] * AnimationFrameHeight;
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/LivingSulfurFireBlock").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY + animate, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}
}