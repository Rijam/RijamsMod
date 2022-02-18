using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class DaybreakGauntlet : ModItem
	{
        //public override string Texture => "Terraria/Item_" + ItemID.FireGauntlet;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daybreak Gauntlet");
			Tooltip.SetDefault("+12% Melee damage\n+12% Melee attack speed\n+100% Knockback\n+8 Defense\nMelee attacks inflict Daybroken\nGives the user master yoyo skills\nYoyos have infinite duration\nCounterweights are 1.5x bigger\nEnemies are more likely to target you");
		}

		public override void SetDefaults()
		{
			//item.color = Color.Yellow; //colors the inventory sprite
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Cyan; //9
			item.value = Item.sellPrice(0, 25, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage += 0.12f;
			player.meleeSpeed += 0.12f;
			player.kbGlove = true;
			player.statDefense += 8;
			player.GetModPlayer<RijamsModPlayer>().daybreakStone = true;
			player.GetModPlayer<RijamsModPlayer>().yoyoBackpack = true;
			player.aggro += 25 * 16; //25 tiles
			//player.autoReuseGlove = true; //For 1.4
			//player.meleeScaleGlove = true; //For 1.4
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FireGauntlet, 1);
			recipe.AddIngredient(ModContent.ItemType<YoyoBackpack>(), 1);
			recipe.AddIngredient(ItemID.FleshKnuckles, 1);
			recipe.AddIngredient(ModContent.ItemType<DaybreakStone>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Back)]
	public class YoyoBackpack : ModItem
	{
		//public override string Texture => "Terraria/Item_" + ItemID.FireGauntlet;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yoyo Backpack");
			Tooltip.SetDefault("Gives the user master yoyo skills\nYoyos have infinite duration\nCounterweights are 1.5x bigger\nRainbow colored string!");
		}

		public override void SetDefaults()
		{
			//item.color = Color.Yellow; //colors the inventory sprite
			item.width = 40;
			item.height = 46;
			item.rare = ItemRarityID.Yellow; //8
			item.value = Item.sellPrice(0, 15, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().yoyoBackpack = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.YoyoBag, 1);
			recipe.AddIngredient(ItemID.RainbowString, 1);
			recipe.AddIngredient(ModContent.ItemType<Materials.InfernicFabric>(), 10);
			recipe.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}