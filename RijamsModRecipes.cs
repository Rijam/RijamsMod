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
            /*
             * Preview
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

            Recipe.Create(ItemID.SlimeStaff)
                .AddIngredient(ItemID.Gel, 500)
                .AddIngredient(ItemID.Wood, 5)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Anvils)
                .Register();

            Recipe.Create(ItemID.ThornsPotion)
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ItemID.Deathweed, 1)
                .AddIngredient(ItemID.Cactus, 1)
                .AddIngredient(ModContent.ItemType<Items.Materials.CrawlerChelicera>(), 1)
                .AddIngredient(ItemID.Stinger, 1)
                .AddTile(TileID.Bottles)
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
                .AddIngredient(ModContent.ItemType<Items.Weapons.HotStick>(), 1)
                .AddIngredient(ItemID.Torch, 2)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Anvils)
                .Register();
            Recipe.Create(ItemID.BlueBerries)
                .AddIngredient(ModContent.ItemType<Items.Consumables.FreshBlueberry>(), 3)
                .Register();
            */
            Mod.CreateRecipe(ItemID.SnowGlobe)
                .AddIngredient(ItemID.Glass, 5)
                .AddIngredient(ItemID.SnowBlock, 5)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            Mod.CreateRecipe(ItemID.BandofStarpower)
                .AddIngredient(ItemID.PanicNecklace, 1)
                .AddIngredient(ItemID.VilePowder, 5)
                .AddTile(TileID.DemonAltar)
                .Register();

            Mod.CreateRecipe(ItemID.PanicNecklace)
                .AddIngredient(ItemID.BandofStarpower, 1)
                .AddIngredient(ItemID.ViciousPowder, 5)
                .AddTile(TileID.DemonAltar)
                .Register();

            Mod.CreateRecipe(ItemID.PutridScent)
                .AddIngredient(ItemID.FleshKnuckles, 1)
                .AddIngredient(ItemID.VilePowder, 5)
                .AddTile(TileID.DemonAltar)
                .Register();

            Mod.CreateRecipe(ItemID.FleshKnuckles)
                .AddIngredient(ItemID.PutridScent, 1)
                .AddIngredient(ItemID.ViciousPowder, 5)
                .AddTile(TileID.DemonAltar)
                .Register();

            Mod.CreateRecipe(ItemID.SlimeStaff)
                .AddIngredient(ItemID.Gel, 500)
                .AddIngredient(ItemID.Wood, 5)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Anvils)
                .Register();

            Mod.CreateRecipe(ItemID.ThornsPotion)
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ItemID.Deathweed, 1)
                .AddIngredient(ItemID.Cactus, 1)
                .AddIngredient(ModContent.ItemType<Items.Materials.CrawlerChelicera>(), 1)
                .AddIngredient(ItemID.Stinger, 1)
                .AddTile(TileID.Bottles)
                .Register();

            Mod.CreateRecipe(ItemID.RainbowString)
                .AddIngredient(ItemID.WhiteString, 1)
                .AddIngredient(ItemID.LivingRainbowDye, 1)
                .AddTile(TileID.DyeVat)
                .Register();

            Mod.CreateRecipe(ItemID.RainbowString)
                .AddIngredient(ItemID.WhiteString, 1)
                .AddIngredient(ItemID.RainbowHairDye, 1)
                .AddTile(TileID.DyeVat)
                .Register();

            Mod.CreateRecipe(ItemID.SunplateBlock)
                .AddIngredient(ModContent.ItemType<Items.Placeable.SunplatePillarBlock>(), 1)
                .Register();

            Mod.CreateRecipe(ItemID.WandofSparking)
                .AddIngredient(ModContent.ItemType<Items.Weapons.HotStick>(), 1)
                .AddIngredient(ItemID.Torch, 2)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Anvils)
                .Register();
            Mod.CreateRecipe(ItemID.BlueBerries)
                .AddIngredient(ModContent.ItemType<Items.Consumables.FreshBlueberry>(), 3)
                .Register();
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup group = new(() => Language.GetTextValue("LegacyMisc.37") + " OOA Sentry accessories", new int[]
            {
                ItemID.ApprenticeScarf,
                ItemID.MonkBelt,
                ItemID.HuntressBuckler,
                ItemID.SquireShield
            });
            RecipeGroup.RegisterGroup("RijamsMod:Defender's Gear", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Copper Bar", new int[]
            {
                ItemID.CopperBar,
                ItemID.TinBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:CopperBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver Bar", new int[]
            {
                ItemID.SilverBar,
                ItemID.TungstenBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:SilverBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:GoldBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Bar", new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:EvilBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cobalt Bar", new int[]
            {
                ItemID.CobaltBar,
                ItemID.PalladiumBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:CobaltBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Mythril Bar", new int[]
            {
                ItemID.MythrilBar,
                ItemID.OrichalcumBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:MythrilBars", group);
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Adamantite Bar", new int[]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar
            });
            RecipeGroup.RegisterGroup("RijamsMod:AdamantiteBars", group);

            if (RecipeGroup.recipeGroupIDs.ContainsKey("Fruit"))
            {
                int index = RecipeGroup.recipeGroupIDs["Fruit"];
                RecipeGroup vanillaGroup = RecipeGroup.recipeGroups[index];
                vanillaGroup.ValidItems.Add(ItemID.BlueBerries);
            }
        }
    }
}