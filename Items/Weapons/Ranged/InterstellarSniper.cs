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
using Steamworks;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class InterstellarSniper : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating Lunatic Cultist]" });
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<InterstellarSMG>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.damage = 220;
			Item.crit = 26; // 30 crit
			Item.DamageType = DamageClass.Ranged;
			Item.width = 100;
			Item.height = 38;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot; //5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 10;
			Item.value = 600000;
			Item.rare = ItemRarityID.Cyan;//9
			Item.UseSound = new SoundStyle(Mod.Name + "/Sounds/Item/InterstellarSniper") with { Volume = 0.75f };
			//Item.UseSound = SoundID.Item41 with { Pitch = -0.2f, Volume = 0.5f };
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<InterstellarLaser>();
			Item.shootSpeed = 10f;
			Item.scale = 0.75f;
			Item.useAmmo = AmmoID.Bullet;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;

				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_MuzzleFlash").Value;
				flash.posOffsetXLeft = 22;
				flash.posOffsetXRight = -8;
				flash.posOffsetY = 10;
				flash.frameCount = 3;
				flash.frameRate = 4;
				flash.colorNoAlpha = new(255, 50, 50);
				flash.alpha = 0;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;
			}
		}


		// What if I wanted this gun to have a 25% chance not to consume ammo?
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= 0.25f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile bullet = new();
			bullet.SetDefaults(type);
			int ammoExtraUpdates = bullet.extraUpdates;
			int penetrate = bullet.penetrate;
			bool coldDamage = bullet.coldDamage;
			bool tileCollide = bullet.tileCollide;
			bool homing = ProjectileID.Sets.CultistIsResistantTo[bullet.type];
			//int immunity = bullet.localNPCHitCooldown;

			// Increase the velocity if the bullet has extraUpdates
			float velocityMultiplier = (1 - (2f / ammoExtraUpdates)) + 2;
			velocityMultiplier = Math.Clamp(velocityMultiplier, 1f, 3f);

			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

			Projectile laser = Projectile.NewProjectileDirect(source, position + new Vector2(0, -4), velocity * new Vector2(velocityMultiplier, velocityMultiplier), Item.shoot, damage, knockback, Main.myPlayer, tileCollide.ToInt());
			laser.penetrate = penetrate; // Seems to be 1 less than the normal bullet?
			laser.coldDamage = coldDamage;
			laser.tileCollide = tileCollide;
			laser.extraUpdates *= 2; // More reliable hitting
			laser.velocity *= 0.5f;
			//laser.idStaticNPCHitCooldown = immunity;
			if (laser.ModProjectile is InterstellarLaser modProjectile)
			{
				modProjectile.drawColor = new(255, 50, 50, 0);
				modProjectile.homing = homing;
				modProjectile.homingDetectionRange = 15;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, laser.whoAmI);
			}

			//SoundEngine.PlaySound(SoundID.Item67 with { Pitch = 0.7f, Volume = 0.5f }, laser.position);

			return false;
		}

		public override void HoldItem(Player player)
		{
			player.scope = true;
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, 0);
		}
	}
}
