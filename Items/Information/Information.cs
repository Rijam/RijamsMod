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
			DisplayName.SetDefault("Life Display");
			Tooltip.SetDefault("Displays your current life bonuses");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
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
			DisplayName.SetDefault("Mana Display");
			Tooltip.SetDefault("Displays your current mana bonuses");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
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
			DisplayName.SetDefault("Defense Display");
			Tooltip.SetDefault("Displays your current defense and knockback bonuses");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "StatDefense", "Defense: " + StatCalc.StatDefense()));
			tooltips.Add(new TooltipLine(Mod, "Endurance", "Damage Reduction: " + StatCalc.Endurance() + "%"));
			tooltips.Add(new TooltipLine(Mod, "KnockbackMelee", "Melee Knockback: " + StatCalc.Knockback(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackRanged", "Ranged Knockback: " + StatCalc.Knockback(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackMagic", "Magic Knockback: " + StatCalc.Knockback(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackThrowing", "Throwing Knockback: " + StatCalc.Knockback(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "KnockbackAll", "All Knockback: " + StatCalc.Knockback(DamageClass.Generic)));
		}
	}
	public class MovementDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Movement Display");
			Tooltip.SetDefault("Displays your current movement bonuses");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
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
			DisplayName.SetDefault("Damage Display");
			Tooltip.SetDefault("Displays your current damage bonuses\nValues greater than 1 means increased damage");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
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
			DisplayName.SetDefault("Critical Hit Display");
			Tooltip.SetDefault("Displays your current critical hit bonuses");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Melee", "Bonus Melee critical hit: " + StatCalc.CritChance(DamageClass.Melee)));
			tooltips.Add(new TooltipLine(Mod, "Ranged", "Bonus Ranged critical hit: " + StatCalc.CritChance(DamageClass.Ranged)));
			tooltips.Add(new TooltipLine(Mod, "Magic", "Bonus Magic critical hit: " + StatCalc.CritChance(DamageClass.Magic)));
			tooltips.Add(new TooltipLine(Mod, "Summon", "Bonus Summon critical hit: " + StatCalc.CritChance(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "Throwing", "Bonus Throwing critical hit: " + StatCalc.CritChance(DamageClass.Throwing)));
			tooltips.Add(new TooltipLine(Mod, "All", "Bonus All critical hit: " + StatCalc.CritChance(DamageClass.Generic)));
		}
	}
	public class SummonsDisplay : LifeDisplay
	{
		//public override string Texture => "Terraria/Item_" + ItemID.REK;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Summons Display");
			Tooltip.SetDefault("Displays your current summons capacity and bonus");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 2));
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "MinionCount", "Maximum minions: " + StatCalc.MaxMinions()));
			tooltips.Add(new TooltipLine(Mod, "SentryCount", "Maximum sentries: " + StatCalc.MaxTurrets()));
			tooltips.Add(new TooltipLine(Mod, "SummonMeleeSpeed", "Summon whip speed: " + StatCalc.AttackSpeed(DamageClass.SummonMeleeSpeed)));
			tooltips.Add(new TooltipLine(Mod, "SummonKB", "Summon knockback: " + StatCalc.Knockback(DamageClass.Summon)));
			tooltips.Add(new TooltipLine(Mod, "SummonCountCurrent", "Current minion count: " + StatCalc.NumMinions()));
		}
	}
	public class InformationInterface : ModItem
	{
		//public override string Texture => "Terraria/Item_" + ItemID.CellPhone;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Information Interface");
			Tooltip.SetDefault("Displays your stats and bonuses\nHold Left Shift to see all player stats\nHold Left Control to see all damage stats");
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
				tooltips.Add(new TooltipLine(Mod, "moveSpeed", "Maximum Running speed: " + StatCalc.MaxRunSpeed()));
				tooltips.Add(new TooltipLine(Mod, "runAcceleration", "Running acceleration speed: " + StatCalc.RunAcceleration()));
				tooltips.Add(new TooltipLine(Mod, "runSlowdown", "Running deceleration speed: " + StatCalc.RunSlowdown()));
				tooltips.Add(new TooltipLine(Mod, "wingTimeMax", "Wing flight time: " + StatCalc.WingTimeMax()));
				tooltips.Add(new TooltipLine(Mod, "wingTime", "Current wing flight time: " + StatCalc.WingTime()));
				tooltips.Add(new TooltipLine(Mod, "rocketTimeMax", "Rocket Boots flight time: " + StatCalc.RocketTimeMax()));
				tooltips.Add(new TooltipLine(Mod, "rocketTime", "Current Rocket Boots flight time: " + StatCalc.RocketTime()));
				tooltips.Add(new TooltipLine(Mod, "noKnockback", "Knockback immunity: " + StatCalc.NoKnockback()));
				tooltips.Add(new TooltipLine(Mod, "noFallDmg", "Fall damage immunity: " + StatCalc.NoFallDmg()));

				//Summons
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

				//Knockback
				tooltips.Add(new TooltipLine(Mod, "KnockbackMelee", "Melee Knockback: " + StatCalc.Knockback(DamageClass.Melee)));
				tooltips.Add(new TooltipLine(Mod, "KnockbackRanged", "Ranged Knockback: " + StatCalc.Knockback(DamageClass.Ranged)));
				tooltips.Add(new TooltipLine(Mod, "KnockbackMagic", "Magic Knockback: " + StatCalc.Knockback(DamageClass.Magic)));
				tooltips.Add(new TooltipLine(Mod, "SummonKB", "Summon knockback: " + StatCalc.Knockback(DamageClass.Summon)));
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
				tooltips.Add(new TooltipLine(Mod, "ZoneWaterCandle", "Near Water Candle: " + StatCalc.ZoneWaterCandle()));
				tooltips.Add(new TooltipLine(Mod, "ZonePeaceCandle", "Near Peace Candle: " + StatCalc.ZonePeaceCandle()));
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

		//Life
		public static string StatLifeMax() => player.statLifeMax.ToString();
		public static string StatLifeMax2() => player.statLifeMax2.ToString();
		public static string LifeRegen() => player.lifeRegen.ToString();
		public static string LifeRegenTime() => player.lifeRegenTime.ToString();

		//Mana
		public static string StatManaMax() => player.statManaMax.ToString();
		public static string StatManaMax2() => player.statManaMax2.ToString();
		public static string ManaCost() => player.manaCost.ToString();
		public static string ManaRegen() => player.manaRegen.ToString();
		public static string ManaRegenBonus() => player.manaRegenBonus.ToString();

		//Defense
		public static string StatDefense() => player.statDefense.ToString();
		public static string Endurance() => (player.endurance * 100).ToString();

		//Movement
		public static string MoveSpeed() => player.moveSpeed.ToString();
		public static string MaxRunSpeed() => player.maxRunSpeed.ToString();
		public static string RunAcceleration() => player.runAcceleration.ToString();
		public static string RunSlowdown() => player.runSlowdown.ToString();
		public static string WingTimeMax() => player.wingTimeMax.ToString();
		public static string WingTime() => player.wingTime.ToString();
		public static string RocketTimeMax() => player.rocketTimeMax.ToString();
		public static string RocketTime() => player.rocketTime.ToString();
		public static string NoKnockback() => player.noKnockback.ToString();

		public static bool NoFallDmg() //Having wings prevents fall damage but doesn't change the noFallDmg bool
		{
			return player.noFallDmg || player.wings > 0;
		}

		//Damage
		public static string Damage(DamageClass damageClass) //Adapted from Fargo's Mutant Mod: https://github.com/Fargowilta/Fargowiltas/blob/master/UI/StatSheetUI.cs#L75
		{
			return Math.Round(player.GetTotalDamage(damageClass).Additive * player.GetTotalDamage(damageClass).Multiplicative, 2).ToString();
		}
		public static string AttackSpeed(DamageClass damageClass) => player.GetAttackSpeed(damageClass).ToString();

		//Crit
		public static string CritChance(DamageClass damageClass) => player.GetCritChance(damageClass).ToString();

		public static string Knockback(DamageClass damageClass)
		{
			return Math.Round(player.GetKnockback(damageClass).Additive * player.GetKnockback(damageClass).Multiplicative, 2).ToString();
		}

		//Summons
		public static string MaxMinions() => player.maxMinions.ToString();
		public static string MaxTurrets() => player.maxTurrets.ToString();
		public static string NumMinions() => player.numMinions.ToString();

		//Other
		public static string TaxMoney() => player.taxMoney.ToString();
		public static string TaxTimer() => player.taxTimer.ToString();
		public static string AnglerQuestsFinished() => player.anglerQuestsFinished.ToString();
		public static string Breath() => player.breath.ToString();
		public static string BreathCD() => player.breathCD.ToString();
		public static string BreathMax() => player.breathMax.ToString();
		public static string LavaImmune() => player.lavaImmune.ToString();
		public static string PickSpeed() => player.pickSpeed.ToString();
		public static string ZoneWaterCandle() => player.ZoneWaterCandle.ToString();
		public static string ZonePeaceCandle() => player.ZonePeaceCandle.ToString();
		public static string PlayerInZonePurity() => player.InZonePurity().ToString();
		public static string PlayerInZoneCorrupt() => player.ZoneCorrupt.ToString();
		public static string PlayerInZoneCrimson() => player.ZoneCrimson.ToString();
		public static string PlayerInZoneHallow() => player.ZoneHallow.ToString();
		public static string GolferScoreAccumulated() => player.golferScoreAccumulated.ToString();
		public static string Luck() => player.luck.ToString();
	}
}