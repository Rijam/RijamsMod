using Microsoft.Xna.Framework;
using Terraria;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class BeltProj : WhipProjBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.WhipSettings.RangeMultiplier = 0.5f;
		}

		public override int DustType()
		{
			return 0;
		}
		public override float MultiHitPenatly()
		{
			return 0.5f;
		}
		public override WhipDrawData DrawPositions(ref bool decrementLineCount, ref Color lineColor)
		{
			decrementLineCount = false;
			lineColor = Color.Brown;
			Rectangle handle = new(0, 0, 10, 26);
			Vector2 handleOffset = new(5, 8);
			WhipDrawData.SegmentData head = new(74, 18);
			WhipDrawData.SegmentData third = new(58, 16);
			WhipDrawData.SegmentData second = new(42, 16);
			WhipDrawData.SegmentData first = new(26, 16);
			return new WhipDrawData(handle, handleOffset, head, third, second, first);
		}
	}
}
