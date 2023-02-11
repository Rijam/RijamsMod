using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	// Buff given in RijamsModNPCs

	[AutoloadEquip(EquipType.HandsOn)]
	public class WarriorRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Warrior Ring");
			// Tooltip.SetDefault("Occasionally infuses the wearer with\n\"Warrior Energy\" after slaying an enemy\nWarrior Energy gives:\n  +10% damage\n  +5% attack speed");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().warriorRing = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("IronBar", 10)
				.AddIngredient(ItemID.Obsidian, 25)
				.AddIngredient(ItemID.FrostCore, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}