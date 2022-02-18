using RijamsMod.Items.Quest;
using RijamsMod.Items.Weapons;
using RijamsMod.Items.Accessories;
using RijamsMod.Items.Information;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace RijamsMod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class InterstellarTraveler : ModNPC
	{
		public override string Texture
		{
			get
			{
				return ModContent.GetInstance<RijamsModConfigClient>().Ornithophobia ? "RijamsMod/NPCs/TownNPCs/InterstellarTraveler_Helmet" : "RijamsMod/NPCs/TownNPCs/InterstellarTraveler";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return ModContent.GetInstance<RijamsModConfigClient>().Ornithophobia ? new string[] { "RijamsMod/NPCs/TownNPCs/InterstellarTraveler_Helmet" } : new string[] { "RijamsMod/NPCs/TownNPCs/InterstellarTraveler" };
			}
		}

		public override bool Autoload(ref string name)
		{
			name = "Interstellar Traveler";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Interstellar Traveler");
			Main.npcFrameCount[npc.type] = 26;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 1000;
			NPCID.Sets.AttackType[npc.type] = 1;
			NPCID.Sets.AttackTime[npc.type] = 30;
			NPCID.Sets.AttackAverageChance[npc.type] = 1;
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
			npc.defense = 60;//def 15
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			animationType = NPCID.Guide;
			Main.npcCatchable[npc.type] = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs;
			npc.catchItem = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtIntTrav>() : (short)-1;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/InterstellarTraveler_Gore_Head"), 1f);
				for (int k = 0; k < 1; k++)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/InterstellarTraveler_Gore_Arm"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/InterstellarTraveler_Gore_Leg"), 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedBoss2 && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) < 1) //EoW or BoC
			{
				if (RijamsModWorld.intTravArived) //That way you don't need the Odd Device in your inventory if the Interstellar Traveler has arrived once before.
				{
					return true;
				}
				else
				{
					for (int k = 0; k < 255; k++)
					{
						Player player = Main.player[k];
						if (player.active)
						{
							for (int j = 0; j < player.inventory.Length; j++)
							{
								if (player.inventory[j].type == ModContent.ItemType<OddDevice>()) //check if the player has the Odd Device in their inventory
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			return true;
		}

		public override string TownNPCName()
		{
			if (!Main.dedServ) RijamsModWorld.intTravArived = true; //Set the flag to true when a name is picked
			else RijamsModWorld.SetIntTravArived();
			string[] names = { "Tlani", "Cuia", "Cuemal", "Teztlal", "Nezal", "Zelelli", "Matlin", "Xoco", "Zillin", "Centia", "Citzil", "Malxoc", "Izta", "Xical", "Mazalch", "Tlazoh", "Checa", "Acnopan", "Uetlac", "Illi", "Zina" };
			return Main.rand.Next(names);
		}

        #region Chat
        public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int interTravel = NPC.FindFirstNPC(ModContent.NPCType<InterstellarTraveler>());
			chat.Add("I'm pretty far from home, but this place is pretty cool.");
			chat.Add("Nice to meet you!");
			chat.Add("I'm pretty lucky to have ended up on this planet. Not only is it inhabitable, but it also contains intelligent life!");
			chat.Add("I have a few things that I can sell you if you want to take a look.");
			chat.Add("Hi there! My name is " + Main.npc[interTravel].GivenName + ".");
			chat.Add("Hey, do you know where I could get some food?", 0.5);

			if (npc.life < npc.lifeMax * 0.5)
			{
				chat.Add("Ouch! I better apply some micronwraps...", 10.0);
			}
			if (Main.dayTime)
			{
				chat.Add(Main.raining ? "The rain is pleasant to watch; maybe not to stand in, though." : "Nice day today, isn't it?");
				chat.Add("This planet is very interesting.");
			}
			else
			{
				chat.Add(Main.bloodMoon ? "You better take shelter. There are some very strange creatures tonight." : "You better take shelter. I've seen strange creatures at night.");
				chat.Add("Well, time to relax inside.");
				chat.Add("I hope you have a weapon to defend yourself with.");
			}
			if (npc.homeless)
			{
				chat.Add("It's dangerous out here. Do you have a place where I could stay?");
			}
			else
			{
				chat.Add("Thanks for letting me stay here.");
			}
			if (Main.LocalPlayer.wingTimeMax > 0)
			{
				chat.Add("Woah! You have wings? And they work - like function? How did you get those? Are they heavy? How exhausting are they to use? Could I get a pair for myself? ...");
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add("Don't mind me, I'm just waiting for my next slice of cake.", 2.0);
				chat.Add("What's the occasion? Ah, it doesn't matter. Parties are fun!", 2.0);
			}

			if (NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 1) //more than one Interstellar Traveler
			{
				chat.Add("What? There two of me!? I have a lot of questions now. Is this your doing? Do you have some sort of divine powers that I wasn't aware of?", 5.0);
			}
			if (!NPC.downedBoss2 || RijamsModWorld.intTravArived == false) //spawn in the Interstellar Traveler before meeting the requirements
			{
				chat.Add("I'm not supposed to be here, yet. Is this your doing? Do you have some sort of divine powers that I wasn't aware of?", 2.0);
			}

			int harpy = NPC.FindFirstNPC(ModContent.NPCType<Harpy>());
			if (harpy >= 0)
			{
				chat.Add(Main.npc[harpy].GivenName + " and I have surprisingly similar biology. Yet, we are different in many ways.", 0.5);
				chat.Add(Main.npc[harpy].GivenName + "'s wings intrigue me. How much lift can she generate with them? How much energy does it take to continuously flap her wings? ...", 0.5);
				chat.Add("I really enjoy " + Main.npc[harpy].GivenName + "'s presence. Some of the things she says makes me laugh!", 0.5);
			}
			int fisherman = NPC.FindFirstNPC(ModContent.NPCType<Fisherman>());
			if (fisherman >= 0)
			{
				chat.Add("Do you think you could convince " + Main.npc[fisherman].GivenName + " to give me some fish?", 0.5);
			}
			int hellTrader = NPC.FindFirstNPC(ModContent.NPCType<HellTrader>());
			if (hellTrader >= 0 && RijamsModWorld.hellTraderArrivable && fisherman >= 0)
			{
				chat.Add("So, " + Main.npc[fisherman].GivenName + " gives " + Main.npc[hellTrader].GivenName + " a bunch of fish, but not me?", 0.5);
			}
			int guide = NPC.FindFirstNPC(NPCID.Guide);
			if (guide >= 0)
			{
				chat.Add(Main.npc[guide].GivenName + " seems to know a lot. Perhaps I could learn more about this planet from him.", 0.25);
				if (Main.npc[guide].GivenName == "Andrew")
				{
					chat.Add("Why would " + Main.npc[guide].GivenName + " carry that hat around if he never wears it?", 0.125);
				}
			}
			int merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (merchant >= 0)
			{
				chat.Add(Main.npc[merchant].GivenName + " refuses to sell me anything. That's fine by me, I don't need the primitive items he has on offer...", 0.25);
			}
			int nurse = NPC.FindFirstNPC(NPCID.Nurse);
			if (nurse >= 0 && Main.rand.Next(6) == 0)
			{
				chat.Add(Main.npc[nurse].GivenName + " is very experienced in her field. It's like she operates on somebody everyday!", 0.17);
			}
			int demolitionist = NPC.FindFirstNPC(NPCID.Demolitionist);
			if (demolitionist >= 0)
			{
				chat.Add(Main.npc[demolitionist].GivenName + " just lobs those grenades around without a care in the universe! He could seriously hurt somebody!", 0.25);
			}
			int dyeTrader = NPC.FindFirstNPC(NPCID.DyeTrader);
			if (dyeTrader >= 0)
			{
				chat.Add(Main.npc[dyeTrader].GivenName + " has some really strange dyes. How does he make them?", 0.17);
			}
			int angler = NPC.FindFirstNPC(NPCID.Angler);
			if (angler >= 0)
			{
				chat.Add(Main.npc[angler].GivenName + " keeps calling me names like 'Chicken Legs' or 'Bird Brain'. I hope he realizes I don't take offense to those phrases.", 0.25);
				chat.Add("Do you know who " + Main.npc[angler].GivenName + "'s parents are? Where are they?", 0.25);
			}
			if (angler >= 0 && fisherman >= 0)
			{
				chat.Add("Looking for these: [i:3120] , [i:3037] , [i:3096] ? Sorry, you're going to have to get them from " + Main.npc[angler].GivenName + " or " + Main.npc[fisherman].GivenName + ".", 0.75);
			}
			/*int zoologist = NPC.FindFirstNPC(NPCID.BestiaryGirl);
			if (zoologist >= 0)
			{
				if (Main.bloodMoon || Main.moonPhase == 0) //Blood Moon or Full Moon
				{
					chat.Add("WOAH! Have you seen " + Main.npc[zoologist].GivenName + "? Is she aware of this?", 0.25);
				}
				else
				{
					chat.Add(Main.npc[zoologist].GivenName + " really likes me for some reason. Do you know why?", 0.25);
				}
			}*/
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0)
			{
				chat.Add("How are you? I'm doing good myself. " + Main.npc[dryad].GivenName + " is very nice to me.", 0.25);
				chat.Add(Main.npc[dryad].GivenName + " has some sort of connection with nature — fascinating!", 0.25);
			}
			int painter = NPC.FindFirstNPC(NPCID.Painter);
			if (painter >= 0)
			{
				chat.Add("I recognize " + Main.npc[painter].GivenName + "'s talent. That is all I have to say.", 0.125);
				if (Main.npc[painter].GivenName == "Victor") //impossible in vanilla because Victor is not a name for the Painter
				{
					chat.Add("Sorry, not now. " + Main.npc[painter].GivenName + " and I are in an argument about whether something is a mouth or nose...", 0.125);
				}
			}
			/*int golfer = NPC.FindFirstNPC(NPCID.Golfer);
			if (golfer >= 0)
			{
				chat.Add(Main.npc[golfer].GivenName + " challenged me to get a par score on all eighteen holes. Now the question is: do I play fair?", 0.25);
			}*/
			int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (armsDealer >= 0)
			{
				chat.Add(Main.npc[armsDealer].GivenName + " took me to the range to show off his gun collection. He was very surprised to see how great of a shot I am.", 0.125);
			}
			int tavernkeep = NPC.FindFirstNPC(NPCID.DD2Bartender);
			if (tavernkeep >= 0)
			{
				chat.Add("So, " + Main.npc[tavernkeep].GivenName + " is from another land you say? Portals? Hm... I'm going to have to make a note of that...", 0.25);
			}
			int stylist = NPC.FindFirstNPC(NPCID.Stylist);
			if (stylist >= 0)
			{
				chat.Add("I don't have any hair for " + Main.npc[stylist].GivenName + " to style, but she is always welcome to preen the feathers on my head.", 0.17);
			}
			int goblinTinkerer = NPC.FindFirstNPC(NPCID.GoblinTinkerer);
			if (goblinTinkerer >= 0)
			{
				chat.Add(Main.npc[goblinTinkerer].GivenName + " has some crazy weird gadgets. I might have to try some out myself.", 0.125);
			}
			int witchDoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
			if (witchDoctor >= 0)
			{
				chat.Add(Main.npc[witchDoctor].GivenName + " is of Lihzahrd species? Fascinating, another intelligent species.", 0.25);
			}
			int clothier = NPC.FindFirstNPC(NPCID.Clothier);
			if (clothier >= 0)
			{
				chat.Add(Main.npc[clothier].GivenName + " had some sort of curse? Well I'm glad he is feeling better now...", 0.125);
				if (Main.npc[clothier].GivenName == "James")
				{
					chat.Add(Main.npc[clothier].GivenName + " has a very interesting couch; one that I have never seen before.", 0.125);
				}
			}
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
			if (mechanic >= 0)
			{
				chat.Add(Main.npc[mechanic].GivenName + " is fascinated with the technology that I have. I would be, too!", 0.17);
			}
			int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
			if (partyGirl >= 0)
			{
				chat.Add(Main.npc[partyGirl].GivenName + "'s dance moves are out of this world!", 0.17);
			}
			int wizard = NPC.FindFirstNPC(NPCID.Wizard);
			if (wizard >= 0)
			{
				chat.Add(Main.npc[wizard].GivenName + " keeps mistaking me for somebody else. The thing is, though, I look nothing like the other villagers here...", 0.17);
			}
			int taxCollector = NPC.FindFirstNPC(NPCID.TaxCollector);
			if (taxCollector >= 0)
			{
				chat.Add(Main.npc[taxCollector].GivenName + " refuses to accept my currency. Doesn't he know it's the way of the future?", 0.25);
			}
			int truffle = NPC.FindFirstNPC(NPCID.Truffle);
			if (truffle >= 0)
			{
				chat.Add(Main.npc[truffle].GivenName + " is proof that life is mysterious.", 0.125);
			}
			int pirate = NPC.FindFirstNPC(NPCID.Pirate);
			if (pirate >= 0)
			{
				chat.Add(Main.npc[pirate].GivenName + " keeps offering me crackers. I'm not going to refuse.", 0.25);
			}
			int steampunker = NPC.FindFirstNPC(NPCID.Steampunker);
			if (steampunker >= 0)
			{
				chat.Add("The Clentaminator that " + Main.npc[steampunker].GivenName + " invented is very powerful. I wonder if it has other uses...", 0.25);
				if (Main.npc[steampunker].GivenName == "Whitney")
				{
					chat.Add("The guitar that " + Main.npc[steampunker].GivenName + " owns is nice to listen to. It reminds me of simpler times.", 0.125);
				}
			}
			int cyborg = NPC.FindFirstNPC(NPCID.Cyborg);
			if (cyborg >= 0)
			{
				chat.Add("I heard " + Main.npc[cyborg].GivenName + " has an invisible building material, but I can't get him to sell them to me. Is the invisible building material out of stock or something?", 0.125);
			}
			int santa = NPC.FindFirstNPC(NPCID.SantaClaus);
			if (santa >= 0)
			{
				chat.Add(Main.npc[santa].GivenName + " is such a jolly fellow. No wonder humans enjoy this holiday!", 0.25);
			}
			/*int princess = NPC.FindFirstNPC(NPCID.Princess);
			if (princess >= 0)
			{
				chat.Add(Main.npc[princess].GivenName + " is so ecstatic! She calls me her 'friendly alien' some times. Heh, well I guess it's true!", 0.25);
				if (Main.npc[princess].GivenName == "Yorai")
				{
					chat.Add(Main.npc[princess].GivenName + " seems to know a lot about technology. They claim to have 'created' most of things on this planet which doesn't make sense to me.", 0.25);
				}
			}*/

			if (ModLoader.GetMod("SGAmod") != null) //SGAmod
			{
				int draken = NPC.FindFirstNPC(ModLoader.GetMod("SGAmod").NPCType("Dergon"));
				if (draken >= 0)
				{
					chat.Add("That Draken has a lot going through his head. He's a nice guy once you get to know him, though.", 0.25);
				}
			}
			if (ModLoader.GetMod("CalamityMod") != null) //Calamity
			{
				int seaKing = NPC.FindFirstNPC(ModLoader.GetMod("CalamityMod").NPCType("SEAHOE")); //Sea King
				if (seaKing >= 0)
				{
					chat.Add("I didn't expect to see somebody like Amidias! This planet is full of surprises!", 0.25);
				}
			}
			if (ModLoader.GetMod("ThoriumMod") != null) //Thorium
			{
				int cook = NPC.FindFirstNPC(ModLoader.GetMod("ThoriumMod").NPCType("Cook"));
				if (cook >= 0)
				{
					chat.Add("I am thankful to see somebody like " + Main.npc[cook].GivenName + "!", 0.33);
					if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
					{
						chat.Add("Whatever " + Main.npc[cook].GivenName + " is cooking smells wonderful!", 0.33);
					}
				}
				int blacksmith = NPC.FindFirstNPC(ModLoader.GetMod("ThoriumMod").NPCType("Blacksmith"));
				if (blacksmith >= 0)
				{
					chat.Add("I'm not sure what kind of Durasteel " + Main.npc[blacksmith].GivenName + " is working with, but it's certainly not the one I'm familiar with.", 0.25);
				}
			}
			if (ModLoader.GetMod("AlchemistNPC") != null) //Alchemist NPC
			{
				int brewer = NPC.FindFirstNPC(ModLoader.GetMod("AlchemistNPC").NPCType("Brewer"));
				if (brewer >= 0)
				{
					chat.Add(Main.npc[brewer].GivenName + " has all sorts of interesting potions. I might have to try some for myself.", 0.25);
				}
			}
			if (ModLoader.GetMod("AlchemistNPCLite") != null) //Alchemist NPC Lite
			{
				int brewer2 = NPC.FindFirstNPC(ModLoader.GetMod("AlchemistNPCLite").NPCType("Brewer"));
				if (brewer2 >= 0)
				{
					chat.Add(Main.npc[brewer2].GivenName + " has all sorts of interesting potions. I might have to try some for myself.", 0.25);
				}
			}
			if (ModLoader.GetMod("ExampleMod") != null) //Example Mod
			{
				int examplePerson = NPC.FindFirstNPC(ModLoader.GetMod("ExampleMod").NPCType("Example Person"));
				if (examplePerson >= 0)
				{
					chat.Add("I feel like I'm not supposed to see " + Main.npc[examplePerson].GivenName + ".", 0.25);
				}
			}
			if (ModLoader.GetMod("Tremor") != null) //Tremor Mod
			{
				int chef = NPC.FindFirstNPC(ModLoader.GetMod("Tremor").NPCType("Chef"));
				if (chef >= 0)
				{
					chat.Add(Main.npc[chef].GivenName + " acts a little strange - as if he doesn't want me to see the food he cooks.", 0.25);
				}
			}
			if (ModLoader.GetMod("HappinessRemoval") != null) //Happiness Removal
			{
				chat.Add("Thanks for removing happiness. Now, I am eternally unhappy.", 2.0);
			}
			return chat;
		}
        /*
			Future happiness notes:
				Liked Biome: Forest
				Disliked Biome: Underground, Cavern, Underworld
				Loved NPCs:
					Harpy (this mod)
					Cook (Thorium)
				
				Liked NPCs:
					Guide
					Dryad
					Princess
					Zoologist
					Pirate
					Mechanic
					Cyborg
					Steampunker
					Party Girl
					Draken (SGAmod)
				
				Disliked NPCs:
					Demolitionist
					Wizard
					Tax Collector
					Chef (Tremor)
				
				Hated NPCs:
					Merchant
			
			
			Other NPCs' thoughts:
				Loved by:
					Zoologist
					Princess
					Harpy (this mod)
				
				Liked by:
					Dryad
					Guide
					Mechanic
					Cyborg
					Steampunker
					Drunk Princess (Calamity)
				
				Disliked by:
					Painter
					Stylist
					Angler
				
				Hated by:
					Merchant
					Tax Collector
				
		*/
        #endregion

        public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28"); //Shop
			button2 = Language.GetTextValue("LegacyInterface.64"); //Quest
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
			{
				button2 = "Quest Checklist";
			}
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{

			if (firstButton)
			{
				shop = true;
			}
			if (!firstButton)
			{
				if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				{
					QuestSystemChecklist();
				}
				else
				{
					QuestSystem();
				}
			}
		}
		public bool AllQuestsCompleted()
		{
			if (RijamsModWorld.intTravQuestOddDevice == true && RijamsModWorld.intTravQuestBlankDisplay == true && RijamsModWorld.intTravQuestTPCore == true
				&& RijamsModWorld.intTravQuestMagicOxygenizer == true && RijamsModWorld.intTravQuestPrimeThruster == true) //Rye Jam quest is not needed
			{
				return true;
			}
			return false;
		}

        #region Quest System
        public void QuestSystem()
		{
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<OddDevice>()) && RijamsModWorld.intTravQuestOddDevice == false)
			{
				Main.npcChatText = "I was tracking that device you have. Where did you get it? Yeah... it's irresponsible of me to enter an uncontacted planet, but I didn't have much choice. Anyway, I'll buy that device from you for 2[i:73].";
				Main.npcChatCornerItem = ModContent.ItemType<OddDevice>();
				//int oddDeviceItemIndex = Main.LocalPlayer.FindItem(ModContent.ItemType<OddDevice>());
				//Main.LocalPlayer.inventory[oddDeviceItemIndex].TurnToAir(); //Currently consumes all the items in a stack (instead of 1).
				Main.LocalPlayer.ConsumeItem(ModContent.ItemType<OddDevice>());
				Main.LocalPlayer.QuickSpawnItem(ItemID.GoldCoin, 2);
				RijamsModWorld.intTravQuestOddDevice = true;
				if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.WorldData);
					//RijamsModWorld.SetIntTravQuestOddDevice();
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestOddDevice);
					//packet.Write((byte)npc.whoAmI);
					packet.Send();
				}
				mod.Logger.Debug("RijamsMod: Odd Device quest completed.");
				PlayCompleteQuestSound();
				return;
			}
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<BlankDisplay>()) && RijamsModWorld.intTravQuestBlankDisplay == false)
			{
				Main.npcChatText = "Ah, I could program this device to display certain useful aspects about yourself. Take a look at my shop if you would like to have one. I will randomly offer two of these displays every time you talk to me. And don't worry, I won't be selling your data or anything.";
				Main.npcChatCornerItem = ModContent.ItemType<BlankDisplay>();
				//int blankDisplayItemIndex = Main.LocalPlayer.FindItem(ModContent.ItemType<BlankDisplay>());
				//Main.LocalPlayer.inventory[blankDisplayItemIndex].TurnToAir(); //Currently consumes all the items in a stack (instead of 1).
				Main.LocalPlayer.ConsumeItem(ModContent.ItemType<BlankDisplay>());
				RijamsModWorld.intTravQuestBlankDisplay = true;
				if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.WorldData);
					//RijamsModWorld.SetIntTravQuestBlankDisplay();
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestBlankDisplay);
					//packet.Write((byte)npc.whoAmI);
					packet.Send();
				}
				mod.Logger.Debug("RijamsMod: Blank Display quest completed.");
				PlayCompleteQuestSound();
				return;
			}
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<TeleportationCore>()) && RijamsModWorld.intTravQuestTPCore == false)
			{
				Main.npcChatText = "This looks interesting! I bet I could use this to repair the hyper-drive on my ship. Its magical properties could let me travel even faster than before! I might even be able to create a device that can let you utilize its teleporting capabilities, too.";
				Main.npcChatCornerItem = ModContent.ItemType<TeleportationCore>();
				//int tPCoreItemIndex = Main.LocalPlayer.FindItem(ModContent.ItemType<TeleportationCore>());
				//Main.LocalPlayer.inventory[tPCoreItemIndex].TurnToAir(); //Currently consumes all the items in a stack (instead of 1).
				Main.LocalPlayer.ConsumeItem(ModContent.ItemType<TeleportationCore>());
				RijamsModWorld.intTravQuestTPCore = true;
				if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.WorldData);
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestTPCore);
					//packet.Write((byte)npc.whoAmI);
					packet.Send();
				}
				mod.Logger.Debug("RijamsMod: Teleportation Core quest completed.");
				PlayCompleteQuestSound();
				return;
			}
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<BreadAndJelly>()) && RijamsModWorld.intTravQuestRyeJam == false)
			{
				Main.npcChatText = "You're offering me food? Well, I'll never deny food. This bread and this jelly seem to be very high quality. Let me open this jar and slice this bread.\nMmmm... Thanks!";
				Main.npcChatCornerItem = ModContent.ItemType<BreadAndJelly>();
				//int breadAndJellyItemIndex = Main.LocalPlayer.FindItem(ModContent.ItemType<BreadAndJelly>());
				//Main.LocalPlayer.inventory[breadAndJellyItemIndex].TurnToAir(); //Currently consumes all the items in a stack (instead of 1).
				Main.LocalPlayer.ConsumeItem(ModContent.ItemType<BreadAndJelly>());
				RijamsModWorld.intTravQuestRyeJam = true;
				if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.WorldData);
					RijamsModWorld.intTravQuestRyeJam = true;
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestRyeJam);
					//packet.Write((byte)npc.whoAmI);
					packet.Send();
				}
				mod.Logger.Debug("RijamsMod: Rye Jam quest completed.");
				PlayCompleteQuestSound();
				return;
			}
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<MagicOxygenizer>()) && RijamsModWorld.intTravQuestMagicOxygenizer == false)
			{
				Main.npcChatText = "This machine seems to create oxygen from only electricity. How does it do that? Well it is magic, I guess. Anyway, I could use this on my ship and to create a personal breathing device!";
				Main.npcChatCornerItem = ModContent.ItemType<MagicOxygenizer>();
				Main.LocalPlayer.ConsumeItem(ModContent.ItemType<MagicOxygenizer>());
				RijamsModWorld.intTravQuestMagicOxygenizer = true;
				if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.WorldData);
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestMagicOxygenizer);
					packet.Send();
				}
				mod.Logger.Debug("RijamsMod: Magic Oxygenizer quest completed.");
				PlayCompleteQuestSound();
				return;
			}
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<PrimeThruster>()) && RijamsModWorld.intTravQuestPrimeThruster == false)
			{
				Main.npcChatText = "This is the perfect replacement for my ship's thrusters. It seems like the magic from the this planet has been very beneficial for repairing my ship. Oh, and you of course! Thanks!";
				Main.npcChatCornerItem = ModContent.ItemType<PrimeThruster>();
				Main.LocalPlayer.ConsumeItem(ModContent.ItemType<PrimeThruster>());
				RijamsModWorld.intTravQuestPrimeThruster = true;
				if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.WorldData);
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestPrimeThruster);
					packet.Send();
				}
				mod.Logger.Debug("RijamsMod: Prime Thruster quest completed.");
				PlayCompleteQuestSound();
				return;
			}
			else
			{			
				if (AllQuestsCompleted())
				{
					Main.npcChatCornerItem = ModContent.ItemType<QuestTrackerComplete>();
					string[] lines = { "It looks like you've found everything I needed, thanks!", "Nice job! You have collected and turned in everything I needed.", "With your help, I have everything I need to repair my ship! I quite like it here, though. I might stay a little longer!" };
					Main.npcChatText = lines[Main.rand.Next(lines.Length)];
				}
				else
				{
					Main.npcChatCornerItem = ModContent.ItemType<QuestTrackerIncomplete>();
					string[] lines = { "I'm looking for some specific items to repair my space ship. If you think have anything I'd be interested in, then feel free to talk to me.", "I need some items to repair my space ship. Do you think you could help me out?", "I'd be happy to take a look at other items, too; if you think I could use them for something.", "Hold LEFT SHIFT to check which quests you have completed and what I still need." };
					Main.npcChatText = lines[Main.rand.Next(lines.Length)];
				}
			}
		}
		public void QuestSystemChecklist()
		{
			bool intTravQuestOddDevice = RijamsModWorld.intTravQuestOddDevice;
			bool intTravQuestBlankDisplay = RijamsModWorld.intTravQuestBlankDisplay;
			bool intTravQuestTPCore = RijamsModWorld.intTravQuestTPCore;
			bool intTravQuestRyeJam = RijamsModWorld.intTravQuestRyeJam;
			bool intTravQuestMagicOxygenizer = RijamsModWorld.intTravQuestMagicOxygenizer;
			bool intTravQuestPrimeThruster = RijamsModWorld.intTravQuestPrimeThruster;

			//Strings for what items are still needed
			string OddDevice = "  Could I look at that device you have?";
			string BlankDisplay = "  I could use some sort of electronic screen.";
			string TPCore = "  I need to repair my hyper-drive.";
			string RyeJam = "  I'm feeling a little peckish... (heh)."; //Will never show in game
			string MagicOxygenizer = "  My oxygen supplier isn't working currently.";
			string PrimeThruster = "  My ship's thrusters are shot.";

			int numCompleted = 0;
			int numNeedTo = 0;
			bool secret = false;

			//Strings for the final message that will show up in game
			StringBuilder completed = new StringBuilder("Here is what you have completed so far:\n");
			StringBuilder needTo = new StringBuilder("Here is what is left to do:\n");
			StringBuilder completed2 = new StringBuilder("You have also completed:\n");
			StringBuilder finalChat = new StringBuilder("");
			string newLine = "\n";

			if (intTravQuestOddDevice)
			{
				//If completed, change the message to the item
				OddDevice = $"[i:{ModContent.ItemType<OddDevice>()}] ";
				//Add it to the end of the completed string
				completed.Append(OddDevice);
				//increment numCompleted (used later)
				numCompleted++;
			}
			else
			{
				//If not completed, add the message above to the end of the completed string
				needTo.Append(OddDevice);
				//Add a new line
				needTo.Append(newLine);
				//increment numNeedTo (used later)
				numNeedTo++;
			}
			if (intTravQuestBlankDisplay)
			{
				BlankDisplay = $"[i:{ModContent.ItemType<BlankDisplay>()}] ";
				completed.Append(BlankDisplay);
				numCompleted++;
			}
			else
			{
				needTo.Append(BlankDisplay);
				needTo.Append(newLine);
				numNeedTo++;
			}
			if (intTravQuestTPCore)
			{
				TPCore = $"[i:{ModContent.ItemType<TeleportationCore>()}] ";
				completed.Append(TPCore);
				numCompleted++;
			}
			else
			{
				needTo.Append(TPCore);
				needTo.Append(newLine);
				numNeedTo++;
			}
			if (intTravQuestRyeJam) //Secret quest so it acts different
			{
				RyeJam = $"[i:{ModContent.ItemType<BreadAndJelly>()}] ";
				completed2.Append(RyeJam);
				secret = true;
			}
			if (intTravQuestMagicOxygenizer)
			{
				MagicOxygenizer = $"[i:{ModContent.ItemType<MagicOxygenizer>()}] ";
				completed.Append(MagicOxygenizer);
				numCompleted++;
			}
			else
			{
				needTo.Append(MagicOxygenizer);
				needTo.Append(newLine);
				numNeedTo++;
			}
			if (intTravQuestPrimeThruster)
			{
				PrimeThruster = $"[i:{ModContent.ItemType<PrimeThruster>()}] ";
				completed.Append(PrimeThruster);
				numCompleted++;
			}
			else
			{
				needTo.Append(PrimeThruster);
				needTo.Append(newLine);
				numNeedTo++;
			}

			//Add everything to finalChat if applicable
			if (numCompleted > 0)
			{
				finalChat.Append(completed);
				finalChat.Append(newLine);
			}
			if (numNeedTo > 0)
			{
				finalChat.Append(needTo);
			}
			if (secret)
            {
				finalChat.Append(completed2);
			}

			Main.npcChatText = finalChat.ToString();
			if (AllQuestsCompleted())
            {
				Main.npcChatCornerItem = ModContent.ItemType<QuestTrackerComplete>();
			}
			else
            {
				Main.npcChatCornerItem = ModContent.ItemType<QuestTrackerIncomplete>();
			}
		}
		public void PlayCompleteQuestSound()
		{
			Main.PlaySound(SoundLoader.customSoundType, -1, -1, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/CelebrationJingle"));
		}
        #endregion
        #region Shop
        public override void SetupShop(Chest shop, ref int nextSlot)
		{
			int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (armsDealer > 0 && NPC.downedBoss3)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<InterstellarPistol>());
				nextSlot++;
			}
			if (NPC.downedPlantBoss)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<PlasmaRifle>());
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<InterstellarSMG>());
				nextSlot++;
			}
			int cyborg = NPC.FindFirstNPC(NPCID.Cyborg);
			if (cyborg > 0 && NPC.downedGolemBoss)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<AGMMissileLauncher>());
				nextSlot++;
			}
			if (NPC.downedMoonlord)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<InterstellarCarbine>());
				nextSlot++;
			}
			if (Main.hardMode)
            {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<SummonersGlove>());
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.GoldWatch);
			shop.item[nextSlot].shopCustomPrice = 10000;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PlatinumWatch);
			shop.item[nextSlot].shopCustomPrice = 10000;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.DepthMeter);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Compass);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Radar);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MetalDetector);
			nextSlot++;
			if (NPC.downedBoss3)
			{
				shop.item[nextSlot].SetDefaults(ItemID.TallyCounter);
				nextSlot++;
			}
			if (NPCID.Count > 4)
			{
				shop.item[nextSlot].SetDefaults(ItemID.LifeformAnalyzer);
				shop.item[nextSlot].shopCustomPrice = 55000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DPSMeter);
				shop.item[nextSlot].shopCustomPrice = 55000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Stopwatch);
				shop.item[nextSlot].shopCustomPrice = 55000;
				nextSlot++;
			}
			/*if (NPC.savedAngler && Main.player[Main.myPlayer].anglerQuestsFinished >= 1)
			{
				shop.item[nextSlot].SetDefaults(ItemID.FishermansGuide);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Sextant);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.WeatherRadio);
				nextSlot++;
			}*/
			if (RijamsModWorld.intTravQuestBlankDisplay)
			{
				switch (Main.rand.Next(0, 4))// randomly choose a shop item
				{
					case 0:
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<LifeDisplay>());
						shop.item[nextSlot].shopCustomPrice = 1000;
						nextSlot++;
						break;
					case 1:
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<ManaDisplay>());
						shop.item[nextSlot].shopCustomPrice = 1000;
						nextSlot++;
						break;
					case 2:
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<DefenseDisplay>());
						shop.item[nextSlot].shopCustomPrice = 1000;
						nextSlot++;
						break;
					default:
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<MovementDisplay>());
						shop.item[nextSlot].shopCustomPrice = 1000;
						nextSlot++;
						break;
				}
			}

			if (RijamsModWorld.intTravQuestBlankDisplay)
			{
				switch (Main.rand.Next(0, 3))// randomly choose a shop item
				{
					case 1:
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<DamageDisplay>());
						shop.item[nextSlot].shopCustomPrice = 1000;
						nextSlot++;
						break;
					case 2:
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<CritDisplay>());
						shop.item[nextSlot].shopCustomPrice = 1000;
						nextSlot++;
						break;
					default:
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<SummonsDisplay>());
						shop.item[nextSlot].shopCustomPrice = 1000;
						nextSlot++;
						break;
				}
			}
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<InformationInterface>()))
            {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.InformationInterfaceTile>());
				shop.item[nextSlot].shopCustomPrice = 5000;
				nextSlot++;
			}
			if (RijamsModWorld.intTravQuestTPCore)
			{
				shop.item[nextSlot].SetDefaults(ItemID.RodofDiscord);
				nextSlot++;
			}
			if (RijamsModWorld.intTravQuestRyeJam)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumables.RyeJam>());
				nextSlot++;
			}
			if (RijamsModWorld.intTravQuestMagicOxygenizer)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<BreathingPack>());
				shop.item[nextSlot].shopCustomPrice = 100000;
				nextSlot++;
			}
			if (RijamsModWorld.intTravQuestPrimeThruster)
            {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<RocketBooster>());
				shop.item[nextSlot].shopCustomPrice = 200000;
				nextSlot++;
			}

			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Pets.InterestingSphere>());
			nextSlot++;
			if (Main.moonPhase >= 6 && !Main.dayTime) //first quarter & waxing gibbous
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.IntTrav_Vanity_Helmet>());
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.IntTrav_Vanity_Chestplate>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.IntTrav_Vanity_Leggings>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.MusicBoxOSW>());
			nextSlot++;
			/*
			if (Main.LocalPlayer.GetModPlayer<ExamplePlayer>(mod).ZoneExample)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleWings"));
				nextSlot++;
			}
			}*/
		}
        #endregion

        /*public override void NPCLoot()
		{
			Item.NewItem(npc.getRect(), mod.ItemType<Items.Armor.ExampleCostume>());
		}*/

        // Make this Town NPC teleport to the King and/or Queen statue when triggered.
        public override bool CanGoToStatue(bool toKingStatue)
		{
			return !toKingStatue;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			if (!Main.hardMode)
			{
			damage = 30;
			}
			if (Main.hardMode && !NPC.downedMoonlord)
			{
			damage = 45;
			}
			if (NPC.downedMoonlord)
			{
			damage = 60;
			}
			knockback = 4f;
		}

		//private void UpdateAltTexture()

		/*public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			//Main.spriteBatch.End();
			return true;
		}*/
		/*public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			// As mentioned above, be sure not to forget this step.
			Main.spriteBatch.End();
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/TownNPCs/InterstellarTraveler_Arm"), new Vector2(0,0), null, Color.White);
		}*/

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 1;
			randExtraCooldown = 3;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ProjectileID.ChlorophyteBullet;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 16f;
		}
		
		public override void TownNPCAttackShoot(ref bool inBetweenShots)
		{
			inBetweenShots = true;
		}

		public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
		//Allows you to customize how this town NPC's weapon is drawn when this NPC is shooting (this NPC must have an attack type of 1).
		//Scale is a multiplier for the item's drawing size, item is the ID of the item to be drawn, and closeness is how close the item should be drawn to the NPC.
		{
			//multiplier = 12f;
			//randomOffset = 2f;
			if (!Main.hardMode)
			{
				item = ModContent.ItemType<InterstellarPistol>();
				//item = 164;//Handgun
				scale = 0.75f;
				closeness = 12;
			}
			if (Main.hardMode && !NPC.downedMoonlord)
			{
				item = ModContent.ItemType<InterstellarSMG>();
				//item = 1255;//Venus Magnum
				scale = 0.75f;
				closeness = 18;
			}
			if (NPC.downedMoonlord)
			{
				item = ModContent.ItemType<InterstellarCarbine>();
				//item = 3475;//Vortex Beater
				scale = 0.75f;
				closeness = 18;
			}
			
		}
	}
}