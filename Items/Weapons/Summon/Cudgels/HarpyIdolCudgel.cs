using RijamsMod.Projectiles.Summon.Support;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;

namespace RijamsMod.Items.Weapons.Summon.Cudgels
{
	public class HarpyIdolCudgel : CudgelDefenseItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Harpy Idol Cudgel");
			// Tooltip.SetDefault("Summons a Harpy Idol to defend you\nPlayers within its aura receive:\n+3 defense\n+1% damage reduction\n5 tile radius");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 1; // Token amount of damage so it can be reforged and so it shows up in summon weapon filters.
			Item.knockBack = 0f;
			Item.mana = 20;
			Item.width = 22;
			Item.height = 48;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.value = Item.sellPrice(silver: 50);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item90; //131, 123
			Item.autoReuse = false;
			
			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;

			Item.buffType = ModContent.BuffType<Buffs.Minions.HarpyIdolBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<HarpyIdol>();
		}

		public override int Defense => base.Defense + 3; // 3 defense
		public override float DamageReduction => base.DamageReduction + 0.01f; // 1% damage reduction
		public override int Radius => base.Radius + 5; // 5 tile radius

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Defense, DamageReduction * 100, Radius);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);

			// Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;

			if (projectile.ModProjectile is HarpyIdol modProjectile)
			{
				modProjectile.additionalDefense = Defense;
				modProjectile.additionalDR = DamageReduction;
				modProjectile.distRadius = Radius;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI);
			}
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
				.AddIngredient(ItemID.Feather)
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 10)
				.AddRecipeGroup(RijamsModRecipes.GoldBars, 5)
				.AddRecipeGroup(RijamsModRecipes.EvilBars, 2)
				.AddTile(TileID.SkyMill)
				.Register();
		}
	}
}
