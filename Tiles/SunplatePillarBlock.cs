using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Tiles
{
	public class SunplatePillarBlock : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileBrick[Type] = true;
			dustType = DustID.Gold;
			drop = ModContent.ItemType<Items.Placeable.SunplatePillarBlock>();
			AddMapEntry(new Color(213, 178, 28));
			soundType = SoundID.Tink;
			soundStyle = 1;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}