using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.NPCs.Enemies
{
    public class FlyingPurpleSlime : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flying Purple Slime");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Slimer];
		}

		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.Slimer);
			npc.damage = 50;
			npc.defense = 16;
			npc.lifeMax = NPC.downedPlantBoss ? 200 : 100; //Doubled in Expert Mode
			npc.value = NPC.downedPlantBoss ? 200 : 100;
			npc.knockBackResist = 0.5f;
			npc.buffImmune[BuffID.Poisoned] = false;
			npc.aiStyle = 14;
			//aiType = NPCID.Slimer;
			animationType = NPCID.Slimer;
			//banner = Item.NPCtoBanner(NPCID.PurpleSlime);
			//bannerItem = Item.BannerToItem(banner);
			npc.npcSlots = 0.5f;
		}

        public override void AI()
        {
			if (npc.velocity.X > 0f)
			{
				npc.spriteDirection = 1;
			}
			if (npc.velocity.X < 0f)
			{
				npc.spriteDirection = -1;
			}
			npc.rotation = npc.velocity.X * 0.1f;
		}

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			if (npc.life > 0)
			{
				for (int num331 = 0; num331 < damage / npc.lifeMax * 100.0; num331++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.t_Slime, 0f, 0f, npc.alpha, Color.Magenta);
				}
			}
		}
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if (npc.life > 0)
			{
				for (int num331 = 0; num331 < damage / npc.lifeMax * 100.0; num331++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.t_Slime, 0f, 0f, npc.alpha, Color.Purple);
				}
			}
		}
        public override void NPCLoot()
        {
			for (int num332 = 0; num332 < 50; num332++)
			{
				int num333 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.t_Slime, npc.velocity.X, 0f, npc.alpha, Color.Purple);
				Dust dust109 = Main.dust[num333];
				Dust dust2 = dust109;
				dust2.velocity *= 2f;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int num334 = NPC.NewNPC((int)(npc.position.X + (npc.width / 2)), (int)(npc.position.Y + npc.height), NPCID.BlueSlime);
				Main.npc[num334].SetDefaults(NPCID.PurpleSlime);
				Main.npc[num334].velocity.X = npc.velocity.X;
				Main.npc[num334].velocity.Y = npc.velocity.Y;
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FlyingPurpleSlime_Gore"), npc.scale);
				if (Main.netMode == NetmodeID.Server && num334 < 200)
				{
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num334);
				}
			}
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.slimeRain && Main.hardMode && spawnInfo.player.ZoneOverworldHeight)
            {
				if (spawnInfo.playerInTown)
                {
					return 1f;
                }
				return 0.5f;
			}
			return 0;
		}
	}
}