using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using RijamsMod.Items.Armor.Vanity.IntTrav;
using RijamsMod.Projectiles.Ranged;
using Terraria.Audio;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class InterstellarCarbine : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Interstellar Carbine");
			// Tooltip.SetDefault("50% not to consume ammo\nInherits many aspects of the bullets used");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating Moon Lord]", null });
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<InterstellarSMG>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.damage = 76;
			Item.crit = 6; // 10 crit
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
			Item.UseSound = new(Mod.Name + "/Sounds/Item/InterstellarRifle");
			//Item.UseSound = SoundID.Item41 with { Pitch = -0.2f, Volume = 0.5f };
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<InterstellarLaser>();
			Item.shootSpeed = 7f;
			Item.scale = 0.75f;
			Item.useAmmo = AmmoID.Bullet;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;

				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_MuzzleFlash").Value;
				flash.posOffsetXLeft = 13;
				flash.posOffsetXRight = -6;
				flash.posOffsetY = 6;
				flash.frameCount = 3;
				flash.frameRate = 4;
				flash.colorNoAlpha = new(0, 255, 50);
				flash.alpha = 0;
				flash.forceFirstFrame = false; // Since the useTime is so low, we'd never see the full animation before it gets replaced.
				flash.animationLoop = true; // So we instead keep the animation looping globally.
			}
		}


		// What if I wanted this gun to have a 50% chance not to consume ammo?
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= 0.5f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile bullet = Projectile.NewProjectileDirect(source, Vector2.Zero, Vector2.Zero, type, 0, 0, -1);
			int ammoExtraUpdates = bullet.extraUpdates;
			int penetrate = bullet.penetrate;
			bool coldDamage = bullet.coldDamage;
			bool tileCollide = bullet.tileCollide;
			bool homing = ProjectileID.Sets.CultistIsResistantTo[bullet.type];
			//int immunity = bullet.localNPCHitCooldown;
			bullet.Kill();

			// Increase the velocity if the bullet has extraUpdates
			float velocityMultiplier = (1 - (2f / ammoExtraUpdates)) + 2;
			velocityMultiplier = Math.Clamp(velocityMultiplier, 1f, 1f);

			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

			Projectile laser = Projectile.NewProjectileDirect(source, position + new Vector2(0, -4), velocity * new Vector2(velocityMultiplier, velocityMultiplier), Item.shoot, damage, knockback, Main.myPlayer);
			laser.penetrate = penetrate; // Seems to be 1 less than the normal bullet?
			laser.coldDamage = coldDamage;
			laser.tileCollide = tileCollide;
			//laser.idStaticNPCHitCooldown = immunity;
			if (laser.ModProjectile is InterstellarLaser modProjectile)
			{
				modProjectile.drawColor = new(0, 255, 50, 0);
				modProjectile.homing = homing;
				modProjectile.homingDetectionRange = 10;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, laser.whoAmI);
			}

			//SoundEngine.PlaySound(SoundID.Item67 with { Pitch = 0.7f, Volume = 0.5f }, laser.position);

			return false;
		}

		// What if I wanted it to work like Uzi, replacing regular bullets with High Velocity Bullets?
		// Uzi/Molten Fury style: Replace normal Bullets with Highvelocity
		/*public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet) // or ProjectileID.WoodenArrowFriendly
			{
				type = ProjectileID.ChlorophyteBullet; // or ProjectileID.FireArrow;
			}
		}*/

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
