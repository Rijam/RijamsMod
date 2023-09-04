using RijamsMod.Projectiles.Summon.Support;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Collections.Generic;

namespace RijamsMod.Items.Weapons.Summon.Cudgels
{
	public class RadiantLanternCudgel : CudgelHealingItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Radiant Lantern Cudgel");
			// Tooltip.SetDefault("Summons a Radiant Lantern to heal you\nTwo players with the lowest HP within its aura will be targeted\n20 HP every 30 seconds\n30 tile radius");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by Empress of Light]" });
		}

		public override void SetDefaults()
		{
			Item.damage = 1; // Token amount of damage so it can be reforged and so it shows up in summon weapon filters.
			Item.knockBack = 0f;
			Item.mana = 20;
			Item.width = 18;
			Item.height = 48;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item90 with { Pitch = 0.2f }; //131, 123
			Item.autoReuse = false;
			
			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;

			Item.buffType = ModContent.BuffType<Buffs.Minions.RadiantLanternBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<RadiantLantern>();
		}

		public override int HealingAmount => base.HealingAmount + 20; // 20 hp
		public override int HealingTime => base.HealingTime + (30 * 60); // 30 seconds
		public override int Radius => base.Radius + 30; // 30 tile radius

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(HealingAmount, HealingTime / 60, Radius);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);

			// Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
			var projectile = Projectile.NewProjectileDirect(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;
			if (projectile.ModProjectile is RadiantLantern modProjectile)
			{
				modProjectile.healAmount = HealingAmount;
				modProjectile.cooldownTime = HealingTime;
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
