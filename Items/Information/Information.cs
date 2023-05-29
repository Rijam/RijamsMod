using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Information
{
	public class LifeDisplay : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Life Display");
			// Tooltip.SetDefault("Displays your current life bonuses");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ManaDisplay>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			//item.color = Color.Red; //colors the inventory sprite
			Item.width = 32;
			Item.height = 28;
			Item.maxStack = 1;
			Item.rare = ItemRarityID.Blue;
			Item.value = 100;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "MaxLife", "[i:LifeCrystal] Maximum life: " + StatCalc.StatLifeMax()));
			tooltips.Add(new TooltipLine(Mod, "MaxTempLife", "[i:LifeCrystal] Maximum temporary life: " + StatCalc.StatLifeMax2()));
			tooltips.Add(new TooltipLine(Mod, "lifeRegen", "[i:Heart] Life regeneration: " + StatCalc.LifeRegen()));
			tooltips.Add(new TooltipLine(Mod, "lifeRegenTime", "[i:Heart] Life regeneration time: " + StatCalc.LifeRegenTime()));
		}
	}
	public class ManaDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mana Display");
			// Tooltip.SetDefault("Displays your current mana bonuses");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<DefenseDisplay>(); // Shimmer transforms the item.
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "statManaMax", "[i:ManaCrystal] Maximum mana: " + StatCalc.StatManaMax()));
			tooltips.Add(new TooltipLine(Mod, "statManaMax2", "[i:ManaCrystal] Maximum temporary mana: " + StatCalc.StatManaMax2()));
			tooltips.Add(new TooltipLine(Mod, "manaCost", "[i:Star] Mana cost multiplier: " + StatCalc.ManaCost()));
			tooltips.Add(new TooltipLine(Mod, "manaRegen", "[i:Star] Mana regeneration: " + StatCalc.ManaRegen()));
			tooltips.Add(new TooltipLine(Mod, "manaRegenBonus", "[i:Star] Mana regeneration bonus: " + StatCalc.ManaRegenBonus()));
		}
	}
	public class DefenseDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Defense Display");
			// Tooltip.SetDefault("Displays your current defense and knockback bonuses");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<MovementDisplay>(); // Shimmer transforms the item.
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "StatDefense", "[i:CobaltShield] Defense: " + StatCalc.StatDefense()));
			tooltips.Add(new TooltipLine(Mod, "Endurance", "[i:CobaltShield] Damage Reduction: " + StatCalc.Endurance() + "%"));
			tooltips.Add(new TooltipLine(Mod, "MeleeAP", "[i:CopperBroadsword] Bonus Melee armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "RangedAP", "[i:WoodenBow] Bonus Ranged armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "MagicAP", "[i:WandofSparking] Bonus Magic armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "SummonAP", "[i:BabyBirdStaff] Bonus Summon armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "ThrowingAP", "[i:ThrowingKnife] Bonus Throwing armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "AllAP", "[i:AvengerEmblem] Bonus All armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Generic)));
		}
	}
	public class MovementDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Movement Display");
			// Tooltip.SetDefault("Displays your current movement bonuses");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<DamageDisplay>(); // Shimmer transforms the item.
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "moveSpeed", "[i:HermesBoots] Movement speed multiplier: " + StatCalc.MoveSpeed()));
			tooltips.Add(new TooltipLine(Mod, "moveSpeed", "[i:HermesBoots] Maximum Running speed: " + StatCalc.MaxRunSpeed()));
			tooltips.Add(new TooltipLine(Mod, "runAcceleration", "[i:HermesBoots] Running acceleration speed: " + StatCalc.RunAcceleration()));
			tooltips.Add(new TooltipLine(Mod, "runSlowdown", "[i:HermesBoots] Running deceleration speed: " + StatCalc.RunSlowdown()));
			tooltips.Add(new TooltipLine(Mod, "wingTimeMax", "[i:AngelWings] Wing flight time: " + StatCalc.WingTimeMax()));
			tooltips.Add(new TooltipLine(Mod, "wingTime", "[i:AngelWings] Current wing flight time: " + StatCalc.WingTime()));
			tooltips.Add(new TooltipLine(Mod, "rocketTimeMax", "[i:RocketBoots] Rocket Boots flight time: " + StatCalc.RocketTimeMax()));
			tooltips.Add(new TooltipLine(Mod, "rocketTime", "[i:RocketBoots] Current Rocket Boots flight time: " + StatCalc.RocketTime()));
			tooltips.Add(new TooltipLine(Mod, "noKnockback", "[i:CobaltShield] Knockback immunity: " + StatCalc.NoKnockback()));
			tooltips.Add(new TooltipLine(Mod, "noFallDmg", "[i:LuckyHorseshoe] Fall damage immunity: " + StatCalc.NoFallDmg()));
		}
	}
	public class DamageDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Damage Display");
			// Tooltip.SetDefault("Displays your current damage bonuses\nValues greater than 1 means increased damage");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CritDisplay>(); // Shimmer transforms the item.
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Melee", "[i:CopperBroadsword] Melee damage multiplier: " + StatCalc.Damage(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "Ranged", "[i:WoodenBow] Ranged damage multiplier: " + StatCalc.Damage(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "Magic", "[i:WandofSparking] Magic damage multiplier: " + StatCalc.Damage(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "Summon", "[i:BabyBirdStaff] Summon damage multiplier: " + StatCalc.Damage(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "Throwing", "[i:ThrowingKnife] Throwing damage multiplier: " + StatCalc.Damage(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "All", "[i:AvengerEmblem] All damage multiplier: " + StatCalc.Damage(DamageClass.Generic)));
			tooltips.Add(new TooltipLine(Mod, "MeleeSpeed", "[i:CopperBroadsword] Melee speed: " + StatCalc.AttackSpeed(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "RangedSpeed", "[i:WoodenBow] Ranged speed: " + StatCalc.AttackSpeed(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "MagicSpeed", "[i:WandofSparking] Magic speed: " + StatCalc.AttackSpeed(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "SummonSpeed", "[i:BabyBirdStaff] Summon speed: " + StatCalc.AttackSpeed(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "ThrowingSpeed", "[i:ThrowingKnife] Throwing speed: " + StatCalc.AttackSpeed(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "AllSpeed", "[i:AvengerEmblem] All speed: " + StatCalc.AttackSpeed(DamageClass.Generic)));
		}
	}
	public class CritDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Critical Hit Display");
			// Tooltip.SetDefault("Displays your current critical hit bonuses");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SummonsDisplay>(); // Shimmer transforms the item.
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Melee", "[i:CopperBroadsword] Bonus Melee critical hit: " + StatCalc.CritChance(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "Ranged", "[i:WoodenBow] Bonus Ranged critical hit: " + StatCalc.CritChance(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "Magic", "[i:WandofSparking] Bonus Magic critical hit: " + StatCalc.CritChance(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "Summon", "[i:BabyBirdStaff] Bonus Summon critical hit: " + StatCalc.CritChance(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "Throwing", "[i:ThrowingKnife] Bonus Throwing critical hit: " + StatCalc.CritChance(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "All", "[i:AvengerEmblem] Bonus All critical hit: " + StatCalc.CritChance(DamageClass.Generic)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackMelee", "[i:CopperBroadsword] Melee Knockback: " + StatCalc.Knockback(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackRanged", "[i:WoodenBow] Ranged Knockback: " + StatCalc.Knockback(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackMagic", "[i:WandofSparking] Magic Knockback: " + StatCalc.Knockback(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "SummonKB", "[i:BabyBirdStaff] Summon Knockback: " + StatCalc.Knockback(DamageClass.Summon) + "    Summon KB Base: " + StatCalc.KnockbackBase(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackThrowing", "[i:ThrowingKnife] Throwing Knockback: " + StatCalc.Knockback(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackAll", "[i:AvengerEmblem] All Knockback: " + StatCalc.Knockback(DamageClass.Generic)));
		}
	}
	public class SummonsDisplay : LifeDisplay
	{
		//public override string Texture => "Terraria/Item_" + ItemID.REK;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Summons Display");
			// Tooltip.SetDefault("Displays your current summons capacity and bonus");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<LifeDisplay>(); // Shimmer transforms the item.
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "MinionCount", "[i:SlimeStaff] Maximum minions: " + StatCalc.MaxMinions()));
			tooltips.Add(new TooltipLine(Mod, "SentryCount", "[i:DD2BallistraTowerT1Popper] Maximum sentries: " + StatCalc.MaxTurrets()));
			tooltips.Add(new TooltipLine(Mod, "SummonMeleeSpeed", "[i:ThornWhip] Summon whip speed: " + StatCalc.AttackSpeed(DamageClass.SummonMeleeSpeed)));
			tooltips.Add(new TooltipLine(Mod, "WhipRangeMultiplier", "[i:BlandWhip] Whip range multiplier: " + StatCalc.WhipRangeMultiplier()));
			tooltips.Add(new TooltipLine(Mod, "SummonKB", "[i:BabyBirdStaff] Summon Knockback: " + StatCalc.Knockback(DamageClass.Summon) + "    Summon KB Base: " + StatCalc.KnockbackBase(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "SummonCountCurrent", "[i:SlimeStaff] Current minion count: " + StatCalc.SlotsMinions()));
		}
	}
	public class InformationInterface : ModItem
	{
		//public override string Texture => "Terraria/Item_" + ItemID.CellPhone;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Information Interface");
			// Tooltip.SetDefault("Displays your stats and bonuses\nHold Left Shift to see all player stats\nHold Left Control to see all damage stats");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(30, 12));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
			}
		}

		public override void SetDefaults()
		{
			//item.color = Color.Gold; //colors the inventory sprite
			Item.width = 48;
			Item.height = 48;
			Item.maxStack = 1;
			Item.rare = ItemRarityID.Orange;
			Item.value = 1000;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			bool isLeftCtrlHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl);
			bool isLeftShiftHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
			bool isRightShiftHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift);
			bool isRightControlHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl);

			if (isLeftShiftHeld)
			{
				//Life
				tooltips.Add(new TooltipLine(Mod, "MaxLife", "[i:LifeCrystal] Maximum life: " + StatCalc.StatLifeMax()));
				tooltips.Add(new TooltipLine(Mod, "MaxTempLife", "[i:LifeCrystal] Maximum temporary life: " + StatCalc.StatLifeMax2()));
				tooltips.Add(new TooltipLine(Mod, "lifeRegen", "[i:Heart] Life regeneration: " + StatCalc.LifeRegen()));
				tooltips.Add(new TooltipLine(Mod, "lifeRegenTime", "[i:Heart] Life regeneration time: " + StatCalc.LifeRegenTime()));

				//Mana
				tooltips.Add(new TooltipLine(Mod, "statManaMax", "[i:ManaCrystal] Maximum mana: " + StatCalc.StatManaMax()));
				tooltips.Add(new TooltipLine(Mod, "statManaMax2", "[i:ManaCrystal] Maximum temporary mana: " + StatCalc.StatManaMax2()));
				tooltips.Add(new TooltipLine(Mod, "manaCost", "[i:Star] Mana cost multiplier: " + StatCalc.ManaCost()));
				tooltips.Add(new TooltipLine(Mod, "manaRegen", "[i:Star] Mana regeneration: " + StatCalc.ManaRegen()));
				tooltips.Add(new TooltipLine(Mod, "manaRegenBonus", "[i:Star] Mana regeneration bonus: " + StatCalc.ManaRegenBonus()));

				//Defense
				tooltips.Add(new TooltipLine(Mod, "StatDefense", "[i:CobaltShield] Defense: " + StatCalc.StatDefense()));
				tooltips.Add(new TooltipLine(Mod, "Endurance", "[i:CobaltShield] Damage Reduction: " + StatCalc.Endurance() + "%"));

				//Movement
				tooltips.Add(new TooltipLine(Mod, "moveSpeed", "[i:HermesBoots] Movement speed multiplier: " + StatCalc.MoveSpeed()));
				tooltips.Add(new TooltipLine(Mod, "maxRunSpeed", "[i:HermesBoots] Maximum Running speed: " + StatCalc.MaxRunSpeed()));
				tooltips.Add(new TooltipLine(Mod, "runAcceleration", "[i:HermesBoots] Running acceleration speed: " + StatCalc.RunAcceleration()));
				tooltips.Add(new TooltipLine(Mod, "runSlowdown", "[i:HermesBoots] Running deceleration speed: " + StatCalc.RunSlowdown()));
				tooltips.Add(new TooltipLine(Mod, "wingTimeMax", "[i:AngelWings] Wing flight time: " + StatCalc.WingTimeMax()));
				tooltips.Add(new TooltipLine(Mod, "wingTime", "[i:AngelWings] Current wing flight time: " + StatCalc.WingTime()));
				tooltips.Add(new TooltipLine(Mod, "rocketTimeMax", "[i:RocketBoots] Rocket Boots flight time: " + StatCalc.RocketTimeMax()));
				tooltips.Add(new TooltipLine(Mod, "rocketTime", "[i:RocketBoots] Current Rocket Boots flight time: " + StatCalc.RocketTime()));
				tooltips.Add(new TooltipLine(Mod, "noKnockback", "[i:CobaltShield] Knockback immunity: " + StatCalc.NoKnockback()));
				tooltips.Add(new TooltipLine(Mod, "noFallDmg", "[i:LuckyHorseshoe] Fall damage immunity: " + StatCalc.NoFallDmg()));

				//Summons
				tooltips.Add(new TooltipLine(Mod, "WhipRangeMultiplier", "[i:BlandWhip] Whip range multiplier: " + StatCalc.WhipRangeMultiplier()));
				tooltips.Add(new TooltipLine(Mod, "MinionCount", "[i:SlimeStaff] Maximum minions: " + StatCalc.MaxMinions()));
				tooltips.Add(new TooltipLine(Mod, "SentryCount", "[i:DD2BallistraTowerT1Popper] Maximum sentries: " + StatCalc.MaxTurrets()));
				tooltips.Add(new TooltipLine(Mod, "SummonCountCurrent", "[i:SlimeStaff] Current minion count: " + StatCalc.SlotsMinions()));
			}

			if (isLeftCtrlHeld)
			{
				//Damage
				tooltips.Add(new TooltipLine(Mod, "Melee", "[i:CopperBroadsword] Melee damage multiplier: " + StatCalc.Damage(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "Ranged", "[i:WoodenBow] Ranged damage multiplier: " + StatCalc.Damage(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "Magic", "[i:WandofSparking] Magic damage multiplier: " + StatCalc.Damage(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "Summon", "[i:BabyBirdStaff] Summon damage multiplier: " + StatCalc.Damage(DamageClass.Summon)));
				tooltips.Add(new TooltipLine(Mod, "Throwing", "[i:ThrowingKnife] Throwing damage multiplier: " + StatCalc.Damage(DamageClass.Throwing)));
				tooltips.Add(new TooltipLine(Mod, "All", "[i:AvengerEmblem] All damage multiplier: " + StatCalc.Damage(DamageClass.Generic)));
				tooltips.Add(new TooltipLine(Mod, "MeleeSpeed", "[i:CopperBroadsword] Melee speed: " + StatCalc.AttackSpeed(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "RangedSpeed", "[i:WoodenBow] Ranged speed: " + StatCalc.AttackSpeed(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "MagicSpeed", "[i:WandofSparking] Magic speed: " + StatCalc.AttackSpeed(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "SummonSpeed", "[i:BabyBirdStaff] Summon speed: " + StatCalc.AttackSpeed(DamageClass.Summon)));
				tooltips.Add(new TooltipLine(Mod, "SummonMeleeSpeed", "[i:ThornWhip] Summon whip speed: " + StatCalc.AttackSpeed(DamageClass.SummonMeleeSpeed)));
				tooltips.Add(new TooltipLine(Mod, "ThrowingSpeed", "[i:ThrowingKnife] Throwing speed: " + StatCalc.AttackSpeed(DamageClass.Throwing)));
				tooltips.Add(new TooltipLine(Mod, "AllSpeed", "[i:AvengerEmblem] All speed: " + StatCalc.AttackSpeed(DamageClass.Generic)));

				//Crit
				tooltips.Add(new TooltipLine(Mod, "Melee", "[i:CopperBroadsword] Bonus Melee critical hit: " + StatCalc.CritChance(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "Ranged", "[i:WoodenBow] Bonus Ranged critical hit: " + StatCalc.CritChance(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "Magic", "[i:WandofSparking] Bonus Magic critical hit: " + StatCalc.CritChance(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "Summon", "[i:BabyBirdStaff] Bonus Summon critical hit: " + StatCalc.CritChance(DamageClass.Summon)));
				tooltips.Add(new TooltipLine(Mod, "Throwing", "[i:ThrowingKnife] Bonus Throwing critical hit: " + StatCalc.CritChance(DamageClass.Throwing)));
				tooltips.Add(new TooltipLine(Mod, "All", "[i:AvengerEmblem] Bonus All critical hit: " + StatCalc.CritChance(DamageClass.Generic)));

				//Armor Penetration
				tooltips.Add(new TooltipLine(Mod, "MeleeAP", "[i:CopperBroadsword] Bonus Melee armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "RangedAP", "[i:WoodenBow] Bonus Ranged armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "MagicAP", "[i:WandofSparking] Bonus Magic armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "SummonAP", "[i:BabyBirdStaff] Bonus Summon armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Summon)));
				tooltips.Add(new TooltipLine(Mod, "ThrowingAP", "[i:ThrowingKnife] Bonus Throwing armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Throwing)));
				tooltips.Add(new TooltipLine(Mod, "AllAP", "[i:AvengerEmblem] Bonus All armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Generic)));

				//Knockback
				tooltips.Add(new TooltipLine(Mod, "KnockbackMelee", "[i:CopperBroadsword] Melee Knockback: " + StatCalc.Knockback(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "KnockbackRanged", "[i:WoodenBow] Ranged Knockback: " + StatCalc.Knockback(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "KnockbackMagic", "[i:WandofSparking] Magic Knockback: " + StatCalc.Knockback(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "SummonKB", "[i:BabyBirdStaff] Summon Knockback: " + StatCalc.Knockback(DamageClass.Summon) + "    Summon KB Base: " + StatCalc.KnockbackBase(DamageClass.Summon)));
				tooltips.Add(new TooltipLine(Mod, "KnockbackThrowing", "[i:ThrowingKnife] Throwing Knockback: " + StatCalc.Knockback(DamageClass.Throwing)));
				tooltips.Add(new TooltipLine(Mod, "KnockbackAll", "[i:AvengerEmblem] All Knockback: " + StatCalc.Knockback(DamageClass.Generic)));
			}
			if (isRightShiftHeld)
			{
				//Other
				tooltips.Add(new TooltipLine(Mod, "taxMoney", "[i:GoldCoin] Tax money: " + StatCalc.TaxMoney()));
				tooltips.Add(new TooltipLine(Mod, "taxTimer", "[i:CopperCoin] Tax timer: " + StatCalc.TaxTimer()));
				//tooltips.Add(new TooltipLine(Mod, "taxRate", "Tax rate: " + taxRateString));
				tooltips.Add(new TooltipLine(Mod, "anglerQuestsFinished", "[i:WoodFishingPole] Angler quests finished: " + StatCalc.AnglerQuestsFinished()));
				tooltips.Add(new TooltipLine(Mod, "breath", "[i:Bubble] Current breath: " + StatCalc.Breath()));
				tooltips.Add(new TooltipLine(Mod, "breathCD", "[i:Bubble] Drowning damage: " + StatCalc.BreathCD()));
				tooltips.Add(new TooltipLine(Mod, "breathMax", "[i:Bubble] Max breath: " + StatCalc.BreathMax()));
				tooltips.Add(new TooltipLine(Mod, "lavaImmune", "[i:ObsidianSkinPotion] Lava immunity: " + StatCalc.LavaImmune()));
				tooltips.Add(new TooltipLine(Mod, "lavaMax", "[i:LavaCharm] Lava immunity Time: " + StatCalc.LavaTime()));
				tooltips.Add(new TooltipLine(Mod, "pickSpeed", "[i:IronPickaxe] Mining speed: " + StatCalc.PickSpeed()));
				tooltips.Add(new TooltipLine(Mod, "aggro", "[i:FleshKnuckles] Aggro range: " + StatCalc.Aggro()));
				tooltips.Add(new TooltipLine(Mod, "ZoneWaterCandle", "[i:WaterCandle] Near Water Candle: " + StatCalc.ZoneWaterCandle()));
				tooltips.Add(new TooltipLine(Mod, "ZonePeaceCandle", "[i:PeaceCandle] Near Peace Candle: " + StatCalc.ZonePeaceCandle()));
				tooltips.Add(new TooltipLine(Mod, "ZoneShadowCandle", "[i:ShadowCandle] Near Shadow Candle: " + StatCalc.ZoneShadowCandle()));
				tooltips.Add(new TooltipLine(Mod, "InZonePurity", "[i:Sunflower] In Purity biome: " + StatCalc.PlayerInZonePurity()));
				tooltips.Add(new TooltipLine(Mod, "InZoneCorrupt", "[i:VileMushroom] In Corruption biome: " + StatCalc.PlayerInZoneCorrupt()));
				tooltips.Add(new TooltipLine(Mod, "InZoneCrimson", "[i:ViciousMushroom] In Crimson biome: " + StatCalc.PlayerInZoneCrimson()));
				tooltips.Add(new TooltipLine(Mod, "InZoneHallow", "[i:CrystalShard] In Hallow biome: " + StatCalc.PlayerInZoneHallow()));
				tooltips.Add(new TooltipLine(Mod, "GolfScore", "[i:GolfClubIron] Golf score: " + StatCalc.GolferScoreAccumulated()));
				tooltips.Add(new TooltipLine(Mod, "Luck", "[i:LuckPotionGreater] Luck: " + StatCalc.Luck()));
				tooltips.Add(new TooltipLine(Mod, "CriticalHitAdditionalDamage", "[i:RijamsMod/CritDisplay] [Rijam's Mod] Additional Critical Hit Damage: " + StatCalc.CriticalHitAdditionalDamage()));
				tooltips.Add(new TooltipLine(Mod, "SupportMinionRadiusIncrease", "[i:RijamsMod/SummonsDisplay] [Rijam's Mod] Support Minion Radius Increase: " + StatCalc.SupportMinionRadiusIncrease()));
				tooltips.Add(new TooltipLine(Mod, "PosX", "[i:WorldGlobe] Position X Pixels: " + Main.LocalPlayer.position.X));
				tooltips.Add(new TooltipLine(Mod, "PosY", "[i:WorldGlobe] Position Y Pixels: " + Main.LocalPlayer.position.Y));
				tooltips.Add(new TooltipLine(Mod, "PosX", "[i:WorldGlobe] Position X Tiles: " + Main.LocalPlayer.position.X / 16f));
				tooltips.Add(new TooltipLine(Mod, "PosY", "[i:WorldGlobe] Position Y Tiles: " + Main.LocalPlayer.position.Y / 16f));
			}
			if (isRightControlHeld)
			{
				tooltips.Add(new TooltipLine(Mod, "DemonHeart", "[i:DemonHeart] Used Demon Heart: " + Main.LocalPlayer.extraAccessory));
				tooltips.Add(new TooltipLine(Mod, "ArtisanLoaf", "[i:ArtisanLoaf] Used Artisan Loaf: " + Main.LocalPlayer.ateArtisanBread));
				tooltips.Add(new TooltipLine(Mod, "TorchGodsFavor", "[i:TorchGodsFavor] Used Torch God's Favor: " + Main.LocalPlayer.unlockedBiomeTorches));
				tooltips.Add(new TooltipLine(Mod, "MinecartUpgradeKit", "[i:MinecartPowerup] Used Minecart Upgrade Kit: " + Main.LocalPlayer.unlockedSuperCart));
				tooltips.Add(new TooltipLine(Mod, "VitalCrystal", "[i:AegisCrystal] Used Vital Crystal: " + Main.LocalPlayer.usedAegisCrystal));
				tooltips.Add(new TooltipLine(Mod, "ArcaneCrystal", "[i:ArcaneCrystal] Used Arcane Crystal: " + Main.LocalPlayer.usedArcaneCrystal));
				tooltips.Add(new TooltipLine(Mod, "AegisFruit", "[i:AegisFruit] Used Aegis Fruit: " + Main.LocalPlayer.usedAegisFruit));
				tooltips.Add(new TooltipLine(Mod, "Ambrosia", "[i:Ambrosia] Used Ambrosia: " + Main.LocalPlayer.usedAmbrosia));
				tooltips.Add(new TooltipLine(Mod, "GummyWorm", "[i:GummyWorm] Used Gummy Worm: " + Main.LocalPlayer.usedGummyWorm));
				tooltips.Add(new TooltipLine(Mod, "GalaxyPearl", "[i:GalaxyPearl] Used Galaxy Pearl: " + Main.LocalPlayer.usedGalaxyPearl));
				tooltips.Add(new TooltipLine(Mod, "CombatBook", "[i:CombatBook] Used Advanced Combat Techniques: " + NPC.combatBookWasUsed));
				tooltips.Add(new TooltipLine(Mod, "CombatBookVolumeTwo", "[i:CombatBookVolumeTwo] Used Advanced Combat Techniques: Volume Two: " + NPC.combatBookVolumeTwoWasUsed));
				tooltips.Add(new TooltipLine(Mod, "PeddlersSatchel", "[i:PeddlersSatchel] Used Peddler's Satchel: " + NPC.peddlersSatchelWasUsed));
				tooltips.Add(new TooltipLine(Mod, "DontHurtCrittersBook", "[i:DontHurtCrittersBook] Guide to Critter Companionship active: " + Main.LocalPlayer.dontHurtCritters));
				tooltips.Add(new TooltipLine(Mod, "DontHurtNatureBook", "[i:DontHurtNatureBook] Guide to Environmental Preservation active: " + Main.LocalPlayer.dontHurtNature));
				tooltips.Add(new TooltipLine(Mod, "EncumberingStone", "[i:EncumberingStone] Encumbering Stone active: " + Main.LocalPlayer.preventAllItemPickups));
				tooltips.Add(new TooltipLine(Mod, "PlayerDifficulty", "[i:TeamBlockWhite] Player Difficulty: " + Main.LocalPlayer.difficulty));
				tooltips.Add(new TooltipLine(Mod, "WorldDifficulty", "[i:TeamBlockWhite] World Difficulty: " + (Main.expertMode.ToInt() + Main.masterMode.ToInt() + Main.getGoodWorld.ToInt())));
			}
			if (!isLeftShiftHeld && !isLeftCtrlHeld && !isRightShiftHeld && !isRightControlHeld)
			{
				tooltips.Add(new TooltipLine(Mod, "lifeRegen", "[i:Heart] Life regeneration: " + StatCalc.LifeRegen()));
				tooltips.Add(new TooltipLine(Mod, "manaRegen", "[i:Star] Mana regeneration: " + StatCalc.ManaRegen()));
				tooltips.Add(new TooltipLine(Mod, "Endurance", "[i:CobaltShield] Damage Reduction: " + StatCalc.Endurance() + "%"));
				tooltips.Add(new TooltipLine(Mod, "moveSpeed", "[i:HermesBoots] Movement speed multiplier: " + StatCalc.MoveSpeed()));
				tooltips.Add(new TooltipLine(Mod, "wingTimeMax", "[i:AngelWings] Wing flight time: " + StatCalc.WingTimeMax()));
				tooltips.Add(new TooltipLine(Mod, "All", "[i:AvengerEmblem] All damage multiplier: " + StatCalc.Damage(DamageClass.Generic)));
				tooltips.Add(new TooltipLine(Mod, "MinionCount", "[i:SlimeStaff] Maximum minions: " + StatCalc.MaxMinions()));
				tooltips.Add(new TooltipLine(Mod, "SentryCount", "[i:DD2BallistraTowerT1Popper] Maximum sentries: " + StatCalc.MaxTurrets()));
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<LifeDisplay>(), 1)
				.AddIngredient(ModContent.ItemType<ManaDisplay>(), 1)
				.AddIngredient(ModContent.ItemType<DefenseDisplay>(), 1)
				.AddIngredient(ModContent.ItemType<MovementDisplay>(), 1)
				.AddIngredient(ModContent.ItemType<DamageDisplay>(), 1)
				.AddIngredient(ModContent.ItemType<CritDisplay>(), 1)
				.AddIngredient(ModContent.ItemType<SummonsDisplay>(), 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
	public class StatCalc
	{
		static readonly Player player = Main.LocalPlayer;
		static readonly RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();

		private static string colorLife = "[c/ff0000:";
		private static string colorMana = "[c/0000ff:";
		private static string colorDefense = "[c/999999:";
		private static string colorKnockback = "[c/ff0077:";
		private static string colorMovement = "[c/00ff00:";
		private static string colorDamage = "[c/ff00ff:";
		private static string colorSpeed = "[c/00ffff:";
		private static string colorCrit = "[c/ffff00:";
		private static string colorAP = "[c/00ff77:";
		private static string colorSummon = "[c/ff7700:";
		private static string colorOther = "[c/ff7777:";
		private static string colorClose = "]";

		//Life
		public static string StatLifeMax() => colorLife + player.statLifeMax.ToString() + colorClose;
		public static string StatLifeMax2() => colorLife + player.statLifeMax2.ToString() + colorClose;
		public static string LifeRegen() => colorLife + player.lifeRegen.ToString() + colorClose;
		public static string LifeRegenTime() => colorLife + player.lifeRegenTime.ToString() + colorClose;

		//Mana
		public static string StatManaMax() => colorMana + player.statManaMax.ToString() + colorClose;
		public static string StatManaMax2() => colorMana + player.statManaMax2.ToString() + colorClose;
		public static string ManaCost() => colorMana + player.manaCost.ToString() + colorClose;
		public static string ManaRegen() => colorMana + player.manaRegen.ToString() + colorClose;
		public static string ManaRegenBonus() => colorMana + player.manaRegenBonus.ToString() + colorClose;

		//Defense
		public static string StatDefense() => colorDefense + player.statDefense.ToString() + colorClose;
		public static string Endurance() => colorDefense + (player.endurance * 100).ToString() + colorClose;

		//Movement
		public static string MoveSpeed() => colorMovement + player.moveSpeed.ToString() + colorClose;
		public static string MaxRunSpeed() => colorMovement + player.maxRunSpeed.ToString() + colorClose;
		public static string RunAcceleration() => colorMovement + player.runAcceleration.ToString() + colorClose;
		public static string RunSlowdown() => colorMovement + player.runSlowdown.ToString() + colorClose;
		public static string WingTimeMax() => colorMovement + player.wingTimeMax.ToString() + colorClose;
		public static string WingTime() => colorMovement + player.wingTime.ToString() + colorClose;
		public static string RocketTimeMax() => colorMovement + player.rocketTimeMax.ToString() + colorClose;
		public static string RocketTime() => colorMovement + player.rocketTime.ToString() + colorClose;
		public static string NoKnockback() => colorMovement + player.noKnockback.ToString() + colorClose;

		public static string NoFallDmg() //Having wings prevents fall damage but doesn't change the noFallDmg bool
		{
			return colorMovement + (player.noFallDmg || player.wings > 0).ToString() + colorClose;
		}

		//Damage
		public static string Damage(DamageClass damageClass) //Adapted from Fargo's Mutant Mod: https://github.com/Fargowilta/Fargowiltas/blob/master/UI/StatSheetUI.cs#L75
		{
			return colorDamage + Math.Round(player.GetTotalDamage(damageClass).Additive * player.GetTotalDamage(damageClass).Multiplicative, 2).ToString() + colorClose;
		}
		public static string DamageBase(DamageClass damageClass)
		{
			return colorDamage + Math.Round(player.GetTotalDamage(damageClass).Base, 2).ToString() + colorClose;
		}
		public static string DamageFlat(DamageClass damageClass)
		{
			return colorDamage + Math.Round(player.GetTotalDamage(damageClass).Flat, 2).ToString() + colorClose;
		}
		public static string AttackSpeed(DamageClass damageClass) => colorSpeed + player.GetAttackSpeed(damageClass).ToString() + colorClose;

		//Crit
		public static string CritChance(DamageClass damageClass) => colorCrit + player.GetCritChance(damageClass).ToString() + colorClose;

		//Knockback
		public static string Knockback(DamageClass damageClass)
		{
			return colorKnockback + Math.Round(player.GetKnockback(damageClass).Additive * player.GetKnockback(damageClass).Multiplicative, 2).ToString() + colorClose;
		}
		public static string KnockbackBase(DamageClass damageClass)
		{
			return colorKnockback + Math.Round(player.GetKnockback(damageClass).Base, 2).ToString() + colorClose;
		}
		public static string KnockbackFlat(DamageClass damageClass)
		{
			return colorKnockback + Math.Round(player.GetKnockback(damageClass).Flat, 2).ToString() + colorClose;
		}

		//Armor Penetration
		public static string ArmorPenetration(DamageClass damageClass) => colorAP + player.GetArmorPenetration(damageClass).ToString() + colorClose;

		//Summons
		public static string MaxMinions() => colorSummon + player.maxMinions.ToString() + colorClose;
		public static string MaxTurrets() => colorSummon + player.maxTurrets.ToString() + colorClose;
		public static string NumMinions() => colorSummon + player.numMinions.ToString() + colorClose;
		public static string SlotsMinions() => colorSummon + player.slotsMinions.ToString() + colorClose;
		public static string WhipRangeMultiplier() => colorSummon + player.whipRangeMultiplier.ToString() + colorClose;

		//Other
		public static string TaxMoney() => colorOther + player.taxMoney.ToString() + colorClose;
		public static string TaxTimer() => colorOther + player.taxTimer.ToString() + colorClose;
		public static string AnglerQuestsFinished() => colorOther + player.anglerQuestsFinished.ToString() + colorClose;
		public static string Breath() => colorOther + player.breath.ToString() + colorClose;
		public static string BreathCD() => colorOther + player.breathCD.ToString() + colorClose;
		public static string BreathMax() => colorOther + player.breathMax.ToString() + colorClose;
		public static string LavaImmune() => colorOther + player.lavaImmune.ToString() + colorClose;
		public static string LavaTime() => colorOther + player.lavaTime.ToString() + colorClose;
		public static string PickSpeed() => colorOther + player.pickSpeed.ToString() + colorClose;
		public static string Aggro() => colorOther + player.aggro.ToString() + colorClose;
		public static string ZoneWaterCandle() => colorOther + player.ZoneWaterCandle.ToString() + colorClose;
		public static string ZonePeaceCandle() => colorOther + player.ZonePeaceCandle.ToString() + colorClose;
		public static string ZoneShadowCandle() => colorOther + player.ZoneShadowCandle.ToString() + colorClose;
		public static string PlayerInZonePurity() => colorOther + player.InZonePurity().ToString() + colorClose;
		public static string PlayerInZoneCorrupt() => colorOther + player.ZoneCorrupt.ToString() + colorClose;
		public static string PlayerInZoneCrimson() => colorOther + player.ZoneCrimson.ToString() + colorClose;
		public static string PlayerInZoneHallow() => colorOther + player.ZoneHallow.ToString() + colorClose;
		public static string GolferScoreAccumulated() => colorOther + player.golferScoreAccumulated.ToString() + colorClose;
		public static string Luck() => colorOther + player.luck.ToString() + colorClose;
		public static string CriticalHitAdditionalDamage() => colorCrit + modPlayer.criticalHitAdditionalDamage + colorClose;
		public static string SupportMinionRadiusIncrease() => colorSummon + modPlayer.supportMinionRadiusIncrease + colorClose;
	}
}