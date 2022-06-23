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
    public class SkeletonCrossbower : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeleton Crossbower");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.GoblinArcher]; //21

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
            NPC.width = 24;
            NPC.height = 48;
            NPC.damage = 40;
            NPC.defense = 9;
            NPC.lifeMax = 100;
            NPC.value = 150f;
            NPC.aiStyle = 3;
            NPC.knockBackResist = 0.4f;
            AIType = NPCID.GoblinArcher;
            AnimationType = NPCID.GoblinArcher;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.SkeletonCrossbowerBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
            });
        }

        public override void OnKill()
        {
            Gore.NewGore(Entity.GetSource_Death(), NPC.Center + new Vector2(NPC.spriteDirection * 16, 0), NPC.velocity, 42, 1f); //Skeleton head gore
            Gore.NewGore(Entity.GetSource_Death(), NPC.Center + new Vector2(NPC.spriteDirection * -16, 0), NPC.velocity, 43, 1f); //Skeleton arm gore
            Gore.NewGore(Entity.GetSource_Death(), NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, 43, 1f); //Skeleton arm gore
            Gore.NewGore(Entity.GetSource_Death(), NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, 44, 1f); //Skeleton leg gore
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Copy the same drops as the angry bones
            var angryBonesDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.AngryBones, false); // false is important here
            foreach (var angryBonesDropRule in angryBonesDropRules)
            {
                // In this foreach loop, we simple add each drop to the PartyZombie drop pool. 
                npcLoot.Add(angryBonesDropRule);
            }
            //additionally, drop the crossbow
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.StockadeCrossbow>(), 15)); //6.67% chance
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) //would be Deeper Dungeons, but for now have it spawn in the normal Dungeon.
        {
            return (spawnInfo.Player.ZoneDungeon) ? 0.02f : 0f;
        }
    }
}