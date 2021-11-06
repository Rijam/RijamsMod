using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons
{
	public class Plumage : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoot a bunch of feathers");
		}

		public override void SetDefaults()
		{
			item.damage = 40;
			item.noMelee = true;
			item.magic = true;
			item.channel = true; //Channel so that you can hold the weapon [Important]
			item.mana = 5;
			item.rare = ItemRarityID.Pink;
			item.width = 28;
			item.height = 30;
			item.useTime = 8;
			item.knockBack = 2;
			item.UseSound = SoundID.Item65;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shootSpeed = 14f;
			item.useAnimation = 8;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<FriendlyHarpyFeather>();
			item.value = Item.sellPrice(gold: 3);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddIngredient(ItemID.Feather, 5);
			recipe.AddIngredient(ItemID.GiantHarpyFeather, 2);
			recipe.AddIngredient(ItemID.SoulofFright, 3);
			recipe.AddTile(TileID.Bookcases);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
	}
	public class PlumageStorm : ModItem
	{
		//public override string Texture => "RijamsMod/Items/Weapons/Plumage";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots even more feathers");

		}

		public override void SetDefaults()
		{
			item.damage = 70;
			item.noMelee = true;
			item.magic = true;
			item.channel = true; //Channel so that you can hold the weapon [Important]
			item.mana = 5;
			item.rare = ItemRarityID.Cyan;
			item.width = 28;
			item.height = 30;
			item.useTime = 8;
			item.knockBack = 2;
			item.UseSound = SoundID.Item63;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shootSpeed = 16f;
			item.useAnimation = 8;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<FriendlyHarpyFeather>();
			item.value = Item.sellPrice(gold: 7);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "Plumage", 1);
			recipe.AddIngredient(mod, "SunEssence", 20);
			recipe.AddTile(TileID.Bookcases);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
		// What if I wanted multiple projectiles in a even spread? (Vampire Knives) 
		// Even Arc style: Multiple Projectile, Even Spread 
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float numberProjectiles = 5;// 5 shots
			float rotation = MathHelper.ToRadians(20);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
	public class TheShredder : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shreds your foes");

		}

		public override void SetDefaults()
		{
			item.damage = 110;
			item.crit = 4;
			item.noMelee = true;
			item.magic = true;
			item.channel = true; //Channel so that you can hold the weapon [Important]
			item.mana = 5;
			item.rare = ItemRarityID.Purple;
			item.width = 28;
			item.height = 30;
			item.useTime = 8;
			item.knockBack = 2;
			item.UseSound = SoundID.Item122;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shootSpeed = 16f;
			item.useAnimation = 8;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<FriendlyHarpyFeatherRed>();
			item.value = Item.sellPrice(gold: 13);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "PlumageStorm", 1);
			recipe.AddIngredient(mod, "GiantRedHarpyFeather", 3);
			recipe.AddIngredient(ItemID.FragmentNebula, 5);
			recipe.AddIngredient(ItemID.LunarBar, 2);
			recipe.AddTile(TileID.Bookcases);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
		// What if I wanted multiple projectiles in a even spread? (Vampire Knives) 
		// Even Arc style: Multiple Projectile, Even Spread 
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float numberProjectiles = 5;// 5 shots
			float rotation = MathHelper.ToRadians(20);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}

			// Here we manually spawn the 2nd projectile, manually specifying the projectile type that we wish to shoot.
			Projectile.NewProjectile(position.X, position.Y, (speedX *= 2.0f), (speedY *= 2.0f), ModContent.ProjectileType<FriendlyHarpyFeatherRazor>(), damage, knockBack, player.whoAmI);
			// By returning true, the vanilla behavior will take place, which will shoot the 1st projectile, the one determined by the ammo.
			return false;
		}
	}
}
