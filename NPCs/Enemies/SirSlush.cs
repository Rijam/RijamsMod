using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.Utilities;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace RijamsMod.NPCs.Enemies
{
	public class SirSlush : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sir Slush");
			Main.npcFrameCount[NPC.type] = 12;
			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				PortraitPositionYOverride = 3
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.height = 82; //52
			NPC.damage = 1;
			NPC.defense = 0;
			NPC.lifeMax = 600;
			NPC.buffImmune[BuffID.Confused] = false;
			NPC.HitSound = SoundID.NPCHit11;
			NPC.DeathSound = SoundID.NPCDeath15;
			NPC.value = 10000f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 0; //0 will face the player
			NPC.dontTakeDamage = false;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Placeable.EnemyBanners.SirSlushBanner>();
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
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.Defense.FrostyRose>(), 25));
			npcLoot.Add(ItemDropRule.Common(ItemID.Present, 50));
			npcLoot.Add(ItemDropRule.Common(ItemID.HandWarmer, 50));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.Vanity.SirSlushsTopHat>(), 3));
			npcLoot.Add(ItemDropRule.Common(ItemID.SlushBlock, 1, 10, 20));

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
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (SpawnCondition.FrostLegion.Active)
			{
				return SpawnCondition.FrostLegion.Chance * 0.1f;
			}
			else
			{
				return 0;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(Entity.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Hat").Type, 1f);
				for (int k = 0; k < 5; k++)
				{
					Gore.NewGore(Entity.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore").Type, 1f);
				}
			}
		}

		public int AIState = 0;
		//0 == idle
		//1 == alert
		//2 == attack

		
		public override void AI()
		{
			NPC.ai[0]++;
			//Main.NewText("npc.ai[0] " + npc.ai[0]);
			//Main.NewText("AIState " + AIState);
			if (AIState == 0) //idle
			{
				NPC.TargetClosest();
				NPC.FaceTarget();
				bool lineOfSight = Collision.CanHitLine(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
				float distance = Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y);
				if (NPC.ai[0] >= 30 && NPC.HasValidTarget && Main.netMode != NetmodeID.Server && distance <= 1000f && lineOfSight)
				{
					NPC.ai[0] = 0;
					SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/SirSlushAlert") { MaxInstances = 5 }, NPC.position);
					AIState = 1;
					NPC.netUpdate = true;
				}
			}
			else if (AIState == 1) //alert
			{
				NPC.FaceTarget();
				float distance = Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y);
				if (NPC.ai[0] == 80 && NPC.HasValidTarget && Main.netMode != NetmodeID.Server && distance <= 1000f)
				{
					NPC.ai[0] = 0;
					NPC.frameCounter = 0;
					AIState = 2;
					NPC.netUpdate = true;
				}
				else if (NPC.ai[0] > 80 || distance > 1000f)
				{
					NPC.ai[0] = 0;
					AIState = 0;
					NPC.netUpdate = true;
				}
			}
			else if (AIState == 2) //attack
			{
				NPC.FaceTarget();
				if (NPC.ai[0] == 20)
				{
					Vector2 vectoryForProj = new(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
					float projSpeedX = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - vectoryForProj.X;
					float projSpeedXAbs; //= Math.Abs(projSpeedX) * 0.1f;
					projSpeedXAbs = Math.Abs(projSpeedX) * Main.rand.Next(10, 20) * 0.01f;
					float projSpeedY = Main.player[NPC.target].position.Y + Main.player[NPC.target].height * 0.5f - vectoryForProj.Y - projSpeedXAbs;
					float sqrtXto2PlusYto2 = (float)Math.Sqrt(projSpeedX * projSpeedX + projSpeedY * projSpeedY);
					NPC.netUpdate = true;
					sqrtXto2PlusYto2 = 10f / sqrtXto2PlusYto2;
					projSpeedX *= sqrtXto2PlusYto2;
					projSpeedY *= sqrtXto2PlusYto2;
					int projDamage = 30;
					int projType = ModContent.ProjectileType<Projectiles.Enemies.SirSlushSnowball>();
					vectoryForProj.X += projSpeedX;
					vectoryForProj.Y += projSpeedY;
					if (!Main.dedServ)
					{
						SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/SirSlushThrow") { MaxInstances = 10 }, NPC.position);
						Projectile.NewProjectile(Entity.GetSource_FromAI(), vectoryForProj.X, vectoryForProj.Y, projSpeedX, projSpeedY, projType, projDamage, 4f, Main.myPlayer);
					}
				}
				if (NPC.ai[0] >= 40)
				{
					NPC.ai[0] = 0;
					AIState = 0;
					NPC.netUpdate = true;
				}
			}
			else
			{
				AIState = 0;
				NPC.netUpdate = true;
			}
		}


		//Animations
		//0-3 idle
		//4-7 alert
		//8-11 attack
		private const int Frame_Idle1 = 0;
		private const int Frame_Idle2 = 1;
		private const int Frame_Idle3 = 2;
		private const int Frame_Idle4 = 3;
		private const int Frame_Alert1 = 4;
		private const int Frame_Alert2 = 5;
		private const int Frame_Alert3 = 6;
		private const int Frame_Alert4 = 7;
		private const int Frame_Attack1 = 8;
		private const int Frame_Attack2 = 9;
		private const int Frame_Attack3 = 10;
		private const int Frame_Attack4 = 11;

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (AIState == 0) //idle
			{
				if (NPC.frameCounter < 10)
				{
					NPC.frame.Y = Frame_Idle1 * frameHeight;
				}
				else if (NPC.frameCounter < 20)
				{
					NPC.frame.Y = Frame_Idle2 * frameHeight;
				}
				else if (NPC.frameCounter < 30)
				{
					NPC.frame.Y = Frame_Idle3 * frameHeight;
				}
				else if (NPC.frameCounter < 40)
				{
					NPC.frame.Y = Frame_Idle4 * frameHeight;
				}
				else
				{
					NPC.frameCounter = 0;
				}
			}
			else if (AIState == 1) //alert
			{
				if (NPC.frameCounter < 10)
				{
					NPC.frame.Y = Frame_Alert1 * frameHeight;
				}
				else if (NPC.frameCounter < 20)
				{
					NPC.frame.Y = Frame_Alert2 * frameHeight;
				}
				else if (NPC.frameCounter < 30)
				{
					NPC.frame.Y = Frame_Alert3 * frameHeight;
				}
				else if (NPC.frameCounter <= 40)
				{
					NPC.frame.Y = Frame_Alert4 * frameHeight;
				}
				else
				{
					NPC.frameCounter = 0;
				}
			}
			else if (AIState == 2) //attack
			{
				if (NPC.frameCounter < 10)
				{
					NPC.frame.Y = Frame_Attack1 * frameHeight;
				}
				else if (NPC.frameCounter < 20)
				{
					NPC.frame.Y = Frame_Attack2 * frameHeight;
				}
				else if (NPC.frameCounter < 30)
				{
					NPC.frame.Y = Frame_Attack3 * frameHeight;
				}
				else if (NPC.frameCounter < 40)
				{
					NPC.frame.Y = Frame_Attack4 * frameHeight;
				}
				else
				{
					NPC.frameCounter = 0;
				}
			}
		}
		/*
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			Rectangle hatHitbox = new(0, 0, 28, 30)
			{
				X = (int)NPC.position.X,
				Y = (int)NPC.position.Y
			};
			// Has flaws. Anything that uses custom collision (Jousting Lances, Whips) doesn't get detected.
			if (hatHitbox.Intersects(projectile.getRect()) && projectile.damage > 0 && projectile.friendly)
			{
				if (NPC.immune[projectile.owner] <= 0)
				{
					Main.NewText("Hit Projectile!");
					return true;
				}
				return false;
			}
			if (NPC.getRect().Intersects(projectile.getRect()) && projectile.friendly)
			{
				if (projectile.CanBeReflected())
				{
					projectile.velocity *= -1;
					projectile.friendly = false;
					projectile.hostile = true;
					if (NPC.immune[projectile.owner] <= 0)
					{
						SoundEngine.PlaySound(SoundID.Item150, NPC.position);
					}
				}
				return false;
			}
			return false;
		}
		public override bool? CanBeHitByItem(Player player, Item item)
		{
			Rectangle hatHitbox = new(0, 0, 28, 30)
			{
				X = (int)NPC.position.X - NPC.width / 2,
				Y = (int)NPC.position.Y - NPC.height / 2
			};
			// Doesn't work. Need to get the melee hitbox: https://github.com/JavidPack/ModdersToolkit/blob/1.4/Tools/Hitboxes/HitboxesTool.cs
			if (hatHitbox.Intersects(item.getRect()) && item.damage > 0)
			{
				Main.NewText("Hit Melee!");
				return true;
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Rectangle hatHitbox = new(0, 0, 28, 30)
			{
				X = (int)NPC.position.X - NPC.width / 2,
				Y = (int)NPC.position.Y - NPC.height / 2
			};
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, NPC.position - screenPos, hatHitbox, Color.Orange * 0.6f);
		}*/
	}
}