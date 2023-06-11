using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Ranger
{
	[AutoloadEquip(EquipType.Waist)]
	public class AmmoPouch : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ammo Pouch");
			// Tooltip.SetDefault("+5% bullet damage\n+5% ranged attack speed");
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 32;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.bulletDamage += 0.05f;
			player.GetAttackSpeed(DamageClass.Ranged) += 0.05f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Leather, 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
	public class RocketBooster : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rocket Booster");
			// Tooltip.SetDefault("+10% rocket damage\n+20% rocket velocity\n+10% rocket knockback");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After completing quest]", null });
		}

		public override void SetDefaults()
		{
			//item.color = Color.Yellow; //colors the inventory sprite
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Lime; //7
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//player.specialistDamage += 0.1f;
			player.GetModPlayer<RijamsModPlayer>().rocketBooster = true;
		}
	}
	[AutoloadEquip(EquipType.Back, EquipType.Face, EquipType.HandsOn)]
	public class GamutApparatus : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gamut Apparatus");
			// Tooltip.SetDefault("+15% Ranged damage\n+10% Ranged Critical Strike chance\n20% chance to not consume ammo\n+20% projectile velocity and knockback\nLights wooden arrows ablaze\nEnemies are slightly less likely to target you\nRight Click to zoom out (Hide to disable)");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Cyan; //9
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Ranged) += 0.15f;
			player.GetCritChance(DamageClass.Ranged) += 10;
			
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
			CreateRecipe()
				.AddIngredient(ItemID.SniperScope, 1)
				.AddIngredient(ItemID.MagicQuiver, 1)
				.AddIngredient(ItemID.PutridScent, 1)
				.AddIngredient(ItemID.MagmaStone, 1)
				.AddIngredient(ModContent.ItemType<RocketBooster>(), 1)
				.AddIngredient(ItemID.LunarBlockVortex, 5)
				.AddIngredient(ItemID.LunarBar, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.AddTile(TileID.LunarCraftingStation)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.ReconScope, 1)
				.AddIngredient(ItemID.MoltenQuiver, 1)
				.AddIngredient(ModContent.ItemType<RocketBooster>(), 1)
				.AddIngredient(ItemID.LunarBlockVortex, 5)
				.AddIngredient(ItemID.LunarBar, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.AddTile(TileID.LunarCraftingStation)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.SniperScope, 1)
				.AddIngredient(ItemID.StalkersQuiver, 1)
				.AddIngredient(ItemID.MagmaStone, 1)
				.AddIngredient(ModContent.ItemType<RocketBooster>(), 1)
				.AddIngredient(ItemID.LunarBlockVortex, 5)
				.AddIngredient(ItemID.LunarBar, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}