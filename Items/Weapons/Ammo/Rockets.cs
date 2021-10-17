//using ExampleMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ammo
{
	public class EndlessRocketBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("Picked up a box of rockets");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.RocketI);
			item.width = 54;
			item.height = 32;
			item.maxStack = 1;
			item.consumable = false;             //You need to set the item consumable so that the ammo would automatically consumed
			item.value = 100000;
			item.rare = ItemRarityID.Green;
			//item.shoot = ProjectileID.RocketI;   //The projectile shoot when your weapon using this ammo
			item.ammo = AmmoID.Rocket;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RocketI, 3996);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
