using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ammo
{
	public class SulfurBullet : ModItem
	{
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Goes through tiles\nInflicts Sulfuric Acid");
		}
        public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MusketBall);
			item.damage = 11;
			item.knockBack = 3f;
			item.value = 6;
			item.width = 8;
			item.height = 12;
			item.rare = ItemRarityID.LightRed;
			item.shootSpeed = 3f;
			item.shoot = ModContent.ProjectileType<Projectiles.SulfurBullet>();   //The projectile shoot when your weapon using this ammo
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MusketBall, 150);
			recipe.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 150);
			recipe.AddRecipe();
		}
	}
}