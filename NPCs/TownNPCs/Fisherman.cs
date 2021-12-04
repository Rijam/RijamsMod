using RijamsMod;
using RijamsMod.Items;
using RijamsMod.Items.Fishing;
using RijamsMod.Items.Armor.Vanity;
using RijamsMod.Projectiles;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using System;
//using static Terraria.ModLoader.ModContent;
//using Terraria.DataStructures;

namespace RijamsMod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Fisherman : ModNPC
	{
		readonly bool loadModdedItems = ModContent.GetInstance<RijamsModConfigServer>().LoadModdedItems;
		readonly bool sellBait = ModContent.GetInstance<RijamsModConfigServer>().SellBait;
		readonly bool sellFish = ModContent.GetInstance<RijamsModConfigServer>().SellFish;
		readonly bool sellFishingRods = ModContent.GetInstance<RijamsModConfigServer>().SellFishingRods;
		readonly bool sellExtraItems = ModContent.GetInstance<RijamsModConfigServer>().SellExtraItems;
		readonly int shopPriceScaling = ModContent.GetInstance<RijamsModConfigServer>().ShopPriceScaling;

		private static bool shop1;
		private static bool shop2;

		public override string Texture
		{
			get
			{
				return "RijamsMod/NPCs/TownNPCs/Fisherman";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "RijamsMod/NPCs/TownNPCs/Fisherman_Alt_1" };
			}
		}

		public override bool Autoload(ref string name)
		{
			name = "Fisherman";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Fisherman");
			Main.npcFrameCount[npc.type] = 25;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 1;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
			NPCID.Sets.HatOffsetY[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.townNPC = true;
			npc.friendly = true;
			npc.width = 18;
			npc.height = 40;
			npc.aiStyle = 7;
			npc.damage = 10;
			npc.defense = 15;//def 15
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			animationType = NPCID.Guide;
			Main.npcCatchable[npc.type] = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs;
			npc.catchItem = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtFisherman>() : (short)-1;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Fisherman_Gore_Head_alt"), 1f);
				}
				else
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Fisherman_Gore_Head"), 1f);
				}
				for (int k = 0; k < 1; k++)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Fisherman_Gore_Arm"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Fisherman_Gore_Leg"), 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.savedAngler && numTownNPCs > 4 && NPC.CountNPCS(ModContent.NPCType<Fisherman>()) < 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public override string TownNPCName()
		{
			string[] names = { "Willy", "Mark", "Charles", "Richard", "Michael", "Davis", "Ray", "Max", "Sherman", "Ike", "Pete", "Hermann", "Colin", "Paul", "Nathaniel", "Malcolm", "Keith", "Jarrell", "Isaac", "Spencer" };
			return Main.rand.Next(names);
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int fisherman = NPC.FindFirstNPC(mod.NPCType("Fisherman"));
			chat.Add("Greetings. Care to do some fishin'?");
			chat.Add("I got all the supplies you'd need if you want to do some fishin'.");
			chat.Add("Fishing rods? Bait? Hooks? You want it?");
			chat.Add("Make sure you have bait on your hook.");
			chat.Add("Ahoy there. My name is " + Main.npc[fisherman].GivenName + ".");
			chat.Add("There are all sorts of strange fish in these waters; see what you can catch!");
			chat.Add("Hm... I seem to have misplaced my rod...", 0.1);
			
			if (npc.life < npc.lifeMax * 0.5)
			{
				chat.Add("That's gonna leave a mark.", 10.0);
			}
			if (Main.dayTime)
			{
				chat.Add(Main.raining ? "Rain is the perfect time to do some fishin'!" : "A nice sunny day today.");
			}
			else
			{
				chat.Add(Main.raining ? "Careful out there. There's a nasty storm out tonight!" : "Time to get some shut-eye.");
			}
			if (npc.homeless)
			{
				chat.Add("Could you provide me with a vessel of some sort? A man needs a place to sleep!");
			}
			else
			{
				chat.Add("Now I can really set up shop! Make sure you purchase some goods!");
			}
			if (Main.LocalPlayer.wingTimeMax > 0)
			{
				chat.Add("Aye, I saw a large fish with wings once...");
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
				chat.Add("That's the biggest catch I've seen yet! Time to celebrate.", 2.0);
				chat.Add("I cooked me up some Bumblebee Tuna. Those things are tasty!", 2.0);
			}
			if (Main.bloodMoon)
			{
				chat.Add("I hear that there are some crazy creatures swimming in the water tonight. Fish them up! ...well not yet...", 2.0);
			}
			if (!NPC.savedAngler) //spawn in the Fisherman before meeting the requirements
			{
				chat.Add("Some sort of storm must've pushed me off course. I know I didn't intent on being here at this time.", 2.0);
			}
			if (!loadModdedItems && !sellBait && !sellFish && !sellFishingRods && !sellExtraItems) //if the config disables everything that he can sell
			{
				chat.Add("A mysterious force told me not to sell anything. They said to check the \"config\", whatever that is.", 20.0);
			}
			int interTravel = NPC.FindFirstNPC(mod.NPCType("Interstellar Traveler"));
			if (interTravel >= 0)
			{
				chat.Add("" + Main.npc[interTravel].GivenName + " is nice and all, but I don't trust her around my stash of fish!", 0.5);
			}
			int harpy = NPC.FindFirstNPC(mod.NPCType("Harpy"));
			if (harpy >= 0)
			{
				chat.Add("" + Main.npc[harpy].GivenName + " sometimes helps me scout ahead on my fishing journies. Very helpful!", 0.5);
			}
			int angler = NPC.FindFirstNPC(NPCID.Angler);
			if (angler >= 0)
			{
				chat.Add("" + Main.npc[angler].GivenName + " is after all sorts of exotic fish. You should see what he wants today. Currently, you have completed " + Main.player[Main.myPlayer].anglerQuestsFinished + " for him.", 0.25);
				if (Main.player[Main.myPlayer].anglerQuestsFinished == 0)
                {
					chat.Add("So far you have completed " + Main.player[Main.myPlayer].anglerQuestsFinished + " quests for " + Main.npc[angler].GivenName + " the Angler. I think it is time for you to start!", 0.5);
				}
				if (Main.player[Main.myPlayer].anglerQuestsFinished > 0)
				{
					chat.Add("So far you have completed " + Main.player[Main.myPlayer].anglerQuestsFinished + " quests for " + Main.npc[angler].GivenName + " the Angler. Keep at it!", 0.5);
				}
				if (Main.player[Main.myPlayer].anglerQuestsFinished > 9)
				{
					chat.Add("So far you have completed " + Main.player[Main.myPlayer].anglerQuestsFinished + " quests for " + Main.npc[angler].GivenName + " the Angler. Great job! Keep going!", 0.5);
				}
				if (Main.player[Main.myPlayer].Male && Main.player[Main.myPlayer].anglerQuestsFinished > 29)
				{
					chat.Add("Nice job, son! You have completed " + Main.player[Main.myPlayer].anglerQuestsFinished + " quests for " + Main.npc[angler].GivenName + " the Angler. Why not keep going?", 0.5);
				}
				else if (!Main.player[Main.myPlayer].Male && Main.player[Main.myPlayer].anglerQuestsFinished > 29)
                {
					chat.Add("Nice job, lass! You have completed " + Main.player[Main.myPlayer].anglerQuestsFinished + " quests for " + Main.npc[angler].GivenName + " the Angler. Why not keep going?", 0.5);
				}
				chat.Add("No I'm not " + Main.npc[angler].GivenName + " the Angler's father, but I'll gladly watch over him!", 0.25); //happiness quote
			}
			int pirate = NPC.FindFirstNPC(NPCID.Pirate);
			if (pirate >= 0)
			{
				chat.Add("I wouldn't trust " + Main.npc[pirate].GivenName + " the Pirate. I've had my fair share of run-ins with pirates.", 0.25); //happiness quote
			}
			/*int princess = NPC.FindFirstNPC(NPCID.Princess);
			if (princess >= 0)
			{
				chat.Add(Main.npc[princess].GivenName + " the Princess has offered to get me a fancy new vessel. How kind of her!", 0.25); //happiness quote
			}*/
			int truffle = NPC.FindFirstNPC(NPCID.Truffle);
			if (truffle >= 0)
			{
				chat.Add("I occasionally take Glowing Mushrooms to use as bait. I don't think " + Main.npc[truffle].GivenName + " knows that I do that, so don't tell 'em!", 0.25);
				chat.Add("I'm friends with " + Main.npc[truffle].GivenName + " the Truffle. Glowing Mushrooms make good bait!", 0.25); //happiness quote
			}
			int nurse = NPC.FindFirstNPC(NPCID.Nurse);
			if (nurse >= 0)
			{
				chat.Add("I dislike " + Main.npc[nurse].GivenName + " the Nurse. She always bugs me for check-ups that I don't need.", 0.25); //happiness quote
			}
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
			if (mechanic >= 0)
			{
				chat.Add(Main.npc[mechanic].GivenName + " the Mechanic is always here to help me when me ship needs repairs.", 0.25); //happiness quote
			}
			if (ModLoader.GetMod("ThoriumMod") != null) //Thorium
			{
				int diverman = NPC.FindFirstNPC(ModLoader.GetMod("ThoriumMod").NPCType("Diverman"));
				if (diverman >= 0)
				{
					chat.Add("Now " + Main.npc[diverman].GivenName + " is the kind of guy I respect!", 0.25);
				}
			}
			if (ModLoader.GetMod("CalamityMod") != null) //Calamity
			{
				int seaKing = NPC.FindFirstNPC(ModLoader.GetMod("CalamityMod").NPCType("SEAHOE")); //Sea King
				if (seaKing >= 0)
				{
					chat.Add("Amdias is such an interesting person. I hope he doesn't mind me fishin'.", 0.25);
				}
				if (Main.player[Main.myPlayer].Male)
				{
					chat.Add("Careful with mermaids, son, not all of them are friendly.", 0.25);
				}
				else
				{
					chat.Add("Careful with mermaids, lass, not all of them are friendly.", 0.25);
				}
			}
			if (ModLoader.GetMod("SGAmod") != null) //SGAmod
			{
				if (Main.hardMode)
				{
					chat.Add("A Shark-what? That is truely a freak of nature.", 0.5);
				}
			}
			if (ModLoader.GetMod("Fargowiltas") != null) //Fargo's Mutant Mod
			{
				int mutant = NPC.FindFirstNPC(ModLoader.GetMod("Fargowiltas").NPCType("Mutant"));
				if (mutant >= 0)
				{
					chat.Add("The wings that" + " " + Main.npc[mutant].GivenName + " " + "have remind me of something else...", 0.25);
				}
			}
			if (ModLoader.GetMod("PboneUtils") != null)
			{
				if (Main.dayTime)
				{
					chat.Add("I heard rumors of a blue hooded man who sells valuables...", 0.25);
				}
			}
			if (ModLoader.GetMod("HelpfulNPCs") != null)
			{
				int expertFisherman = NPC.FindFirstNPC(ModLoader.GetMod("HelpfulNPCs").NPCType("ExpertFisherman"));
				if (expertFisherman >= 0)
				{
					chat.Add("Don't let 'Expert' in " + Main.npc[expertFisherman].GivenName + "'s title fool ya. I'm just as much an expert as he is.", 0.25);
				}
			}
			if (ModLoader.GetMod("FishermanNPC") != null)
			{
				chat.Add("Sorry FishermanNPC for taking yer mod name in 1.4, I hope yer don't mind.", 0.25);
			}
			return chat;
		}
		/*
			Future happiness notes:
				Liked Biome: Ocean
				Disliked Biome: Desert
				Loved NPCs:
					Angler
				
				Liked NPCs:
					Princess
					Truffle
					Harpy (this mod)
					Diverman (Thorium)
				
				Disliked NPCs:
					Pirate
					Nurse
				
				Hated NPCs:
			
			Other NPCs' thoughts:
				Loved by:
					Princess
				
				Liked by:
					Truffle
					Angler
				
				Disliked by:
					Zoologist
					Pirate
				
				Hated by:
				
		*/

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = "Bait & Fish"; //shop1
			button2 = "Rods & Extras"; //shop2
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
				shop1 = true;
				shop2 = false;
			}
			if (!firstButton)
			{
				shop = true;
				shop1 = false;
				shop2 = true;
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			//Do the math for the price scaling config option. Divide by 100 to turn it into a decimal.
			float shopMulti = (shopPriceScaling / 100f);
			//
			// Bait
			//
			if (sellBait && shop1)
			{
				if (loadModdedItems)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<PlasticWormLure>());//5% bait power
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50 * shopMulti); //50 copper coins is the base price. Multiply that by the shop price multiplier. Round to the nearest whole number. Convert float to integer.
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.Snail);//10%
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(2000 * shopMulti);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ApprenticeBait);//15%
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(2500 * shopMulti);
				nextSlot++;
				/*if (NPC.downedBoss1)//EoC
				{
					shop.item[nextSlot].SetDefaults(ItemID.HellButterfly);//15%
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3000 * shopMulti);
					nextSlot++;
				}*/
				if (loadModdedItems)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Mealworm>());//18%
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(2800 * shopMulti);
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.Firefly);//20%
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3000 * shopMulti);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Worm);//25%
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3500 * shopMulti);
				nextSlot++;
				/*if (NPC.downedBoss2)//EoW or BoC
				{
					shop.item[nextSlot].SetDefaults(ItemID.Lavafly);//25%
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 * shopMulti);
					nextSlot++;
				}*/
				if (NPC.downedBoss1)//EoC
				{
					shop.item[nextSlot].SetDefaults(ItemID.JourneymanBait);//30%
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 * shopMulti);
					nextSlot++;
					if (loadModdedItems)
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<RedWorm>());//32%
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4200 * shopMulti);
						nextSlot++;
					}
				}
				if (NPC.downedBoss2)//EoW or BoC
				{
					shop.item[nextSlot].SetDefaults(ItemID.EnchantedNightcrawler);//35%
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4500 * shopMulti);
					nextSlot++;
				}
				/*if (NPC.downedBoss3)//Skeletron
				{
					shop.item[nextSlot].SetDefaults(ItemID.MagmaSnail);//35%
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7000 * shopMulti);
					nextSlot++;
				}*/
				if (NPC.downedQueenBee)
				{
					shop.item[nextSlot].SetDefaults(ItemID.Buggy);//40%
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 * shopMulti);
					nextSlot++;
				}
				if (Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MasterBait);//50%
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 * shopMulti);
					nextSlot++;
				}
				if (Main.hardMode && NPC.CountNPCS(NPCID.Truffle) > 0)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<GlowingMushroomChunk>());//66%
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(8000 * shopMulti);
					nextSlot++;
				}
				if (NPC.downedFishron)
				{
					shop.item[nextSlot].SetDefaults(ItemID.TruffleWorm);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(150000 * shopMulti);
					nextSlot++;
				}
				/*shop.item[nextSlot].SetDefaults(ItemID.CanOfWorms);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 * shopMulti);
				nextSlot++;*/
				if (loadModdedItems)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<BaitBox>());
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
					nextSlot++;
				}
			}
			//
			// Fish
			//
			if (sellFish && shop1)
			{
				if (Main.player[Main.myPlayer].ZoneDirtLayerHeight || Main.player[Main.myPlayer].ZoneRockLayerHeight || Main.player[Main.myPlayer].ZoneUnderworldHeight || Main.hardMode) //if Underground, Caverns, Underworld, or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.ArmoredCavefish);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti); //Fish's prices are based off of their vanilla values. 
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneSnow) //if Snow biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.AtlanticCod);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3750 * shopMulti);
					nextSlot++;
				}
				if (!Main.player[Main.myPlayer].ZoneDesert) //if not Desert
				{
					shop.item[nextSlot].SetDefaults(ItemID.Bass);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(2500 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneDirtLayerHeight || Main.player[Main.myPlayer].ZoneRockLayerHeight || Main.player[Main.myPlayer].ZoneUnderworldHeight) //if Underground, Caverns, Underworld
				{
					if (Main.player[Main.myPlayer].ZoneHoly) //if Hallow
					{
						shop.item[nextSlot].SetDefaults(ItemID.ChaosFish);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(150000 * shopMulti);
						nextSlot++;
					}
				}
				if ((NPC.downedBoss2 && WorldGen.crimson) || Main.hardMode) //if EoW/BoC defeated and Crimson world, or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.CrimsonTigerfish);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3750 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneSkyHeight) //if Space layer
				{
					shop.item[nextSlot].SetDefaults(ItemID.Damselfish);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneJungle) //if Jungle
				{
					shop.item[nextSlot].SetDefaults(ItemID.DoubleCod);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti);
					nextSlot++;
				}
				if ((NPC.downedBoss2 && !WorldGen.crimson) || Main.hardMode) //if EoW/BoC defeated and Corruption world, or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.Ebonkoi);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneUnderworldHeight || Main.hardMode) //if Underworld or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.FlarefinKoi);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 * shopMulti);
					nextSlot++;
				}
				/*if (Main.player[Main.myPlayer].ZoneDesert) //if Desert
				{
					shop.item[nextSlot].SetDefaults(ItemID.Flounder);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(750 * shopMulti);
					nextSlot++;
				}*/
				if (Main.player[Main.myPlayer].ZoneSnow) //if Snow biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.FrostMinnow);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti);
					nextSlot++;
				}
				if ((NPC.downedBoss2 && WorldGen.crimson) || Main.hardMode) //if EoW/BoC defeated and Crimson world, or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.Hemopiranha);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneJungle) //if Jungle
				{
					shop.item[nextSlot].SetDefaults(ItemID.Honeyfin);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.NeonTetra);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneUnderworldHeight || Main.hardMode) //if Underworld or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.Obsidifish);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneHoly) //if Hallow
				{
					shop.item[nextSlot].SetDefaults(ItemID.PrincessFish);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(12500 * shopMulti);
					nextSlot++;
				}
				if (Main.hardMode) //if Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.Prismite);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneBeach) //if Ocean
				{
					shop.item[nextSlot].SetDefaults(ItemID.RedSnapper);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3750 * shopMulti);
					nextSlot++;
				}
				/*if (Main.player[Main.myPlayer].ZoneDesert) //if Desert
				{
					shop.item[nextSlot].SetDefaults(ItemID.RockLobster);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 * shopMulti);
					nextSlot++;
				}*/
				if (Main.player[Main.myPlayer].ZoneSkyHeight || Main.player[Main.myPlayer].ZoneOverworldHeight) //if Sky or Surface
				{
					shop.item[nextSlot].SetDefaults(ItemID.Salmon);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3750 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneBeach) //if Ocean
				{
					shop.item[nextSlot].SetDefaults(ItemID.Shrimp);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti);
					nextSlot++;
				}
				if ((Main.player[Main.myPlayer].ZoneDirtLayerHeight || Main.player[Main.myPlayer].ZoneRockLayerHeight || Main.player[Main.myPlayer].ZoneUnderworldHeight) && !Main.player[Main.myPlayer].ZoneDesert) //if Underground or Caverns or Underworld, and not Desert
				{
					shop.item[nextSlot].SetDefaults(ItemID.SpecularFish);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3750 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneDirtLayerHeight || Main.player[Main.myPlayer].ZoneRockLayerHeight || Main.player[Main.myPlayer].ZoneUnderworldHeight) //if Underground or Caverns or Underworld
				{
					shop.item[nextSlot].SetDefaults(ItemID.Stinkfish);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(12500 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneBeach) //if Ocean
				{
					shop.item[nextSlot].SetDefaults(ItemID.Trout);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(2500 * shopMulti);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.Tuna);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3750 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].ZoneJungle || Main.hardMode) //if Jungle or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.VariegatedLardfish);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti);
					nextSlot++;
				}
			}
			//
			// Fishing rods
			//
			if (sellFishingRods && shop2)
			{
				shop.item[nextSlot].SetDefaults(ItemID.ReinforcedFishingPole);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
				nextSlot++;
				if ((NPC.downedBoss2 && WorldGen.crimson) || NPC.downedMoonlord)//EoW or BoC and Crimson world. Or defeated Moon Lord
				{
					shop.item[nextSlot].SetDefaults(ItemID.Fleshcatcher);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(150000 * shopMulti);
					nextSlot++;
				}
				if ((NPC.downedBoss2 && !WorldGen.crimson) || NPC.downedMoonlord)//EoW or BoC and Corruption world. Or defeated Moon Lord
				{
					shop.item[nextSlot].SetDefaults(ItemID.FisherofSouls);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(150000 * shopMulti);
					nextSlot++;
				}
				/*if (Main.bloodMoon)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BloodFishingRod);//Chum Caster
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(250000 * shopMulti);
					nextSlot++;
				}*/
				/*if (NPC.downedBoss1 && Main.player[Main.myPlayer].ZoneDesert)//EoC and in desert
				{
					shop.item[nextSlot].SetDefaults(ItemID.ScarabFishingRod);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(250000 * shopMulti);
					nextSlot++;
				}*/
				if (NPC.downedQueenBee)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FiberglassFishingPole);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(250000 * shopMulti);
					nextSlot++;
				}
				if (NPC.downedBoss3)//Skeletron
				{
					shop.item[nextSlot].SetDefaults(ItemID.MechanicsRod);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(300000 * shopMulti);
					nextSlot++;
				}
				if (NPC.downedBoss3 && NPCID.Count > 8)//Skeletron and more than 8 NPCs
				{
					shop.item[nextSlot].SetDefaults(ItemID.SittingDucksFishingRod);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(400000 * shopMulti);
					nextSlot++;
				}
				if (Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.HotlineFishingHook);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(450000 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].anglerQuestsFinished >= 30)
				{
					shop.item[nextSlot].SetDefaults(ItemID.GoldenFishingRod);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(500000 * shopMulti);
					nextSlot++;
				}
			}
			//
			// Other
			//
			if (sellExtraItems && shop2)
			{
				if (Main.player[Main.myPlayer].anglerQuestsFinished >= 1)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FishingPotion);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 * shopMulti);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.CratePotion);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 * shopMulti);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SonarPotion);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].anglerQuestsFinished >= 5)
				{
					// 0 Full Moon
					// 1 Waning Gibbous
					// 2 Third Quarter
					// 3 Waning Crescent
					// 4 New Moon
					// 5 Waxing Crescent
					// 6 First Quarter
					// 7 Waxing Gibbous
					/*if (Main.moonPhase == 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.ChumBucket);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(2500 * shopMulti);
						nextSlot++;
					}*/
					if (Main.moonPhase == 1)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Sextant);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 2)
					{
						shop.item[nextSlot].SetDefaults(ItemID.AnglerEarring);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 3)
					{
						shop.item[nextSlot].SetDefaults(ItemID.FishermansGuide);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					/*if (Main.moonPhase == 4)
					{
						shop.item[nextSlot].SetDefaults(ItemID.LavaFishingHook);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 * shopMulti);
						nextSlot++;
					}*/
					if (Main.moonPhase == 5)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HighTestFishingLine);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 6)
					{
						shop.item[nextSlot].SetDefaults(ItemID.WeatherRadio);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 7)
					{
						shop.item[nextSlot].SetDefaults(ItemID.TackleBox);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
				}
				if (Main.player[Main.myPlayer].anglerQuestsFinished >= 10)
				{
					/*if (Main.moonPhase == 4)
					{
						shop.item[nextSlot].SetDefaults(ItemID.ChumBucket);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(2500 * shopMulti);
						nextSlot++;
					}*/
					if (Main.moonPhase == 5)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Sextant);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 6)
					{
						shop.item[nextSlot].SetDefaults(ItemID.AnglerEarring);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 7)
					{
						shop.item[nextSlot].SetDefaults(ItemID.FishermansGuide);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					/*if (Main.moonPhase == 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.LavaFishingHook);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 * shopMulti);
						nextSlot++;
					}*/
					if (Main.moonPhase == 1)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HighTestFishingLine);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 2)
					{
						shop.item[nextSlot].SetDefaults(ItemID.WeatherRadio);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 3)
					{
						shop.item[nextSlot].SetDefaults(ItemID.TackleBox);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
				}
				if (Main.player[Main.myPlayer].anglerQuestsFinished >= 15)
				{
					/*if (Main.moonPhase == 2)
					{
						shop.item[nextSlot].SetDefaults(ItemID.ChumBucket);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(2500 * shopMulti);
						nextSlot++;
					}*/
					if (Main.moonPhase == 3)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Sextant);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 4)
					{
						shop.item[nextSlot].SetDefaults(ItemID.AnglerEarring);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 5)
					{
						shop.item[nextSlot].SetDefaults(ItemID.FishermansGuide);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					/*if (Main.moonPhase == 6)
					{
						shop.item[nextSlot].SetDefaults(ItemID.LavaFishingHook);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 * shopMulti);
						nextSlot++;
					}*/
					if (Main.moonPhase == 7)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HighTestFishingLine);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.WeatherRadio);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 1)
					{
						shop.item[nextSlot].SetDefaults(ItemID.TackleBox);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
				}
				if (Main.player[Main.myPlayer].anglerQuestsFinished >= 20)
				{
					/*if (Main.moonPhase == 6)
					{
						shop.item[nextSlot].SetDefaults(ItemID.ChumBucket);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(2500 * shopMulti);
						nextSlot++;
					}*/
					if (Main.moonPhase == 7)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Sextant);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.AnglerEarring);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 1)
					{
						shop.item[nextSlot].SetDefaults(ItemID.FishermansGuide);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					/*if (Main.moonPhase == 2)
					{
						shop.item[nextSlot].SetDefaults(ItemID.LavaFishingHook);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 * shopMulti);
						nextSlot++;
					}*/
					if (Main.moonPhase == 3)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HighTestFishingLine);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 4)
					{
						shop.item[nextSlot].SetDefaults(ItemID.WeatherRadio);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
					if (Main.moonPhase == 5)
					{
						shop.item[nextSlot].SetDefaults(ItemID.TackleBox);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
						nextSlot++;
					}
				}
				if (Main.player[Main.myPlayer].anglerQuestsFinished >= 10)
				{
					shop.item[nextSlot].SetDefaults(ItemID.AnglerHat);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].anglerQuestsFinished >= 15)
				{
					shop.item[nextSlot].SetDefaults(ItemID.AnglerVest);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 * shopMulti);
					nextSlot++;
				}
				if (Main.player[Main.myPlayer].anglerQuestsFinished >= 20)
				{
					shop.item[nextSlot].SetDefaults(ItemID.AnglerPants);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 * shopMulti);
					nextSlot++;
				}
				if (loadModdedItems)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.RecyclingMachine>());
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 * shopMulti);
					nextSlot++;
				}
				if (loadModdedItems && NPC.CountNPCS(NPCID.Clothier) > 0)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Fisherman_Vanity_Shirt>());
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Fisherman_Vanity_Pants>());
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti);
					nextSlot++;
				}
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return toKingStatue;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			if (!Main.hardMode)
			{
			damage = 25;
			}
			if (Main.hardMode && !NPC.downedMoonlord)
			{
			damage = 30;
			}
			if (NPC.downedMoonlord)
			{
			damage = 35;
			}
			knockback = 6f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = mod.ProjectileType("HarpoonSpear");
			//projType = ProjectileID.Harpoon;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 16f;
		}
		public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness) //Allows you to customize how this town NPC's weapon is drawn when this NPC is shooting (this NPC must have an attack type of 1). Scale is a multiplier for the item's drawing size, item is the ID of the item to be drawn, and closeness is how close the item should be drawn to the NPC.
		{
			//multiplier = 12f;
			//randomOffset = 2f;
			item = 160;//Harpoon
			scale = 1f;
			closeness = 7;
		}
	}
}