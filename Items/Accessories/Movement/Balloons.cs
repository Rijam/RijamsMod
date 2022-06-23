using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Movement
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
			Item.width = 30;
			Item.height = 28;
			Item.rare = ItemRarityID.Yellow; //8
			Item.value = Item.sellPrice(0, 3, 50, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.jumpSpeedBoost += 2.5f;
			player.hasJumpOption_Fart = true;
			player.hasJumpOption_Sail = true;
			player.honeyCombItem = Item;
			//player.GetModPlayer<RijamsModPlayer>().honeyComb = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FartInABalloon, 1)
				.AddIngredient(ItemID.SharkronBalloon, 1)
				.AddIngredient(ItemID.HoneyBalloon, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
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
			Item.width = 32;
			Item.height = 42;
			Item.rare = ItemRarityID.Cyan; //9
			Item.value = Item.sellPrice(0, 8, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.jumpSpeedBoost += 5f;
			player.hasJumpOption_Cloud = true;
			player.hasJumpOption_Blizzard = true;
			player.hasJumpOption_Sandstorm = true;
			player.hasJumpOption_Fart = true;
			player.hasJumpOption_Sail = true;
			player.honeyCombItem = Item;
			//player.GetModPlayer<RijamsModPlayer>().honeyComb = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BundleofBalloons, 1)
				.AddIngredient(ModContent.ItemType<DifferentBundleOfBalloons>(), 1)
				.AddIngredient(ItemID.PartyBundleOfBalloonsAccessory, 1)
				.AddIngredient(ItemID.PartyBundleOfBalloonTile, 1)
				.AddIngredient(ItemID.PartyBalloonAnimal, 1)
				.AddIngredient(ItemID.SillyBalloonGreen, 10)
				.AddIngredient(ItemID.SillyBalloonPink, 10)
				.AddIngredient(ItemID.SillyBalloonPurple, 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
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
			Item.width = 32;
			Item.height = 42;
			Item.rare = ItemRarityID.Red; //10
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.noFallDmg = true;
			player.fireWalk = true;
			player.jumpSpeedBoost += 5f;
			player.hasJumpOption_Cloud = true;
			player.hasJumpOption_Blizzard = true;
			player.hasJumpOption_Sandstorm = true;
			player.hasJumpOption_Fart = true;
			player.hasJumpOption_Sail = true;
			player.honeyCombItem = Item;
			//player.GetModPlayer<RijamsModPlayer>().honeyComb = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<TooManyBalloons>(), 1)
				.AddIngredient(ItemID.ObsidianHorseshoe, 6)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.BlueHorseshoeBalloon, 1)
				.AddIngredient(ItemID.WhiteHorseshoeBalloon, 1)
				.AddIngredient(ItemID.YellowHorseshoeBalloon, 1)
				.AddIngredient(ItemID.BalloonHorseshoeFart, 1)
				.AddIngredient(ItemID.BalloonHorseshoeSharkron, 1)
				.AddIngredient(ItemID.BalloonHorseshoeHoney, 1)
				.AddIngredient(ItemID.ObsidianSkull, 6)
				.AddIngredient(ItemID.PartyBundleOfBalloonsAccessory, 1)
				.AddIngredient(ItemID.PartyBundleOfBalloonTile, 1)
				.AddIngredient(ItemID.PartyBalloonAnimal, 1)
				.AddIngredient(ItemID.SillyBalloonGreen, 10)
				.AddIngredient(ItemID.SillyBalloonPink, 10)
				.AddIngredient(ItemID.SillyBalloonPurple, 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}