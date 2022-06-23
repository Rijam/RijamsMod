using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace RijamsMod.NPCs.TownNPCs
{
	public partial class GlobalNPCs : GlobalNPC
	{
		public override void GetChat(NPC npc, ref string chat)
		{
			int interTravel = NPC.FindFirstNPC(ModContent.NPCType<InterstellarTraveler>());
			int harpy = NPC.FindFirstNPC(ModContent.NPCType<Harpy>());
			//int fisherman = NPC.FindFirstNPC(ModContent.NPCType<Fisherman>());
			switch (npc.type)
			{		
				case NPCID.Guide:
					NPCHelper.GetNearbyResidentNPCs(Main.npc[NPCID.Guide], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage, out List<int> _);
					if (Main.rand.NextBool(2) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						if (npcTypeListVillage.Contains(ModContent.NPCType<InterstellarTraveler>()))
						{
							string[] lines = { "I was not expecting to see a friendly alien like " + Main.npc[interTravel].GivenName + ". I'd like to know about where they are from.",
							"" + Main.npc[interTravel].GivenName + " would like to learn about our planet's history. I hope they find it interesting and not confusing."};
							chat = lines[Main.rand.Next(lines.Length)];
						}
					}
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) <= 0 && NPC.downedBoss2)
					{
						chat = "There is an odd device that you can craft now. Maybe you can find out what its purpose is?";
					}
					if (Main.rand.NextBool(2) && NPC.CountNPCS(ModContent.NPCType<Harpy>()) > 0)
					{
						if (npcTypeListVillage.Contains(ModContent.NPCType<Harpy>()))
						{
							chat = "I wonder what is different about " + Main.npc[harpy].GivenName + " to cause her to become friendly. What do you think it is?";
						}
					}
					break;
				case NPCID.Pirate:
					if (Main.rand.Next(0, 10) == 0)
					{
						chat = "I were in this fine game...";
					}
					break;
				case NPCID.Steampunker:
					if (Main.rand.NextBool(2) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						NPCHelper.GetNearbyResidentNPCs(Main.npc[NPCID.Steampunker], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage2, out List<int> _);
						if (npcTypeListVillage2.Contains(ModContent.NPCType<InterstellarTraveler>()))
						{
							chat = "Do ya know if the armor " + Main.npc[interTravel].GivenName + " is wearing comes in brass? I'd even settle for brass-colored!";
						}
					}
					break;
				case NPCID.Nurse:
					if (Main.rand.NextBool(3) && NPC.CountNPCS(ModContent.NPCType<Harpy>()) > 0 && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						NPCHelper.GetNearbyResidentNPCs(Main.npc[NPCID.Nurse], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage3, out List<int> _);
						if (npcTypeListVillage3.Contains(ModContent.NPCType<InterstellarTraveler>()) && npcTypeListVillage3.Contains(ModContent.NPCType<Harpy>()))
						{
							chat = "Don't get me started on how difficult it is to operate on " + Main.npc[harpy].GivenName + ". At least " + Main.npc[interTravel].GivenName + " knows how to take care of herself.";
						}
					}
					break;
				case NPCID.BestiaryGirl:
					NPCHelper.GetNearbyResidentNPCs(Main.npc[NPCID.Nurse], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage4, out List<int> _);
					if (Main.rand.NextBool(2) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						if (npcTypeListVillage4.Contains(ModContent.NPCType<InterstellarTraveler>()))
						{
							chat = "Wow " + Main.npc[interTravel].GivenName + " is like, such an exciting person! We should totally get to know each other!";
						}
					}
					if (Main.rand.NextBool(2) && NPC.CountNPCS(ModContent.NPCType<Harpy>()) > 0)
					{
						chat = "A friendly harpy! We could learn so much about harpy society from " + Main.npc[harpy].GivenName + ", if there even is such a thing!";
					}
					break;
				case NPCID.Merchant:
					if (Main.rand.Next(0, 4) == 0 && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						chat = "Next time you speak with " + Main.npc[interTravel].GivenName + ", tell them that their business is not welcomed.";
					}
					break;
				case NPCID.Stylist:
					if (Main.rand.Next(0, 5) == 0 && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						chat = "I'm not sure what to do with " + Main.npc[interTravel].GivenName + ". Feathers are not the same as hair!";
					}
					break;
				case NPCID.Angler:
					if (Main.rand.Next(0, 8) == 0 && NPC.CountNPCS(ModContent.NPCType<Harpy>()) > 0 && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						chat = "Dodo " + Main.npc[harpy].GivenName + " definitely fits the definition for Bird-Brain! Ugly Duckling over there hides herself behind that armor!";
					}
					break;
				case NPCID.TravellingMerchant:
					if (Main.rand.Next(0, 10) == 0)
					{
						chat = "It has been quite a while since I have seen my ...good friend. I should visit The Valley at some point...";
					}
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						string[] lines = { "I've never seen anyone like " + Main.npc[interTravel].GivenName + " throughout my travels. This is quite interesting!",
						"No other merchants' goods are as exotic as mine! Not even " + Main.npc[interTravel].GivenName + "'s!"};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;
				case NPCID.TaxCollector:
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						chat = "That alien " + Main.npc[interTravel].GivenName + " keeps giving me this weird currency. I refuse to accept it! What am I supposed to do with it?";
					}
					break;
			}
		}
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<RijamsModConfigServer>().TownNPCsCrossModSupport;

			if (type == NPCID.Merchant && Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.SharpeningStation);
				shop.item[nextSlot].shopCustomPrice = 100000;
				nextSlot++;
			}
			if (type == NPCID.ArmsDealer)
			{
				if ((!Main.dayTime && NPC.downedBoss2) || Main.hardMode) //EoW or BoC
                {
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Ammo.BloodyArrow>());
					shop.item[nextSlot].shopCustomPrice = 40;
					nextSlot++;
				}
				if (Main.hardMode)
                {
					shop.item[nextSlot].SetDefaults(ItemID.AmmoBox);
					shop.item[nextSlot].shopCustomPrice = 150000;
					nextSlot++;
				}
			}
			if (type == NPCID.WitchDoctor)
			{
				if (Main.hardMode || WorldGen.crimson)
				{
					shop.item[nextSlot].SetDefaults(ItemID.WormTooth);
					shop.item[nextSlot].shopCustomPrice = 100;
					nextSlot++;
				}
				if (Main.hardMode || !WorldGen.crimson)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Materials.CrawlerChelicera>());
					shop.item[nextSlot].shopCustomPrice = 125;
					nextSlot++;
				}
			}
			if (type == NPCID.DyeTrader)
			{
				if (Main.LocalPlayer.ZoneRockLayerHeight) //Caverns layer
				{
					shop.item[nextSlot].SetDefaults(ItemID.RedHusk);
					nextSlot++;
				}
				if (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight) //Underground or Caverns layer
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
				if (Main.LocalPlayer.ZoneOverworldHeight && !Main.LocalPlayer.ZoneSnow && !Main.LocalPlayer.ZoneJungle && !Main.LocalPlayer.ZoneDesert && !Main.LocalPlayer.ZoneHallow) //if Surface layer and forest or ocean
				{
					shop.item[nextSlot].SetDefaults(ItemID.YellowMarigold);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BlueBerries);
					nextSlot++;
				}
				if (Main.LocalPlayer.ZoneSnow) //if Snow biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.CyanHusk);
					nextSlot++;
				}
				if (Main.LocalPlayer.ZoneJungle) //if Jungle biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.SkyBlueFlower);
					nextSlot++;
					if (Main.LocalPlayer.ZoneRockLayerHeight)
					{
						shop.item[nextSlot].SetDefaults(ItemID.VioletHusk);
						nextSlot++;
					}
				}
				if (Main.LocalPlayer.ZoneDesert) //if Desert biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.PinkPricklyPear);
					nextSlot++;
				}
				if (Main.LocalPlayer.ZoneBeach) //if Ocean biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.PurpleMucos);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BlackInk);
					nextSlot++;
				}
				int interTravel = NPC.FindFirstNPC(ModContent.NPCType<InterstellarTraveler>());
				if (interTravel > 0)
                {
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Dyes.BeamDye>());
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
				if (Main.LocalPlayer.ZoneJungle) //if Jungle biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.JungleGrassSeeds);
					shop.item[nextSlot].shopCustomPrice = 1500;
					nextSlot++;
				}
				if (NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedBoss3 && !Main.hardMode)
                {
					shop.item[nextSlot].SetDefaults(ItemID.FireBlossomPlanterBox);
					shop.item[nextSlot].shopCustomPrice = 100;
					nextSlot++;
				}
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
			
			if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs) && townNPCsCrossModSupport)
			{
				float shopMulti = (float)bossesAsNPCs.Call("shopMulti");
				if (type == NPCID.GoblinTinkerer)
				{
					if ((bool)bossesAsNPCs.Call("GoblinSellInvasionItems") && (bool)bossesAsNPCs.Call("downedGoblinSummoner"))
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.ShadowflameStaff>());
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value * shopMulti);
						nextSlot++;
					}
				}
				if (type == bossesAsNPCs.Find<ModNPC>("IceQueen").Type)
				{
					if (!(bool)bossesAsNPCs.Call("GetStatusShop1") && (bool)bossesAsNPCs.Call("GetStatusShop2"))
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Materials.FestivePlating>());
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value * shopMulti);
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Accessories.Summoner.NaughtyList>());
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value * shopMulti);
						nextSlot++;
					}
					if ((bool)bossesAsNPCs.Call("GetStatusShop1") && !(bool)bossesAsNPCs.Call("GetStatusShop2"))
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.FestiveWhip>());
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value * shopMulti);
						nextSlot++;
					}
				}
			}
			if (ModLoader.TryGetMod("PboneUtils", out Mod pboneUtils) && townNPCsCrossModSupport)
			{
				if (type == pboneUtils.Find<ModNPC>("Miner").Type)
				{
					if (Main.hardMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MiningShirt);
						shop.item[nextSlot].shopCustomPrice = 100000;
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.MiningPants);
						shop.item[nextSlot].shopCustomPrice = 100000;
						nextSlot++;
					}
				}
			}
		}
		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			if (NPC.downedBoss1 && Main.rand.NextBool(5)) //EoC
			{
				shop[nextSlot] = ModContent.ItemType<Items.Consumables.StrangeRoll>();
				nextSlot++;
			}
			// If in Hardmode, increase the chance for these items to be sold.
			if (Main.hardMode)
			{
				if (!NPCHelper.FindItemInShop(shop, ItemID.CelestialMagnet, out int? _) && Main.rand.NextBool(3))
				{
					shop[nextSlot] = ItemID.CelestialMagnet;
					nextSlot++;
				}
				if (!NPCHelper.FindItemInShop(shop, ItemID.AmmoBox, out int? _) && Main.rand.NextBool(3))
				{
					shop[nextSlot] = ItemID.AmmoBox;
					nextSlot++;
				}
				if (!NPCHelper.FindItemInShop(shop, ItemID.PulseBow, out int? _) && Main.rand.NextBool(3) && NPC.downedAncientCultist)
				{
					shop[nextSlot] = ItemID.PulseBow;
					nextSlot++;
				}
				if (!NPCHelper.FindItemInShop(shop, ItemID.BirdieRattle, out int? _) && Main.rand.NextBool(3))
				{
					shop[nextSlot] = ItemID.BirdieRattle;
					nextSlot++;
				}
				if (!NPCHelper.FindItemInShop(shop, ItemID.Gatligator, out int? _) && Main.rand.NextBool(5) && NPC.downedMechBossAny)
				{
					shop[nextSlot] = ItemID.Gatligator;
					nextSlot++;
				}
				if (!NPCHelper.FindItemInShop(shop, ItemID.BouncingShield, out int? _) && Main.rand.NextBool(5) && NPCHelper.DownedMechBossAll())
				{
					shop[nextSlot] = ItemID.BouncingShield;
					nextSlot++;
				}
			}
		}

		public class RijamsModNPCHappiness : GlobalNPC
		{
			public override void SetStaticDefaults()
			{
				bool townNPCsCrossModSupport = ModContent.GetInstance<RijamsModConfigServer>().TownNPCsCrossModSupport;

				int harpy = ModContent.NPCType<Harpy>(); // Get NPC's type
				int intTrav = ModContent.NPCType<InterstellarTraveler>();
				int hellTrader = ModContent.NPCType<HellTrader>();

				var harpyHappiness = NPCHappiness.Get(harpy);
				var intTravHappiness = NPCHappiness.Get(intTrav);
				var hellTraderHappiness = NPCHappiness.Get(hellTrader);

				var guide = NPCHappiness.Get(NPCID.Guide); // Get the key into the NPC's happiness
				var merchant = NPCHappiness.Get(NPCID.Merchant);
				var nurse = NPCHappiness.Get(NPCID.Nurse);
				var demolitionist = NPCHappiness.Get(NPCID.Demolitionist);
				var dyeTrader = NPCHappiness.Get(NPCID.DyeTrader);
				var angler = NPCHappiness.Get(NPCID.Angler);
				var zoologist = NPCHappiness.Get(NPCID.BestiaryGirl);
				var dryad = NPCHappiness.Get(NPCID.Dryad);
				var painter = NPCHappiness.Get(NPCID.Painter);
				var golfer = NPCHappiness.Get(NPCID.Golfer);
				var armsDealer = NPCHappiness.Get(NPCID.ArmsDealer);
				var tavernkeep = NPCHappiness.Get(NPCID.DD2Bartender);
				var stylist = NPCHappiness.Get(NPCID.Stylist);
				var goblinTinkerer = NPCHappiness.Get(NPCID.GoblinTinkerer);
				var witchDoctor = NPCHappiness.Get(NPCID.WitchDoctor);
				var clothier = NPCHappiness.Get(NPCID.Clothier);
				var mechanic = NPCHappiness.Get(NPCID.Mechanic);
				var partyGirl = NPCHappiness.Get(NPCID.PartyGirl);
				var wizard = NPCHappiness.Get(NPCID.Wizard);
				var taxCollector = NPCHappiness.Get(NPCID.TaxCollector);
				var truffle = NPCHappiness.Get(NPCID.Truffle);
				var pirate = NPCHappiness.Get(NPCID.Pirate);
				var steampunker = NPCHappiness.Get(NPCID.Steampunker);
				var cyborg = NPCHappiness.Get(NPCID.Cyborg);
				var santa = NPCHappiness.Get(NPCID.SantaClaus);
				//Princess automatically Loves everyone

				zoologist.SetNPCAffection(harpy, AffectionLevel.Love);
				guide.SetNPCAffection(harpy, AffectionLevel.Like); // Make the Guide like the Harpy!
				dryad.SetNPCAffection(harpy, AffectionLevel.Like);
				golfer.SetNPCAffection(harpy, AffectionLevel.Like);
				witchDoctor.SetNPCAffection(harpy, AffectionLevel.Like);
				partyGirl.SetNPCAffection(harpy, AffectionLevel.Like);
				steampunker.SetNPCAffection(harpy, AffectionLevel.Like);
				pirate.SetNPCAffection(harpy, AffectionLevel.Like);
				stylist.SetNPCAffection(harpy, AffectionLevel.Like);
				armsDealer.SetNPCAffection(intTrav, AffectionLevel.Dislike);
				angler.SetNPCAffection(intTrav, AffectionLevel.Dislike);
				nurse.SetNPCAffection(intTrav, AffectionLevel.Hate);

				zoologist.SetNPCAffection(intTrav, AffectionLevel.Love);
				dryad.SetNPCAffection(intTrav, AffectionLevel.Like);
				guide.SetNPCAffection(intTrav, AffectionLevel.Like);
				mechanic.SetNPCAffection(intTrav, AffectionLevel.Like);
				cyborg.SetNPCAffection(intTrav, AffectionLevel.Like);
				steampunker.SetNPCAffection(intTrav, AffectionLevel.Like);
				goblinTinkerer.SetNPCAffection(intTrav, AffectionLevel.Like);
				painter.SetNPCAffection(intTrav, AffectionLevel.Dislike);
				stylist.SetNPCAffection(intTrav, AffectionLevel.Dislike);
				angler.SetNPCAffection(intTrav, AffectionLevel.Dislike);
				merchant.SetNPCAffection(intTrav, AffectionLevel.Hate);
				taxCollector.SetNPCAffection(intTrav, AffectionLevel.Hate);

				wizard.SetNPCAffection(hellTrader, AffectionLevel.Like);
				tavernkeep.SetNPCAffection(hellTrader, AffectionLevel.Like);
				goblinTinkerer.SetNPCAffection(hellTrader, AffectionLevel.Like);
				clothier.SetNPCAffection(hellTrader, AffectionLevel.Like);
				truffle.SetNPCAffection(hellTrader, AffectionLevel.Like);
				dyeTrader.SetNPCAffection(hellTrader, AffectionLevel.Dislike);
				demolitionist.SetNPCAffection(hellTrader, AffectionLevel.Dislike);
				santa.SetNPCAffection(hellTrader, AffectionLevel.Dislike);

				if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC) && townNPCsCrossModSupport)
				{
					int fishermanType = fishermanNPC.Find<ModNPC>("Fisherman").Type;
					var fishermanHappiness = NPCHappiness.Get(fishermanNPC.Find<ModNPC>("Fisherman").Type);

					fishermanHappiness.SetNPCAffection(harpy, AffectionLevel.Like);
					harpyHappiness.SetNPCAffection(fishermanType, AffectionLevel.Love);

					hellTraderHappiness.SetNPCAffection(fishermanType, AffectionLevel.Like);
				}

				if (ModLoader.TryGetMod("SGAmod", out Mod sgamod) && townNPCsCrossModSupport)
				{
					int drakenType = sgamod.Find<ModNPC>("Dergon").Type;
					var drakenHappiness = NPCHappiness.Get(sgamod.Find<ModNPC>("Dergon").Type);

					drakenHappiness.SetNPCAffection(harpy, AffectionLevel.Like);
					harpyHappiness.SetNPCAffection(drakenType, AffectionLevel.Like);

					drakenHappiness.SetNPCAffection(intTrav, AffectionLevel.Like);
					intTravHappiness.SetNPCAffection(drakenType, AffectionLevel.Like);

					int goatType = sgamod.Find<ModNPC>("Goat").Type;
					hellTraderHappiness.SetNPCAffection(goatType, AffectionLevel.Like);
				}

				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && townNPCsCrossModSupport)
				{
					int cookType = thorium.Find<ModNPC>("Cook").Type;

					intTravHappiness.SetNPCAffection(cookType, AffectionLevel.Love);

					int weaponMasterType = thorium.Find<ModNPC>("WeaponMaster").Type;
					var weaponMasterHappiness = NPCHappiness.Get(thorium.Find<ModNPC>("WeaponMaster").Type);

					hellTraderHappiness.SetNPCAffection(weaponMasterType, AffectionLevel.Love);
					weaponMasterHappiness.SetNPCAffection(hellTrader, AffectionLevel.Like);
				}

				if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs) && townNPCsCrossModSupport)
				{
					int martianSaucerType = bossesAsNPCs.Find<ModNPC>("MartianSaucer").Type;
					var martianSaucerHappiness = NPCHappiness.Get(bossesAsNPCs.Find<ModNPC>("MartianSaucer").Type);

					martianSaucerHappiness.SetNPCAffection(intTrav, AffectionLevel.Like);
					intTravHappiness.SetNPCAffection(martianSaucerType, AffectionLevel.Like);
				}
			}
		}
	}
}