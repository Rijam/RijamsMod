using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Fishing
{
	public class HornetTail : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanBePlacedOnWeaponRacks[Type] = true;
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Fished in Honey underground]" });
			Item.ResearchUnlockCount = 3;
		}
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.rare = ItemRarityID.Blue;
			Item.value = 4000;
			Item.maxStack = Item.CommonMaxStack;
		}
	}
	public class FungiEel : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanBePlacedOnWeaponRacks[Type] = true;
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Fished in underground Glowing Mushroom]" });
			Item.ResearchUnlockCount = 3;
		}
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.rare = ItemRarityID.Blue;
			Item.value = 2750;
			Item.maxStack = Item.CommonMaxStack;
		}
	}
}