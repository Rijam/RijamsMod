using Microsoft.Xna.Framework;
using Terraria.ID;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class VileWhipProj : WhipProjBase
	{
		public override int DustType()
		{
			return DustID.Demonite;
		}
		public override float MultiHitPenatly()
		{
			return 0.7f;
		}
		public override WhipDrawData DrawPositions(ref bool decrementLineCount, ref Color lineColor)
		{
			decrementLineCount = true;
			lineColor = Color.Purple;
			Rectangle handle = new(0, 0, 22, 32);
			Vector2 handleOffset = new(11, 8);
			WhipDrawData.SegmentData head = new(86, 32);
			WhipDrawData.SegmentData third = new(68, 18);
			WhipDrawData.SegmentData second = new(50, 18);
			WhipDrawData.SegmentData first = new(32, 18);
			return new WhipDrawData(handle, handleOffset, head, third, second, first);
		}
	}
}
