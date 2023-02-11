using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	[AutoloadEquip(EquipType.Back, EquipType.Waist)]
	public class MasterBuilderPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Master Builder Pack");
			/* Tooltip.SetDefault("+5 block placement & tool range\n+25% mining speed\nIncreases block & wall placement speed\nAutomatically paints placed objects\n" +
				"Creates measurement lines on screen for block placement\nIncreases pickup range for items\nHold Up to reach higher\nEnables Echo Sight, showing hidden blocks (Hide to disable)"); */
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.equippedAnyWallSpeedAcc = true;
			player.equippedAnyTileSpeedAcc = true;
			player.autoPaint = true;
			player.portableStoolInfo.SetStats(26, 26, 26);
			//player.equippedAnyTileRangeAcc = true;
			//player.blockRange++;
			if (player.whoAmI == Main.myPlayer)
			{
				Player.tileRangeX += 5;
				Player.tileRangeY += 5;
			}
			player.treasureMagnet = true;
			player.rulerGrid = true;
			//player.pickSpeed -= 0.25f;
			player.chiselSpeed = true;
			if (!hideVisual)
			{
				player.CanSeeInvisibleBlocks = true;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HandOfCreation)
				.AddIngredient(ItemID.Toolbelt)
				.AddIngredient(ItemID.Toolbox)
				.AddIngredient(ItemID.LaserRuler) // Mechanical Ruler
				.AddIngredient(ItemID.SpectreGoggles)
				.AddIngredient(ItemID.SpectreBar)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}