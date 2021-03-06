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
using Terraria.WorldBuilding;
using System.Linq;

namespace RijamsMod
{ 
    public class RijamsModWorld : ModSystem
    {
		public static bool savedHarpy = false;
        public static bool intTravArrived = false;
        public static bool intTravQuestOddDevice = false;
        public static bool intTravQuestBlankDisplay = false;
        public static bool intTravQuestTPCore = false;
        public static bool intTravQuestBreadAndJelly = false;
        public static bool intTravQuestMagicOxygenizer = false;
        public static bool intTravQuestPrimeThruster = false;
        public static bool hellTraderArrivable = false;

        public override void OnWorldLoad()
        {
			savedHarpy = false;
            intTravArrived = false;
            intTravQuestOddDevice = false;
            intTravQuestBlankDisplay = false;
            intTravQuestTPCore = false;
            intTravQuestBreadAndJelly = false;
            intTravQuestMagicOxygenizer = false;
            intTravQuestPrimeThruster = false;
            hellTraderArrivable = false;
        }

        public override void OnWorldUnload()
        {
            savedHarpy = false;
            intTravArrived = false;
            intTravQuestOddDevice = false;
            intTravQuestBlankDisplay = false;
            intTravQuestTPCore = false;
            intTravQuestBreadAndJelly = false;
            intTravQuestMagicOxygenizer = false;
            intTravQuestPrimeThruster = false;
            hellTraderArrivable = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (savedHarpy)
            {
                tag["savedHarpy"] = true;
            }
            if (intTravArrived)
            {
                tag["intTravArived"] = true;
            }
            if (intTravQuestOddDevice)
            {
                tag["intTravQuestOddDevice"] = true;
            }
            if (intTravQuestBlankDisplay)
            {
                tag["intTravQuestBlankDisplay"] = true;
            }
            if (intTravQuestTPCore)
            {
                tag["intTravQuestTPCore"] = true;
            }
            if (intTravQuestBreadAndJelly)
            {
                tag["intTravQuestRyeJam"] = true;
            }
            if (intTravQuestMagicOxygenizer)
            {
                tag["intTravQuestBreathingPack"] = true;
            }
            if (intTravQuestPrimeThruster)
            {
                tag["intTravQuestPrimeThruster"] = true;
            }
            if (hellTraderArrivable)
            {
                tag["hellTraderArrivable"] = true;
            }
        }
        public override void LoadWorldData(TagCompound tag)
        {
            savedHarpy = tag.ContainsKey("savedHarpy");
            intTravArrived = tag.ContainsKey("intTravArived");
            intTravQuestOddDevice = tag.ContainsKey("intTravQuestOddDevice");
            intTravQuestBlankDisplay = tag.ContainsKey("intTravQuestBlankDisplay");
            intTravQuestTPCore = tag.ContainsKey("intTravQuestTPCore");
            intTravQuestBreadAndJelly = tag.ContainsKey("intTravQuestRyeJam");
            intTravQuestMagicOxygenizer = tag.ContainsKey("intTravQuestBreathingPack");
            intTravQuestPrimeThruster = tag.ContainsKey("intTravQuestPrimeThruster");
            hellTraderArrivable = tag.ContainsKey("hellTraderArrivable");
        }

        public override void NetSend(BinaryWriter writer)
        {
            //Remember that Bytes/BitsByte only have 8 entries. If you have more than 8 flags you want to sync, use multiple BitsByte
            var flags = new BitsByte();
            flags[0] = savedHarpy;
            flags[1] = intTravArrived;
            flags[2] = intTravQuestOddDevice;
            flags[3] = intTravQuestBlankDisplay;
            flags[4] = intTravQuestTPCore;
            flags[5] = intTravQuestBreadAndJelly;
            flags[6] = intTravQuestMagicOxygenizer;
            flags[7] = intTravQuestPrimeThruster;
            writer.Write(flags);

            var flags2 = new BitsByte();
            flags2[0] = hellTraderArrivable;
            writer.Write(flags2);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            savedHarpy = flags[0];
            intTravArrived = flags[1];
            intTravQuestOddDevice = flags[2];
            intTravQuestBlankDisplay = flags[3];
            intTravQuestTPCore = flags[4];
            intTravQuestBreadAndJelly = flags[5];
            intTravQuestMagicOxygenizer = flags[6];
            intTravQuestPrimeThruster = flags[7];

            BitsByte flags2 = reader.ReadByte();
            hellTraderArrivable = flags2[0];
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
            intTravArrived = true;
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
                        if (WorldGen.genRand.Next(0, 100) < (Main.tile[chest.x, chest.y].TileFrameX / 36 == 17 ? 50 : (Main.tile[chest.x, chest.y].TileFrameX / 36 == 1 ? 15 : Main.tile[chest.x, chest.y].TileFrameX / 36 == 0 ? 10 : 5)))
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