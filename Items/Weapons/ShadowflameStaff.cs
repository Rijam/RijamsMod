using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons
{
	public class ShadowflameStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Staff");
			Tooltip.SetDefault("Summons a Shadowflame Phantom to fight for you" + "\n[c/403638:Dropped by Goblin Summoner]");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 30;
			item.knockBack = 0.5f;
			item.mana = 10;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item80;
			item.autoReuse = true;
			
			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			//item.buffType = ModContent.BuffType<ShadowflamePhantomBuff>();
			item.buffType = mod.BuffType("ShadowflamePhantomBuff");
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<ShadowflamePhantom>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(item.buffType, 2);

			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
			position = Main.MouseWorld;
			return true;
		}
	}
}
