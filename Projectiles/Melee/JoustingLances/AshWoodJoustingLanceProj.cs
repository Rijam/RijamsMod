using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	public class AshWoodJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.Ash;
			dustTypeRare = DustID.Smoke;
			offset = 2;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 61.75f;
			widthMultiplier = 20.75f;
			lanceHitboxBounds = new(0, 0, 150, 150);
		}
	}
}