using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class FestiveWhipProj : WhipProjBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.WhipSettings.RangeMultiplier = 2f;
		}

		public override int DustType()
		{
			return Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.RedTorch);
		}
		public override float MultiHitPenatly()
		{
			return 0.8f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);

			Player owner = Main.player[Projectile.owner];
			float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;

			//Solar Eruption source
			if (Projectile.owner == Main.myPlayer)
			{
				if (Projectile.damage > Projectile.originalDamage * 0.5f)
				{
					//Main.NewText("Timer: " + Timer);
					//Main.NewText("swingTime: " + swingTime);
					if (Timer > (int)(swingTime * 0.55f) && Timer < (int)(swingTime * 0.75f) && owner.itemAnimation > 0)
					{
						//Main.NewText("Spawn Ornament");
						Projectile.NewProjectile(Entity.GetSource_FromThis(), new Vector2(target.position.X, target.position.Y - Main.screenHeight - 100 - (target.position.Y - owner.position.Y)), new Vector2(Main.rand.NextFloat(-2, 2f), Main.rand.Next(10, 14)), ModContent.ProjectileType<FestiveOrnament>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, -1, Main.rand.Next(0, 4));
					}
				}
			}
		}

		public override WhipDrawData DrawPositions(ref bool decrementLineCount, ref Color lineColor)
		{
			decrementLineCount = true;
			Projectile.GetWhipSettings(Projectile, out float timeToFlyOutForLine, out int _, out float _);
			float tForLine = Timer / timeToFlyOutForLine;
			lineColor = Color.Lerp(Color.Green, Color.Red, Utils.GetLerpValue(0.1f, 0.7f, tForLine, true) * Utils.GetLerpValue(0.9f, 0.7f, tForLine, true));
			Rectangle handle = new(0, 0, 14, 32);
			Vector2 handleOffset = new(7, 10);
			WhipDrawData.SegmentData head = new(116, 30);
			WhipDrawData.SegmentData third = new(88, 28);
			WhipDrawData.SegmentData second = new(60, 28);
			WhipDrawData.SegmentData first = new(32, 28);
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
