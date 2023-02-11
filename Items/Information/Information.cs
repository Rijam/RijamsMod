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
			tooltips.Add(new TooltipLine(Mod, "MaxLife", "Maximum life: " + StatCalc.StatLifeMax()));
			tooltips.Add(new TooltipLine(Mod, "MaxTempLife", "Maximum temporary life: " + StatCalc.StatLifeMax2()));
			tooltips.Add(new TooltipLine(Mod, "lifeRegen", "Life regeneration: " + StatCalc.LifeRegen()));
			tooltips.Add(new TooltipLine(Mod, "lifeRegenTime", "Life regeneration time: " + StatCalc.LifeRegenTime()));
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
			tooltips.Add(new TooltipLine(Mod, "statManaMax", "Maximum mana: " + StatCalc.StatManaMax()));
			tooltips.Add(new TooltipLine(Mod, "statManaMax2", "Maximum temporary mana: " + StatCalc.StatManaMax2()));
			tooltips.Add(new TooltipLine(Mod, "manaCost", "Mana cost multiplier: " + StatCalc.ManaCost()));
			tooltips.Add(new TooltipLine(Mod, "manaRegen", "Mana regeneration: " + StatCalc.ManaRegen()));
			tooltips.Add(new TooltipLine(Mod, "manaRegenBonus", "Mana regeneration bonus: " + StatCalc.ManaRegenBonus()));
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
			tooltips.Add(new TooltipLine(Mod, "StatDefense", "Defense: " + StatCalc.StatDefense()));
			tooltips.Add(new TooltipLine(Mod, "Endurance", "Damage Reduction: " + StatCalc.Endurance() + "%"));
			tooltips.Add(new TooltipLine(Mod, "MeleeAP", "Bonus Melee armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "RangedAP", "Bonus Ranged armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "MagicAP", "Bonus Magic armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "SummonAP", "Bonus Summon armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "ThrowingAP", "Bonus Throwing armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "AllAP", "Bonus All armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Generic)));
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
			tooltips.Add(new TooltipLine(Mod, "moveSpeed", "Movement speed multiplier: " + StatCalc.MoveSpeed()));
			tooltips.Add(new TooltipLine(Mod, "moveSpeed", "Maximum Running speed: " + StatCalc.MaxRunSpeed()));
			tooltips.Add(new TooltipLine(Mod, "runAcceleration", "Running acceleration speed: " + StatCalc.RunAcceleration()));
			tooltips.Add(new TooltipLine(Mod, "runSlowdown", "Running deceleration speed: " + StatCalc.RunSlowdown()));
			tooltips.Add(new TooltipLine(Mod, "wingTimeMax", "Wing flight time: " + StatCalc.WingTimeMax()));
			tooltips.Add(new TooltipLine(Mod, "wingTime", "Current wing flight time: " + StatCalc.WingTime()));
			tooltips.Add(new TooltipLine(Mod, "rocketTimeMax", "Rocket Boots flight time: " + StatCalc.RocketTimeMax()));
			tooltips.Add(new TooltipLine(Mod, "rocketTime", "Current Rocket Boots flight time: " + StatCalc.RocketTime()));
			tooltips.Add(new TooltipLine(Mod, "noKnockback", "Knockback immunity: " + StatCalc.NoKnockback()));
			tooltips.Add(new TooltipLine(Mod, "noFallDmg", "Fall damage immunity: " + StatCalc.NoFallDmg()));
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
			tooltips.Add(new TooltipLine(Mod, "Melee", "Melee damage multiplier: " + StatCalc.Damage(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "Ranged", "Ranged damage multiplier: " + StatCalc.Damage(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "Magic", "Magic damage multiplier: " + StatCalc.Damage(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "Summon", "Summon damage multiplier: " + StatCalc.Damage(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "Throwing", "Throwing damage multiplier: " + StatCalc.Damage(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "All", "All damage multiplier: " + StatCalc.Damage(DamageClass.Generic)));
			tooltips.Add(new TooltipLine(Mod, "MeleeSpeed", "Melee speed: " + StatCalc.AttackSpeed(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "RangedSpeed", "Ranged speed: " + StatCalc.AttackSpeed(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "MagicSpeed", "Magic speed: " + StatCalc.AttackSpeed(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "SummonSpeed", "Summon speed: " + StatCalc.AttackSpeed(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "ThrowingSpeed", "Throwing speed: " + StatCalc.AttackSpeed(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "AllSpeed", "All speed: " + StatCalc.AttackSpeed(DamageClass.Generic)));
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
			tooltips.Add(new TooltipLine(Mod, "Melee", "Bonus Melee critical hit: " + StatCalc.CritChance(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "Ranged", "Bonus Ranged critical hit: " + StatCalc.CritChance(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "Magic", "Bonus Magic critical hit: " + StatCalc.CritChance(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "Summon", "Bonus Summon critical hit: " + StatCalc.CritChance(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "Throwing", "Bonus Throwing critical hit: " + StatCalc.CritChance(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "All", "Bonus All critical hit: " + StatCalc.CritChance(DamageClass.Generic)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackMelee", "Melee Knockback: " + StatCalc.Knockback(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackRanged", "Ranged Knockback: " + StatCalc.Knockback(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackMagic", "Magic Knockback: " + StatCalc.Knockback(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "SummonKB", "Summon knockback: " + StatCalc.Knockback(DamageClass.Summon) + "    Summon KB Base: " + StatCalc.KnockbackBase(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackThrowing", "Throwing Knockback: " + StatCalc.Knockback(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackAll", "All Knockback: " + StatCalc.Knockback(DamageClass.Generic)));
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
			tooltips.Add(new TooltipLine(Mod, "MinionCount", "Maximum minions: " + StatCalc.MaxMinions()));
			tooltips.Add(new TooltipLine(Mod, "SentryCount", "Maximum sentries: " + StatCalc.MaxTurrets()));
			tooltips.Add(new TooltipLine(Mod, "SummonMeleeSpeed", "Summon whip speed: " + StatCalc.AttackSpeed(DamageClass.SummonMeleeSpeed)));
			tooltips.Add(new TooltipLine(Mod, "WhipRangeMultiplier", "Whip range multiplier: " + StatCalc.WhipRangeMultiplier()));
			tooltips.Add(new TooltipLine(Mod, "SummonKB", "Summon knockback: " + StatCalc.Knockback(DamageClass.Summon) + "    Summon KB Base: " + StatCalc.KnockbackBase(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "SummonCountCurrent", "Current minion count: " + StatCalc.NumMinions()));
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

			if (isLeftShiftHeld)
			{
				//Life
				tooltips.Add(new TooltipLine(Mod, "MaxLife", "Maximum life: " + StatCalc.StatLifeMax()));
				tooltips.Add(new TooltipLine(Mod, "MaxTempLife", "Maximum temporary life: " + StatCalc.StatLifeMax2()));
				tooltips.Add(new TooltipLine(Mod, "lifeRegen", "Life regeneration: " + StatCalc.LifeRegen()));
				tooltips.Add(new TooltipLine(Mod, "lifeRegenTime", "Life regeneration time: " + StatCalc.LifeRegenTime()));

				//Mana
				tooltips.Add(new TooltipLine(Mod, "statManaMax", "Maximum mana: " + StatCalc.StatManaMax()));
				tooltips.Add(new TooltipLine(Mod, "statManaMax2", "Maximum temporary mana: " + StatCalc.StatManaMax2()));
				tooltips.Add(new TooltipLine(Mod, "manaCost", "Mana cost multiplier: " + StatCalc.ManaCost()));
				tooltips.Add(new TooltipLine(Mod, "manaRegen", "Mana regeneration: " + StatCalc.ManaRegen()));
				tooltips.Add(new TooltipLine(Mod, "manaRegenBonus", "Mana regeneration bonus: " + StatCalc.ManaRegenBonus()));

				//Defense
				tooltips.Add(new TooltipLine(Mod, "StatDefense", "Defense: " + StatCalc.StatDefense()));
				tooltips.Add(new TooltipLine(Mod, "Endurance", "Damage Reduction: " + StatCalc.Endurance() + "%"));

				//Movement
				tooltips.Add(new TooltipLine(Mod, "moveSpeed", "Movement speed multiplier: " + StatCalc.MoveSpeed()));
				tooltips.Add(new TooltipLine(Mod, "maxRunSpeed", "Maximum Running speed: " + StatCalc.MaxRunSpeed()));
				tooltips.Add(new TooltipLine(Mod, "runAcceleration", "Running acceleration speed: " + StatCalc.RunAcceleration()));
				tooltips.Add(new TooltipLine(Mod, "runSlowdown", "Running deceleration speed: " + StatCalc.RunSlowdown()));
				tooltips.Add(new TooltipLine(Mod, "wingTimeMax", "Wing flight time: " + StatCalc.WingTimeMax()));
				tooltips.Add(new TooltipLine(Mod, "wingTime", "Current wing flight time: " + StatCalc.WingTime()));
				tooltips.Add(new TooltipLine(Mod, "rocketTimeMax", "Rocket Boots flight time: " + StatCalc.RocketTimeMax()));
				tooltips.Add(new TooltipLine(Mod, "rocketTime", "Current Rocket Boots flight time: " + StatCalc.RocketTime()));
				tooltips.Add(new TooltipLine(Mod, "noKnockback", "Knockback immunity: " + StatCalc.NoKnockback()));
				tooltips.Add(new TooltipLine(Mod, "noFallDmg", "Fall damage immunity: " + StatCalc.NoFallDmg()));

				//Summons
				tooltips.Add(new TooltipLine(Mod, "WhipRangeMultiplier", "Whip range multiplier: " + StatCalc.WhipRangeMultiplier()));
				tooltips.Add(new TooltipLine(Mod, "MinionCount", "Maximum minions: " + StatCalc.MaxMinions()));
				tooltips.Add(new TooltipLine(Mod, "SentryCount", "Maximum sentries: " + StatCalc.MaxTurrets()));
				tooltips.Add(new TooltipLine(Mod, "SummonCountCurrent", "Current minion count: " + StatCalc.NumMinions()));
			}

			if (isLeftCtrlHeld)
			{
				//Damage
				tooltips.Add(new TooltipLine(Mod, "Melee", "Melee damage multiplier: " + StatCalc.Damage(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "Ranged", "Ranged damage multiplier: " + StatCalc.Damage(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "Magic", "Magic damage multiplier: " + StatCalc.Damage(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "Summon", "Summon damage multiplier: " + StatCalc.Damage(DamageClass.Summon)));
				tooltips.Add(new TooltipLine(Mod, "Throwing", "Throwing damage multiplier: " + StatCalc.Damage(DamageClass.Throwing)));
				tooltips.Add(new TooltipLine(Mod, "All", "All damage multiplier: " + StatCalc.Damage(DamageClass.Generic)));
				tooltips.Add(new TooltipLine(Mod, "MeleeSpeed", "Melee speed: " + StatCalc.AttackSpeed(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "RangedSpeed", "Ranged speed: " + StatCalc.AttackSpeed(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "MagicSpeed", "Magic speed: " + StatCalc.AttackSpeed(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "SummonSpeed", "Summon speed: " + StatCalc.AttackSpeed(DamageClass.Summon)));
				tooltips.Add(new TooltipLine(Mod, "SummonMeleeSpeed", "Summon whip speed: " + StatCalc.AttackSpeed(DamageClass.SummonMeleeSpeed)));
				tooltips.Add(new TooltipLine(Mod, "ThrowingSpeed", "Throwing speed: " + StatCalc.AttackSpeed(DamageClass.Throwing)));
				tooltips.Add(new TooltipLine(Mod, "AllSpeed", "All speed: " + StatCalc.AttackSpeed(DamageClass.Generic)));

				//Crit
				tooltips.Add(new TooltipLine(Mod, "Melee", "Bonus Melee critical hit: " + StatCalc.CritChance(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "Ranged", "Bonus Ranged critical hit: " + StatCalc.CritChance(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "Magic", "Bonus Magic critical hit: " + StatCalc.CritChance(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "Summon", "Bonus Summon critical hit: " + StatCalc.CritChance(DamageClass.Summon)));
				tooltips.Add(new TooltipLine(Mod, "Throwing", "Bonus Throwing critical hit: " + StatCalc.CritChance(DamageClass.Throwing)));
				tooltips.Add(new TooltipLine(Mod, "All", "Bonus All critical hit: " + StatCalc.CritChance(DamageClass.Generic)));

				//Armor Penetration
				tooltips.Add(new TooltipLine(Mod, "MeleeAP", "Bonus Melee armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "RangedAP", "Bonus Ranged armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "MagicAP", "Bonus Magic armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "SummonAP", "Bonus Summon armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Summon)));
				tooltips.Add(new TooltipLine(Mod, "ThrowingAP", "Bonus Throwing armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Throwing)));
				tooltips.Add(new TooltipLine(Mod, "AllAP", "Bonus All armor penetration: " + StatCalc.ArmorPenetration(DamageClass.Generic)));

				//Knockback
				tooltips.Add(new TooltipLine(Mod, "KnockbackMelee", "Melee Knockback: " + StatCalc.Knockback(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "KnockbackRanged", "Ranged Knockback: " + StatCalc.Knockback(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "KnockbackMagic", "Magic Knockback: " + StatCalc.Knockback(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "SummonKB", "Summon knockback: " + StatCalc.Knockback(DamageClass.Summon) + "    Summon KB Base: " + StatCalc.KnockbackBase(DamageClass.Summon)));
				tooltips.Add(new TooltipLine(Mod, "KnockbackThrowing", "Throwing Knockback: " + StatCalc.Knockback(DamageClass.Throwing)));
				tooltips.Add(new TooltipLine(Mod, "KnockbackAll", "All Knockback: " + StatCalc.Knockback(DamageClass.Generic)));
			}
			if (isRightShiftHeld)
			{
				//Other
				tooltips.Add(new TooltipLine(Mod, "taxMoney", "Tax money: " + StatCalc.TaxMoney()));
				tooltips.Add(new TooltipLine(Mod, "taxTimer", "Tax timer: " + StatCalc.TaxTimer()));
				//tooltips.Add(new TooltipLine(Mod, "taxRate", "Tax rate: " + taxRateString));
				tooltips.Add(new TooltipLine(Mod, "anglerQuestsFinished", "Angler quests finished: " + StatCalc.AnglerQuestsFinished()));
				tooltips.Add(new TooltipLine(Mod, "breath", "Current breath: " + StatCalc.Breath()));
				tooltips.Add(new TooltipLine(Mod, "breathCD", "Drowning damage: " + StatCalc.BreathCD()));
				tooltips.Add(new TooltipLine(Mod, "breathMax", "Max breath: " + StatCalc.BreathMax()));
				tooltips.Add(new TooltipLine(Mod, "lavaImmune", "Lava immunity: " + StatCalc.LavaImmune()));
				tooltips.Add(new TooltipLine(Mod, "pickSpeed", "Mining speed: " + StatCalc.PickSpeed()));
				tooltips.Add(new TooltipLine(Mod, "aggro", "Aggro range: " + StatCalc.Aggro()));
				tooltips.Add(new TooltipLine(Mod, "ZoneWaterCandle", "Near Water Candle: " + StatCalc.ZoneWaterCandle()));
				tooltips.Add(new TooltipLine(Mod, "ZonePeaceCandle", "Near Peace Candle: " + StatCalc.ZonePeaceCandle()));
				tooltips.Add(new TooltipLine(Mod, "ZoneShadowCandle", "Near Shadow Candle: " + StatCalc.ZoneShadowCandle()));
				tooltips.Add(new TooltipLine(Mod, "InZonePurity", "In Purity biome: " + StatCalc.PlayerInZonePurity()));
				tooltips.Add(new TooltipLine(Mod, "InZoneCorrupt", "In Corruption biome: " + StatCalc.PlayerInZoneCorrupt()));
				tooltips.Add(new TooltipLine(Mod, "InZoneCrimson", "In Crimson biome: " + StatCalc.PlayerInZoneCrimson()));
				tooltips.Add(new TooltipLine(Mod, "InZoneHallow", "In Hallow biome: " + StatCalc.PlayerInZoneHallow()));
				tooltips.Add(new TooltipLine(Mod, "GolfScore", "Golf score: " + StatCalc.GolferScoreAccumulated()));
				tooltips.Add(new TooltipLine(Mod, "Luck", "Luck: " + StatCalc.Luck()));
				tooltips.Add(new TooltipLine(Mod, "PosX", "Position X Pixels: " + Main.LocalPlayer.position.X));
				tooltips.Add(new TooltipLine(Mod, "PosY", "Position Y Pixels: " + Main.LocalPlayer.position.Y));
				tooltips.Add(new TooltipLine(Mod, "PosX", "Position X Tiles: " + Main.LocalPlayer.position.X / 16f));
				tooltips.Add(new TooltipLine(Mod, "PosY", "Position Y Tiles: " + Main.LocalPlayer.position.Y / 16f));
			}
			if (!isLeftShiftHeld && !isLeftCtrlHeld && !isRightShiftHeld)
			{
				tooltips.Add(new TooltipLine(Mod, "lifeRegen", "Life regeneration: " + StatCalc.LifeRegen()));
				tooltips.Add(new TooltipLine(Mod, "manaRegen", "Mana regeneration: " + StatCalc.ManaRegen()));
				tooltips.Add(new TooltipLine(Mod, "Endurance", "Damage Reduction: " + StatCalc.Endurance() + "%"));
				tooltips.Add(new TooltipLine(Mod, "moveSpeed", "Movement speed multiplier: " + StatCalc.MoveSpeed()));
				tooltips.Add(new TooltipLine(Mod, "wingTimeMax", "Wing flight time: " + StatCalc.WingTimeMax()));
				tooltips.Add(new TooltipLine(Mod, "All", "All damage multiplier: " + StatCalc.Damage(DamageClass.Generic)));
				tooltips.Add(new TooltipLine(Mod, "MinionCount", "Maximum minions: " + StatCalc.MaxMinions()));
				tooltips.Add(new TooltipLine(Mod, "SentryCount", "Maximum sentries: " + StatCalc.MaxTurrets()));
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
		public static string WhipRangeMultiplier() => colorSummon + player.whipRangeMultiplier.ToString() + colorClose;

		//Other
		public static string TaxMoney() => colorOther + player.taxMoney.ToString() + colorClose;
		public static string TaxTimer() => colorOther + player.taxTimer.ToString() + colorClose;
		public static string AnglerQuestsFinished() => colorOther + player.anglerQuestsFinished.ToString() + colorClose;
		public static string Breath() => colorOther + player.breath.ToString() + colorClose;
		public static string BreathCD() => colorOther + player.breathCD.ToString() + colorClose;
		public static string BreathMax() => colorOther + player.breathMax.ToString() + colorClose;
		public static string LavaImmune() => colorOther + player.lavaImmune.ToString() + colorClose;
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
	}
}