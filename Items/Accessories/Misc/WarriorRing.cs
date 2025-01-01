using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Items.Materials;

namespace RijamsMod.Items.Accessories.Misc
{
	// Effects in RingGlobalNPCs

	[AutoloadEquip(EquipType.HandsOn)]
	public class WarriorRing : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().warriorRing = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(RecipeGroupID.IronBar, 10)
				.AddIngredient(ItemID.Obsidian, 25)
				.AddIngredient(ItemID.FrostCore, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.HandsOn)]
	public class PeakPerformanceRing : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().peakPerformanceRing = true;
			PeakPerformanceUpdate(player, player.GetModPlayer<RijamsModPlayer>().peakPerformanceRing);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			PeakPerformanceTooltip(Main.LocalPlayer, tooltips, Mod, Main.LocalPlayer.GetModPlayer<RijamsModPlayer>().peakPerformanceRing, 3);
		}

		public static void PeakPerformanceUpdate(Player player, bool equipped)
		{
			if (!equipped)
			{
				return;
			}

			float percentOfLife = player.statLife / (float)player.statLifeMax2;
			if (percentOfLife > 0.5f)
			{
				// Decrease the bonuses sharply if not at max health
				float multiplier = (float)Math.Pow(percentOfLife, 8);
				// 50% HP -> multiplier = <0.01
				// 75% HP -> multiplier = 0.10 -> +2% Damage  +1% Attack Speed
				// 85% HP -> multiplier = 0.27 -> +5% Damage  +2% Attack Speed
				// 90% HP -> multiplier = 0.43 -> +8% Damage  +4% Attack Speed
				// 95% HP -> multiplier = 0.66 -> +13% Damage +6% Attack Speed
				// 99% HP -> multiplier = 0.92 -> +18% Damage +9% Attack Speed
				// (Give or take some rounding)

				player.GetDamage(DamageClass.Generic) += 0.2f * multiplier;
				player.GetAttackSpeed(DamageClass.Generic) += 0.1f * multiplier;
			}
		}

		public static void PeakPerformanceTooltip(Player player, List<TooltipLine> tooltips, Mod mod, bool equipped, int tooltipNum)
		{
			if (equipped && GlobalItems.FindTooltipIndex(tooltips, $"Tooltip{tooltipNum}", "Terraria", out int index))
			{
				float percentOfLife = player.statLife / (float)player.statLifeMax2;
				float bonus = 0.2f;
				if (percentOfLife <= 0.5f)
				{
					bonus = 0f;
				}
				else
				{
					float multiplier = (float)Math.Pow(percentOfLife, 8);
					bonus *= multiplier;
					bonus *= 100f;
				}
				tooltips.Insert(index + 1, new(mod, "CurrentBonuses", $"[c/AAAAAA:Current Bonuses are: +{Math.Round(bonus, 2)}% & +{Math.Round((bonus * 0.5f), 2)}%]"));
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 5)
				.AddIngredient(ModContent.ItemType<InfernicFabric>(), 5)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.HandsOn)]
	public class PeakWarriorRing : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 1, 75, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.peakPerformanceRing = true;
			modPlayer.warriorRing = true;
			PeakPerformanceRing.PeakPerformanceUpdate(player, modPlayer.peakPerformanceRing);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			PeakPerformanceRing.PeakPerformanceTooltip(Main.LocalPlayer, tooltips, Mod, Main.LocalPlayer.GetModPlayer<RijamsModPlayer>().peakPerformanceRing, 10);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<PeakPerformanceRing>())
				.AddIngredient(ModContent.ItemType<WarriorRing>())
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}