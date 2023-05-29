using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace RijamsMod.Projectiles.Ranged
{
	public class SulfurRocket : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			RijamsModProjectile.RocketsAffectedByRocketBoosterExtraUpdates.Add(Type);
			ProjectileID.Sets.IsARocketThatDealsDoubleDamageToPrimaryEnemy[Type] = true;
			ProjectileID.Sets.RocketsSkipDamageForPlayers[Type] = true;
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
				PrepareBombToBlow(Projectile);
			}
			else
			{
				if (Math.Abs(Projectile.velocity.X) >= 8f || Math.Abs(Projectile.velocity.Y) >= 8f)
				{
					for (int i = 0; i < 2; i++)
					{
						float posOffsetX = 0f;
						float posOffsetY = 0f;
						if (i == 1)
						{
							posOffsetX = Projectile.velocity.X * 0.5f;
							posOffsetY = Projectile.velocity.Y * 0.5f;
						}

						// Spawn fire dusts at the back of the rocket.
						Dust fireDust = Dust.NewDustDirect(new Vector2(Projectile.position.X + 3f + posOffsetX, Projectile.position.Y + 3f + posOffsetY) - Projectile.velocity * 0.5f,
							Projectile.width - 8, Projectile.height - 8, ModContent.DustType<Dusts.SulfurDust>(), 0f, 0f, 100);
						fireDust.scale *= 2f + Main.rand.Next(10) * 0.1f;
						fireDust.velocity *= 0.2f;
						fireDust.noGravity = true;
						if (Projectile.timeLeft % 8 == 0)
						{
							fireDust.noGravity = false;
							fireDust.noLight = true;
						}

						// Spawn smoke dusts at the back of the rocket.
						Dust smokeDust = Dust.NewDustDirect(new Vector2(Projectile.position.X + 3f + posOffsetX, Projectile.position.Y + 3f + posOffsetY) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.Smoke, 0f, 0f, 100, default, 0.5f);
						smokeDust.fadeIn = 1f + Main.rand.Next(5) * 0.1f;
						smokeDust.velocity *= 0.05f;
					}
				}

				if (Math.Abs(Projectile.velocity.X) <= 16f && Math.Abs(Projectile.velocity.Y) <= 16f)
				{
					Projectile.velocity *= 1.1f;
				}

				// Explosives behave differently when touching Shimmer.
				if (Projectile.shimmerWet)
				{
					int projTileX = (int)(Projectile.Center.X / 16f);
					int projTileY = (int)(Projectile.position.Y / 16f);
					// If the projectile is inside of Shimmer:
					if (WorldGen.InWorld(projTileX, projTileY) && Main.tile[projTileX, projTileY] != null &&
							Main.tile[projTileX, projTileY].LiquidAmount == byte.MaxValue &&
							Main.tile[projTileX, projTileY].LiquidType == LiquidID.Shimmer &&
							WorldGen.InWorld(projTileX, projTileY - 1) && Main.tile[projTileX, projTileY - 1] != null &&
							Main.tile[projTileX, projTileY - 1].LiquidAmount > 0 &&
							Main.tile[projTileX, projTileY - 1].LiquidType == LiquidID.Shimmer)
					{
						Projectile.Kill(); // Kill the projectile with no blast radius.
					}
					// Otherwise, bounce off of the top of the Shimmer if traveling downwards.
					else if (Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y *= -1f; // Reverse the Y velocity.
						Projectile.netUpdate = true; // Sync the change in multiplayer.
						if (Projectile.timeLeft > 600)
						{
							Projectile.timeLeft = 600; // Set the max time to 10 seconds (instead of the default 1 minute).
						}

						Projectile.timeLeft -= 60; // Subtract 1 second from the time left.
						Projectile.shimmerWet = false;
						Projectile.wet = false;
					}
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
			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3; // Set the timeLeft to 3 so it can get ready to explode.
			}

			// Set the direction of the projectile so the knockback is always in the correct direction.
			if (target.position.X + (target.width / 2) < Projectile.position.X + (Projectile.width / 2))
			{
				Projectile.direction = -1;
			}
			else
			{
				Projectile.direction = 1;
			}
		}

		// This is only to make it so the rocket explodes when hitting a player in PVP. Otherwise the rocket will continue through the enemy player.
		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			if (modifiers.PvP && Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3; // Set the timeLeft to 3 so it can get ready to explode.
			}
			// Set the direction of the projectile so the knockback is always in the correct direction.
			if (target.position.X + (target.width / 2) < Projectile.position.X + (Projectile.width / 2))
			{
				Projectile.direction = -1;
			}
			else
			{
				Projectile.direction = 1;
			}
		}

		/// <summary> Resizes the projectile for the explosion blast radius. </summary>
		public static void PrepareBombToBlow(Projectile projectile)
		{
			projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the rocket explodes on slopes.
			projectile.alpha = 255; // Make the rocket invisible.

			// Resize the hitbox of the projectile for the blast "radius".
			// Rocket I: 128, Rocket III: 200, Mini Nuke Rocket: 250
			// Measurements are in pixels, so 128 / 16 = 8 tiles.
			projectile.Resize(128, 128);
			// Set the knockback of the blast.
			// Rocket I: 8f, Rocket III: 10f, Mini Nuke Rocket: 12f
			projectile.knockBack = 8f;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

			// Resize the projectile again so the explosion dust and gore spawn from the middle.
			// Rocket I: 22, Rocket III: 80, Mini Nuke Rocket: 50
			Projectile.Resize(22, 22);

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

				Gore smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity += Vector2.One;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y += 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X += 1f;
				smokeGore.velocity.Y -= 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity -= Vector2.One;
			}
		}
	}
	public class SulfurGrenade : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.RocketsSkipDamageForPlayers[Type] = true;
		}

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
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				SulfurRocket.PrepareBombToBlow(Projectile);
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

			Projectile.ai[0]++;
			if (Projectile.ai[0] > 20f) // 15f
			{
				if (Projectile.velocity.Y == 0f)
				{
					Projectile.velocity.X *= 0.97f; // 0.95f
				}

				Projectile.velocity.Y += 0.2f; // 0.2f
			}
			Projectile.rotation += Projectile.velocity.X * 0.1f;

			// Explosives behave differently when touching Shimmer.
			if (Projectile.shimmerWet)
			{
				int projX = (int)(Projectile.Center.X / 16f);
				int projY = (int)(Projectile.position.Y / 16f);
				// If the projectile is inside of Shimmer:
				if (WorldGen.InWorld(projX, projY) && Main.tile[projX, projY] != null &&
						Main.tile[projX, projY].LiquidAmount == byte.MaxValue &&
						Main.tile[projX, projY].LiquidType == LiquidID.Shimmer &&
						WorldGen.InWorld(projX, projY - 1) && Main.tile[projX, projY - 1] != null &&
						Main.tile[projX, projY - 1].LiquidAmount > 0 &&
						Main.tile[projX, projY - 1].LiquidType == LiquidID.Shimmer)
				{
					Projectile.Kill(); // Kill the projectile with no blast radius.
				}
				// Otherwise, bounce off of the top of the Shimmer if traveling downwards.
				else if (Projectile.velocity.Y > 0f)
				{
					Projectile.velocity.Y *= -1f; // Reverse the Y velocity.
					Projectile.netUpdate = true; // Sync the change in multiplayer.
					if (Projectile.timeLeft > 600)
					{
						Projectile.timeLeft = 600; // Set the max time to 10 seconds (instead of the default 1 minute).
					}

					Projectile.timeLeft -= 60; // Subtract 1 second from the time left.
					Projectile.shimmerWet = false;
					Projectile.wet = false;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3; // Set the timeLeft to 3 so it can get ready to explode.
			}

			// Set the direction of the projectile so the knockback is always in the correct direction.
			if (target.position.X + (target.width / 2) < Projectile.position.X + (Projectile.width / 2))
			{
				Projectile.direction = -1;
			}
			else
			{
				Projectile.direction = 1;
			}
		}

		// This is only to make it so the grenade explodes when hitting a player in PVP. Otherwise the grenade will continue through the enemy player.
		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			if (modifiers.PvP && Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3; // Set the timeLeft to 3 so it can get ready to explode.
			}
			// Set the direction of the projectile so the knockback is always in the correct direction.
			if (target.position.X + (target.width / 2) < Projectile.position.X + (Projectile.width / 2))
			{
				Projectile.direction = -1;
			}
			else
			{
				Projectile.direction = 1;
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

			// Resize the projectile again so the explosion dust and gore spawn from the middle.
			// Rocket I: 22, Rocket III: 80, Mini Nuke Rocket: 50
			Projectile.Resize(22, 22);

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

				Gore smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity += Vector2.One;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y += 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X += 1f;
				smokeGore.velocity.Y -= 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity -= Vector2.One;
			}
		}
	}
	public class SulfurMine : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAMineThatDealsTripleDamageWhenStationary[Type] = true;
			ProjectileID.Sets.RocketsSkipDamageForPlayers[Type] = true;
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
				SulfurRocket.PrepareBombToBlow(Projectile);
			}
			else
			{
				if (Projectile.velocity.X > -0.2f && Projectile.velocity.X < 0.2f && Projectile.velocity.Y > -0.2f && Projectile.velocity.Y < 0.2f)
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

			// Explosives behave differently when touching Shimmer.
			if (Projectile.shimmerWet)
			{
				int projX = (int)(Projectile.Center.X / 16f);
				int projY = (int)(Projectile.position.Y / 16f);
				// If the projectile is inside of Shimmer:
				if (WorldGen.InWorld(projX, projY) && Main.tile[projX, projY] != null &&
						Main.tile[projX, projY].LiquidAmount == byte.MaxValue &&
						Main.tile[projX, projY].LiquidType == LiquidID.Shimmer &&
						WorldGen.InWorld(projX, projY - 1) && Main.tile[projX, projY - 1] != null &&
						Main.tile[projX, projY - 1].LiquidAmount > 0 &&
						Main.tile[projX, projY - 1].LiquidType == LiquidID.Shimmer)
				{
					Projectile.Kill(); // Kill the projectile with no blast radius.
				}
				// Otherwise, bounce off of the top of the Shimmer if traveling downwards.
				else if (Projectile.velocity.Y > 0f)
				{
					Projectile.velocity.Y *= -1f; // Reverse the Y velocity.
					Projectile.netUpdate = true; // Sync the change in multiplayer.
					if (Projectile.timeLeft > 600)
					{
						Projectile.timeLeft = 600; // Set the max time to 10 seconds (instead of the default 1 minute).
					}

					Projectile.timeLeft -= 60; // Subtract 1 second from the time left.
					Projectile.shimmerWet = false;
					Projectile.wet = false;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = oldVelocity.X * -0.6f; // -0.4f
			}

			if (Projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 0.7f)
			{
				Projectile.velocity.Y = oldVelocity.Y * -0.6f; // -0.4f
			}
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3; // Set the timeLeft to 3 so it can get ready to explode.
			}

			// Set the direction of the projectile so the knockback is always in the correct direction.
			if (target.position.X + (target.width / 2) < Projectile.position.X + (Projectile.width / 2))
			{
				Projectile.direction = -1;
			}
			else
			{
				Projectile.direction = 1;
			}
		}

		// This is only to make it so the mine explodes when hitting a player in PVP. Otherwise the mine will continue through the enemy player.
		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			if (modifiers.PvP && Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3; // Set the timeLeft to 3 so it can get ready to explode.
			}
			// Set the direction of the projectile so the knockback is always in the correct direction.
			if (target.position.X + (target.width / 2) < Projectile.position.X + (Projectile.width / 2))
			{
				Projectile.direction = -1;
			}
			else
			{
				Projectile.direction = 1;
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

			// Resize the projectile again so the explosion dust and gore spawn from the middle.
			// Rocket I: 22, Rocket III: 80, Mini Nuke Rocket: 50
			Projectile.Resize(22, 22);

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

				Gore smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity += Vector2.One;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y += 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X += 1f;
				smokeGore.velocity.Y -= 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity -= Vector2.One;
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
			ProjectileID.Sets.RocketsSkipDamageForPlayers[Type] = true;
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
				SulfurRocket.PrepareBombToBlow(Projectile);
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
				Projectile.ai[0]++;
				if (Projectile.ai[0] > 15f)
				{
					Projectile.ai[0] = 15f;
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
			}

			// Explosives behave differently when touching Shimmer.
			if (Projectile.shimmerWet)
			{
				int projX = (int)(Projectile.Center.X / 16f);
				int projY = (int)(Projectile.position.Y / 16f);
				// If the projectile is inside of Shimmer:
				if (WorldGen.InWorld(projX, projY) && Main.tile[projX, projY] != null &&
						Main.tile[projX, projY].LiquidAmount == byte.MaxValue &&
						Main.tile[projX, projY].LiquidType == LiquidID.Shimmer &&
						WorldGen.InWorld(projX, projY - 1) && Main.tile[projX, projY - 1] != null &&
						Main.tile[projX, projY - 1].LiquidAmount > 0 &&
						Main.tile[projX, projY - 1].LiquidType == LiquidID.Shimmer)
				{
					Projectile.Kill(); // Kill the projectile with no blast radius.
				}
				// Otherwise, bounce off of the top of the Shimmer if traveling downwards.
				else if (Projectile.velocity.Y > 0f)
				{
					Projectile.velocity.Y *= -1f; // Reverse the Y velocity.
					Projectile.netUpdate = true; // Sync the change in multiplayer.
					if (Projectile.timeLeft > 600)
					{
						Projectile.timeLeft = 600; // Set the max time to 10 seconds (instead of the default 1 minute).
					}

					Projectile.timeLeft -= 60; // Subtract 1 second from the time left.
					Projectile.shimmerWet = false;
					Projectile.wet = false;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3; // Set the timeLeft to 3 so it can get ready to explode.
			}

			// Set the direction of the projectile so the knockback is always in the correct direction.
			if (target.position.X + (target.width / 2) < Projectile.position.X + (Projectile.width / 2))
			{
				Projectile.direction = -1;
			}
			else
			{
				Projectile.direction = 1;
			}
		}

		// This is only to make it so the rocket explodes when hitting a player in PVP. Otherwise the rocket will continue through the enemy player.
		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 300);
			if (modifiers.PvP && Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3; // Set the timeLeft to 3 so it can get ready to explode.
			}
			// Set the direction of the projectile so the knockback is always in the correct direction.
			if (target.position.X + (target.width / 2) < Projectile.position.X + (Projectile.width / 2))
			{
				Projectile.direction = -1;
			}
			else
			{
				Projectile.direction = 1;
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

			// Resize the projectile again so the explosion dust and gore spawn from the middle.
			// Rocket I: 22, Rocket III: 80, Mini Nuke Rocket: 50
			Projectile.Resize(22, 22);

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

				Gore smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity += Vector2.One;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y += 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X += 1f;
				smokeGore.velocity.Y -= 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity -= Vector2.One;
			}
		}
	}
}