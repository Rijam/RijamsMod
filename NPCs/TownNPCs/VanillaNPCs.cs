using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace RijamsMod.NPCs.TownNPCs
{
	public partial class VanillaNPCs : GlobalNPC
	{
		public override void GetChat(NPC npc, ref string chat)
		{
			int interTravel = NPC.FindFirstNPC(mod.NPCType("Interstellar Traveler"));
			int harpy = NPC.FindFirstNPC(mod.NPCType("Harpy"));
			int fisherman = NPC.FindFirstNPC(mod.NPCType("Fisherman"));
			switch (npc.type)
			{		
				case NPCID.Guide:
					if (Main.rand.Next(0, 4) == 0 && NPC.CountNPCS(mod.NPCType("Interstellar Traveler")) > 0)
					{
						string[] lines = { "I was not expecting to see a friendly alien like " + Main.npc[interTravel].GivenName + ". I'd like to know about where they are from.",
						"" + Main.npc[interTravel].GivenName + " would like to learn about our planet's history. I hope they find it interesting and not confusing."};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					if (Main.rand.Next(0, 4) == 0 && NPC.CountNPCS(mod.NPCType("Interstellar Traveler")) <= 0 && NPC.downedBoss2)
					{
						chat = "There is an odd device that you can craft now. Maybe you can find out what its purpose is?";
					}
					if (Main.rand.Next(0, 4) == 0 && NPC.CountNPCS(mod.NPCType("Harpy")) > 0)
					{
						chat = "I wonder what is different about " + Main.npc[harpy].GivenName + " to cause her to become friendly. What do you think it is?";
					}
					break;
				case NPCID.Pirate:
					if (Main.rand.Next(0, 10) == 0)
					{
						chat = "I were in this fine game...";
					}
					if (Main.rand.Next(0, 4) == 0 && NPC.CountNPCS(mod.NPCType("Fisherman")) > 0)
					{
						chat = "Arr, that " + Main.npc[fisherman].GivenName + " is the type to steal me crew's waters and then get me crew into trouble with the authorities. Do not trust such a man.";
					}
					break;
				case NPCID.Steampunker:
					if (Main.rand.Next(0, 7) == 0 && NPC.CountNPCS(mod.NPCType("Interstellar Traveler")) > 0)
					{
						chat = "Do ya know if the armor " + Main.npc[interTravel].GivenName + " is wearing comes in brass? I'd even settle for brass-colored!";
					}
					break;
				case NPCID.Nurse:
					if (Main.rand.Next(0, 6) == 0 && NPC.CountNPCS(mod.NPCType("Harpy")) > 0 && NPC.CountNPCS(mod.NPCType("Interstellar Traveler")) > 0)
					{
						chat = "Don't get me started on how difficult it is to operate on " + Main.npc[harpy].GivenName + ". At least " + Main.npc[interTravel].GivenName + " knows how to take care of herself.";
					}
					break;
				/*case NPCID.Zoologist:
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Fisherman")) > 0)
					{
						chat = "I keep telling " + Main.npc[fisherman].GivenName + " the dangers of over-fishing and he just shrugs me off! What nerve!";
					}
					if (Main.rand.Next(0, 4) == 0 && NPC.CountNPCS(mod.NPCType("Interstellar Traveler")) > 0)
					{
						chat = "Wow " + Main.npc[interTravel].GivenName + " is like, such an exciting person! We should totally get to know each other!";
					}
					if (Main.rand.Next(0, 4) == 0 && NPC.CountNPCS(mod.NPCType("Harpy")) > 0)
					{
						chat = "A friendly harpy! We could learn so much about harpy society from " + Main.npc[harpy].GivenName + ", if there even is such a thing!";
					}
					break;*/
				case NPCID.Merchant:
					if (Main.rand.Next(0, 4) == 0 && NPC.CountNPCS(mod.NPCType("Interstellar Traveler")) > 0)
					{
						chat = "Next time you speak with " + Main.npc[interTravel].GivenName + ", tell them that their business is not welcomed.";
					}
					break;
				case NPCID.Stylist:
					if (Main.rand.Next(0, 5) == 0 && NPC.CountNPCS(mod.NPCType("Interstellar Traveler")) > 0)
					{
						chat = "I'm not sure what to do with " + Main.npc[interTravel].GivenName + ". Feathers are not the same as hair!";
					}
					break;
				case NPCID.Angler:
					if (Main.rand.Next(0, 4) == 0 && NPC.CountNPCS(mod.NPCType("Fisherman")) > 0)
					{
						chat = "Yeah? What about " + Main.npc[fisherman].GivenName + "? He's alright I guess. Back to work with you!";
					}
					if (Main.rand.Next(0, 8) == 0 && NPC.CountNPCS(mod.NPCType("Harpy")) > 0 && NPC.CountNPCS(mod.NPCType("Interstellar Traveler")) > 0)
					{
						chat = "Dodo " + Main.npc[harpy].GivenName + " definity fits the definition for Bird-Brain! Ugly Duckling over there hides herself behind that armor!";
					}
					break;
				case NPCID.TravellingMerchant:
					if (Main.rand.Next(0, 10) == 0)
					{
						chat = "It has been quite a while since I have seen my ...good friend. I should visit The Valley at some point...";
					}
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Interstellar Traveler")) > 0)
					{
						string[] lines = { "I've never seen anyone like " + Main.npc[interTravel].GivenName + " throughout my travels. This is quite interesting!",
						"No other merchants' goods are as exotic as mine! Not even " + Main.npc[interTravel].GivenName + "'s!"};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;
				case NPCID.TaxCollector:
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Interstellar Traveler")) > 0)
					{
						chat = "That alien " + Main.npc[interTravel].GivenName + " keeps giving me this weird currency. I refuse to accept it! What am I supposed to do with it?";
					}
					break;
			}
		}
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == NPCID.Merchant && Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.SharpeningStation);
				shop.item[nextSlot].shopCustomPrice = 100000;
				nextSlot++;
			}
			if (type == NPCID.ArmsDealer && Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.AmmoBox);
				shop.item[nextSlot].shopCustomPrice = 150000;
				nextSlot++;
			}
			if (type == NPCID.WitchDoctor)
			{
				if (Main.hardMode || WorldGen.crimson)
				{
					shop.item[nextSlot].SetDefaults(ItemID.WormTooth);
					shop.item[nextSlot].shopCustomPrice = 100;
					nextSlot++;
				}
				if (!NPC.downedPlantBoss && !Main.dayTime)
                {
					shop.item[nextSlot].SetDefaults(ItemID.PygmyNecklace);
					shop.item[nextSlot].shopCustomPrice = 200000;
					nextSlot++;
				}
			}
			if (type == NPCID.DyeTrader)
			{
				if (Main.player[Main.myPlayer].ZoneRockLayerHeight) //Caverns layer
				{
					shop.item[nextSlot].SetDefaults(ItemID.RedHusk);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneDirtLayerHeight || Main.player[Main.myPlayer].ZoneRockLayerHeight) //Underground or Caverns layer
				{
					shop.item[nextSlot].SetDefaults(ItemID.OrangeBloodroot);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.LimeKelp);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.GreenMushroom);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.TealMushroom);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneOverworldHeight && !Main.player[Main.myPlayer].ZoneSnow && !Main.player[Main.myPlayer].ZoneJungle && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneHoly) //if Surface layer and forest or ocean
				{
					shop.item[nextSlot].SetDefaults(ItemID.YellowMarigold);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BlueBerries);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneSnow) //if Snow biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.CyanHusk);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneJungle) //if Jungle biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.SkyBlueFlower);
					nextSlot++;
					if (Main.player[Main.myPlayer].ZoneRockLayerHeight)
					{
						shop.item[nextSlot].SetDefaults(ItemID.VioletHusk);
						nextSlot++;
					}
				}
				if (Main.player[Main.myPlayer].ZoneDesert) //if Desert biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.PinkPricklyPear);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneBeach) //if Ocean biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.PurpleMucos); //Need to update to PurpleMucus for 1.4
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BlackInk);
					nextSlot++;
				}
				int interTravel = NPC.FindFirstNPC(mod.NPCType("Interstellar Traveler"));
				if (interTravel > 0)
                {
					shop.item[nextSlot].SetDefaults(mod.ItemType("BeamDye"));
					nextSlot++;
				}
			}
			if (type == NPCID.Demolitionist)
			{
				if (NPC.savedAngler)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BombFish);
					shop.item[nextSlot].shopCustomPrice = 1000;
					nextSlot++;
				}
				if (NPC.downedQueenBee)
				{
					shop.item[nextSlot].SetDefaults(ItemID.Beenade);
					shop.item[nextSlot].shopCustomPrice = 200;
					nextSlot++;
				}
				if (NPC.downedBoss3) //Skeletron
				{
					shop.item[nextSlot].SetDefaults(ItemID.MolotovCocktail);
					shop.item[nextSlot].shopCustomPrice = 500;
					nextSlot++;
				}
				if (NPC.savedMech)
				{
					shop.item[nextSlot].SetDefaults(ItemID.Explosives);
					shop.item[nextSlot].shopCustomPrice = 7000;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.Detonator);
				shop.item[nextSlot].shopCustomPrice = 10000;
				nextSlot++;
			}
			if (type == NPCID.Dryad)
			{
				if (Main.player[Main.myPlayer].ZoneJungle) //if Jungle biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.JungleGrassSeeds);
					shop.item[nextSlot].shopCustomPrice = 1500;
					nextSlot++;
				}
			}
			if (type == NPCID.Wizard)
			{
				shop.item[nextSlot].SetDefaults(ItemID.Book); //Remove in 1.4
				shop.item[nextSlot].shopCustomPrice = 1500;
				nextSlot++;
			}
			if (type == NPCID.Truffle)
			{
				shop.item[nextSlot].SetDefaults(ItemID.MushroomGrassSeeds);
				shop.item[nextSlot].shopCustomPrice = 1500;
				nextSlot++;
			}
			if (type == NPCID.SkeletonMerchant)
			{
				if (NPC.downedGoblins)
				{
					shop.item[nextSlot].SetDefaults(ItemID.Hook);
					shop.item[nextSlot].shopCustomPrice = 5000;
					nextSlot++;
				}
				if (NPC.downedBoss3) //Skeletron
				{
					shop.item[nextSlot].SetDefaults(ItemID.Bone);
					shop.item[nextSlot].shopCustomPrice = 250;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BoneWand);
					shop.item[nextSlot].shopCustomPrice = 12500;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.BoneSword);
				shop.item[nextSlot].shopCustomPrice = 45000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BonePickaxe);
				shop.item[nextSlot].shopCustomPrice = 75000;
				nextSlot++;
			}
		}
		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			if (NPC.downedBoss1 && Main.rand.Next(0, 5) == 0) //EoC
			{
				shop[nextSlot] = mod.ItemType("StrangeRoll");
				nextSlot++;
			}
		}
	}
}