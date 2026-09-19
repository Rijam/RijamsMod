using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class ForbiddenWhipProj : WhipProjBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.WhipSettings.RangeMultiplier = 0.9f;
		}

		public override int DustType()
		{
			return DustID.GemAmber;
		}
		public override float MultiHitPenatly()
		{
			return 0.9f;
		}
		public override WhipDrawData DrawPositions(ref bool decrementLineCount, ref Color lineColor)
		{
			decrementLineCount = true;
			lineColor = Color.Goldenrod;
			Rectangle handle = new(0, 0, 22, 28);
			Vector2 handleOffset = new(11, 10);
			WhipDrawData.SegmentData head = new(88, 28);
			WhipDrawData.SegmentData third = new(68, 20);
			WhipDrawData.SegmentData second = new(48, 20);
			WhipDrawData.SegmentData first = new(28, 20);
			return new WhipDrawData(handle, handleOffset, head, third, second, first);
		}

		public override Color DrawColor(int i, Vector2 currentElement, List<Vector2> controlPoints)
		{
			if (i == controlPoints.Count - 2) // Make the head full bright
			{
				return Color.White;
			}
			return Color.Lerp(Lighting.GetColor(currentElement.ToTileCoordinates()), Color.White, i / 21f); // Make the segments brighter the closer to the head they are
		}
	}
}
