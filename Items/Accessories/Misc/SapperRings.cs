using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RijamsMod.Items.Armor.Vanity.IntTrav;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	// Buffs given in RijamsModNPCs

	[AutoloadEquip(EquipType.HandsOn)]
	public class LifeSapperRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Life Sapper Ring");
			// Tooltip.SetDefault("Gain a little bit of life after slaying an enemy\nScales with max health");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Hell Trader]", null, null });
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ManaSapperRing>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 28;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().lifeSapperRing = true;
		}
	}

	[AutoloadEquip(EquipType.HandsOn)]
	public class ManaSapperRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mana Sapper Ring");
			// Tooltip.SetDefault("Gain a little bit of mana after slaying an enemy\nScales with max mana");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Hell Trader]", null, null });
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<LifeSapperRing>(); // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 28;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().manaSapperRing = true;
		}
	}
}