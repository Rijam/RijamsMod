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
}