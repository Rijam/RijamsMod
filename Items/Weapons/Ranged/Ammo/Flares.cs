using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Items.Materials;
using RijamsMod.Projectiles.Ranged;
using RijamsMod.Dusts;

namespace RijamsMod.Items.Weapons.Ranged.Ammo
{
	public class IchorFlare : ModItem
	{
		public override void SetDefaults()
		{
			Item.shootSpeed = 6f;
			Item.damage = 3;
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.ammo = AmmoID.Flare;
			Item.knockBack = 1.5f;
			Item.value = 7;
			Item.DamageType = DamageClass.Ranged;
			Item.shoot = ModContent.ProjectileType<IchorFlareProj>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(33)
				.AddRecipeGroup(RijamsModRecipes.Flares, 33)
				.AddIngredient(ItemID.Ichor)
				.Register();
		}
	}
	public class SulfurFlare : ModItem
	{
		public override void SetDefaults()
		{
			Item.shootSpeed = 6f;
			Item.damage = 3;
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.ammo = AmmoID.Flare;
			Item.knockBack = 1.5f;
			Item.value = 7;
			Item.DamageType = DamageClass.Ranged;
			Item.shoot = ModContent.ProjectileType<SulfurFlareProj>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(33)
				.AddRecipeGroup(RijamsModRecipes.Flares, 33)
				.AddIngredient(ModContent.ItemType<Sulfur>())
				.Register();
		}
	}

	public class SolarFlareFlare : ModItem
	{
		public override void SetDefaults()
		{
			Item.shootSpeed = 8f;
			Item.damage = 6;
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.ammo = AmmoID.Flare;
			Item.knockBack = 2f;
			Item.value = 14;
			Item.DamageType = DamageClass.Ranged;
			Item.shoot = ModContent.ProjectileType<SolarFlareFlareProj>();
			Item.rare = ItemRarityID.Blue;
		}
		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddRecipeGroup(RijamsModRecipes.Flares, 100)
				.AddIngredient(ItemID.FragmentSolar)
				.Register();
		}
	}

	public class GreekFireFlare : ModItem
	{
		public override void SetDefaults()
		{
			Item.shootSpeed = 7f;
			Item.damage = 4;
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.ammo = AmmoID.Flare;
			Item.knockBack = 1.5f;
			Item.value = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.shoot = ModContent.ProjectileType<GreekFireFlareProj>();
			Item.rare = ItemRarityID.Blue;
		}
		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddRecipeGroup(RijamsModRecipes.Flares, 100)
				.AddIngredient(ItemID.SpookyWood)
				.Register();
		}
	}

	public class GlobalFlare : GlobalItem
	{
		public override void SetDefaults(Item entity)
		{
			if (entity.type == ItemID.CursedFlare)
			{
				entity.damage = 3;
			}
			if (entity.type == ItemID.ShimmerFlare)
			{
				entity.damage = 2;
			}
			if (entity.type == ItemID.RainbowFlare)
			{
				entity.damage = 4;
			}
		}
		public override void HoldItem(Item item, Player player)
		{
			if (item.type == ItemID.FlareGun)
			{
				player.itemLocation.X = player.position.X + player.width * 0.5f - 2 * player.direction; // forward port from 1.4.5
				float x = player.position.X + (player.width / 2) + (38 * player.direction);
				if (player.direction == 1)
				{
					x -= 10f;
				}

				float y = player.MountedCenter.Y - 4f * player.gravDir;
				if (player.gravDir == -1f)
				{
					y -= 8f;
				}

				player.RotateRelativePoint(ref x, ref y);
				int ammoType = SearchInvAmmoFirst(player);

				// If it is a vanilla flare, don't do anything. (So the dust's don't get doubled up.)
				ammoType = ModdedGetDustTypeForFlareType(ammoType);

				if (ammoType > 0)
				{
					DrawFlareDust(ammoType, player, x, y);
				}
			}
			if (CustomItemIDSets.IsCombatFlareGun[item.type])
			{
				if (player.itemAnimation > 0) // Only display the flames while not firing.
				{
					return;
				}

				player.itemLocation.X = player.position.X + player.width * 0.5f - 2 * player.direction; // forward port from 1.4.5
				float x = player.position.X + (player.width / 2) + (38 * player.direction);
				if (player.direction == 1)
				{
					x -= 10f;
				}

				float y = player.MountedCenter.Y - 4f * player.gravDir;
				if (player.gravDir == -1f)
				{
					y -= 8f;
				}

				if (item.type == ModContent.ItemType<CombatFlarePistol>())
				{
					x += 2 * player.direction;
				}
				if (item.type == ModContent.ItemType<TripleBarrelFlarePistol>())
				{
					x += 5 * player.direction;
					y += 3f;
				}
				if (item.type == ModContent.ItemType<FlareSubmachineGun>())
				{
					x += 19 * player.direction;
					y -= 1f;
				}

				player.RotateRelativePoint(ref x, ref y);
				int ammoType = SearchInvAmmoFirst(player);

				ammoType = AllGetDustTypeForFlareType(ammoType);

				if (ammoType > 0)
				{
					DrawFlareDust(ammoType, player, x, y);
					if (item.type == ModContent.ItemType<TripleBarrelFlarePistol>())
					{
						y -= 5f;
						DrawFlareDust(ammoType, player, x, y);
					}
				}
			}
		}
		internal static int SearchInvAmmoFirst(Player player)
		{
			for (int i = 54; i < 58; i++) // Search the ammo slots first
			{
				if (player.inventory[i].stack > 0 && player.inventory[i].ammo == AmmoID.Flare)
				{
					return player.inventory[i].type;
				}
			}

			for (int j = 0; j < 54; j++) // Search the inventory next
			{
				if (player.inventory[j].stack > 0 && player.inventory[j].ammo == AmmoID.Flare)
				{
					return player.inventory[j].type;
				}
			}

			return 0;
		}

		internal static int ModdedGetDustTypeForFlareType(int ammoType)
		{
			if (ammoType == ModContent.ItemType<IchorFlare>())
			{
				return DustID.IchorTorch;
			}
			else if (ammoType == ModContent.ItemType<SulfurFlare>())
			{
				return ModContent.DustType<SulfurDust>();
			}
			else if (ammoType == ModContent.ItemType<SolarFlareFlare>())
			{
				return ModContent.DustType<SolarFlareFlareDust>();
			}
			else if (ammoType == ModContent.ItemType<GreekFireFlare>())
			{
				return DustID.YellowTorch;
			}
			return 0;
		}

		internal static int AllGetDustTypeForFlareType(int ammoType)
		{
			switch (ammoType)
			{
				case ItemID.Flare:
					return DustID.Flare;
				case ItemID.BlueFlare:
					return DustID.Flare_Blue;
				case ItemID.SpelunkerFlare:
					return DustID.IchorTorch;
				case ItemID.CursedFlare:
					return DustID.CursedTorch;
				case ItemID.RainbowFlare:
					return ModContent.DustType<RainbowFlareDust>(); // DustID.RainbowTorch;
				case ItemID.ShimmerFlare:
					return DustID.ShimmerTorch;
				default:
					break;
			}
			return ModdedGetDustTypeForFlareType(ammoType);
		}

		internal static void DrawFlareDust(int ammoType, Player player, float x, float y)
		{
			int flareDust = Dust.NewDust(new Vector2(x, y + player.gfxOffY), 6, 6, ammoType, 0f, 0f, 100, default, 1.6f);
			Main.dust[flareDust].noGravity = true;
			Main.dust[flareDust].velocity.Y -= 4f * player.gravDir;
		}
	}
}