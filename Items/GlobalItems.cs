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
            if (item.type == ItemID.PharaohsMask)
            {
                item.vanity = false;
                item.defense = 2;
                item.value = 5000;
            }
            if (item.type == ItemID.PharaohsRobe)
            {
                item.vanity = false;
                item.defense = 3;
                item.value = 5000;
            }
            if (item.type == ItemID.AncientArmorHat)
            {
                item.vanity = false;
                item.defense = 10;
            }
            if (item.type == ItemID.AncientArmorShirt)
            {
                item.vanity = false;
                item.defense = 14;
            }
            if (item.type == ItemID.AncientArmorPants)
            {
                item.vanity = false;
                item.defense = 9;
            }
            if (item.type == ItemID.PinkPricklyPear)
            {
                item.consumable = true;
                item.useStyle = ItemUseStyleID.EatingUsing;
                item.useAnimation = 15;
                item.useTime = 15;
                item.useTurn = true;
                item.UseSound = SoundID.Item2;
                item.buffType = ModContent.BuffType<Buffs.Satiated>(); //Specify an existing buff to be applied when used.
                item.buffTime = 3600; //1 minute
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            bool isLeftShiftHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
            if (item.buffType == ModContent.BuffType<Buffs.ExceptionalFeast>())
            {
                if (isLeftShiftHeld)
                {
                    tooltips.Add(new TooltipLine(mod, "ExceptionalFeast", "Exceptional Feast provides:"));
                    tooltips.Add(new TooltipLine(mod, "EFStats1", "+5 Defense"));
                    tooltips.Add(new TooltipLine(mod, "EFStats2", "+5% Critical Hit Chance"));
                    tooltips.Add(new TooltipLine(mod, "EFStats3", "+12.5% Melee Speed"));
                    tooltips.Add(new TooltipLine(mod, "EFStats4", "+12.5% Damage"));
                    tooltips.Add(new TooltipLine(mod, "EFStats5", "+1.25 Minion Knockback"));
                    tooltips.Add(new TooltipLine(mod, "EFStats6", "+50% Movement Speed"));
                    tooltips.Add(new TooltipLine(mod, "EFStats7", "+20% Mining Speed"));
                    tooltips.Add(new TooltipLine(mod, "EFOverride", "Exceptional Feast will override Satiated & Well Fed"));
                }
                else
                {
                    tooltips.Add(new TooltipLine(mod, "EFInfo", "Hold Left Shift for more information"));
                }
            }
            if (item.buffType == ModContent.BuffType<Buffs.Satiated>())
            {
                if (item.type == ItemID.PinkPricklyPear)
                {
                    tooltips.Insert(4, new TooltipLine(mod, "SItemInfo", "Minuscule improvements to all stats"));
                }
               
                if (isLeftShiftHeld)
                {
                    tooltips.Add(new TooltipLine(mod, "Satiated", "Satiated provides:"));
                    tooltips.Add(new TooltipLine(mod, "SStats1", "+1 Defense"));
                    tooltips.Add(new TooltipLine(mod, "SStats2", "+1% Critical Hit Chance"));
                    tooltips.Add(new TooltipLine(mod, "SStats3", "+2.5% Melee Speed"));
                    tooltips.Add(new TooltipLine(mod, "SStats4", "+2.5% Damage"));
                    tooltips.Add(new TooltipLine(mod, "SStats5", "+0.25 Minion Knockback"));
                    tooltips.Add(new TooltipLine(mod, "SStats6", "+10% Movement Speed"));
                    tooltips.Add(new TooltipLine(mod, "SStats7", "Does not provide increase life regeneration in Expert Mode"));
                    tooltips.Add(new TooltipLine(mod, "SOverride", "Satiated will be overridden by Well Fed & Exceptional Feast"));
                }
                else
                {
                    tooltips.Add(new TooltipLine(mod, "SInfo", "Hold Left Shift for more information"));
                }
            }
            if (item.type == ItemID.AncientArmorHat)
            {
                tooltips.Insert(3, new TooltipLine(mod, "AncientHeaddress", "+5 critical strike chance"));
                tooltips.Insert(4, new TooltipLine(mod, "AncientHeaddress", "20% chance to not consume ammo"));
            }
            if (item.type == ItemID.AncientArmorShirt)
            {
                tooltips.Insert(3, new TooltipLine(mod, "AncientGarments", "20% increased damage"));
                tooltips.Insert(4, new TooltipLine(mod, "AncientGarments", "+1 Minion capacity"));
            }
            if (item.type == ItemID.AncientArmorPants)
            {
                tooltips.Insert(3, new TooltipLine(mod, "AncientSlacks", "10% increased melee speed"));
                tooltips.Insert(4, new TooltipLine(mod, "AncientSlacks", "10% reduced mana usage"));
                tooltips.Insert(5, new TooltipLine(mod, "AncientSlacks", "+2 life regeneration"));
                
            }
            if (item.type == ItemID.CelestialCuffs)
            {
                tooltips.Insert(5, new TooltipLine(mod, "CelestialCuffs", "Increases maximum mana by 20"));
            }
            if (item.type == ItemID.FireGauntlet)
            {
                tooltips.RemoveAt(4);
                tooltips.Insert(4, new TooltipLine(mod, "FireGauntlet", "12% increased melee damage and speed"));
            }
            base.ModifyTooltips(item, tooltips);
        }
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.CrimsonHelmet && (body.type == ItemID.CrimsonScalemail || body.type == ModContent.ItemType<DilapidatedCrimsonScalemail>()) && (legs.type == ItemID.CrimsonGreaves || legs.type == ModContent.ItemType<DilapidatedCrimsonGreaves>()))
            {
                return Language.GetTextValue("ArmorSetBonus.Crimson");
            }
            if (head.type == ItemID.PharaohsMask && body.type == ItemID.PharaohsRobe)
            {
                return "Pharaoh";
            }
            if (head.type == ItemID.AncientArmorHat && body.type == ItemID.AncientArmorShirt && legs.type == ItemID.AncientArmorPants)
            {
                return "Ancient";
            }
            return "";
        }
        public override void UpdateEquip(Item item, Player player)
        {
            if (item.type == ItemID.AncientArmorHat)
            {
                player.meleeCrit += 5;
                player.rangedCrit += 5;
                player.magicCrit += 5;
                player.thrownCrit += 5;
                player.ammoCost80 = true;
            }
            if (item.type == ItemID.AncientArmorShirt)
            {
                player.maxMinions += 1;
                player.allDamage *= 1.2f;
            }
            if (item.type == ItemID.AncientArmorPants)
            {
                player.meleeSpeed *= 1.1f;
                player.manaCost *= 0.9f;
                player.lifeRegen += 2;
            }
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "Pharaoh")
            {
                player.setBonus = "\n10% reduced mana usage\n+1 Minion capacity\n5% increased movement speed\nGrants Immunity to Mighty Wind";
                player.manaCost *= 0.9f;
                player.maxMinions++;
                player.moveSpeed += 0.05f;
                player.buffImmune[BuffID.WindPushed] = true;
            }
            if (set == "Ancient")
            {
                player.GetModPlayer<RijamsModPlayer>().ancientSet = true;
                player.setBonus = "\nIncreased maximum running speed\nIncreased running acceleration\n+0.5 seconds flight time\nAllows Shield of Cthulhu style dashing";
                player.dash = 2; //Shield of Cthulhu dash
                //Set in PostUpdateRunSpeeds() in RijamsModPlayer.cs
                //player.runAcceleration += 0.1f;
                //player.maxRunSpeed += 2;
                player.wingTimeMax += 30;
            }
        }
        public override void ArmorSetShadows(Player player, string set)
        {
            if (set == "Ancient")
            {
                player.armorEffectDrawShadow = true;
            }
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
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.buffType == BuffID.WellFed && player.HasBuff(ModContent.BuffType<Buffs.ExceptionalFeast>()))
            {
                return false;
            }
            if (item.buffType == ModContent.BuffType<Buffs.Satiated>() && (player.HasBuff(ModContent.BuffType<Buffs.ExceptionalFeast>()) || player.HasBuff(BuffID.WellFed)))
            {
                return false;
            }
            return base.CanUseItem(item, player);
        }
    }
}