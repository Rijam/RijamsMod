using RijamsMod.Buffs.Minions;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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