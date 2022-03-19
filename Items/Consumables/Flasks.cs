using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Consumables
{
	public class FlaskOfSulfuricAcid : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flask of Sulfuric Acid");
            Tooltip.SetDefault("Melee attacks inflict enemies with Sulfuric Acid");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 28;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 15;
            item.useTime = 15;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = ItemRarityID.LightRed;
            item.value = Item.sellPrice(silver: 10);
            item.buffType = ModContent.BuffType<Buffs.ImbueSulfuricAcid>();
            item.buffTime = 72000; //20 minutes
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 5);
            recipe.AddTile(TileID.ImbuingStation);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
