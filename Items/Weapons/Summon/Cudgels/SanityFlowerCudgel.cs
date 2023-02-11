using RijamsMod.Projectiles.Summon.Support;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons.Summon.Cudgels
{
	public class SanityFlowerCudgel : CudgelHealingItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sanity Flower Cudgel");
			// Tooltip.SetDefault("Summons a Sanity Flower to heal you\nThe player with the lowest HP within its aura will be targeted\n20 HP every 30 seconds\n15 tile radius");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Deerclops]", null, null });
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.knockBack = 0f;
			Item.mana = 20;
			Item.width = 22;
			Item.height = 48;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.value = Item.sellPrice(gold: 1, silver: 50);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item90 with { Pitch = 0.2f }; //131, 123
			Item.autoReuse = false;
			
			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;

			Item.buffType = ModContent.BuffType<Buffs.Minions.SanityFlowerBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<SanityFlower>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);

			// Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
			var projectile = Projectile.NewProjectileDirect(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;
			if (projectile.ModProjectile is SanityFlower modProjectile)
			{
				modProjectile.cooldownTime = 30 * 60; // 30 seconds
				modProjectile.distRadius = 15; // 15 tile radius
			}

			// Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
			return false;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
			position = Main.MouseWorld;
		}
	}
}
