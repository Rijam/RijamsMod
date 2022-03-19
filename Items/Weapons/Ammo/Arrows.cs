using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ammo
{
	public class BloodyArrow : ModItem
	{
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("7% damage penalty per enemy pierced");
		}
        public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.UnholyArrow);
			item.damage = 14;
			item.shoot = ModContent.ProjectileType<Projectiles.BloodyArrow>();   //The projectile shoot when your weapon using this ammo
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenArrow, 5);
			recipe.AddIngredient(ModContent.ItemType<Materials.CrawlerChelicera>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class SulfurArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Goes through tiles\nInflicts Sulfuric Acid");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.WoodenArrow);
			item.damage = 14;
			item.knockBack = 3f;
			item.value = 10;
			item.width = 14;
			item.height = 36;
			item.rare = ItemRarityID.LightRed;
			item.shootSpeed = 4f;
			item.shoot = ModContent.ProjectileType<Projectiles.SulfurArrow>();   //The projectile shoot when your weapon using this ammo
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenArrow, 150);
			recipe.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 150);
			recipe.AddRecipe();
		}
	}
}