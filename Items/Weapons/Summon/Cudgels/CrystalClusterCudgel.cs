using RijamsMod.Projectiles.Summon.Support;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;

namespace RijamsMod.Items.Weapons.Summon.Cudgels
{
	public class CrystalClusterCudgel : CudgelDefenseItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crystal Cluster Cudgel");
			// Tooltip.SetDefault("Summons a Crystal Cluster to defend you\nPlayers within its aura receive:\n+7 defense\n+4% damage reduction\n15 tile radius");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Queen Slime]", null, null });
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
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item90; //131, 123
			Item.autoReuse = false;
			
			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;

			Item.buffType = ModContent.BuffType<Buffs.Minions.CrystalClusterBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<CrystalCluster>();
		}

		public override int Defense => base.Defense + 7; // 7 defense
		public override float DamageReduction => base.DamageReduction + 0.04f; // 4% damage reduction
		public override int Radius => base.Radius + 15; // 15 tile radius

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Defense, DamageReduction * 100, Radius);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);

			// Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;

			if (projectile.ModProjectile is CrystalCluster modProjectile)
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
	}
}
