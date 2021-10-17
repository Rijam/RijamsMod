using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Vanity
{
	[AutoloadEquip(EquipType.Face)]
	public class CarrotNose : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Carrot Nose");
			Tooltip.SetDefault("Vanity Accessory" + "\n[c/403638:Dropped by The Frost Legion]");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 12;
			item.rare = ItemRarityID.LightRed; //4
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.vanity = true;
			item.accessory = true;
		}
	}	
}