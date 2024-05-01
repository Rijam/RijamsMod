using System;
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
			AmmoID.Sets.IsSpecialist[Type] = true;

			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.RocketLauncher].Add(Type, ProjectileID.RocketI);
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.GrenadeLauncher].Add(Type, ProjectileID.GrenadeI);
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.ProximityMineLauncher].Add(Type, ProjectileID.ProximityMineI);
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.SnowmanCannon].Add(Type, ProjectileID.RocketSnowmanI);
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.Celeb2].Add(Type, ProjectileID.Celeb2Rocket);
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
			//Item.shoot = ProjectileID.RocketI;   //The projectile shoot when your weapon using this ammo
			Item.ammo = AmmoID.Rocket;				//The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.RocketI, 3996)
				.AddTile(TileID.CrystalBall)
				.Register();
		}

		/*public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
		{
			if (weapon.type == ItemID.GrenadeLauncher || type == ProjectileID.GrenadeI)
			{
				type = ProjectileID.GrenadeI;
			}
			else if (weapon.type == ItemID.ProximityMineLauncher || type == ProjectileID.ProximityMineI)
			{
				type = ProjectileID.ProximityMineI;
			}
			else if (weapon.type == ItemID.SnowmanCannon || type == ProjectileID.RocketSnowmanI)
			{
				type = ProjectileID.RocketSnowmanI;
			}
			else if (weapon.type == ItemID.Celeb2)
			{
				type = ProjectileID.Celeb2Rocket;
			}
			else
			{
				type = ProjectileID.RocketI;
			}
		}*/
	}

	public class SulfurRocket : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.RocketI"));
			AmmoID.Sets.IsSpecialist[Type] = true;

			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.RocketLauncher].Add(Type, ModContent.ProjectileType<Projectiles.Ranged.SulfurRocket>());
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.GrenadeLauncher].Add(Type, ModContent.ProjectileType<Projectiles.Ranged.SulfurGrenade>());
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.ProximityMineLauncher].Add(Type, ModContent.ProjectileType<Projectiles.Ranged.SulfurMine>());
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.SnowmanCannon].Add(Type, ModContent.ProjectileType<Projectiles.Ranged.SulfurSnowmanRocket>());
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ItemID.Celeb2].Add(Type, ProjectileID.Celeb2Rocket);

			// This works by chance because of the load order.
			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[ModContent.ItemType<AGMMissileLauncher>()].Add(Type, ModContent.ProjectileType<Projectiles.Ranged.SulfurRocket>());
		}

		public override void SetDefaults()
		{
			//Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.SulfurRocket>();
			Item.damage = 50;
			Item.width = 20;
			Item.height = 14;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.ammo = AmmoID.Rocket;
			Item.knockBack = 4f;
			Item.value = Item.buyPrice(0, 0, 1, 0);
			Item.DamageType = DamageClass.Ranged;
			Item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			CreateRecipe(150)
				.AddIngredient(ItemID.RocketI, 150)
				.AddIngredient(ModContent.ItemType<Items.Materials.Sulfur>(), 1)
				.AddTile(TileID.Anvils)
				.Register();
		}

		/*public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
		{
			if (weapon.type == ItemID.GrenadeLauncher || type == ProjectileID.GrenadeI)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.SulfurGrenade>();
			}
			else if (weapon.type == ItemID.ProximityMineLauncher || type == ProjectileID.ProximityMineI)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.SulfurMine>();
			}
			else if (weapon.type == ItemID.SnowmanCannon || type == ProjectileID.RocketSnowmanI)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.SulfurSnowmanRocket>();
			}
			// else if (weapon.type == ItemID.FireworksLauncher) // Celebration
			// {
			//	type = ProjectileID.RocketFireworkRed + Main.rand.Next(0, 4);
			// }
			// else if (weapon.type == ItemID.ElectrosphereLauncher)
			// {
			//	type = ProjectileID.ElectrosphereMissile;
			// }
			else if (weapon.type == ItemID.Celeb2)
			{
				type = ProjectileID.Celeb2Rocket;
			}
			else
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.SulfurRocket>();
			}
		}*/
	}
}
