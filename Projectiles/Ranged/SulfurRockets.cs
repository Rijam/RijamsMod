using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Steamworks;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static Terraria.ModLoader.PlayerDrawLayer;
using static Terraria.WorldBuilding.IL_Searches;

namespace RijamsMod.Projectiles.Ranged
{
	public class SulfurRocket : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			RijamsModProjectile.RocketsAffectedByRocketBoosterExtraUpdates.Add(Type);
			ProjectileID.Sets.IsARocketThatDealsDoubleDamageToPrimaryEnemy[Type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1; // 16
			Projectile.friendly = true;
			Projectile.penetrate = -1; 
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300; //5 seconds
		}
		public override void AI()
		{
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				PreKill(Projectile.timeLeft);
			}
			else
			{
				if (Math.Abs(Projectile.velocity.X) >= 8f || Math.Abs(Projectile.velocity.Y) >= 8f)
				{
					for (int n = 0; n < 2; n++)
					{
						float num23 = 0f;
						float num24 = 0f;
						if (n == 1)
						{
							num23 = Projectile.velocity.X * 0.5f;
							num24 = Projectile.velocity.Y * 0.5f;
						}

						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X + 3f + num23, Projectile.position.Y + 3f + num24) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100);
						dust.scale *= 2f + (float)Main.rand.Next(10) * 0.1f;
						dust.velocity *= 0.2f;
						dust.noGravity = true;
						if (Projectile.timeLeft % 8 == 0)
						{
							dust.noGravity = false;
							dust.noLight = true;
						}
						if (dust.type == Dust.dustWater())
						{
							dust.scale *= 0.65f;
							dust.velocity += Projectile.velocity * 0.1f;
						}

						dust = Dust.NewDustDirect(new Vector2(Projectile.position.X + 3f + num23, Projectile.position.Y + 3f + num24) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.Smoke, 0f, 0f, 100, default, 0.5f);
						dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
						dust.velocity *= 0.05f;
					}
				}

				if (Math.Abs(Projectile.velocity.X) <= 16f && Math.Abs(Projectile.velocity.Y) <= 16f)
				{
					Projectile.velocity *= 1.1f;
				}
			}

			if (Projectile.velocity != Vector2.Zero)
			{
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.Resize(150, 150); // RocketI: 128, Rocket III: 200
			Projectile.knockBack = 8f; // 10
			return base.PreKill(timeLeft);
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

			Projectile.position.X += Projectile.width / 2;
			Projectile.position.Y += Projectile.height / 2;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
			for (int i = 0; i < 30; i++)
			{
				Dust smoke = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
				smoke.velocity *= 1.4f;
			}

			for (int j = 0; j < 20; j++)
			{
				Dust sulfurDust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100, default, 3.5f);
				sulfurDust.noGravity = true;
				sulfurDust.velocity *= 7f;
				sulfurDust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100, default, 1.5f);
				sulfurDust.velocity *= 3f;
			}

			for (int k = 0; k < 2; k++)
			{
				float speedMulti = 0.4f;
				if (k == 1)
				{
					speedMulti = 0.8f;
				}

				Gore smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity += Vector2.One;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y += 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X += 1f;
				smokeGore.velocity.Y -= 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y -= 1f;
			}
		}
	}
	public class SulfurGrenade : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1; // 16
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180; //3 seconds
		}
		public override void AI()
		{
			if (Projectile.owner == Main.myPlayer)
			{
				//Projectile.timeLeft = 180;
			}

			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				PreKill(Projectile.timeLeft);
			}
			else
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100);
				dust.scale *= 1f + (float)Main.rand.Next(10) * 0.1f;
				dust.velocity *= 0.2f;
				dust.noGravity = true;

				if (Projectile.timeLeft % 2 == 0)
				{
					Dust sulfurDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), Projectile.velocity.X, Projectile.velocity.Y, 200, default, 1f);
					sulfurDust.noGravity = true;
					if (Projectile.timeLeft % 8 == 0)
					{
						sulfurDust.noGravity = false;
						sulfurDust.noLight = true;
					}
				}
			}

			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > 20f) // 15f
			{
				if (Projectile.velocity.Y == 0f)
				{
					Projectile.velocity.X *= 0.97f; // 0.95f
				}

				Projectile.velocity.Y += 0.2f; // 0.2f
			}
			Projectile.rotation += Projectile.velocity.X * 0.1f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			/*
			if (velocity.X != lastVelocity.X)
			{
				velocity.X = lastVelocity.X * -0.4f;
			}

			if Projectile.velocity.Y != lastVelocity.Y && (double)lastVelocity.Y > 0.7 && type != 102)
			{
				Projectile.velocity.Y = lastVelocity.Y * -0.4f;
			}
			*/
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.Resize(150, 150); // RocketI: 128, Rocket III: 200
			Projectile.knockBack = 8f; // 10
			return base.PreKill(timeLeft);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

			Projectile.position.X += Projectile.width / 2;
			Projectile.position.Y += Projectile.height / 2;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
			for (int i = 0; i < 30; i++)
			{
				Dust smoke = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
				smoke.velocity *= 1.4f;
			}

			for (int j = 0; j < 20; j++)
			{
				Dust sulfurDust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100, default, 3.5f);
				sulfurDust.noGravity = true;
				sulfurDust.velocity *= 7f;
				sulfurDust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100, default, 1.5f);
				sulfurDust.velocity *= 3f;
			}

			for (int k = 0; k < 2; k++)
			{
				float speedMulti = 0.4f;
				if (k == 1)
				{
					speedMulti = 0.8f;
				}

				Gore smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity += Vector2.One;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y += 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X += 1f;
				smokeGore.velocity.Y -= 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y -= 1f;
			}
		}
	}
	public class SulfurMine : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAMineThatDealsTripleDamageWhenStationary[Type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1; // 16
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			//Projectile.tileCollide = false;
			//Projectile.timeLeft = 180; //3 seconds
		}
		public override void AI()
		{
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				PreKill(Projectile.timeLeft);
			}
			else
			{
				if (Projectile.velocity.X > -0.2 && Projectile.velocity.X < 0.2 && Projectile.velocity.Y > -0.2 && Projectile.velocity.Y < 0.2)
				{
					Projectile.alpha += 2;
					if (Projectile.alpha > 200)
					{
						Projectile.alpha = 200;
					}
				}
				else
				{
					Projectile.alpha = 0;
					Dust smoke = Dust.NewDustDirect(new Vector2(Projectile.position.X + 3f, Projectile.position.Y + 3f) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.Smoke, 0f, 0f, 100);
					smoke.scale *= 1.6f + (float)Main.rand.Next(5) * 0.1f;
					smoke.velocity *= 0.05f;
					smoke.noGravity = true;

					if (Projectile.timeLeft % 2 == 0)
					{
						Dust sulfurDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), Projectile.velocity.X, Projectile.velocity.Y, 200, default, 1f);
						sulfurDust.noGravity = true;
						if (Projectile.timeLeft % 8 == 0)
						{
							sulfurDust.noGravity = false;
							sulfurDust.noLight = true;
						}
					}
				}
			}

			Projectile.velocity.Y += 0.2f;
			Projectile.velocity *= 0.98f; // 0.97f
			if (Projectile.velocity.X > -0.1 && Projectile.velocity.X < 0.1)
			{
				Projectile.velocity.X = 0f;
			}

			if (Projectile.velocity.Y > -0.1 && Projectile.velocity.Y < 0.1)
			{
				Projectile.velocity.Y = 0f;
			}
			Projectile.rotation += Projectile.velocity.X * 0.1f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = oldVelocity.X * -0.6f; // -0.4f
			}

			if (Projectile.velocity.Y != oldVelocity.Y && (double)oldVelocity.Y > 0.7)
			{
				Projectile.velocity.Y = oldVelocity.Y * -0.6f; // -0.4f
			}
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.Resize(150, 150); // RocketI: 128, Rocket III: 200
			Projectile.knockBack = 8f; // 10
			return base.PreKill(timeLeft);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

			Projectile.position.X += Projectile.width / 2;
			Projectile.position.Y += Projectile.height / 2;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
			for (int i = 0; i < 30; i++)
			{
				Dust smoke = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
				smoke.velocity *= 1.4f;
			}

			for (int j = 0; j < 20; j++)
			{
				Dust sulfurDust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100, default, 3.5f);
				sulfurDust.noGravity = true;
				sulfurDust.velocity *= 7f;
				sulfurDust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100, default, 1.5f);
				sulfurDust.velocity *= 3f;
			}

			for (int k = 0; k < 2; k++)
			{
				float speedMulti = 0.4f;
				if (k == 1)
				{
					speedMulti = 0.8f;
				}

				Gore smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity += Vector2.One;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y += 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X += 1f;
				smokeGore.velocity.Y -= 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y -= 1f;
			}
		}
	}
	public class SulfurSnowmanRocket : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			RijamsModProjectile.RocketsAffectedByRocketBoosterExtraUpdates.Add(Type);
			ProjectileID.Sets.IsARocketThatDealsDoubleDamageToPrimaryEnemy[Type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1; // 16
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.scale = 0.9f;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300; //5 seconds
		}
		public override void AI()
		{
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				PreKill(Projectile.timeLeft);
			}
			else
			{
				Projectile.localAI[1] += 1f;
				if (Projectile.localAI[1] > 6f)
				{
					Projectile.alpha = 0;
				}
				else
				{
					Projectile.alpha = (int)(255f - 42f * Projectile.localAI[1]) + 100;
					if (Projectile.alpha > 255)
					{
						Projectile.alpha = 255;
					}
				}

				for (int l = 0; l < 2; l++)
				{
					float velocityXAdder = 0f;
					float velocityYAdder = 0f;
					if (l == 1)
					{
						velocityXAdder = Projectile.velocity.X * 0.5f;
						velocityYAdder = Projectile.velocity.Y * 0.5f;
					}

					if (!(Projectile.localAI[1] > 9f))
					{
						continue;
					}

					if (Main.rand.NextBool(2))
					{
						Dust sulfurDust = Dust.NewDustDirect(new Vector2(Projectile.position.X + 3f + velocityXAdder, Projectile.position.Y + 3f + velocityYAdder) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100);
						sulfurDust.scale *= 1.4f + (float)Main.rand.Next(10) * 0.1f;
						sulfurDust.velocity *= 0.2f;
						sulfurDust.noGravity = true;
						if (sulfurDust.type == Dust.dustWater())
						{
							sulfurDust.scale *= 0.65f;
							sulfurDust.velocity += Projectile.velocity * 0.1f;
						}
					}

					if (Main.rand.NextBool(2))
					{
						Dust smoke = Dust.NewDustDirect(new Vector2(Projectile.position.X + 3f + velocityXAdder, Projectile.position.Y + 3f + velocityYAdder) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.Smoke, 0f, 0f, 100, default, 0.5f);
						smoke.fadeIn = 0.5f + (float)Main.rand.Next(5) * 0.1f;
						smoke.velocity *= 0.05f;
					}
				}

				float x = Projectile.position.X;
				float y = Projectile.position.Y;
				float maxHomingDistance = 600f;

				bool canHome = false;
				Projectile.ai[0] += 1f;
				if (Projectile.ai[0] > 30f)
				{
					Projectile.ai[0] = 30f;
					for (int m = 0; m < Main.maxNPCs; m++)
					{
						if (Main.npc[m].CanBeChasedBy(this))
						{
							float targetPosX = Main.npc[m].position.X + (Main.npc[m].width / 2);
							float targetPosY = Main.npc[m].position.Y + (Main.npc[m].height / 2);
							float distanceFromProjToTarget = Math.Abs(Projectile.position.X + (Projectile.width / 2) - targetPosX) + Math.Abs(Projectile.position.Y + (Projectile.height / 2) - targetPosY);
							if (distanceFromProjToTarget < maxHomingDistance && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[m].position, Main.npc[m].width, Main.npc[m].height))
							{
								maxHomingDistance = distanceFromProjToTarget;
								x = targetPosX;
								y = targetPosY;
								canHome = true;
							}
						}
					}
				}

				if (!canHome)
				{
					x = Projectile.position.X + (Projectile.width / 2) + Projectile.velocity.X * 100f;
					y = Projectile.position.Y + (Projectile.height / 2) + Projectile.velocity.Y * 100f;
				}

				float speed = 16f;

				Vector2 finalVelocity = (new Vector2(x, y) - Projectile.Center).SafeNormalize(-Vector2.UnitY) * speed;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, finalVelocity, 1f / 12f);

				if (Projectile.velocity.X < 0f)
				{
					Projectile.spriteDirection = -1;
					Projectile.rotation = (float)Math.Atan2(0f - Projectile.velocity.Y, 0f - Projectile.velocity.X) - MathHelper.PiOver2;
				}
				else
				{
					Projectile.spriteDirection = 1;
					Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
				}

				if (Math.Abs(Projectile.velocity.X) <= 16f && Math.Abs(Projectile.velocity.Y) <= 16f)
				{
					Projectile.velocity *= 1.1f;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			/*if (velocity.X != lastVelocity.X)
			{
				velocity.X = lastVelocity.X * -0.4f;
				if (type == 29)
					velocity.X *= 0.8f;
			}

			if (velocity.Y != lastVelocity.Y && (double)lastVelocity.Y > 0.7 && type != 102)
			{
				velocity.Y = lastVelocity.Y * -0.4f;
				if (type == 29)
					velocity.Y *= 0.8f;
			}*/

			// Projectile.velocity *= 0f;
			// Projectile.alpha = 255;
			// Projectile.timeLeft = 3;
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.Resize(150, 150); // RocketI: 128, Rocket III: 200
			Projectile.knockBack = 8f; // 10
			return base.PreKill(timeLeft);
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

			Projectile.position.X += Projectile.width / 2;
			Projectile.position.Y += Projectile.height / 2;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
			for (int i = 0; i < 30; i++)
			{
				Dust smoke = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
				smoke.velocity *= 1.4f;
			}

			for (int j = 0; j < 20; j++)
			{
				Dust sulfurDust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100, default, 3.5f);
				sulfurDust.noGravity = true;
				sulfurDust.velocity *= 7f;
				sulfurDust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100, default, 1.5f);
				sulfurDust.velocity *= 3f;
			}

			for (int k = 0; k < 2; k++)
			{
				float speedMulti = 0.4f;
				if (k == 1)
				{
					speedMulti = 0.8f;
				}

				Gore smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity += Vector2.One;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y += 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X += 1f;
				smokeGore.velocity.Y -= 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y -= 1f;
			}
		}
	}
}