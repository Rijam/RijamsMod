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
			// DisplayName.SetDefault("Carrot Nose");
			// Tooltip.SetDefault("Vanity Accessory");
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Dropped by The Frost Legion]"]);
			ItemID.Sets.CanGetPrefixes[Type] = false;
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.LightRed; //4
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.vanity = true;
			Item.accessory = true;
		}
	}	
}