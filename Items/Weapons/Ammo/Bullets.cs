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
			Item.CloneDefaults(ItemID.MusketBall);
			Item.damage = 11;
			Item.knockBack = 3f;
			Item.value = 6;
			Item.width = 8;
			Item.height = 12;
			Item.rare = ItemRarityID.LightRed;
			Item.shootSpeed = 3f;
			Item.shoot = ModContent.ProjectileType<Projectiles.SulfurBullet>();   //The projectile shoot when your weapon using this ammo
		}

		public override void AddRecipes()
		{
			CreateRecipe(150)
				.AddIngredient(ItemID.MusketBall, 150)
				.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}