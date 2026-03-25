using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;
using RijamsMod.Projectiles.Ranged;
using RijamsMod.Items.Weapons.Ranged.Ammo;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class CombatFlarePistol : ModItem
	{
		public static readonly Dictionary<int, int> ExplodingFlareTypeMatching = new()
		{
			{ ItemID.Flare, ModContent.ProjectileType<ExplodingFlareProj>() },
			{ ItemID.BlueFlare, ModContent.ProjectileType<ExplodingBlueFlareProj>() },
			{ ItemID.SpelunkerFlare, ModContent.ProjectileType<ExplodingSpelunkerFlareProj>() },
			{ ItemID.CursedFlare, ModContent.ProjectileType<ExplodingCursedFlareProj>() },
			{ ItemID.RainbowFlare, ModContent.ProjectileType<ExplodingRainbowFlareProj>() },
			{ ItemID.ShimmerFlare, ModContent.ProjectileType<ExplodingShimmerFlareProj>() },
			{ ModContent.ItemType<IchorFlare>(), ModContent.ProjectileType<ExplodingIchorFlareProj>() },
			{ ModContent.ItemType<SulfurFlare>(), ModContent.ProjectileType<ExplodingSulfurFlareProj>() },
			{ ModContent.ItemType<GreekFireFlare>(), ModContent.ProjectileType<ExplodingGreekFireFlareProj>() },
			{ ModContent.ItemType<SolarFlareFlare>(), ModContent.ProjectileType<ExplodingSolarFlareFlareProj>() }
		};

		public override void SetStaticDefaults()
		{
			ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;

			AmmoID.Sets.SpecificLauncherAmmoProjectileFallback[Type] = ItemID.FlareGun;

			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches.Add(Type, ExplodingFlareTypeMatching);

			CustomItemIDSets.IsCombatFlareGun[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 48;
			Item.height = 28;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot; // 5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 1;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green; // 2
			Item.UseSound = new(Mod.Name + "/Sounds/Item/FlarePistol");
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<InterstellarLaser>();
			Item.shootSpeed = 6f;
			Item.scale = 0.75f;
			Item.useAmmo = AmmoID.Flare;
			Item.holdStyle = ItemHoldStyleID.HoldHeavy;
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 10;
				flash.posOffsetXRight = -2;
				flash.posOffsetY = 0;
				flash.posOffsetYGravity = 4;
				flash.frameCount = 2;
				flash.frameRate = 4;
				flash.colorNoAlpha = new(255, 150, 0);
				flash.alpha = 0;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 15f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/FlareFly") { MaxInstances = 5, Volume = 0.75f }, position);
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, default, default, Main.rand.NextFloat(0.75f, 1.5f));
			return false;
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 2);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FlareGun)
				.AddIngredient(ItemID.IllegalGunParts)
				.AddRecipeGroup(RijamsModRecipes.EvilBars, 3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class TripleBarrelFlarePistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;

			AmmoID.Sets.SpecificLauncherAmmoProjectileFallback[Type] = ItemID.FlareGun;

			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches.Add(Type, CombatFlarePistol.ExplodingFlareTypeMatching);

			CustomItemIDSets.IsCombatFlareGun[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 48;
			Item.height = 28;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot; // 5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 1;
			Item.value = 50000;
			Item.rare = ItemRarityID.LightRed; // 4
			Item.UseSound = new(Mod.Name + "/Sounds/Item/FlarePistol");
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<InterstellarLaser>();
			Item.shootSpeed = 6f;
			Item.scale = 0.75f;
			Item.useAmmo = AmmoID.Flare;
			Item.holdStyle = ItemHoldStyleID.HoldHeavy;
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 12;
				flash.posOffsetXRight = -2;
				flash.posOffsetY = 0;
				flash.posOffsetYGravity = 4;
				flash.frameCount = 2;
				flash.frameRate = 4;
				flash.colorNoAlpha = new(255, 150, 0);
				flash.alpha = 0;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 15f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/FlareFly") { MaxInstances = 5, Volume = 0.75f }, position);
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, default, default, Main.rand.NextFloat(0.75f, 1.5f));
			Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(0.9f, 1.1f), type, damage, knockback, player.whoAmI, default, default, Main.rand.NextFloat(0.75f, 1.5f));
			Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(0.9f, 1.1f), type, damage, knockback, player.whoAmI, default, default, Main.rand.NextFloat(0.75f, 1.5f));
			return false;
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 2);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<CombatFlarePistol>())
				.AddIngredient(ItemID.SoulofNight, 5)
				.AddIngredient(ItemID.SoulofLight, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class FlareSubmachineGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;

			AmmoID.Sets.SpecificLauncherAmmoProjectileFallback[Type] = ItemID.FlareGun;

			AmmoID.Sets.SpecificLauncherAmmoProjectileMatches.Add(Type, CombatFlarePistol.ExplodingFlareTypeMatching);

			CustomItemIDSets.IsCombatFlareGun[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 80;
			Item.height = 40;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Shoot; // 5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 2;
			Item.value = 100000;
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = new(Mod.Name + "/Sounds/Item/FlarePistol");
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<InterstellarLaser>();
			Item.shootSpeed = 7f;
			Item.scale = 0.75f;
			Item.useAmmo = AmmoID.Flare;
			Item.holdStyle = ItemHoldStyleID.HoldHeavy;
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 18;
				flash.posOffsetXRight = -2;
				flash.posOffsetY = 0;
				flash.posOffsetYGravity = 4;
				flash.frameCount = 3;
				flash.frameRate = 4;
				flash.colorNoAlpha = new(255, 150, 0);
				flash.alpha = 0;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 15f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			SoundEngine.PlaySound(new(Mod.Name + "/Sounds/Custom/FlareFly") { MaxInstances = 5, Volume = 0.75f }, position);
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, default, default, Main.rand.NextFloat(0.75f, 1.5f));
			Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(0.9f, 1.1f), type, damage, knockback, player.whoAmI, default, default, Main.rand.NextFloat(0.75f, 1.5f));
			Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(0.9f, 1.1f), type, damage, knockback, player.whoAmI, default, default, Main.rand.NextFloat(0.75f, 1.5f));
			return false;
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, 0);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<TripleBarrelFlarePistol>())
				.AddIngredient(ItemID.BeetleHusk, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
