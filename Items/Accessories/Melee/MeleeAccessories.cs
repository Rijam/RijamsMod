using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Melee
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class DaybreakGauntlet : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Daybreak Gauntlet");
			// Tooltip.SetDefault("+12% Melee damage\n+12% Melee attack speed\n+100% Knockback\n+8 Defense\nEnables auto swing for melee weapons\nIncreases the size of melee weapons\nMelee attacks inflict Daybroken\nGives the user master yoyo skills\nYoyos have infinite duration\nCounterweights are 1.5x bigger\nEnemies are more likely to target you");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Cyan; //9
			Item.value = Item.sellPrice(0, 25, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Melee) += 0.12f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
			player.kbGlove = true;
			player.statDefense += 8;
			player.GetModPlayer<RijamsModPlayer>().daybreakStone = true;
			player.GetModPlayer<RijamsModPlayer>().yoyoBackpack = true;
			player.aggro += 25 * 16; //25 tiles
			player.autoReuseGlove = true;
			player.meleeScaleGlove = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FireGauntlet, 1)
				.AddIngredient(ModContent.ItemType<YoyoBackpack>(), 1)
				.AddIngredient(ItemID.FleshKnuckles, 1)
				.AddIngredient(ModContent.ItemType<DaybreakStone>(), 1)
				.AddIngredient(ItemID.LunarBar, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Back)]
	public class YoyoBackpack : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Yoyo Backpack");
			// Tooltip.SetDefault("Gives the user master yoyo skills\nYoyos have infinite duration\nCounterweights are 1.5x bigger\nRainbow colored string!");
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 46;
			Item.rare = ItemRarityID.Yellow; //8
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().yoyoBackpack = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.YoyoBag, 1)
				.AddIngredient(ItemID.RainbowString, 1)
				.AddIngredient(ModContent.ItemType<Materials.InfernicFabric>(), 10)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}