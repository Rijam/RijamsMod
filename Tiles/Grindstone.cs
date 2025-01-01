using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace RijamsMod.Tiles
{
	public class Grindstone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = [16, 18];
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(68, 61, 61), CreateMapEntryName());
			DustType = DustID.Stone;

			AnimationFrameHeight = 38;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter >= 3)
			{
				frameCounter = 0;
				frame++;
				if (frame > 3)
				{
					frame = 0;
				}
			}
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if ((player.HeldItem.prefix > 0 || Main.mouseItem.prefix > 0) && !player.HeldItem.favorited)
			{
				SoundEngine.PlaySound(SoundID.Item55 with { Volume = 2f, PitchRange = (-1f, -0.5f)}, new Vector2(i * 16, j * 16));
				SoundEngine.PlaySound(SoundID.Item55 with { Volume = 2f, PitchRange = (-1f, -0.5f)}, new Vector2(i * 16, j * 16));
				SoundEngine.PlaySound(SoundID.Item55 with { Volume = 2f, PitchRange = (-1f, -0.5f)}, new Vector2(i * 16, j * 16));
				player.HeldItem.ResetPrefix();
				if (Main.mouseItem.type > ItemID.None)
				{
					Main.mouseItem.ResetPrefix();
				}
			}
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			if (player.HeldItem.prefix > 0 && !player.HeldItem.favorited)
			{
				player.cursorItemIconID = player.HeldItem.type;
			}
			else
			{
				player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Grindstone>();
			}
		}
	}
}