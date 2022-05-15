using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Consumables
{
	public class StrangeRoll : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Roll");
			Tooltip.SetDefault("Minor improvements to all stats\n'A strange food from an unknown place'\n'What's inside?'");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Sold by Traveling Merchant]");
		}
		/*public override bool CanUseItem(Player player)
		{
			return !(player.HasBuff(BuffID.WellFed)) ; //can't eat if the player has Well Fed
		}*/

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.Yellow;
			item.value = 10000;
			item.consumable = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useTurn = true;
			item.UseSound = SoundID.Item2;
			item.buffType = BuffID.WellFed; //Specify an existing buff to be applied when used.
			item.buffTime = 72000; //20 minutes
		}
	}
	public class RyeJam : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rye Jam");
			Tooltip.SetDefault("Extreme improvements to all stats\n'Wait, how do you pronounce it?'");
		}
		/*public override bool CanUseItem(Player player)
		{
			return !(player.HasBuff(BuffID.WellFed)) ; //can't eat if the player has Well Fed
		}*/

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.maxStack = 30;
			item.rare = ItemRarityID.Cyan;
			item.value = 200000;
			item.consumable = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useTurn = true;
			item.UseSound = SoundID.Item2;
			item.buffType = ModContent.BuffType<Buffs.ExceptionalFeast>(); //Specify an existing buff to be applied when used.
			item.buffTime = 18000; //5 minutes
		}
		//Tool tip set in GlobalItems
	}
	public class ReefCola : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reef-Cola");
			Tooltip.SetDefault("Move freely in liquids\n'Taste the ocean, with Reef-Cola.'");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Sold by Interstellar Traveler]");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 28;
			item.maxStack = 30;
			item.rare = ItemRarityID.Blue;
			item.value = 4000;
			item.consumable = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.buffType = ModContent.BuffType<Buffs.SwimBoostBuff>();
			item.buffTime = 3600; //1 minute
		}
	}
}