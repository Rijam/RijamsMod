using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Walls
{
	public class BlueSkywareWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Cobalt;
			VanillaFallbackOnModDeletion = WallID.DiscWall;

			AddMapEntry(new Color(14, 34, 66));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}

	public class RedSkywareWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Adamantite;
			VanillaFallbackOnModDeletion = WallID.DiscWall;

			AddMapEntry(new Color(70, 10, 14));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}

	public class SunplateColumnWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Gold;
			VanillaFallbackOnModDeletion = WallID.DiscWall;

			AddMapEntry(new Color(114, 103, 27));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}