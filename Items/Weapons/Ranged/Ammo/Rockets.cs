using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ranged.Ammo
{
	public class EndlessRocketBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.RocketI"));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.RocketI);
			Item.width = 54;
			Item.height = 32;
			Item.maxStack = 1;
			Item.consumable = false;			 //You need to set the item consumable so that the ammo would automatically consumed
			Item.value = 100000;
			Item.rare = ItemRarityID.Green;
			//item.shoot = ProjectileID.RocketI;   //The projectile shoot when your weapon using this ammo
			//item.ammo = AmmoID.Rocket;			  //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.RocketI, 3996)
				.AddTile(TileID.CrystalBall)
				.Register();
		}
	}
}
