using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.NPCs.Enemies
{
    public class ArmedSkeleton : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Armed Skeleton");
            Main.npcFrameCount[NPC.type] = 7;
            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new(0)
            {
                Hide = true // Hides this NPC from the bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
        }

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 48;
            NPC.damage = 50;
            NPC.defense = 7;
            NPC.lifeMax = 80;
            NPC.value = 100f;
            NPC.aiStyle = 3;
            NPC.knockBackResist = 0.5f;
            AIType = NPCID.ArmedZombie;
            AnimationType = NPCID.ArmedZombie;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            //banner = npc.type;
            //bannerItem = mod.ItemType("ArmedSkeletonBanner");
        }

		public override void OnKill()
		{
            Gore.NewGore(Entity.GetSource_Death(), NPC.Center + new Vector2(NPC.spriteDirection * 16, 0), NPC.velocity, 42, 1f); //Skeleton head gore
            Gore.NewGore(Entity.GetSource_Death(), NPC.Center + new Vector2(NPC.spriteDirection * -16, 0), NPC.velocity, 43, 1f); //Skeleton arm gore
            Gore.NewGore(Entity.GetSource_Death(), NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, 43, 1f); //Skeleton arm gore
            Gore.NewGore(Entity.GetSource_Death(), NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, 44, 1f); //Skeleton leg gore
        }
        //For some reason, cloning the Armed Zombie doesn't include the extra melee reach. Copied from vanilla.
        public override void AI()
        {
            //Main.NewText("npc.ai[2] " + npc.ai[2]);
            Rectangle npcRect = NPC.Hitbox;
            if (NPC.ai[2] > 5f)
            {
                int num = 34;
                if (NPC.spriteDirection < 0)
                {
                    npcRect.X -= num;
                    npcRect.Width += num;
                }
                else
                {
                    npcRect.Width += num;
                }
            }
        }


        //Vanilla. NPCID 430 through 436 are the different armed zombies
        /*public static void GetMeleeCollisionData(Rectangle victimHitbox, int enemyIndex, ref int specialHitSetter, ref float damageMultiplier, ref Rectangle npcRect)
        {
            NPC nPC = Main.npc[enemyIndex];
            if (nPC.type >= 430 && nPC.type <= 436 && nPC.ai[2] > 5f)
            {
                int num = 34;
                if (nPC.spriteDirection < 0)
                {
                    npcRect.X -= num;
                    npcRect.Width += num;
                }
                else
                {
                    npcRect.Width += num;
                }
                damageMultiplier *= 1.25f;
            }
        }*/

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Copy the same drops as a normal skeleton
            var skeletonDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.Skeleton, false); // false is important here
            foreach (var skeletonDropRule in skeletonDropRules)
            {
                // In this foreach loop, we simple add each drop to the PartyZombie drop pool. 
                npcLoot.Add(skeletonDropRule);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) //would be Deeper Dungeons, but for now have it spawn in the normal Dungeon.
        {
            //int rand = Main.rand.Next(2);
            //return (spawnInfo.player.ZoneDungeon) ? 0.03f : 0f;
            return 0f;
        }
    }
}