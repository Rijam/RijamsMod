using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Buffs.Debuffs;

namespace RijamsMod.Projectiles.Ranged
{
	public class TAGProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;
			// This set handles some things for us already:
			// Sets the timeLeft to 3 and the projectile direction when colliding with an NPC or player in PVP (so the explosive can detonate).
			// Explosives also bounce off the top of Shimmer, detonate with no blast damage when touching the bottom or sides of Shimmer, and damage other players in For the Worthy worlds.
			ProjectileID.Sets.Explosive[Type] = true;

			ProjectileID.Sets.RocketsSkipDamageForPlayers[Type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1; // 16
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 600; // 10 seconds
			Projectile.tileCollide = false;
			DrawOriginOffsetY -= 8;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			// Projectile.velocity = Vector2.Zero;
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Tagged>(), 1200);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<Tagged>(), 1200);
		}

		public override void AI()
		{
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				Projectile.PrepareBombToBlow(); // Get ready to explode.
			}
			
			try
			{
				int projTilePosX = (int)(Projectile.position.X / 16f) - 1;
				int projTileCenterX = (int)(Projectile.Center.X / 16f) + 2;
				int projTilePosY = (int)(Projectile.position.Y / 16f) - 1;
				int projTileCenterY = (int)(Projectile.Center.Y / 16f) + 2;

				Utils.ClampWithinWorld(ref projTilePosX, ref projTilePosY, ref projTileCenterX, ref projTileCenterY);

				Vector2 vector = default;
				for (int j = projTilePosX; j < projTileCenterX; j++)
				{
					for (int k = projTilePosY; k < projTileCenterY; k++)
					{
						if (Main.tile[j, k] == null || !Main.tile[j, k].HasUnactuatedTile || !Main.tileSolid[Main.tile[j, k].TileType] || Main.tileSolidTop[Main.tile[j, k].TileType])
						{
							continue;
						}

						vector.X = j * 16;
						vector.Y = k * 16;
						if (!(Projectile.position.X + (float)Projectile.width - 4f > vector.X) || !(Projectile.position.X + 4f < vector.X + 16f) || !(Projectile.position.Y + (float)Projectile.height - 4f > vector.Y) || !(Projectile.position.Y + 4f < vector.Y + 16f))
						{
							continue;
						}

						Projectile.velocity.X = 0f;
						Projectile.velocity.Y = -0.2f;
						Projectile.ai[0]++;
					}
				}
			}
			catch
			{
			}
			if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
			{
				Projectile.velocity.X *= 0.97f;

				if (Projectile.velocity.X > -0.01f && Projectile.velocity.X < 0.01f)
				{
					Projectile.velocity.X = 0f;
					Projectile.netUpdate = true;
				}
			}

			Projectile.velocity.Y += 0.2f;
			Projectile.rotation += Projectile.velocity.X * 0.05f;

			if (Projectile.ai[0] <= 5f && Projectile.ai[0] > 0)
			{
				Projectile.timeLeft = 180;
				if (Projectile.soundDelay == 0)
				{
					SoundEngine.PlaySound(SoundID.Item4, Projectile.position);
				}
				Projectile.soundDelay = 10;
			}
			// Main.NewText($"Projectile.ai[0] {Projectile.ai[0]} Projectile.timeLeft {Projectile.timeLeft}");
		}
		public override void PrepareBombToBlow()
		{
			Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the rocket explodes on slopes.
			Projectile.alpha = 255; // Make the rocket invisible.

			Projectile.Resize(480, 480); // 30 tile range
		}
		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

			// Resize the projectile again so the explosion dust and gore spawn from the middle.
			// Rocket I: 22, Rocket III: 80, Mini Nuke Rocket: 50
			Projectile.Resize(16, 16);

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