using Microsoft.Xna.Framework;
using Terraria.ID;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	public class BorealWoodJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.BorealWood;
			dustTypeRare = 0;
			offset = 2;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 50.25f;
			widthMultiplier = 20.1f;
			lanceHitboxBounds = new(0, 0, 125, 125);
		}

		public override int DollHeldOffset() => 72;
	}
}