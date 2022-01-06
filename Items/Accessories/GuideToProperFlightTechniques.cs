using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	public class GuideToProperFlightTechniques : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Guide to Proper Flight Techniques");
			Tooltip.SetDefault("Only works while Wings are equipped\n+1 Second Flight Time\nIncreased vertical and horizontal flight speed\nIncreased jump height\nIncreased movement speed");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Sold by Harpy]");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 32;
			item.rare = ItemRarityID.LightPurple; //6
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().guideToProperFlightTechniques = true;
			//See RijamsModPlayer PostUpdateEquips() and GlobalAccessoryItem for the effects of the accessory
		}
	}	
}