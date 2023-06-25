using RijamsMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Linq;

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

			Recipe.Create(ItemID.BandofStarpower)
				.AddIngredient(ItemID.PanicNecklace, 1)
				.AddIngredient(ItemID.VilePowder, 5)
				.AddTile(TileID.DemonAltar)
				.Register();

			Recipe.Create(ItemID.PanicNecklace)
				.AddIngredient(ItemID.BandofStarpower, 1)
				.AddIngredient(ItemID.ViciousPowder, 5)
				.AddTile(TileID.DemonAltar)
				.Register();

			Recipe.Create(ItemID.PutridScent)
				.AddIngredient(ItemID.FleshKnuckles, 1)
				.AddIngredient(ItemID.VilePowder, 5)
				.AddTile(TileID.DemonAltar)
				.Register();

			Recipe.Create(ItemID.FleshKnuckles)
				.AddIngredient(ItemID.PutridScent, 1)
				.AddIngredient(ItemID.ViciousPowder, 5)
				.AddTile(TileID.DemonAltar)
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

			Recipe.Create(ItemID.WandofSparking)
				.AddIngredient(ModContent.ItemType<Items.Weapons.Magic.HotStick>(), 1)
				.AddIngredient(ItemID.Torch, 2)
				.AddIngredient(ItemID.FallenStar, 1)
				.AddTile(TileID.Anvils)
				.Register();

			Recipe.Create(ItemID.BlueBerries)
				.AddIngredient(ModContent.ItemType<Items.Consumables.FreshBlueberry>(), 3)
				.Register();

			Recipe.Create(ItemID.SeafoodDinner)
				.AddIngredient(ModContent.ItemType<Items.Fishing.HornetTail>(), 2)
				.AddTile(TileID.CookingPots)
				.Register();

			if (ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs)
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
				Recipe.Create(ItemID.FleshBlock, 1)
					.AddIngredient(ModContent.ItemType<CaughtIntTrav>())
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

		public override void AddRecipeGroups()
		{
			RecipeGroup group = new(() => Language.GetTextValue("LegacyMisc.37") + " OOA Sentry accessories", new int[]
			{
				ItemID.ApprenticeScarf,
				ItemID.MonkBelt,
				ItemID.HuntressBuckler,
				ItemID.SquireShield
			});
			RecipeGroup.RegisterGroup(DefendersGear, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Copper Bar", new int[]
			{
				ItemID.CopperBar,
				ItemID.TinBar
			});
			RecipeGroup.RegisterGroup(CopperBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver Bar", new int[]
			{
				ItemID.SilverBar,
				ItemID.TungstenBar
			});
			RecipeGroup.RegisterGroup(SilverBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold Bar", new int[]
			{
				ItemID.GoldBar,
				ItemID.PlatinumBar
			});
			RecipeGroup.RegisterGroup(GoldBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Bar", new int[]
			{
				ItemID.DemoniteBar,
				ItemID.CrimtaneBar
			});
			RecipeGroup.RegisterGroup(EvilBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cobalt Bar", new int[]
			{
				ItemID.CobaltBar,
				ItemID.PalladiumBar
			});
			RecipeGroup.RegisterGroup(CobaltBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Mythril Bar", new int[]
			{
				ItemID.MythrilBar,
				ItemID.OrichalcumBar
			});
			RecipeGroup.RegisterGroup(MythrilBars, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Adamantite Bar", new int[]
			{
				ItemID.AdamantiteBar,
				ItemID.TitaniumBar
			});
			RecipeGroup.RegisterGroup(AdamantiteBars, group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Honey Balloon", new int[]
			{
				ItemID.HoneyBalloon,
				ItemID.BalloonHorseshoeHoney
			});
			RecipeGroup.RegisterGroup(HoneyBalloons, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Fart Balloon", new int[]
			{
				ItemID.FartInABalloon,
				ItemID.BalloonHorseshoeFart
			});
			RecipeGroup.RegisterGroup(FartBalloons, group);
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Sharkron Balloon", new int[]
			{
				ItemID.SharkronBalloon,
				ItemID.BalloonHorseshoeSharkron
			});
			RecipeGroup.RegisterGroup(SharkronBalloons, group);

			if (RecipeGroup.recipeGroupIDs.ContainsKey("Fruit"))
			{
				int index = RecipeGroup.recipeGroupIDs["Fruit"];
				RecipeGroup vanillaGroup = RecipeGroup.recipeGroups[index];
				vanillaGroup.ValidItems.Add(ItemID.BlueBerries);
			}
		}
	}
}