using RijamsMod.Projectiles.Summon.Minions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace RijamsMod.Items.Weapons.Summon.Minions
{
	public class BabyBloodEelStaff : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return false;
		}

		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:t]" });
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.knockBack = 1f;
			Item.mana = 10;
			Item.width = 48;
			Item.height = 44;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item44;
			Item.autoReuse = true;
			
			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<Buffs.Minions.BabyBloodEelBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<BabyBloodEel>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.slotsMinions < player.maxMinions)
			{
				// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
				player.AddBuff(Item.buffType, 2);

				// Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
				var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
				projectile.originalDamage = Item.damage;

				// Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
			}
			return false;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.slotsMinions < player.maxMinions)
			{
				return true;
			}
			return false;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
			position = Main.MouseWorld;
		}
	}
}
