using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using RijamsMod.Projectiles.Misc;
using System;

namespace RijamsMod.Items.Weapons.Melee.JoustingLances
{
	// I made Example Jousting Lance so I'm going to use it!
	public class SkywareJoustingLance : ModItem
	{
		// In case anyone is curious, Lonkhe is a butchering of the Greek word for Lance. So then name is like "Lance Jousting Lance" lol.
		public override void SetStaticDefaults()
		{
			// The (English) text shown below your weapon's name. "ItemTooltip.HallowJoustingLance" will automatically be translated to "Build momentum to increase attack power".
			// Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.HallowJoustingLance"));
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Dropped by Hoplites]", "[c/474747:Or crafted]"]);
			CustomItemIDSets.IsJoustingLance[Type] = true;
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // The number of sacrifices that is required to research the item in Journey Mode.
		}

		public override void SetDefaults()
		{
			// A special method that sets a variety of item parameters that make the item act like a spear weapon.
			// To see everything DefaultToSpear() does, right click the method in Visual Studios and choose "Go To Definition" (or press F12).
			// The shoot speed will affect how far away the projectile spawns from the player's hand.
			// If you are using the custom AI in your projectile (and not aiStyle 19 and AIType = ProjectileID.JoustingLance), the standard value is 1f.
			// If you are using aiStyle 19 and AIType = ProjectileID.JoustingLance, then multiply the value by about 3.5f.
			Item.DefaultToSpear(ModContent.ProjectileType<Projectiles.Melee.JoustingLances.SkywareJoustingLanceProj>(), 0.775f, 30);

			Item.width = 46;
			Item.height = 46;
			Item.DamageType = DamageClass.MeleeNoSpeed; // We need to use MeleeNoSpeed here so that attack speed doesn't effect our held projectile.

			Item.SetWeaponValues(32, 8f, 4); // A special method that sets the damage, knockback, and bonus critical strike chance.

			Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice(0, 0, 20)); // A special method that sets the rarity and value.

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
			velocity *= 10f + (player.velocity.Length() * movementInLanceDirection);
			velocity.X = Math.Clamp(velocity.X, -16, 16);
			velocity.Y = Math.Clamp(velocity.Y, -16, 16);

			Projectile feather = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<SkywareArmorHarpyFeather>(), (int)newDamage, knockback);
			feather.DamageType = DamageClass.Melee;

			return true;
		}

		// This will allow our Jousting Lance to receive the same modifiers as melee weapons.
		public override bool MeleePrefix() => true;

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 10)
				.AddRecipeGroup(RijamsModRecipes.GoldBars, 5)
				.AddRecipeGroup(RijamsModRecipes.EvilBars, 5)
				.AddTile(TileID.SkyMill)
				.Register();
		}
	}
}
