using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Ranged
{
	public class SulfurBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sulfur Bullet");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300; //5 seconds
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
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
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.alpha > 0)
			{
				Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.2f);
				Projectile.alpha -= 20;
			}
			if (Projectile.timeLeft % 2 == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), Projectile.velocity.X, Projectile.velocity.Y, 200, default, 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				if (Projectile.timeLeft % 8 == 0)
				{
					Main.dust[dust].noGravity = false;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, ModContent.DustType<Dusts.SulfurDust>());
			SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.position);
		}
	}
}