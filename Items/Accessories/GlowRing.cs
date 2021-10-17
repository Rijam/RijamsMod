using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class SmallGlowRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Small Glow Ring");
			Tooltip.SetDefault("Emits a small amount of light");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 24;
			item.rare = ItemRarityID.White;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Lighting.AddLight(player.Center, Color.Yellow.ToVector3() * 0.35f);
		}
		public override Color? GetAlpha(Color newColor)
		{
			return new Color(255, 255, 255, 255);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddRecipeGroup("RijamsMod:CopperBars", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.HandsOn)]
	public class LargeGlowRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Large Glow Ring");
			Tooltip.SetDefault("Emits a large amount of light");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 24;
			item.rare = ItemRarityID.Orange; //3
			item.value = Item.sellPrice(0, 0, 75, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Lighting.AddLight(player.Center, 1.0f, 0.90f, 0.40f);
		}
		public override Color? GetAlpha(Color newColor)
		{
			return new Color(255, 255, 255, 255);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SmallGlowRing", 1);
			recipe.AddIngredient(ItemID.Meteorite, 1);
			recipe.AddIngredient(ItemID.Hellstone, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}