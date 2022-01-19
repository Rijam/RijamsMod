using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	public class RocketBooster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rocket Booster");
			Tooltip.SetDefault("+10% rocket damage\n+20% rocket velocity\n+10% rocket knockback");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Sold by Interstellar Traveler]");
		}

		public override void SetDefaults()
		{
			//item.color = Color.Yellow; //colors the inventory sprite
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Lime; //7
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.rocketDamage += 0.1f;
			player.GetModPlayer<RijamsModPlayer>().rocketBooster = true;
		}
	}
	[AutoloadEquip(EquipType.Back, EquipType.Face, EquipType.HandsOn)]
	public class GamutApparatus : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gamut Apparatus");
			Tooltip.SetDefault("+15% Ranged damage\n+10% Ranged Critical Strike chance\n20% chance to not consume ammo\n+20% projectile velocity and knockback\nLights wooden arrows ablaze\nEnemies are slightly less likely to target you\nRight Click to zoom out (Hide to disable)");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Cyan; //9
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.rangedDamage += 0.15f;
			player.rangedCrit += 10;
			
			player.ammoCost80 = true;
			player.GetModPlayer<RijamsModPlayer>().rocketBooster = true;
			player.GetModPlayer<RijamsModPlayer>().gamutApparatus = true;
			player.aggro -= 15 * 16; //15 tiles
			if (!hideVisual)
			{
				player.scope = true;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SniperScope, 1);
			recipe.AddIngredient(ItemID.MagicQuiver, 1);
			recipe.AddIngredient(ItemID.PutridScent, 1);
			recipe.AddIngredient(ItemID.MagmaStone, 1);
			recipe.AddIngredient(ModContent.ItemType<RocketBooster>(), 1);
			recipe.AddIngredient(ItemID.LunarBlockVortex, 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}