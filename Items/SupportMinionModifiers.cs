using System;
using System.Collections.Generic;
using System.Globalization;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items
{
	public class CudgelPrefixBase : ModPrefix
	{
		/// <summary> Smaller numbers are better </summary>
		public virtual float ManaCostMultiplicitive => 0;
		/// <summary> Bigger numbers are better </summary>
		public virtual int IncreaseRadius => 0;
		/// <summary> Bigger numbers are better </summary>
		public virtual int IncreaseDefense => 0;
		/// <summary> Bigger numbers are better </summary>
		public virtual float IncreaseDamageReduction => 0;
		/// <summary> Bigger numbers are better </summary>
		public virtual int IncreaseHealingAmount => 0;
		/// <summary> Bigger numbers are better </summary>
		public virtual int DecreaseHealingTime => 0;

		private float CalculatePower()
		{
			float runningPower = 0;
			//runningPower += Math.Clamp(1f - ManaCostAdditive, -2, 2); // Smaller numbers are better
			runningPower += Math.Clamp(IncreaseRadius, -2, 2);
			runningPower += Math.Clamp(IncreaseDefense, -2, 2);
			runningPower += Math.Clamp(IncreaseDamageReduction, -2, 2);
			runningPower += Math.Clamp(IncreaseHealingAmount, -2, 2);
			runningPower += Math.Clamp(DecreaseHealingTime, -2, 2);
			return runningPower;
		}

		// See documentation for vanilla weights and more information.
		// In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
		// Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
		// Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
		public override PrefixCategory Category => PrefixCategory.Custom;

		// See documentation for vanilla weights and more information.
		// In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
		// Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
		// Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
		public override float RollChance(Item item)
		{
			return 2f;
		}

		// Determines if it can roll at all.
		// Use this to control if a prefix can be rolled or not.
		public override bool CanRoll(Item item)
		{
			return true;
		}

		// Use this function to modify these stats for items which have this prefix:
		// Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			manaMult *= ManaCostMultiplicitive;
		}

		// Modify the cost of items with this modifier with this function.
		public override void ModifyValue(ref float valueMult)
		{
			valueMult *= 1f + 0.05f * CalculatePower();
		}

		// This is used to modify most other stats of items which have this modifier.
		public override void Apply(Item item)
		{
			if (item.ModItem is CudgelDefenseItem defenseItem)
			{
				defenseItem.AdditionalRadius += IncreaseRadius;
				defenseItem.AdditionalDefense += IncreaseDefense;
				defenseItem.AdditionalDamageReduction += IncreaseDamageReduction;
			}
			if (item.ModItem is CudgelHealingItem healingItem)
			{
				healingItem.AdditionalRadius += IncreaseRadius;
				healingItem.AdditionalHealingAmount += IncreaseHealingAmount;
				healingItem.DecreasedHealingTime -= DecreaseHealingTime;
			}
		}

		// This prefix doesn't affect any non-standard stats, so these additional tooltiplines aren't actually necessary, but this pattern can be followed for a prefix that does affect other stats.
		public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
		{
			// Due to inheritance, this code runs for ExamplePrefix and ExampleDerivedPrefix. We add 2 tooltip lines, the first is the typical prefix tooltip line showing the stats boost, while the other is just some additional flavor text.

			// The localization key for Mods.ExampleMod.Prefixes.PowerTooltip uses a special format that will automatically prefix + or - to the value.
			// This shared localization is formatted with the Power value, resulting in different text for ExamplePrefix and ExampleDerivedPrefix.
			// This results in "+1 Power" for ExamplePrefix and "+2 Power" for ExampleDerivedPrefix.
			// Power isn't an actual stat, the effects of Power are already shown in the "+X% damage" tooltip, so this example is purely educational.
			/*yield return new TooltipLine(Mod, "ManaCostAdditiveTooltip", ManaCostAdditiveTooltip.Format(ManaCostAdditive.ToString("P")))
			{
				IsModifier = true, // Sets the color to the positive modifier color.
			};*/
			if (IncreaseRadius > 0) // 0 not included
			{
				yield return new TooltipLine(Mod, "IncreaseRadiusTooltip", IncreaseRadiusTooltip.Format(IncreaseRadius))
				{
					IsModifier = true, // Sets the color to the positive modifier color.
				};
			}
			if (IncreaseRadius < 0)
			{
				yield return new TooltipLine(Mod, "IncreaseRadiusTooltip", IncreaseRadiusTooltip.Format(IncreaseRadius))
				{
					IsModifier = true, // Sets the color to the positive modifier color.
					IsModifierBad = true
				};
			}
			if (IncreaseDefense > 0)
			{
				yield return new TooltipLine(Mod, "IncreaseDefenseTooltip", IncreaseDefenseTooltip.Format(IncreaseDefense))
				{
					IsModifier = true, // Sets the color to the positive modifier color.
				};
			}
			if (IncreaseDefense < 0)
			{
				yield return new TooltipLine(Mod, "IncreaseDefenseTooltip", IncreaseDefenseTooltip.Format(IncreaseDefense))
				{
					IsModifier = true, // Sets the color to the positive modifier color.
					IsModifierBad = true
				};
			}
			if (IncreaseDamageReduction > 0)
			{
				yield return new TooltipLine(Mod, "IncreaseDamageReductionTooltip", IncreaseDamageReductionTooltip.Format(IncreaseDamageReduction * 100f))
				{
					IsModifier = true, // Sets the color to the positive modifier color.
				};
			}
			if (IncreaseDamageReduction < 0)
			{
				yield return new TooltipLine(Mod, "IncreaseDamageReductionTooltip", IncreaseDamageReductionTooltip.Format(IncreaseDamageReduction * 100f))
				{
					IsModifier = true, // Sets the color to the positive modifier color.
					IsModifierBad = true
				};
			}
			if (IncreaseHealingAmount > 0)
			{
				yield return new TooltipLine(Mod, "IncreaseHealingAmountTooltip", IncreaseHealingAmountTooltip.Format(IncreaseHealingAmount))
				{
					IsModifier = true, // Sets the color to the positive modifier color.
				};
			}
			if (IncreaseHealingAmount < 0)
			{
				yield return new TooltipLine(Mod, "IncreaseHealingAmountTooltip", IncreaseHealingAmountTooltip.Format(IncreaseHealingAmount))
				{
					IsModifier = true, // Sets the color to the positive modifier color.
					IsModifierBad = true
				};
			}
			if (DecreaseHealingTime > 0)
			{
				yield return new TooltipLine(Mod, "DecreaseHealingTimeTooltip", DecreaseHealingTimeTooltip.Format(DecreaseHealingTime / 60))
				{
					IsModifier = true, // Sets the color to the positive modifier color.
				};
			}
			if (DecreaseHealingTime < 0)
			{
				yield return new TooltipLine(Mod, "DecreaseHealingTimeTooltip", DecreaseHealingTimeTooltip.Format(DecreaseHealingTime / 60))
				{
					IsModifier = true, // Sets the color to the positive modifier color.
					IsModifierBad = true
				};
			}

			// This localization is not shared with the inherited classes. ExamplePrefix and ExampleDerivedPrefix have their own translations for this line.
			/*yield return new TooltipLine(Mod, "PrefixWeaponAwesomeDescription", AdditionalTooltip.Value)
			{
				IsModifier = true,
			};*/
			// If possible and suitable, try to reuse the name identifier and translation value of Terraria prefixes. For example, this code uses the vanilla translation for the word defense, resulting in "-5 defense". Note that IsModifierBad is used for this bad modifier.
			/*yield return new TooltipLine(Mod, "PrefixAccDefense", "-5" + Lang.tip[25].Value) {
				IsModifier = true,
				IsModifierBad = true,
			};*/
		}

		// PowerTooltip is shared between ExamplePrefix and ExampleDerivedPrefix. 
		public static LocalizedText ManaCostAdditiveTooltip { get; private set; }
		public static LocalizedText IncreaseRadiusTooltip { get; private set; }
		public static LocalizedText IncreaseDefenseTooltip { get; private set; }
		public static LocalizedText IncreaseDamageReductionTooltip { get; private set; }
		public static LocalizedText IncreaseHealingAmountTooltip { get; private set; }
		public static LocalizedText DecreaseHealingTimeTooltip { get; private set; }


		public override void SetStaticDefaults()
		{
			// this.GetLocalization is not used here because we want to use a shared key
			ManaCostAdditiveTooltip = Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(ManaCostAdditiveTooltip)}"));
			IncreaseRadiusTooltip = Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(IncreaseRadiusTooltip)}"));
			IncreaseDefenseTooltip = Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(IncreaseDefenseTooltip)}"));
			IncreaseDamageReductionTooltip = Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(IncreaseDamageReductionTooltip)}"));
			IncreaseHealingAmountTooltip = Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(IncreaseHealingAmountTooltip)}"));
			DecreaseHealingTimeTooltip = Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(DecreaseHealingTimeTooltip)}"));
		}
	}

	/// <summary> For all support minions </summary>
	public class CudgelPrefixReaching : CudgelPrefixBase
	{
		public override float ManaCostMultiplicitive => 1f;
		public override int IncreaseRadius => 10;
	}
	/// <summary> For all support minions </summary>
	public class CudgelPrefixShort : CudgelPrefixBase
	{
		public override float ManaCostMultiplicitive => 1f;
		public override int IncreaseRadius => -5;
	}

	/// <summary> For defense support minions </summary>
	public class CudgelPrefixHampered : CudgelPrefixBase
	{
		public override float ManaCostMultiplicitive => 1.4f;
		public override int IncreaseRadius => -5;
		public override int IncreaseDefense => -3;
		public override float IncreaseDamageReduction => -0.05f;
	}
	/// <summary> For defense support minions </summary>
	public class CudgelPrefixProtective : CudgelPrefixBase
	{
		public override float ManaCostMultiplicitive => 0.75f;
		public override float IncreaseDamageReduction => 0.05f;
	}
	/// <summary> For defense support minions </summary>
	public class CudgelPrefixSupportive : CudgelPrefixBase
	{
		public override float ManaCostMultiplicitive => 0.85f;
		public override int IncreaseRadius => 5;
		public override int IncreaseDefense => 3;
		public override float IncreaseDamageReduction => 0.02f;
	}

	/// <summary> For healing support minions </summary>
	public class CudgelPrefixNoxious : CudgelPrefixBase
	{
		public override float ManaCostMultiplicitive => 1.2f;
		public override int IncreaseHealingAmount => -10;
		public override int DecreaseHealingTime => -10 * 60; // The time is in ticks
		public override int IncreaseRadius => -5;
	}
	/// <summary> For healing support minions </summary>
	public class CudgelPrefixVigilant : CudgelPrefixBase
	{
		public override float ManaCostMultiplicitive => 0.8f;
		public override int DecreaseHealingTime => 10 * 60; // The time is in ticks
	}
	/// <summary> For healing support minions </summary>
	public class CudgelPrefixCurative : CudgelPrefixBase
	{
		public override float ManaCostMultiplicitive => 0.85f;
		public override int IncreaseHealingAmount => 10;
		public override int DecreaseHealingTime => 5 * 60; // The time is in ticks
		public override int IncreaseRadius => 5;
	}
}