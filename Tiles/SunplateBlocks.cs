using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Tiles
{
	public class SunplatePillarBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileBrick[Type] = true;
			DustType = DustID.Gold;
			AddMapEntry(new Color(228, 190, 30)); // Vanilla (213, 178, 28)
			HitSound = SoundID.Tink;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}

	public class RedSunplateBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileBrick[Type] = true;
			DustType = DustID.Gold;
			AddMapEntry(new Color(255, 109, 28));
			HitSound = SoundID.Tink;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}

	public class BlueSkywareBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileBrick[Type] = true;
			DustType = DustID.Cobalt;
			AddMapEntry(new Color(9, 58, 135));
			HitSound = SoundID.Tink;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}

	public class RedSkywareBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileBrick[Type] = true;
			DustType = DustID.Adamantite;
			AddMapEntry(new Color(146, 0, 10));
			HitSound = SoundID.Tink;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}

	public class SunplateColumnBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileBrick[Type] = true;
			DustType = DustID.Gold;
			AddMapEntry(new Color(255, 224, 28));
			HitSound = SoundID.Tink;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}

	public class SunplateBeamBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.IsBeam[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = true;
			DustType = DustID.Gold;
			AddMapEntry(new Color(255, 180, 28));
			HitSound = SoundID.Tink;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}