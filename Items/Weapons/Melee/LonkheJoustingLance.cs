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
	public class LonkheJoustingLance : ModItem
	{
		// In case anyone is curious, Lonkhe is a butchering of the Greek word for Lance. So then name is like "Lance Jousting Lance" lol.
		public override void SetStaticDefaults()
		{
			// The (English) text shown below your weapon's name. "ItemTooltip.HallowJoustingLance" will automatically be translated to "Build momentum to increase attack power".
			Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.HallowJoustingLance"));
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Hoplites]", "[c/474747:Or crafted]", null });
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
			Item.DefaultToSpear(ModContent.ProjectileType<Projectiles.Melee.LonkheJoustingLanceProj>(), 0.7f, 24);

			Item.width = 46;
			Item.height = 46;
			Item.DamageType = DamageClass.MeleeNoSpeed; // We need to use MeleeNoSpeed here so that attack speed doesn't effect our held projectile.

			Item.SetWeaponValues(25, 8f, 4); // A special method that sets the damage, knockback, and bonus critical strike chance.

			Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(0, 0, 20)); // A special method that sets the rarity and value.

			Item.channel = true; // Channel is important for our projectile.
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// If the player has increased melee speed, it will effect the shootSpeed of the Jousting Lance which will cause the projectile to spawn further away than it is supposed to.
			// This ensures that the velocity of the projectile is always the shootSpeed.
			float inverseMeleeSpeed = 1f / (player.GetAttackSpeed(DamageClass.Melee) * player.GetAttackSpeed(DamageClass.Generic));
			velocity *= inverseMeleeSpeed;
		}

		// This will allow our Jousting Lance to receive the same modifiers as melee weapons.
		public override bool MeleePrefix() => true;

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.MarbleBlock, 20) // Smooth Marble
				.AddIngredient(ItemID.GoldBar, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	// This will cause the Jousting Lance to become inactive if the player is hit with it out. Make sure to change the itemType to your item.
	public class LonkheJoustingLancePlayer : ModPlayer
	{
		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			int itemType = ModContent.ItemType<LonkheJoustingLance>(); // Change this to your item

			if (Player.inventory[Player.selectedItem].type == itemType)
			{
				for (int j = 0; j < Main.maxProjectiles; j++)
				{
					Projectile currentProj = Main.projectile[j];
					if (currentProj.active && currentProj.owner == Player.whoAmI && currentProj.type == ItemLoader.GetItem(itemType).Item.shoot)
					{
						currentProj.active = false;
					}
				}
			}
		}
	}
}
