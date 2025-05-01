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
	public class FriendlyGreekFire1 : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.GreekFire1}";

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = 15;
			Projectile.timeLeft = 360;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = oldVelocity.X * -0.1f;
			}
			if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(Projectile.velocity.Y) > 1f) // Added
			{
				Projectile.velocity.Y = oldVelocity.Y * -0.1f;
			}
			return false;
		}
		public override void AI()
		{
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > 5f)
			{
				Projectile.ai[0] = 5f;
				if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
				{
					Projectile.velocity.X *= 0.97f;
					if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
					{
						Projectile.velocity.X = 0f;
						Projectile.netUpdate = true;
					}
				}

				Projectile.velocity.Y += 0.2f;
			}

			Projectile.rotation += Projectile.velocity.X * 0.1f;

			if (Projectile.wet)
			{
				Projectile.Kill();
			}

			if (Projectile.ai[1] == 0f)
			{
				Projectile.ai[1] = 1f;
				SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
			}

			if (!Main.rand.NextBool(3))
			{
				Dust dust10 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
				dust10.velocity.Y -= 2f;
				dust10.noGravity = true;
				Dust dust2 = dust10;
				dust2.scale += Main.rand.NextFloat() * 0.8f + 0.3f;
				dust2 = dust10;
				dust2.velocity += Projectile.velocity * 1f;
			}

			/*
			if (Projectile.velocity.Y < 0.25 && Projectile.velocity.Y > 0.15)
			{
				Projectile.velocity.X *= 0.8f;
			}
			*/

			Projectile.rotation = (0f - Projectile.velocity.X) * 0.05f;

			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Transparent;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			ulong seed4 = Main.TileFrameSeed;
			for (int i = 0; i < 4; i++)
			{
				Vector2 worldRandom = new(Utils.RandomInt(ref seed4, -2, 3), Utils.RandomInt(ref seed4, -2, 3));
				float width = (float)(TextureAssets.Projectile[Type].Width() - Projectile.width) * 0.5f + Projectile.width * 0.5f;
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (Main.rand.NextBool()) // Added to randomly flip the sprite.
				{
					spriteEffects = SpriteEffects.FlipHorizontally;
				}
				Main.EntitySpriteDraw(
					TextureAssets.Projectile[Type].Value,
					new Vector2(Projectile.position.X - Main.screenPosition.X + width, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2)) + worldRandom - Projectile.velocity * 0.25f * i,
					new Rectangle(0, 0, TextureAssets.Projectile[Type].Width(),
					TextureAssets.Projectile[Type].Height()),
					new Color(120, 120, 120, 60) * 1f,
					Projectile.rotation,
					new Vector2(width, Projectile.height / 2),
					Projectile.scale - (float)i * 0.2f,
					spriteEffects);
			}
			return false;
		}
	}

	public class FriendlyGreekFire2 : FriendlyGreekFire1
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.GreekFire2}";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 12;
			Projectile.height = 14;
		}
	}

	public class FriendlyGreekFire3 : FriendlyGreekFire1
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.GreekFire3}";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 6;
			Projectile.height = 12;
		}
	}
}
