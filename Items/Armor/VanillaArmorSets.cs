using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor
{
	public class VanillaArmorSets : GlobalItem
	{
		public override void SetStaticDefaults()
		{
			RijamsModConfigServer.VanillaArmorList vanillaArmorChanges = ModContent.GetInstance<RijamsModConfigServer>().VanillaArmorChanges;
			if (vanillaArmorChanges.PharaohsSet)
			{
				ArmorSetBonuses.Add(ItemID.PharaohsMask, ItemID.PharaohsRobe, ItemID.None,
					LocalizedText: Language.GetText($"Mods.{Mod.Name}.ArmorSetBonus.Pharaoh"),
					PrimaryPart: ArmorSetBonus.PartType.None,
					Identifier: "RijamsMod/Pharaoh",
					Effect: SetBonus_Pharaoh);
			}
			if (vanillaArmorChanges.AncientSet)
			{
				ArmorSetBonuses.Add(ItemID.AncientArmorHat, ItemID.AncientArmorShirt, ItemID.AncientArmorPants,
					LocalizedText: Language.GetText($"Mods.{Mod.Name}.ArmorSetBonus.Ancient"),
					PrimaryPart: ArmorSetBonus.PartType.None,
					Identifier: "RijamsMod/Ancient",
					Effect: SetBonus_Ancient);
			}
		}

		public override void SetDefaults(Item item)
		{
			RijamsModConfigServer.VanillaArmorList vanillaArmorChanges = ModContent.GetInstance<RijamsModConfigServer>().VanillaArmorChanges;
			if (vanillaArmorChanges.PharaohsSet)
			{
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
			}
			if (vanillaArmorChanges.AncientSet)
			{
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
			}
			if (item.type == ItemID.FlinxFurCoat && vanillaArmorChanges.BuffFlinxFurCoat)
			{
				item.defense = 3;
			}
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			RijamsModConfigServer.VanillaArmorList vanillaArmorChanges = ModContent.GetInstance<RijamsModConfigServer>().VanillaArmorChanges;
			if (vanillaArmorChanges.AncientSet)
			{
				if (item.type == ItemID.AncientArmorHat)
				{
					if (GlobalItems.FindTooltipIndex(tooltips, "Defense", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "AncientHeaddress", "+5 critical strike chance"));
						tooltips.Insert(index + 2, new TooltipLine(Mod, "AncientHeaddress", "20% chance to not consume ammo"));
					}
				}
				if (item.type == ItemID.AncientArmorShirt)
				{
					if (GlobalItems.FindTooltipIndex(tooltips, "Defense", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "AncientGarments", "20% increased damage"));
						tooltips.Insert(index + 2, new TooltipLine(Mod, "AncientGarments", "+1 Minion capacity"));
						tooltips.Insert(index + 3, new TooltipLine(Mod, "AncientGarments", "+5 Support minion radius"));
					}
				}
				if (item.type == ItemID.AncientArmorPants)
				{
					if (GlobalItems.FindTooltipIndex(tooltips, "Defense", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "AncientSlacks", "10% increased melee speed"));
						tooltips.Insert(index + 2, new TooltipLine(Mod, "AncientSlacks", "10% reduced mana usage"));
						tooltips.Insert(index + 3, new TooltipLine(Mod, "AncientSlacks", "+2 life regeneration"));
					}
				}
			}
			if (item.type == ItemID.BeeGreaves && vanillaArmorChanges.BuffBeeGreaves)
			{
				if (GlobalItems.FindTooltipIndex(tooltips, "Tooltip0", "Terraria", out int index))
				{
					tooltips.Insert(index + 1, new TooltipLine(Mod, "BeeGreaves", "+1 Sentry capacity"));
				}
			}
		}

		public override void UpdateEquip(Item item, Player player)
		{
			RijamsModConfigServer.VanillaArmorList vanillaArmorChanges = ModContent.GetInstance<RijamsModConfigServer>().VanillaArmorChanges;

			if (vanillaArmorChanges.AncientSet)
			{
				if (item.type == ItemID.AncientArmorHat)
				{
					player.GetCritChance(DamageClass.Melee) += 5;
					player.GetCritChance(DamageClass.Ranged) += 5;
					player.GetCritChance(DamageClass.Magic) += 5;
					player.GetCritChance(DamageClass.Throwing) += 5;
					player.ammoCost80 = true;
				}
				if (item.type == ItemID.AncientArmorShirt)
				{
					player.maxMinions += 1;
					player.GetDamage(DamageClass.Generic) *= 1.2f;
					player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionRadiusIncrease += 5;
				}
				if (item.type == ItemID.AncientArmorPants)
				{
					player.GetAttackSpeed(DamageClass.Melee) *= 1.1f;
					player.manaCost *= 0.9f;
					player.lifeRegen += 2;
				}
			}
			if (item.type == ItemID.BeeGreaves && vanillaArmorChanges.BuffBeeGreaves)
			{
				player.maxTurrets++;
			}
		}

		public override void UpdateArmorSet(Player player, ArmorSetBonus armorSetBonus)
		{
			RijamsModConfigServer.VanillaArmorList vanillaArmorChanges = ModContent.GetInstance<RijamsModConfigServer>().VanillaArmorChanges;

			if ((player.setStardust || armorSetBonus?.Identifier == "Stardust") && vanillaArmorChanges.BuffStardustSetBonus)
			{
				// TODO player.setBonus = Language.GetTextValue("ArmorSetBonus.Stardust") + "\n+15% Whip speed";
				player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
			}
		}

		public override void ModifyArmorSets()
		{
			// To add to an existing armor set description, we will append to the existing description.
			// This localization key is "{0}" followed by a newline and then our additional description. The {0} will be replaced with the existing description.
			LocalizedText stardustSetBonusAddition = Language.GetOrRegister($"Mods.{Mod.Name}.ArmorSetBonus.Stardust");

			// We loop through all armor set bonuses and find the ones with the identifier "MagicHat", which is the identifier for the Magic Hat armor set bonus. There are multiple ArmorSetBonus entries for the Magic Hat armor set, one for each of the different body pieces (robes) that can be used to complete the set. We will want to affect all of them.
			foreach (var armorSetBonus in ArmorSetBonuses.All.Where(x => x.Identifier == "Stardust"))
			{
				armorSetBonus.Description = stardustSetBonusAddition.WithFormatArgs(armorSetBonus.Description);
			}
		}


		public static void SetBonus_Pharaoh(Player player)
		{
			player.manaCost -= 0.1f;
			player.maxMinions++;
			player.whipRangeMultiplier += 0.1f;
			player.moveSpeed += 0.05f;
			player.buffImmune[BuffID.WindPushed] = true;
		}
		public static void SetBonus_Ancient(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().Armor_AncientSet = true;
			player.dashType = 2; //Shield of Cthulhu dash
			//Set in PostUpdateRunSpeeds() in RijamsModPlayer.cs
			//player.runAcceleration += 0.1f;
			//player.maxRunSpeed += 2;
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
			player.whipRangeMultiplier += 0.2f;
			player.wingTimeMax += 30;
		}

		public override void ArmorSetShadows(Player player, string set)
		{
			RijamsModConfigServer.VanillaArmorList vanillaArmorChanges = ModContent.GetInstance<RijamsModConfigServer>().VanillaArmorChanges;

			if (set == "RijamsMod/Ancient" && vanillaArmorChanges.AncientSet)
			{
				player.armorEffectDrawShadow = true;
			}
		}
	}
}
