using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons
{
	public class InterstellarCarbine : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Interstellar Carbine");
			Tooltip.SetDefault("50% not to consume ammo\nConverts musket balls into Chlorophyte Bullets");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating Moon Lord]", null });
		}

		public override void SetDefaults()
		{
			Item.damage = 76;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 94;
			Item.height = 32;
			Item.useTime = 5;
			Item.useAnimation = 5;
			Item.useStyle = ItemUseStyleID.Shoot; //5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = 750000;
			Item.rare = ItemRarityID.Purple;//11
			Item.UseSound = SoundID.Item41;
			Item.autoReuse = true;
			Item.shoot = AmmoID.Bullet; //idk why but all the guns in the vanilla source have this
			Item.shootSpeed = 16f;
			Item.scale = 0.75f;
			Item.useAmmo = AmmoID.Bullet;
			if (!Main.dedServ)
            {
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -16;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 0;
            }
		}


		// What if I wanted this gun to have a 50% chance not to consume ammo?
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= .50f;
		}

		// What if I wanted it to work like Uzi, replacing regular bullets with High Velocity Bullets?
		// Uzi/Molten Fury style: Replace normal Bullets with Highvelocity
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet) // or ProjectileID.WoodenArrowFriendly
			{
				type = ProjectileID.ChlorophyteBullet; // or ProjectileID.FireArrow;
			}
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, 0);
		}

		// How can I make the shots appear out of the muzzle exactly?
		// Also, when I do this, how do I prevent shooting through tiles?
		/*public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}*/
	}
}
