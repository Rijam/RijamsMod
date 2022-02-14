using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class FriendlyHarpyFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Feather");
		}

		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			aiType = ProjectileID.Bullet;
			if (!Main.hardMode)
			{
				projectile.penetrate = 1;
			}
			if (Main.hardMode)
			{
				projectile.penetrate = 2;
			}
			projectile.timeLeft = 600;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
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
			projectile.arrow = false;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.light = 0.1f;            //How much light emit around the projectile
			aiType = ProjectileID.Bullet;
			projectile.penetrate = 5;
			projectile.timeLeft = 180;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			return true;
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, 0.5f, 0.1f, 0.18f);

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
			if (Main.myPlayer == projectile.owner)
			{
				if (projectile.ai[1] >= 0f)
				{
					projectile.penetrate = -1;
				}
				else if (projectile.penetrate < 0)
				{
					projectile.penetrate = 1;
				}
				if (projectile.ai[1] >= 0f)
				{
					projectile.ai[1] += 1f;
				}
				if (projectile.ai[1] > (float)Main.rand.Next(5, 30))
				{
					projectile.ai[1] = -1000f;
					float num196 = projectile.velocity.Length();
					Vector2 vector28 = projectile.velocity;
					vector28.Normalize();
					int num197 = 2;
					for (int num198 = 0; num198 < num197; num198++)
					{
						Vector2 vector29 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
						vector29.Normalize();
						vector29 += vector28 * 2f;
						vector29.Normalize();
						vector29 *= num196 * 3f;
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector29.X, vector29.Y, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, 0f, -1000f);
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
			projectile.arrow = false;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.light = 0.5f;            //How much light emit around the projectile
			aiType = ProjectileID.Bullet;
			projectile.penetrate = 5;
			projectile.timeLeft = 600;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			return true;
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, 0.65f, 0.56f, 0.56f);

			projectile.localAI[0] += 1f;
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
			}
			if (projectile.alpha < 170)
			{
				for (int num164 = 0; num164 < 10; num164++)
				{
					float x2 = projectile.position.X - projectile.velocity.X / 10f * (float)num164;
					float y2 = projectile.position.Y - projectile.velocity.Y / 10f * (float)num164;
					int newDust = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.Iron);
					Main.dust[newDust].alpha = projectile.alpha;
					Main.dust[newDust].position.X = x2;
					Main.dust[newDust].position.Y = y2;
					Main.dust[newDust].velocity *= 0f;
					Main.dust[newDust].noGravity = true;
				}
			}
		}
	}
}