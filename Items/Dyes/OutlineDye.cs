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
	public class OutlineDye : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Beam Dye");
			// ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Dye Trader]", "[c/474747:when Interstellar Traveler is present]" });
			// Avoid loading assets on dedicated servers. They don't use graphics cards.
			if (!Main.dedServ)
			{
				// The following code creates an effect (shader) reference and associates it with this item's type Id.
				GameShaders.Armor.BindShader
				(
					Item.type,
					// new ArmorShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/BeamShader", AssetRequestMode.ImmediateLoad).Value), "BeamDyePass") // Be sure to update the effect path and pass name here.
					new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Outline"), "OutlinePass") // Be sure to update the effect path and pass name here.
				).UseColor(0.9f, 0.9f, 1f);
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
}