using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.NPCs.Enemies
{
	public class AngryGuster : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 8;

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
			NPC.width = 40;
			NPC.height = 38;
			NPC.friendly = false;
			NPC.aiStyle = -1;
			NPC.noTileCollide = false;
			NPC.damage = 20;
			NPC.lifeMax = 100;
			NPC.HitSound = SoundID.NPCHit30;
			NPC.DeathSound = SoundID.NPCDeath33;
			NPC.value = 1000;
			NPC.knockBackResist = 0.25f;
			NPC.noGravity = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Placeable.EnemyBanners.AngryGusterBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.WindyDay,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
			});
		}

		public override bool? CanFallThroughPlatforms() => true;

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < 3; i++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, GoreID.Smoke1 + i);
				}
			}
		}

		public override void AI()
		{
			NPC.TargetClosest();
			NPC.FaceTarget();
			NPC.rotation = Math.Abs(NPC.velocity.X) * NPC.direction * 0.05f;
			NPC.spriteDirection = NPC.direction;

			Player target = Main.player[NPC.target];

			NPC.ai[0]++;
			Vector2 targetPos = (target.Center - NPC.Center);
			float targetDist = (float)Math.Sqrt(targetPos.X * targetPos.X + targetPos.Y * targetPos.Y);

			if (NPC.target >= 0)
			{
				// Close to the target
				if (targetDist < 8 * 16f)
				{
					int attacking = NPC.ai[1] == 1f ? 0 : 1;
					NPC.velocity.Y += (float)Math.Sin(NPC.ai[0] / 5f) / 10f * attacking;
					NPC.velocity *= 0.9f;
				}
				else
				{
					int inertia = 40;
					Vector2 npcNewCenter = new(NPC.Center.X + (NPC.direction * 30), NPC.Center.Y - 10f);
					float targetPosX = target.position.X + target.width * 0.5f - npcNewCenter.X;
					float targetPosY = target.position.Y - npcNewCenter.Y;
					float newTargetDist = (float)Math.Sqrt(targetPosX * targetPosX + targetPosY * targetPosY);

					targetPosX *= 7f / newTargetDist;
					targetPosY *= 7f / newTargetDist;
					NPC.velocity.X = (NPC.velocity.X * (inertia - 1) + targetPosX + Main.rand.Next(-5, 5)) / (float)inertia;
					NPC.velocity.Y = (NPC.velocity.Y * (inertia - 1) + targetPosY + Main.rand.Next(-5, 5)) / (float)inertia;

					if (NPC.localAI[1] > 5f)
					{
						NPC.localAI[1]++;
					}
				}
			}

			// Slow down, even more so if wet.
			if (NPC.wet || NPC.honeyWet || NPC.lavaWet)
			{
				NPC.velocity *= 0.95f;
			}

			// Once close enough to the target, attack
			if (targetDist < 10 * 16f)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.localAI[1]++;
					if (NPC.localAI[1] == 1f && Collision.CanHitLine(NPC.Center, 16, 16, target.Center, 16, 16))
					{
						NPC.ai[1] = 1f;
						Vector2 velocity = targetPos;
						velocity.Normalize();
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(8 * NPC.direction, 8), velocity, ModContent.ProjectileType<Projectiles.Enemies.WindGust>(), 5, 32f);

						ModContent.GetInstance<RijamsMod>().PlayNetworkSound(SoundID.Item45 with { Pitch = -1f }, NPC.Center, Main.player[Main.myPlayer]);
						ModContent.GetInstance<RijamsMod>().PlayNetworkSound("Terraria/Sounds/Custom/dd2_dark_mage_attack_0", volume: 1f, pitch: -1f, NPC.Center, Main.player[Main.myPlayer]);
						NPC.velocity -= new Vector2(NPC.direction * 0.5f, 0);
						NPC.netUpdate = true;
					}
					if (NPC.localAI[1] >= 30f)
					{
						NPC.ai[1] = 0f;
					}
					if (NPC.localAI[1] >= 60f)
					{
						NPC.localAI[1] = 0f;
						NPC.ai[1] = 0f;
					}
				}
			}
			else
			{
				NPC.ai[1] = 0f;
			}
		}

		// Animations
		// 0-3 idle
		// 4-8 attacking
		private const int Frame_Idle1 = 0;
		private const int Frame_Idle2 = 1;
		private const int Frame_Idle3 = 2;
		private const int Frame_Idle4 = 3;
		private const int Frame_Attack = 4;

		public override void FindFrame(int frameHeight)
		{
			int frameSpeed = 10;
			NPC.frameCounter++;
			// If attacking, then simply add 4 to the frame
			int addAttack = 0; 
			if (NPC.ai[1] is 1f or 1f + float.Epsilon or 1f - float.Epsilon)
			{
				addAttack += Frame_Attack;
			}
			if (NPC.frameCounter < frameSpeed)
			{
				NPC.frame.Y = (Frame_Idle1 + addAttack) * frameHeight;
			}
			else if (NPC.frameCounter < frameSpeed * 2)
			{
				NPC.frame.Y = (Frame_Idle2 + addAttack) * frameHeight;
			}
			else if (NPC.frameCounter < frameSpeed * 3)
			{
				NPC.frame.Y = (Frame_Idle3 + addAttack) * frameHeight;
			}
			else if (NPC.frameCounter < frameSpeed * 4)
			{
				NPC.frame.Y = (Frame_Idle4 + addAttack) * frameHeight;
			}
			else
			{
				NPC.frameCounter = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = TextureAssets.Npc[Type].Value;
			SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor * 0.75f, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 0);

			return false;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Magic.MiniGuster>(), 7));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.CountNPCS(Type) > 3)
			{
				return 0f;
			}
			if (spawnInfo.Player.position.Y > Main.worldSurface * 16) // Below the surface. * 16 because Player.position is in pixels while Main.worldSurface is in tiles.
			{
				return 0f;
			}

			float spawnChance = 0f;

			if (spawnInfo.Sky)
			{
				spawnChance += 0.25f;
			}
			if (Main.IsItAHappyWindyDay)
			{
				spawnChance += 0.08f;
			}
			if (Main.WindyEnoughForKiteDrops)
			{
				spawnChance += 0.02f;
			}
			if (Condition.InRain.IsMet())
			{
				spawnChance += 0.1f;
			}
			return spawnChance;
		}
	}
}