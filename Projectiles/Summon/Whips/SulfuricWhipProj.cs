using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class SulfuricWhipProj : WhipProjBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.ownerHitCheck = false; // This prevents the projectile from hitting through solid tiles.
			Projectile.WhipSettings.RangeMultiplier = 1.2f;
		}

		public override int DustType()
		{
			return ModContent.DustType<Dusts.SulfurDust>();
		}
		public override float MultiHitPenatly()
		{
			return 0.6f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 240);
		}

		public override WhipDrawData DrawPositions(ref bool decrementLineCount, ref Color lineColor)
		{
			decrementLineCount = false;
			lineColor = Color.Orange;
			Rectangle handle = new(0, 0, 22, 30);
			Vector2 handleOffset = new(11, 8);
			WhipDrawData.SegmentData head = new(90, 32);
			WhipDrawData.SegmentData third = new(70, 20);
			WhipDrawData.SegmentData second = new(50, 20);
			WhipDrawData.SegmentData first = new(30, 20);
			return new WhipDrawData(handle, handleOffset, head, third, second, first);
		}

		public override Color DrawColor(int i, Vector2 currentElement, List<Vector2> controlPoints)
		{
			if (i == controlPoints.Count - 2) // Make the head full bright
			{
				return Color.White;
			}
			return base.DrawColor(i, currentElement, controlPoints);
		}
	}
}
