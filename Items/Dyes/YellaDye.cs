using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace RijamsMod.Items.Dyes
{
	public class YellaDye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yella Dye");
			Tooltip.SetDefault("'It may or may not be a color'");
			// Avoid loading assets on dedicated servers. They don't use graphics cards.
			if (!Main.dedServ)
			{
				// The following code creates an effect (shader) reference and associates it with this item's type Id.
				GameShaders.Armor.BindShader
				(
					Item.type,
					new ArmorShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/YellaShader", AssetRequestMode.ImmediateLoad).Value), "YellaDyePass") // Be sure to update the effect path and pass name here.
				).UseColor(2f, 2f, 0f).UseSecondaryColor(0.6f, 0.3f, 0f);
			}
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}
		public override void SetDefaults()
		{
			int dye = Item.dye;
			Item.CloneDefaults(ItemID.ShiftingSandsDye);
			Item.dye = dye;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.DyeVat)
				.Register();
		}
	}
}