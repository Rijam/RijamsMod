using RijamsMod.Items;
using RijamsMod.NPCs;
//using RijamsMod.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using System.Linq;

namespace RijamsMod
{ 
    public class RijamsModWorld : ModWorld
    {
		public static bool savedHarpy = false;
        public static bool intTravArived = false; //oops typo
        public static bool intTravQuestOddDevice = false;
        public static bool intTravQuestBlankDisplay = false;
        public static bool intTravQuestTPCore = false;
        public static bool intTravQuestRyeJam = false;
        public static bool intTravQuestMagicOxygenizer = false;
        public static bool intTravQuestPrimeThruster = false;
        public static bool hellTraderArrivable = false;

        public override void Initialize()
        {
			savedHarpy = false;
            intTravArived = false;
            intTravQuestOddDevice = false;
            intTravQuestBlankDisplay = false;
            intTravQuestTPCore = false;
            intTravQuestRyeJam = false;
            intTravQuestMagicOxygenizer = false;
            intTravQuestPrimeThruster = false;
            hellTraderArrivable = false;
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            //var rescueNPCs = new List<string>();
            //var intTravQuest = new List<string>();
            if (savedHarpy)
            {
                downed.Add("savedHarpy");
            }
            if (intTravArived)
            {
                downed.Add("intTravArived");
            }
            if (intTravQuestOddDevice)
            {
                downed.Add("intTravQuestOddDevice");
            }
            if (intTravQuestBlankDisplay)
            {
                downed.Add("intTravQuestBlankDisplay");
            }
            if (intTravQuestTPCore)
            {
                downed.Add("intTravQuestTPCore");
            }
            if (intTravQuestRyeJam)
            {
                downed.Add("intTravQuestRyeJam");
            }
            if (intTravQuestMagicOxygenizer)
            {
                downed.Add("intTravQuestBreathingPack");
            }
            if (intTravQuestPrimeThruster)
            {
                downed.Add("intTravQuestPrimeThruster");
            }
            if (hellTraderArrivable)
            {
                downed.Add("hellTraderArrivable");
            }


            return new TagCompound
            {

                ["downed"] = downed
                //["rescueNPCs"] = rescueNPCs
                //["intTravQuest"] = intTravQuest
            };

            /*TagCompound tag = new TagCompound();
            tag["savedHarpy"] = savedHarpy;
            tag["intTravArived"] = intTravArived;
            tag["intTravQuestOddDevice"] = intTravQuestOddDevice;
            tag["intTravQuestBlankDisplay"] = intTravQuestBlankDisplay;

            return tag;*/
        }
        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            //var rescueNPCs = tag.GetList<string>("rescueNPCs");
            //var intTravQuest = tag.GetList<string>("intTravQuest");
            savedHarpy = downed.Contains("savedHarpy");
            intTravArived = downed.Contains("intTravArived");
            intTravQuestOddDevice = downed.Contains("intTravQuestOddDevice");
            intTravQuestBlankDisplay = downed.Contains("intTravQuestBlankDisplay");
            intTravQuestTPCore = downed.Contains("intTravQuestTPCore");
            intTravQuestRyeJam = downed.Contains("intTravQuestRyeJam");
            intTravQuestMagicOxygenizer = downed.Contains("intTravQuestBreathingPack");
            intTravQuestPrimeThruster = downed.Contains("intTravQuestPrimeThruster");
            hellTraderArrivable = downed.Contains("hellTraderArrivable");

            /*savedHarpy = tag.GetBool("savedHarpy");
            intTravArived = tag.GetBool("intTravArived");
            intTravQuestOddDevice = tag.GetBool("intTravQuestOddDevice");
            intTravQuestBlankDisplay = tag.GetBool("intTravQuestBlankDisplay");*/
        }
        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            if (loadVersion == 0)
            {
                BitsByte flags = reader.ReadByte();
                savedHarpy = flags[0];
                intTravArived = flags[1];
                intTravQuestOddDevice = flags[2];
                intTravQuestBlankDisplay = flags[3];
                intTravQuestTPCore = flags[4];
                intTravQuestRyeJam = flags[5];
                intTravQuestMagicOxygenizer = flags[6];
                intTravQuestPrimeThruster = flags[7];

                BitsByte flags2 = reader.ReadByte();
                hellTraderArrivable = flags2[0];
            }
            else
            {
                mod.Logger.WarnFormat("RijamsMod: Unknown loadVersion: {0}", loadVersion);
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            //Remember that Bytes/BitsByte only have 8 entries. If you have more than 8 flags you want to sync, use multiple BitsByte
            var flags = new BitsByte();
            flags[0] = savedHarpy;
            flags[1] = intTravArived;
            flags[2] = intTravQuestOddDevice;
            flags[3] = intTravQuestBlankDisplay;
            flags[4] = intTravQuestTPCore;
            flags[5] = intTravQuestRyeJam;
            flags[6] = intTravQuestMagicOxygenizer;
            flags[7] = intTravQuestPrimeThruster;
            writer.Write(flags);

            var flags2 = new BitsByte();
            flags2[0] = hellTraderArrivable;
            writer.Write(flags2);

            /*writer.Write(savedHarpy);
            writer.Write(intTravArived);*/
            //writer.Write(intTravQuestOddDevice);
            //writer.Write(intTravQuestBlankDisplay);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            savedHarpy = flags[0];
            intTravArived = flags[1];
            intTravQuestOddDevice = flags[2];
            intTravQuestBlankDisplay = flags[3];
            intTravQuestTPCore = flags[4];
            intTravQuestRyeJam = flags[5];
            intTravQuestMagicOxygenizer = flags[6];
            intTravQuestPrimeThruster = flags[7];

            BitsByte flags2 = reader.ReadByte();
            hellTraderArrivable = flags2[0];

            /*savedHarpy = reader.ReadBoolean();
            intTravArived = reader.ReadBoolean();*/
            //intTravQuestOddDevice = reader.ReadBoolean();
            //intTravQuestBlankDisplay = reader.ReadBoolean();
        }

