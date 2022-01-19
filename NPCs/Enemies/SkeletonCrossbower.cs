using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.NPCs.Enemies
{
    public class SkeletonCrossbower : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeleton Crossbower");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.GoblinArcher]; //21
        }

        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 48;
            npc.damage = 40;
            npc.defense = 9;
            npc.lifeMax = 100;
            npc.value = 150f;
            npc.aiStyle = 3;
            npc.knockBackResist = 0.4f;
            aiType = NPCID.GoblinArcher;
            animationType = NPCID.GoblinArcher;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Placeable.SkeletonCrossbowerBanner>();
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 16, 0), npc.velocity, 42, 1f); //Skeleton head gore
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * -16, 0), npc.velocity, 43, 1f); //Skeleton arm gore
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 8, 0), npc.velocity, 43, 1f); //Skeleton arm gore
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 8, 0), npc.velocity, 44, 1f); //Skeleton leg gore

            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(100) <= 98) //98% chance
            {
                for (int i = 0; i <= Main.rand.Next(1, 3); i++)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bone);
                }
            }
            if (Main.rand.Next(65) == 0) //1.53% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldenKey);
            }
            if (Main.rand.Next(250) == 0) //0.4% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BoneWand);
            }
            if (Main.rand.Next(300) == 0) //0.33% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ClothierVoodooDoll);
            }
            if (Main.rand.Next(100) == 0) //1% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TallyCounter);
            }
            //additionally, drop the crossbow
            if (Main.rand.Next(15) == 0) //6.67% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.StockadeCrossbow>());
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) //would be Deeper Dungeons, but for now have it spawn in the normal Dungeon.
        {
            return (spawnInfo.player.ZoneDungeon) ? 0.02f : 0f;
        }
    }
}