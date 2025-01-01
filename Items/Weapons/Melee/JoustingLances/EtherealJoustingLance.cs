using Terraria;
using Terraria.ModLoader;
using Terraria.Enums;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace RijamsMod.Items.Weapons.Melee.JoustingLances
{
	public class EtherealJoustingLance : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by Empress of Light]" });
			GlobalItems.isJoustingLance.Add(Type, Item.shoot);
		}

		public override void SetDefaults()
		{
			// A special method that sets a variety of item parameters that make the item act like a spear weapon.
			// To see everything DefaultToSpear() does, right click the method in Visual Studios and choose "Go To Definition" (or press F12).
			// The shoot speed will affect how far away the projectile spawns from the player's hand.
			// If you are using the custom AI in your projectile (and not aiStyle 19 and AIType = ProjectileID.JoustingLance), the standard value is 1f.
			// If you are using aiStyle 19 and AIType = ProjectileID.JoustingLance, then multiply the value by about 3.5f.
			Item.DefaultToSpear(ModContent.ProjectileType<Projectiles.Melee.JoustingLances.EtherealJoustingLanceProj>(), 1.4f, 24);
			Item.width = 48;
			Item.height = 48;
			Item.DamageType = DamageClass.MeleeNoSpeed; // We need to use MeleeNoSpeed here so that attack speed doesn't effect our held projectile.

			Item.SetWeaponValues(160, 15f, 0); // A special method that sets the damage, knockback, and bonus critical strike chance.

			Item.SetShopValues(ItemRarityColor.Yellow8, Item.sellPrice(0, 10)); // A special method that sets the rarity and value.

			Item.channel = true; // Channel is important for our projectile.

			// This will make sure our projectile completely disappears on hurt.
			// It's not enough just to stop the channel, as the lance can still deal damage while being stowed
			// If two players charge at each other, the first one to hit should cancel the other's lance
			Item.StopAnimationOnHurt = true;
		}

		// This will allow our Jousting Lance to receive the same modifiers as melee weapons.
		public override bool MeleePrefix() => true;

		public override Color? GetAlpha(Color lightColor)
		{
			Color drawColor = Color.White;
			Color minColor = new(127, 127, 127, 0);
			drawColor.R = Math.Max(lightColor.R, minColor.R);
			drawColor.G = Math.Max(lightColor.G, minColor.G);
			drawColor.B = Math.Max(lightColor.B, minColor.B);
			drawColor.A = lightColor.A;
			return drawColor;
		}
	}
}