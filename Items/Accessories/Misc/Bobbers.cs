using RijamsMod.Projectiles.Misc;
using System.Collections.Generic;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	public class CuriosityLure : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Additional reward from]", "[c/474747:completing Angler quests]" });
		}

		public override void SetDefaults()
		{
			Item.DefaultToAccessory(20, 32);
			Item.SetShopValues(ItemRarityColor.Green2, Item.sellPrice(0, 1));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().curiosityLure = true;
			if (!hideVisual)
			{
				player.overrideFishingBobber = ModContent.ProjectileType<CuriosityLureProj>();
			}
		}
	}

	public class TrapBobber : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Can be crafted after]", "[c/474747:completing 1 Angler quest]" });
		}

		public override void SetDefaults()
		{
			Item.DefaultToAccessory(18, 26);
			Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice(0, 0, 50));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().trapBobber = true;
			if (!hideVisual)
			{
				player.overrideFishingBobber = ModContent.ProjectileType<TrapBobberProj>();
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(RijamsModRecipes.CopperBars)
				.AddIngredient(ItemID.Gel, 10)
				.AddCondition(Condition.AnglerQuestsFinishedOver(1))
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class SpinnerBobber : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Can be crafted after]", "[c/474747:completing 1 Angler quest]" });
		}

		public override void SetDefaults()
		{
			Item.DefaultToAccessory(14, 24);
			Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice(0, 0, 50));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().spinnerBobber = true;
			if (!hideVisual)
			{
				player.overrideFishingBobber = ModContent.ProjectileType<SpinnerBobberProj>();
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(RecipeGroups.IronBar, 2)
				.AddIngredient(ItemID.Silk)
				.AddCondition(Condition.AnglerQuestsFinishedOver(1))
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}