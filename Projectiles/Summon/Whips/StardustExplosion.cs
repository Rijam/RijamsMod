using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class StardustExplosion : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
		}
        public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.alpha = 100;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = -1;
			AIType = -1;
			Projectile.timeLeft = 16;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 32;
			DrawOffsetX = 8;
			DrawOriginOffsetY += 8;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, Color.Lerp(Color.Yellow, Color.LightBlue, Projectile.frameCounter / 16f).ToVector3() * 0.2f);
			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 4;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.Kill();
				}
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.BlueTorch);
				dust.noGravity = true;
			}
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.StardustPunch, new ParticleOrchestraSettings
			{
				PositionInWorld = Projectile.Center,
				MovementVector = new Vector2(0f, -1f)
			});
			SoundEngine.PlaySound(new("Terraria/Sounds/Item_29") { Volume = 0.25f }, Projectile.position);
		}
	}
}