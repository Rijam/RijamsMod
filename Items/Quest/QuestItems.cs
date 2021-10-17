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
		public override string Texture => "Terraria/UI/Cursor_2";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Keep searching!");
			Tooltip.SetDefault("[c/403638:UI Item]");
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 22;
			item.height = 24;
			item.value = 0;
			item.rare = ItemRarityID.White;
		}
	}
	public class QuestTrackerComplete : ModItem
	{
		public override string Texture => "Terraria/UI/Cursor_3";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You have completed all of the quests!");
			Tooltip.SetDefault("[c/403638:UI Item]");
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 22;
			item.height = 24;
			item.value = 0;
			item.rare = ItemRarityID.White;
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
			item.maxStack = 99;
			item.width = 16;
			item.height = 34;
			item.value = 100;
			item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (RijamsModWorld.intTravQuestOddDevice == false)
			{
				tooltips.Add(new TooltipLine(mod, "IncompleteQuest", "'It looks like something is tracking it...'"));
				tooltips.Add(new TooltipLine(mod, "IncompleteQuest", "[c/a7a7a7: Quest item]"));
			}
			if (RijamsModWorld.intTravQuestOddDevice == true)
			{
				tooltips.Add(new TooltipLine(mod, "CompleteQuest", "'The Interstellar Traveler was tracking it.'"));
				tooltips.Add(new TooltipLine(mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
			}
		}
		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient(ItemID.ShadowScale, 2);
			recipe.AddIngredient(ItemID.Glass, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient(ItemID.TissueSample, 2);
			recipe.AddIngredient(ItemID.Glass, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
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
			item.maxStack = 99;
			item.width = 32;
			item.height = 24;
			item.value = 100;
			item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (RijamsModWorld.intTravQuestBlankDisplay == false)
			{
				tooltips.Add(new TooltipLine(mod, "IncompleteQuest", "'It looks like it could be programmed...'"));
				tooltips.Add(new TooltipLine(mod, "IncompleteQuest", "[c/a7a7a7: Quest item]"));
			}
			if (RijamsModWorld.intTravQuestBlankDisplay == true)
			{
				tooltips.Add(new TooltipLine(mod, "CompleteQuest", "'The Interstellar Traveler is now selling Information Displays.'"));
				tooltips.Add(new TooltipLine(mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("RijamsMod:SilverBars", 5);
			recipe.AddIngredient(ItemID.Lens, 1);
			recipe.AddIngredient(ItemID.Glass, 2);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class TeleportationCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 28;
			item.height = 40;
			item.value = 50000;
			item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (RijamsModWorld.intTravQuestTPCore == false)
			{
				tooltips.Add(new TooltipLine(mod, "IncompleteQuest", "'It looks like it could be used to teleport...'"));
				tooltips.Add(new TooltipLine(mod, "IncompleteQuest", "[c/a7a7a7: Quest item]"));
			}
			if (RijamsModWorld.intTravQuestTPCore == true)
			{
				tooltips.Add(new TooltipLine(mod, "CompleteQuest", "'The Interstellar Traveler is now selling the Rod of Discord.'"));
				tooltips.Add(new TooltipLine(mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
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
			item.maxStack = 99;
			item.width = 42;
			item.height = 24;
			item.value = 5000;
			item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (RijamsModWorld.intTravQuestRyeJam == false)
			{
				tooltips.Add(new TooltipLine(mod, "IncompleteQuest", "'It looks tasty...'"));
				tooltips.Add(new TooltipLine(mod, "IncompleteQuest", "[c/a7a7a7: Quest item?]"));
			}
			if (RijamsModWorld.intTravQuestRyeJam == true)
			{
				tooltips.Add(new TooltipLine(mod, "CompleteQuest", "'The Interstellar Traveler is now selling Rye Jam.'"));
				tooltips.Add(new TooltipLine(mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
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
			item.maxStack = 99;
			item.width = 42;
			item.height = 24;
			item.value = 10000;
			item.rare = ItemRarityID.Quest;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (RijamsModWorld.intTravQuestMagicOxygenizer == false)
			{
				tooltips.Add(new TooltipLine(mod, "IncompleteQuest", "'It can create a lot of oxygen...'"));
				tooltips.Add(new TooltipLine(mod, "IncompleteQuest", "[c/a7a7a7: Quest item]"));
			}
			if (RijamsModWorld.intTravQuestMagicOxygenizer == true)
			{
				tooltips.Add(new TooltipLine(mod, "CompleteQuest", "'The Interstellar Traveler is now selling the Breathing Pack.'"));
				tooltips.Add(new TooltipLine(mod, "CompleteQuest", "[c/ffc100: Quest Complete!]"));
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 3);
			recipe.AddIngredient(ItemID.CloudinaBottle, 1);
			recipe.AddIngredient(ItemID.PixieDust, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}