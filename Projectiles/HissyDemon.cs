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
	public class HissyDemon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hissy Demon");
			// Sets the amount of frames projectile minion has on its spritesheet
			Main.projFrames[projectile.type] = 8;
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
			//projectile.CloneDefaults(ProjectileID.FlyingImp);
			projectile.width = 30;
			projectile.height = 30;
			// Makes the minion go through tiles freely
			projectile.tileCollide = false;
			projectile.ignoreWater = true;

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
			//aiType = ProjectileID.FlyingImp;
			projectile.minionSlots = 1f;

			// Using local NPC immunity allows each to strike independently from one another.
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;

			drawOffsetX = -10;
			drawOriginOffsetY -= 8;
		}

		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// projectile is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return false;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
		}
		bool shooting = false;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];


			#region Active check
			// projectile is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active) {
				//player.ClearBuff(ModContent.BuffType<HissyDemonBuff>());
				player.ClearBuff(ModContent.BuffType<Buffs.HissyDemonBuff>());
			}
			//if (player.HasBuff(ModContent.BuffType<HissyDemonBuff>()))
			if (player.HasBuff(ModContent.BuffType<Buffs.HissyDemonBuff>()))
			{
				projectile.timeLeft = 2;
			}
			#endregion
			//Copied from AIStyle 62
			float num5 = 0.07f;
			float num6 = projectile.width;
			for (int m = 0; m < 1000; m++)
			{
				if (m != projectile.whoAmI && Main.projectile[m].active && Main.projectile[m].owner == projectile.owner && Main.projectile[m].type == projectile.type && Math.Abs(projectile.position.X - Main.projectile[m].position.X) + Math.Abs(projectile.position.Y - Main.projectile[m].position.Y) < num6)
				{
					if (projectile.position.X < Main.projectile[m].position.X)
					{
						projectile.velocity.X -= num5;
					}
					else
					{
						projectile.velocity.X += num5;
					}
					if (projectile.position.Y < Main.projectile[m].position.Y)
					{
						projectile.velocity.Y -= num5;
					}
					else
					{
						projectile.velocity.Y += num5;
					}
				}
			}
			Vector2 vector = projectile.position;
			float num7 = 400f;
			bool flag = false;
			int num8 = -1;
			projectile.tileCollide = true;
			NPC ownerMinionAttackTargetNPC2 = projectile.OwnerMinionAttackTargetNPC;
			if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(projectile))
			{
				float num11 = Vector2.Distance(ownerMinionAttackTargetNPC2.Center, projectile.Center);
				if (((Vector2.Distance(projectile.Center, vector) > num11 && num11 < num7) || !flag) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
				{
					num7 = num11;
					vector = ownerMinionAttackTargetNPC2.Center;
					flag = true;
					num8 = ownerMinionAttackTargetNPC2.whoAmI;
				}
			}
			if (!flag)
			{
				for (int num12 = 0; num12 < 200; num12++)
				{
					NPC nPC2 = Main.npc[num12];
					if (nPC2.CanBeChasedBy(projectile))
					{
						float num13 = Vector2.Distance(nPC2.Center, projectile.Center);
						if (((Vector2.Distance(projectile.Center, vector) > num13 && num13 < num7) || !flag) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, nPC2.position, nPC2.width, nPC2.height))
						{
							num7 = num13;
							vector = nPC2.Center;
							flag = true;
							num8 = num12;
						}
					}
				}
			}
			int num14 = 500;
			if (flag)
			{
				num14 = 1000;
			}
			if (Vector2.Distance(player.Center, projectile.Center) > (float)num14)
			{
				projectile.ai[0] = 1f;
				projectile.netUpdate = true;
			}
			if (projectile.ai[0] == 1f)
			{
				projectile.tileCollide = false;
			}
			if (flag && projectile.ai[0] == 0f)
			{
				Vector2 vector5 = vector - projectile.Center;
				float num15 = vector5.Length();
				vector5.Normalize();
				if (projectile.type == 423)
				{
					vector5 = vector - Vector2.UnitY * 80f;
					int num16 = (int)vector5.Y / 16;
					if (num16 < 0)
					{
						num16 = 0;
					}
					Tile tile = Main.tile[(int)vector5.X / 16, num16];
					if (tile != null && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
					{
						vector5 += Vector2.UnitY * 16f;
						tile = Main.tile[(int)vector5.X / 16, (int)vector5.Y / 16];
						if (tile != null && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
						{
							vector5 += Vector2.UnitY * 16f;
						}
					}
					vector5 -= projectile.Center;
					num15 = vector5.Length();
					vector5.Normalize();
					if (num15 > 300f && num15 <= 800f && projectile.localAI[0] == 0f)
					{
						projectile.ai[0] = 2f;
						projectile.ai[1] = (int)(num15 / 10f);
						projectile.extraUpdates = (int)projectile.ai[1];
						projectile.velocity = vector5 * 10f;
						projectile.localAI[0] = 60f;
						return;
					}
				}
				if (num15 > 200f)
				{
					float num19 = 6f;
					vector5 *= num19;
					projectile.velocity.X = (projectile.velocity.X * 40f + vector5.X) / 41f;
					projectile.velocity.Y = (projectile.velocity.Y * 40f + vector5.Y) / 41f;
				}
				else if ((projectile.type == 375 || true))
				{
					if (num15 < 150f)
					{
						float num21 = 4f;
						vector5 *= 0f - num21;
						projectile.velocity.X = (projectile.velocity.X * 40f + vector5.X) / 41f;
						projectile.velocity.Y = (projectile.velocity.Y * 40f + vector5.Y) / 41f;
					}
					else
					{
						projectile.velocity *= 0.97f;
					}
				}
				else if (projectile.velocity.Y > -1f)
				{
					projectile.velocity.Y -= 0.1f;
				}
			}
			else
			{
				if (!Collision.CanHitLine(projectile.Center, 1, 1, Main.player[projectile.owner].Center, 1, 1))
				{
					projectile.ai[0] = 1f;
				}
				float num22 = 6f;
				if (projectile.ai[0] == 1f)
				{
					num22 = 15f;
				}
				Vector2 center2 = projectile.Center;
				Vector2 vector7 = player.Center - center2 + new Vector2(0f, -60f);
				if (((projectile.type == 375 || true) || true))
				{
					projectile.ai[1] = 3600f;
					projectile.netUpdate = true;
					vector7 = player.Center - center2;
					int num23 = 1;
					for (int num24 = 0; num24 < projectile.whoAmI; num24++)
					{
						if (Main.projectile[num24].active && Main.projectile[num24].owner == projectile.owner && Main.projectile[num24].type == projectile.type)
						{
							num23++;
						}
					}
					vector7.X -= 15 * Main.player[projectile.owner].direction;
					vector7.X -= num23 * 41 * Main.player[projectile.owner].direction;
					vector7.Y -= 15f;
				}
				float num25 = vector7.Length();
				if (num25 > 200f && num22 < 9f)
				{
					num22 = 9f;
				}
				if ((projectile.type == 375 || true))
				{
					num22 = (int)((double)num22 * 0.75);
				}
				if (num25 < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.ai[0] = 0f;
					projectile.netUpdate = true;
				}
				if (num25 > 2000f)
				{
					projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
					projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.width / 2);
				}
				if ((projectile.type == 375 || true))
				{
					if (num25 > 10f)
					{
						vector7.Normalize();
						if (num25 < 50f)
						{
							num22 /= 2f;
						}
						vector7 *= num22;
						projectile.velocity = (projectile.velocity * 20f + vector7) / 21f;
					}
					else
					{
						projectile.direction = Main.player[projectile.owner].direction;
						projectile.velocity *= 0.9f;
					}
				}
				else if (num25 > 70f)
				{
					vector7.Normalize();
					vector7 *= num22;
					projectile.velocity = (projectile.velocity * 20f + vector7) / 21f;
				}
				else
				{
					if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
					{
						projectile.velocity.X = -0.15f;
						projectile.velocity.Y = -0.05f;
					}
					projectile.velocity *= 1.01f;
				}
			}
			projectile.rotation = projectile.velocity.X * 0.05f;

			//Main.NewText("projectile.frameCounter " + projectile.frameCounter);
			//Main.NewText("projectile.ai[1] " + projectile.ai[1]);
			//Main.NewText("shooting " + shooting);

			int frameSpeed = 32;
			
			projectile.frameCounter++;
			//if (projectile.ai[1] > 0f && projectile.ai[1] < 16f) //If shooting
			if (shooting == true)
			{
				if (projectile.frameCounter > 32 && projectile.frameCounter <= 40)
				{
					projectile.frame = 4;
				}
				if (projectile.frameCounter > 40 && projectile.frameCounter <= 48)
                {
					projectile.frame = 5;
				}
				if (projectile.frameCounter > 48 && projectile.frameCounter <= 56)
				{
					projectile.frame = 6;
				}
				if (projectile.frameCounter > 56 && projectile.frameCounter <= 64)
				{
					projectile.frame = 7;
				}
				if (projectile.frameCounter > 64)
				{
					projectile.frameCounter = 0;
					projectile.frame = 0;
					shooting = false;
				}
				if (projectile.frame >= 8)
				{
					projectile.frame = 0;
				}
			}
			else if (projectile.frameCounter >= frameSpeed && shooting == false) //if not shooting
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (shooting == false && projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
				if (projectile.frame >= 8)
				{
					projectile.frame = 0;
				}
			}
			/*projectile.frameCounter++;
			if ((projectile.type == 375 || true))
			{
				if (projectile.frameCounter >= 32)
				{
					projectile.frameCounter = 0;
				}
				projectile.frame = projectile.frameCounter / 8;
				if (projectile.ai[1] > 0f && projectile.ai[1] < 16f)
				{
					projectile.frame += 4;
				}
			}*/
			if (projectile.velocity.X > 0f)
			{
				projectile.spriteDirection = (projectile.direction = -1);
			}
			else if (projectile.velocity.X < 0f)
			{
				projectile.spriteDirection = (projectile.direction = 1);
			}
			if (projectile.type == 373)
			{
				if (projectile.ai[1] > 0f)
				{
					projectile.ai[1] += Main.rand.Next(1, 4);
				}
				if (projectile.ai[1] > 90f)
				{
					projectile.ai[1] = 0f;
					projectile.netUpdate = true;
				}
			}
			else if ((projectile.type == 375 || true))
			{
				if (projectile.ai[1] > 0f)
				{
					projectile.ai[1] += 1f;
					if (Main.rand.Next(3) == 0)
					{
						projectile.ai[1] += 1f;
					}
				}
				if (projectile.ai[1] > (float)Main.rand.Next(180, 240))
				{
					projectile.ai[1] = 0f;
					projectile.netUpdate = true;
				}
			}
			if (projectile.ai[0] != 0f)
			{
				return;
			}
			float num30 = 11f;
			if (!flag)
			{
				return;
			}
			if ((projectile.type == 375 || true))
			{
				if ((vector - projectile.Center).X > 0f)
				{
					projectile.spriteDirection = (projectile.direction = -1);
				}
				else if ((vector - projectile.Center).X < 0f)
				{
					projectile.spriteDirection = (projectile.direction = 1);
				}
			}
			if (projectile.ai[1] == 0f)
			{
				projectile.ai[1] += 1f;
				if (Main.myPlayer == projectile.owner)
				{
					shooting = true;
					projectile.frameCounter = 32;
					Vector2 vector11 = vector - projectile.Center;
					vector11.Normalize();
					vector11 *= num30;
					int num36 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector11.X, vector11.Y, ModContent.ProjectileType<LightningBall>(), projectile.damage, 0f, Main.myPlayer);
					Main.projectile[num36].timeLeft = 300;
					Main.projectile[num36].netUpdate = true;
					projectile.netUpdate = true;
					if (!Main.dedServ)
					{
						Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/HissyDemonShoot").WithVolume(0.8f), (int)projectile.position.X, (int)projectile.position.Y);
					}
				}
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			if (shooting == true)
            {
				return Color.White;
			}
			else return null;
		}
	}
}