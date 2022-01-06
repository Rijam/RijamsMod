using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace RijamsMod.NPCs.Enemies
{
	class DarkSoldier : ModNPC
	{
		//public override string Texture => "Terraria/NPC_" + NPCID.GreekSkeleton;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Soldier");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BoneThrowingSkeleton];
		}

		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.BoneThrowingSkeleton);
			npc.width = 18;
			npc.height = 40;
			npc.damage = 14;
			npc.defense = 6;
			npc.lifeMax = 200;
			npc.lavaImmune = true;
			npc.buffImmune[BuffID.OnFire] = true;
			if (!Main.dedServ) //Need to check if a server is running, otherwise it will break multiplayer
            {
				npc.HitSound = mod.GetLegacySoundSlot(SoundType.NPCHit, "Sounds/NPCHit/DarkSoldierHurt").WithVolume(.7f);
				npc.DeathSound = mod.GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/NPCKilled/DarkSoldierDeath").WithVolume(.7f);
			}
			npc.value = 6000f;
			npc.knockBackResist = 0.5f;
			npc.aiStyle = -1;
			aiType = NPCID.BoneThrowingSkeleton;
			animationType = NPCID.BoneThrowingSkeleton;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Placeable.DarkSoldierBanner>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DarkSoldier_Gore_Head"), 1f);
				for (int k = 0; k < 1; k++)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DarkSoldier_Gore_Arm"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DarkSoldier_Gore_Leg"), 1f);
				}
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(1000) == 0) //0.1% chance
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Quest.BreadAndJelly>());
			}
			if (Main.rand.Next(1000) == 0 && !NPC.downedBoss3) //0.1% chance & not defeated Skeletron
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.HissyStaff>());
			}
			if (Main.rand.Next(20) == 0 && NPC.downedBoss3) //5% chance & defeated Skeletron
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.HissyStaff>());
			}
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
			if (npc.velocity.X == 0f)
			{
				flagVelocityX0OrJustHit = true;
			}
			if (npc.justHit)
			{
				flagVelocityX0OrJustHit = false;
			}
			int numIs60 = 60;
			bool flagVelocityY0 = false;
			bool flagIfCorrectEnemyFalse;
			if (npc.type >= NPCID.BoneThrowingSkeleton && npc.type <= NPCID.BoneThrowingSkeleton4 || true)
			{
				flagIfCorrectEnemyFalse = false;
			}
			bool flag6False = false;
			//int npcType = npc.type;
			bool flag7True = true;
			if (!flag6False && flag7True)
			{
				if (npc.velocity.Y == 0f && ((npc.velocity.X > 0f && npc.direction < 0) || (npc.velocity.X < 0f && npc.direction > 0)))
				{
					flagVelocityY0 = true;
				}
				if (npc.position.X == npc.oldPosition.X || npc.ai[3] >= (float)numIs60 || flagVelocityY0)
				{
					npc.ai[3] += 1f;
				}
				else if ((double)Math.Abs(npc.velocity.X) > 0.9 && npc.ai[3] > 0f)
				{
					npc.ai[3] -= 1f;
				}
				if (npc.ai[3] > (float)(numIs60 * 10))
				{
					npc.ai[3] = 0f;
				}
				if (npc.justHit)
				{
					npc.ai[3] = 0f;
				}
				if (npc.ai[3] == (float)numIs60)
				{
					npc.netUpdate = true;
				}
			}
			if (npc.ai[3] < (float)numIs60 && (Main.eclipse || !Main.dayTime || (double)npc.position.Y > Main.worldSurface * 16.0 ))
			{
				if ( (npc.type >= NPCID.BoneThrowingSkeleton && npc.type <= NPCID.BoneThrowingSkeleton4 || true) && Main.rand.Next(800) == 0)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/DarkSoldierIdle"));
				}
				npc.TargetClosest();
			}
			else if (!(npc.ai[2] > 0f))
			{
				if (Main.dayTime && (double)(npc.position.Y / 16f) < Main.worldSurface && npc.timeLeft > 10)
				{
					npc.timeLeft = 10;
				}
				if (npc.velocity.X == 0f)
				{
					if (npc.velocity.Y == 0f)
					{
						//stand still and then turns around only for walking
						npc.ai[0] += 1f;
						if (npc.ai[0] >= 2f)
						{
							npc.direction *= -1;
							npc.spriteDirection = npc.direction;
							npc.ai[0] = 0f;
						}
					}
				}
				else
				{
					npc.ai[0] = 0f;
				}
				if (npc.direction == 0)
				{
					npc.direction = 1;
				}
			}
			if (!(npc.type < NPCID.BoneThrowingSkeleton || npc.type > NPCID.BoneThrowingSkeleton4 || false))
			{
				float numIs1f = 1f;
				if (npc.velocity.X < 0f - numIs1f || npc.velocity.X > numIs1f)
				{
					if (npc.velocity.Y == 0f)
					{
						npc.velocity *= 0.8f;//0.8f;
					}
				}
				else if (npc.velocity.X < numIs1f && npc.direction == 1)
				{
					npc.velocity.X += 0.07f;
					if (npc.velocity.X > numIs1f)
					{
						npc.velocity.X = numIs1f;
					}
				}
				else if (npc.velocity.X > 0f - numIs1f && npc.direction == -1)
				{
					npc.velocity.X -= 0.07f;
					if (npc.velocity.X < 0f - numIs1f)
					{
						npc.velocity.X = 0f - numIs1f;
					}
				}
			}
			if ((npc.type >= NPCID.BoneThrowingSkeleton && npc.type <= NPCID.BoneThrowingSkeleton4 || true) )
			{
				bool flagIfMartians = npc.type == NPCID.BrainScrambler || npc.type == NPCID.RayGunner || npc.type == NPCID.MartianWalker;
				bool flagIfVortexQueen = npc.type == NPCID.VortexHornetQueen;
				bool flag18True = true;
				int numIsNeg1 = -1;
				int numIsAlsoNeg1 = -1;
				if (npc.confused)
				{
					npc.ai[2] = 0f;
				}
				else
				{
					if (npc.ai[1] > 0f)
					{
						npc.ai[1] -= 1f;
					}
					if (npc.justHit)
					{
						npc.ai[1] = 30f;
						npc.ai[2] = 0f;
					}
					int numIs70 = 70;
					int numIs35 = numIs70 / 2;
					if (npc.ai[2] > 0f)
					{
						if (flag18True)
						{
							npc.TargetClosest();
						}
						if (npc.ai[1] == (float)numIs35)
						{
							float numIs7f = 7f;
							Vector2 vectoryForProj = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
							float projSpeedX = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vectoryForProj.X;
							float projSpeedXAbs; //= Math.Abs(projSpeedX) * 0.1f;
							if (npc.type >= NPCID.BoneThrowingSkeleton && npc.type <= NPCID.BoneThrowingSkeleton4 || true)
							{
								projSpeedXAbs = Math.Abs(projSpeedX) * (float)Main.rand.Next(10, 20) * 0.01f;
							}
							float projSpeedY = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vectoryForProj.Y - projSpeedXAbs;
							float sqrtXto2PlusYto2 = (float)Math.Sqrt(projSpeedX * projSpeedX + projSpeedY * projSpeedY);
							npc.netUpdate = true;
							sqrtXto2PlusYto2 = numIs7f / sqrtXto2PlusYto2;
							projSpeedX *= sqrtXto2PlusYto2;
							projSpeedY *= sqrtXto2PlusYto2;
							int projDamage = 20;
							int projType = ProjectileID.Fireball; //471 SkeletonBone;
							vectoryForProj.X += projSpeedX;
							vectoryForProj.Y += projSpeedY;
							if (!Main.dedServ)
							{
								Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/DarkSoldierShoot"));
								Projectile.NewProjectile(vectoryForProj.X, vectoryForProj.Y, projSpeedX, projSpeedY, projType, projDamage, 0f, Main.myPlayer);
							}
							if (Math.Abs(projSpeedY) > Math.Abs(projSpeedX) * 2f)
							{
								if (projSpeedY > 0f)
								{
									npc.ai[2] = 1f;
								}
								else
								{
									npc.ai[2] = 5f;
								}
							}
							else if (Math.Abs(projSpeedX) > Math.Abs(projSpeedY) * 2f)
							{
								npc.ai[2] = 3f;
							}
							else if (projSpeedY > 0f)
							{
								npc.ai[2] = 2f;
							}
							else
							{
								npc.ai[2] = 4f;
							}
						}
						if ((npc.velocity.Y != 0f && !flagIfVortexQueen) || npc.ai[1] <= 0f)
						{
							npc.ai[2] = 0f;
							npc.ai[1] = 0f;
						}
						else if (!flagIfMartians || (numIsNeg1 != -1 && npc.ai[1] >= (float)numIsNeg1 && npc.ai[1] < (float)(numIsNeg1 + numIsAlsoNeg1) && (!flagIfVortexQueen || npc.velocity.Y == 0f)))
						{
							npc.velocity.X *= 0.9f;//0.9f;
							npc.spriteDirection = npc.direction;
						}
					}
					else if ((npc.ai[2] <= 0f || flagIfMartians) && (npc.velocity.Y == 0f || flagIfVortexQueen) && npc.ai[1] <= 0f && !Main.player[npc.target].dead)
					{
						bool flag20 = Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height);
						if (Main.player[npc.target].stealth == 0f && Main.player[npc.target].itemAnimation == 0)
						{
							flag20 = false;
						}
						if (flag20)//about to attack
						{
							float numIs10f = 10f;
							Vector2 vector32 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
							float numPosXPlusWitdth = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector32.X;
							float numPosXPlusWitdthAbs = Math.Abs(numPosXPlusWitdth) * 0.1f;
							float numPosYPlusHeight = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector32.Y - numPosXPlusWitdthAbs;
							numPosXPlusWitdth += (float)Main.rand.Next(-40, 41);
							numPosYPlusHeight += (float)Main.rand.Next(-40, 41);
							float numSqrtPosX2PlusPosY2 = (float)Math.Sqrt(numPosXPlusWitdth * numPosXPlusWitdth + numPosYPlusHeight * numPosYPlusHeight);
							float numIs200f = 200f;
							if (numSqrtPosX2PlusPosY2 < numIs200f)
							{
								if (!Main.dedServ && alertSoundCounter == 0)
								{
									Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/DarkSoldierAlert"));
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
								npc.netUpdate = true;
								npc.velocity.X *= 0.5f;
								numSqrtPosX2PlusPosY2 = numIs10f / numSqrtPosX2PlusPosY2;
								numPosXPlusWitdth *= numSqrtPosX2PlusPosY2;
								numPosYPlusHeight *= numSqrtPosX2PlusPosY2;
								npc.ai[2] = 3f;
								npc.ai[1] = numIs70;
								if (Math.Abs(numPosYPlusHeight) > Math.Abs(numPosXPlusWitdth) * 2f)
								{
									if (numPosYPlusHeight > 0f)
									{
										npc.ai[2] = 1f;
									}
									else
									{
										npc.ai[2] = 5f;
									}
								}
								else if (Math.Abs(numPosXPlusWitdth) > Math.Abs(numPosYPlusHeight) * 2f)
								{
									npc.ai[2] = 3f;
								}
								else if (numPosYPlusHeight > 0f)
								{
									npc.ai[2] = 2f;
								}
								else
								{
									npc.ai[2] = 4f;
								}
							}
						}
					}
					if (npc.ai[2] <= 0f || (flagIfMartians && (numIsNeg1 == -1 || !(npc.ai[1] >= (float)numIsNeg1) || !(npc.ai[1] < (float)(numIsNeg1 + numIsAlsoNeg1)))))
					{
						float numIs1f = 1f;
						float numIs007f = 0.07f;
						float numIs08f = 0.8f;
						if (npc.velocity.X < 0f - numIs1f || npc.velocity.X > numIs1f)
						{
							if (npc.velocity.Y == 0f)
							{
								npc.velocity *= numIs08f;//When jumping and lands
							}
						}
						else if (npc.velocity.X < numIs1f && npc.direction == 1)
						{
							npc.velocity.X += numIs007f;
							if (npc.velocity.X > numIs1f)
							{
								npc.velocity.X = numIs1f; //Moving right. Setting a higher value does make them walk faster, but its super jittery
							}
						}
						else if (npc.velocity.X > 0f - numIs1f && npc.direction == -1)
						{
							npc.velocity.X -= numIs007f;
							if (npc.velocity.X < 0f - numIs1f)
							{
								npc.velocity.X = 0f - numIs1f; //moving left
							}
						}
					}
				}
			}
			bool flagTileChecker = false;
			if (npc.velocity.Y == 0f)
			{
				int numNPCPosYAndHeight = (int)(npc.position.Y + (float)npc.height + 7f) / 16;
				int numNPCPosX = (int)npc.position.X / 16;
				int numNPCPosXAndWidth = (int)(npc.position.X + (float)npc.width) / 16;
				for (int num174 = numNPCPosX; num174 <= numNPCPosXAndWidth; num174++)
				{
					if (Main.tile[num174, numNPCPosYAndHeight] == null)
					{
						return;
					}
					if (Main.tile[num174, numNPCPosYAndHeight].nactive() && Main.tileSolid[Main.tile[num174, numNPCPosYAndHeight].type])
					{
						flagTileChecker = true;
						break;
					}
				}
			}
			if (npc.velocity.Y >= 0f)
			{
				int numNPCVelocityX = 0;
				if (npc.velocity.X < 0f)
				{
					numNPCVelocityX = -1;//-1
				}
				if (npc.velocity.X > 0f)
				{
					numNPCVelocityX = 1;//1
				}
				Vector2 vectorNPCPos = npc.position;
				vectorNPCPos.X += npc.velocity.X;
				int numNPCPosXAndStuff = (int)((vectorNPCPos.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 1) * numNPCVelocityX)) / 16f);
				int numNPCPosYAndStuff = (int)((vectorNPCPos.Y + (float)npc.height - 1f) / 16f);
				if (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff] == null)
				{
					Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff] = new Tile();
				}
				if (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1] == null)
				{
					Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1] = new Tile();
				}
				if (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 2] == null)
				{
					Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 2] = new Tile();
				}
				if (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 3] == null)
				{
					Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 3] = new Tile();
				}
				if (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff + 1] == null)
				{
					Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff + 1] = new Tile();
				}
				if (Main.tile[numNPCPosXAndStuff - numNPCVelocityX, numNPCPosYAndStuff - 3] == null)
				{
					Main.tile[numNPCPosXAndStuff - numNPCVelocityX, numNPCPosYAndStuff - 3] = new Tile();
				}
				if ((float)(numNPCPosXAndStuff * 16) < vectorNPCPos.X + (float)npc.width && (float)(numNPCPosXAndStuff * 16 + 16) > vectorNPCPos.X && ((Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff].nactive() && !Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff].topSlope() && !Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].topSlope() && Main.tileSolid[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff].type] && !Main.tileSolidTop[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff].type]) || (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].halfBrick() && Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].nactive())) && (!Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].nactive() || !Main.tileSolid[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].type] || Main.tileSolidTop[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].type] || (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].halfBrick() && (!Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 4].nactive() || !Main.tileSolid[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 4].type] || Main.tileSolidTop[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 4].type]))) && (!Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 2].nactive() || !Main.tileSolid[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 2].type] || Main.tileSolidTop[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 2].type]) && (!Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 3].nactive() || !Main.tileSolid[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 3].type] || Main.tileSolidTop[Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 3].type]) && (!Main.tile[numNPCPosXAndStuff - numNPCVelocityX, numNPCPosYAndStuff - 3].nactive() || !Main.tileSolid[Main.tile[numNPCPosXAndStuff - numNPCVelocityX, numNPCPosYAndStuff - 3].type]))
				{
					float numNPCPosYTimes16 = numNPCPosYAndStuff * 16;
					if (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff].halfBrick())
					{
						numNPCPosYTimes16 += 8f;
					}
					if (Main.tile[numNPCPosXAndStuff, numNPCPosYAndStuff - 1].halfBrick())
					{
						numNPCPosYTimes16 -= 8f;
					}
					if (numNPCPosYTimes16 < vectorNPCPos.Y + (float)npc.height)
					{
						float numNPCPosYPlusHeight = vectorNPCPos.Y + (float)npc.height - numNPCPosYTimes16;
						float numIs16_1f = 16.1f;
						if (numNPCPosYPlusHeight <= numIs16_1f)
						{
							npc.gfxOffY += npc.position.Y + (float)npc.height - numNPCPosYTimes16;
							npc.position.Y = numNPCPosYTimes16 - (float)npc.height;
							if (numNPCPosYPlusHeight < 9f)
							{
								npc.stepSpeed = 1f;//1f;
							}
							else
							{
								npc.stepSpeed = 2f;//2f;
							}
						}
					}
				}
			}
			if (flagTileChecker)
			{
				int numNPCPosXWidthDirect = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
				int numNPCPosYandHeight = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
				if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight] == null)
				{
					Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight] = new Tile();
				}
				if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1] == null)
				{
					Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1] = new Tile();
				}
				if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 2] == null)
				{
					Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 2] = new Tile();
				}
				if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 3] == null)
				{
					Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 3] = new Tile();
				}
				if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight + 1] == null)
				{
					Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight + 1] = new Tile();
				}
				if (Main.tile[numNPCPosXWidthDirect + npc.direction, numNPCPosYandHeight - 1] == null)
				{
					Main.tile[numNPCPosXWidthDirect + npc.direction, numNPCPosYandHeight - 1] = new Tile();
				}
				if (Main.tile[numNPCPosXWidthDirect + npc.direction, numNPCPosYandHeight + 1] == null)
				{
					Main.tile[numNPCPosXWidthDirect + npc.direction, numNPCPosYandHeight + 1] = new Tile();
				}
				if (Main.tile[numNPCPosXWidthDirect - npc.direction, numNPCPosYandHeight + 1] == null)
				{
					Main.tile[numNPCPosXWidthDirect - npc.direction, numNPCPosYandHeight + 1] = new Tile();
				}
				Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight + 1].halfBrick();
				if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].nactive() && (TileLoader.IsClosedDoor(Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1]) || Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].type == 388) && flagIfCorrectEnemyFalse)
				{
					npc.ai[2] += 1f;
					npc.ai[3] = 0f;
					if (npc.ai[2] >= 60f)
					{
						npc.velocity.X = 0.5f * (float)(-npc.direction);
						int num183 = 5;
						if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].type == 388) //Tall Gate Closed
						{
							num183 = 2;
						}
						npc.ai[1] += num183;
						npc.ai[2] = 0f;
						bool flag23AI = false;
						if (npc.ai[1] >= 10f)
						{
							flag23AI = true;
							npc.ai[1] = 10f;
						}
						WorldGen.KillTile(numNPCPosXWidthDirect, numNPCPosYandHeight - 1, fail: true);
						if ((Main.netMode != NetmodeID.MultiplayerClient || !flag23AI) && flag23AI && Main.netMode != NetmodeID.MultiplayerClient)
						{
							if (TileLoader.OpenDoorID(Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1]) >= 0)
							{
								bool flag24 = WorldGen.OpenDoor(numNPCPosXWidthDirect, numNPCPosYandHeight - 1, npc.direction);
								if (!flag24)
								{
									npc.ai[3] = numIs60;
									npc.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag24)
								{
									NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 0, numNPCPosXWidthDirect, numNPCPosYandHeight - 1, npc.direction); 
								}
							}
							if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].type == 388) //Tall Gate Closed
							{
								bool flag25 = WorldGen.ShiftTallGate(numNPCPosXWidthDirect, numNPCPosYandHeight - 1, closing: false);
								if (!flag25)
								{
									npc.ai[3] = numIs60;
									npc.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag25)
								{
									NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 4, numNPCPosXWidthDirect, numNPCPosYandHeight - 1);
								}
							}
						}
					}
				}
				else
				{
					int spriteDirection = npc.spriteDirection;
					if ((npc.velocity.X < 0f && spriteDirection == -1) || (npc.velocity.X > 0f && spriteDirection == 1))
					{
						if (npc.height >= 32 && Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 2].nactive() && Main.tileSolid[Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 2].type])
						{
							if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 3].nactive() && Main.tileSolid[Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 3].type])
							{
								npc.velocity.Y = -8f;
								npc.netUpdate = true;
							}
							else
							{
								npc.velocity.Y = -7f;
								npc.netUpdate = true;
							}
						}
						else if (Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].nactive() && Main.tileSolid[Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight - 1].type])
						{
							npc.velocity.Y = -6f;
							npc.netUpdate = true;
						}
						else if (npc.position.Y + (float)npc.height - (float)(numNPCPosYandHeight * 16) > 20f && Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight].nactive() && !Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight].topSlope() && Main.tileSolid[Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight].type])
						{
							npc.velocity.Y = -5f;
							npc.netUpdate = true;
						}
						else if (npc.directionY < 0 && (!Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight + 1].nactive() || !Main.tileSolid[Main.tile[numNPCPosXWidthDirect, numNPCPosYandHeight + 1].type]) && (!Main.tile[numNPCPosXWidthDirect + npc.direction, numNPCPosYandHeight + 1].nactive() || !Main.tileSolid[Main.tile[numNPCPosXWidthDirect + npc.direction, numNPCPosYandHeight + 1].type]))
						{
							npc.velocity.Y = -8f;
							npc.velocity.X *= 3f;//1.5
							npc.netUpdate = true;
						}
						else if (flagIfCorrectEnemyFalse)
						{
							npc.ai[1] = 0f;
							npc.ai[2] = 0f;
						}
						if (npc.velocity.Y == 0f && flagVelocityX0OrJustHit && npc.ai[3] == 1f)
						{
							npc.velocity.Y = -5f;
						}
					}
					//The little jumpy thing when the player is close and the enemy is standing still
					if (npc.velocity.Y == 0f && Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) < 100f && Math.Abs(npc.position.Y + (float)(npc.height / 2) - (Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2))) < 50f && ((npc.direction > 0 && npc.velocity.X >= 1f) || (npc.direction < 0 && npc.velocity.X <= -1f)))
					{
						npc.velocity.X *= 2f;
						if (npc.velocity.X > 3f)
						{
							npc.velocity.X = 3f;
						}
						if (npc.velocity.X < -3f)
						{
							npc.velocity.X = -3f;
						}
						npc.velocity.Y = -4f;
						npc.netUpdate = true;
					}
				}
			}
			else if (flagIfCorrectEnemyFalse)
			{
				npc.ai[1] = 0f;
				npc.ai[2] = 0f;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient || true || !(npc.ai[3] >= (float)numIs60))
			{
				return;
			}
			int numPlayerTargetX = (int)Main.player[npc.target].position.X / 16;
			int numPlayerTargetY = (int)Main.player[npc.target].position.Y / 16;
			int numIsNPCPosX = (int)npc.position.X / 16;
			int numIsNPCPosY = (int)npc.position.Y / 16;
			int numIs20 = 20;
			int numIs0 = 0;
			bool flag26False = false;
			if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
			{
				numIs0 = 100;
				flag26False = true;
			}
			while (!flag26False && numIs0 < 100)
			{
				numIs0++;
				int numIsRandPlrTarX = Main.rand.Next(numPlayerTargetX - numIs20, numPlayerTargetX + numIs20);
				for (int numIsRandPlrTarY = Main.rand.Next(numPlayerTargetY - numIs20, numPlayerTargetY + numIs20); numIsRandPlrTarY < numPlayerTargetY + numIs20; numIsRandPlrTarY++)
				{
					if ((numIsRandPlrTarY < numPlayerTargetY - 4 || numIsRandPlrTarY > numPlayerTargetY + 4 || numIsRandPlrTarX < numPlayerTargetX - 4 || numIsRandPlrTarX > numPlayerTargetX + 4) && (numIsRandPlrTarY < numIsNPCPosY - 1 || numIsRandPlrTarY > numIsNPCPosY + 1 || numIsRandPlrTarX < numIsNPCPosX - 1 || numIsRandPlrTarX > numIsNPCPosX + 1) && Main.tile[numIsRandPlrTarX, numIsRandPlrTarY].nactive())
					{
						bool flagLava = true;
						if (Main.tile[numIsRandPlrTarX, numIsRandPlrTarY - 1].lava())
						{
							flagLava = false;
						}
						if (flagLava && Main.tileSolid[Main.tile[numIsRandPlrTarX, numIsRandPlrTarY].type] && !Collision.SolidTiles(numIsRandPlrTarX - 1, numIsRandPlrTarX + 1, numIsRandPlrTarY - 4, numIsRandPlrTarY - 1))
						{
							npc.position.X = numIsRandPlrTarX * 16 - npc.width / 2;
							npc.position.Y = numIsRandPlrTarY * 16 - npc.height;
							npc.netUpdate = true;
							npc.ai[3] = -120f;
						}
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.player.ZoneUnderworldHeight) //Underworld
			{
				return 0.3f;
			}
			if ((spawnInfo.spawnTileY <= Main.maxTilesY - 200 && spawnInfo.spawnTileY > (Main.rockLayer + Main.maxTilesY - 200) / 2)) //lower half of the caverns?
            {
				return 0.08f;
            }
			else
			{
				return 0f;
			}
		}
	}
}