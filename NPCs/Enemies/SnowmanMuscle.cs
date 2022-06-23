using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.Utilities;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace RijamsMod.NPCs.Enemies
{
	class SnowmanMuscle : ModNPC
	{
		//public override string Texture => "Terraria/NPC_" + NPCID.GreekSkeleton;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snowman Muscle");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.SnowmanGangsta];

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.SnowmanGangsta);
			NPC.damage = 30;
			NPC.defense = 0;
			NPC.lifeMax = 250;
			NPC.lavaImmune = true;
			NPC.buffImmune[BuffID.Confused] = false;
			NPC.value = 500f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
			AIType = NPCID.SnowmanGangsta;
			AnimationType = NPCID.SnowmanGangsta;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Placeable.SnowmanMuscleBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Shotgun, 20));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.Vanity.CarrotNose>(), 25));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.Defense.FrostyRose>(), 25));
			npcLoot.Add(ItemDropRule.Common(ItemID.Present, 150));
			npcLoot.Add(ItemDropRule.Common(ItemID.SnowBlock, 1, 5, 10));
			npcLoot.Add(ItemDropRule.Common(ItemID.Fedora, 50));

			//From Spirit Mod FrostSaucer.cs
			if (Main.invasionType == InvasionID.SnowLegion)
			{
				Main.invasionSize -= 1;
				if (Main.invasionSize < 0)
                {
					Main.invasionSize = 0;
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
                {
					//icon: 1 = Frost Moon, 2 = Pumpkin Moon, 3 = Old One's Army, 4 = Goblin Army, 5 = Frost Legion, 6 = Pirate Invasion, 7 = Martian Madness
					//Any other number (including 0) will be a generic invasion icon
					Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, 5, 0);
				}
				if (Main.netMode == NetmodeID.Server)
                {
					NetMessage.SendData(MessageID.InvasionProgressReport, -1, -1, null, Main.invasionProgress, (float)Main.invasionProgressMax, (float)Main.invasionProgressIcon, 0f, 0, 0, 0);
				}
			}
		}

		//AI copied from the Snowman Gangsta

		public override void AI()
		{
			float speed = 2f;
			float num526 = 0.6f;
			if (true)
			{
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= 180f)
				{
					NPC.ai[2] = 0f;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 projectileVector = new(NPC.position.X + (float)NPC.width * 0.5f - (float)(NPC.direction * 12), NPC.position.Y + (float)NPC.height * 0.5f);
						float speedX = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - projectileVector.X;
						float speedY = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - projectileVector.Y;
						float sqrtX2Y2 = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						NPC.netUpdate = true;
						sqrtX2Y2 = 9f / sqrtX2Y2;
						speedX *= sqrtX2Y2;
						speedY *= sqrtX2Y2;
						projectileVector.X += speedX;
						projectileVector.Y += speedY;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int projectileDamage = 15;
							int projectileType = ProjectileID.BulletSnowman;
							SoundEngine.PlaySound(SoundID.Item36, NPC.position);
							for (int i=0; i < 3; i++)
                            {
								int newProjectile = Projectile.NewProjectile(Entity.GetSource_FromAI(), projectileVector.X, projectileVector.Y, speedX, speedY, projectileType, projectileDamage, 0.5f, Main.myPlayer);
								Main.projectile[newProjectile].ai[0] = 2f;
								Main.projectile[newProjectile].timeLeft = 300;
								Main.projectile[newProjectile].friendly = false;
								NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, newProjectile);
								NPC.netUpdate = true;
								speedY += Main.rand.Next(-60, 60) * sqrtX2Y2;
								speedX += Main.rand.Next(-20, 20) * sqrtX2Y2;
							}
						}
					}
				}
			}
			if (true)
			{
				if (NPC.velocity.Y == 0f)
				{
					if (NPC.localAI[2] == NPC.position.X)
					{
						NPC.direction *= -1;
						NPC.ai[3] = 60f;
					}
					NPC.localAI[2] = NPC.position.X;
					if (NPC.ai[3] == 0f)
					{
						NPC.TargetClosest();
					}
					NPC.ai[0] += 1f;
					if (NPC.ai[0] > 2f)
					{
						NPC.ai[0] = 0f;
						NPC.ai[1] += 1f;
						NPC.velocity.Y = -8.2f;
						NPC.velocity.X += (float)NPC.direction * num526 * 1.1f;
					}
					else
					{
						NPC.velocity.Y = -6f;
						NPC.velocity.X += (float)NPC.direction * num526 * 0.9f;
					}
					NPC.spriteDirection = NPC.direction;
				}
				NPC.velocity.X += (float)NPC.direction * num526 * 0.01f;
			}
			if (NPC.ai[3] > 0f)
			{
				NPC.ai[3] -= 1f;
			}
			if (NPC.velocity.X > speed && NPC.direction > 0)
			{
				NPC.velocity.X = 4f;
			}
			if (NPC.velocity.X < -speed && NPC.direction < 0)
			{
				NPC.velocity.X = -4f;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (SpawnCondition.FrostLegion.Active)
            {
				return SpawnCondition.FrostLegion.Chance * 0.2f;
			}
			else
            {
				return 0;
			}
		}
	}
}