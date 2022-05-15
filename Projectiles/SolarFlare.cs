using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;

namespace RijamsMod.Projectiles
{
	public class SolarFlare : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
		}
        public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.penetrate = 1;
			projectile.melee = true;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 60;
			projectile.timeLeft = 500;
			projectile.alpha = 200;
			aiType = -1;
		}

        public override void AI()
        {
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			projectile.velocity.Y += 0.4f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			int selectRand = Utils.SelectRandom(Main.rand, DustID.OrangeTorch, DustID.SolarFlare, DustID.Fire);
			Dust killDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, selectRand)];
			killDust.noGravity = true;
			killDust.fadeIn = 0.25f;
			Dust killDust3 = killDust;
			killDust3.velocity *= 2f;
			killDust.noLight = true;

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
			for (int i = 0; i < 30; i++)
			{
				int selectRand = Utils.SelectRandom(Main.rand, DustID.OrangeTorch, DustID.SolarFlare, DustID.Fire);
				Dust killDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, selectRand)];
				killDust.noGravity = true;
				killDust.scale = 1.25f + Main.rand.NextFloat();
				killDust.fadeIn = 0.25f;
				Dust killDust3 = killDust;
				killDust3.velocity *= 2f;
				killDust.noLight = true;
			}
		}
		public override Color? GetAlpha(Color newColor)
		{
			return Color.White;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Daybreak, 30);
		}
	}
}