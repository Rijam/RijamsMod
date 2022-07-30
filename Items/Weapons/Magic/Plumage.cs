using RijamsMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons.Magic
{
	public class Plumage : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoot a bunch of feathers");
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true; //Channel so that you can hold the weapon [Important]
			Item.mana = 5;
			Item.rare = ItemRarityID.Pink;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 8;
			Item.knockBack = 2;
			Item.UseSound = SoundID.Item65;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 14f;
			Item.useAnimation = 8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<FriendlyHarpyFeather>();
			Item.value = Item.sellPrice(gold: 3);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SpellTome, 1)
				.AddIngredient(ItemID.Feather, 5)
				.AddIngredient(ItemID.GiantHarpyFeather, 2)
				.AddIngredient(ItemID.SoulofFright, 3)
				.AddTile(TileID.Bookcases)
				.Register();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
	}
	public class PlumageStorm : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots even more feathers");

		}

		public override void SetDefaults()
		{
			Item.damage = 70;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true; //Channel so that you can hold the weapon [Important]
			Item.mana = 5;
			Item.rare = ItemRarityID.Cyan;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 8;
			Item.knockBack = 2;
			Item.UseSound = SoundID.Item63;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 16f;
			Item.useAnimation = 8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<FriendlyHarpyFeather>();
			Item.value = Item.sellPrice(gold: 7);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Plumage>(), 1)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 20)
				.AddTile(TileID.Bookcases)
				.Register();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
		// What if I wanted multiple projectiles in a even spread? (Vampire Knives) 
		// Even Arc style: Multiple Projectile, Even Spread 
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float numberProjectiles = 5;// 5 shots
			float rotation = MathHelper.ToRadians(20);
			position += Vector2.Normalize(velocity) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
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
			Item.damage = 110;
			Item.crit = 4;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true; //Channel so that you can hold the weapon [Important]
			Item.mana = 5;
			Item.rare = ItemRarityID.Purple;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 8;
			Item.knockBack = 2;
			Item.UseSound = SoundID.Item122;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 16f;
			Item.useAnimation = 8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<FriendlyHarpyFeatherRed>();
			Item.value = Item.sellPrice(gold: 13);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<PlumageStorm>(), 1)
				.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 3)
				.AddIngredient(ItemID.FragmentNebula, 5)
				.AddIngredient(ItemID.LunarBar, 2)
				.AddTile(TileID.Bookcases)
				.Register();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
		// What if I wanted multiple projectiles in a even spread? (Vampire Knives) 
		// Even Arc style: Multiple Projectile, Even Spread 
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float numberProjectiles = 5;// 5 shots
			float rotation = MathHelper.ToRadians(20);
			position += Vector2.Normalize(velocity) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}

			// Here we manually spawn the 2nd projectile, manually specifying the projectile type that we wish to shoot.
			Projectile.NewProjectile(source, position.X, position.Y, (velocity.X *= 2.0f), (velocity.Y *= 2.0f), ModContent.ProjectileType<FriendlyHarpyFeatherRazor>(), damage, knockback, player.whoAmI);
			// By returning true, the vanilla behavior will take place, which will shoot the 1st projectile, the one determined by the ammo.
			return false;
		}
	}
}
