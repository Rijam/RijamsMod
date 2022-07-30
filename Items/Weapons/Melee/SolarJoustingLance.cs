using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons.Melee
{
	// I made Example Jousting Lance so I'm going to use it!
	public class SolarJoustingLance : ModItem
	{
		public override void SetStaticDefaults()
		{
			// The (English) text shown below your weapon's name. "ItemTooltip.HallowJoustingLance" will automatically be translated to "Build momentum to increase attack power".
			Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.HallowJoustingLance") + "\nInflicts Daybroken\nCauses Solar Explosion on hit");
			GlobalItems.isJoustingLance.Add(Type);
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // The number of sacrifices that is required to research the item in Journey Mode.
		}

		public override void SetDefaults()
		{
			Item.DefaultToSpear(ModContent.ProjectileType<Projectiles.Melee.SolarJoustingLanceProj>(), 3.5f, 24); // A special method that sets a variety of item parameters that make the item act like a spear weapon.

			// The above Item.DefaultToSpear() does the following. Uncomment these if you don't want to use the above method or want to change something about it.
			// Item.useStyle = ItemUseStyleID.Shoot;
			// Item.useAnimation = 31;
			// Item.useTime = 31;
			// Item.shootSpeed = 3.5f;
			Item.width = 48;
			Item.height = 48;
			// Item.UseSound = SoundID.Item1;
			// Item.shoot = ModContent.ProjectileType<Projectiles.ExampleJoustingLance>();
			// Item.noMelee = true;
			// Item.noUseGraphic = true;
			Item.DamageType = DamageClass.MeleeNoSpeed; // We need to use MeleeNoSpeed here so that attack speed doesn't effect our held projectile.
			// Item.useAnimation = 24
			// Item.useTime = 24;

			Item.SetWeaponValues(200, 16f, 0); // A special method that sets the damage, knockback, and bonus critical strike chance.

			// The above Item.SetWeaponValues() does the following. Uncomment these if you don't want to use the above method.
			// Item.damage = 56;
			// Item.knockBack = 12f;
			// Item.crit = 2; // Even though this says 2, this is more like "bonus critical strike chance". All weapons have a base critical strike chance of 4.

			Item.SetShopValues(ItemRarityColor.StrongRed10, Item.buyPrice(0, 90)); // A special method that sets the rarity and value.

			// The above Item.SetShopValues() does the following. Uncomment these if you don't want to use the above method.
			// Item.rare = ItemRarityID.Red;
			// Item.value = Item.buyPrice(0, 6); // The value of the item. In this case, 6 gold. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.

			Item.channel = true; // Channel is important for our projectile.
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// If the player has increased melee speed, it will effect the shootSpeed of the Jousting Lance which will cause the projectile to spawn further away than it is supposed to.
			// This ensures that the velocity of the projectile is always the shootSpeed.
			float inverseMeleeSpeed = 1f / player.GetAttackSpeed(DamageClass.Melee);
			velocity *= inverseMeleeSpeed;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FragmentSolar, 9)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}

	// This will cause the Jousting Lance to become inactive if the player is hit with it out. Make sure to change the itemType to your item.
	public class SolarJoustingLancePlayer : ModPlayer
	{
		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{

			int itemType = ModContent.ItemType<SolarJoustingLance>(); // Change this to your item
			double damageTaken = Main.CalculateDamagePlayersTake((int)damage, Player.statDefense);

			if (!Player.immune && damageTaken >= 1.0 && Player.inventory[Player.selectedItem].type == itemType)
			{
				for (int j = 0; j < 1000; j++)
				{
					if (Main.projectile[j].active && Main.projectile[j].owner == Player.whoAmI && Main.projectile[j].type == ModContent.ProjectileType<Projectiles.Melee.SolarJoustingLanceProj>())
					{
						Main.projectile[j].active = false;
					}
				}
			}
		}
		/*public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			int itemType = ModContent.ItemType<SolarJoustingLance>();

			if (Player.inventory[Player.selectedItem].type == itemType && (target.value > 0f || (target.damage > 0 && !target.friendly)) && proj.type == ModContent.ProjectileType<Projectiles.SolarJoustingLanceProj>())
			{
				Vector2 center = target.Center;
				Projectile.NewProjectile(proj.GetSource_ItemUse(Player.HeldItem), center.X, center.Y, 0, 0, ProjectileID.SolarWhipSwordExplosion, damage / 2, knockback / 2, Player.whoAmI);
			}
		}*/
	}
}
