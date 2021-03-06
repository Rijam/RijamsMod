using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.GameContent.Dyes;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.UI;
using RijamsMod.UI;

namespace RijamsMod
{
    public class RijamsMod : Mod
    {
        public static RijamsMod Instance;
        public static RijamsModConfigClient ConfigClient;
        public static RijamsModConfigServer ConfigServer;
        public static RijamsModNPCs RijamsModNPCs;
        public static ItemUseGlow ItemUseGlow;
        public static ArmorUseGlowHead ArmorUseGlowHead;
        public static ArmorUseGlowBody ArmorUseGlowBody;
        public static ArmorUseGlowLegs ArmorUseGlowLegs;

        /*internal UserInterface MyInterface;
        internal TheUI MyUI;*/

        public override void Load()
        {
            
            if (!Main.dedServ)
            {
                /*MyUI = new TheUI();
                MyUI.Activate(); // Activate calls Initialize() on the UIState if not initialized, then calls OnActivate and then calls Activate on every child element
                MyInterface = new UserInterface();
                MyInterface.SetState(MyUI);*/
            }
            /*ModTranslation text;
            text = LocalizationLoader.CreateTranslation("Config.Ornithophobia");
            text.SetDefault($"[i:{ModContent.ItemType<Items.Armor.Vanity.IntTrav.IntTrav_Helmet>()}]   Ornithophobia");
            LocalizationLoader.AddTranslation(text);
            text = LocalizationLoader.CreateTranslation("Config.BurglarsRingSound");
            text.SetDefault($"[i:{ModContent.ItemType<Items.Accessories.Misc.BurglarsRing>()}]   Burglar's Ring Notification Sound");
            LocalizationLoader.AddTranslation(text);*/
        }

