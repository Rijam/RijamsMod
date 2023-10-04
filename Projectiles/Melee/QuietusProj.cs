using Microsoft.Xna.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee
{
	public class QuietusProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.alpha = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 1;
			AIType = ProjectileID.Bullet;
			Projectile.timeLeft = 180;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
			//projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lime * (Projectile.timeLeft / 100f);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalPassive, new ParticleOrchestraSettings
			{
				PositionInWorld = Projectile.Center,
				MovementVector = Vector2.Zero
			});
			Projectile.Kill();
        }
        public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.alpha += 25;
			int selectRand = Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.CursedTorch, DustID.GreenFairy);
			Dust killDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, selectRand)];
			killDust.noGravity = true;
			killDust.fadeIn = 0.25f;
			Dust killDust3 = killDust;
			killDust3.velocity *= 2f;
			killDust.noLight = true;
			killDust3.alpha /= 2;
		}
		public override bool PreKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.position = Projectile.Center;
				Projectile.width = 32;
				Projectile.height = 32;
				Projectile.Center = Projectile.position;
			}
			return true;
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
            {
				int selectRand = Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.CursedTorch, DustID.GreenFairy);
				Dust killDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, selectRand)];
				killDust.noGravity = true;
				killDust.scale = 1.25f + Main.rand.NextFloat();
				killDust.fadeIn = 0.25f;
				Dust killDust3 = killDust;
				killDust3.velocity *= 2f;
				killDust.noLight = true;
			}
			SoundEngine.PlaySound(SoundID.Item66 with { MaxInstances = 6 }, Projectile.position) ;
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		}
	}
}