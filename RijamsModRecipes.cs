using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RijamsMod.Items;

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

		public const string DefendersGear = "RijamsMod:DefendersGear";
		public const string CopperBars = "RijamsMod:CopperBars";
		public const string SilverBars = "RijamsMod:SilverBars";
		public const string GoldBars = "RijamsMod:GoldBars";
		public const string EvilBars = "RijamsMod:EvilBars";
		public const string CobaltBars = "RijamsMod:CobaltBars";
		public const string MythrilBars = "RijamsMod:MythrilBars";
		public const string AdamantiteBars = "RijamsMod:AdamantiteBars";
		public const string HoneyBalloons = "RijamsMod:HoneyBalloons";
		public const string FartBalloons = "RijamsMod:FartBalloons";
		public const string SharkronBalloons = "RijamsMod:SharkronBalloons";
		public const string Counterweights = "RijamsMod:Counterweights";
		public const string Flares = "RijamsMod:Flares";

		public override void AddRecipeGroups()
		{
			RecipeGroup group = new(() => Language.GetTextValue("LegacyMisc.37") + " OOA Sentry accessories",
			[
				ItemID.ApprenticeScarf,
				ItemID.MonkBelt,
				ItemID.HuntressBuckler,
				ItemID.SquireShield
			]);
			RecipeGroup.RegisterGroup(DefendersGear, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Copper Bar",
			[
				ItemID.CopperBar,
				ItemID.TinBar
			]);
			RecipeGroup.RegisterGroup(CopperBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver Bar",
			[
				ItemID.SilverBar,
				ItemID.TungstenBar
			]);
			RecipeGroup.RegisterGroup(SilverBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold Bar",
			[
				ItemID.GoldBar,
				ItemID.PlatinumBar
			]);
			RecipeGroup.RegisterGroup(GoldBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Bar",
			[
				ItemID.DemoniteBar,
				ItemID.CrimtaneBar
			]);
			RecipeGroup.RegisterGroup(EvilBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cobalt Bar",
			[
				ItemID.CobaltBar,
				ItemID.PalladiumBar
			]);
			RecipeGroup.RegisterGroup(CobaltBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Mythril Bar",
			[
				ItemID.MythrilBar,
				ItemID.OrichalcumBar
			]);
			RecipeGroup.RegisterGroup(MythrilBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Adamantite Bar",
			[
				ItemID.AdamantiteBar,
				ItemID.TitaniumBar
			]);
			RecipeGroup.RegisterGroup(AdamantiteBars, group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Honey Balloon",
			[
				ItemID.HoneyBalloon,
				ItemID.BalloonHorseshoeHoney
			]);
			RecipeGroup.RegisterGroup(HoneyBalloons, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Fart Balloon",
			[
				ItemID.FartInABalloon,
				ItemID.BalloonHorseshoeFart
			]);
			RecipeGroup.RegisterGroup(FartBalloons, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Sharkron Balloon",
			[
				ItemID.SharkronBalloon,
				ItemID.BalloonHorseshoeSharkron
			]);
			RecipeGroup.RegisterGroup(SharkronBalloons, group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Counterweight",
			[
				ItemID.BlackCounterweight,
				ItemID.BlueCounterweight,
				ItemID.GreenCounterweight,
				ItemID.PurpleCounterweight,
				ItemID.RedCounterweight,
				ItemID.YellowCounterweight
			]);
			RecipeGroup.RegisterGroup(Counterweights, group);

			group = new RecipeGroup(() => Language.GetTextValue("ItemName.Flare"),
			[
				ItemID.Flare,
				ItemID.BlueFlare
			]);
			RecipeGroup.RegisterGroup(Flares, group);

			if (RecipeGroup.recipeGroupIDs.TryGetValue("Fruit", out int fruitIndex))
			{
				RecipeGroup vanillaGroup = RecipeGroup.recipeGroups[fruitIndex];
				vanillaGroup.ValidItems.Add(ItemID.BlueBerries);
			}

			if (RecipeGroup.recipeGroupIDs.TryGetValue("Fargowiltas:AnyCaughtNPC", out int anyCaughtNPCIndex))
			{
				RecipeGroup fargosAnyCaughtNPCGroup = RecipeGroup.recipeGroups[anyCaughtNPCIndex];
				fargosAnyCaughtNPCGroup.ValidItems.Add(ModContent.ItemType<CaughtHarpy>());
				fargosAnyCaughtNPCGroup.ValidItems.Add(ModContent.ItemType<CaughtHellTrader>());
				fargosAnyCaughtNPCGroup.ValidItems.Add(ModContent.ItemType<CaughtIntTrav>());
				fargosAnyCaughtNPCGroup.ValidItems.Add(ModContent.ItemType<CaughtSnugget>());
			}
		}
	}
}