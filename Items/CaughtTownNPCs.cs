using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using RijamsMod.NPCs.TownNPCs;
using System.Collections.Generic;
using Terraria.Chat;

namespace RijamsMod.Items
{
	//Code adapted from Fargo's Mutant Mod (CaughtNPCItem.cs)
	//and code adapted from Alchemist NPC (BrewerHorcrux.cs and similar)
	public class CaughtHarpy : ModItem
	{
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/Harpy";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Harpy");
			// Tooltip.SetDefault("'Friends? I am friendly.'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 10;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item44;
			Item.makeNPC = (short)ModContent.NPCType<Harpy>();
			Item.tileBoost += 20;
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Harpy>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			/*Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			if (!ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs) //So it still spawns the Town NPC if the config is off. If it is on, it automatically does this.
			{
				NPC.NewNPC((int)mousePos.X, (int)mousePos.Y, ModContent.NPCType<Harpy>());
			}*/

			string chatmessage = "The Harpy has been spawned!";
			if (Main.netMode != NetmodeID.Server)
			{
				//Main.NewText(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Harpy>())].GivenName + " the " + chatmessage, 50, 125, 255); //index was outside the bounds of the array on a server
				Main.NewText(chatmessage, 50, 125, 255);
			}
			else
			{
				NetworkText text = NetworkText.FromLiteral(chatmessage);
				ChatHelper.BroadcastChatMessage(text, new Color(50, 125, 255));
			}
		}
	}
	public class CaughtIntTrav : ModItem
	{
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/InterstellarTraveler";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Interstellar Traveler");
			// Tooltip.SetDefault("'I'm pretty far from home, but this place is pretty cool.'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 26));
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 10;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item44;
			Item.makeNPC = (short)ModContent.NPCType<InterstellarTraveler>();
			Item.tileBoost += 20;
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}
		public override void OnConsumeItem(Player player)
		{
			/*Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			if (!ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs) //So it still spawns the Town NPC if the config is off. If it is on, it automatically does this.
			{
				NPC.NewNPC((int)mousePos.X, (int)mousePos.Y, ModContent.NPCType<InterstellarTraveler>());
			}*/

			string chatmessage = "The Interstellar Traveler has been spawned!";
			if (Main.netMode != NetmodeID.Server)
			{
				//Main.NewText(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<InterstellarTraveler>())].GivenName + " the " + chatmessage, 50, 125, 255);
				Main.NewText(chatmessage, 50, 125, 255);
			}
			else
			{
				NetworkText text = NetworkText.FromLiteral(chatmessage);
				ChatHelper.BroadcastChatMessage(text, new Color(50, 125, 255));
			}
		}
	}
	public class CaughtHellTrader : ModItem
	{
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/HellTrader";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hell Trader");
			//Tooltip.SetDefault("'Hey, human! Good to see you again.'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 10;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item44;
			Item.makeNPC = (short)ModContent.NPCType<HellTrader>();
			Item.tileBoost += 20;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (RijamsModWorld.hellTraderArrivable)
			{
				tooltips.Insert(3, new TooltipLine(Mod, "Quote1", "'Hey, human! Good to see you again.'"));
			}
			else
			{
				tooltips.Insert(3, new TooltipLine(Mod, "Quote2", "'Hello, human. An unexpected confrontation, for sure.'"));
			}			
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<HellTrader>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}
		public override void OnConsumeItem(Player player)
		{
			/*Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			if (!ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs) //So it still spawns the Town NPC if the config is off. If it is on, it automatically does this.
			{
				NPC.NewNPC((int)mousePos.X, (int)mousePos.Y, ModContent.NPCType<HellTrader>());
			}*/

			string chatmessage = "The Hell Trader has been spawned!";
			if (Main.netMode != NetmodeID.Server)
			{
				//Main.NewText(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<HellTrader>())].GivenName + " the " + chatmessage, 50, 125, 255);
				Main.NewText(chatmessage, 50, 125, 255);
			}
			else
			{
				NetworkText text = NetworkText.FromLiteral(chatmessage);
				ChatHelper.BroadcastChatMessage(text, new Color(50, 125, 255));
			}
		}
	}
}