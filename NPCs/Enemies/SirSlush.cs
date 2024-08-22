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
using System.IO;

namespace RijamsMod.NPCs.Enemies
{
	public class SirSlush : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.BelongsToInvasionFrostLegion[NPC.type] = true; // Make it count towards Frost Legion for music playback and invasion progress
			NPCID.Sets.InvasionSlotCount[NPC.type] = 1; // Make it count as 1 enemies defeated for the invasion progress
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frozen] = true; // Frozen or Chilled don't do anything to NPCs I don't think.
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Chilled] = true;

			// DisplayName.SetDefault("Sir Slush");
			Main.npcFrameCount[NPC.type] = 12;
			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
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
			/*if (Main.invasionType == InvasionID.SnowLegion)
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
			}*/
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (SpawnCondition.FrostLegion.Active)
			{
				return SpawnCondition.FrostLegion.Chance * 0.15f;
			}
			return 0;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(Entity.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Hat").Type, 1f);
				for (int k = 0; k < 5; k++)
				{
					Gore.NewGore(Entity.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name).Type, 1f);
				}
			}
		}

		public ref float AIState => ref NPC.ai[1];
		// 0 == idle
		// 1 == alert
		// 2 == attack

		public override void AI()
		{
			NPC.ai[0]++;
			// Main.NewText("npc.ai[0] " + npc.ai[0]);
			// Main.NewText("AIState " + AIState);

			float requiredDistance = Main.expertMode ? 1200f : 800f;

			if (AIState == 0) // idle
			{
				NPC.TargetClosest();
				NPC.FaceTarget();
				bool lineOfSight = Collision.CanHitLine(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
				float distance = Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) + Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y);
				// Can attack the player
				if (NPC.ai[0] >= 30 && NPC.HasValidTarget && Main.netMode != NetmodeID.Server && distance <= requiredDistance && lineOfSight)
				{
					NPC.ai[0] = 0;
					SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/SirSlushAlert") { MaxInstances = 5 }, NPC.position);
					AIState = 1;
					NPC.netUpdate = true;
				}
				// Too far away from the player. Move forward very slowly.
				else if (distance > requiredDistance)
				{
					float acceleration = Main.getGoodWorld ? 1.1f : Main.expertMode ? 0.105f : 0.1f;
					NPC.velocity.X += acceleration * NPC.direction;
					Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y + (NPC.height * 0.9f)), NPC.width, 1, DustID.Snow, SpeedX: NPC.velocity.X, SpeedY: 0);
					dust.noGravity = true;

					// Step up single tiles and half tiles.
					Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
				}
				// Doesn't take any damage while idle.
				NPC.immortal = true;
				// NPC.reflectsProjectiles = true;
				NPC.dontTakeDamage = true; // Needs to be false or the projectiles won't reflect. But then you still see the damage numbers even though the NPC takes no damage.
				NPC.DiscourageDespawn(60);
			}
			else if (AIState == 1) // alert
			{
				NPC.FaceTarget();
				float distance = Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) + Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y);
				// Can attack the player
				if (NPC.ai[0] == 80 && NPC.HasValidTarget && Main.netMode != NetmodeID.Server && distance <= requiredDistance)
				{
					NPC.ai[0] = 0;
					NPC.frameCounter = 0;
					AIState = 2; // attack
					NPC.netUpdate = true;
				}
				// Can't attack the player, go back to idling.
				else if (NPC.ai[0] > 80 || distance > requiredDistance)
				{
					NPC.ai[0] = 0;
					AIState = 0; // idle
					NPC.netUpdate = true;
				}
				// Can take damage while creating a snowball
				NPC.immortal = false;
				// NPC.reflectsProjectiles = false;
				NPC.dontTakeDamage = false;
			}
			else if (AIState == 2) // attack
			{
				NPC.FaceTarget();
				// Wait 20 ticks before the projectile is created.
				if (NPC.ai[0] == 20)
				{
					Vector2 positionForProj = NPC.Center;
					float projSpeedX = Main.player[NPC.target].Center.X - positionForProj.X;
					//float projSpeedXAbs; //= Math.Abs(projSpeedX) * 0.1f;
					float projSpeedXAbs = Math.Abs(projSpeedX) * 0.05f;
					float projSpeedY = Main.player[NPC.target].Center.Y - positionForProj.Y - projSpeedXAbs;
					float speedDistance = (float)Math.Sqrt(projSpeedX * projSpeedX + projSpeedY * projSpeedY);
					NPC.netUpdate = true;
					speedDistance = 10f / speedDistance;
					float speedMulti = Main.expertMode ? 1.3f : 1.1f;
					projSpeedX *= speedDistance * speedMulti;
					projSpeedY *= speedDistance * speedMulti;
					int projDamage = 30;
					int projType = ModContent.ProjectileType<Projectiles.Enemies.SirSlushSnowball>();
					positionForProj += new Vector2(projSpeedX, projSpeedY);
					if (NPC.confused)
					{
						projSpeedX *= -1f;
					}
					if (!Main.dedServ)
					{
						SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/SirSlushThrow") { MaxInstances = 10 }, NPC.position);
						Projectile.NewProjectile(Entity.GetSource_FromAI(), positionForProj, new Vector2(projSpeedX, projSpeedY), projType, projDamage, 4f, Main.myPlayer);
					}
				}
				// Wait 20 ticks after the projectile has been created. Go back to idling.
				if (NPC.ai[0] >= 40)
				{
					NPC.ai[0] = 0;
					AIState = 0; // idle
					NPC.netUpdate = true;
				}
				// Can take damage while throwing snowball
				NPC.immortal = false;
				// NPC.reflectsProjectiles = false;
				NPC.dontTakeDamage = false;
			}
			else
			{
				AIState = 0; // idle
				NPC.netUpdate = true;
			}
		}


		// Animations
		// 0-3 idle
		// 4-7 alert
		// 8-11 attack
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
			if (AIState == 0) // idle
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
			else if (AIState == 1) // alert
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
			else if (AIState == 2) // attack
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
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(AIState);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			AIState = reader.ReadInt32();
		}
		*/

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