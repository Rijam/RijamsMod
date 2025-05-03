using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Tiles
{
	[ReinitializeDuringResizeArrays]
	public class CustomTileIDSets
	{
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public const string IsPianoKey = "IsPiano";
		/// <summary> This set is a set of all pianos for the Pianist's Glove. </summary>
		public static bool[] IsPiano = TileID.Sets.Factory.CreateNamedSet(IsPianoKey)
			.Description("This set is a set of all pianos for the Pianist's Glove.")
			.RegisterBoolSet(false, TileID.Pianos);

		/// <summary> This set is a set of all pianos for the Pianist's Glove. </summary>
		// public static List<int> isPiano = new() { TileID.Pianos };
	}
#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning restore IDE0079 // Remove unnecessary suppression
}