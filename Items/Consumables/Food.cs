using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Buffs.Potions;
using RijamsMod.Items.Armor.Vanity.IntTrav;

namespace RijamsMod.Items.Consumables
{
	public class StrangeRoll : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Strange Roll");
			// Tooltip.SetDefault("{$CommonItemTooltip.MediumStats}\n'A strange food from an unknown place'\n'What's inside?'");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Traveling Merchant]", "[c/474747:After defeating Eye of Cthulhu]" } );
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3]
			{
				new Color(187, 76, 218),
				new Color(168, 35, 12),
				new Color(211, 172, 98)
			};
			ItemID.Sets.IsFood[Type] = true;
		}
		/*public override bool CanUseItem(Player player)
		{
			return !(player.HasBuff(BuffID.WellFed)) ; //can't eat if the player has Well Fed
		}*/

		public override void SetDefaults()
		{
			Item.DefaultToFood(20, 24, BuffID.WellFed2, 72000);
			Item.rare = ItemRarityID.Yellow;
			Item.value = 10000;
		}
	}
	public class RyeJam : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rye Jam");
			// Tooltip.SetDefault("{$Mods.RijamsMod.Common.ExtremeStats}\n'Wait, how do you pronounce it?'");
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3]
			{
				new Color(242, 225, 23),
				new Color(195, 181, 155),
				new Color(105, 56, 15)
			};
			ItemID.Sets.IsFood[Type] = true;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<StrangeRoll>(); // Shimmer transforms the item.
		}
		/*public override bool CanUseItem(Player player)
		{
			return !(player.HasBuff(BuffID.WellFed)) ; //can't eat if the player has Well Fed
		}*/

		public override void SetDefaults()
		{
			Item.DefaultToFood(26, 28, ModContent.BuffType<ExceptionalFeast>(), 18000);
			Item.rare = ItemRarityID.Cyan;
			Item.value = 200000;
		}
		//Tool tip set in GlobalItems
	}
	public class ReefCola : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Reef-Cola");
			// Tooltip.SetDefault("Move freely in liquids\n'Taste the ocean, with Reef-Cola.'");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating Duke Fishron]" } );
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.DrinkParticleColors[Item.type] = new Color[3]
			{
				new Color(85, 136, 212),
				new Color(52, 68, 149),
				new Color(26, 28, 81)
			};
			ItemID.Sets.IsFood[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.DefaultToFood(16, 28, ModContent.BuffType<SwimBoostBuff>(), 3600, true); //The extra true here tells it the item is a drink.
			Item.rare = ItemRarityID.Blue;
			Item.value = 4000;
		}
	}
	public class RoastedMushroom : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Roasted Mushroom");
			// Tooltip.SetDefault("{$Mods.RijamsMod.Common.MinusculeStats}");
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3]
			{
				new Color(237, 160, 69),
				new Color(115, 57, 15),
				new Color(57, 25, 3)
			};
			ItemID.Sets.IsFood[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.DefaultToFood(16, 28, ModContent.BuffType<Satiated>(), 3600);
			Item.rare = ItemRarityID.White;
			Item.value = 300;
		}
		public override void AddRecipes()
		{
			
			CreateRecipe()
				.AddIngredient(ItemID.Mushroom)
				.AddTile(TileID.Campfire)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.GreenMushroom)
				.AddTile(TileID.Campfire)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.TealMushroom)
				.AddTile(TileID.Campfire)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.GlowingMushroom)
				.AddTile(TileID.Campfire)
				.Register();
			
			CreateRecipe()
				.AddIngredient(ItemID.Mushroom)
				.AddTile(TileID.CookingPots)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.GreenMushroom)
				.AddTile(TileID.CookingPots)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.TealMushroom)
				.AddTile(TileID.CookingPots)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.GlowingMushroom)
				.AddTile(TileID.CookingPots)
				.Register();
		}
	}
	public class FreshBlueberry : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fresh Blueberry");
			// Tooltip.SetDefault("{$Mods.RijamsMod.Common.MinusculeStats}");
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3]
			{
				new Color(93, 115, 196),
				new Color(44, 60, 114),
				new Color(15, 9, 44)
			};
			ItemID.Sets.IsFood[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.DefaultToFood(16, 28, ModContent.BuffType<Satiated>(), 1200);
			Item.rare = ItemRarityID.White;
			Item.value = 650;
		}
		public override void AddRecipes()
		{
			CreateRecipe(3)
				.AddIngredient(ItemID.BlueBerries)
				.Register();
		}
	}
	public class CaveCarrot : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cave Carrot");
			// Tooltip.SetDefault("{$Mods.RijamsMod.Common.MinusculeStats}");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by pots in the Caverns layer]", "[c/474747:Sold by Skeleton Merchant]", "[c/474747:  after any Old One's Army has been defeated]", "[c/474747:  and even moon phases]" });
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3]
			{
				new Color(193, 112, 38),
				new Color(116, 57, 2),
				new Color(74, 93, 13)
			};
			ItemID.Sets.IsFood[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.DefaultToFood(30, 32, ModContent.BuffType<Satiated>(), 1800);
			Item.rare = ItemRarityID.White;
			Item.value = 100;
		}
	}
	public class HardyStew : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hardy Stew");
			// Tooltip.SetDefault("{$CommonItemTooltip.MajorStats}");
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3]
			{
				new Color(205, 139, 15),
				new Color(105, 83, 30),
				new Color(74, 32, 1)
			};
			ItemID.Sets.IsFood[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.DefaultToFood(30, 22, BuffID.WellFed3, 18000, true);
			Item.rare = ItemRarityID.Orange;
			Item.value = 10000;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SeafoodDinner)
				.AddIngredient(ModContent.ItemType<RoastedMushroom>(), 2)
				.AddIngredient(ModContent.ItemType<CaveCarrot>(), 2)
				.AddTile(TileID.CookingPots)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.CookedFish, 2)
				.AddIngredient(ModContent.ItemType<RoastedMushroom>(), 2)
				.AddIngredient(ModContent.ItemType<CaveCarrot>(), 2)
				.AddTile(TileID.CookingPots)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.Sashimi, 2)
				.AddIngredient(ModContent.ItemType<RoastedMushroom>(), 2)
				.AddIngredient(ModContent.ItemType<CaveCarrot>(), 2)
				.AddTile(TileID.CookingPots)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.RoastedDuck)
				.AddIngredient(ModContent.ItemType<RoastedMushroom>(), 2)
				.AddIngredient(ModContent.ItemType<CaveCarrot>(), 2)
				.AddTile(TileID.CookingPots)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.RoastedBird, 2)
				.AddIngredient(ModContent.ItemType<RoastedMushroom>(), 2)
				.AddIngredient(ModContent.ItemType<CaveCarrot>(), 2)
				.AddTile(TileID.CookingPots)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.LobsterTail)
				.AddIngredient(ModContent.ItemType<RoastedMushroom>(), 2)
				.AddIngredient(ModContent.ItemType<CaveCarrot>(), 2)
				.AddTile(TileID.CookingPots)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.GrilledSquirrel, 2)
				.AddIngredient(ModContent.ItemType<RoastedMushroom>(), 2)
				.AddIngredient(ModContent.ItemType<CaveCarrot>(), 2)
				.AddTile(TileID.CookingPots)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.BunnyStew, 2)
				.AddIngredient(ModContent.ItemType<RoastedMushroom>(), 2)
				.AddIngredient(ModContent.ItemType<CaveCarrot>(), 2)
				.AddTile(TileID.CookingPots)
				.Register();
		}
	}
}