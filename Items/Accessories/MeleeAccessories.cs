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
			Tooltip.SetDefault("+12% Melee damage\n+12% Melee attack speed\n+100% Knockback\n+8 Defense\nMelee attacks inflict Daybroken\nGives the user master yoyo skills\nEnemies are more likely to target you");
		}

		public override void SetDefaults()
		{
			//item.color = Color.Yellow; //colors the inventory sprite
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Cyan; //9
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage += 0.12f;
			player.meleeSpeed += 0.12f;
			player.kbGlove = true;
			player.statDefense += 8;
			player.GetModPlayer<RijamsModPlayer>().daybreakStone = true;
			player.counterWeight = ProjectileID.BlackCounterweight + Main.rand.Next(6); 
			player.yoyoGlove = true;
			player.yoyoString = true;
			player.aggro += 25 * 16; //25 tiles
			//player.autoReuseGlove = true; //For 1.4
			//player.meleeScaleGlove = true; //For 1.4
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FireGauntlet, 1);
			recipe.AddIngredient(ItemID.YoyoBag, 1);
			recipe.AddIngredient(ItemID.FleshKnuckles, 1);
			recipe.AddIngredient(ModContent.ItemType<DaybreakStone>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}