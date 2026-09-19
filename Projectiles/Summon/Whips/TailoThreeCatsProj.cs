using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class TailoThreeCatsProj : WhipProjBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.WhipSettings.Segments = 18;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.Confused, 240);
			}

			// This is needed in order for OnProcHit in the WhipTagEffect to activate.
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
				Main.player[Projectile.owner].TagEffectStack.TryEnableProcOnNPC(Projectile.tagEffectType, target);
			}
		}

		public override int DustType()
		{
			return DustID.Ash;
		}
		public override float MultiHitPenatly()
		{
			return 0.75f;
		}
		public override WhipDrawData DrawPositions(ref bool decrementLineCount, ref Color lineColor)
		{
			decrementLineCount = true;
			lineColor = Color.DarkGray;
			Rectangle handle = new(0, 0, 18, 32);
			Vector2 handleOffset = new(9, 13);
			WhipDrawData.SegmentData head = new(92, 20);
			WhipDrawData.SegmentData third = new(72, 20);
			WhipDrawData.SegmentData second = new(52, 20);
			WhipDrawData.SegmentData first = new(32, 20);
			return new WhipDrawData(handle, handleOffset, head, third, second, first);
		}

		public override void InsertDrawingAfterFrameIsPicked(int i, List<Vector2> controlPoints, ref float scale)
		{
			if (i != 0 && i != controlPoints.Count - 2)
			{
				// Scale down or up the middle segments. Helps make the overlap between segments less noticeable when the whip is all curled up.
				Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
				float t = Timer / timeToFlyOut;
				scale = MathHelper.Lerp(0.5f, 1.0f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
			}
		}
	}
}
