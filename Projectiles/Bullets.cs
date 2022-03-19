using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class SulfurBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sulfur Bullet");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.timeLeft = 300; //5 seconds
			projectile.extraUpdates = 1;
			aiType = ProjectileID.Bullet;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
			target.netUpdate = true;
		}
		public override void PostAI()
        {
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (projectile.alpha > 0)
			{
				Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.2f);
				projectile.alpha -= 20;
			}
			if (projectile.timeLeft % 2 == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.SulfurDust>(), projectile.velocity.X, projectile.velocity.Y, 200, default, 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				if (projectile.timeLeft % 8 == 0)
				{
					Main.dust[dust].noGravity = false;
				}
			}
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

		public override void Kill(int timeLeft)
		{
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, ModContent.DustType<Dusts.SulfurDust>());
			Main.PlaySound(SoundID.NPCDeath3, projectile.position);
		}
	}
}