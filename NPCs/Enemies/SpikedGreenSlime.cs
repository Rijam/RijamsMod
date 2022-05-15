using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.NPCs.Enemies
{
    public class SpikedGreenSlime : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiked Green Slime");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.SlimeSpiked];
		}

		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.SlimeSpiked);
			npc.damage = 40;
			npc.defense = 6;
			npc.lifeMax = NPC.downedPlantBoss ? 300 : 200; //Doubled in Expert Mode
			npc.value = NPC.downedPlantBoss ? 400 : 200;
			npc.knockBackResist = 0.1f;
			npc.aiStyle = -1;
			//aiType = NPCID.SlimeSpiked;
			animationType = NPCID.SlimeSpiked;
			//banner = Item.NPCtoBanner(NPCID.GreenSlime);
			//bannerItem = Item.BannerToItem(banner);
			npc.npcSlots = 0.5f;
		}

        public override void AI()
        {
			bool flag = true;
			if (npc.localAI[0] > 0f)
			{
				npc.localAI[0] -= 1f;
			}
			if (!npc.wet && !Main.player[npc.target].npcTypeNoAggro[npc.type])
			{
				Vector2 vector3 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num11 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector3.X;
				float num12 = Main.player[npc.target].position.Y - vector3.Y;
				float num13 = (float)Math.Sqrt(num11 * num11 + num12 * num12);
				if (Main.expertMode && num13 < 120f && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && npc.velocity.Y == 0f)
				{
					npc.ai[0] = -40f;
					if (npc.velocity.Y == 0f)
					{
						npc.velocity.X *= 0.9f;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient && npc.localAI[0] == 0f)
					{
						for (int j = 0; j < 5; j++)
						{
							Vector2 vector4 = new Vector2(j - 2, -4f);
							vector4.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.005f;
							vector4.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.005f;
							vector4.Normalize();
							vector4 *= 4f + (float)Main.rand.Next(-50, 51) * 0.01f;
							Projectile.NewProjectile(vector3.X, vector3.Y, vector4.X, vector4.Y, ModContent.ProjectileType<Projectiles.SpikedGreenSlimeSpike>(), 9, 0f, Main.myPlayer);
							npc.localAI[0] = 30f;
						}
					}
				}
				else if (num13 < 200f && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && npc.velocity.Y == 0f)
				{
					npc.ai[0] = -40f;
					if (npc.velocity.Y == 0f)
					{
						npc.velocity.X *= 0.9f;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient && npc.localAI[0] == 0f)
					{
						num12 = Main.player[npc.target].position.Y - vector3.Y - (float)Main.rand.Next(0, 200);
						num13 = (float)Math.Sqrt(num11 * num11 + num12 * num12);
						num13 = 4.5f / num13;
						num11 *= num13;
						num12 *= num13;
						npc.localAI[0] = 50f;
						Projectile.NewProjectile(vector3.X, vector3.Y, num11, num12, ModContent.ProjectileType<Projectiles.SpikedGreenSlimeSpike>(), 9, 0f, Main.myPlayer);
					}
				}
			}
			if (npc.wet)
			{
				if (npc.collideY)
				{
					npc.velocity.Y = -2f;
				}
				if (npc.velocity.Y < 0f && npc.ai[3] == npc.position.X)
				{
					npc.direction *= -1;
					npc.ai[2] = 200f;
				}
				if (npc.velocity.Y > 0f)
				{
					npc.ai[3] = npc.position.X;
				}
				else
				{
					if (npc.velocity.Y > 2f)
					{
						npc.velocity.Y *= 0.9f;
					}
					npc.velocity.Y -= 0.5f;
					if (npc.velocity.Y < -4f)
					{
						npc.velocity.Y = -4f;
					}
				}
				if (npc.ai[2] == 1f && flag)
				{
					npc.TargetClosest();
				}
			}
			npc.aiAction = 0;
			if (npc.ai[2] == 0f)
			{
				npc.ai[0] = -100f;
				npc.ai[2] = 1f;
				npc.TargetClosest();
			}
			if (npc.velocity.Y == 0f)
			{
				if (npc.collideY && npc.oldVelocity.Y != 0f && Collision.SolidCollision(npc.position, npc.width, npc.height))
				{
					npc.position.X -= npc.velocity.X + (float)npc.direction;
				}
				if (npc.ai[3] == npc.position.X)
				{
					npc.direction *= -1;
					npc.ai[2] = 200f;
				}
				npc.ai[3] = 0f;
				npc.velocity.X *= 0.8f;
				if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
				{
					npc.velocity.X = 0f;
				}
				if (flag)
				{
					npc.ai[0] += 1f;
				}
				npc.ai[0] += 1f;
				int num19 = 0;
				if (npc.ai[0] >= 0f)
				{
					num19 = 1;
				}
				if (npc.ai[0] >= -1000f && npc.ai[0] <= -500f)
				{
					num19 = 2;
				}
				if (npc.ai[0] >= -2000f && npc.ai[0] <= -1500f)
				{
					num19 = 3;
				}
				if (num19 > 0)
				{
					npc.netUpdate = true;
					if (flag && npc.ai[2] == 1f)
					{
						npc.TargetClosest();
					}
					if (num19 == 3)
					{
						npc.velocity.Y = -8f;
						npc.velocity.X += 3 * npc.direction;
						npc.ai[0] = -200f;
						npc.ai[3] = npc.position.X;
					}
					else
					{
						npc.velocity.Y = -6f;
						npc.velocity.X += 2 * npc.direction;
						npc.ai[0] = -120f;
						if (num19 == 1)
						{
							npc.ai[0] -= 1000f;
						}
						else
						{
							npc.ai[0] -= 2000f;
						}
					}
				}
				else if (npc.ai[0] >= -30f)
				{
					npc.aiAction = 1;
				}
			}
			else if (npc.target < 255 && ((npc.direction == 1 && npc.velocity.X < 3f) || (npc.direction == -1 && npc.velocity.X > -3f)))
			{
				if (npc.collideX && Math.Abs(npc.velocity.X) == 0.2f)
				{
					npc.position.X -= 1.4f * (float)npc.direction;
				}
				if (npc.collideY && npc.oldVelocity.Y != 0f && Collision.SolidCollision(npc.position, npc.width, npc.height))
				{
					npc.position.X -= npc.velocity.X + (float)npc.direction;
				}
				if ((npc.direction == -1 && (double)npc.velocity.X < 0.01) || (npc.direction == 1 && (double)npc.velocity.X > -0.01))
				{
					npc.velocity.X += 0.2f * (float)npc.direction;
				}
				else
				{
					npc.velocity.X *= 0.93f;
				}
			}
		}
		public override void NPCLoot()
        {
			if (Main.rand.Next(2) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(1, 4));
			}
		}


		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.slimeRain && Main.hardMode && spawnInfo.player.ZoneOverworldHeight)
            {
				if (spawnInfo.playerInTown)
				{
					return 1.25f;
				}
				return 0.75f;
			}
			return 0;
		}
	}
}