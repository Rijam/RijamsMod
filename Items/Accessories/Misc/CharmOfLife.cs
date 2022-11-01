using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class CharmOfLife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charm of Life");
			Tooltip.SetDefault("\n+25 Max Life\nProvides life regeneration\nReduces the cooldown of healing potions by 25%\nGain a little bit of life after slaying an enemy\n  Scales with max health");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 5, 50, 0);
			Item.lifeRegen = 2;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// Life regen set in SetDefaults ^
			player.pStone = true;
			player.statLifeMax2 += 25;
			player.GetModPlayer<RijamsModPlayer>().lifeSapperRing = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CharmofMyths, 1)
				.AddIngredient(ModContent.ItemType<LifeSapperRing>(), 1)
				.AddIngredient(ItemID.LifeCrystal, 1)
				.AddIngredient(ItemID.LifeFruit, 1)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}