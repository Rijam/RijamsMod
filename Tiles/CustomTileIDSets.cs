using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Tiles
{
	[ReinitializeDuringResizeArrays]
	public class CustomTileIDSets
	{
		public const string IsPianoKey = "IsPiano";
		/// <summary> This set is a set of all pianos for the Pianist's Glove. </summary>
		public static bool[] IsPiano = TileID.Sets.Factory.CreateNamedSet(IsPianoKey)
			.Description("This set is a set of all pianos for the Pianist's Glove.")
			.RegisterBoolSet(false, TileID.Pianos);

		/// <summary> This set is a set of all pianos for the Pianist's Glove. </summary>
		// public static List<int> isPiano = new() { TileID.Pianos };
	}
}