        public static void UpdateWorldBool() //from Calamity's Vanities
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public static void SetIntTravArived()
        {
            intTravArived = true;
            UpdateWorldBool();
        }
        public override void PostWorldGen()
        {
            List<Chest> Chests = Main.chest.Where(checkfor => checkfor != null).ToList();
            for (int i = 0; i < 5; i++)
            {
                for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                {
                    Chest chest = Main.chest[chestIndex];
                    if (i == 0 && chest != null)
                    {
                        if (WorldGen.genRand.Next(0, 100) < (Main.tile[chest.x, chest.y].frameX / 36 == 17 ? 50 : (Main.tile[chest.x, chest.y].frameX / 36 == 1 ? 15 : Main.tile[chest.x, chest.y].frameX / 36 == 0 ? 10 : 5)))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].IsAir)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Belt>());
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        //NPCs don't sync properly in multiplayer. Probably need to use a ModPacket.
        //https://github.com/JavidPack/CheatSheet/blob/1.3/Menus/NPCSlot.cs#L219
        //https://github.com/JavidPack/CheatSheet/blob/1.3/CheatSheet.cs#L531

        /*
        public override void PostUpdate()
        {
            
            if (Main.slimeRain && Main.hardMode)
            {
                //NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Main.slimeRain && Main.hardMode passed"), Color.DarkGray);
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    NPC.SlimeRainSpawns(i);
                    if (player.ZoneOverworldHeight && player.active)
                    {
                        //NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("player.ZoneOverworldHeight && player.active passed"), Color.Gray);

                        //If the player has the battle potion buff, double the chance
                        if (Main.rand.Next(player.enemySpawns ? 1750 : 3500) == 0 && Main.expertMode)
                        {
                            int spawnNPC = NPC.NewNPC((int)player.position.X + Main.rand.Next(-600, 600), (int)player.position.Y - 2000 - Main.rand.Next(100, 200), NPCID.SlimeSpiked);
                            Main.npc[spawnNPC].SetDefaults(NPCID.SlimeSpiked);
                            if (Main.netMode != NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, spawnNPC);
                            }
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Spiked Slime spawned?"), Color.White);
                        }
                        if (Main.rand.Next(player.enemySpawns ? 1000 : 2000) == 0)
                        {
                            int spawnNPC = NPC.NewNPC((int)player.position.X + Main.rand.Next(-600, 600), (int)player.position.Y - 2000 - Main.rand.Next(100, 200), NPCID.BlueSlime);
                            Main.npc[spawnNPC].SetDefaults(NPCID.RedSlime);
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, spawnNPC);
                            }
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Red Slime spawned?"), Color.White);
                        }
                        if (Main.rand.Next(player.enemySpawns ? 1050 : 2100) == 0)
                        {
                            int spawnNPC = NPC.NewNPC((int)player.position.X + Main.rand.Next(-600, 600), (int)player.position.Y - 2000 - Main.rand.Next(100, 200), NPCID.BlueSlime);
                            Main.npc[spawnNPC].SetDefaults(NPCID.YellowSlime);
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, spawnNPC);
                            }
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Yellow Slime spawned?"), Color.White);
                        }
                        if (Main.rand.Next(player.enemySpawns ? 1250 : 2500) == 0)
                        {
                            int spawnNPC = NPC.NewNPC((int)player.position.X + Main.rand.Next(-600, 600), (int)player.position.Y - 2000 - Main.rand.Next(100, 200), NPCID.BlueSlime);
                            Main.npc[spawnNPC].SetDefaults(NPCID.BlackSlime);
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, spawnNPC);
                            }
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Black Slime spawned?"), Color.White);
                        }
                        if (Main.rand.Next(player.enemySpawns ? 1500 : 3000) == 0)
                        {
                            int spawnNPC = NPC.NewNPC((int)player.position.X + Main.rand.Next(-600, 600), (int)player.position.Y - 2000 - Main.rand.Next(100, 200), NPCID.ToxicSludge);
                            Main.npc[spawnNPC].SetDefaults(NPCID.ToxicSludge);
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, spawnNPC);
                            }
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Toxic Sludge spawned?"), Color.White);
                        }
                    }
                }
            }
        }*/
    }
}