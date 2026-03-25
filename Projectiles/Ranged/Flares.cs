using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Buffs.Debuffs;
using RijamsMod.Dusts;

namespace RijamsMod.Projectiles.Ranged
{
	public class FlareProjBase : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(FlareProjBase);
		public override string Texture => Projectile.type == ModContent.ProjectileType<FlareProjBase>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 36000;
			DrawOriginOffsetY = -10;
		}

		/// <summary>
		/// Modifies the dust type that is used on the flare and the explosion.
		/// </summary>
		/// <returns>DustID.Torch by default</returns>
		public virtual int DustType(ref float dustScale)
		{
			return DustID.Torch;
		}

		/// <summary>
		/// Modifies the debuff type that is applied to NPCs and players.
		/// </summary>
		/// <returns>BuffID.OnFire by default</returns>
		public virtual int DebuffType()
		{
			return BuffID.OnFire;
		}

		/// <summary>
		/// <br>The duration of the debuff when applied to enemies.</br>
		/// <br>There is a 33% chance it will be this value. There is a 67% chance it will be half of this value.</br>
		/// </summary>
		/// <returns>600 by default</returns>
		public virtual int EnemyDebuffDuration()
		{
			return 600;
		}

		/// <summary>
		/// The duration of the debuff when applied to players.
		/// </summary>
		/// <returns>300 by default</returns>
		public virtual int PlayerDebuffDuration()
		{
			return 300;
		}

		/// <summary>
		/// Adds the spelunker shining effect.
		/// </summary>
		/// <returns>false by default</returns>
		public virtual bool SpelunkerEffect()
		{
			return false;
		}

		public override void AI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 50;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}

			float numIs4f = 4f;
			float projVelX = Projectile.ai[0];
			float projVelY = Projectile.ai[1];
			if (projVelX == 0f && projVelY == 0f)
			{
				projVelX = 1f;
			}

			float distance = (float)Math.Sqrt(projVelX * projVelX + projVelY * projVelY);
			distance = numIs4f / distance;
			projVelX *= distance;
			projVelY *= distance;
			if (Projectile.alpha < 70)
			{
				float dustScale = 1f;
				int dustType = DustType(ref dustScale);

				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y - 2f), 6, 6, dustType, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.6f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].position.X -= projVelX * 1f;
				Main.dust[dust].position.Y -= projVelY * 1f;
				Main.dust[dust].velocity.X -= projVelX;
				Main.dust[dust].velocity.Y -= projVelY;
				if (dustScale != 1f)
				{
					Main.dust[dust].scale *= dustScale;
				}
			}

			if (Projectile.localAI[0] == 0f)
			{
				Projectile.ai[0] = Projectile.velocity.X;
				Projectile.ai[1] = Projectile.velocity.Y;
				Projectile.localAI[1] += 1f;
				if (Projectile.localAI[1] >= 30f)
				{
					Projectile.velocity.Y += 0.09f;
					Projectile.localAI[1] = 30f;
				}
			}
			else
			{
				if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.localAI[0] = 0f;
					Projectile.localAI[1] = 30f;
				}

				if (SpelunkerEffect() && Main.netMode != NetmodeID.Server)
				{
					int num287 = 30;
					if ((Projectile.Center - Main.LocalPlayer.Center).Length() < (float)(Main.screenWidth + num287 * 16))
					{
						Main.instance.SpelunkerProjectileHelper.AddSpotToCheck(Projectile.Center);
					}
				}

				Projectile.damage = 0;
			}

			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

			Projectile.rotation = (float)Math.Atan2(Projectile.ai[1], Projectile.ai[0]) + MathHelper.PiOver2;

			if (Projectile.shimmerWet)
			{
				int centerX = (int)(Projectile.Center.X / 16f);
				int posY = (int)(Projectile.position.Y / 16f);
				if (WorldGen.InWorld(centerX, posY) // In the world
					&& Main.tile[centerX, posY] != null // Hits a tile
					&& Main.tile[centerX, posY].LiquidAmount == byte.MaxValue // The liquid in the tile is full
					&& Main.tile[centerX, posY].LiquidType == LiquidID.Shimmer // The liquid is Shimmer
					&& WorldGen.InWorld(centerX, posY - 1) // The tile below it is also in the world
					&& Main.tile[centerX, posY - 1] != null
					&& Main.tile[centerX, posY - 1].LiquidAmount > 0
					&& Main.tile[centerX, posY - 1].LiquidType == LiquidID.Shimmer)
				{
					Projectile.Kill();
				}
				else if (Projectile.velocity.Y > 0f)
				{
					Projectile.velocity.Y *= -1f;
					Projectile.netUpdate = true;
					Projectile.shimmerWet = false;
					Projectile.wet = false;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity != Projectile.velocity)
			{
				if (Projectile.localAI[0] == 0f)
				{
					if (Projectile.wet)
					{
						Projectile.position += oldVelocity / 2f;
					}
					else
					{
						Projectile.position += oldVelocity;
					}

					Projectile.velocity *= 0f;
					Projectile.localAI[0] = 1f;
				}
			}

			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(3))
			{
				target.AddBuff(DebuffType(), EnemyDebuffDuration());
			}
			else
			{
				target.AddBuff(DebuffType(), EnemyDebuffDuration() / 2);
			}
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(DebuffType(), PlayerDebuffDuration(), quiet: false);
		}
	}


	public class IchorFlareProj : FlareProjBase
	{
		public override int DustType(ref float dustScale)
		{
			return DustID.IchorTorch;
		}

		public override int DebuffType()
		{
			return BuffID.Ichor;
		}
	}

	public class SulfurFlareProj : FlareProjBase
	{
		public override int DustType(ref float dustScale)
		{
			return ModContent.DustType<SulfurDust>();
		}

		public override int DebuffType()
		{
			return ModContent.BuffType<SulfuricAcid>();
		}
	}

	public class SolarFlareFlareProj : FlareProjBase
	{
		public override int DustType(ref float dustScale)
		{
			//dustScale = 0.5f;
			return ModContent.DustType<SolarFlareFlareDust>();
		}

		public override int DebuffType()
		{
			return BuffID.Daybreak;
		}
		public override int PlayerDebuffDuration()
		{
			return 60;
		}
	}

	public class GreekFireFlareProj : FlareProjBase
	{
		public override int DustType(ref float dustScale)
		{
			return DustID.YellowTorch;
		}

		public override int DebuffType()
		{
			return BuffID.OnFire3;
		}
	}
}