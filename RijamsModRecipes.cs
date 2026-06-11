using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RijamsMod.Items;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace RijamsMod
{
	public class RijamsModRecipes : ModSystem
	{
		public override void AddRecipes()
		{
			Recipe.Create(ItemID.SnowGlobe)
				.AddIngredient(ItemID.Glass, 5)
				.AddIngredient(ItemID.SnowBlock, 5)
				.AddIngredient(ItemID.SoulofLight, 5)
				.AddIngredient(ItemID.SoulofNight, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			Recipe.Create(ItemID.Leather)
				.AddIngredient(ItemID.Vertebrae, 5)
				.AddTile(TileID.WorkBenches)
				.Register();

			Recipe.Create(ItemID.SlimeStaff)
				.AddIngredient(ItemID.Gel, 500)
				.AddIngredient(ItemID.Wood, 5)
				.AddIngredient(ItemID.FallenStar, 1)
				.AddTile(TileID.Anvils)
				.Register();

			Recipe.Create(ItemID.RainbowString)
				.AddIngredient(ItemID.WhiteString, 1)
				.AddIngredient(ItemID.LivingRainbowDye, 1)
				.AddTile(TileID.DyeVat)
				.Register();

			Recipe.Create(ItemID.RainbowString)
				.AddIngredient(ItemID.WhiteString, 1)
				.AddIngredient(ItemID.RainbowHairDye, 1)
				.AddTile(TileID.DyeVat)
				.Register();

			Recipe.Create(ItemID.SunplateBlock)
				.AddIngredient(ModContent.ItemType<Items.Placeable.SunplatePillarBlock>(), 1)
				.Register();

			Recipe.Create(ItemID.SunplateBlock)
				.AddIngredient(ModContent.ItemType<Items.Placeable.RedSunplateBlock>(), 1)
				.Register();

			Recipe.Create(ItemID.WandofSparking)
				.AddIngredient(ModContent.ItemType<Items.Weapons.Magic.HotStick>(), 1)
				.AddIngredient(ItemID.Torch, 99)
				.AddIngredient(ItemID.FallenStar, 2)
				.AddTile(TileID.Anvils)
				.Register();

			Recipe.Create(ItemID.BlueBerries)
				.AddIngredient(ModContent.ItemType<Items.Consumables.FreshBlueberry>(), 3)
				.Register();

			Recipe.Create(ItemID.SeafoodDinner)
				.AddIngredient(ModContent.ItemType<Items.Fishing.HornetTail>(), 2)
				.AddTile(TileID.CookingPots)
				.Register();

			Recipe.Create(ItemID.Sashimi)
				.AddIngredient(ModContent.ItemType<Items.Fishing.FungiEel>())
				.AddTile(TileID.WorkBenches)
				.Register();

			Recipe.Create(ItemID.Flare)
				.AddIngredient(ItemID.BlueFlare)
				.AddTile(TileID.DyeVat)
				.Register();

			Recipe.Create(ItemID.BlueFlare)
				.AddIngredient(ItemID.Flare)
				.AddTile(TileID.DyeVat)
				.Register();

			if (ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs && !ModLoader.TryGetMod("Fargowiltas", out Mod _))
			{
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtHarpy>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtHellTrader>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtIntTrav>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtSnugget>())
					.AddTile(TileID.MeatGrinder)
					.Register();
			}
		}

		public static RecipeGroup DefendersGear;
		public static RecipeGroup CopperBars;
		public static RecipeGroup SilverBars;
		public static RecipeGroup GoldBars;
		public static RecipeGroup EvilBars;
		public static RecipeGroup HoneyBalloons;
		public static RecipeGroup FartBalloons;
		public static RecipeGroup SharkronBalloons;
		public static RecipeGroup Counterweights;
		public static RecipeGroup Flares;
		public const string DefendersGearKey = "RijamsMod:DefendersGear";
		public const string CopperBarsKey = "RijamsMod:CopperBars";
		public const string SilverBarsKey = "RijamsMod:SilverBars";
		public const string GoldBarsKey = "RijamsMod:GoldBars";
		public const string EvilBarsKey = "RijamsMod:EvilBars";
		public const string HoneyBalloonsKey = "RijamsMod:HoneyBalloons";
		public const string FartBalloonsKey = "RijamsMod:FartBalloons";
		public const string SharkronBalloonsKey = "RijamsMod:SharkronBalloons";
		public const string CounterweightsKey = "RijamsMod:Counterweights";
		public const string FlaresKey = "RijamsMod:Flares";

		public override void Unload()
		{
			DefendersGear = null;
			CopperBars = null;
			SilverBars = null;
			GoldBars = null;
			EvilBars = null;
			HoneyBalloons = null;
			FartBalloons = null;
			SharkronBalloons = null;
			Counterweights = null;
			Flares = null;
		}

		public override void AddRecipeGroups()
		{
			DefendersGear = RecipeGroup.Register(
				DefendersGearKey,
				"OOA Sentry accessories",
					ItemID.ApprenticeScarf,
					ItemID.MonkBelt,
					ItemID.HuntressBuckler,
					ItemID.SquireShield
			);
			CopperBars = RecipeGroup.Register(
				CopperBarsKey,
				"Copper Bar",
					ItemID.CopperBar,
					ItemID.TinBar
			);
			SilverBars = RecipeGroup.Register(
				SilverBarsKey,
				"Silver Bar",
					ItemID.SilverBar,
					ItemID.TungstenBar
			);
			GoldBars = RecipeGroup.Register(
				GoldBarsKey,
				"Gold Bar",
					ItemID.GoldBar,
					ItemID.PlatinumBar
			);
			EvilBars = RecipeGroup.Register(
				EvilBarsKey,
				"Evil Bar",
					ItemID.DemoniteBar,
					ItemID.CrimtaneBar
			);
			HoneyBalloons = RecipeGroup.Register(
				HoneyBalloonsKey,
				"Honey Balloon",
					ItemID.HoneyBalloon,
					ItemID.BalloonHorseshoeHoney
			);
			FartBalloons = RecipeGroup.Register(
				FartBalloonsKey,
				"Fart Balloon",
					ItemID.FartInABalloon,
					ItemID.BalloonHorseshoeFart
			);
			SharkronBalloons = RecipeGroup.Register(
				SharkronBalloonsKey,
				"Sharkron Balloon",
					ItemID.SharkronBalloon,
					ItemID.BalloonHorseshoeSharkron
			);
			SharkronBalloons = RecipeGroup.Register(
				SharkronBalloonsKey,
				"Sharkron Balloon",
					ItemID.SharkronBalloon,
					ItemID.BalloonHorseshoeSharkron
			);
			Counterweights = RecipeGroup.Register(
				CounterweightsKey,
				"Counterweight",
					ItemID.BlackCounterweight,
					ItemID.BlueCounterweight,
					ItemID.GreenCounterweight,
					ItemID.PurpleCounterweight,
					ItemID.RedCounterweight,
					ItemID.YellowCounterweight
			);
			Flares = RecipeGroup.Register(
				FlaresKey,
				Language.GetTextValue("ItemName.Flare"),
					ItemID.Flare,
					ItemID.BlueFlare
			);

			RecipeGroups.Fruit.ValidItems.Add(ItemID.BlueBerries);

			RecipeGroup fargosMutantAnyCaughtNPC = RecipeGroup.recipeGroups.Where(rg => rg.Value.Key == "Fargowiltas:AnyCaughtNPC").FirstOrDefault().Value;
			if (fargosMutantAnyCaughtNPC is not null)
			{
				fargosMutantAnyCaughtNPC.ValidItems.Add(ModContent.ItemType<CaughtHarpy>());
				fargosMutantAnyCaughtNPC.ValidItems.Add(ModContent.ItemType<CaughtHellTrader>());
				fargosMutantAnyCaughtNPC.ValidItems.Add(ModContent.ItemType<CaughtIntTrav>());
				fargosMutantAnyCaughtNPC.ValidItems.Add(ModContent.ItemType<CaughtSnugget>());
			}

			/*
			if (RecipeGroup.recipeGroupIDs.TryGetValue("Fargowiltas:AnyCaughtNPC", out int anyCaughtNPCIndex))
			{
				RecipeGroup fargosAnyCaughtNPCGroup = RecipeGroup.recipeGroups[anyCaughtNPCIndex];
				fargosAnyCaughtNPCGroup.ValidItems.Add(ModContent.ItemType<CaughtHarpy>());
				fargosAnyCaughtNPCGroup.ValidItems.Add(ModContent.ItemType<CaughtHellTrader>());
				fargosAnyCaughtNPCGroup.ValidItems.Add(ModContent.ItemType<CaughtIntTrav>());
				fargosAnyCaughtNPCGroup.ValidItems.Add(ModContent.ItemType<CaughtSnugget>());
			}
			*/
		}
	}
}