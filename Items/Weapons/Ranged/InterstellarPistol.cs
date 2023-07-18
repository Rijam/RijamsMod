using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using RijamsMod.Projectiles.Ranged;
using Microsoft.CodeAnalysis;
using Terraria.Audio;
using Steamworks;
using log4net;
using Terraria.Chat;
using Terraria.Localization;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class InterstellarPistol : ModItem
	{
		//public static bool tileCollide;

		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Inherits many aspects of the bullets used");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating Skeletron]", "[c/474747:and Arms Dealer is present]" } );
		}

		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.crit = 6; // 10 crit
			Item.DamageType = DamageClass.Ranged;
			Item.width = 52;
			Item.height = 32;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Shoot; // 5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 4;
			Item.value = 100000;
			Item.rare = ItemRarityID.Green; // 2
			Item.UseSound = new(Mod.Name + "/Sounds/Item/InterstellarPistol");
			//Item.UseSound = SoundID.Item41 with { Volume = 0.5f };
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<InterstellarLaser>();
			Item.shootSpeed = 3f;
			Item.scale = 0.75f;
			Item.useAmmo = AmmoID.Bullet;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;

				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_MuzzleFlash").Value;
				flash.posOffsetXLeft = 10;
				flash.posOffsetXRight = -2;
				flash.posOffsetY = 0;
				flash.posOffsetYGravity = 4;
				flash.frameCount = 3;
				flash.frameRate = 4;
				flash.colorNoAlpha = new(100, 150, 255);
				flash.alpha = 0;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;
			}
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

			// Increase the velocity if the bullet has extraUpdates
			float velocityMultiplier = (1 - (2f / ammoExtraUpdates)) + 2;
			velocityMultiplier = Math.Clamp(velocityMultiplier, 1f, 3f);

			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

			Projectile laser = Projectile.NewProjectileDirect(source, position + new Vector2(0, -6), velocity * new Vector2(velocityMultiplier, velocityMultiplier), Item.shoot, damage, knockback, Main.myPlayer, tileCollide.ToInt());
			laser.penetrate = penetrate; // Seems to be 1 less than the normal bullet?
			laser.coldDamage = coldDamage;
			laser.tileCollide = tileCollide;
			if (laser.ModProjectile is InterstellarLaser modProjectile)
			{
				modProjectile.drawColor = new(100, 150, 255, 0);
				modProjectile.homing = homing;
				modProjectile.homingDetectionRange = 5;
				//Mod.Logger.DebugFormat("IntersetllarPistol: drawColor {0}, homing {1}, homingDetectionRange {2}", modProjectile.drawColor, modProjectile.homing, modProjectile.homingDetectionRange);
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, laser.whoAmI);
			}
			//Mod.Logger.DebugFormat("IntersetllarPistol: penetrate {0}, coldDamage {1}, tileCollide {2}", laser.penetrate, laser.coldDamage, laser.tileCollide);

			/*if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(SoundID.Item67 with { Pitch = 0.5f, Volume = 0.5f }, laser.position);
			}*/

			//bool? playedSound = RijamsMod.PlayNetworkSound(SoundID.Item67 with { Pitch = 0.5f, Volume = 0.5f }, laser.position);

			//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("playedSound " + playedSound.ToString()), Color.White);

			//NetMessage.SendData(MessageID.Sync, -1, -1, null, laser.whoAmI);

			return false;
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 2);
		}
	}
}
