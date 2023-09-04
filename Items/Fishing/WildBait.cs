using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Fishing
{
	public class WildBait : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Requires 3 Fishing Quests]", "[c/474747:to be completed to craft]" });
			Item.ResearchUnlockCount = 5;
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.rare = ItemRarityID.White;
			Item.value = 400;
			Item.maxStack = Item.CommonMaxStack;
			Item.bait = 25;
			Item.consumable = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Hay, 2)
				.AddIngredient(ItemID.Gel, 1)
				.AddIngredient(ItemID.RottenChunk, 1)
				.AddCondition(Condition.AnglerQuestsFinishedOver(3))
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.Hay, 2)
				.AddIngredient(ItemID.Gel, 1)
				.AddIngredient(ItemID.Vertebrae, 1)
				.AddCondition(Condition.AnglerQuestsFinishedOver(3))
				.Register();
		}

		// Double fish chance in HornetTail.cs
	}
}