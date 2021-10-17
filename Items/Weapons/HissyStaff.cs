using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons
{
	public class HissyStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hissy Staff");
			Tooltip.SetDefault("Summons a Hissy Demon to fight for you" + "\n[c/403638:Dropped by Dark Soldier]");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.knockBack = 1f;
			item.mana = 10;
			item.width = 48;
			item.height = 44;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.LightRed;
			if (!Main.dedServ) //Need to check if a server is running, otherwise it will break multiplayer
			{
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/HissyStaff").WithVolume(.8f);
			}
			item.autoReuse = true;
			
			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			//item.buffType = ModContent.BuffType<HissyDemonBuff>();
			item.buffType = mod.BuffType("HissyDemonBuff");
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<HissyDemon>();
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
