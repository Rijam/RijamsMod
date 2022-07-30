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
using Terraria.Audio;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class FestiveOrnament : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.OrnamentFriendly;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.timeLeft = 300;
			Projectile.aiStyle = -1;
			Projectile.alpha = 255;
		}

        public override void AI()
        {
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 10;
			}
			int num229 = (Projectile.frame = (int)Projectile.ai[1]);
			if (Projectile.ai[0] < 0f)
			{
				Projectile.velocity.Y += 0.25f;
				if (Projectile.velocity.Y > 14f)
				{
					Projectile.velocity.Y = 14f;
				}
			}
			else
			{
				if (Main.rand.NextBool(Math.Max(4, 8 - (int)Projectile.velocity.Length())))
				{
					Color newColor = Color.White;
					switch (num229)
					{
						case 0:
							newColor = new Color(255, 100, 100);
							break;
						case 1:
							newColor = new Color(100, 255, 100);
							break;
						case 2:
							newColor = new Color(100, 100, 255);
							break;
						case 3:
							newColor = new Color(255, 255, 100);
							break;
					}
					int num230 = 5;
					int num231 = Dust.NewDust(Projectile.position + new Vector2(num230, num230), Projectile.width - num230 * 2, Projectile.height - num230 * 2, DustID.TintableDustLighted, 0f, 0f, 254, newColor);
					Main.dust[num231].velocity = Projectile.velocity * 0.75f;
				}
				Projectile.velocity *= 0.95f;
			}
			if (Projectile.ai[0] >= 0f && Projectile.velocity.Length() < 0.25f)
			{
				if (Projectile.velocity != Vector2.Zero)
				{
					Projectile.velocity = Vector2.Zero;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.ai[0] = 50f;
						Projectile.netUpdate = true;
					}
				}
				Projectile.ai[0]--;
			}
			Projectile.localAI[0]++;
			Projectile.rotation = (float)Math.Sin(Projectile.localAI[0] / 10f);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int colorType = 90 - (int)Projectile.ai[1];
				int killDust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, colorType);
				Main.dust[killDust].noLight = true;
				Main.dust[killDust].scale = 0.8f;
			}
		}
	}
}