using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Magic
{
	public class SulfurSphere : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
		}
        public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.alpha = 255;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			AIType = ProjectileID.Bullet;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			Projectile.ai[0]++;
			if (Projectile.alpha > 0)
            {
				Projectile.alpha -= 20;
            }
			Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.5f);
			if (Projectile.ai[0] % 2 == 0)
            {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), Projectile.velocity.X, Projectile.velocity.Y, 200, default, 2f);
				Main.dust[dust].noGravity = true;
				if (Projectile.ai[0] % 4 == 0)
				{
					Main.dust[dust].noLight = true;
				}
				if (Projectile.ai[0] % 8 == 0)
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
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override void Kill(int timeLeft)
		{
			Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, ModContent.DustType<Dusts.SulfurDust>());
			SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.position);
		}
	}
}