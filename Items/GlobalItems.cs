using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using RijamsMod.Items.Armor;

namespace RijamsMod.Items
{
    public class GlobalItems : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Coal)
            {
                item.maxStack = 99;
            }
        }
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.CrimsonHelmet && (body.type == ItemID.CrimsonScalemail || body.type == ModContent.ItemType<DilapidatedCrimsonScalemail>()) && (legs.type == ItemID.CrimsonGreaves || legs.type == ModContent.ItemType<DilapidatedCrimsonGreaves>()))
            {
                return Language.GetTextValue("ArmorSetBonus.Crimson");
            }
            return "";
        }
    }
}