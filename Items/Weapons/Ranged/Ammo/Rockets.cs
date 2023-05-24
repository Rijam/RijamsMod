using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
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

	public class SulfurRocket : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.RocketI"));
			AmmoID.Sets.IsSpecialist[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.shoot = ProjectileID.None;
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

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
		{
			if (weapon.type == ItemID.GrenadeLauncher || type == ProjectileID.GrenadeI)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.SulfurGrenade>();
			}
			else if (weapon.type == ItemID.ProximityMineLauncher || type == ProjectileID.ProximityMineI)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.SulfurMine>();
			}
			else if (weapon.type == ItemID.SnowmanCannon)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.SulfurSnowmanRocket>();
			}
			else if (weapon.type == ItemID.FireworksLauncher) // Celebration
			{
				type = ProjectileID.RocketFireworkRed + Main.rand.Next(0, 4);
			}
			else if (weapon.type == ItemID.ElectrosphereLauncher)
			{
				type = ProjectileID.ElectrosphereMissile;
			}
			else if (weapon.type == ItemID.Celeb2)
			{
				type = ProjectileID.Celeb2Rocket;
			}
			else
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.SulfurRocket>();
			}
		}
	}
}
