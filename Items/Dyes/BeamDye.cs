using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Creative;

namespace RijamsMod.Items.Dyes
{
    public class BeamDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beam Dye");
            ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Dye Trader]", "[c/474747:when Interstellar Traveler is present]", null });
            // Avoid loading assets on dedicated servers. They don't use graphics cards.
            if (!Main.dedServ)
            {
                // The following code creates an effect (shader) reference and associates it with this item's type Id.
                GameShaders.Armor.BindShader
                (
                    Item.type,
                    new ArmorShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/BeamShader", AssetRequestMode.ImmediateLoad).Value), "BeamDyePass") // Be sure to update the effect path and pass name here.
                ).UseColor(0f, 1f, 2f);
            }
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            int dye = Item.dye;
            Item.CloneDefaults(ItemID.ShiftingSandsDye);
            Item.dye = dye;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }
    }
    public class OrangeBeamDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orange Beam Dye");
            // Avoid loading assets on dedicated servers. They don't use graphics cards.
            if (!Main.dedServ)
            {
                // The following code creates an effect (shader) reference and associates it with this item's type Id.
                GameShaders.Armor.BindShader
                (
                    Item.type,
                    new ArmorShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/BeamShader", AssetRequestMode.ImmediateLoad).Value), "BeamDyePass") // Be sure to update the effect path and pass name here.
                ).UseColor(2f, 1f, 0f);
            }
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            int dye = Item.dye;
            Item.CloneDefaults(ItemID.ShiftingSandsDye);
            Item.dye = dye;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BeamDye>(), 1)
                .AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 5)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }
}