using RijamsMod.Items.Armor.RedSkyware;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.GodsentKing
{
	[AutoloadEquip(EquipType.Head)]
	public class GodsentKingMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<GodsentQueenMask>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 13;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.15f;
			player.GetDamage(DamageClass.Ranged) += 0.15f;
			player.GetModPlayer<RijamsModPlayer>().criticalHitAdditionalDamage += 0.25f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (body.type == ModContent.ItemType<GodsentKingBreastplate>() || body.type == ModContent.ItemType<GodsentQueenBreastplate>())
				&& (legs.type == ModContent.ItemType<GodsentKingPants>() || legs.type == ModContent.ItemType<GodsentQueenPants>());
		}

		public override void UpdateArmorSet(Player player)
		{
			SetBonus(player);
		}

		internal static void SetBonus(Player player)
		{
			player.GetCritChance(DamageClass.Generic) += 10f;
			player.GetDamage(DamageClass.SummonMeleeSpeed) += 0.1f;
			player.whipRangeMultiplier += 0.25f;

			//Main.NewText("player.dash " + player.dash + " player.dashDelay " + player.dashDelay + " player.dashTime " + player.dashTime + " player.timeSinceLastDashStarted " + player.timeSinceLastDashStarted);
			player.setBonus = Language.GetTextValue("Mods.RijamsMod.ArmorSetBonus.GodsentKing");
			if (player.timeSinceLastDashStarted == 1)
			{
				player.velocity.X *= 1.25f;
			}
			if (player.dashDelay > 10 && player.timeSinceLastDashStarted > 20)
			{
				player.dashDelay -= 10;
			}
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlinesForbidden = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FossilOre, 10)
				.AddIngredient(ItemID.GoldBar, 5)
				.AddIngredient(ItemID.SoulofLight)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class GodsentKingBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<GodsentQueenBreastplate>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 14;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
			player.GetAttackSpeed(DamageClass.Ranged) += 0.1f;
			player.GetModPlayer<RijamsModPlayer>().criticalHitAdditionalDamage += 0.5f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FossilOre, 10)
				.AddIngredient(ItemID.GoldBar, 5)
				.AddIngredient(ItemID.SoulofLight)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class GodsentKingPants : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<GodsentQueenPants>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 10;
		}

		public override void UpdateEquip(Player player)
		{
			//player.dashType = 5;
			player.moveSpeed += 0.5f;
			// Additional movement set in PostUpdateRunSpeeds in RijamsModPlayer
			player.GetModPlayer<RijamsModPlayer>().criticalHitAdditionalDamage += 0.25f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FossilOre, 10)
				.AddIngredient(ItemID.GoldBar, 5)
				.AddIngredient(ItemID.SoulofLight)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class GodsentQueenMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<GodsentKingMask>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.15f;
			player.GetDamage(DamageClass.Summon) += 0.15f;
			player.maxMinions++;
			player.GetModPlayer<RijamsModPlayer>().criticalHitAdditionalDamage += 0.25f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (body.type == ModContent.ItemType<GodsentKingBreastplate>() || body.type == ModContent.ItemType<GodsentQueenBreastplate>())
				&& (legs.type == ModContent.ItemType<GodsentKingPants>() || legs.type == ModContent.ItemType<GodsentQueenPants>());
		}

		public override void UpdateArmorSet(Player player)
		{
			GodsentKingMask.SetBonus(player);
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlinesForbidden = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FossilOre, 10)
				.AddIngredient(ItemID.PlatinumBar, 5)
				.AddIngredient(ItemID.SoulofLight)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class GodsentQueenBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<GodsentKingBreastplate>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Magic) += 0.1f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.1f;
			player.maxMinions++;
			player.GetModPlayer<RijamsModPlayer>().criticalHitAdditionalDamage += 0.5f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FossilOre, 10)
				.AddIngredient(ItemID.PlatinumBar, 5)
				.AddIngredient(ItemID.SoulofLight)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class GodsentQueenPants : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<GodsentKingPants>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 10;
		}

		public override void UpdateEquip(Player player)
		{
			player.dashType = 5; // Crystal Assassin Dash
			player.GetModPlayer<RijamsModPlayer>().criticalHitAdditionalDamage += 0.25f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FossilOre, 10)
				.AddIngredient(ItemID.PlatinumBar, 5)
				.AddIngredient(ItemID.SoulofLight)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}