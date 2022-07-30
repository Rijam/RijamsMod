using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
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
	public class MeatballDemon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hissy Demon");
			// Sets the amount of frames projectile minion has on its spritesheet
			Main.projFrames[Projectile.type] = 8;
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
			//projectile.CloneDefaults(ProjectileID.FlyingImp);
			Projectile.width = 30;
			Projectile.height = 30;
			// Makes the minion go through tiles freely
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

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
			//AIType = ProjectileID.FlyingImp;
			Projectile.minionSlots = 1f;

			// Using local NPC immunity allows each to strike independently from one another.
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;

			DrawOffsetX = -16;
			DrawOriginOffsetY -= 16;
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
			Player player = Main.player[Projectile.owner];


			#region Active check
			// projectile is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<Buffs.MeatballDemonBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.MeatballDemonBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			#endregion
			//Copied from AIStyle 62
			float num5 = 0.07f;
			float num6 = Projectile.width;
			for (int m = 0; m < 1000; m++)
			{
				if (m != Projectile.whoAmI && Main.projectile[m].active && Main.projectile[m].owner == Projectile.owner && Main.projectile[m].type == Projectile.type && Math.Abs(Projectile.position.X - Main.projectile[m].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[m].position.Y) < num6)
				{
					if (Projectile.position.X < Main.projectile[m].position.X)
					{
						Projectile.velocity.X -= num5;
					}
					else
					{
						Projectile.velocity.X += num5;
					}
					if (Projectile.position.Y < Main.projectile[m].position.Y)
					{
						Projectile.velocity.Y -= num5;
					}
					else
					{
						Projectile.velocity.Y += num5;
					}
				}
			}
			Vector2 vector = Projectile.position;
			float num7 = 400f;
			bool flag = false;
			Projectile.tileCollide = true;
			NPC ownerMinionAttackTargetNPC2 = Projectile.OwnerMinionAttackTargetNPC;
			if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(Projectile))
			{
				float num11 = Vector2.Distance(ownerMinionAttackTargetNPC2.Center, Projectile.Center);
				if (((Vector2.Distance(Projectile.Center, vector) > num11 && num11 < num7) || !flag) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
				{
					num7 = num11;
					vector = ownerMinionAttackTargetNPC2.Center;
					flag = true;
				}
			}
			if (!flag)
			{
				for (int num12 = 0; num12 < 200; num12++)
				{
					NPC nPC2 = Main.npc[num12];
					if (nPC2.CanBeChasedBy(Projectile))
					{
						float num13 = Vector2.Distance(nPC2.Center, Projectile.Center);
						if (((Vector2.Distance(Projectile.Center, vector) > num13 && num13 < num7) || !flag) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, nPC2.position, nPC2.width, nPC2.height))
						{
							num7 = num13;
							vector = nPC2.Center;
							flag = true;
						}
					}
				}
			}
			int num14 = 500;
			if (flag)
			{
				num14 = 1000;
			}
			if (Vector2.Distance(player.Center, Projectile.Center) > (float)num14)
			{
				Projectile.ai[0] = 1f;
				Projectile.netUpdate = true;
			}
			if (Projectile.ai[0] == 1f)
			{
				Projectile.tileCollide = false;
			}
			if (flag && Projectile.ai[0] == 0f)
			{
				Vector2 vector5 = vector - Projectile.Center;
				float num15 = vector5.Length();
				vector5.Normalize();
				if (Projectile.type == 423)
				{
					vector5 = vector - Vector2.UnitY * 80f;
					int num16 = (int)vector5.Y / 16;
					if (num16 < 0)
					{
						num16 = 0;
					}
					Tile tile = Main.tile[(int)vector5.X / 16, num16];
					if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
					{
						vector5 += Vector2.UnitY * 16f;
						tile = Main.tile[(int)vector5.X / 16, (int)vector5.Y / 16];
						if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
						{
							vector5 += Vector2.UnitY * 16f;
						}
					}
					vector5 -= Projectile.Center;
					num15 = vector5.Length();
					vector5.Normalize();
					if (num15 > 300f && num15 <= 800f && Projectile.localAI[0] == 0f)
					{
						Projectile.ai[0] = 2f;
						Projectile.ai[1] = (int)(num15 / 10f);
						Projectile.extraUpdates = (int)Projectile.ai[1];
						Projectile.velocity = vector5 * 10f;
						Projectile.localAI[0] = 60f;
						return;
					}
				}
				if (num15 > 200f)
				{
					float num19 = 6f;
					vector5 *= num19;
					Projectile.velocity.X = (Projectile.velocity.X * 40f + vector5.X) / 41f;
					Projectile.velocity.Y = (Projectile.velocity.Y * 40f + vector5.Y) / 41f;
				}
				else if ((Projectile.type == 375 || true))
				{
					if (num15 < 150f)
					{
						float num21 = 4f;
						vector5 *= 0f - num21;
						Projectile.velocity.X = (Projectile.velocity.X * 40f + vector5.X) / 41f;
						Projectile.velocity.Y = (Projectile.velocity.Y * 40f + vector5.Y) / 41f;
					}
					else
					{
						Projectile.velocity *= 0.97f;
					}
				}
				else if (Projectile.velocity.Y > -1f)
				{
					Projectile.velocity.Y -= 0.1f;
				}
			}
			else
			{
				if (!Collision.CanHitLine(Projectile.Center, 1, 1, Main.player[Projectile.owner].Center, 1, 1))
				{
					Projectile.ai[0] = 1f;
				}
				float num22 = 6f;
				if (Projectile.ai[0] == 1f)
				{
					num22 = 15f;
				}
				Vector2 center2 = Projectile.Center;
				Vector2 vector7;
				if (true)
				{
					Projectile.ai[1] = 3600f;
					Projectile.netUpdate = true;
					vector7 = player.Center - center2;
					int num23 = 1;
					for (int num24 = 0; num24 < Projectile.whoAmI; num24++)
					{
						if (Main.projectile[num24].active && Main.projectile[num24].owner == Projectile.owner && Main.projectile[num24].type == Projectile.type)
						{
							num23++;
						}
					}
					vector7.X -= 15 * Main.player[Projectile.owner].direction;
					vector7.X -= num23 * 44 * Main.player[Projectile.owner].direction;
					vector7.Y -= 55f;
				}
				float num25 = vector7.Length();
				if (num25 > 200f && num22 < 9f)
				{
					num22 = 9f;
				}
				if ((Projectile.type == 375 || true))
				{
					num22 = (int)((double)num22 * 0.75);
				}
				if (num25 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					Projectile.netUpdate = true;
				}
				if (num25 > 2000f)
				{
					Projectile.position.X = Main.player[Projectile.owner].Center.X - (float)(Projectile.width / 2);
					Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (float)(Projectile.width / 2);
				}
				if ((Projectile.type == 375 || true))
				{
					if (num25 > 10f)
					{
						vector7.Normalize();
						if (num25 < 50f)
						{
							num22 /= 2f;
						}
						vector7 *= num22;
						Projectile.velocity = (Projectile.velocity * 20f + vector7) / 21f;
					}
					else
					{
						Projectile.direction = Main.player[Projectile.owner].direction;
						Projectile.velocity *= 0.9f;
					}
				}
				else if (num25 > 70f)
				{
					vector7.Normalize();
					vector7 *= num22;
					Projectile.velocity = (Projectile.velocity * 20f + vector7) / 21f;
				}
				else
				{
					if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
					{
						Projectile.velocity.X = -0.15f;
						Projectile.velocity.Y = -0.05f;
					}
					Projectile.velocity *= 1.01f;
				}
			}
			Projectile.rotation = Projectile.velocity.X * 0.05f;

			//Main.NewText("projectile.frameCounter " + projectile.frameCounter);
			//Main.NewText("projectile.ai[1] " + projectile.ai[1]);
			//Main.NewText("shooting " + shooting);

			int frameSpeed = 32;
			
			Projectile.frameCounter++;
			//if (projectile.ai[1] > 0f && projectile.ai[1] < 16f) //If shooting
			if (shooting == true)
			{
				if (Projectile.frameCounter > 32 && Projectile.frameCounter <= 40)
				{
					Projectile.frame = 4;
				}
				if (Projectile.frameCounter > 40 && Projectile.frameCounter <= 48)
                {
					Projectile.frame = 5;
				}
				if (Projectile.frameCounter > 48 && Projectile.frameCounter <= 56)
				{
					Projectile.frame = 6;
				}
				if (Projectile.frameCounter > 56 && Projectile.frameCounter <= 64)
				{
					Projectile.frame = 7;
				}
				if (Projectile.frameCounter > 64)
				{
					Projectile.frameCounter = 0;
					Projectile.frame = 0;
					shooting = false;
				}
				if (Projectile.frame >= 8)
				{
					Projectile.frame = 0;
				}
			}
			else if (Projectile.frameCounter >= frameSpeed/2 && shooting == false) //if not shooting
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (shooting == false && Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
				if (Projectile.frame >= 8)
				{
					Projectile.frame = 0;
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
			if (Projectile.velocity.X > 0f)
			{
				Projectile.spriteDirection = (Projectile.direction = -1);
			}
			else if (Projectile.velocity.X < 0f)
			{
				Projectile.spriteDirection = (Projectile.direction = 1);
			}
			if (Projectile.ai[1] > 0f)
			{
				Projectile.ai[1] += 1f;
				if (Main.rand.NextBool(3))
				{
					Projectile.ai[1] += 1f;
				}
			}
			if (Projectile.ai[1] > (float)Main.rand.Next(120, 180))
			{
				Projectile.ai[1] = 0f;
				Projectile.netUpdate = true;
			}
			if (Projectile.ai[0] != 0f)
			{
				return;
			}
			float num30 = 11f;
			if (!flag)
			{
				return;
			}
			if ((vector - Projectile.Center).X > 0f)
			{
				Projectile.spriteDirection = (Projectile.direction = -1);
			}
			else if ((vector - Projectile.Center).X < 0f)
			{
				Projectile.spriteDirection = (Projectile.direction = 1);
			}
			if (Projectile.ai[1] == 0f)
			{
				Projectile.ai[1] += 1f;
				if (Main.myPlayer == Projectile.owner)
				{
					shooting = true;
					Projectile.netUpdate = true;
					Projectile.frameCounter = 32;
					Vector2 vector11 = vector - Projectile.Center;
					vector11.Normalize();
					vector11 *= num30;
					int num36 = Projectile.NewProjectile(Entity.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, vector11.X, vector11.Y, ModContent.ProjectileType<LostSkull>(), Projectile.damage, 0f, Main.myPlayer);
					Main.projectile[num36].timeLeft = 300;
					Main.projectile[num36].netUpdate = true;
					Projectile.netUpdate = true;
					if (!Main.dedServ)
					{
						SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/MeatballDemonShoot") { Volume = 0.8f, MaxInstances = 10 }, Projectile.position);
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