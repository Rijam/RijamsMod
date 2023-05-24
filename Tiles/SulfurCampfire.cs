using RijamsMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using ReLogic.Content;
using Terraria.GameContent.ObjectInteractions;
using Terraria.GameContent;
using Terraria.Audio;
using FullSerializer.Internal;

namespace RijamsMod.Tiles
{
	public class SulfurCampfire : ModTile
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
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleWrapLimit = 16;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.WaterDeath = false;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);

			LocalizedText name = CreateMapEntryName();

			AddMapEntry(new Color(250, 250, 0), name);
			DustType = ModContent.DustType<SulfurDust>();

			AnimationFrameHeight = 36;

			// Assets
			if (!Main.dedServ)
			{
				flameTexture = ModContent.Request<Texture2D>("RijamsMod/Tiles/SulfurCampfire_Flame");
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = Main.rand.Next(1, 3);
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			//if (TileFrameY < 36)
			//{
				frame = Main.tileFrame[TileID.Campfire];
			/*}
			else
			{
				frame = 8;
			}*/
		}
		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			if (Main.tile[i, j].TileFrameY < 36)
			{
				frameYOffset = Main.tileFrame[TileID.Campfire] * AnimationFrameHeight;
			}
			else
			{
				frameYOffset = 7 * AnimationFrameHeight;
			}
		}

		public override bool RightClick(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			SoundEngine.PlaySound(SoundID.Mech, new Vector2(i * 16, j * 16));
			Wiring.ToggleCampFire(i, j, tile, null, true);
			/*
			if (Main.LocalPlayer.releaseUseTile)
			{
				SoundEngine.PlaySound(SoundID.Mech, new Vector2(i * 16, j * 16));
				int num13 = Main.tile[i, j].TileFrameX % 54 / 18;
				int num14 = Main.tile[i, j].TileFrameY % 36 / 18;
				int num15 = i - num13;
				int num16 = j - num14;
				int num17 = 36;
				if (Main.tile[num15, num16].TileFrameY >= 36)
					num17 = -36;

				for (int k = num15; k < num15 + 3; k++)
				{
					for (int l = num16; l < num16 + 2; l++)
					{
						Main.tile[k, l].TileFrameY = (short)(Main.tile[k, l].TileFrameY + num17);
					}
				}

				NetMessage.SendTileSquare(-1, num15, num16, 3, 2);
			}
			*/
			return true;
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			Wiring.ToggleCampFire(i, j, tile, null, true);
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameY < 36)
			{
				r = 1.2f;
				g = 1.2f;
				b = 0.3f;
			}
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (Main.tile[i, j].TileFrameY < 36)
			{
				Main.LocalPlayer.AddBuff(BuffID.Campfire, 5, quiet: true);
			}
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Placeable.SulfurCampfire>();
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int width = 18;
			int height = 18;

			int addFrY = 252;
			if (tile.TileFrameY < 36)
			{
				addFrY = Main.tileFrame[Type] * AnimationFrameHeight;
			}
			int tileTop = 2;

			Vector2 screenOffset = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				screenOffset = Vector2.Zero;
			}

			if (tile.TileFrameY < 36)
			{
				Color color = new(255, 255, 255, 200);
				spriteBatch.Draw(flameTexture.Value,
					new Vector2((i * 16 - (int)Main.screenPosition.X) - (width - 16f) / 2f + 1, j * 16 - (int)Main.screenPosition.Y + tileTop) + screenOffset,
					new Rectangle(tile.TileFrameX, tile.TileFrameY + addFrY, width, height),
					color, 0f, default, 1f, SpriteEffects.None, 0f);
			}

			if (tile.TileFrameY < 36 && Main.rand.NextBool(3) && ((Main.drawToScreen && Main.rand.NextBool(4)) || !Main.drawToScreen) && tile.TileFrameY == 0)
			{
				int dustSmoke = Dust.NewDust(new Vector2(i * 16 + 2, j * 16 - 4), 4, 8, DustID.Smoke, 0f, 0f, 100);
				if (tile.TileFrameX == 0)
					Main.dust[dustSmoke].position.X += Main.rand.Next(8);

				if (tile.TileFrameX == 36)
					Main.dust[dustSmoke].position.X -= Main.rand.Next(8);

				Main.dust[dustSmoke].alpha += Main.rand.Next(100);
				Main.dust[dustSmoke].velocity *= 0.2f;
				Main.dust[dustSmoke].velocity.Y -= 0.5f + (float)Main.rand.Next(10) * 0.1f;
				Main.dust[dustSmoke].fadeIn = 0.5f + (float)Main.rand.Next(10) * 0.1f;
			}
		}
	}
}