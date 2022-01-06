using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Dyes
{
    public class YellaDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yella Dye");
            Tooltip.SetDefault("'It may or may not be a color'");
        }
        public override void SetDefaults()
        {
            byte dye = item.dye;
            item.CloneDefaults(ItemID.ShiftingSandsDye);
            item.dye = dye;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}