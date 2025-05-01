using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Enums;
using RijamsMod.Items.Armor.Skyware;
using Terraria.ID;
using RijamsMod.Projectiles.Misc;
using System;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons.Melee.JoustingLances
{
	// I made Example Jousting Lance so I'm going to use it!
	public class RedSkywareJoustingLance : ModItem
	{
		public override void SetStaticDefaults()
		{
			CustomItemIDSets.IsJoustingLance[Type] = Item.shoot;
		}

		public override void SetDefaults()
		{
			// A special method that sets a variety of item parameters that make the item act like a spear weapon.
			// To see everything DefaultToSpear() does, right click the method in Visual Studios and choose "Go To Definition" (or press F12).
			// The shoot speed will affect how far away the projectile spawns from the player's hand.
			// If you are using the custom AI in your projectile (and not aiStyle 19 and AIType = ProjectileID.JoustingLance), the standard value is 1f.
			// If you are using aiStyle 19 and AIType = ProjectileID.JoustingLance, then multiply the value by about 3.5f.
			Item.DefaultToSpear(ModContent.ProjectileType<Projectiles.Melee.JoustingLances.RedSkywareJoustingLanceProj>(), 1.34f, 30);
			Item.width = 48;
			Item.height = 48;
			Item.DamageType = DamageClass.MeleeNoSpeed; // We need to use MeleeNoSpeed here so that attack speed doesn't effect our held projectile.

			Item.SetWeaponValues(140, 15f, 4); // A special method that sets the damage, knockback, and bonus critical strike chance.

			Item.SetShopValues(ItemRarityColor.Yellow8, Item.sellPrice(0, 10)); // A special method that sets the rarity and value.

			Item.channel = true; // Channel is important for our projectile.

			// This will make sure our projectile completely disappears on hurt.
			// It's not enough just to stop the channel, as the lance can still deal damage while being stowed
			// If two players charge at each other, the first one to hit should cancel the other's lance
			Item.StopAnimationOnHurt = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float movementInLanceDirection = Vector2.Dot(velocity.SafeNormalize(Vector2.UnitX * player.direction), player.velocity.SafeNormalize(Vector2.UnitX * player.direction));
			movementInLanceDirection = Math.Clamp(movementInLanceDirection, 0, 1);

			knockback *= player.velocity.Length() * movementInLanceDirection / 7f;
			float newDamage = damage * (0.1f + (player.velocity.Length() * movementInLanceDirection) / 7f * 0.9f);
			newDamage = Math.Clamp(newDamage, 1f, Item.damage); // It does a minimum of 1 damage.

			velocity.Normalize();
			velocity *= 5f + (player.velocity.Length() * movementInLanceDirection);
			velocity.X = Math.Clamp(velocity.X, -16, 16);
			velocity.Y = Math.Clamp(velocity.Y, -16, 16);

			Projectile feather = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<RedSkywareArmorHarpyFeather>(), (int)newDamage, knockback);
			feather.DamageType = DamageClass.Melee;

			return true;
		}

		// This will allow our Jousting Lance to receive the same modifiers as melee weapons.
		public override bool MeleePrefix() => true;

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SkywareJoustingLance>())
				.AddIngredient(ItemID.ChlorophyteBar, 2)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 5)
				.AddTile(TileID.SkyMill)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 10)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 15)
				.AddTile(TileID.SkyMill)
				.Register();
		}
	}
}