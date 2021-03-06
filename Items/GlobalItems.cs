using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using RijamsMod.Items.Armor;
using System.Linq;
using Terraria.GameContent.Creative;
using Terraria.Utilities;

namespace RijamsMod.Items
{
    public class GlobalItems : GlobalItem
    {
        public static List<int> isWhip = new();

		public override void SetDefaults(Item item)
        {
            bool vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;

            if (item.type == ItemID.Coal)
            {
                item.maxStack = 99;
            }
            if (item.type == ItemID.PharaohsMask && vanillaVanityToArmor)
            {
                item.vanity = false;
                item.defense = 2;
                item.value = 5000;
            }
            if (item.type == ItemID.PharaohsRobe && vanillaVanityToArmor)
            {
                item.vanity = false;
                item.defense = 3;
                item.value = 5000;
            }
            if (item.type == ItemID.AncientArmorHat && vanillaVanityToArmor)
            {
                item.vanity = false;
                item.defense = 10;
            }
            if (item.type == ItemID.AncientArmorShirt && vanillaVanityToArmor)
            {
                item.vanity = false;
                item.defense = 14;
            }
            if (item.type == ItemID.AncientArmorPants && vanillaVanityToArmor)
            {
                item.vanity = false;
                item.defense = 9;
            }
            if (item.type == ItemID.PinkPricklyPear)
            {
                item.consumable = true;
                item.useStyle = ItemUseStyleID.EatFood;
                item.useAnimation = 15;
                item.useTime = 15;
                item.useTurn = true;
                item.UseSound = SoundID.Item2;
                item.buffType = ModContent.BuffType<Buffs.Satiated>(); //Specify an existing buff to be applied when used.
                item.buffTime = 3600; //1 minute
            }
            if (item.ModItem != null && item.ModItem.Mod == Mod) //Hacky solution because I'm lazy lol
			{
                if (item.maxStack == 1)
                {
                    item.ModItem.SacrificeTotal = 1;
                }
                else if (item.consumable)
                {
                    if (item.buffType > 1)
					{
                        item.ModItem.SacrificeTotal = 30;
                    }
                    else if (item.createTile > TileID.Stone)
					{
                        item.ModItem.SacrificeTotal = 100;
                    }
                    else if (item.createWall > 1)
                    {
                        item.ModItem.SacrificeTotal = 400;
                    }
                    else if (item.ammo > 1)
                    {
                        item.ModItem.SacrificeTotal = 99;
                    }
                    else if (item.makeNPC > 1)
					{
                        item.ModItem.SacrificeTotal = 3;
					}
                    else
					{
                        item.ModItem.SacrificeTotal = 10;
                    }
                }
                else if (item.ModItem.GetType().Namespace.ToString() == "RijamsMod.Items.Materials")
				{
                    item.ModItem.SacrificeTotal = 25;
                }
                else if (item.dye > 1)
				{
                    item.ModItem.SacrificeTotal = 3;
                }
                else
				{
                    item.ModItem.SacrificeTotal = 2;
                }
            }
        }
        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            if (player.GetModPlayer<RijamsModPlayer>().flaskBuff == 1 && item.DamageType == DamageClass.Melee && !item.noMelee && !item.noUseGraphic && Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.SulfurDust>(), player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.7f;
                Main.dust[dust].velocity.Y -= 0.5f;
                Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.Yellow.ToVector3() * 0.1f);
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            bool vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;
            bool isLeftShiftHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
            int index = 0;
            if (item.favorited)
            {
                index += 2;
            }
            if (item.buffType == ModContent.BuffType<Buffs.ExceptionalFeast>())
            {
                if (isLeftShiftHeld)
                {
                    tooltips.Add(new TooltipLine(Mod, "ExceptionalFeast", "Exceptional Feast provides:"));
                    tooltips.Add(new TooltipLine(Mod, "EFStats1", "+5 Defense"));
                    tooltips.Add(new TooltipLine(Mod, "EFStats2", "+5% Critical Hit Chance"));
                    tooltips.Add(new TooltipLine(Mod, "EFStats3", "+12.5% Melee Speed"));
                    tooltips.Add(new TooltipLine(Mod, "EFStats4", "+12.5% Damage"));
                    tooltips.Add(new TooltipLine(Mod, "EFStats5", "+1.25 Minion Knockback"));
                    tooltips.Add(new TooltipLine(Mod, "EFStats6", "+50% Movement Speed"));
                    tooltips.Add(new TooltipLine(Mod, "EFStats7", "+20% Mining Speed"));
                    tooltips.Add(new TooltipLine(Mod, "EFOverride", "Counts as a Well Fed buff"));
                }
                else
                {
                    tooltips.Add(new TooltipLine(Mod, "EFInfo", "Hold Left Shift for more information"));
                }
            }
            if (item.buffType == ModContent.BuffType<Buffs.Satiated>())
            {
                if (item.type == ItemID.PinkPricklyPear)
                {
                    tooltips.Insert(index + 4, new TooltipLine(Mod, "SItemInfo", "Minuscule improvements to all stats"));
                }
               
                if (isLeftShiftHeld)
                {
                    tooltips.Add(new TooltipLine(Mod, "Satiated", "Satiated provides:"));
                    tooltips.Add(new TooltipLine(Mod, "SStats1", "+1 Defense"));
                    tooltips.Add(new TooltipLine(Mod, "SStats2", "+1% Critical Hit Chance"));
                    tooltips.Add(new TooltipLine(Mod, "SStats3", "+2.5% Melee Speed"));
                    tooltips.Add(new TooltipLine(Mod, "SStats4", "+2.5% Damage"));
                    tooltips.Add(new TooltipLine(Mod, "SStats5", "+0.25 Minion Knockback"));
                    tooltips.Add(new TooltipLine(Mod, "SStats6", "+10% Movement Speed"));
                    tooltips.Add(new TooltipLine(Mod, "SStats7", "Does not provide increase life regeneration in Expert Mode"));
                    tooltips.Add(new TooltipLine(Mod, "SOverride", "Counts as a Well Fed buff"));
                }
                else
                {
                    tooltips.Add(new TooltipLine(Mod, "SInfo", "Hold Left Shift for more information"));
                }
            }
            if (item.type == ItemID.AncientArmorHat && vanillaVanityToArmor)
            {
                tooltips.Insert(index + 3, new TooltipLine(Mod, "AncientHeaddress", "+5 critical strike chance"));
                tooltips.Insert(index + 4, new TooltipLine(Mod, "AncientHeaddress", "20% chance to not consume ammo"));
            }
            if (item.type == ItemID.AncientArmorShirt && vanillaVanityToArmor)
            {
                tooltips.Insert(index + 3, new TooltipLine(Mod, "AncientGarments", "20% increased damage"));
                tooltips.Insert(index + 4, new TooltipLine(Mod, "AncientGarments", "+1 Minion capacity"));
            }
            if (item.type == ItemID.AncientArmorPants && vanillaVanityToArmor)
            {
                tooltips.Insert(index + 3, new TooltipLine(Mod, "AncientSlacks", "10% increased melee speed"));
                tooltips.Insert(index + 4, new TooltipLine(Mod, "AncientSlacks", "10% reduced mana usage"));
                tooltips.Insert(index + 5, new TooltipLine(Mod, "AncientSlacks", "+2 life regeneration"));
                
            }
            /*if (item.type == ItemID.CelestialCuffs)
            {
                tooltips.Insert(index + 5, new TooltipLine(Mod, "CelestialCuffs", "Increases maximum mana by 20"));
            }*/
            /*if (item.type == ItemID.FireGauntlet)
            {
                TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "Tooltip1" && x.Mod == "Terraria");
                if (line != null)
                {
                    line.Text = "12% increased melee damage and speed";
                }
                //tooltips.RemoveAt(4);
                //tooltips.Insert(4, new TooltipLine(mod, "FireGauntlet", "12% increased melee damage and speed"));
            }*/
        }
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            bool vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;

