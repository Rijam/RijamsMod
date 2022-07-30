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
			DisplayName.SetDefault("Antlion Biter");
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
			Projectile.localNPCHitCooldown = 30;

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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
		}

        public override void AI()
		{
			Player player = Main.player[Projectile.owner];


			#region Active check
			// projectile is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active) {
				//player.ClearBuff(ModContent.BuffType<AntlionBiterBuff>());
				player.ClearBuff(ModContent.BuffType<Buffs.AntlionBiterBuff>());
			}
			//if (player.HasBuff(ModContent.BuffType<AntlionBiterBuff>()))
			if (player.HasBuff(ModContent.BuffType<Buffs.AntlionBiterBuff>()))
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
			bool flag26 = true;
			Vector2 vector62 = player.Center;
			if (flag26)
			{
				vector62.X -= (15 + player.width / 2) * player.direction;
				vector62.X -= Projectile.minionPos * 40 * player.direction;
			}
			bool flag27 = true;
			int num672 = -1;
			float num673 = 450f;
			if (flag26)
			{
				num673 = 800f;
			}
			int num674 = 15;
			if (Projectile.ai[0] == 0f && flag27)
			{
				NPC ownerMinionAttackTargetNPC4 = Projectile.OwnerMinionAttackTargetNPC;
				if (ownerMinionAttackTargetNPC4 != null && ownerMinionAttackTargetNPC4.CanBeChasedBy(Projectile))
				{
					float num675 = (ownerMinionAttackTargetNPC4.Center - Projectile.Center).Length();
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
						if (nPC3.CanBeChasedBy(Projectile))
						{
							float num677 = (nPC3.Center - Projectile.Center).Length();
							if (num677 < num673)
							{
								num672 = num676;
								num673 = num677;
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
				Vector2 vector66 = player.Center - Projectile.Center;
				float num681 = vector66.Length();
				if (num681 > 2000f)
				{
					Projectile.position = player.Center - new Vector2(Projectile.width, Projectile.height) / 2f;
				}
				if (num681 < (float)num680 && player.velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= player.position.Y + (float)player.height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					Projectile.netUpdate = true;
					if (Projectile.velocity.Y < -6f)
					{
						Projectile.velocity.Y = -6f;
					}
				}
				if (!(num681 < 60f))
				{
					vector66.Normalize();
					vector66 *= num679;
					if (Projectile.velocity.X < vector66.X)
					{
						Projectile.velocity.X += num678;
						if (Projectile.velocity.X < 0f)
						{
							Projectile.velocity.X += num678 * 1.5f;
						}
					}
					if (Projectile.velocity.X > vector66.X)
					{
						Projectile.velocity.X -= num678;
						if (Projectile.velocity.X > 0f)
						{
							Projectile.velocity.X -= num678 * 1.5f;
						}
					}
					if (Projectile.velocity.Y < vector66.Y)
					{
						Projectile.velocity.Y += num678;
						if (Projectile.velocity.Y < 0f)
						{
							Projectile.velocity.Y += num678 * 1.5f;
						}
					}
					if (Projectile.velocity.Y > vector66.Y)
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
				if (flag26)
				{
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
			if (num672 >= 0)
			{
				float num682 = 400f;
				float num683 = 20f;
				if (flag26)
				{
					num682 = 700f;
				}
				if ((double)Projectile.position.Y > Main.worldSurface * 16.0)
				{
					num682 *= 0.7f;
				}
				NPC nPC4 = Main.npc[num672];
				Vector2 center5 = nPC4.Center;
				float num684 = (center5 - Projectile.Center).Length();
				if (num684 < num682)
				{
					vector62 = center5;
					if (center5.Y < Projectile.Center.Y - 30f && Projectile.velocity.Y == 0f)
					{
						float num685 = Math.Abs(center5.Y - Projectile.Center.Y);
						if (num685 < 120f)
						{
							Projectile.velocity.Y = -10f;
						}
						else if (num685 < 210f)
						{
							Projectile.velocity.Y = -13f;
						}
						else if (num685 < 270f)
						{
							Projectile.velocity.Y = -15f;
						}
						else if (num685 < 310f)
						{
							Projectile.velocity.Y = -17f;
						}
						else if (num685 < 380f)
						{
							Projectile.velocity.Y = -18f;
						}
					}
				}
				if (num684 < num683)
				{
					Projectile.ai[0] = 2f;
					Projectile.ai[1] = num674;
					Projectile.netUpdate = true;
				}
			}
			if (Projectile.ai[0] == 0f && num672 < 0)
			{
				float num686 = 500f;
				if (Projectile.type == 500)
				{
					num686 = 200f;
				}
				if (Projectile.type == 653)
				{
					num686 = 170f;
				}
				if (Main.player[Projectile.owner].rocketDelay2 > 0)
				{
					Projectile.ai[0] = 1f;
					Projectile.netUpdate = true;
				}
				Vector2 vector67 = player.Center - Projectile.Center;
				if (vector67.Length() > 2000f)
				{
					Projectile.position = player.Center - new Vector2(Projectile.width, Projectile.height) / 2f;
				}
				else if (vector67.Length() > num686 || Math.Abs(vector67.Y) > 300f)
				{
					Projectile.ai[0] = 1f;
					Projectile.netUpdate = true;
					if (Projectile.velocity.Y > 0f && vector67.Y < 0f)
					{
						Projectile.velocity.Y = 0f;
					}
					if (Projectile.velocity.Y < 0f && vector67.Y > 0f)
					{
						Projectile.velocity.Y = 0f;
					}
				}
			}
			if (Projectile.ai[0] == 0f)
			{
				Projectile.tileCollide = true;
				float num687 = 0.5f;
				float num688 = 4f;
				float num689 = 4f;
				float num690 = 0.1f;
				if (num689 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
				{
					num689 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
					num687 = 0.7f;
				}
				int num691 = 0;
				bool flag29 = false;
				float num692 = vector62.X - Projectile.Center.X;
				if (Math.Abs(num692) > 5f)
				{
					if (num692 < 0f)
					{
						num691 = -1;
						if (Projectile.velocity.X > 0f - num688)
						{
							Projectile.velocity.X -= num687;
						}
						else
						{
							Projectile.velocity.X -= num690;
						}
					}
					else
					{
						num691 = 1;
						if (Projectile.velocity.X < num688)
						{
							Projectile.velocity.X += num687;
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
					if (Math.Abs(Projectile.velocity.X) < num687 * 2f)
					{
						Projectile.velocity.X = 0f;
					}
				}
				if (num691 != 0)
				{
					int num693 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
					int num694 = (int)Projectile.position.Y / 16;
					num693 += num691;
					num693 += (int)Projectile.velocity.X;
					for (int num695 = num694; num695 < num694 + Projectile.height / 16 + 1; num695++)
					{
						if (WorldGen.SolidTile(num693, num695))
						{
							flag29 = true;
						}
					}
				}
				Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
				if (Projectile.velocity.Y == 0f && flag29)
				{
					for (int num696 = 0; num696 < 3; num696++)
					{
						int num697 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
						if (num696 == 0)
						{
							num697 = (int)Projectile.position.X / 16;
						}
						if (num696 == 2)
						{
							num697 = (int)(Projectile.position.X + (float)Projectile.width) / 16;
						}
						int num698 = (int)(Projectile.position.Y + (float)Projectile.height) / 16;
						if (!WorldGen.SolidTile(num697, num698) && !Main.tile[num697, num698].IsHalfBlock && Main.tile[num697, num698].Slope <= 0 && (!TileID.Sets.Platforms[Main.tile[num697, num698].TileType] || !Main.tile[num697, num698].HasTile || Main.tile[num697, num698].IsActuated))
						{
							continue;
						}
						try
						{
							num697 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
							num698 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
							num697 += num691;
							num697 += (int)Projectile.velocity.X;
							if (!WorldGen.SolidTile(num697, num698 - 1) && !WorldGen.SolidTile(num697, num698 - 2))
							{
								Projectile.velocity.Y = -5.1f;
							}
							else if (!WorldGen.SolidTile(num697, num698 - 2))
							{
								Projectile.velocity.Y = -7.1f;
							}
							else if (WorldGen.SolidTile(num697, num698 - 5))
							{
								Projectile.velocity.Y = -11.1f;
							}
							else if (WorldGen.SolidTile(num697, num698 - 4))
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
				if (Projectile.velocity.X > num687 && num691 == 1)
				{
					Projectile.direction = 1;
				}
				if (Projectile.velocity.X < 0f - num687 && num691 == -1)
				{
					Projectile.direction = -1;
				}
				Projectile.spriteDirection = Projectile.direction;
				if (flag26)
				{
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