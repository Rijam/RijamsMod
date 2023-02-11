using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.NPCs.Enemies
{
	public class SpikedGreenSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spiked Green Slime");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.SlimeSpiked];
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.SlimeSpiked);
			NPC.damage = 40;
			NPC.defense = 6;
			NPC.lifeMax = NPC.downedPlantBoss ? 300 : 200; //Doubled in Expert Mode
			NPC.value = NPC.downedPlantBoss ? 400 : 200;
			NPC.knockBackResist = 0.1f;
			NPC.aiStyle = -1;
			//AIType = NPCID.SlimeSpiked;
			AnimationType = NPCID.SlimeSpiked;
			//banner = Item.NPCtoBanner(NPCID.GreenSlime);
			//bannerItem = Item.BannerToItem(banner);
			NPC.npcSlots = 0.5f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.SlimeRain,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
			});
		}

		public override void AI()
		{
			bool flag = true;
			if (NPC.localAI[0] > 0f)
			{
				NPC.localAI[0] -= 1f;
			}
			if (!NPC.wet && !Main.player[NPC.target].npcTypeNoAggro[NPC.type])
			{
				Vector2 vector3 = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num11 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector3.X;
				float num12 = Main.player[NPC.target].position.Y - vector3.Y;
				float num13 = (float)Math.Sqrt(num11 * num11 + num12 * num12);
				if (Main.expertMode && num13 < 120f && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && NPC.velocity.Y == 0f)
				{
					NPC.ai[0] = -40f;
					if (NPC.velocity.Y == 0f)
					{
						NPC.velocity.X *= 0.9f;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] == 0f)
					{
						for (int j = 0; j < 5; j++)
						{
							Vector2 vector4 = new(j - 2, -4f);
							vector4.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.005f;
							vector4.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.005f;
							vector4.Normalize();
							vector4 *= 4f + (float)Main.rand.Next(-50, 51) * 0.01f;
							Projectile.NewProjectile(Entity.GetSource_FromAI(), vector3.X, vector3.Y, vector4.X, vector4.Y, ModContent.ProjectileType<Projectiles.Enemies.SpikedGreenSlimeSpike>(), 9, 0f, Main.myPlayer);
							NPC.localAI[0] = 30f;
						}
					}
				}
				else if (num13 < 200f && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && NPC.velocity.Y == 0f)
				{
					NPC.ai[0] = -40f;
					if (NPC.velocity.Y == 0f)
					{
						NPC.velocity.X *= 0.9f;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] == 0f)
					{
						num12 = Main.player[NPC.target].position.Y - vector3.Y - (float)Main.rand.Next(0, 200);
						num13 = (float)Math.Sqrt(num11 * num11 + num12 * num12);
						num13 = 4.5f / num13;
						num11 *= num13;
						num12 *= num13;
						NPC.localAI[0] = 50f;
						Projectile.NewProjectile(Entity.GetSource_FromAI(), vector3.X, vector3.Y, num11, num12, ModContent.ProjectileType<Projectiles.Enemies.SpikedGreenSlimeSpike>(), 9, 0f, Main.myPlayer);
					}
				}
			}
			if (NPC.wet)
			{
				if (NPC.collideY)
				{
					NPC.velocity.Y = -2f;
				}
				if (NPC.velocity.Y < 0f && NPC.ai[3] == NPC.position.X)
				{
					NPC.direction *= -1;
					NPC.ai[2] = 200f;
				}
				if (NPC.velocity.Y > 0f)
				{
					NPC.ai[3] = NPC.position.X;
				}
				else
				{
					if (NPC.velocity.Y > 2f)
					{
						NPC.velocity.Y *= 0.9f;
					}
					NPC.velocity.Y -= 0.5f;
					if (NPC.velocity.Y < -4f)
					{
						NPC.velocity.Y = -4f;
					}
				}
				if (NPC.ai[2] == 1f && flag)
				{
					NPC.TargetClosest();
				}
			}
			NPC.aiAction = 0;
			if (NPC.ai[2] == 0f)
			{
				NPC.ai[0] = -100f;
				NPC.ai[2] = 1f;
				NPC.TargetClosest();
			}
			if (NPC.velocity.Y == 0f)
			{
				if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
				{
					NPC.position.X -= NPC.velocity.X + (float)NPC.direction;
				}
				if (NPC.ai[3] == NPC.position.X)
				{
					NPC.direction *= -1;
					NPC.ai[2] = 200f;
				}
				NPC.ai[3] = 0f;
				NPC.velocity.X *= 0.8f;
				if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
				{
					NPC.velocity.X = 0f;
				}
				if (flag)
				{
					NPC.ai[0] += 1f;
				}
				NPC.ai[0] += 1f;
				int num19 = 0;
				if (NPC.ai[0] >= 0f)
				{
					num19 = 1;
				}
				if (NPC.ai[0] >= -1000f && NPC.ai[0] <= -500f)
				{
					num19 = 2;
				}
				if (NPC.ai[0] >= -2000f && NPC.ai[0] <= -1500f)
				{
					num19 = 3;
				}
				if (num19 > 0)
				{
					NPC.netUpdate = true;
					if (flag && NPC.ai[2] == 1f)
					{
						NPC.TargetClosest();
					}
					if (num19 == 3)
					{
						NPC.velocity.Y = -8f;
						NPC.velocity.X += 3 * NPC.direction;
						NPC.ai[0] = -200f;
						NPC.ai[3] = NPC.position.X;
					}
					else
					{
						NPC.velocity.Y = -6f;
						NPC.velocity.X += 2 * NPC.direction;
						NPC.ai[0] = -120f;
						if (num19 == 1)
						{
							NPC.ai[0] -= 1000f;
						}
						else
						{
							NPC.ai[0] -= 2000f;
						}
					}
				}
				else if (NPC.ai[0] >= -30f)
				{
					NPC.aiAction = 1;
				}
			}
			else if (NPC.target < 255 && ((NPC.direction == 1 && NPC.velocity.X < 3f) || (NPC.direction == -1 && NPC.velocity.X > -3f)))
			{
				if (NPC.collideX && Math.Abs(NPC.velocity.X) == 0.2f)
				{
					NPC.position.X -= 1.4f * (float)NPC.direction;
				}
				if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
				{
					NPC.position.X -= NPC.velocity.X + (float)NPC.direction;
				}
				if ((NPC.direction == -1 && (double)NPC.velocity.X < 0.01) || (NPC.direction == 1 && (double)NPC.velocity.X > -0.01))
				{
					NPC.velocity.X += 0.2f * (float)NPC.direction;
				}
				else
				{
					NPC.velocity.X *= 0.93f;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 2, 1, 4));
		}


		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.slimeRain && Main.hardMode && spawnInfo.Player.ZoneOverworldHeight)
			{
				if (spawnInfo.PlayerInTown)
				{
					return 1.25f;
				}
				return 0.75f;
			}
			return 0;
		}
	}
}