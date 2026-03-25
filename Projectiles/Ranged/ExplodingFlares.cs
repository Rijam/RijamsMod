using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Buffs.Debuffs;
using RijamsMod.Dusts;
using System.Reflection.Emit;

namespace RijamsMod.Projectiles.Ranged
{
	public class ExplodingFlarePop : ModProjectile
	{
		// Exploding Flare Pop texture taken from Venturous 1.6.1 by Pixelfox

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 10;
		}

		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 50;
			DrawOffsetX = -64;
			DrawOriginOffsetY = -64;
			Projectile.tileCollide = false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanHitPlayer(Player target)
		{
			return false;
		}

		public uint packedColor;

		public override Color? GetAlpha(Color lightColor)
		{
			// uint packedColor = (uint)Projectile.ai[0]; // Projectile.ai[] wasn't large/precise enough to hold the data.
			Color unpackedColor = new()
			{
				PackedValue = packedColor,
				A = 0
			};
			return unpackedColor;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(packedColor);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			packedColor = reader.ReadUInt32();
		}

		public override void AI()
		{
			if (Projectile.ai[0] > 1f)
			{
				Projectile.scale = Projectile.ai[0];
			}
			if (++Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
			}
		}
	}

	public class ExplodingFlareBase : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(ExplodingFlareBase);
		public override string Texture => Projectile.type == ModContent.ProjectileType<ExplodingFlareBase>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsARocketThatDealsDoubleDamageToPrimaryEnemy[Type] = true;
			ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;
			ProjectileID.Sets.Explosive[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300;
			DrawOriginOffsetY = -10;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
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
		/// The duration of the debuff when applied to enemies.
		/// </summary>
		/// <returns>600 by default</returns>
		public virtual int EnemyDebuffDuration()
		{
			return 600;
		}

		/// <summary>
		/// The duration of the debuff when applied to players.
		/// </summary>
		/// <returns>150 by default</returns>
		public virtual int PlayerDebuffDuration()
		{
			return 150;
		}

		/// <summary>
		/// Modifies the color of the visual flare pop.
		/// </summary>
		/// <returns>Color.White by default</returns>
		public virtual Color PopColor()
		{
			return Color.White;
		}

		/// <summary>
		/// Modifies the explosion radius of the flare.
		/// </summary>
		/// <returns>Vector2(80, 80) by default</returns>
		public virtual Vector2 ExplosionRadius()
		{
			return new Vector2(80, 80);
		}

		/// <summary>
		/// Adds the spelunker shining effect.
		/// </summary>
		/// <returns>false by default</returns>
		public virtual bool SpelunkerEffect()
		{
			return false;
		}

		/// <summary>
		/// If true, friendly Greek Fire will be launched on the flare's death.
		/// </summary>
		/// <returns>False by default</returns>
		public virtual bool SpitGreekFire()
		{
			return false;
		}

		/// <summary>
		/// The scaling of the pop projectile.
		/// </summary>
		/// <returns>1f by default</returns>
		public virtual float PopScale()
		{
			return 1f;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.timeLeft = (int)(Projectile.timeLeft * Projectile.ai[2]);
			Projectile.netUpdate = true;
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

			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				Projectile.PrepareBombToBlow();
			}
			else
			{
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
					Main.dust[dust].position.X -= projVelX;
					Main.dust[dust].position.Y -= projVelY;
					Main.dust[dust].velocity.X -= projVelX;
					Main.dust[dust].velocity.Y -= projVelY;
					/*
					if (dustType == DustID.RainbowTorch)
					{
						Main.dust[dust].color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.5f);
						Main.dust[dust].scale *= 0.5f;
						Main.dust[dust].velocity *= 0.75f;
					}
					*/
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
				}
			}
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
			Projectile.rotation = (float)Math.Atan2(Projectile.ai[1], Projectile.ai[0]) + MathHelper.PiOver2;
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
			target.AddBuff(DebuffType(), EnemyDebuffDuration(), quiet: false);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(DebuffType(), PlayerDebuffDuration(), quiet: false);
		}

		public override void PrepareBombToBlow()
		{
			Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the rocket explodes on slopes.
			Projectile.alpha = 255; // Make the rocket invisible.

			// Resize the hitbox of the projectile for the blast "radius".
			// Rocket I: 128, Rocket III: 200, Mini Nuke Rocket: 250
			// Measurements are in pixels, so 128 / 16 = 8 tiles.
			Vector2 explosionRadius = ExplosionRadius();
			Projectile.Resize((int)explosionRadius.X, (int)explosionRadius.Y);
			// Set the knockback of the blast.
			// Rocket I: 8f, Rocket III: 10f, Mini Nuke Rocket: 12f
			Projectile.knockBack *= 3f;
		}
		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/FlarePop") { MaxInstances = 10 }, Projectile.position);

			// Resize the projectile again so the explosion dust and gore spawn from the middle.
			// Rocket I: 22, Rocket III: 80, Mini Nuke Rocket: 50
			Projectile.Resize(6, 6);
			Color popColor = PopColor();
			float popScale = PopScale();
			Projectile popProj = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<ExplodingFlarePop>(), 0, 0, Projectile.owner, popScale);
			if (popProj.ModProjectile is ExplodingFlarePop flarePop)
			{
				flarePop.packedColor = popColor.PackedValue;
			}

			float dustScale = 1f;
			for (int j = 0; j < 20; j++)
			{
				Dust fireDust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustType(ref dustScale), 0f, 0f, 100, default, 3.5f);
				fireDust.noGravity = true;
				fireDust.velocity *= 7f;
				Dust fireDust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustType(ref dustScale), 0f, 0f, 100, default, 1.5f);
				fireDust2.velocity *= 3f;
				if (dustScale != 1f)
				{
					fireDust.scale *= dustScale;
					fireDust2.scale *= dustScale;
				}
			}

			if (SpitGreekFire())
			{
				for (int k = 0; k < 1 + Main.rand.Next(3); k++) // 1-3 projectiles
				{
					Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextVector2Circular(8f, 8f), ModContent.ProjectileType<FriendlyGreekFire1>() + Main.rand.Next(3), Projectile.damage / 2, 0, Projectile.owner);
				}
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.timeLeft);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.timeLeft = reader.ReadInt32();
		}
	}

	public class ExplodingFlareProj : ExplodingFlareBase
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Flare}";

		public override int DustType(ref float dustScale)
		{
			return DustID.Flare;
		}

		public override Color PopColor()
		{
			return Color.OrangeRed;
		}
	}

	public class ExplodingBlueFlareProj : ExplodingFlareBase
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.BlueFlare}";

		public override int DustType(ref float dustScale)
		{
			return DustID.Flare_Blue;
		}

		public override Color PopColor()
		{
			return Color.Blue;
		}
	}

	public class ExplodingSpelunkerFlareProj : ExplodingFlareBase
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SpelunkerFlare}";

		public override int DustType(ref float dustScale)
		{
			return DustID.IchorTorch;
		}

		public override Color PopColor()
		{
			return Color.Yellow;
		}

		public override bool SpelunkerEffect()
		{
			return true;
		}
	}

	public class ExplodingCursedFlareProj : ExplodingFlareBase
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.CursedFlare}";

		public override int DustType(ref float dustScale)
		{
			return DustID.CursedTorch;
		}

		public override Color PopColor()
		{
			return Color.Green;
		}

		public override int DebuffType()
		{
			return BuffID.CursedInferno;
		}
	}

	public class ExplodingRainbowFlareProj : ExplodingFlareBase
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.RainbowFlare}";

		public override int DustType(ref float dustScale)
		{
			return ModContent.DustType<RainbowFlareDust>(); //DustID.RainbowTorch;
		}

		public override Color PopColor()
		{
			return Main.DiscoColor;
		}
	}

	public class ExplodingShimmerFlareProj : ExplodingFlareBase
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.ShimmerFlare}";

		public override int DustType(ref float dustScale)
		{
			return DustID.ShimmerTorch;
		}

		public override Color PopColor()
		{
			return Color.Magenta;
		}
	}

	public class ExplodingIchorFlareProj : ExplodingFlareBase
	{
		public override string Texture => $"RijamsMod/Projectiles/Ranged/{nameof(IchorFlareProj)}";

		public override int DustType(ref float dustScale)
		{
			return DustID.IchorTorch;
		}

		public override Color PopColor()
		{
			return Color.Gold;
		}

		public override int DebuffType()
		{
			return BuffID.Ichor;
		}
	}
	public class ExplodingSulfurFlareProj : ExplodingFlareBase
	{
		public override string Texture => $"RijamsMod/Projectiles/Ranged/{nameof(SulfurFlareProj)}";

		public override int DustType(ref float dustScale)
		{
			return ModContent.DustType<SulfurDust>();
		}

		public override Color PopColor()
		{
			return Color.Orange;
		}

		public override int DebuffType()
		{
			return ModContent.BuffType<SulfuricAcid>();
		}
	}
	public class ExplodingSolarFlareFlareProj : ExplodingFlareBase
	{
		public override string Texture => $"RijamsMod/Projectiles/Ranged/{nameof(SolarFlareFlareProj)}";

		public override int DustType(ref float dustScale)
		{
			//dustScale = 0.5f;
			return ModContent.DustType<SolarFlareFlareDust>();
		}

		public override Color PopColor()
		{
			return Color.DarkOrange;
		}

		public override int DebuffType()
		{
			return BuffID.Daybreak;
		}
		public override Vector2 ExplosionRadius()
		{
			return new Vector2(160, 160);
		}
		public override float PopScale()
		{
			return 2f;
		}
		public override int PlayerDebuffDuration()
		{
			return 30;
		}
	}

	public class ExplodingGreekFireFlareProj : ExplodingFlareBase
	{
		public override string Texture => $"RijamsMod/Projectiles/Ranged/{nameof(GreekFireFlareProj)}";

		public override int DustType(ref float dustScale)
		{
			return DustID.YellowTorch;
		}

		public override Color PopColor()
		{
			return Color.Yellow;
		}

		public override int DebuffType()
		{
			return BuffID.OnFire3;
		}

		public override bool SpitGreekFire()
		{
			return true;
		}
	}
}
