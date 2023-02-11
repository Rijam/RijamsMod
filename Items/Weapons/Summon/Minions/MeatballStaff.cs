using RijamsMod.Projectiles.Summon.Minions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons.Summon.Minions
{
	public class MeatballStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Meatball Staff");
			// Tooltip.SetDefault("Summons a Meatball Demon to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.knockBack = 1f;
			Item.mana = 10;
			Item.width = 48;
			Item.height = 44;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = new(Mod.Name + "/Sounds/Item/MeatballStaff") { Volume = 0.8f, MaxInstances = 5 };
			Item.autoReuse = true;
			
			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			//item.buffType = ModContent.BuffType<HissyDemonBuff>();
			Item.buffType = ModContent.BuffType<Buffs.Minions.MeatballDemonBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<MeatballDemon>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);

			// Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;

			// Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
			return false;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
			position = Main.MouseWorld;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<HissyStaff>())
				.AddIngredient(ItemID.Ectoplasm, 3)
				.AddIngredient(ModContent.ItemType<Items.Materials.SunEssence>(), 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
