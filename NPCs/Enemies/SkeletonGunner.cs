using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.NPCs.Enemies
{
    public class SkeletonGunner : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeleton Gunner");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.PirateDeadeye]; //20
        }

        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 48;
            npc.damage = 40;
            npc.defense = 9;
            npc.lifeMax = 120;
            npc.value = 500f;
            npc.aiStyle = 3;
            npc.knockBackResist = 0.4f;
            aiType = NPCID.PirateDeadeye;
            animationType = NPCID.PirateDeadeye;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            banner = npc.type;
            bannerItem = mod.ItemType("SkeletonGunnerBanner");
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
            if (Main.rand.Next(25) == 0) //4% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Hook);
            }
            /*if (Main.rand.Next(150) == 0) //0.67% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CartonOfMilk);
            }*/
            if (Main.rand.Next(100) == 0) //1% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.AncientIronHelmet);
            }
            if (Main.rand.Next(200) == 0) //0.5% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.AncientGoldHelmet);
            }
            if (Main.rand.Next(201) == 0) //0.49% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BoneSword);
            }
            if (Main.rand.Next(500) == 0) //0.2% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Skull);
            }
            //additionally, drop the Handgun
            if (Main.rand.Next(50) == 0) //2% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Handgun);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) //would be Deeper Dungeons, but for now have it spawn in the normal Dungeon.
        {
            return (spawnInfo.player.ZoneDungeon) ? 0.01f : 0f;
        }
    }
}