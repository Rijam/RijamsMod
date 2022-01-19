using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons
{
	public class MeatballStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meatball Staff");
			Tooltip.SetDefault("Summons a Meatball Demon to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 45;
			item.knockBack = 1f;
			item.mana = 10;
			item.width = 48;
			item.height = 44;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(gold: 3);
			item.rare = ItemRarityID.Yellow;
			if (!Main.dedServ) //Need to check if a server is running, otherwise it will break multiplayer
			{
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/MeatballStaff").WithVolume(.8f);
			}
			item.autoReuse = true;
			
			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			//item.buffType = ModContent.BuffType<HissyDemonBuff>();
			item.buffType = ModContent.BuffType<Buffs.MeatballDemonBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<MeatballDemon>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(item.buffType, 2);

			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
			position = Main.MouseWorld;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HissyStaff>());
			recipe.AddIngredient(ItemID.Ectoplasm, 3);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.SunEssence>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
