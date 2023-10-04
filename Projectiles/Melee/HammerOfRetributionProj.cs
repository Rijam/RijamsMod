using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee
{
	public class HammerOfRetributionProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.alpha = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			//projectile.extraUpdates = 1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Red * (Projectile.timeLeft / 100f);
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}

		public override void AI()
		{
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				PreKill(Projectile.timeLeft);
			}
			Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
			Projectile.rotation += 0.4f * Projectile.direction;
			Projectile.alpha += 25;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.Resize(96, 96);
			return true;
		}
		public override void OnKill(int timeLeft)
		{
			Projectile.position.X += Projectile.width / 2;
			Projectile.position.Y += Projectile.height / 2;
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;

			for (int i = 0; i < 10; i++)
            {
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
				Gore.NewGore(Entity.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), default, Main.rand.Next(61, 64), 1f);//Smoke gore
			}
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		}
	}
}