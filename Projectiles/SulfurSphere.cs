using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class SulfurSphere : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
		}
        public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 16;
			projectile.height = 16;
			projectile.alpha = 255;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.magic = true;
			projectile.penetrate = 1;
			aiType = ProjectileID.Bullet;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			projectile.ai[0]++;
			if (projectile.alpha > 0)
            {
				projectile.alpha -= 20;
            }
			Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.5f);
			if (projectile.ai[0] % 2 == 0)
            {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.SulfurDust>(), projectile.velocity.X, projectile.velocity.Y, 200, default, 2f);
				Main.dust[dust].noGravity = true;
				if (projectile.ai[0] % 4 == 0)
				{
					Main.dust[dust].noLight = true;
				}
				if (projectile.ai[0] % 8 == 0)
				{
					Main.dust[dust].noGravity = false;
				}
			}
		}
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			target.AddBuff(ModContent.BuffType<Buffs.SulfuricAcid>(), 300 + Main.rand.Next(0, 120));
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(ModContent.BuffType<Buffs.SulfuricAcid>(), 300 + Main.rand.Next(0, 120));
			target.netUpdate = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override void Kill(int timeLeft)
		{
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, ModContent.DustType<Dusts.SulfurDust>());
			Main.PlaySound(SoundID.NPCDeath3, projectile.position);
		}
	}
}