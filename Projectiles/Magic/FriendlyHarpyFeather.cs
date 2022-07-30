using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Magic
{
	public class FriendlyHarpyFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Feather");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			AIType = ProjectileID.Bullet;
			if (!Main.hardMode)
			{
				Projectile.penetrate = 1;
			}
			if (Main.hardMode)
			{
				Projectile.penetrate = 2;
			}
			Projectile.timeLeft = 600;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			return true;
		}
	}
	public class FriendlyHarpyFeatherRed : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Harpy Feather");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.light = 0.1f;            //How much light emit around the projectile
			AIType = ProjectileID.Bullet;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 180;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			return true;
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.5f, 0.1f, 0.18f);

			/*projectile.localAI[0] += 1f;
			if (projectile.localAI[0] > 3f)
			{
				projectile.alpha = 0;
			}
			if (projectile.ai[0] >= 20f)
			{
				projectile.ai[0] = 20f;
				if (projectile.type != 477)
				{
					projectile.velocity.Y += 0.05f;
				}
			}*/
			
			//Taken from Ichor Dart
			if (Main.myPlayer == Projectile.owner)
			{
				if (Projectile.ai[1] >= 0f)
				{
					Projectile.penetrate = -1;
				}
				else if (Projectile.penetrate < 0)
				{
					Projectile.penetrate = 1;
				}
				if (Projectile.ai[1] >= 0f)
				{
					Projectile.ai[1] += 1f;
				}
				if (Projectile.ai[1] > (float)Main.rand.Next(5, 30))
				{
					Projectile.ai[1] = -1000f;
					float num196 = Projectile.velocity.Length();
					Vector2 vector28 = Projectile.velocity;
					vector28.Normalize();
					int num197 = 2;
					for (int num198 = 0; num198 < num197; num198++)
					{
						Vector2 vector29 = new(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
						vector29.Normalize();
						vector29 += vector28 * 2f;
						vector29.Normalize();
						vector29 *= num196 * 3f;
						Projectile.NewProjectile(Entity.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, vector29.X, vector29.Y, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, -1000f);
					}
				}
			}
		}
	}
	public class FriendlyHarpyFeatherRazor : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Razor Harpy Feather");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.light = 0.5f;            //How much light emit around the projectile
			AIType = ProjectileID.Bullet;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 600;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			return true;
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.65f, 0.56f, 0.56f);

			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 3f)
			{
				Projectile.alpha = 0;
			}
			if (Projectile.ai[0] >= 20f)
			{
				Projectile.ai[0] = 20f;
				if (Projectile.type != 477)
				{
					Projectile.velocity.Y += 0.05f;
				}
			}
			if (Projectile.alpha < 170)
			{
				for (int num164 = 0; num164 < 10; num164++)
				{
					float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)num164;
					float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)num164;
					int newDust = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.Iron);
					Main.dust[newDust].alpha = Projectile.alpha;
					Main.dust[newDust].position.X = x2;
					Main.dust[newDust].position.Y = y2;
					Main.dust[newDust].velocity *= 0f;
					Main.dust[newDust].noGravity = true;
				}
			}
		}
	}
}