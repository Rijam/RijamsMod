//using ExampleMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ammo
{
	public class BasicDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("This is a modded dart ammo.");
		}

		public override void SetDefaults()
		{
			item.damage = 7;
			item.ranged = true;
			item.width = 10;
			item.height = 22;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 0.5f;
			item.value = 10;
			item.rare = ItemRarityID.Green;
			item.shoot = ModContent.ProjectileType<Projectiles.BasicDart>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 16f;                  //The speed of the projectile
			item.ammo = AmmoID.Dart;              //The ammo class this ammo belongs to.
		}

		// Give each bullet consumed a 20% chance of granting the Wrath buff for 5 seconds
		/*public override void OnConsumeAmmo(Player player) {
			if (Main.rand.NextBool(5)) {
				player.AddBuff(BuffID.Wrath, 300);
			}
		}*/

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 1);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
	public class EndlessDartCase : BasicDart
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("Dart Monke");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ModContent.ItemType<BasicDart>());
			item.width = 30;
			item.height = 36;
			item.maxStack = 1;
			item.consumable = false;             //You need to set the item consumable so that the ammo would automatically consumed
			item.value = 5000;
			item.rare = ItemRarityID.Green;
			item.shoot = ModContent.ProjectileType<Projectiles.BasicDart>();   //The projectile shoot when your weapon using this ammo
			item.ammo = AmmoID.Dart;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BasicDart>(), 3996);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class ChlorophyteDart : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Grants Dryad's Blessing");
		}

		public override void SetDefaults()
		{
			item.damage = 16;
			item.ranged = true;
			item.width = 14;
			item.height = 28;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 4f;
			item.value = 1000;
			item.rare = ItemRarityID.Lime;
			item.shoot = ModContent.ProjectileType<Projectiles.ChlorophyteDart>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 16f;                  //The speed of the projectile
			item.ammo = AmmoID.Dart;              //The ammo class this ammo belongs to.
		}

		// Give each bullet consumed grants the Dryad's Blessing buff for 2 seconds
		public override void OnConsumeAmmo(Player player)
		{
			//if (Main.rand.NextBool(2)) {
			player.AddBuff(BuffID.DryadsWard, 120);
			//}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 1);
			recipe.AddIngredient(ModContent.ItemType<BasicDart>(), 100);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
	public class SpectreDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Splits into five darts\nWIP");
		}

		public override void SetDefaults()
		{
			item.damage = 15;
			item.ranged = true;
			item.width = 14;
			item.height = 30;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 4f;
			item.value = 1500;
			item.rare = ItemRarityID.Yellow;
			item.shoot = ModContent.ProjectileType<Projectiles.SpectreDart>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 16f;                  //The speed of the projectile
			item.ammo = AmmoID.Dart;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpectreBar, 1);
			recipe.AddIngredient(ModContent.ItemType<BasicDart>(), 100);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
	public class ShroomiteDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Very fast projectiles\nWIP");
		}

		public override void SetDefaults()
		{
			item.damage = 19;
			item.ranged = true;
			item.width = 14;
			item.height = 30;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 4f;
			item.value = 1500;
			item.rare = ItemRarityID.Yellow;
			item.shoot = ModContent.ProjectileType<Projectiles.ShroomiteDart>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 16f;                  //The speed of the projectile
			item.ammo = AmmoID.Dart;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShroomiteBar, 1);
			recipe.AddIngredient(ModContent.ItemType<BasicDart>(), 100);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
	public class LuminiteDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Homing");
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.ranged = true;
			item.width = 18;
			item.height = 36;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 4f;
			item.value = 1500;
			item.rare = ItemRarityID.Yellow;
			item.shoot = ModContent.ProjectileType<Projectiles.LuminiteDart>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 16f;                  //The speed of the projectile
			item.ammo = AmmoID.Dart;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 333);
			recipe.AddRecipe();
		}
	}
}
