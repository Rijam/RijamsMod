using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class SupernovaWhipProj : WhipProjBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.ownerHitCheck = false; // This prevents the projectile from hitting through solid tiles.
			Projectile.WhipSettings.RangeMultiplier = 3f;
		}

		private float Timer2
		{
			get => Projectile.ai[2];
			set => Projectile.ai[2] = value;
		}

		public override int DustType()
		{
			return Utils.SelectRandom(Main.rand, DustID.YellowTorch, DustID.BlueTorch);
		}

		public override bool OverrideDustSpawning(float swingProgress, float dustChance)
		{
			if (dustChance > 0.1f && Main.rand.NextFloat() < dustChance / 2f)
			{
				Projectile.WhipPointsForCollision.Clear();
				Projectile.FillWhipControlPoints(Projectile, Projectile.WhipPointsForCollision);
				Rectangle spawnArea = Utils.CenteredRectangle(Projectile.WhipPointsForCollision[^1], new Vector2(30f, 30f));
				Lighting.AddLight(spawnArea.Center(), Color.Yellow.ToVector3() * 0.2f);
				for (int i = 0; i < 5; i++)
				{
					int dust = Dust.NewDust(spawnArea.TopLeft(), spawnArea.Width, spawnArea.Height, DustType(), 0f, 0f, 0, Color.White, 2f);
					Main.dust[dust].noGravity = true;
				}
			}
			return true;
		}

		public override float MultiHitPenatly()
		{
			return 0.95f;
		}

		public override void AI()
		{
			base.AI();
			Timer2 = 1; // Each segment can only spawn one Stardust Explosion
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);
			target.AddBuff(BuffID.StardustMinionBleed, 300); // Stardust Cell debuff

			Player owner = Main.player[Projectile.owner];
			float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;

			//Solar Eruption source
			if (Projectile.owner == Main.myPlayer)
			{
				if (Projectile.damage > Projectile.originalDamage * 0.5f)
				{
					// Main.NewText("Timer: " + Timer);
					// Main.NewText("swingTime: " + swingTime);
					// Main.NewText("Timer2: " + Timer2);
					if (Timer2 > 0 && Timer < swingTime)
					{
						// Main.NewText("Spawn StardustExplosion");
						Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<StardustExplosion>(), Projectile.damage, 0, Projectile.owner);
						Timer2 -= 1;
					}
				}
			}
		}

		public override WhipDrawData DrawPositions(ref bool decrementLineCount, ref Color lineColor)
		{
			decrementLineCount = true;
			Projectile.GetWhipSettings(Projectile, out float timeToFlyOutForLine, out int _, out float _);
			float tForLine = Timer / timeToFlyOutForLine;
			lineColor = Color.Lerp(Color.Blue, Color.Yellow, Utils.GetLerpValue(0.1f, 0.7f, tForLine, true) * Utils.GetLerpValue(0.9f, 0.7f, tForLine, true));

			Rectangle handle = new(0, 0, 22, 34);
			Vector2 handleOffset = new(11, 10);
			WhipDrawData.SegmentData head = new(112, 30);
			WhipDrawData.SegmentData third = new(86, 26);
			WhipDrawData.SegmentData second = new(60, 26);
			WhipDrawData.SegmentData first = new(34, 26);
			return new WhipDrawData(handle, handleOffset, head, third, second, first);
		}

		public override Color DrawColor(int i, Vector2 currentElement, List<Vector2> controlPoints)
		{
			return Color.White;
		}
	}
}
