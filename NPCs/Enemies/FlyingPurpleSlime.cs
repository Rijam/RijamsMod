using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.NPCs.Enemies
{
    public class FlyingPurpleSlime : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flying Purple Slime");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Slimer];
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.Slimer);
			NPC.damage = 50;
			NPC.defense = 16;
			NPC.lifeMax = NPC.downedPlantBoss ? 200 : 100; //Doubled in Expert Mode
			NPC.value = NPC.downedPlantBoss ? 200 : 100;
			NPC.knockBackResist = 0.5f;
			NPC.buffImmune[BuffID.Poisoned] = false;
			NPC.aiStyle = 14;
			//AIType = NPCID.Slimer;
			AnimationType = NPCID.Slimer;
			//banner = Item.NPCtoBanner(NPCID.PurpleSlime);
			//bannerItem = Item.BannerToItem(banner);
			NPC.npcSlots = 0.5f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.SlimeRain,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
			});
		}

		public override void AI()
        {
			if (NPC.velocity.X > 0f)
			{
				NPC.spriteDirection = 1;
			}
			if (NPC.velocity.X < 0f)
			{
				NPC.spriteDirection = -1;
			}
			NPC.rotation = NPC.velocity.X * 0.1f;
		}

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			if (NPC.life > 0)
			{
				for (int num331 = 0; num331 < damage / NPC.lifeMax * 100.0; num331++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, 0f, 0f, NPC.alpha, Color.Magenta);
				}
			}
		}
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if (NPC.life > 0)
			{
				for (int num331 = 0; num331 < damage / NPC.lifeMax * 100.0; num331++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, 0f, 0f, NPC.alpha, Color.Purple);
				}
			}
		}
		public override void OnKill()
		{
			for (int num332 = 0; num332 < 50; num332++)
			{
				int num333 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, NPC.velocity.X, 0f, NPC.alpha, Color.Purple);
				Dust dust109 = Main.dust[num333];
				Dust dust2 = dust109;
				dust2.velocity *= 2f;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int num334 = NPC.NewNPC(Entity.GetSource_Death(), (int)(NPC.position.X + (NPC.width / 2)), (int)(NPC.position.Y + NPC.height), NPCID.BlueSlime);
				Main.npc[num334].SetDefaults(NPCID.PurpleSlime);
				Main.npc[num334].velocity.X = NPC.velocity.X;
				Main.npc[num334].velocity.Y = NPC.velocity.Y;
				Gore.NewGore(Entity.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore").Type, NPC.scale);
				if (Main.netMode == NetmodeID.Server && num334 < 200)
				{
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num334);
				}
			}
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.slimeRain && Main.hardMode && spawnInfo.Player.ZoneOverworldHeight)
            {
				if (spawnInfo.PlayerInTown)
                {
					return 1f;
                }
				return 0.5f;
			}
			return 0;
		}
	}
}