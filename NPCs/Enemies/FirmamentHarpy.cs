using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.ItemDropRules;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;
using System.IO;

namespace RijamsMod.NPCs.Enemies
{
	class FirmamentHarpy : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Firmament Harpy");
			Main.npcFrameCount[NPC.type] = 12;

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
			NPC.CloneDefaults(NPCID.Harpy);
			NPC.width = 24;
			NPC.height = 34;
			NPC.damage = 70;
			NPC.defense = 16;
			NPC.lifeMax = 400;
			NPC.buffImmune[BuffID.Confused] = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 1000f;
			NPC.knockBackResist = 0.4f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Placeable.EnemyBanners.FirmamentHarpyBanner>();
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Helmet").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Wing").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Leg").Type, 1f);
				}
			}
		}

		public int AIState = 0;
		// 0: normal flying
		// 1: attacking
		public int timesAttacked = 0;
		public bool forceRelocate = false;

		// Copied from aiStyle 62 (Elf Copter) and modified
		public override void AI()
		{
			// Find target
			NPC.TargetClosest();
			NPC.FaceTarget();
			NPC.rotation = Math.Abs(NPC.velocity.X) * NPC.direction * 0.1f;
			NPC.spriteDirection = NPC.direction;
			float num1577 = 7f;
			Vector2 npcNewCenter = new(NPC.Center.X + (NPC.direction * 30), NPC.Center.Y - 10f);
			Player target = Main.player[NPC.target];
			float targetPosX = target.position.X + target.width * 0.5f - npcNewCenter.X;
			float targetPosY = target.position.Y - npcNewCenter.Y;
			float targetDist = (float)Math.Sqrt(targetPosX * targetPosX + targetPosY * targetPosY);
			float num1582 = num1577 / targetDist;
			targetPosX *= num1582;
			targetPosY *= num1582;

			// If the Harpy has attacked 3 times (2 times in Expert Mode), force it to move to another position
			if (timesAttacked >= (Main.expertMode ? 2 : 3))
			{
				forceRelocate = true;
				timesAttacked = 0;
				NPC.netUpdate = true;
			}
			else
			{
				forceRelocate = false;
				NPC.netUpdate = true;
			}

			// If the player is far away, not in the line of sight, or forced to move, then move to the player.
			bool canHit = Collision.CanHit(NPC.Center, 1, 1, target.Center, 1, 1);
			if (targetDist > 800f || !canHit || forceRelocate)
			{
				int inertia = 60;

				// Move much more if forced to relocate
				int min = forceRelocate ? -100 : -20;
				int max = forceRelocate ? 201 : 21;
				// Random added so groups don't bunch up in the same spot
				NPC.velocity.X = (NPC.velocity.X * (inertia - 1) + targetPosX + Main.rand.Next(min, max)) / (float)inertia;
				NPC.velocity.Y = (NPC.velocity.Y * (inertia - 1) + targetPosY + Main.rand.Next(min, max)) / (float)inertia;

				// Stop attacking if it was attacking before.
				NPC.localAI[0] = 0;
				NPC.localAI[1] = 0;
				AIState = 0;
				NPC.netUpdate = true;
				return;
			}

			// Slow down, even more so if wet.
			NPC.velocity *= 0.98f;
			if (NPC.wet || NPC.honeyWet || NPC.lavaWet)
			{
				NPC.velocity *= 0.95f;
			}

			// If moving very slowly or not moving at all.
			if (Math.Abs(NPC.velocity.X) < 1f && Math.Abs(NPC.velocity.Y) < 1f && Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (NPC.localAI[0] <= 0)
				{
					NPC.localAI[0] = 0f;
					NPC.localAI[1]++;
					AIState = 1;
					NPC.netUpdate = true;
					if (NPC.localAI[1] % 10 == 0) // Shoot 6 times with a 10 tick delay between shots
					{
						targetPosX = target.position.X + (float)target.width * 0.5f - npcNewCenter.X;
						targetPosY = target.Center.Y - npcNewCenter.Y;
						targetPosX += (float)Main.rand.Next(-35, 36);
						targetPosY += (float)Main.rand.Next(-35, 36);
						targetPosX *= 1f + (float)Main.rand.Next(-20, 21) * 0.015f;
						targetPosY *= 1f + (float)Main.rand.Next(-20, 21) * 0.015f;
						targetDist = (float)Math.Sqrt(targetPosX * targetPosX + targetPosY * targetPosY);
						num1577 = 10f;
						num1582 = num1577 / targetDist;
						targetPosX *= num1582;
						targetPosY *= num1582;
						targetPosX *= 1f + (float)Main.rand.Next(-10, 11) * 0.0125f;
						targetPosY *= 1f + (float)Main.rand.Next(-10, 11) * 0.0125f;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), npcNewCenter, new Vector2(targetPosX, targetPosY),
							ModContent.ProjectileType<Projectiles.Enemies.FirmamentHarpyFeather>(), 25, 2f, Main.myPlayer);
					}
					if (NPC.localAI[1] >= 60)
					{
						NPC.localAI[0] = 120; // 2 second delay before attacking again.
						timesAttacked++; // increment the number of times it has attacked.
					}
				}
				else
				{
					NPC.localAI[0]--;
					NPC.localAI[1] = 0;
					AIState = 0;
					NPC.netUpdate = true;
				}
			}
		}

		// Animations
		// 0-5 normal flying
		// 6-11 attacking
		private const int Frame_Fly1 = 0;
		private const int Frame_Fly2 = 1;
		private const int Frame_Fly3 = 2;
		private const int Frame_Fly4 = 3;
		private const int Frame_Fly5 = 4;
		private const int Frame_Fly6 = 5;
		private const int Frame_Attack = 6;

		public override void FindFrame(int frameHeight)
		{
			int frameSpeed = 5;
			NPC.frameCounter++;
			int addAttack = AIState == 1 ? Frame_Attack : 0; // If attacking, then simply add 6 to the frame
			if (NPC.frameCounter < frameSpeed)
			{
				NPC.frame.Y = (Frame_Fly1 + addAttack) * frameHeight;
			}
			else if (NPC.frameCounter < frameSpeed * 2)
			{
				NPC.frame.Y = (Frame_Fly2 + addAttack) * frameHeight;
			}
			else if (NPC.frameCounter < frameSpeed * 3)
			{
				NPC.frame.Y = (Frame_Fly3 + addAttack) * frameHeight;
			}
			else if (NPC.frameCounter < frameSpeed * 4)
			{
				NPC.frame.Y = (Frame_Fly4 + addAttack) * frameHeight;
			}
			else if (NPC.frameCounter < frameSpeed * 5)
			{
				NPC.frame.Y = (Frame_Fly5 + addAttack) * frameHeight;
			}
			else if (NPC.frameCounter < frameSpeed * 6)
			{
				NPC.frame.Y = (Frame_Fly6 + addAttack) * frameHeight;
			}
			else
			{
				NPC.frameCounter = 0;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (AIState == 1)
			{
				if (Main.expertMode)
				{
					target.AddBuff(BuffID.Bleeding, 1200);
				}
				target.AddBuff(ModContent.BuffType<Buffs.Debuffs.BleedingOut>(), 300);
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(AIState);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			AIState = reader.ReadInt32();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = TextureAssets.Npc[Type].Value;
			SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 0);
			return false;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.ChickenNugget, 50));
			npcLoot.Add(ItemDropRule.Common(ItemID.Feather, 2, 1, 2));
			npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Materials.GiantRedHarpyFeather>(), 200, 150)); //0.5% chance normal mode, 0.67% chance in expert
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SunEssence>(), 1, 1, 4));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Sky && NPC.downedGolemBoss) // Sky & Golem defeated
			{
				return 0.3f;
			}
			return 0f;
		}
	}
}