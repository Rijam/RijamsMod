using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
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
			DisplayName.SetDefault("Antlion Biter");
			// Sets the amount of frames projectile minion has on its spritesheet
			Main.projFrames[projectile.type] = 15;
			// projectile is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

			// These below are needed for a minion
			// Denotes that projectile projectile is a pet or minion
			Main.projPet[projectile.type] = true;
			// projectile is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			// Don't mistake projectile with "if projectile is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = false;
		}

		public sealed override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.OneEyedPirate);
			projectile.width = 24;
			projectile.height = 24;
			// Makes the minion go through tiles freely
			projectile.tileCollide = true;

			// These below are needed for a minion weapon
			// Only controls if it deals damage to enemies on contact (more on that later)
			projectile.friendly = true;
			// Only determines the damage type
			projectile.minion = true;
			// Amount of slots projectile minion occupies from the total minion slots available to the player (more on that later)
			projectile.minionSlots = 1f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			projectile.penetrate = -1;
			projectile.netImportant = true;
			//aiType = ProjectileID.OneEyedPirate;
			//aiType = 0;

			// Using local NPC immunity allows each to strike independently from one another.
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;

			drawOffsetX = -12;
			drawOriginOffsetY -= 7;
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
		}

        public override void AI()
		{
			Player player = Main.player[projectile.owner];


			#region Active check
			// projectile is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active) {
				//player.ClearBuff(ModContent.BuffType<AntlionBiterBuff>());
				player.ClearBuff(mod.BuffType("AntlionBiterBuff"));
			}
			//if (player.HasBuff(ModContent.BuffType<AntlionBiterBuff>()))
			if (player.HasBuff(mod.BuffType("AntlionBiterBuff")))
			{
				projectile.timeLeft = 2;
			}
			#endregion
			//Copied from AIStyle 67

			if (true)
			{
				Player player6 = Main.player[projectile.owner];
				if (!player6.active)
				{
					projectile.active = false;
					return;
				}
				bool flag26 = true;
				Vector2 vector62 = player6.Center;
				if (flag26)
				{
					vector62.X -= (15 + player6.width / 2) * player6.direction;
					vector62.X -= projectile.minionPos * 40 * player6.direction;
				}
				bool flag27 = true;
				int num672 = -1;
				float num673 = 450f;
				if (flag26)
				{
					num673 = 800f;
				}
				int num674 = 15;
				if (projectile.ai[0] == 0f && flag27)
				{
					NPC ownerMinionAttackTargetNPC4 = projectile.OwnerMinionAttackTargetNPC;
					if (ownerMinionAttackTargetNPC4 != null && ownerMinionAttackTargetNPC4.CanBeChasedBy(projectile))
					{
						float num675 = (ownerMinionAttackTargetNPC4.Center - projectile.Center).Length();
						if (num675 < num673)
						{
							num672 = ownerMinionAttackTargetNPC4.whoAmI;
							num673 = num675;
						}
					}
					if (num672 < 0)
					{
						for (int num676 = 0; num676 < 200; num676++)
						{
							NPC nPC3 = Main.npc[num676];
							if (nPC3.CanBeChasedBy(projectile))
							{
								float num677 = (nPC3.Center - projectile.Center).Length();
								if (num677 < num673)
								{
									num672 = num676;
									num673 = num677;
								}
							}
						}
					}
				}
				if (projectile.ai[0] == 1f)
				{
					projectile.tileCollide = false;
					float num678 = 0.2f;
					float num679 = 10f;
					int num680 = 200;
					if (num679 < Math.Abs(player6.velocity.X) + Math.Abs(player6.velocity.Y))
					{
						num679 = Math.Abs(player6.velocity.X) + Math.Abs(player6.velocity.Y);
					}
					Vector2 vector66 = player6.Center - projectile.Center;
					float num681 = vector66.Length();
					if (num681 > 2000f)
					{
						projectile.position = player6.Center - new Vector2(projectile.width, projectile.height) / 2f;
					}
					if (num681 < (float)num680 && player6.velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= player6.position.Y + (float)player6.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
					{
						projectile.ai[0] = 0f;
						projectile.netUpdate = true;
						if (projectile.velocity.Y < -6f)
						{
							projectile.velocity.Y = -6f;
						}
					}
					if (!(num681 < 60f))
					{
						vector66.Normalize();
						vector66 *= num679;
						if (projectile.velocity.X < vector66.X)
						{
							projectile.velocity.X += num678;
							if (projectile.velocity.X < 0f)
							{
								projectile.velocity.X += num678 * 1.5f;
							}
						}
						if (projectile.velocity.X > vector66.X)
						{
							projectile.velocity.X -= num678;
							if (projectile.velocity.X > 0f)
							{
								projectile.velocity.X -= num678 * 1.5f;
							}
						}
						if (projectile.velocity.Y < vector66.Y)
						{
							projectile.velocity.Y += num678;
							if (projectile.velocity.Y < 0f)
							{
								projectile.velocity.Y += num678 * 1.5f;
							}
						}
						if (projectile.velocity.Y > vector66.Y)
						{
							projectile.velocity.Y -= num678;
							if (projectile.velocity.Y > 0f)
							{
								projectile.velocity.Y -= num678 * 1.5f;
							}
						}
					}
					if (projectile.velocity.X != 0f)
					{
						projectile.spriteDirection = Math.Sign(projectile.velocity.X);
					}
					if (flag26)
					{
						projectile.frameCounter++;
						if (projectile.frameCounter > 3)
						{
							projectile.frame++;
							projectile.frameCounter = 0;
						}
						if ((projectile.frame < 10) | (projectile.frame > 13))
						{
							projectile.frame = 10;
						}
						projectile.rotation = projectile.velocity.X * 0.1f;
					}
				}
				if (projectile.ai[0] == 2f)
				{
					projectile.friendly = true;
					projectile.spriteDirection = projectile.direction;
					projectile.rotation = 0f;
					projectile.frame = 4 + (int)((float)num674 - projectile.ai[1]) / (num674 / 3);
					if (projectile.velocity.Y != 0f)
					{
						projectile.frame += 3;
					}
					projectile.velocity.Y += 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
					projectile.ai[1]--;
					if (projectile.ai[1] <= 0f)
					{
						projectile.ai[1] = 0f;
						projectile.ai[0] = 0f;
						projectile.friendly = false;
						projectile.netUpdate = true;
						return;
					}
				}
				if (num672 >= 0)
				{
					float num682 = 400f;
					float num683 = 20f;
					if (flag26)
					{
						num682 = 700f;
					}
					if ((double)projectile.position.Y > Main.worldSurface * 16.0)
					{
						num682 *= 0.7f;
					}
					NPC nPC4 = Main.npc[num672];
					Vector2 center5 = nPC4.Center;
					float num684 = (center5 - projectile.Center).Length();
					if (num684 < num682)
					{
						vector62 = center5;
						if (center5.Y < projectile.Center.Y - 30f && projectile.velocity.Y == 0f)
						{
							float num685 = Math.Abs(center5.Y - projectile.Center.Y);
							if (num685 < 120f)
							{
								projectile.velocity.Y = -10f;
							}
							else if (num685 < 210f)
							{
								projectile.velocity.Y = -13f;
							}
							else if (num685 < 270f)
							{
								projectile.velocity.Y = -15f;
							}
							else if (num685 < 310f)
							{
								projectile.velocity.Y = -17f;
							}
							else if (num685 < 380f)
							{
								projectile.velocity.Y = -18f;
							}
						}
					}
					if (num684 < num683)
					{
						projectile.ai[0] = 2f;
						projectile.ai[1] = num674;
						projectile.netUpdate = true;
					}
				}
				if (projectile.ai[0] == 0f && num672 < 0)
				{
					float num686 = 500f;
					if (projectile.type == 500)
					{
						num686 = 200f;
					}
					if (projectile.type == 653)
					{
						num686 = 170f;
					}
					if (Main.player[projectile.owner].rocketDelay2 > 0)
					{
						projectile.ai[0] = 1f;
						projectile.netUpdate = true;
					}
					Vector2 vector67 = player6.Center - projectile.Center;
					if (vector67.Length() > 2000f)
					{
						projectile.position = player6.Center - new Vector2(projectile.width, projectile.height) / 2f;
					}
					else if (vector67.Length() > num686 || Math.Abs(vector67.Y) > 300f)
					{
						projectile.ai[0] = 1f;
						projectile.netUpdate = true;
						if (projectile.velocity.Y > 0f && vector67.Y < 0f)
						{
							projectile.velocity.Y = 0f;
						}
						if (projectile.velocity.Y < 0f && vector67.Y > 0f)
						{
							projectile.velocity.Y = 0f;
						}
					}
				}
				if (projectile.ai[0] == 0f)
				{
					projectile.tileCollide = true;
					float num687 = 0.5f;
					float num688 = 4f;
					float num689 = 4f;
					float num690 = 0.1f;
					if (num689 < Math.Abs(player6.velocity.X) + Math.Abs(player6.velocity.Y))
					{
						num689 = Math.Abs(player6.velocity.X) + Math.Abs(player6.velocity.Y);
						num687 = 0.7f;
					}
					int num691 = 0;
					bool flag29 = false;
					float num692 = vector62.X - projectile.Center.X;
					if (Math.Abs(num692) > 5f)
					{
						if (num692 < 0f)
						{
							num691 = -1;
							if (projectile.velocity.X > 0f - num688)
							{
								projectile.velocity.X -= num687;
							}
							else
							{
								projectile.velocity.X -= num690;
							}
						}
						else
						{
							num691 = 1;
							if (projectile.velocity.X < num688)
							{
								projectile.velocity.X += num687;
							}
							else
							{
								projectile.velocity.X += num690;
							}
						}
					}
					else
					{
						projectile.velocity.X *= 0.9f;
						if (Math.Abs(projectile.velocity.X) < num687 * 2f)
						{
							projectile.velocity.X = 0f;
						}
					}
					if (num691 != 0)
					{
						int num693 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
						int num694 = (int)projectile.position.Y / 16;
						num693 += num691;
						num693 += (int)projectile.velocity.X;
						for (int num695 = num694; num695 < num694 + projectile.height / 16 + 1; num695++)
						{
							if (WorldGen.SolidTile(num693, num695))
							{
								flag29 = true;
							}
						}
					}
					Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
					if (projectile.velocity.Y == 0f && flag29)
					{
						for (int num696 = 0; num696 < 3; num696++)
						{
							int num697 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
							if (num696 == 0)
							{
								num697 = (int)projectile.position.X / 16;
							}
							if (num696 == 2)
							{
								num697 = (int)(projectile.position.X + (float)projectile.width) / 16;
							}
							int num698 = (int)(projectile.position.Y + (float)projectile.height) / 16;
							if (!WorldGen.SolidTile(num697, num698) && !Main.tile[num697, num698].halfBrick() && Main.tile[num697, num698].slope() <= 0 && (!TileID.Sets.Platforms[Main.tile[num697, num698].type] || !Main.tile[num697, num698].active() || Main.tile[num697, num698].inActive()))
							{
								continue;
							}
							try
							{
								num697 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
								num698 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
								num697 += num691;
								num697 += (int)projectile.velocity.X;
								if (!WorldGen.SolidTile(num697, num698 - 1) && !WorldGen.SolidTile(num697, num698 - 2))
								{
									projectile.velocity.Y = -5.1f;
								}
								else if (!WorldGen.SolidTile(num697, num698 - 2))
								{
									projectile.velocity.Y = -7.1f;
								}
								else if (WorldGen.SolidTile(num697, num698 - 5))
								{
									projectile.velocity.Y = -11.1f;
								}
								else if (WorldGen.SolidTile(num697, num698 - 4))
								{
									projectile.velocity.Y = -10.1f;
								}
								else
								{
									projectile.velocity.Y = -9.1f;
								}
							}
							catch
							{
								projectile.velocity.Y = -9.1f;
							}
						}
					}
					if (projectile.velocity.X > num689)
					{
						projectile.velocity.X = num689;
					}
					if (projectile.velocity.X < 0f - num689)
					{
						projectile.velocity.X = 0f - num689;
					}
					if (projectile.velocity.X < 0f)
					{
						projectile.direction = -1;
					}
					if (projectile.velocity.X > 0f)
					{
						projectile.direction = 1;
					}
					if (projectile.velocity.X > num687 && num691 == 1)
					{
						projectile.direction = 1;
					}
					if (projectile.velocity.X < 0f - num687 && num691 == -1)
					{
						projectile.direction = -1;
					}
					projectile.spriteDirection = projectile.direction;
					if (flag26)
					{
						projectile.rotation = 0f;
						if (projectile.velocity.Y == 0f)
						{
							if (projectile.velocity.X == 0f)
							{
								projectile.frame = 0;
								projectile.frameCounter = 0;
							}
							else if (Math.Abs(projectile.velocity.X) >= 0.5f)
							{
								projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
								projectile.frameCounter++;
								if (projectile.frameCounter > 10)
								{
									projectile.frame++;
									projectile.frameCounter = 0;
								}
								if (projectile.frame >= 4)
								{
									projectile.frame = 0;
								}
							}
							else
							{
								projectile.frame = 0;
								projectile.frameCounter = 0;
							}
						}
						else if (projectile.velocity.Y != 0f)
						{
							projectile.frameCounter = 0;
							projectile.frame = 14;
						}
					}
					projectile.velocity.Y += 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
			}
		}
	}
}