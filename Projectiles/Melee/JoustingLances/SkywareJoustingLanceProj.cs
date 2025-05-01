using Microsoft.Xna.Framework;
using Terraria.ID;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	// I made Example Jousting Lance so I'm going to use it!
	public class SkywareJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.BlueTorch;
			dustTypeRare = DustID.GoldCritter_LessOutline;
			offset = 2;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 70f;
			widthMultiplier = 21f;
			lanceHitboxBounds = new(0, 0, 170, 170);
		}
	}
}