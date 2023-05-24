using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;

namespace RijamsMod.Projectiles.Melee
{
	public class SolarFlare : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
		}
        public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.timeLeft = 500;
			Projectile.alpha = 200;
			AIType = -1;
		}

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.velocity.Y += 0.4f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
		}

        public override bool PreDraw(ref Color lightColor)
		{
			int selectRand = Utils.SelectRandom(Main.rand, DustID.OrangeTorch, DustID.SolarFlare, DustID.Torch);
			Dust killDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, selectRand)];
			killDust.noGravity = true;
			killDust.fadeIn = 0.25f;
			Dust killDust3 = killDust;
			killDust3.velocity *= 2f;
			killDust.noLight = true;

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
			for (int i = 0; i < 30; i++)
			{
				int selectRand = Utils.SelectRandom(Main.rand, DustID.OrangeTorch, DustID.SolarFlare, DustID.Torch);
				Dust killDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, selectRand)];
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
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Daybreak, 30);
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
			{
				PositionInWorld = Projectile.Center,
				MovementVector = new Vector2(0f, 0f)
			});
		}
	}
}