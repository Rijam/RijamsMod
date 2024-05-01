using Microsoft.Xna.Framework;
using RijamsMod.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Other
{
	public class ClankBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 3;
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
			ProjectileID.Sets.SummonTagDamageMultiplier[Type] = 0.5f;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.MagicSummonHybrid;
			Projectile.penetrate = 1;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 60;
		}

		public override void AI()
		{
			float vecolityMultiplier = 6f;
			int homingRange = 180;
			float timeBeforeItCanStartHoming = 270f;
			float timeLeftBeforeItStopsHoming = 30f;
			bool homingNeedsLineOfSight = true;

			bool canHome = false;
			float lerpValue1 = 0.075f;
			float lerpValue2 = 0.125f;

			if (Projectile.timeLeft > timeLeftBeforeItStopsHoming)
			{
				canHome = true;
			}

			int newTarget;
			if (homingNeedsLineOfSight)
			{
				newTarget = Projectile.FindTargetWithLineOfSight(homingRange);
			}
			else
			{
				NPC maybeTarget = Projectile.FindTargetWithinRange(homingRange);
				newTarget = maybeTarget != null ? maybeTarget.whoAmI : -1;
			}

			if (canHome)
			{
				Vector2 projVelocity = Projectile.velocity;
				if (Main.npc.IndexInRange(newTarget))
				{
					NPC npc = Main.npc[newTarget];
					projVelocity = Projectile.DirectionTo(npc.Center) * vecolityMultiplier;
				}
				else
				{
					Projectile.timeLeft--;
				}

				float lerpAmount = MathHelper.Lerp(lerpValue1, lerpValue2, Utils.GetLerpValue(timeBeforeItCanStartHoming, 30f, Projectile.timeLeft, clamped: true));
				Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, projVelocity, lerpAmount);
				Projectile.velocity *= MathHelper.Lerp(0.85f, 1f, Utils.GetLerpValue(0f, 90f, Projectile.timeLeft, clamped: true));

				Projectile.scale = MathHelper.Lerp(Math.Abs((float)Math.Sin(Projectile.timeLeft / 10f) / 2f), 1f, Projectile.timeLeft / 300f);
			}
			else
			{
				//Projectile.scale = 1f;

				if (Projectile.frameCounter++ >= 5)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.velocity *= 0.1f;
			Projectile.timeLeft = 30;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.velocity *= 0.1f;
			Projectile.timeLeft = 30;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.damage = 0;
			Projectile.tileCollide = false;
			Projectile.velocity *= 0.2f;
			Projectile.timeLeft = 30;
			return false;
		}
	}
}