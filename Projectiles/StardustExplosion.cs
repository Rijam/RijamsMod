using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class StardustExplosion : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			Main.projFrames[projectile.type] = 4;
		}
        public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.alpha = 100;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.minion = true;
			projectile.penetrate = -1;
			aiType = -1;
			projectile.timeLeft = 16;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 32;
			drawOffsetX = 8;
			drawOriginOffsetY += 4;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, Color.Lerp(Color.Yellow, Color.LightBlue, projectile.frameCounter / 16f).ToVector3() * 0.2f);
			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 4;
			projectile.frameCounter++;
			if (projectile.frameCounter >= frameSpeed)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.Kill();
				}
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.BlueTorch);
				Main.dust[dust].noGravity = true;
			}
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 29, 0.25f);
		}
	}
}