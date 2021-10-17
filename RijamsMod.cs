using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace RijamsMod
{
    public class RijamsMod : Mod
    {
        //public static RijamsMod Instance;
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.Glass, 5);
            recipe.AddIngredient(ItemID.SnowBlock, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.SnowGlobe, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.FleshKnuckles, 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.PutridScent, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.PutridScent, 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.FleshKnuckles, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.Gel, 500);
            recipe.AddIngredient(ItemID.Wood, 5);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(ItemID.SlimeStaff, 1);
            recipe.AddRecipe();
        }
        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " OOA Sentry accessories", new int[]
            {
                ItemID.ApprenticeScarf,
                ItemID.MonkBelt,
                ItemID.HuntressBuckler,
                ItemID.SquireShield
            });
            RecipeGroup.RegisterGroup("RijamsMod:Defender's Gear", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Copper Bar", new int[]
            {
                ItemID.CopperBar,
                ItemID.TinBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:CopperBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver Bar", new int[]
            {
                ItemID.SilverBar,
                ItemID.TungstenBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:SilverBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:GoldBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Bar", new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:EvilBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cobalt Bar", new int[]
            {
                ItemID.CobaltBar,
                ItemID.PalladiumBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:CobaltBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Mythril Bar", new int[]
            {
                ItemID.MythrilBar,
                ItemID.OrichalcumBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:MythrilBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Adamantite Bar", new int[]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:AdamantiteBars", group);
        }
        public override void Load()
        {
            if (!Main.dedServ)
            {
                AddEquipTexture(null, EquipType.Legs, "Harpy_Vanity_Shorts", "RijamsMod/Items/Armor/Vanity/Harpy_Vanity_Shorts_Legs");
                AddEquipTexture(null, EquipType.Legs, "Harpy_Vanity_Shorts_Female", "RijamsMod/Items/Armor/Vanity/Harpy_Vanity_Shorts_FemaleLegs");
                //AddEquipTexture(null, EquipType.Back, "MechaArmor_Chestplate_Back", "RijamsMod/Items/Armor/MechaArmor_Chestplate_Back");
                //AddEquipTexture(null, EquipType.Head, "ShroomiteVisor", "RijamsMod/Items/Armor/ShroomiteVisor_Head");

                AddEquipTexture(null, EquipType.Head, "DilapidatedCrimsonHelmet", "RijamsMod/Items/Armor/DilapidatedCrimsonHelmet_Head");
                AddEquipTexture(null, EquipType.Body, "DilapidatedCrimsonScalemail", "RijamsMod/Items/Armor/DilapidatedCrimsonScalemail_Body", "RijamsMod/Items/Armor/DilapidatedCrimsonScalemail_Arms", "RijamsMod/Items/Armor/DilapidatedCrimsonScalemail_FemaleBody");
                AddEquipTexture(null, EquipType.Legs, "DilapidatedCrimsonGreaves", "RijamsMod/Items/Armor/DilapidatedCrimsonGreaves_Legs");

                AddEquipTexture(new Items.Armor.DrawDilapidatedCrimsonHelmet(), null, EquipType.Head, "DilapidatedCrimsonHelmet", "RijamsMod/Items/Armor/DilapidatedCrimsonHelmet_Head");
                AddEquipTexture(new Items.Armor.DrawDilapidatedCrimsonScalemail(), null, EquipType.Body, "DilapidatedCrimsonScalemail", "RijamsMod/Items/Armor/DilapidatedCrimsonScalemail_Body", "RijamsMod/Items/Armor/DilapidatedCrimsonScalemail_Arms", "RijamsMod/Items/Armor/DilapidatedCrimsonScalemail_FemaleBody");
                AddEquipTexture(new Items.Armor.DrawDilapidatedCrimsonGreaves(), null, EquipType.Legs, "DilapidatedCrimsonGreaves", "RijamsMod/Items/Armor/DilapidatedCrimsonGreaves_Legs");

                //Item dilapidatedCrimsonHelmet = new Item();
                //dilapidatedCrimsonHelmet.SetDefaults(ItemID.CrimsonHelmet);
                //dilapidatedCrimsonHelmet.SetDefaults(ItemType("DilapidatedCrimsonHelmet"));//Index was outside the bounds of the array
                //Main.armorHeadLoaded[dilapidatedCrimsonHelmet.headSlot] = true;
                //Main.armorHeadTexture[dilapidatedCrimsonHelmet.headSlot] = GetTexture("Items/Armor/DilapidatedCrimsonHelmet_Head");

                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/FreedoomPhase2_MAP07_OuterStorageWarehouse"), ItemType("TestMusicBox"), TileType("TestMusicBox"));
            }
            ModTranslation text;
            text = CreateTranslation("Config.Ornithophobia");
            text.SetDefault($"[i:{ModContent.ItemType<Items.Armor.Vanity.IntTrav_Vanity_Helmet>()}]   Ornithophobia");
            AddTranslation(text);
        }
        public override void PostSetupContent()
        {
            Mod censusMod = ModLoader.GetMod("Census");
            if (censusMod != null)
            {
                // Here I am using Chat Tags to make my condition even more interesting.
                // If you localize your mod, pass in a localized string instead of just English.
                //censusMod.Call("TownNPCCondition", NPCType("Example Person"), $"Have [i:{ItemType<Items.ExampleItem>()}] or [i:{ItemType<Items.Placeable.ExampleBlock>()}] in inventory and build a house out of [i:{ItemType<Items.Placeable.ExampleBlock>()}] and [i:{ItemType<Items.Placeable.ExampleWall>()}]");
                censusMod.Call("TownNPCCondition", NPCType("Interstellar Traveler"), $"Defeat EoW or BoW and have [i:{ItemType("OddDevice")}] Odd Device in your inventory");
                // Additional lines for additional town npc that your mod adds
                // Simpler example:
                // censusMod.Call("TownNPCCondition", NPCType("Simple"), "Defeat Duke Fishron");
                censusMod.Call("TownNPCCondition", NPCType("Fisherman"), "Rescue the Angler and have at least 5 Town NPC");
                censusMod.Call("TownNPCCondition", NPCType("Harpy"), "Rescue her in space");
            }
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            RijamsModMessageType msgType = (RijamsModMessageType)reader.ReadByte();
            switch (msgType)
            {
                case RijamsModMessageType.SetQuestOddDevice:
                    //int questOddDevice = reader.ReadInt32();
                    //RijamsModWorld world = ModContent.GetInstance<RijamsModWorld>();
                    RijamsModWorld.intTravQuestOddDevice = true;
                    Logger.Debug("RijamsMod: Odd Device quest completed (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetQuestBlankDisplay:
                    //int questBlankDisplay = reader.ReadInt32();
                    //RijamsModWorld world = ModContent.GetInstance<RijamsModWorld>();
                    RijamsModWorld.intTravQuestBlankDisplay = true;
                    Logger.Debug("RijamsMod: Blank Display quest completed (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetQuestTPCore:
                    RijamsModWorld.intTravQuestTPCore = true;
                    Logger.Debug("RijamsMod: Teleportation Core quest completed (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetQuestRyeJam:
                    RijamsModWorld.intTravQuestRyeJam = true;
                    Logger.Debug("RijamsMod: Rye Jam quest completed (Multiplayer packet).");
                    break;
                case RijamsModMessageType.SetQuestMagicOxygenizer:
                    RijamsModWorld.intTravQuestMagicOxygenizer = true;
                    Logger.Debug("RijamsMod: Magic Oxygenizer quest completed (Multiplayer packet).");
                    break;
                default:
                    Logger.WarnFormat("RijamsMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
    internal enum RijamsModMessageType : byte
    {
        SetQuestOddDevice,
        SetQuestBlankDisplay,
        SetQuestTPCore,
        SetQuestRyeJam,
        SetQuestMagicOxygenizer
    }
}