using Microsoft.Xna.Framework;
using Terraria.ID;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	// I made Example Jousting Lance so I'm going to use it!
	public class RedSkywareJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.RedTorch;
			dustTypeRare = DustID.GoldCritter_LessOutline;
			offset = 4;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 135f;
			widthMultiplier = 24f;
			lanceHitboxBounds = new(0, 0, 310, 310);
		}
	}
}