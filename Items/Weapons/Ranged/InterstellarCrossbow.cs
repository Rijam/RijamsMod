using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;
using RijamsMod.Projectiles.Ranged;
using Terraria.Audio;
using System.Collections.Generic;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class InterstellarCrossbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating any Mechanical Boss]" });
			//ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<InterstellarPistol>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.crit = 6; // 10 crit
			Item.DamageType = DamageClass.Ranged;
			Item.width = 82;
			Item.height = 36;
			Item.useTime = 19;
			Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Shoot; //5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 4;
			Item.value = 300000;
			Item.rare = ItemRarityID.LightPurple;//6
			Item.UseSound = new SoundStyle(Mod.Name + "/Sounds/Item/InterstellarSMG") with { Pitch = -0.5f };
			//Item.UseSound = SoundID.Item41 with { Pitch = -0.1f, Volume = 0.5f };
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<InterstellarLaser>();
			Item.shootSpeed = 5f;
			Item.scale = 0.75f;
			Item.useAmmo = AmmoID.Arrow;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;

				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_MuzzleFlash").Value;
				flash.posOffsetXLeft = 18;
				flash.posOffsetXRight = -36;
				flash.posOffsetY = 8;
				flash.frameCount = 3;
				flash.frameRate = 4;
				flash.colorNoAlpha = new(150, 0, 255);
				flash.alpha = 0;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;
			}
		}


		// What if I wanted this gun to have a 20% chance not to consume ammo?
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= 0.2f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile arrow = new();
			arrow.SetDefaults(type);
			int ammoExtraUpdates = arrow.extraUpdates;
			int penetrate = arrow.penetrate;
			bool coldDamage = arrow.coldDamage;
			bool tileCollide = arrow.tileCollide;
			bool homing = ProjectileID.Sets.CultistIsResistantTo[arrow.type];

			// Increase the velocity if the bullet has extraUpdates
			float velocityMultiplier = (1 - (2f / ammoExtraUpdates)) + 2;
			velocityMultiplier = Math.Clamp(velocityMultiplier, 1f, 3f);

			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 35f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

			Projectile laser = Projectile.NewProjectileDirect(source, position + new Vector2(0, -4), velocity * new Vector2(velocityMultiplier, velocityMultiplier), Item.shoot, damage, knockback, Main.myPlayer, tileCollide.ToInt());
			laser.penetrate = penetrate; // Seems to be 1 less than the normal bullet?
			laser.coldDamage = coldDamage;
			laser.tileCollide = tileCollide;
			if (laser.ModProjectile is InterstellarLaser modProjectile)
			{
				modProjectile.drawColor = new(150, 0, 255, 0);
				modProjectile.homing = homing;
				modProjectile.homingDetectionRange = 10;
			}
			//SoundEngine.PlaySound(SoundID.Item67 with { Pitch = 0.6f, Volume = 0.5f }, laser.position);

			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, laser.whoAmI);
			}

			return false;
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 0);
		}
	}
}
