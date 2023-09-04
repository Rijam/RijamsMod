using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	[AutoloadEquip(EquipType.Neck)]
	public class LegionScarf : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Legion Scarf");
			// Tooltip.SetDefault("+5% increased damage\nWhile in the Snow biome or Space:\n +9% increased damage");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by Mister Stabby & Snow Balla]" });
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.rare = ItemRarityID.LightRed; //4
			Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.ZoneSnow || player.ZoneSkyHeight) //if Snow biome or Space Layer
			{
				player.GetDamage(DamageClass.Generic) += 0.09f;
			}
			else
			{
				player.GetDamage(DamageClass.Generic) += 0.05f;
			}
		}
	}	
}