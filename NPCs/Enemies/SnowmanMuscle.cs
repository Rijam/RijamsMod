using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace RijamsMod.NPCs.Enemies
{
	class SnowmanMuscle : ModNPC
	{
		//public override string Texture => "Terraria/NPC_" + NPCID.GreekSkeleton;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snowman Muscle");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.SnowmanGangsta];
		}

		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.SnowmanGangsta);
			npc.damage = 30;
			npc.defense = 0;
			npc.lifeMax = 250;
			npc.lavaImmune = true;
			npc.buffImmune[BuffID.Confused] = false;
			npc.value = 500f;
			npc.knockBackResist = 0.5f;
			npc.aiStyle = -1;
			aiType = NPCID.SnowmanGangsta;
			animationType = NPCID.SnowmanGangsta;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Placeable.SnowmanMuscleBanner>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(20) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Shotgun);
			}
			if (Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessories.Vanity.CarrotNose>());
			}
			if (Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessories.FrostyRose>());
			}
			if (Main.rand.Next(150) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Present);
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SnowBlock, Main.rand.Next(5, 10));

			//From Spirit Mod FrostSaucer.cs
			if (Main.invasionType == 2)
			{
				Main.invasionSize -= 1;
				if (Main.invasionSize < 0)
                {
					Main.invasionSize = 0;
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
                {
					Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, 1, 0);
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
				npc.ai[2] += 1f;
				if (npc.ai[2] >= 180f)
				{
					npc.ai[2] = 0f;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 projectileVector = new Vector2(npc.position.X + (float)npc.width * 0.5f - (float)(npc.direction * 12), npc.position.Y + (float)npc.height * 0.5f);
						float speedX = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - projectileVector.X;
						float speedY = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - projectileVector.Y;
						float sqrtX2Y2 = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						npc.netUpdate = true;
						sqrtX2Y2 = 9f / sqrtX2Y2;
						speedX *= sqrtX2Y2;
						speedY *= sqrtX2Y2;
						projectileVector.X += speedX;
						projectileVector.Y += speedY;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int projectileDamage = 15;
							int projectileType = ProjectileID.BulletSnowman;
							for (int i=0; i < 3; i++)
                            {
								int newProjectile = Projectile.NewProjectile(projectileVector.X, projectileVector.Y, speedX, speedY, projectileType, projectileDamage, 0.5f, Main.myPlayer);
								Main.projectile[newProjectile].ai[0] = 2f;
								Main.projectile[newProjectile].timeLeft = 300;
								Main.projectile[newProjectile].friendly = false;
								NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, newProjectile);
								npc.netUpdate = true;
								speedY += Main.rand.Next(-60, 60) * sqrtX2Y2;
								speedX += Main.rand.Next(-20, 20) * sqrtX2Y2;
							}
						}
					}
				}
			}
			if (true)
			{
				if (npc.velocity.Y == 0f)
				{
					if (npc.localAI[2] == npc.position.X)
					{
						npc.direction *= -1;
						npc.ai[3] = 60f;
					}
					npc.localAI[2] = npc.position.X;
					if (npc.ai[3] == 0f)
					{
						npc.TargetClosest();
					}
					npc.ai[0] += 1f;
					if (npc.ai[0] > 2f)
					{
						npc.ai[0] = 0f;
						npc.ai[1] += 1f;
						npc.velocity.Y = -8.2f;
						npc.velocity.X += (float)npc.direction * num526 * 1.1f;
					}
					else
					{
						npc.velocity.Y = -6f;
						npc.velocity.X += (float)npc.direction * num526 * 0.9f;
					}
					npc.spriteDirection = npc.direction;
				}
				npc.velocity.X += (float)npc.direction * num526 * 0.01f;
			}
			if (npc.ai[3] > 0f)
			{
				npc.ai[3] -= 1f;
			}
			if (npc.velocity.X > speed && npc.direction > 0)
			{
				npc.velocity.X = 4f;
			}
			if (npc.velocity.X < -speed && npc.direction < 0)
			{
				npc.velocity.X = -4f;
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