        public override void Unload()
        {
            ItemOriginDesc.itemList.Clear();
            Items.GlobalItems.isWhip.Clear();
            Instance = null;
            ConfigClient = null;
            ConfigServer = null;
            RijamsModNPCs = null;
            ItemUseGlow = null;
            ArmorUseGlowHead = null;
            ArmorUseGlowBody = null;
            ArmorUseGlowLegs = null;
            //MyUI = null;
        }

        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("Census", out Mod censusMod))
            {
                // Here I am using Chat Tags to make my condition even more interesting.
                // If you localize your mod, pass in a localized string instead of just English.
                //censusMod.Call("TownNPCCondition", NPCType("Example Person"), $"Have [i:{ItemType<Items.ExampleItem>()}] or [i:{ItemType<Items.Placeable.ExampleBlock>()}] in inventory and build a house out of [i:{ItemType<Items.Placeable.ExampleBlock>()}] and [i:{ItemType<Items.Placeable.ExampleWall>()}]");
                censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.TownNPCs.InterstellarTraveler>(), $"Defeat EoW or BoW and have [i:{Find<ModItem>("OddDevice").Type}] Odd Device in your inventory");
                // Additional lines for additional town npc that your mod adds
                // Simpler example:
                // censusMod.Call("TownNPCCondition", NPCType("Simple"), "Defeat Duke Fishron");
                //censusMod.Call("TownNPCCondition", Find<ModNPC>("Fisherman").Type, "Rescue the Angler and have at least 5 Town NPCs");
                censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.TownNPCs.Harpy>(), "Rescue her in space");
                censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.TownNPCs.HellTrader>(), "Found in Hell. Can move in in Hardmode");
            }
            if (ModLoader.TryGetMod("PboneUtils", out Mod pboneUtils))
            {
                //Something must be wrong, I can't get it to work.
                byte rarity = 1;
                Func<bool> condition = () => NPC.downedBoss1;
                pboneUtils.Call("MysteriousTraderItem", ModLoader.GetMod("RijamsMod"), ModContent.ItemType<Items.Consumables.StrangeRoll>(), rarity, condition);
                //rarity = 0;
                //condition = () => true;
                //pboneUtils.Call("MysteriousTraderItem", ModLoader.GetMod("RijamsMod"), ModContent.ItemType<Items.Consumables.StrangeRoll>(), rarity, condition);
            }
        }

        /*public override void UpdateUI(GameTime gameTime)
        {
            //if (MyInterface?.CurrentState != null)
            if (TheUI.Visible)
            {
                MyInterface?.Update(gameTime);
            }
        }*/

        /*public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "RijamsMod: MyUI",
                    delegate
                    {
                        if (TheUI.Visible)
                        //if (_lastUpdateUiGameTime != null && MyInterface?.CurrentState != null)
                        {
                            MyInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }*/

        //Adapted from absoluteAquarian's GraphicsLib
        public override object Call(params object[] args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            if (args[0] is not string function)
                throw new ArgumentException("Expected a function name for the first argument");

            void CheckArgsLength(int expected, params string[] argNames)
            {
                if (args.Length != expected)
                    throw new ArgumentOutOfRangeException($"Expected {expected} arguments for Mod.Call(\"{function}\", {string.Join(",", argNames)}), got {args.Length} arguments instead");
            }

            switch (function)
            {
                case "VanillaVanityToArmor":
                    return ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;
                case "TownNPCsCrossModSupport":
                    return ModContent.GetInstance<RijamsModConfigServer>().TownNPCsCrossModSupport;
                case "LoadDebugItems":
                    return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
                case "CatchNPCs":
                    return ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs;
                case "savedHarpy":
                    return RijamsModWorld.savedHarpy;
                case "intTravArrived":
                    return RijamsModWorld.intTravArrived;
                case "hellTraderArrivable":
                    return RijamsModWorld.hellTraderArrivable;
                case "intTravQuest":
                    CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
                    return args[1].ToString() switch
                    {
                        "OddDevice" => RijamsModWorld.intTravQuestOddDevice,
                        "BlankDisplay" => RijamsModWorld.intTravQuestBlankDisplay,
                        "TPCore" => RijamsModWorld.intTravQuestTPCore,
                        "BreadAndJelly" => RijamsModWorld.intTravQuestBreadAndJelly,
                        "MagicOxygenizer" => RijamsModWorld.intTravQuestMagicOxygenizer,
                        "PrimeThruster" => RijamsModWorld.intTravQuestPrimeThruster,
                        _ => throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Rijam's Mod"),
                    };
                case "GetNearbyResidentNPCs":
                    CheckArgsLength(3, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString() });
                    return NPCs.NPCHelper.GetNearbyResidentNPCs((NPC)args[1], (int)args[2], out List<int> _, out List<int> _, out List<int> _, out List<int> _);
                case "GetStatusShop1":
                    return NPCs.NPCHelper.StatusShop1();
                case "GetStatusShop2":
                    return NPCs.NPCHelper.StatusShop2();
                default:
                    throw new ArgumentException($"Function \"{function}\" is not defined by Rijam's Mod");
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            RijamsModMessageType msgType = (RijamsModMessageType)reader.ReadByte();
            switch (msgType)
            {
                case RijamsModMessageType.DummyPacket: 
                    NetMessage.SendData(MessageID.WorldData);
                    Logger.Debug("RijamsMod: Dummy Packet (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetQuestOddDevice:
                    //int questOddDevice = reader.ReadInt32();
                    //RijamsModWorld world = ModContent.GetInstance<RijamsModWorld>();
                    RijamsModWorld.intTravQuestOddDevice = true;
                    NetMessage.SendData(MessageID.WorldData);
                    Logger.Debug("RijamsMod: Odd Device quest completed (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetQuestBlankDisplay:
                    //int questBlankDisplay = reader.ReadInt32();
                    //RijamsModWorld world = ModContent.GetInstance<RijamsModWorld>();
                    RijamsModWorld.intTravQuestBlankDisplay = true;
                    NetMessage.SendData(MessageID.WorldData);
                    Logger.Debug("RijamsMod: Blank Display quest completed (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetQuestTPCore:
                    RijamsModWorld.intTravQuestTPCore = true;
                    NetMessage.SendData(MessageID.WorldData);
                    Logger.Debug("RijamsMod: Teleportation Core quest completed (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetQuestRyeJam:
                    RijamsModWorld.intTravQuestBreadAndJelly = true;
                    NetMessage.SendData(MessageID.WorldData);
                    Logger.Debug("RijamsMod: Rye Jam quest completed (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetQuestMagicOxygenizer:
                    RijamsModWorld.intTravQuestMagicOxygenizer = true;
                    NetMessage.SendData(MessageID.WorldData);
                    Logger.Debug("RijamsMod: Magic Oxygenizer quest completed (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetQuestPrimeThruster:
                    RijamsModWorld.intTravQuestPrimeThruster = true;
                    NetMessage.SendData(MessageID.WorldData);
                    Logger.Debug("RijamsMod: Prime Thruster quest completed (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetHellTraderArrivable:
                    RijamsModWorld.hellTraderArrivable = true;
                    NetMessage.SendData(MessageID.WorldData);
                    Logger.Debug("RijamsMod: Hell Trader Arrivable (Multiplayer packet).");
                    break;
                default:
                    Logger.WarnFormat("RijamsMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
    internal enum RijamsModMessageType : byte
    {
        DummyPacket, //I'm not sure why, but the first packet gets triggered when entering the world (in multiplayer)
        SetQuestOddDevice,
        SetQuestBlankDisplay,
        SetQuestTPCore,
        SetQuestRyeJam,
        SetQuestMagicOxygenizer,
        SetQuestPrimeThruster,
        SetHellTraderArrivable
    }
}