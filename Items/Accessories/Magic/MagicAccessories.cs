using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Magic
{
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class DestroyerCuffs : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Lime; //7
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Magic) += 0.10f;
			player.GetCritChance(DamageClass.Magic) += 8;
			player.statManaMax2 += 20;
			player.manaMagnet = true;
			player.magicCuffs = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CelestialCuffs, 1)
				.AddIngredient(ItemID.DestroyerEmblem, 1)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.MagicCuffs, 1)
				.AddIngredient(ItemID.CelestialEmblem, 1)
				.AddIngredient(ItemID.EyeoftheGolem, 1)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Front, EquipType.Back, EquipType.HandsOn, EquipType.HandsOff)]
	public class CosmicSorcery : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Cyan; //9
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Magic) += 0.15f;
			player.GetCritChance(DamageClass.Magic) += 10;
			player.statManaMax2 += 20;
			player.manaRegenDelayBonus++;
			player.manaRegenBonus += 25;
			player.manaMagnet = true;
			player.magicCuffs = true;
			player.manaFlower = true;
			player.starCloakItem = Item;
			player.aggro -= 20 * 16; //20 tiles
			player.manaCost -= 0.08f;
			player.GetModPlayer<RijamsModPlayer>().manaSapperRing = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<DestroyerCuffs>(), 1)
				.AddIngredient(ItemID.ManaCloak, 1)
				.AddIngredient(ItemID.PutridScent, 1)
				.AddIngredient(ModContent.ItemType<Misc.ManaSapperRing>(), 1)
				.AddIngredient(ItemID.LunarBlockNebula, 5)
				.AddIngredient(ItemID.LunarBar, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.AddTile(TileID.LunarCraftingStation)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<DestroyerCuffs>(), 1)
				.AddIngredient(ItemID.StarCloak, 1)
				.AddIngredient(ItemID.ArcaneFlower, 1)
				.AddIngredient(ModContent.ItemType<Misc.ManaSapperRing>(), 1)
				.AddIngredient(ItemID.LunarBlockNebula, 5)
				.AddIngredient(ItemID.LunarBar, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}