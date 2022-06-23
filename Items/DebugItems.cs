using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace RijamsMod.Items
{
	public class DebugDisplayStates : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override string Texture => "Terraria/Images/Item_" + ItemID.PurpleSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Display States");
			Tooltip.SetDefault("Left click to show all current states");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 14;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.White;
			Item.value = 0;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item9;
			Item.consumable = true;
		}
		public override bool? UseItem(Player player)
		{
			Main.NewText("intTravQuestOddDevice is currenty: " + RijamsModWorld.intTravQuestOddDevice);
			Main.NewText("intTravQuestBlankDisplay is currenty: " + RijamsModWorld.intTravQuestBlankDisplay);
			Main.NewText("intTravQuestTPCore is currenty: " + RijamsModWorld.intTravQuestTPCore);
			Main.NewText("intTravQuestRyeJam is currenty: " + RijamsModWorld.intTravQuestBreadAndJelly);
			Main.NewText("intTravQuestMagicOxygenizer is currenty: " + RijamsModWorld.intTravQuestMagicOxygenizer);
			Main.NewText("intTravQuestPrimeThruster is currenty: " + RijamsModWorld.intTravQuestPrimeThruster);
			Main.NewText("savedHarpy is currenty: " + RijamsModWorld.savedHarpy);
			Main.NewText("intTravArived is currenty: " + RijamsModWorld.intTravArrived);
			Main.NewText("hellTraderArrivable is currenty: " + RijamsModWorld.hellTraderArrivable);
			return true;
		}
	}
	public class DebugIntTravQuestOddDevice : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override string Texture => "Terraria/Images/Item_" + ItemID.GreenSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Odd Device Quest");
			Tooltip.SetDefault("Changes the states of the Odd Device quest from the Interstellar Traveler\nLeft click to set false\nRight click to set true");
		}

		public override void SetDefaults()
		{
			Item.color = Color.Orange;
			Item.width = 14;
			Item.height = 14;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.White;
			Item.value = 0;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item9;
			Item.consumable = true;
		}
		public override bool? UseItem(Player player)
		{
			RijamsModWorld.intTravQuestOddDevice = false;
			RijamsModWorld.UpdateWorldBool();
			return true;
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			RijamsModWorld.intTravQuestOddDevice = true;
			RijamsModWorld.UpdateWorldBool();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			string currentState = RijamsModWorld.intTravQuestOddDevice.ToString();
			tooltips.Add(new TooltipLine(Mod, "CurrentState", "intTravQuestOddDevice == " + currentState));
		}
	}
	public class DebugIntTravQuestBlankDisplay : DebugIntTravQuestOddDevice
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.GreenSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Blank Display Quest");
			Tooltip.SetDefault("Changes the states of the Blank Display quest from the Interstellar Traveler\nLeft click to set false\nRight click to set true");
		}
		public override bool? UseItem(Player player)
		{
			RijamsModWorld.intTravQuestBlankDisplay = false;
			RijamsModWorld.UpdateWorldBool();
			return true;
		}
		public override void RightClick(Player player)
		{
			RijamsModWorld.intTravQuestBlankDisplay = true;
			RijamsModWorld.UpdateWorldBool();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string currentState = RijamsModWorld.intTravQuestBlankDisplay.ToString();
			tooltips.Add(new TooltipLine(Mod, "CurrentState", "intTravQuestBlankDisplay == " + currentState));
		}
	}
	public class DebugIntTravQuestTPCore : DebugIntTravQuestOddDevice
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.GreenSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Teleportation Core Quest");
			Tooltip.SetDefault("Changes the states of the Teleportation Core quest from the Interstellar Traveler\nLeft click to set false\nRight click to set true");
		}
		public override bool? UseItem(Player player)
		{
			RijamsModWorld.intTravQuestTPCore = false;
			RijamsModWorld.UpdateWorldBool();
			return true;
		}
		public override void RightClick(Player player)
		{
			RijamsModWorld.intTravQuestTPCore = true;
			RijamsModWorld.UpdateWorldBool();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string currentState = RijamsModWorld.intTravQuestTPCore.ToString();
			tooltips.Add(new TooltipLine(Mod, "CurrentState", "intTravQuestTPCore == " + currentState));
		}
	}
	public class DebugIntTravQuestRyeJam : DebugIntTravQuestOddDevice
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.GreenSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Rye Jam");
			Tooltip.SetDefault("Changes the states of the Rye Jam quest from the Interstellar Traveler\nLeft click to set false\nRight click to set true");
		}
		public override bool? UseItem(Player player)
		{
			RijamsModWorld.intTravQuestBreadAndJelly = false;
			RijamsModWorld.UpdateWorldBool();
			return true;
		}
		public override void RightClick(Player player)
		{
			RijamsModWorld.intTravQuestBreadAndJelly = true;
			RijamsModWorld.UpdateWorldBool();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string currentState = RijamsModWorld.intTravQuestBreadAndJelly.ToString();
			tooltips.Add(new TooltipLine(Mod, "CurrentState", "intTravQuestRyeJam == " + currentState));
		}
	}
	public class DebugIntTravQuestMagicOxygenizer : DebugIntTravQuestOddDevice
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.GreenSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Magic Oxygenizer");
			Tooltip.SetDefault("Changes the states of the Magic Oxygenizer quest from the Interstellar Traveler\nLeft click to set false\nRight click to set true");
		}
		public override bool? UseItem(Player player)
		{
			RijamsModWorld.intTravQuestMagicOxygenizer = false;
			RijamsModWorld.UpdateWorldBool();
			return true;
		}
		public override void RightClick(Player player)
		{
			RijamsModWorld.intTravQuestMagicOxygenizer = true;
			RijamsModWorld.UpdateWorldBool();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string currentState = RijamsModWorld.intTravQuestMagicOxygenizer.ToString();
			tooltips.Add(new TooltipLine(Mod, "CurrentState", "intTravQuestMagicOxygenizer == " + currentState));
		}
	}
	public class DebugIntTravQuestPrimeThruster : DebugIntTravQuestOddDevice
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.GreenSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Prime Thruster");
			Tooltip.SetDefault("Changes the states of the Prime Thruster quest from the Interstellar Traveler\nLeft click to set false\nRight click to set true");
		}
		public override bool? UseItem(Player player)
		{
			RijamsModWorld.intTravQuestPrimeThruster = false;
			RijamsModWorld.UpdateWorldBool();
			return true;
		}
		public override void RightClick(Player player)
		{
			RijamsModWorld.intTravQuestPrimeThruster = true;
			RijamsModWorld.UpdateWorldBool();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string currentState = RijamsModWorld.intTravQuestPrimeThruster.ToString();
			tooltips.Add(new TooltipLine(Mod, "CurrentState", "intTravQuestPrimeThruster == " + currentState));
		}
	}
	public class DebugIntTravArived : DebugIntTravQuestOddDevice
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.GreenSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Interstellar Traveler Arrived Flag");
			Tooltip.SetDefault("Changes the flag that the Interstellar Traveler has arrived\nLeft click to set false\nRight click to set true");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.color = Color.Red;
		}
		public override bool? UseItem(Player player)
		{
			RijamsModWorld.intTravArrived = false;
			return true;
		}
		public override void RightClick(Player player)
		{
			RijamsModWorld.intTravArrived = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string currentState = RijamsModWorld.intTravArrived.ToString();
			tooltips.Add(new TooltipLine(Mod, "CurrentState", "intTravArived == " + currentState));
		}
	}
	public class DebugSavedHarpy : DebugIntTravQuestOddDevice
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.GreenSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Saved Harpy Flag");
			Tooltip.SetDefault("Changes the flag that says the Harpy has been saved\nLeft click to set false\nRight click to set true");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.color = Color.Red;
		}
		public override bool? UseItem(Player player)
		{
			RijamsModWorld.savedHarpy = false;
			return true;
		}
		public override void RightClick(Player player)
		{
			RijamsModWorld.savedHarpy = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string currentState = RijamsModWorld.savedHarpy.ToString();
			tooltips.Add(new TooltipLine(Mod, "CurrentState", "savedHarpy == " + currentState));
		}
	}
	public class DebugHellTraderArrivable : DebugIntTravQuestOddDevice
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.GreenSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Hell Trader Arrivable Flag");
			Tooltip.SetDefault("Changes the flag that the Hell Trader can arrive\nLeft click to set false\nRight click to set true");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.color = Color.Red;
		}
		public override bool? UseItem(Player player)
		{
			RijamsModWorld.hellTraderArrivable = false;
			return true;
		}
		public override void RightClick(Player player)
		{
			RijamsModWorld.hellTraderArrivable = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string currentState = RijamsModWorld.hellTraderArrivable.ToString();
			tooltips.Add(new TooltipLine(Mod, "CurrentState", "hellTraderArrivable == " + currentState));
		}
	}
	public class DebugMethodTester : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override string Texture => "Terraria/Images/Item_" + ItemID.PurpleSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Method Tester");
			Tooltip.SetDefault("Left click to show all current states");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 14;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.White;
			Item.value = 0;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item9;
			Item.consumable = true;
		}
		public override bool? UseItem(Player player)
		{
			Main.NewText("Main.player[Main.myPlayer].lastCreatureHit is currenty: " + Main.player[Main.myPlayer].lastCreatureHit);
			Main.NewText("NPC.killCount[Item.NPCtoBanner(NPCID.Harpy)] is currenty: " + NPC.killCount[Item.NPCtoBanner(NPCID.Harpy)]);
			Main.NewText("TallyCounter is currenty: " + Main.LocalPlayer.HasItem(ItemID.TallyCounter));
			Main.NewText("REK is currenty: " + Main.LocalPlayer.HasItem(ItemID.REK));
			Main.NewText("PDA is currenty: " + Main.LocalPlayer.HasItem(ItemID.PDA));
			Main.NewText("CellPhone is currenty: " + Main.LocalPlayer.HasItem(ItemID.CellPhone));
			Main.NewText("player.GetModPlayer<RijamsModPlayer>().summonersGlove is currenty: " + player.GetModPlayer<RijamsModPlayer>().summonersGlove);
			Main.NewText("player.GetModPlayer<RijamsModPlayer>().breathingPack is currenty: " + player.GetModPlayer<RijamsModPlayer>().breathingPack);
			Main.NewText("GetType().Namespace.ToString(): " + GetType().Namespace.ToString());
			//Main.NewText("player.GetModPlayer<RijamsModPlayer>().breathingPackUsed is currenty: " + player.GetModPlayer<RijamsModPlayer>().breathingPackUsed);
			return true;
		}
	}
	public class DebugAnglerQuestChanger : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override string Texture => "Terraria/Images/Item_" + ItemID.BlueSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Angler Quest Changer");
			Tooltip.SetDefault("Changes the number of Angler quests you have completed\nLeft click to add one\nRight click to remove one");
		}

		public override void SetDefaults()
		{
			Item.color = Color.RoyalBlue;
			Item.width = 14;
			Item.height = 14;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.White;
			Item.value = 0;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item9;
			Item.consumable = true;
		}
		public override bool? UseItem(Player player)
		{
			Main.player[Main.myPlayer].anglerQuestsFinished++;
			return true;
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			//Don't allow the number to become negative.
			if (Main.player[Main.myPlayer].anglerQuestsFinished > 0)
			{
				Main.player[Main.myPlayer].anglerQuestsFinished--;
			}
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string anglerQuestsCompleted = Main.player[Main.myPlayer].anglerQuestsFinished.ToString();
			tooltips.Add(new TooltipLine(Mod, "AnglerQuestsCompleted", "You have completed " + anglerQuestsCompleted + " Angler quests"));
		}
	}
}