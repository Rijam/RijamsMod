using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class MorphasRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by King Slime]" });
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().criticalHitAdditionalDamage += 0.36f;
		}
	}
}