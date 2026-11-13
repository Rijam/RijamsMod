using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.Festive
{
	[AutoloadEquip(EquipType.Head)]
	public class FestiveMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Festive Mask");
			// Tooltip.SetDefault("+1 Minion capacity\n+15% Summon damage");
			if (!Main.dedServ)
			{
				ArmorUseGlowHead.RegisterData(Item.headSlot, new ArmorHeadLegsOptions(Texture + "_Head_Glowmask", Color.White, GlowMaskEffects.LerpOnOff));
			}
			AddArmorSet<FestiveMask, FestiveChestplate, FestiveLeggings>
			(
				LocalizedText: Language.GetText($"Mods.{Mod.Name}.ArmorSetBonus.Festive"),
				PrimaryPart: ArmorSetBonus.PartType.None,
				Identifier: "RijamsMod/Festive",
				Effect: SetBonus_Festive
			);
		}

		public override void SetDefaults()
		{
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;//8
			Item.defense = 10;
		}
		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
			player.GetDamage(DamageClass.Summon) += 0.15f;
		}

		public static void SetBonus_Festive(Player player)
		{
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.35f;
			player.whipRangeMultiplier += 0.5f;
			player.GetDamage(DamageClass.SummonMeleeSpeed) += 0.25f;
			if (Math.Abs(player.velocity.X) > 2 || Math.Abs(player.velocity.Y) > 2)
			{
				float x2 = player.position.X - player.velocity.X / 10f;
				float y2 = player.position.Y - player.velocity.Y / 10f;
				int selectRand = Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.RedTorch);
				Dust.NewDust(new Vector2(x2, y2), player.width / 2, player.height, selectRand, 0f, 0f, 100, default, 0.5f);
			}
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.FestivePlating>(), 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class FestiveChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Festive Chestplate");
			// Tooltip.SetDefault("+1 Minion capacity\n+15% Summon damage");
		}

		public override void SetDefaults()
		{
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
			player.GetDamage(DamageClass.Summon) += 0.15f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.FestivePlating>(), 22)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class FestiveLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Festive Leggings");
			// Tooltip.SetDefault("+1 Minion capacity\n+20% Movement speed");
		}

		public override void SetDefaults()
		{
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 11;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
			player.moveSpeed += 0.2f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.FestivePlating>(), 18)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}