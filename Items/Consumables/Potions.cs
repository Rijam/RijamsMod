using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Buffs.Potions;
using RijamsMod.Items.Fishing;

namespace RijamsMod.Items.Consumables
{
	public class FuryPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fury Potion");
			// Tooltip.SetDefault("+10% attack speed");
			ItemID.Sets.DrinkParticleColors[Item.type] =
			[
				new(255, 181, 229),
				new(159, 36, 159),
				new(168, 24, 24)
			];
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 2);
			Item.buffType = ModContent.BuffType<Fury>();
			Item.buffTime = 14400; //4 minutes
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater, 1)
				.AddIngredient(ItemID.PrincessFish, 1)
				.AddIngredient(ItemID.Cog, 1)
				.AddIngredient(ItemID.Pumpkin, 1)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}
	public class SupportPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Support Potion");
			// Tooltip.SetDefault("+1 sentry capacity");
			ItemID.Sets.DrinkParticleColors[Item.type] =
			[
				new(252, 98, 100),
				new(223, 17, 20),
				new(132, 17, 19)
			];
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 2);
			Item.buffType = ModContent.BuffType<Support>();
			Item.buffTime = 28800; //8 minutes
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater, 1)
				.AddIngredient(ItemID.RockLobster, 1)
				.AddIngredient(ItemID.Waterleaf, 1)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}
	public class SoaringPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Soaring Potion");
			// Tooltip.SetDefault("+0.5 seconds wing flight");
			ItemID.Sets.DrinkParticleColors[Item.type] =
			[
				new(139, 235, 239),
				new(15, 167, 211),
				new(1, 70, 161)
			];
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 30;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 2);
			Item.buffType = ModContent.BuffType<Soaring>();
			Item.buffTime = 10800; //3 minutes
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater, 1)
				.AddIngredient(ItemID.Damselfish, 1)
				.AddIngredient(ItemID.Feather, 1)
				.AddIngredient(ItemID.SoulofFlight, 1)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}
	public class FerociousPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ferocious Potion");
			// Tooltip.SetDefault("+10 armor penetration");
			ItemID.Sets.DrinkParticleColors[Item.type] =
			[
				new(251, 194, 29),
				new(209, 127, 147),
				new(140, 85, 10)
			];
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 2);
			Item.buffType = ModContent.BuffType<Ferocious>();
			Item.buffTime = 14400; //4 minutes
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater)
				.AddIngredient(ModContent.ItemType<Fishing.HornetTail>())
				.AddIngredient(ItemID.WormTooth)
				.AddIngredient(ModContent.ItemType<Materials.Sulfur>())
				.AddTile(TileID.Bottles)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater)
				.AddIngredient(ModContent.ItemType<Fishing.HornetTail>())
				.AddIngredient(ModContent.ItemType<Materials.CrawlerChelicera>())
				.AddIngredient(ModContent.ItemType<Materials.Sulfur>())
				.AddTile(TileID.Bottles)
				.Register();
			if (ModLoader.TryGetMod("ExxoAvalonOrigins", out Mod avalon))
			{
				if (avalon.TryFind<ModItem>("YuckyBit", out ModItem yuckyBit))
				{
					CreateRecipe()
						.AddIngredient(ItemID.BottledWater)
						.AddIngredient(ModContent.ItemType<Fishing.HornetTail>())
						.AddIngredient(yuckyBit)
						.AddIngredient(ModContent.ItemType<Materials.Sulfur>())
						.AddTile(TileID.Bottles)
						.Register();
				}
			}
		}
	}
	public class FrenzyPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.DrinkParticleColors[Item.type] =
			[
				new(96, 211, 255),
				new(24, 168, 168),
				new(44, 47, 176)
			];
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 1);
			Item.buffType = ModContent.BuffType<Frenzy>();
			Item.buffTime = 14400; //4 minutes
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater, 1)
				.AddIngredient(ModContent.ItemType<FungiEel>(), 1)
				.AddIngredient(ItemID.SharkFin, 1)
				.AddIngredient(ItemID.MagicLavaDropper)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}
}
