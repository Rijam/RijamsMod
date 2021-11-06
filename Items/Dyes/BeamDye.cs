using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Dyes
{
    public class BeamDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beam Dye");
        }
        public override void SetDefaults()
        {
            byte dye = item.dye;
            item.CloneDefaults(ItemID.ShiftingSandsDye);
            item.dye = dye;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }
    }
    public class OrangeBeamDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orange Beam Dye");
        }
        public override void SetDefaults()
        {
            byte dye = item.dye;
            item.CloneDefaults(ItemID.ShiftingSandsDye);
            item.dye = dye;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "BeamDye", 1);
            recipe.AddIngredient(mod, "SunEssence", 5);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}