            if (head.type == ItemID.CrimsonHelmet && (body.type == ItemID.CrimsonScalemail || body.type == ModContent.ItemType<Armor.DilapidatedCrimson.DilapidatedCrimsonScalemail>()) && (legs.type == ItemID.CrimsonGreaves || legs.type == ModContent.ItemType<Armor.DilapidatedCrimson.DilapidatedCrimsonGreaves>()))
            {
                return Language.GetTextValue("ArmorSetBonus.Crimson");
            }
            if (head.type == ItemID.PharaohsMask && body.type == ItemID.PharaohsRobe && vanillaVanityToArmor)
            {
                return "Pharaoh";
            }
            if (head.type == ItemID.AncientArmorHat && body.type == ItemID.AncientArmorShirt && legs.type == ItemID.AncientArmorPants && vanillaVanityToArmor)
            {
                return "Ancient";
            }
            if (head.type == ItemID.StardustHelmet && body.type == ItemID.StardustBreastplate && legs.type == ItemID.StardustLeggings && vanillaVanityToArmor)
            {
                return "Stardust";
            }
            return "";
        }
        public override void UpdateEquip(Item item, Player player)
        {
            bool vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;

            if (item.type == ItemID.AncientArmorHat && vanillaVanityToArmor)
            {
                player.GetCritChance(DamageClass.Melee) += 5;
                player.GetCritChance(DamageClass.Ranged) += 5;
                player.GetCritChance(DamageClass.Magic) += 5;
                player.GetCritChance(DamageClass.Throwing) += 5;
                player.ammoCost80 = true;
            }
            if (item.type == ItemID.AncientArmorShirt && vanillaVanityToArmor)
            {
                player.maxMinions += 1;
                player.GetDamage(DamageClass.Generic) *= 1.2f;
            }
            if (item.type == ItemID.AncientArmorPants && vanillaVanityToArmor)
            {
                player.GetAttackSpeed(DamageClass.Melee) *= 1.1f;
                player.manaCost *= 0.9f;
                player.lifeRegen += 2;
            }
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            bool vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;

            if (set == "Pharaoh" && vanillaVanityToArmor)
            {
                player.setBonus = "\n10% reduced mana usage\n+1 Minion capacity\n+10% Whip range\n5% increased movement speed\nGrants Immunity to Mighty Wind";
                player.manaCost -= 0.1f;
                player.maxMinions++;
                player.whipRangeMultiplier += 0.1f;
                player.moveSpeed += 0.05f;
                player.buffImmune[BuffID.WindPushed] = true;
            }
            if (set == "Ancient" && vanillaVanityToArmor)
            {
                player.GetModPlayer<RijamsModPlayer>().ancientSet = true;
                player.setBonus = "\nIncreased maximum running speed\nIncreased running acceleration\n+0.5 seconds flight time\n+15% Whip speed\n+20% Whip range\nAllows Shield of Cthulhu style dashing";
                player.dashType = 2; //Shield of Cthulhu dash
                //Set in PostUpdateRunSpeeds() in RijamsModPlayer.cs
                //player.runAcceleration += 0.1f;
                //player.maxRunSpeed += 2;
                player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
                player.whipRangeMultiplier += 0.2f;
                player.wingTimeMax += 30;
            }
            if ((player.setStardust || set == "Stardust") && vanillaVanityToArmor)
			{
                player.setBonus = Language.GetTextValue("ArmorSetBonus.Stardust") + "\n+15% Whip speed\n+20% Whip range";
                player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
                player.whipRangeMultiplier += 0.2f;
            }
        }
        public override void ArmorSetShadows(Player player, string set)
        {
            bool vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;

            if (set == "Ancient" && vanillaVanityToArmor)
            {
                player.armorEffectDrawShadow = true;
            }
        }
		public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
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
                if (arg == ItemID.EyeOfCthulhuBossBag && WorldGen.crimson)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.EyeOfCthulhuBossBag), ModContent.ItemType<Items.Weapons.Ammo.BloodyArrow>(), Main.rand.Next(20, 50));
                }
            }
            if (context == "crate")
            {
                if (arg == ItemID.WoodenCrate && Main.rand.NextBool(10))
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.WoodenCrate), ModContent.ItemType<Weapons.Belt>(), 1);
                }
                if (arg == ItemID.WoodenCrateHard && Main.rand.NextBool(10))
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.WoodenCrate), ModContent.ItemType<Weapons.Belt>(), 1);
                }
            }
        }

        private static readonly int[] whipPrefixes = new int[] { PrefixID.Broken, PrefixID.Terrible, PrefixID.Annoying, PrefixID.Unhappy,
            PrefixID.Shoddy, PrefixID.Shameful, PrefixID.Sluggish, PrefixID.Weak, PrefixID.Tiny, PrefixID.Slow, PrefixID.Dull, PrefixID.Damaged, PrefixID.Small,
            PrefixID.Lazy, PrefixID.Light, PrefixID.Heavy, PrefixID.Nimble, PrefixID.Keen, PrefixID.Ruthless, PrefixID.Bulky, PrefixID.Nasty, PrefixID.Pointy,
            PrefixID.Zealous, PrefixID.Quick, PrefixID.Hurtful, PrefixID.Large, PrefixID.Dangerous, PrefixID.Sharp, PrefixID.Strong, PrefixID.Forceful,
            PrefixID.Agile, PrefixID.Massive, PrefixID.Murderous, PrefixID.Unpleasant, PrefixID.Deadly, PrefixID.Demonic, PrefixID.Superior, PrefixID.Savage,
            PrefixID.Godly, PrefixID.Legendary};

        public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
            if (isWhip.Contains(item.type))
			{
                return rand.Next(whipPrefixes);
            }
			return base.ChoosePrefix(item, rand);
		}
		// Not needed anymore
		/*public override bool CanUseItem(Item item, Player player)
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
        }*/
	}

    //This is used for the Timon's Axe, Hammer of Retribution, and Quietus for checking if the glow mask should be drawn in ItemUseGlow
    public class MagicMeleeGlow : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(MagicMeleeGlow);
        public override string Texture => Item.type == ModContent.ItemType<MagicMeleeGlow>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');
    }
}