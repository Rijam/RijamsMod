using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	[AutoloadEquip(EquipType.Neck)]
	public class LegionScarf : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Legion Scarf");
			Tooltip.SetDefault("+5% increased damage\nWhile in the Snow biome or Space:\n +9% increased damage");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Dropped by Mister Stabby & Snow Balla]");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 26;
			item.rare = ItemRarityID.LightRed; //4
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.ZoneSnow || player.ZoneSkyHeight) //if Snow biome or Space Layer
			{
				player.allDamage += 0.09f;
			}
			else
			{
				player.allDamage += 0.05f;
			}
		}
	}	
}