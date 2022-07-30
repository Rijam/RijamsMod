using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace RijamsMod.Items.Weapons.Melee
{
	// I made Example Jousting Lance so I'm going to use it!
	public class HorsemansJoustingLance : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Horseman's Jousting Lance");
			// The (English) text shown below your weapon's name. "ItemTooltip.HallowJoustingLance" will automatically be translated to "Build momentum to increase attack power".
			Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.HallowJoustingLance") + "\nOccasionally summons Pumpkin heads to attack your enemies");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Headless Horseman]", null, null });
			GlobalItems.isJoustingLance.Add(Type);
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // The number of sacrifices that is required to research the item in Journey Mode.
		}

		public override void SetDefaults()
		{
			Item.DefaultToSpear(ModContent.ProjectileType<Projectiles.Melee.HorsemansJoustingLanceProj>(), 3.5f, 24); // A special method that sets a variety of item parameters that make the item act like a spear weapon.

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

			Item.SetWeaponValues(150, 15f, 0); // A special method that sets the damage, knockback, and bonus critical strike chance.

			// The above Item.SetWeaponValues() does the following. Uncomment these if you don't want to use the above method.
			// Item.damage = 56;
			// Item.knockBack = 12f;
			// Item.crit = 2; // Even though this says 2, this is more like "bonus critical strike chance". All weapons have a base critical strike chance of 4.

			Item.SetShopValues(ItemRarityColor.Yellow8, Item.buyPrice(0, 75)); // A special method that sets the rarity and value.

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
	}

	// This will cause the Jousting Lance to become inactive if the player is hit with it out. Make sure to change the itemType to your item.
	public class HorsemansJoustingLancePlayer : ModPlayer
	{
		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{

			int itemType = ModContent.ItemType<HorsemansJoustingLance>(); // Change this to your item
			double damageTaken = Main.CalculateDamagePlayersTake((int)damage, Player.statDefense);

			if (!Player.immune && damageTaken >= 1.0 && Player.inventory[Player.selectedItem].type == itemType)
			{
				for (int j = 0; j < 1000; j++)
				{
					if (Main.projectile[j].active && Main.projectile[j].owner == Player.whoAmI && Main.projectile[j].type == ModContent.ProjectileType<Projectiles.Melee.HorsemansJoustingLanceProj>())
					{
						Main.projectile[j].active = false;
					}
				}
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			int itemType = ModContent.ItemType<HorsemansJoustingLance>();
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].active && Main.npc[i] == target && proj.type == ModContent.ProjectileType<Projectiles.Melee.HorsemansJoustingLanceProj>())
				{
					if (Player.inventory[Player.selectedItem].type == itemType && (target.value > 0f || (target.damage > 0 && !target.friendly)))
					{
						if (Main.rand.NextBool(5)) // Better than the normal The Horseman's Blade without this
						{
							Vector2 center = target.Center;
							int logicCheckScreenHeight = Main.LogicCheckScreenHeight;
							int logicCheckScreenWidth = Main.LogicCheckScreenWidth;
							int posX = Main.rand.Next(100, 300);
							int posY = Main.rand.Next(100, 300);
							posX = ((!Main.rand.NextBool(2)) ? (posX + (logicCheckScreenWidth / 2 - posX)) : (posX - (logicCheckScreenWidth / 2 + posX)));
							posY = ((!Main.rand.NextBool(2)) ? (posY + (logicCheckScreenHeight / 2 - posY)) : (posY - (logicCheckScreenHeight / 2 + posY)));
							posX += (int)proj.position.X;
							posY += (int)proj.position.Y;
							Vector2 vector = new(posX, posY);
							float velocityX = center.X - vector.X;
							float velocityY = center.Y - vector.Y;
							float velocitySqrt = (float)Math.Sqrt(velocityX * velocityX + velocityY * velocityY);
							velocitySqrt = 8f / velocitySqrt;
							velocityX *= velocitySqrt;
							velocityY *= velocitySqrt;
							Projectile.NewProjectile(proj.GetSource_ItemUse(Player.HeldItem), posX, posY, velocityX, velocityY, ProjectileID.FlamingJack, damage / 2, knockback / 2, Player.whoAmI, i);
						}
					}
				}
			}
		}
	}
}
