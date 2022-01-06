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

namespace RijamsMod
{ 
    public class RijamsModWorld : ModWorld
    {
		public static bool savedHarpy = false;
        public static bool intTravArived = false;
        public static bool intTravQuestOddDevice = false;
        public static bool intTravQuestBlankDisplay = false;
        public static bool intTravQuestTPCore = false;
        public static bool intTravQuestRyeJam = false;
        public static bool intTravQuestMagicOxygenizer = false;
        public static bool intTravQuestPrimeThruster = false;

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
    }
}