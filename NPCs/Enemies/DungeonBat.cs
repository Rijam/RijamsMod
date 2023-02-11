using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.NPCs.Enemies
{
	public class DungeonBat : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dungeon Bat");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.CaveBat]; //5
		}

		public override void SetDefaults()
		{
			NPC.width = 22;
			NPC.height = 18;
			NPC.damage = 20;
			NPC.defense = 0;
			NPC.lifeMax = 32;
			NPC.value = 100f;
			NPC.aiStyle = 14;
			NPC.knockBackResist = 0.3f;
			NPC.npcSlots = 0.5f;
			AIType = NPCID.CaveBat;
			AnimationType = NPCID.CaveBat;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath4;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Placeable.EnemyBanners.DungeonBatBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity + new Vector2(NPC.spriteDirection * -8, 0), ModContent.Find<ModGore>(Mod.Name + "/" + Name).Type, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			//Copy the drops from the normal cave bat (Chain Knife and Depth Meter)
			var batDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.CaveBat, false); // false is important here
			foreach (var batDropRule in batDropRules)
			{
				// In this foreach loop, we simple add each drop to the PartyZombie drop pool. 
				npcLoot.Add(batDropRule);
			}
			npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 5)); //20% chance
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) //would be Deeper Dungeons, but for now have it spawn in the normal Dungeon.
		{
			return (spawnInfo.Player.ZoneDungeon) ? 0.03f : 0f;
		}
	}
}