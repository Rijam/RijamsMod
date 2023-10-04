using RijamsMod.Items.Accessories.Melee;
using RijamsMod.Items.Consumables;
using RijamsMod.Projectiles.Pets;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
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

			switch (npc.type)
			{		
				case NPCID.Guide:
					NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.FindFirstNPC(NPCID.Guide)], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage, out List<int> _);
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
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
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<Harpy>()) > 0)
					{
						if (npcTypeListVillage.Contains(ModContent.NPCType<Harpy>()))
						{
							chat = "I wonder what is different about " + Main.npc[harpy].GivenName + " to cause her to become friendly. What do you think it is?";
						}
					}
					break;
				case NPCID.Pirate:
					if (Main.rand.NextBool(10))
					{
						chat = "I were in this fine game...";
					}
					break;
				case NPCID.Steampunker:
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.FindFirstNPC(NPCID.Steampunker)], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage2, out List<int> _);
						if (npcTypeListVillage2.Contains(ModContent.NPCType<InterstellarTraveler>()))
						{
							chat = "Do ya know if the armor " + Main.npc[interTravel].GivenName + " is wearing comes in brass? I'd even settle for brass-colored!";
						}
					}
					break;
				case NPCID.Nurse:
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<Harpy>()) > 0 && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.FindFirstNPC(NPCID.Nurse)], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage3, out List<int> _);
						if (npcTypeListVillage3.Contains(ModContent.NPCType<InterstellarTraveler>()) && npcTypeListVillage3.Contains(ModContent.NPCType<Harpy>()))
						{
							chat = "Don't get me started on how difficult it is to operate on " + Main.npc[harpy].GivenName + ". At least " + Main.npc[interTravel].GivenName + " knows how to take care of herself.";
						}
					}
					break;
				case NPCID.BestiaryGirl:
					NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.FindFirstNPC(NPCID.BestiaryGirl)], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage4, out List<int> _);
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						if (npcTypeListVillage4.Contains(ModContent.NPCType<InterstellarTraveler>()))
						{
							if (Main.bloodMoon || Main.moonPhase == 0)
							{
								chat = Main.npc[interTravel].GivenName + " exciting! Want to CHAT!";
							}
							else
							{
								chat = "Wow " + Main.npc[interTravel].GivenName + " is like, such an exciting person! We should totally get to know each other!";
							}
						}
					}
					if (Main.rand.NextBool(4) && npcTypeListVillage4.Contains(ModContent.NPCType<Harpy>()))
					{
						if (Main.bloodMoon || Main.moonPhase == 0)
						{
							chat = "Friendly " + Main.npc[harpy].GivenName + "! LEARN about them!";
						}
						else
						{
							chat = "A friendly harpy! We could learn so much about harpy society from " + Main.npc[harpy].GivenName + ", if there even is such a thing!";
						}
					}
					break;
				case NPCID.Merchant:
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.FindFirstNPC(NPCID.Merchant)], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage5, out List<int> _);
						if (npcTypeListVillage5.Contains(ModContent.NPCType<InterstellarTraveler>()))
						{
							chat = "Next time you speak with " + Main.npc[interTravel].GivenName + ", tell them that their business is not welcomed.";
						}
					}
					break;
				case NPCID.Stylist:
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.FindFirstNPC(NPCID.Stylist)], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage6, out List<int> _);
						if (npcTypeListVillage6.Contains(ModContent.NPCType<InterstellarTraveler>()))
						{
							chat = "I'm not sure what to do with " + Main.npc[interTravel].GivenName + ". Feathers are not the same as hair!";
						}
					}
					break;
				case NPCID.Angler:
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<Harpy>()) > 0 && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.FindFirstNPC(NPCID.Angler)], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage7, out List<int> _);
						if (npcTypeListVillage7.Contains(ModContent.NPCType<InterstellarTraveler>()) && npcTypeListVillage7.Contains(ModContent.NPCType<Harpy>()))
						{
							chat = "Dodo " + Main.npc[harpy].GivenName + " definitely fits the definition for Bird-Brain! Ugly Duckling over there hides herself behind that armor!";
						}
					}
					break;
				case NPCID.TravellingMerchant:
					if (Main.rand.NextBool(10))
					{
						chat = "It has been quite a while since I have seen my ...good friend. I should visit The Valley at some point...";
					}
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.FindFirstNPC(NPCID.TravellingMerchant)], 1, out List<int> _, out List<int> _, out List<int> npcTypeListVillage8, out List<int> _);
						if (npcTypeListVillage8.Contains(ModContent.NPCType<InterstellarTraveler>()))
						{
							string[] lines = { "I've never seen anyone like " + Main.npc[interTravel].GivenName + " throughout my travels. This is quite interesting!",
							"No other merchants' goods are as exotic as mine! Not even " + Main.npc[interTravel].GivenName + "'s!"};
							chat = lines[Main.rand.Next(lines.Length)];
						}
					}
					break;
				case NPCID.TaxCollector:
					if (Main.rand.NextBool(4) && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 0)
					{
						NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.FindFirstNPC(NPCID.TaxCollector)], 3, out List<int> _, out List<int> _, out List<int> npcTypeListVillage9, out List<int> _);
						if (npcTypeListVillage9.Contains(ModContent.NPCType<InterstellarTraveler>()))
						{
							chat = "That alien " + Main.npc[interTravel].GivenName + " keeps giving me this weird currency. I refuse to accept it! What am I supposed to do with it?";
						}
					}
					break;
			}
		}
		public override void ModifyShop(NPCShop shop)
		{
			//bool townNPCsCrossModSupport = ModContent.GetInstance<RijamsModConfigServer>().TownNPCsCrossModSupport;
			//int interTravel = NPC.FindFirstNPC(ModContent.NPCType<InterstellarTraveler>());

			// BossesAsNPCs support in RijamsMod.cs PostSetupContent()

			if (shop.NpcType == NPCID.ArmsDealer)
			{
				shop.Add(new Item(ModContent.ItemType<Items.Weapons.Ranged.Ammo.BloodyArrow>()) { shopCustomPrice = 40 },
					new Condition(Condition.DownedEowOrBoc.Description + " and " + Condition.TimeNight.Description + ", or in Hardmode",
					() => (Condition.DownedEowOrBoc.IsMet() && Condition.TimeNight.IsMet()) || Condition.Hardmode.IsMet()));
			}
			if (shop.NpcType == NPCID.WitchDoctor)
			{
				shop.Add(new Item(ItemID.WormTooth) { shopCustomPrice = 100 },
					new Condition(Condition.CrimsonWorld.Description + ", or in Hardmode", () => Condition.CrimsonWorld.IsMet() || Condition.Hardmode.IsMet()));
				shop.Add(new Item(ModContent.ItemType<Items.Materials.CrawlerChelicera>()) { shopCustomPrice = 125 },
					new Condition(Condition.CorruptWorld.Description + ", or in Hardmode", () => Condition.CorruptWorld.IsMet() || Condition.Hardmode.IsMet()));
			}
			if (shop.NpcType == NPCID.DyeTrader)
			{
				shop.Add(ItemID.RedHusk, Condition.InRockLayerHeight);
				Condition undergroundOrCaverns = new("In the Underground or Caverns",
					() => Condition.InDirtLayerHeight.IsMet() || Condition.InRockLayerHeight.IsMet());
				shop.Add(ItemID.OrangeBloodroot, undergroundOrCaverns);
				shop.Add(ItemID.LimeKelp, undergroundOrCaverns);
				shop.Add(ItemID.GreenMushroom, undergroundOrCaverns);
				shop.Add(ItemID.TealMushroom, undergroundOrCaverns);

				Condition surfaceForestOrOcean = new("On the Surface in the Forest or Ocean",
					() => Condition.InOverworldHeight.IsMet() && (Condition.InShoppingZoneForest.IsMet() || Condition.InBeach.IsMet()));

				shop.Add(ItemID.YellowMarigold, surfaceForestOrOcean);
				shop.Add(ItemID.BlueBerries, surfaceForestOrOcean);

				shop.Add(ItemID.CyanHusk, Condition.InSnow);
				shop.Add(ItemID.SkyBlueFlower, Condition.InJungle);
				shop.Add(ItemID.VioletHusk, Condition.InJungle, Condition.InRockLayerHeight);

				shop.Add(ItemID.PinkPricklyPear, Condition.InDesert);
				shop.Add(ItemID.PurpleMucos, Condition.InBeach);
				shop.Add(ItemID.BlackInk, Condition.InBeach);

				shop.Add(ModContent.ItemType<Items.Dyes.BeamDye>(), Condition.NpcIsPresent(ModContent.NPCType<InterstellarTraveler>()));
			}
			if (shop.NpcType == NPCID.Demolitionist)
			{
				shop.Add(new Item(ItemID.BombFish) { shopCustomPrice = 1000 }, new Condition("Rescued Angler", () => NPC.savedAngler));
				shop.Add(new Item(ItemID.ScarabBomb) { shopCustomPrice = 1500 }, Condition.DownedEyeOfCthulhu);
				shop.Add(new Item(ItemID.DryBomb) { shopCustomPrice = 2500 }, ShopConditions.HellTraderMovedIn, Condition.NpcIsPresent(ModContent.NPCType<HellTrader>()));
				shop.Add(new Item(ItemID.Beenade) { shopCustomPrice = 200 }, Condition.DownedQueenBee);
				shop.Add(new Item(ItemID.MolotovCocktail) { shopCustomPrice = 500 }, Condition.DownedSkeletron);
				shop.Add(new Item(ItemID.Explosives) { shopCustomPrice = 7000 }, new Condition("Rescued Mechanic", () => NPC.savedMech));
				shop.Add(new Item(ItemID.Detonator) { shopCustomPrice = 10000 });
			}
			if (shop.NpcType == NPCID.Dryad)
			{
				shop.Add(new Item(ItemID.JungleGrassSeeds) { shopCustomPrice = 1500 }, Condition.InJungle);
				shop.Add(new Item(ItemID.FireBlossomPlanterBox) { shopCustomPrice = 100 }, 
					Condition.DownedEyeOfCthulhu, Condition.DownedEowOrBoc, Condition.DownedSkeletron, Condition.PreHardmode);
			}
			if (shop.NpcType == NPCID.Truffle)
			{
				shop.Add(new Item(ItemID.MushroomGrassSeeds) { shopCustomPrice = 1500 });
				shop.Add(ModContent.ItemType<Items.Weapons.Summon.Cudgels.GleamingFungusCudgel>());
			}
			if (shop.NpcType == NPCID.Pirate)
			{
				shop.Add(ItemID.SnowballLauncher, Condition.InSnow);
			}
			if (shop.NpcType == NPCID.SkeletonMerchant)
			{
				shop.Add(new Item(ItemID.Hook) { shopCustomPrice = 5000 }, Condition.DownedGoblinArmy);
				shop.Add(new Item(ItemID.Bone) { shopCustomPrice = 250 }, Condition.DownedSkeletron);
				shop.Add(new Item(ItemID.BoneWand) { shopCustomPrice = 12500 }, Condition.DownedSkeletron);
				shop.Add(new Item(ItemID.BoneSword) { shopCustomPrice = 45000 });
				shop.Add(new Item(ItemID.BonePickaxe) { shopCustomPrice = 75000 }, Condition.DownedEarlygameBoss);
				shop.Add(ModContent.ItemType<CaveCarrot>(), Condition.InBelowSurface, Condition.DownedOldOnesArmyAny, Condition.MoonPhasesEven);
			}
			if (shop.NpcType == NPCID.GoblinTinkerer)
			{
				shop.Add(ModContent.ItemType<Items.Placeable.StripLight>(), Condition.NpcIsPresent(ModContent.NPCType<InterstellarTraveler>()));
			}

			if (shop.NpcType == NPCID.BestiaryGirl)
			{
				shop.Add(ModContent.ItemType<Items.Pets.StardustDragonCrest>(), Condition.DownedStardustPillar);
				shop.Add(ModContent.ItemType<SnuggetPetLicense>(),
					new Condition(Language.GetText("Conditions.BestiaryPercentage").WithFormatArgs(60),
					() => Main.GetBestiaryProgressReport().CompletionPercent >= 0.6f), 
					Condition.NpcIsPresent(ModContent.NPCType<InterstellarTraveler>()));
			}
			
			if (ModLoader.TryGetMod("PboneUtils", out Mod pboneUtils) && ShopConditions.TownNPCsCrossModSupport.IsMet())
			{
				if (pboneUtils.TryFind<ModNPC>("Miner", out ModNPC minerModNPC) && shop.NpcType == minerModNPC.Type)
				{
					shop.Add(new Item(ItemID.MiningShirt) { shopCustomPrice = 100000 }, Condition.Hardmode);
					shop.Add(new Item(ItemID.MiningPants) { shopCustomPrice = 100000 }, Condition.Hardmode);
				}
			}

			/*
			if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC) && ShopConditions.TownNPCsCrossModSupport.IsMet())
			{
				if (fishermanNPC.TryFind<ModNPC>("Fisherman", out ModNPC fisherman) && shop.NpcType == fisherman.Type)
				{
					if (shop.Name == "Fish") // Fish shop is open
					{
						Item hornetTail = new(ModContent.ItemType<Items.Fishing.HornetTail>());
						int itemValue = hornetTail.GetStoreValue();
						shop.Add(new Item(hornetTail.type) { shopCustomPrice = (int)Math.Round(itemValue * (float)fishermanNPC.Call("shopMulti")) });
					}
					if (shop.Name == "Bait") // Bait shop is open
					{
						Item wildBait = new(ModContent.ItemType<Items.Fishing.WildBait>());
						int itemValue = wildBait.GetStoreValue();
						shop.Add(new Item(wildBait.type) { shopCustomPrice = (int)Math.Round(itemValue * (float)fishermanNPC.Call("shopMulti")) }, Condition.AnglerQuestsFinishedOver(3));
					}
				}
			}*/

		}
		// TODO: Figure out the new way to add items to the Traveling Merchant.
		/*public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
		{
			if (npc.type == NPCID.TravellingMerchant && shopName == "Shop")
			{
				TravellingMerchantShop.AddInfoEntry(ModContent.ItemType<Items.Consumables.StrangeRoll>(), Condition.DownedEyeOfCthulhu);
			}
		}*/
		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			if (NPC.downedBoss1 && Main.rand.NextBool(5)) //EoC
			{
				shop[nextSlot] = ModContent.ItemType<StrangeRoll>();
				nextSlot++;
			}
			if (Condition.DownedEarlygameBoss.IsMet() && Main.rand.NextBool(5))
			{
				shop[nextSlot] = ModContent.ItemType<LoopingOil>();
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
				// If the Black or Yellow Counterweight are being sold and the Looping Oil isn't, add the Looping Oil.
				if (NPCHelper.FindItemInShop(shop, ItemID.BlackCounterweight, out int? _) || NPCHelper.FindItemInShop(shop, ItemID.YellowCounterweight, out int? _))
				{
					if (!NPCHelper.FindItemInShop(shop, ModContent.ItemType<LoopingOil>(), out int? _))
					{
						shop[nextSlot] = ModContent.ItemType<LoopingOil>();
						nextSlot++;
					}
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
					if (fishermanNPC.TryFind<ModNPC>("Fisherman", out ModNPC fishermanModNPC))
					{
						var fishermanHappiness = NPCHappiness.Get(fishermanModNPC.Type);

						fishermanHappiness.SetNPCAffection(harpy, AffectionLevel.Like);
						harpyHappiness.SetNPCAffection(fishermanModNPC.Type, AffectionLevel.Love);

						hellTraderHappiness.SetNPCAffection(fishermanModNPC.Type, AffectionLevel.Like);
					}
				}

				if (ModLoader.TryGetMod("SGAmod", out Mod sgamod) && townNPCsCrossModSupport)
				{
					if (sgamod.TryFind<ModNPC>("Dergon", out ModNPC dergonModNPC))
					{
						int drakenType = dergonModNPC.Type;
						var drakenHappiness = NPCHappiness.Get(drakenType);

						drakenHappiness.SetNPCAffection(harpy, AffectionLevel.Like);
						harpyHappiness.SetNPCAffection(drakenType, AffectionLevel.Like);

						drakenHappiness.SetNPCAffection(intTrav, AffectionLevel.Like);
						intTravHappiness.SetNPCAffection(drakenType, AffectionLevel.Like);
					}
					if (sgamod.TryFind<ModNPC>("Goat", out ModNPC goatModNPC))
					{
						hellTraderHappiness.SetNPCAffection(goatModNPC.Type, AffectionLevel.Like);
					}
				}

				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && townNPCsCrossModSupport)
				{
					if (thorium.TryFind<ModNPC>("Cook", out ModNPC cookModNPC))
					{
						intTravHappiness.SetNPCAffection(cookModNPC.Type, AffectionLevel.Love);
					}
					if (thorium.TryFind<ModNPC>("WeaponMaster", out ModNPC weaponMasterModNPC))
					{
						var weaponMasterHappiness = NPCHappiness.Get(weaponMasterModNPC.Type);

						hellTraderHappiness.SetNPCAffection(weaponMasterModNPC.Type, AffectionLevel.Love);
						weaponMasterHappiness.SetNPCAffection(hellTrader, AffectionLevel.Like);
					}
				}

				if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs) && townNPCsCrossModSupport)
				{
					if (bossesAsNPCs.TryFind<ModNPC>("MartianSaucer", out ModNPC martianSaucer))
					{
						var martianSaucerHappiness = NPCHappiness.Get(martianSaucer.Type);

						martianSaucerHappiness.SetNPCAffection(intTrav, AffectionLevel.Like);
						intTravHappiness.SetNPCAffection(martianSaucer.Type, AffectionLevel.Like);
					}
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) && townNPCsCrossModSupport) //Calamity
				{
					if (calamity.TryFind<ModNPC>("WITCH", out ModNPC brimstoneWitchModNPC) && calamity.TryFind<ModNPC>("DILF", out ModNPC archmageModNPC))
					{
						var brimstoneWitchHappiness = NPCHappiness.Get(brimstoneWitchModNPC.Type);

						hellTraderHappiness.SetNPCAffection(brimstoneWitchModNPC.Type, AffectionLevel.Love);
						hellTraderHappiness.SetNPCAffection(archmageModNPC.Type, AffectionLevel.Like);

						brimstoneWitchHappiness.SetNPCAffection(hellTrader, AffectionLevel.Like);
					}
				}

				if (ModLoader.TryGetMod("LivingWorldMod", out Mod livingWorldMod) && townNPCsCrossModSupport)
				{
					if (livingWorldMod.TryFind<ModNPC>("HarpyVillager", out ModNPC harpyVillagerModNPC))
					{
						harpyHappiness.SetNPCAffection(harpyVillagerModNPC.Type, AffectionLevel.Love);
					}
				}

				if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage) && townNPCsCrossModSupport)
				{
					if (magicStorage.TryFind<ModNPC>("Golem", out ModNPC automaton))
					{
						var automatonHappiness = NPCHappiness.Get(automaton.Type);

						intTravHappiness.SetNPCAffection(automaton.Type, AffectionLevel.Like);
						automatonHappiness.SetNPCAffection(intTrav, AffectionLevel.Like);
					}
				}
			}
		}

		public override void Load()
		{
			Terraria.GameContent.UI.On_EmoteBubble.ProbeExceptions += EmoteBubble_Hook_ProbeEmotions;
		}

		private delegate void orig_EmoteBubble_ProbeExceptions(EmoteBubble self);

		private static void EmoteBubble_Hook_ProbeEmotions(Terraria.GameContent.UI.On_EmoteBubble.orig_ProbeExceptions orig, EmoteBubble self, List<int> list, Player plr, WorldUIAnchor other)
		{
			orig(self, list, plr, other);

			NPC nPC = (NPC)self.anchor.entity;

			if (nPC.type == ModContent.NPCType<Harpy>())
			{
				list.Add(EmoteID.BiomeSky); // Sky or Space
				list.Add(EmoteID.TownBestiaryGirl);
				
				if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC) && fishermanNPC.TryFind<ModNPC>("Fisherman", out ModNPC fisherman))
				{
					if (other != null && ((NPC)other.entity).type == fisherman.Type)
					{
						list.Add(EmoteID.EmotionLove);
					}
				}
				if (!Main.dayTime)
				{
					list.Add(EmoteID.EmoteSleep);
				}
				else if (!Main.raining)
				{
					list.Add(EmoteID.WeatherSunny);
				}
				else
				{
					list.Add(EmoteID.EmoteSadness);
				}
			}
			if (nPC.type == ModContent.NPCType<InterstellarTraveler>())
			{
				list.Add(EmoteID.BiomeSky);
				list.Add(EmoteID.BossMartianship);
				list.Add(EmoteID.CritterBird);

				if (!RijamsModWorld.intTravQuestBreadAndJelly)
				{
					list.Add(EmoteID.Hungry);
				}
				if (other != null && ((NPC)other.entity).type == ModContent.NPCType<Harpy>())
				{
					list.Add(EmoteID.EmoteHappiness);
					list.Add(EmoteID.EmoteLaugh);
				}
				if (other != null && ((NPC)other.entity).type == NPCID.Merchant)
				{
					list.Add(EmoteID.EmoteAnger); // The face emote
					list.Add(EmoteID.EmotionAnger); // The 4 red corner angry emote
				}
			}
			if (nPC.type == ModContent.NPCType<HellTrader>())
			{
				list.Add(EmoteID.BiomeLavalayer); // Underworld
				list.Add(EmoteID.MiscFire);
				list.Add(EmoteID.DebuffBurn);
				list.Add(EmoteID.BossWoF);
				list.Add(EmoteID.ItemManaPotion);
			}

			if (nPC.type == ModContent.NPCType<SnuggetPet.SnuggetPet>())
			{
				list.Clear();
				list.Add(EmoteID.EmoteSilly);
				list.Add(EmoteID.EmoteHappiness);
				list.Add(EmoteID.EmoteSleep);
				list.Add(EmoteID.EmotionLove);
				list.Add(EmoteID.EmoteNote);
				list.Add(EmoteID.EmoteEating);
			}
		}
	}
}