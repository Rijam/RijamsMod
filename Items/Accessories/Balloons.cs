using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	[AutoloadEquip(EquipType.Balloon)]
	public class DifferentBundleOfBalloons : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Different Bundle of Balloons");
			Tooltip.SetDefault("Allows the holder to triple jump\nIncreases jump height\nReleases bees and douses the user in honey when damaged");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.rare = ItemRarityID.Yellow; //8
			item.value = Item.sellPrice(0, 3, 50, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.jumpSpeedBoost += 2.5f;
			player.doubleJumpFart = true;
			player.doubleJumpSail = true;
			player.GetModPlayer<RijamsModPlayer>().honeyComb = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FartInABalloon, 1);
			recipe.AddIngredient(ItemID.SharkronBalloon, 1);
			recipe.AddIngredient(ItemID.HoneyBalloon, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Balloon)]
	public class TooManyBalloons : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Too Many Balloons");
			Tooltip.SetDefault("Allows the holder to sextuple jump\nGreatly increases jump height\nReleases bees and douses the user in honey when damaged");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 42;
			item.rare = ItemRarityID.Cyan; //9
			item.value = Item.sellPrice(0, 8, 0, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.jumpSpeedBoost += 5f;
			player.doubleJumpCloud = true;
			player.doubleJumpBlizzard = true;
			player.doubleJumpSandstorm = true;
			player.doubleJumpFart = true;
			player.doubleJumpSail = true;
			player.GetModPlayer<RijamsModPlayer>().honeyComb = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BundleofBalloons, 1);
			recipe.AddIngredient(ModContent.ItemType<DifferentBundleOfBalloons>(), 1);
			recipe.AddIngredient(ItemID.PartyBundleOfBalloonsAccessory, 1);
			recipe.AddIngredient(ItemID.PartyBundleOfBalloonTile, 1);
			recipe.AddIngredient(ItemID.PartyBalloonAnimal, 1);
			recipe.AddIngredient(ItemID.SillyBalloonGreen, 10);
			recipe.AddIngredient(ItemID.SillyBalloonPink, 10);
			recipe.AddIngredient(ItemID.SillyBalloonPurple, 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Balloon)]
	public class TooManyHorseshoeBalloons : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Too Many Horseshoe Balloons");
			Tooltip.SetDefault("Allows the holder to sextuple jump\nGreatly increases jump height\nReleases bees and douses the user in honey when damaged\nNegates fall damage\nImmunity to fire blocks");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 42;
			item.rare = ItemRarityID.Red; //10
			item.value = Item.sellPrice(0, 15, 0, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.noFallDmg = true;
			player.fireWalk = true;
			player.jumpSpeedBoost += 5f;
			player.doubleJumpCloud = true;
			player.doubleJumpBlizzard = true;
			player.doubleJumpSandstorm = true;
			player.doubleJumpFart = true;
			player.doubleJumpSail = true;
			player.GetModPlayer<RijamsModPlayer>().honeyComb = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TooManyBalloons>(), 1);
			recipe.AddIngredient(ItemID.ObsidianHorseshoe, 6);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BlueHorseshoeBalloon, 1);
			recipe.AddIngredient(ItemID.WhiteHorseshoeBalloon, 1);
			recipe.AddIngredient(ItemID.YellowHorseshoeBalloon, 1);
			recipe.AddIngredient(ItemID.BalloonHorseshoeFart, 1);
			recipe.AddIngredient(ItemID.BalloonHorseshoeSharkron, 1);
			recipe.AddIngredient(ItemID.BalloonHorseshoeHoney, 1);
			recipe.AddIngredient(ItemID.ObsidianSkull, 6);
			recipe.AddIngredient(ItemID.PartyBundleOfBalloonsAccessory, 1);
			recipe.AddIngredient(ItemID.PartyBundleOfBalloonTile, 1);
			recipe.AddIngredient(ItemID.PartyBalloonAnimal, 1);
			recipe.AddIngredient(ItemID.SillyBalloonGreen, 10);
			recipe.AddIngredient(ItemID.SillyBalloonPink, 10);
			recipe.AddIngredient(ItemID.SillyBalloonPurple, 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}