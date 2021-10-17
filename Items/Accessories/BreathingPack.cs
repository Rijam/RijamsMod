using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RijamsMod.Items.Accessories
{
	[AutoloadEquip(EquipType.Back)]
	public class BreathingPack : ModItem
	{

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Refills the player's air supply two seconds after it reaches zero" + "\n[c/403638:Sold by Interstellar Traveler]");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 34;
			item.value = 50000;
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().breathingPack = true;
		}
	}
}