using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.ItemDropRules;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;

namespace RijamsMod.NPCs.Enemies
{
	class DarkSoldier : ModNPC
	{
		//public override string Texture => "Terraria/NPC_" + NPCID.GreekSkeleton;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dark Soldier");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BoneThrowingSkeleton];

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Specify the debuffs it is immune to
			NPCDebuffImmunityData debuffData = new()
			{
				SpecificallyImmuneTo = new int[] {
					BuffID.OnFire,
				}
			};
			NPCID.Sets.DebuffImmunitySets[Type] = debuffData;
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.BoneThrowingSkeleton);
			NPC.width = 18;
			NPC.height = 40;
			NPC.damage = 14;
			NPC.defense = 6;
			NPC.lifeMax = 200;
			NPC.lavaImmune = true;
			NPC.HitSound = new(Mod.Name + "/Sounds/NPCHit/DarkSoldierHurt") { Volume = 0.7f, MaxInstances = 5 };
			NPC.DeathSound = new(Mod.Name + "/Sounds/NPCKilled/DarkSoldierDeath") { Volume = 0.7f, MaxInstances = 5 };
			NPC.value = 6000f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
			//AIType = NPCID.BoneThrowingSkeleton;
			AnimationType = NPCID.BoneThrowingSkeleton;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Placeable.EnemyBanners.DarkSoldierBanner>();
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
			});
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Leg").Type, 1f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Quest.BreadAndJelly>(), 1000)); //0.1% chance
			npcLoot.Add(ItemDropRule.ByCondition(Condition.DownedSkeletron.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<Items.Weapons.Summon.Minions.HissyStaff>(), 20)); //5% chance & defeated Skeletron
			npcLoot.Add(ItemDropRule.ByCondition(Condition.NotDownedSkeletron.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<Items.Weapons.Summon.Minions.HissyStaff>(), 1000)); //0.1% chance & not defeated Skeletron
		}

		//Frames
		//0 jump
		//1-14 walk
		//15-18 attack

		int alertSoundCounter = 0;
		//AI copied from the Bone Throwing Skeleton
		//Variables renamed to maybe make more sense
		public override void AI()
		{
			//Main.NewText("alertSoundCounter is currenty: " + alertSoundCounter);
			bool flagVelocityX0OrJustHit = false;
			if (NPC.velocity.X == 0f)
			{
				flagVelocityX0OrJustHit = true;
			}
			if (NPC.justHit)
			{
				flagVelocityX0OrJustHit = false;
			}
			int numIs60 = 60;
			bool flagVelocityY0 = false;
			bool flag6False = false;
			//int npcType = npc.type;
			bool flag7True = true;
			if (!flag6False && flag7True)
			{
				if (NPC.velocity.Y == 0f && ((NPC.velocity.X > 0f && NPC.direction < 0) || (NPC.velocity.X < 0f && NPC.direction > 0)))
				{
					flagVelocityY0 = true;
				}
				if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= numIs60 || flagVelocityY0)
				{
					NPC.ai[3] += 1f;
				}
				else if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
				{
					NPC.ai[3] -= 1f;
				}
				if (NPC.ai[3] > (numIs60 * 10))
				{
					NPC.ai[3] = 0f;
				}
				if (NPC.justHit)
				{
					NPC.ai[3] = 0f;
				}
				if (NPC.ai[3] == numIs60)
				{
					NPC.netUpdate = true;
				}
			}
			if (NPC.ai[3] < numIs60 && (Main.eclipse || !Main.dayTime || NPC.position.Y > Main.worldSurface * 16.0 ))
			{
				if (Main.rand.NextBool(800))
				{
					SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/DarkSoldierIdle") { MaxInstances = 5 }, NPC.position);
				}
				NPC.TargetClosest();
			}
			else if (!(NPC.ai[2] > 0f))
			{
				if (Main.dayTime && (double)(NPC.position.Y / 16f) < Main.worldSurface && NPC.timeLeft > 10)
				{
					NPC.timeLeft = 10;
				}
				if (NPC.velocity.X == 0f)
				{
					if (NPC.velocity.Y == 0f)
					{
						//stand still and then turns around only for walking
						NPC.ai[0] += 1f;
						if (NPC.ai[0] >= 2f)
						{
							NPC.direction *= -1;
							NPC.spriteDirection = NPC.direction;
							NPC.ai[0] = 0f;
						}
					}
				}
				else
				{
					NPC.ai[0] = 0f;
				}
				if (NPC.direction == 0)
				{
					NPC.direction = 1;
				}
			}
			if (true)
			{
				bool flag18True = true;
				int numIsNeg1 = -1;
				int numIsAlsoNeg1 = -1;
				if (NPC.confused)
				{
					NPC.ai[2] = 0f;
				}
				else
				{
					if (NPC.ai[1] > 0f)
					{
						NPC.ai[1] -= 1f;
					}
					if (NPC.justHit)
					{
						NPC.ai[1] = 30f;
						NPC.ai[2] = 0f;
					}
					int numIs70 = 70;
					int numIs35 = numIs70 / 2;
					if (NPC.ai[2] > 0f)
					{
						if (flag18True)
						{
							NPC.TargetClosest();
						}
						if (NPC.ai[1] == numIs35)
						{
							NPC.FaceTarget();
							float numIs7f = 7f;
							Vector2 vectoryForProj = new(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
							float projSpeedX = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - vectoryForProj.X;
							float projSpeedXAbs; //= Math.Abs(projSpeedX) * 0.1f;
							if (NPC.type >= NPCID.BoneThrowingSkeleton && NPC.type <= NPCID.BoneThrowingSkeleton4 || true)
							{
								projSpeedXAbs = Math.Abs(projSpeedX) * Main.rand.Next(10, 20) * 0.01f;
							}
							float projSpeedY = Main.player[NPC.target].position.Y + Main.player[NPC.target].height * 0.5f - vectoryForProj.Y - projSpeedXAbs;
							float sqrtXto2PlusYto2 = (float)Math.Sqrt(projSpeedX * projSpeedX + projSpeedY * projSpeedY);
							NPC.netUpdate = true;
							sqrtXto2PlusYto2 = numIs7f / sqrtXto2PlusYto2;
							projSpeedX *= sqrtXto2PlusYto2;
							projSpeedY *= sqrtXto2PlusYto2;
							int projDamage = 20;
							int projType = ProjectileID.Fireball; //471 SkeletonBone;
							vectoryForProj.X += projSpeedX;
							vectoryForProj.Y += projSpeedY;
							if (!Main.dedServ)
							{
								SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/DarkSoldierShoot") { MaxInstances = 10 }, NPC.position);
								Projectile.NewProjectile(Entity.GetSource_FromAI(), vectoryForProj.X, vectoryForProj.Y, projSpeedX, projSpeedY, projType, projDamage, 0f, Main.myPlayer);
							}
							if (Math.Abs(projSpeedY) > Math.Abs(projSpeedX) * 2f)
							{
								if (projSpeedY > 0f)
								{
									NPC.ai[2] = 1f;
								}
								else
								{
									NPC.ai[2] = 5f;
								}
							}
							else if (Math.Abs(projSpeedX) > Math.Abs(projSpeedY) * 2f)
							{
								NPC.ai[2] = 3f;
							}
							else if (projSpeedY > 0f)
							{
								NPC.ai[2] = 2f;
							}
							else
							{
								NPC.ai[2] = 4f;
							}
						}
						if ((NPC.velocity.Y != 0f) || NPC.ai[1] <= 0f)
						{
							NPC.ai[2] = 0f;
							NPC.ai[1] = 0f;
						}
						else if (numIsNeg1 != -1 && NPC.ai[1] >= numIsNeg1 && NPC.ai[1] < (numIsNeg1 + numIsAlsoNeg1) && NPC.velocity.Y == 0f)
						{
							NPC.velocity.X *= 0.9f;//0.9f;
							NPC.spriteDirection = NPC.direction;
						}
					}
					else if ((NPC.ai[2] <= 0f) && (NPC.velocity.Y == 0f) && NPC.ai[1] <= 0f && !Main.player[NPC.target].dead)
					{
						bool canAndAboutToAttack = Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
						if (Main.player[NPC.target].stealth == 0f && Main.player[NPC.target].itemAnimation == 0)
						{
							canAndAboutToAttack = false;
						}
						if (canAndAboutToAttack)//about to attack
						{
							float numIs10f = 10f;
							Vector2 vector32 = new (NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
							float numPosXPlusWitdth = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - vector32.X;
							float numPosXPlusWitdthAbs = Math.Abs(numPosXPlusWitdth) * 0.1f;
							float numPosYPlusHeight = Main.player[NPC.target].position.Y + Main.player[NPC.target].height * 0.5f - vector32.Y - numPosXPlusWitdthAbs;
							numPosXPlusWitdth += Main.rand.Next(-40, 41);
							numPosYPlusHeight += Main.rand.Next(-40, 41);
							float numSqrtPosX2PlusPosY2 = (float)Math.Sqrt(numPosXPlusWitdth * numPosXPlusWitdth + numPosYPlusHeight * numPosYPlusHeight);
							float numIs200f = 200f;
							if (numSqrtPosX2PlusPosY2 < numIs200f)
							{
								if (!Main.dedServ && alertSoundCounter == 0)
								{
									SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/DarkSoldierAlert") { MaxInstances = 5 }, NPC.position);
									alertSoundCounter += 5;
								}
								if (alertSoundCounter > 0)
								{
									alertSoundCounter--;
								}
								if (alertSoundCounter < 0)
								{
									alertSoundCounter = 0;
								}
								NPC.netUpdate = true;
								NPC.velocity.X *= 0f; // 0.5f
								numSqrtPosX2PlusPosY2 = numIs10f / numSqrtPosX2PlusPosY2;
								numPosXPlusWitdth *= numSqrtPosX2PlusPosY2;
								numPosYPlusHeight *= numSqrtPosX2PlusPosY2;
								NPC.ai[2] = 3f;
								NPC.ai[1] = numIs70;
								if (Math.Abs(numPosYPlusHeight) > Math.Abs(numPosXPlusWitdth) * 2f)
								{
									if (numPosYPlusHeight > 0f)
									{
										NPC.ai[2] = 1f;
									}
									else
									{
										NPC.ai[2] = 5f;
									}
								}
								else if (Math.Abs(numPosXPlusWitdth) > Math.Abs(numPosYPlusHeight) * 2f)
								{
									NPC.ai[2] = 3f;
								}
								else if (numPosYPlusHeight > 0f)
								{
									NPC.ai[2] = 2f;
								}
								else
								{
									NPC.ai[2] = 4f;
								}
							}
						}
					}
					if (NPC.ai[2] <= 0f)
					{
						float maxVelocity = Condition.ForTheWorthyWorld.IsMet() ? 2f : 1.5f;
						float numIs007f = 0.07f;
						float numIs08f = 0.8f;
						if (NPC.velocity.X < 0f - maxVelocity || NPC.velocity.X > maxVelocity)
						{
							if (NPC.velocity.Y == 0f)
							{
								NPC.velocity *= numIs08f;//When jumping and lands
							}
						}
						else if (NPC.velocity.X < maxVelocity && NPC.direction == 1)
						{
							NPC.velocity.X += numIs007f;
							if (NPC.velocity.X > maxVelocity)
							{
								NPC.velocity.X = maxVelocity; //Moving right. Setting a higher value does make them walk faster, but its super jittery
							}
						}
						else if (NPC.velocity.X > 0f - maxVelocity && NPC.direction == -1)
						{
							NPC.velocity.X -= numIs007f;
							if (NPC.velocity.X < 0f - maxVelocity)
							{
								NPC.velocity.X = 0f - maxVelocity; //moving left
							}
						}
					}
				}
			}
			bool flagTileChecker = false;
			if (NPC.velocity.Y == 0f)
			{
				int numNPCPosYAndHeight = (int)(NPC.position.Y + NPC.height + 7f) / 16;
				int numNPCPosX = (int)NPC.position.X / 16;
				int numNPCPosXAndWidth = (int)(NPC.position.X + NPC.width) / 16;
				for (int num174 = numNPCPosX; num174 <= numNPCPosXAndWidth; num174++)
				{
					if (Main.tile[num174, numNPCPosYAndHeight] == null)
					{
						return;
					}
					if (Main.tile[num174, numNPCPosYAndHeight].HasUnactuatedTile && Main.tileSolid[Main.tile[num174, numNPCPosYAndHeight].TileType])
					{
						flagTileChecker = true;
						break;
					}
				}
			}
			if (NPC.velocity.Y >= 0f)
			{
				int numNPCVelocityX = 0;
				if (NPC.velocity.X < 0f)
				{
					numNPCVelocityX = -1;//-1
				}
				if (NPC.velocity.X > 0f)
				{
					numNPCVelocityX = 1;//1
				}
				Vector2 vectorNPCPos = NPC.position;
				vectorNPCPos.X += NPC.velocity.X;
				int numNPCPosXAndStuff = (int)((vectorNPCPos.X + (NPC.width / 2) + ((NPC.width / 2 + 1) * numNPCVelocityX)) / 16f);
				int numNPCPosYAndStuff = (int)((vectorNPCPos.Y + NPC.height - 1f) / 16f);
				if (WorldGen.InWorld(numNPCPosXAndStuff, numNPCPosYAndStuff, 4))
				{
					if ((numNPCPosXAndStuff * 16) < vectorNPCPos.X + NPC.width && (numNPCPosXAndStuff * 16 + 16) > vectorNPCPos.X && 
						((Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff].HasUnactuatedTile && !Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff].TopSlope && 
						!Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].TopSlope && Main.tileSolid[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff].TileType] && 
						!Main.tileSolidTop[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff].TileType]) || (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].IsHalfBlock && 
						Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].HasUnactuatedTile)) && (!Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].HasUnactuatedTile || 
						!Main.tileSolid[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].TileType] || 
						Main.tileSolidTop[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].TileType] || (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].IsHalfBlock && 
						(!Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 4].TileType] || 
						Main.tileSolidTop[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 4].TileType]))) && (!Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 2].HasUnactuatedTile || 
						!Main.tileSolid[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 2].TileType] || 
						Main.tileSolidTop[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 2].TileType]) && (!Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 3].HasUnactuatedTile || 
						!Main.tileSolid[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 3].TileType] || 
						Main.tileSolidTop[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 3].TileType]) && 
						(!Main.tile[numNPCPosXAndStuff - numNPCVelocityX, numNPCPosYAndStuff - 3].HasUnactuatedTile || 
						!Main.tileSolid[Main.tile[numNPCPosXAndStuff - numNPCVelocityX, numNPCPosYAndStuff - 3].TileType]))
					{
						float num82 = numNPCPosYAndStuff * 16;
						if (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff].IsHalfBlock)
						{
							num82 += 8f;
						}
						if (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].IsHalfBlock)
						{
							num82 -= 8f;
						}
						if (num82 < vectorNPCPos.Y + NPC.height)
						{
							float num83 = vectorNPCPos.Y + NPC.height - num82;
							float num84 = 16.1f;
							if (num83 <= num84)
							{
								NPC.gfxOffY += NPC.position.Y + NPC.height - num82;
								NPC.position.Y = num82 - NPC.height;
								if (num83 < 9f)
								{
									NPC.stepSpeed = 1f;
								}
								else
								{
									NPC.stepSpeed = 2f;
								}
							}
						}
					}
				}
			}
			if (flagTileChecker)
			{
				int numNPCPosXWidthDirect = (int)((NPC.position.X + (NPC.width / 2) + (15 * NPC.direction)) / 16f);
				int numNPCPosYandHeight = (int)((NPC.position.Y + NPC.height - 15f) / 16f);
				if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].HasUnactuatedTile && (TileLoader.IsClosedDoor(Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1]) || Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].TileType == TileID.TallGateClosed))
				{
					NPC.ai[2] += 1f;
					NPC.ai[3] = 0f;
					if (NPC.ai[2] >= 60f)
					{
						bool flag14 = true;
						bool flag15 = Main.player[NPC.target].ZoneGraveyard && Main.rand.NextBool(60);
						if ((!Main.bloodMoon || Main.getGoodWorld) && !flag15 && flag14)
						{
							NPC.ai[1] = 0f;
						}
						NPC.velocity.X = 0.5f * (-NPC.direction);
						int num87 = 5;
						if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].TileType == TileID.TallGateClosed)
						{
							num87 = 2;
						}
						NPC.ai[1] += num87;
						NPC.ai[2] = 0f;
						bool flag16 = false;
						if (NPC.ai[1] >= 10f)
						{
							flag16 = true;
							NPC.ai[1] = 10f;
						}
						WorldGen.KillTile(numNPCPosXWidthDirect, numNPCPosYandHeight - 1, fail: true);
						if ((Main.netMode != NetmodeID.MultiplayerClient || !flag16) && flag16 && Main.netMode != NetmodeID.MultiplayerClient)
						{
							if (TileLoader.OpenDoorID(Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1]) >= 0)
							{
								bool flag17 = WorldGen.OpenDoor(numNPCPosXWidthDirect, numNPCPosYandHeight - 1, NPC.direction);
								if (!flag17)
								{
									NPC.ai[3] = numIs60;
									NPC.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag17)
								{
									NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, numNPCPosXWidthDirect, numNPCPosYandHeight - 1, NPC.direction);
								}
							}
							if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].TileType == TileID.TallGateClosed)
							{
								bool flag18 = WorldGen.ShiftTallGate(numNPCPosXWidthDirect, numNPCPosYandHeight - 1, closing: false);
								if (!flag18)
								{
									NPC.ai[3] = numIs60;
									NPC.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag18)
								{
									NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, numNPCPosXWidthDirect, numNPCPosYandHeight - 1);
								}
							}
						}
					}
				}
				else
				{
					int spriteDirection = NPC.spriteDirection;
					if ((NPC.velocity.X < 0f && spriteDirection == -1) || (NPC.velocity.X > 0f && spriteDirection == 1))
					{
						if (NPC.height >= 32 && Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 2].TileType])
						{
							if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 3].TileType])
							{
								NPC.velocity.Y = -8f;
								NPC.netUpdate = true;
							}
							else
							{
								NPC.velocity.Y = -7f;
								NPC.netUpdate = true;
							}
						}
						else if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].TileType])
						{
							NPC.velocity.Y = -6f;
							NPC.netUpdate = true;
						}
						else if (NPC.position.Y + NPC.height - (numNPCPosYandHeight * 16) > 20f && Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight].HasUnactuatedTile && !Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight].TopSlope && Main.tileSolid[Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight].TileType])
						{
							NPC.velocity.Y = -5f;
							NPC.netUpdate = true;
						}
						else if (NPC.directionY < 0 && (!Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight + 1].TileType]) && (!Main.tile[numNPCPosXWidthDirect + NPC.direction, numNPCPosYandHeight + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[numNPCPosXWidthDirect + NPC.direction, numNPCPosYandHeight + 1].TileType]))
						{
							NPC.velocity.Y = -8f;
							NPC.velocity.X *= 3f;//1.5
							NPC.netUpdate = true;
						}
						if (NPC.velocity.Y == 0f && flagVelocityX0OrJustHit && NPC.ai[3] == 1f)
						{
							NPC.velocity.Y = -5f;
						}
					}
					//The little jumpy thing when the player is close and the enemy is standing still
					if (NPC.velocity.Y == 0f && Math.Abs(NPC.position.X + (NPC.width / 2) - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2))) < 100f && Math.Abs(NPC.position.Y + (NPC.height / 2) - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2))) < 50f && ((NPC.direction > 0 && NPC.velocity.X >= 1f) || (NPC.direction < 0 && NPC.velocity.X <= -1f)))
					{
						NPC.velocity.X *= 2f;
						if (NPC.velocity.X > 3f)
						{
							NPC.velocity.X = 3f;
						}
						if (NPC.velocity.X < -3f)
						{
							NPC.velocity.X = -3f;
						}
						NPC.velocity.Y = -4f;
						NPC.netUpdate = true;
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			float spawnChance = 0f;
			if (spawnInfo.Player.ZoneUnderworldHeight) //Underworld
			{
				spawnChance += 0.3f;
			}
			if (spawnInfo.Player.ZoneRockLayerHeight && // Player in the caverns layer
				spawnInfo.SpawnTileY <= Main.maxTilesY - 200 && // Above the underworld
				spawnInfo.SpawnTileY > (Main.rockLayer + Main.maxTilesY - 200) / 2) // Lower half of the canverns above the underworld.
			{
				spawnChance += 0.08f;
			}
			if (spawnInfo.PlayerInTown) // Decrease the chance dramatically if in a town
			{
				spawnChance -= 0.1f;
			}
			if (Main.remixWorld && !Main.hardMode) // Don't Dig Up or Get Fixed Boi worlds and Pre-Hardmode.
			{
				spawnChance -= 0.1f;
			}
			return (float)Math.Clamp(spawnChance, 0.0, 1.0);
		}
	}
}