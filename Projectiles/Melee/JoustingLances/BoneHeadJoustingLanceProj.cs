using Microsoft.Xna.Framework;
using Terraria.ID;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	// I made Example Jousting Lance so I'm going to use it!
	public class BoneHeadJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.Bone;
			dustTypeRare = DustID.BoneTorch;
			offset = 2;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 80f;
			widthMultiplier = 22f;
			lanceHitboxBounds = new(0, 0, 200, 200);
		}

		public override int DollHeldOffset() => 106;
	}
}