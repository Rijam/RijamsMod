using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ranged.Ammo
{
	public class BasicDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("This is a modded dart ammo.");
		}

		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 10;
			Item.height = 22;
			Item.maxStack = 999;
			Item.consumable = true;			 //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 0.5f;
			Item.value = 10;
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.BasicDart>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 16f;				  //The speed of the projectile
			Item.ammo = AmmoID.Dart;			  //The ammo class this ammo belongs to.
		}

		// Give each bullet consumed a 20% chance of granting the Wrath buff for 5 seconds
		/*public override void OnConsumeAmmo(Player player) {
			if (Main.rand.NextBool(5)) {
				player.AddBuff(BuffID.Wrath, 300);
			}
		}*/

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddRecipeGroup("IronBar", 1)
				.AddTile(TileID.WorkBenches)
				.Register();
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
			Item.CloneDefaults(ModContent.ItemType<BasicDart>());
			Item.width = 30;
			Item.height = 36;
			Item.maxStack = 1;
			Item.consumable = false;			 //You need to set the item consumable so that the ammo would automatically consumed
			Item.value = 5000;
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.BasicDart>();   //The projectile shoot when your weapon using this ammo
			Item.ammo = AmmoID.Dart;			  //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<BasicDart>(), 3996)
				.AddTile(TileID.CrystalBall)
				.Register();
		}
	}

	public class SulfurDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Goes through tiles\nInflicts Sulfuric Acid");
		}

		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 28;
			Item.maxStack = 999;
			Item.consumable = true;			 //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 2f;
			Item.value = 8;
			Item.rare = ItemRarityID.LightRed;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.SulfurDart>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 3.5f;				  //The speed of the projectile
			Item.ammo = AmmoID.Dart;			  //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 1)
				.Register();
		}
	}

	public class ChlorophyteDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Grants Dryad's Blessing if consumed\n10% damage penalty per enemy pierced");
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 28;
			Item.maxStack = 999;
			Item.consumable = true;			 //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 4f;
			Item.value = 1000;
			Item.rare = ItemRarityID.Lime;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ChlorophyteDart>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 8f;				  //The speed of the projectile
			Item.ammo = AmmoID.Dart;			  //The ammo class this ammo belongs to.
		}

		// Give each bullet consumed grants the Dryad's Blessing buff for 2 seconds
		public override void OnConsumedAsAmmo(Item weapon, Player player)
		{
			//if (Main.rand.NextBool(2)) {
			player.AddBuff(BuffID.DryadsWard, 120);
			//}
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemID.ChlorophyteBar, 1)
				.AddIngredient(ModContent.ItemType<BasicDart>(), 100)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	public class SpectreDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Splits into five darts\n30% damage penalty per enemy pierced");
		}

		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 30;
			Item.maxStack = 999;
			Item.consumable = true;			 //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 4f;
			Item.value = 1500;
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.SpectreDart>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 10f;				  //The speed of the projectile
			Item.ammo = AmmoID.Dart;			  //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemID.SpectreBar, 1)
				.AddIngredient(ModContent.ItemType<BasicDart>(), 100)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	public class ShroomiteDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Moves extremely fast\n10% damage penalty per enemy pierced");
		}

		public override void SetDefaults()
		{
			Item.damage = 19;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 30;
			Item.maxStack = 999;
			Item.consumable = true;			 //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 4f;
			Item.value = 1500;
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ShroomiteDart>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 16f;				  //The speed of the projectile
			Item.ammo = AmmoID.Dart;			  //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemID.ShroomiteBar, 1)
				.AddIngredient(ModContent.ItemType<BasicDart>(), 100)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	public class LuminiteDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Homing\n25% damage penalty per enemy pierced");
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 18;
			Item.height = 36;
			Item.maxStack = 999;
			Item.consumable = true;			 //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 4f;
			Item.value = 1500;
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.LuminiteDart>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 16f;				  //The speed of the projectile
			Item.ammo = AmmoID.Dart;			  //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(333)
				.AddIngredient(ItemID.LunarBar, 1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
