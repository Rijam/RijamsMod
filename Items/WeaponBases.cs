using RijamsMod.Buffs.Minions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace RijamsMod.Items
{
	// This is used for the Timon's Axe, Hammer of Retribution, and Quietus for checking if the glow mask should be drawn in ItemUseGlow
	public class MagicMeleeGlow : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(MagicMeleeGlow);
		public override string Texture => Item.type == ModContent.ItemType<MagicMeleeGlow>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');
	}

	public class SupportMinionCanUseCheck : GlobalItem
	{

		// The player can't use the support minion weapons if they already have one active.
		public static List<int> SupportMinionsDefenseBuffs = new();
		public static List<int> SupportMinionsHealingBuffs = new();

		public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
			return entity.ModItem is CudgelDefenseItem || entity.ModItem is CudgelHealingItem || entity.ModItem is CudgelBuffItem;
		}

		public override bool CanUseItem(Item item, Player player)
		{
			if (item.ModItem is CudgelDefenseItem)
			{
				for (int i = 0; i < SupportMinionsDefenseBuffs.Count; i++)
				{
					if (player.HasBuff(SupportMinionsDefenseBuffs[i]))
					{
						return false;
					}
				}
			}
			if (item.ModItem is CudgelHealingItem)
			{
				for (int i = 0; i < SupportMinionsHealingBuffs.Count; i++)
				{
					if (player.HasBuff(SupportMinionsHealingBuffs[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Hide the damage and knockback lines because they don't really apply to the items.
		// The items need damage set for reforging.
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.ModItem is CudgelDefenseItem || item.ModItem is CudgelHealingItem || item.ModItem is CudgelBuffItem)
			{
				if (GlobalItems.FindTooltipIndex(tooltips, "Damage", "Terraria", out int indexDamage))
				{
					tooltips.RemoveAt(indexDamage);
				}
				if (GlobalItems.FindTooltipIndex(tooltips, "Knockback", "Terraria", out int indexKnockback))
				{
					tooltips.RemoveAt(indexKnockback);
				}
			}
		}
	}

	// This is used for the Cudgel support minion items.
	public class CudgelDefenseItem : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(CudgelDefenseItem);
		public override string Texture => Item.type == ModContent.ItemType<CudgelDefenseItem>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GlobalItems.FindTooltipIndex(tooltips, "Tooltip0", "Terraria", out int index))
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "SupportMinionMessage1", "[c/00af00:- Defense Support Minion -]"));
				tooltips.Insert(index + 2, new TooltipLine(Mod, "SupportMinionMessage2", "[c/00af00:Maximum of one defense support minion per player]"));
			}
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			ItemID.Sets.CanGetPrefixes[Item.type] = true;
		}

		public int AdditionalDefense = 0;
		public float AdditionalDamageReduction = 0;
		public int AdditionalRadius = 0;

		public virtual int Defense => 0 + AdditionalDefense;
		public virtual float DamageReduction => 0 + AdditionalDamageReduction;
		public virtual int Radius => 0 + AdditionalRadius;

		internal static readonly int[] allowedPrefixes = new int[] { PrefixID.Adept, PrefixID.Inept, ModContent.PrefixType<CudgelPrefixReaching>(),
			ModContent.PrefixType<CudgelPrefixShort>(), ModContent.PrefixType<CudgelPrefixHampered>(), ModContent.PrefixType<CudgelPrefixProtective>(),
			ModContent.PrefixType<CudgelPrefixSupportive>() };

		public override bool CanReforge()
		{
			return true;
		}

		public override int ChoosePrefix(UnifiedRandom rand)
		{
			return rand.Next(allowedPrefixes);
		}

		public override bool? PrefixChance(int pre, UnifiedRandom rand)
		{
			if (pre == -3)
			{
				return true;
			}
			return base.PrefixChance(pre, rand);
		}

		/*public override bool AllowPrefix(int pre)
		{
			if (Array.IndexOf(allowedPrefixes, pre) > -1)
			{
				// IndexOf returns a positive index of the element you search for. If not found, it's less than 0.
				// Here we check if the selected prefix is positive (it was found).
				// If so, we found a prefix that we do want.
				return true;
			}

			// Reroll.
			return false;
		}*/
	}
	public class CudgelHealingItem : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(CudgelHealingItem);
		public override string Texture => Item.type == ModContent.ItemType<CudgelHealingItem>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GlobalItems.FindTooltipIndex(tooltips, "Tooltip0", "Terraria", out int index))
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "SupportMinionMessage1", "[c/00af00:- Healing Support Minion -]"));
				tooltips.Insert(index + 2, new TooltipLine(Mod, "SupportMinionMessage2", "[c/00af00:Maximum of one healing support minion per player]"));
			}
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			ItemID.Sets.CanGetPrefixes[Item.type] = true;
		}

		public int AdditionalHealingAmount = 0;
		public int DecreasedHealingTime = 0;
		public int AdditionalRadius = 0;

		public virtual int HealingAmount => 0 + AdditionalHealingAmount;
		public virtual int HealingTime => 0 + DecreasedHealingTime;
		public virtual int Radius => 0 + AdditionalRadius;

		internal static readonly int[] allowedPrefixes = new int[] { PrefixID.Adept, PrefixID.Inept, ModContent.PrefixType<CudgelPrefixReaching>(),
			ModContent.PrefixType<CudgelPrefixShort>(), ModContent.PrefixType<CudgelPrefixNoxious>(), ModContent.PrefixType<CudgelPrefixVigilant>(),
			ModContent.PrefixType<CudgelPrefixCurative>() };

		public override int ChoosePrefix(UnifiedRandom rand)
		{
			return rand.Next(allowedPrefixes);
		}
	}
	public class CudgelBuffItem : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(CudgelBuffItem);
		public override string Texture => Item.type == ModContent.ItemType<CudgelBuffItem>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GlobalItems.FindTooltipIndex(tooltips, "Tooltip0", "Terraria", out int index))
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "SupportMinionMessage1", "[c/00af00:- Buff Support Minion -]"));
				tooltips.Insert(index + 2, new TooltipLine(Mod, "SupportMinionMessage2", "[c/00af00:Maximum of one buff support minion per player]"));
			}
		}
	}
}