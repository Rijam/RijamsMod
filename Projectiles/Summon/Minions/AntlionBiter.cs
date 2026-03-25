using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Minions
{
	/* Taken from ExampleMod ExampleSimpleMinion
	 * projectile minion shows a few mandatory things that make it behave properly. 
	 * Its attack pattern is simple: If an enemy is in range of 43 tiles, it will fly to it and deal contact damage
	 * If the player targets a certain NPC with right-click, it will fly through tiles to it
	 * If it isn't attacking, it will float near the player with minimal movement
	 */
	public class AntlionBiter : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Antlion Biter");
			// Sets the amount of frames projectile minion has on its spritesheet
			Main.projFrames[Projectile.type] = 15;
			// projectile is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			// These below are needed for a minion
			// Denotes that projectile projectile is a pet or minion
			Main.projPet[Projectile.type] = true;
			// projectile is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			// Don't mistake projectile with "if projectile is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
		}

		public sealed override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.OneEyedPirate);
			Projectile.width = 24;
			Projectile.height = 24;
			// Makes the minion go through tiles freely
			Projectile.tileCollide = true;

			// These below are needed for a minion weapon
			// Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.friendly = true;
			// Only determines the damage type
			Projectile.minion = true;
			// Declares the damage type (needed for it to deal damage)
			Projectile.DamageType = DamageClass.Summon;
			// Amount of slots projectile minion occupies from the total minion slots available to the player (more on that later)
			Projectile.minionSlots = 1f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			//AIType = ProjectileID.OneEyedPirate;
			//AIType = 0;

			// Using local NPC immunity allows each to strike independently from one another.
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 13;

			DrawOffsetX = -12;
			DrawOriginOffsetY -= 14;
		}

		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// projectile is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return true;
		}

        public override void AI()
		{
			Player player = Main.player[Projectile.owner];


			#region Active check
			// projectile is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active) {
				//player.ClearBuff(ModContent.BuffType<AntlionBiterBuff>());
				player.ClearBuff(ModContent.BuffType<Buffs.Minions.AntlionBiterBuff>());
			}
			//if (player.HasBuff(ModContent.BuffType<AntlionBiterBuff>()))
			if (player.HasBuff(ModContent.BuffType<Buffs.Minions.AntlionBiterBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			#endregion
			//Copied from AIStyle 67

			

			if (!player.active)
			{
				Projectile.active = false;
				return;
			}
			Vector2 followPlayerDistance = player.Center;
			followPlayerDistance.X -= (15 + player.width / 2) * player.direction;
			followPlayerDistance.X -= Projectile.minionPos * 40 * player.direction;
			int targetWhoAmI = -1;
			float maxDistToTarget = 800f;
			int num674 = 15;
			if (Projectile.ai[0] == 0f)
			{
				NPC ownerMinionAttackTargetNPC4 = Projectile.OwnerMinionAttackTargetNPC;
				if (ownerMinionAttackTargetNPC4 != null && ownerMinionAttackTargetNPC4.CanBeChasedBy(Projectile))
				{
					float distToTarget = (ownerMinionAttackTargetNPC4.Center - Projectile.Center).Length();
					if (distToTarget < maxDistToTarget)
					{
						targetWhoAmI = ownerMinionAttackTargetNPC4.whoAmI;
						maxDistToTarget = distToTarget;
					}
				}
				if (targetWhoAmI < 0)
				{
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC nPC3 = Main.npc[i];
						if (nPC3.CanBeChasedBy(Projectile))
						{
							float distToTarget = (nPC3.Center - Projectile.Center).Length();
							if (distToTarget < maxDistToTarget)
							{
								targetWhoAmI = i;
								maxDistToTarget = distToTarget;
							}
						}
					}
				}
			}
			if (Projectile.ai[0] == 1f)
			{
				Projectile.tileCollide = false;
				float num678 = 0.2f;
				float num679 = 10f;
				int num680 = 200;
				if (num679 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
				{
					num679 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
				}
				Vector2 distToPlayer = player.Center - Projectile.Center;
				float distToPlayerLength = distToPlayer.Length();
				if (distToPlayerLength > 2000f)
				{
					Projectile.position = player.Center - new Vector2(Projectile.width, Projectile.height) / 2f;
				}
				if (distToPlayerLength < (float)num680 && player.velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= player.position.Y + (float)player.height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					Projectile.netUpdate = true;
					if (Projectile.velocity.Y < -6f)
					{
						Projectile.velocity.Y = -6f;
					}
				}
				if (!(distToPlayerLength < 60f))
				{
					distToPlayer.Normalize();
					distToPlayer *= num679;
					if (Projectile.velocity.X < distToPlayer.X)
					{
						Projectile.velocity.X += num678;
						if (Projectile.velocity.X < 0f)
						{
							Projectile.velocity.X += num678 * 1.5f;
						}
					}
					if (Projectile.velocity.X > distToPlayer.X)
					{
						Projectile.velocity.X -= num678;
						if (Projectile.velocity.X > 0f)
						{
							Projectile.velocity.X -= num678 * 1.5f;
						}
					}
					if (Projectile.velocity.Y < distToPlayer.Y)
					{
						Projectile.velocity.Y += num678;
						if (Projectile.velocity.Y < 0f)
						{
							Projectile.velocity.Y += num678 * 1.5f;
						}
					}
					if (Projectile.velocity.Y > distToPlayer.Y)
					{
						Projectile.velocity.Y -= num678;
						if (Projectile.velocity.Y > 0f)
						{
							Projectile.velocity.Y -= num678 * 1.5f;
						}
					}
				}
				if (Projectile.velocity.X != 0f)
				{
					Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
				}
				Projectile.frameCounter++;
				if (Projectile.frameCounter > 3)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
				}
				if ((Projectile.frame < 10) | (Projectile.frame > 13))
				{
					Projectile.frame = 10;
				}
				Projectile.rotation = Projectile.velocity.X * 0.1f;
			}
			if (Projectile.ai[0] == 2f)
			{
				Projectile.friendly = true;
				Projectile.spriteDirection = Projectile.direction;
				Projectile.rotation = 0f;
				Projectile.frame = 4 + (int)((float)num674 - Projectile.ai[1]) / (num674 / 3);
				if (Projectile.velocity.Y != 0f)
				{
					Projectile.frame += 3;
				}
				Projectile.velocity.Y += 0.4f;
				if (Projectile.velocity.Y > 10f)
				{
					Projectile.velocity.Y = 10f;
				}
				Projectile.ai[1]--;
				if (Projectile.ai[1] <= 0f)
				{
					Projectile.ai[1] = 0f;
					Projectile.ai[0] = 0f;
					Projectile.friendly = false;
					Projectile.netUpdate = true;
					return;
				}
			}
			if (targetWhoAmI >= 0)
			{
				float num682 = 700f;
				float num683 = 20f;
				if ((double)Projectile.position.Y > Main.worldSurface * 16.0)
				{
					num682 *= 0.7f;
				}
				NPC nPC4 = Main.npc[targetWhoAmI];
				Vector2 targetCenter = nPC4.Center;
				float distToTarget = (targetCenter - Projectile.Center).Length();
				if (distToTarget < num682)
				{
					followPlayerDistance = targetCenter;
					if (targetCenter.Y < Projectile.Center.Y - 30f && Projectile.velocity.Y == 0f)
					{
						float absDistToTarget = Math.Abs(targetCenter.Y - Projectile.Center.Y);
						if (absDistToTarget < 120f)
						{
							Projectile.velocity.Y = -10f;
						}
						else if (absDistToTarget < 210f)
						{
							Projectile.velocity.Y = -13f;
						}
						else if (absDistToTarget < 270f)
						{
							Projectile.velocity.Y = -15f;
						}
						else if (absDistToTarget < 310f)
						{
							Projectile.velocity.Y = -17f;
						}
						else if (absDistToTarget < 380f)
						{
							Projectile.velocity.Y = -18f;
						}
					}
				}
				if (distToTarget < num683)
				{
					Projectile.ai[0] = 2f;
					Projectile.ai[1] = num674;
					Projectile.netUpdate = true;
				}
			}
			if (Projectile.ai[0] == 0f && targetWhoAmI < 0)
			{
				float num686 = 500f;
				if (Main.player[Projectile.owner].rocketDelay2 > 0)
				{
					Projectile.ai[0] = 1f;
					Projectile.netUpdate = true;
				}
				Vector2 distToPlayer = player.Center - Projectile.Center;
				if (distToPlayer.Length() > 2000f)
				{
					Projectile.position = player.Center - new Vector2(Projectile.width, Projectile.height) / 2f;
				}
				else if (distToPlayer.Length() > num686 || Math.Abs(distToPlayer.Y) > 300f)
				{
					Projectile.ai[0] = 1f;
					Projectile.netUpdate = true;
					if (Projectile.velocity.Y > 0f && distToPlayer.Y < 0f)
					{
						Projectile.velocity.Y = 0f;
					}
					if (Projectile.velocity.Y < 0f && distToPlayer.Y > 0f)
					{
						Projectile.velocity.Y = 0f;
					}
				}
			}
			if (Projectile.ai[0] == 0f)
			{
				Projectile.tileCollide = true;
				float numIs05f = 0.5f;
				float num688 = 4f;
				float num689 = 4f;
				float num690 = 0.1f;
				if (num689 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
				{
					num689 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
					numIs05f = 0.7f;
				}
				int directionToChangeTo = 0;
				bool solidTile = false;
				float distToFollowPlayerDistance = followPlayerDistance.X - Projectile.Center.X;
				if (Math.Abs(distToFollowPlayerDistance) > 5f)
				{
					if (distToFollowPlayerDistance < 0f)
					{
						directionToChangeTo = -1;
						if (Projectile.velocity.X > 0f - num688)
						{
							Projectile.velocity.X -= numIs05f;
						}
						else
						{
							Projectile.velocity.X -= num690;
						}
					}
					else
					{
						directionToChangeTo = 1;
						if (Projectile.velocity.X < num688)
						{
							Projectile.velocity.X += numIs05f;
						}
						else
						{
							Projectile.velocity.X += num690;
						}
					}
				}
				else
				{
					Projectile.velocity.X *= 0.9f;
					if (Math.Abs(Projectile.velocity.X) < numIs05f * 2f)
					{
						Projectile.velocity.X = 0f;
					}
				}
				if (directionToChangeTo != 0)
				{
					int tilePosX = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
					int tilePosY = (int)Projectile.position.Y / 16;
					tilePosX += directionToChangeTo;
					tilePosX += (int)Projectile.velocity.X;
					for (int i = tilePosY; i < tilePosY + Projectile.height / 16 + 1; i++)
					{
						if (WorldGen.SolidTile(tilePosX, i))
						{
							solidTile = true;
						}
					}
				}
				Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
				if (Projectile.velocity.Y == 0f && solidTile)
				{
					for (int i = 0; i < 3; i++)
					{
						int tilePosX = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
						if (i == 0)
						{
							tilePosX = (int)Projectile.position.X / 16;
						}
						if (i == 2)
						{
							tilePosX = (int)(Projectile.position.X + (float)Projectile.width) / 16;
						}
						int tilePosY = (int)(Projectile.position.Y + (float)Projectile.height) / 16;
						if (!WorldGen.SolidTile(tilePosX, tilePosY) && !Main.tile[tilePosX, tilePosY].IsHalfBlock && Main.tile[tilePosX, tilePosY].Slope <= 0 && (!TileID.Sets.Platforms[Main.tile[tilePosX, tilePosY].TileType] || !Main.tile[tilePosX, tilePosY].HasTile || Main.tile[tilePosX, tilePosY].IsActuated))
						{
							continue;
						}
						try
						{
							tilePosX = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
							tilePosY = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
							tilePosX += directionToChangeTo;
							tilePosX += (int)Projectile.velocity.X;
							if (!WorldGen.SolidTile(tilePosX, tilePosY - 1) && !WorldGen.SolidTile(tilePosX, tilePosY - 2))
							{
								Projectile.velocity.Y = -5.1f;
							}
							else if (!WorldGen.SolidTile(tilePosX, tilePosY - 2))
							{
								Projectile.velocity.Y = -7.1f;
							}
							else if (WorldGen.SolidTile(tilePosX, tilePosY - 5))
							{
								Projectile.velocity.Y = -11.1f;
							}
							else if (WorldGen.SolidTile(tilePosX, tilePosY - 4))
							{
								Projectile.velocity.Y = -10.1f;
							}
							else
							{
								Projectile.velocity.Y = -9.1f;
							}
						}
						catch
						{
							Projectile.velocity.Y = -9.1f;
						}
					}
				}
				if (Projectile.velocity.X > num689)
				{
					Projectile.velocity.X = num689;
				}
				if (Projectile.velocity.X < 0f - num689)
				{
					Projectile.velocity.X = 0f - num689;
				}
				if (Projectile.velocity.X < 0f)
				{
					Projectile.direction = -1;
				}
				if (Projectile.velocity.X > 0f)
				{
					Projectile.direction = 1;
				}
				if (Projectile.velocity.X > numIs05f && directionToChangeTo == 1)
				{
					Projectile.direction = 1;
				}
				if (Projectile.velocity.X < 0f - numIs05f && directionToChangeTo == -1)
				{
					Projectile.direction = -1;
				}
				if (Projectile.velocity.X == 0f && targetWhoAmI == -1)
				{
					Projectile.direction = player.direction;
				}
				Projectile.spriteDirection = Projectile.direction;
				Projectile.rotation = 0f;
				if (Projectile.velocity.Y == 0f)
				{
					if (Projectile.velocity.X == 0f)
					{
						Projectile.frame = 0;
						Projectile.frameCounter = 0;
					}
					else if (Math.Abs(Projectile.velocity.X) >= 0.5f)
					{
						Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 10)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame >= 4)
						{
							Projectile.frame = 0;
						}
					}
					else
					{
						Projectile.frame = 0;
						Projectile.frameCounter = 0;
					}
				}
				else if (Projectile.velocity.Y != 0f)
				{
					Projectile.frameCounter = 0;
					Projectile.frame = 14;
				}
				Projectile.velocity.Y += 0.4f;
				if (Projectile.velocity.Y > 10f)
				{
					Projectile.velocity.Y = 10f;
				}
			}
		}
	}
}