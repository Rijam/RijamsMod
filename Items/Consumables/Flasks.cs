using Microsoft.Xna.Framework;
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
            ItemID.Sets.DrinkParticleColors[Item.type] = new Color[3]
            {
                new Color(255, 255, 0),
                new Color(193, 43, 43),
                new Color(181, 115, 20)
            };
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
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(silver: 10);
            Item.buffType = ModContent.BuffType<Buffs.ImbueSulfuricAcid>();
            Item.buffTime = 72000; //20 minutes
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 5)
                .AddTile(TileID.ImbuingStation)
                .Register();
        }
    }
}
