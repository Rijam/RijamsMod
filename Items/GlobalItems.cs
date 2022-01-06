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
        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
        {
            if (weapon.type == ItemID.SnowmanCannon)
            {
                if (ammo.type == ModContent.ItemType<Weapons.Ammo.EndlessRocketBox>())
                {
                    type = ProjectileID.RocketSnowmanI;
                }
            }
        }
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag")
            {
                if (arg == ItemID.EyeOfCthulhuBossBag)
                {
                    player.QuickSpawnItem(ModContent.ItemType<Items.Weapons.Ammo.BloodyArrow>(), Main.rand.Next(20, 50));
                }
            }
        }
    }
}