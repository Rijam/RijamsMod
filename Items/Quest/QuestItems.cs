using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Quest
{
	public class QuestTrackerIncomplete : ModItem
	{
		public override string Texture => "Terraria/Images/UI/Cursor_2";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Keep searching!");
			Tooltip.SetDefault("[c/403638:UI Item]");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 22;
			Item.height = 24;
			Item.value = 0;
			Item.rare = ItemRarityID.White;
		}
	}
	public class QuestTrackerComplete : ModItem
	{
		public override string Texture => "Terraria/Images/UI/Cursor_3";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You have completed all of the quests!");
			Tooltip.SetDefault("[c/403638:UI Item]");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 22;
			Item.height = 24;
			Item.value = 0;
			Item.rare = ItemRarityID.White;
		}
	}
	public class OddDevice : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//Tooltip.SetDefault("");
		}

		public override void SetDefaults() 
		{
			Item.maxStack = 99;
			Item.width = 16;
			Item.height = 34;
			Item.value = 100;
			Item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int index = 1;
			if (Item.favorited)
			{
				index += 2;
			}
			if (RijamsModWorld.intTravQuestOddDevice == false)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "IncompleteQuest", "'It looks like something is tracking it...'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "IncompleteQuest", "[c/a7a7a7: Quest item]"));
			}
			if (RijamsModWorld.intTravQuestOddDevice == true)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "CompleteQuest", "'The Interstellar Traveler was tracking it.'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
			}
		}
		public override void AddRecipes() 
		{
			CreateRecipe()
				.AddRecipeGroup("IronBar", 5)
				.AddIngredient(ItemID.ShadowScale, 2)
				.AddIngredient(ItemID.Glass, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
			
			CreateRecipe()
				.AddRecipeGroup("IronBar", 5)
				.AddIngredient(ItemID.TissueSample, 2)
				.AddIngredient(ItemID.Glass, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	public class BlankDisplay : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 32;
			Item.height = 24;
			Item.value = 100;
			Item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int index = 1;
			if (Item.favorited)
			{
				index += 2;
			}
			if (RijamsModWorld.intTravQuestBlankDisplay == false)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "IncompleteQuest", "'It looks like it could be programmed...'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "IncompleteQuest", "[c/a7a7a7: Quest item]"));
			}
			if (RijamsModWorld.intTravQuestBlankDisplay == true)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "CompleteQuest", "'The Interstellar Traveler is now selling Information Displays.'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("RijamsMod:SilverBars", 5)
				.AddIngredient(ItemID.Lens, 1)
				.AddIngredient(ItemID.Glass, 2)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
	public class TeleportationCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Chaos Elementals and Enchanted Swords]", null, null });
		}

		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 28;
			Item.height = 40;
			Item.value = 50000;
			Item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int index = 1;
			if (Item.favorited)
			{
				index += 2;
			}
			if (RijamsModWorld.intTravQuestTPCore == false)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "IncompleteQuest", "'It looks like it could be used to teleport...'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "IncompleteQuest", "[c/a7a7a7: Quest item]"));
			}
			if (RijamsModWorld.intTravQuestTPCore == true)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "CompleteQuest", "'The Interstellar Traveler is now selling the Rod of Discord.'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
			}
		}
	}
	public class BreadAndJelly : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 42;
			Item.height = 24;
			Item.value = 5000;
			Item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int index = 1;
			if (Item.favorited)
			{
				index += 2;
			}
			if (RijamsModWorld.intTravQuestBreadAndJelly == false)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "IncompleteQuest", "'It looks tasty...'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "IncompleteQuest", "[c/a7a7a7: Quest item?]"));
			}
			if (RijamsModWorld.intTravQuestBreadAndJelly == true)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "CompleteQuest", "'The Interstellar Traveler is now selling Rye Jam.'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
			}
		}
	}
	public class MagicOxygenizer : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 42;
			Item.height = 24;
			Item.value = 10000;
			Item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int index = 1;
			if (Item.favorited)
			{
				index += 2;
			}
			if (RijamsModWorld.intTravQuestMagicOxygenizer == false)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "IncompleteQuest", "'It can create a lot of oxygen...'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "IncompleteQuest", "[c/a7a7a7: Quest item]"));
			}
			if (RijamsModWorld.intTravQuestMagicOxygenizer == true)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "CompleteQuest", "'The Interstellar Traveler is now selling the Breathing Pack.'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HallowedBar, 3)
				.AddIngredient(ItemID.CloudinaBottle, 1)
				.AddIngredient(ItemID.PixieDust, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	public class PrimeThruster : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 60;
			Item.height = 36;
			Item.value = 15000;
			Item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int index = 1;
			if (Item.favorited)
			{
				index += 2;
			}
			if (RijamsModWorld.intTravQuestPrimeThruster == false)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "IncompleteQuest", "'It can produce a lot of thrust...'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "IncompleteQuest", "[c/a7a7a7: Quest item]"));
			}
			if (RijamsModWorld.intTravQuestPrimeThruster == true)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "CompleteQuest", "'The Interstellar Traveler is now selling the Rocket Booster.'"));
				tooltips.Insert(index + 1, new TooltipLine(Mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.RocketI, 10)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ItemID.SoulofFright, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}