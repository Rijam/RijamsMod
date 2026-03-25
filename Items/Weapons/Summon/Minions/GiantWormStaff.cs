using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Buffs.Minions;
using RijamsMod.Projectiles.Summon.Minions;

namespace RijamsMod.Items.Weapons.Summon.Minions
{
	public class GiantWormStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Dropped from Giant Worms]"]);
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.knockBack = 0.5f;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 2);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item44;
			Item.autoReuse = true;
			
			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<GiantWormBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<GiantWorm>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Spawn the projectile for the first time if the player doesn't have a minion of that kind yet
			if (!player.HasBuff(ModContent.BuffType<GiantWormBuff>()))
			{
				// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
				player.AddBuff(Item.buffType, 2);

				// Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
				try
				{
					Projectile projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
					projectile.originalDamage = damage;
				}
				catch (Exception e)
				{
					ModContent.GetInstance<RijamsMod>().Logger.Error($"Failed to spawn Giant Worm minion. {e}");
				}

				// Return to not run the rest of the Shoot() code when spawning the projectile
				// Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
				return false;
			}

			// If there is enough room for the next summon, raise the damage of it, and make it take up more minion slots
			foreach (Projectile projectile in Main.ActiveProjectiles)
			{
				if (projectile.type == ModContent.ProjectileType<GiantWorm>() && projectile.ai[0] < player.maxMinions && projectile.owner == Main.myPlayer)
				{
					projectile.ai[0]++; // NumberOfTimesSummoned
					projectile.minionSlots++; // The projectile counts as more minion slots.
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						try
						{
							ProjectileID.Sets.TrailCacheLength[projectile.type] += 8; // Increase the trail cache for drawing the body and tail.
							Array.Resize(ref projectile.oldPos, ProjectileID.Sets.TrailCacheLength[projectile.type]);
							// projectile.netUpdate = true;
						}
						catch (Exception e)
						{
							ModContent.GetInstance<RijamsMod>().Logger.Warn($"Giant Worm Staff: unable to resize oldPos array. {e}");
						}
					}
					projectile.netUpdate = true;
				}
			}
			return false;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				return true;
			}
			foreach (Projectile projectile in Main.ActiveProjectiles)
			{
				if (projectile.type == ModContent.ProjectileType<GiantWorm>() && projectile.ai[0] == player.maxMinions && projectile.owner == Main.myPlayer)
				{
					// Main.NewText($"Max slots used up: projectile.ai[0] {projectile.ai[0]}   projectile.owner {projectile.owner}");
					return false;
				}
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
			position = Main.MouseWorld;
		}
	}
}
