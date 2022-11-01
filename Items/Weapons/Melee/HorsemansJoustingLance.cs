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
			// A special method that sets a variety of item parameters that make the item act like a spear weapon.
			// To see everything DefaultToSpear() does, right click the method in Visual Studios and choose "Go To Definition" (or press F12).
			// The shoot speed will affect how far away the projectile spawns from the player's hand.
			// If you are using the custom AI in your projectile (and not aiStyle 19 and AIType = ProjectileID.JoustingLance), the standard value is 1f.
			// If you are using aiStyle 19 and AIType = ProjectileID.JoustingLance, then multiply the value by about 3.5f.
			Item.DefaultToSpear(ModContent.ProjectileType<Projectiles.Melee.HorsemansJoustingLanceProj>(), 1.3f, 24);
			Item.width = 48;
			Item.height = 48;
			Item.DamageType = DamageClass.MeleeNoSpeed; // We need to use MeleeNoSpeed here so that attack speed doesn't effect our held projectile.

			Item.SetWeaponValues(150, 15f, 0); // A special method that sets the damage, knockback, and bonus critical strike chance.

			Item.SetShopValues(ItemRarityColor.Yellow8, Item.buyPrice(0, 75)); // A special method that sets the rarity and value.

			Item.channel = true; // Channel is important for our projectile.

			// This will make sure our projectile completely disappears on hurt.
			// It's not enough just to stop the channel, as the lance can still deal damage while being stowed
			// If two players charge at each other, the first one to hit should cancel the other's lance
			Item.StopAnimationOnHurt = true;
		}

		// This will allow our Jousting Lance to receive the same modifiers as melee weapons.
		public override bool MeleePrefix() => true;
	}

	// Spawn projectiles
	public class HorsemansJoustingLancePlayer : ModPlayer
	{
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
