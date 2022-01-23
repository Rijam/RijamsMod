using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using RijamsMod.NPCs.TownNPCs;

namespace RijamsMod.Items
{
	//Code adapted from Fargo's Mutant Mod (CaughtNPCItem.cs)
	//and code adapted from Alchemist NPC (BrewerHorcrux.cs and similar)
	public class CaughtFisherman : ModItem
	{
		public override string Texture => "RijamsMod/NPCs/TownNPCs/Fisherman";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fisherman");
			Tooltip.SetDefault("'Greetings. Care to do some fishin'?'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 10;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noUseGraphic = true;
			item.consumable = true;
			item.UseSound = SoundID.Item44;
			item.makeNPC = (short)ModContent.NPCType<Fisherman>();
			item.tileBoost += 20;
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Fisherman>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			/*Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			if (!ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs) //So it still spawns the Town NPC if the config is off. If it is on, it automatically does this.
            {
				NPC.NewNPC((int)mousePos.X, (int)mousePos.Y, ModContent.NPCType<Fisherman>());
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, ModContent.NPCType<Fisherman>(), 0f, 0f, 0f, 0, 0, 0);
			}*/
			string chatmessage = "The Fisherman has been spawned!";
			if (Main.netMode != NetmodeID.Server)
            {
				//Main.NewText(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Harpy>())].GivenName + " the " + chatmessage, 50, 125, 255);//index was outside the bounds of the array on a server
				Main.NewText(chatmessage, 50, 125, 255);
			}
			else
            {
				NetworkText text = NetworkText.FromLiteral(chatmessage);
				NetMessage.BroadcastChatMessage(text, new Color(50, 125, 255));
            }
		}
	}
	public class CaughtHarpy : ModItem
	{
		public override string Texture => "RijamsMod/NPCs/TownNPCs/Harpy";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy");
			Tooltip.SetDefault("'Friends? I am friendly.'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 10;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noUseGraphic = true;
			item.consumable = true;
			item.UseSound = SoundID.Item44;
			item.makeNPC = (short)ModContent.NPCType<Harpy>();
			item.tileBoost += 20;
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
				NetMessage.BroadcastChatMessage(text, new Color(50, 125, 255));
			}
		}
	}
	public class CaughtIntTrav : ModItem
	{
		public override string Texture => "RijamsMod/NPCs/TownNPCs/InterstellarTraveler";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Interstellar Traveler");
			Tooltip.SetDefault("'I'm pretty far from home, but this place is pretty cool.'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 26));
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 10;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noUseGraphic = true;
			item.consumable = true;
			item.UseSound = SoundID.Item44;
			item.makeNPC = (short)ModContent.NPCType<InterstellarTraveler>();
			item.tileBoost += 20;
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
				NetMessage.BroadcastChatMessage(text, new Color(50, 125, 255));
			}
		}
	}
	public class CaughtHellTrader : ModItem
	{
		public override string Texture => "RijamsMod/NPCs/TownNPCs/HellTrader";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hell Trader");
			Tooltip.SetDefault("'Hey, human! Good to see you again.'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 23));
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 10;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noUseGraphic = true;
			item.consumable = true;
			item.UseSound = SoundID.Item44;
			item.makeNPC = (short)ModContent.NPCType<HellTrader>();
			item.tileBoost += 20;
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
				NetMessage.BroadcastChatMessage(text, new Color(50, 125, 255));
			}
		}
	}
}