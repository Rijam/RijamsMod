using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Movement
{
	public class GuideToProperFlightTechniques : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Guide to Proper Flight Techniques");
			Tooltip.SetDefault("Only works while Wings are equipped\n+1 Second Flight Time\nIncreased vertical and horizontal flight speed\nIncreased jump height\nIncreased movement speed");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Harpy]", "[c/474747:After defeating Mechanical Bosses]", null } );
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 32;
			Item.rare = ItemRarityID.LightPurple; //6
			Item.value = Item.buyPrice(0, 10, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().guideToProperFlightTechniques = true;
			//See RijamsModPlayer PostUpdateEquips() and GlobalAccessoryItem for the effects of the accessory
		}
	}	
}