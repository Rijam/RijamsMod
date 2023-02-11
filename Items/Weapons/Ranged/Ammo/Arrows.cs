using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ranged.Ammo
{
	public class BloodyArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("7% damage penalty per enemy pierced");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.UnholyArrow);
			Item.damage = 14;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.BloodyArrow>();   //The projectile shoot when your weapon using this ammo
		}

		public override void AddRecipes()
		{
			CreateRecipe(5)
				.AddIngredient(ItemID.WoodenArrow, 5)
				.AddIngredient(ModContent.ItemType<Materials.CrawlerChelicera>(), 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	public class SulfurArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Goes through tiles\nInflicts Sulfuric Acid");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodenArrow);
			Item.damage = 14;
			Item.knockBack = 3f;
			Item.value = 10;
			Item.width = 14;
			Item.height = 36;
			Item.rare = ItemRarityID.LightRed;
			Item.shootSpeed = 4f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.SulfurArrow>();   //The projectile shoot when your weapon using this ammo
		}

		public override void AddRecipes()
		{
			CreateRecipe(150)
				.AddIngredient(ItemID.WoodenArrow, 150)
				.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	public class EvilArrows : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Both an Unholy Arrow and Bloody Arrow");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.UnholyArrow);
			Item.damage = 13;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.EvilArrows>();   //The projectile shoot when your weapon using this ammo
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemID.UnholyArrow, 50)
				.AddIngredient(ModContent.ItemType<BloodyArrow>(), 50)
				.AddIngredient(ItemID.SoulofNight, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}