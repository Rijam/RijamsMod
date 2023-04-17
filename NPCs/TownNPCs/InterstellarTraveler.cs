using RijamsMod.Items.Quest;
using RijamsMod.Items.Weapons.Melee;
using RijamsMod.Items.Weapons.Magic;
using RijamsMod.Items.Weapons.Ranged;
using RijamsMod.Items.Weapons.Summon;
using RijamsMod.Items.Accessories;
using RijamsMod.Items.Information;
using RijamsMod.Items.Accessories.Summoner;
using RijamsMod.Items.Accessories.Ranger;
using RijamsMod.Items.Accessories.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Personalities;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.Audio;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI;
using System;

namespace RijamsMod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class InterstellarTraveler : ModNPC
	{
		private bool usedMicronWrap = false;
		private int usedMicronWrapTime = 0;

		private uint questExclamationTimer = 0;

		#region Set Defaults

		private const string ShopName = "Shop";
		internal static int ShimmerHeadIndex;
		internal static int HelmetHeadIndex;
		private static ITownNPCProfile NPCProfile;

		public override void Load()
		{
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, GetType().Namespace.Replace('.', '/') + "/Shimmered/" + Name + "_Head");
			HelmetHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Helmet_Head");
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Interstellar Traveler");
			Main.npcFrameCount[NPC.type] = 26;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 10;
			NPCID.Sets.AttackFrameCount[NPC.type] = 5;
			NPCID.Sets.DangerDetectRange[NPC.type] = 1000;
			NPCID.Sets.AttackType[NPC.type] = 1;
			NPCID.Sets.AttackTime[NPC.type] = 30; 
			NPCID.Sets.AttackAverageChance[NPC.type] = 1; // Lower numbers actually make the NPC more likely to attack
			NPCID.Sets.HatOffsetY[NPC.type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<Harpy>(), AffectionLevel.Love)
				//Love Cook (cross mod)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Like)
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Cyborg, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Like)
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<SnuggetPet.SnuggetPet>(), AffectionLevel.Like)
				//Like Draken (cross mod)
				//Like Martian Saucer (cross mod)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Painter, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Hate)
				//Princess is automatically set
			; // < Mind the semicolon!

			NPCProfile = new InterstellarTravelerProfile();
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 60;//def 15
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtIntTrav>() : -1;
		}
		#endregion

		#region Bestiary, Gore, Spawn, Names
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			});
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				if (NPC.IsShimmerVariant)
				{
					if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head_Alt_Shimmered").Type, 1f);
					}
					else
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head_Shimmered").Type, 1f);
					}
					for (int k = 0; k < 2; k++)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Arm_Shimmered").Type, 1f);
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Leg_Shimmered").Type, 1f);
					}
				}
				else
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head").Type, 1f);
					for (int k = 0; k < 2; k++)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Arm").Type, 1f);
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Leg").Type, 1f);
					}
				}			
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			if (NPC.downedBoss2 && NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) < 1) //EoW or BoC
			{
				if (RijamsModWorld.intTravArrived) //That way you don't need the Odd Device in your inventory if the Interstellar Traveler has arrived once before.
				{
					return true;
				}
				else
				{
					for (int k = 0; k < Main.maxPlayers; k++)
					{
						Player player = Main.player[k];
						if (player.active)
						{
							if (player.HasItem(ModContent.ItemType<OddDevice>())) //check if the player has the Odd Device in their inventory
							{
								return true;
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

		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}

		public override List<string> SetNPCNameList()
		{
			if (!Main.dedServ)
			{
				RijamsModWorld.intTravArrived = true; //Set the flag to true when a name is picked
			}
			else 
			{
				RijamsModWorld.intTravArrived = true;
				RijamsModWorld.UpdateWorldBool();
			}
			
			return new List<string>()
			{
				"Tlani", "Cuia", "Cuemal", "Teztlal", "Nezal", "Zelelli", "Matlin", "Xoco", "Zillin", "Centia", "Citzil", "Malxoc", "Izta", "Xical", "Mazalch", "Tlazoh", "Checa", "Acnopan", "Uetlac", "Illi", "Zina"
			};
		}
		#endregion

		#region AI
		public override void AI()
		{
			//Main.NewText("NPC.ai[0] " + NPC.ai[0]);
			//Main.NewText("NPC.ai[1] " + NPC.ai[1]);
			//Main.NewText("NPC.ai[2] " + NPC.ai[2]);
			//Main.NewText("NPC.ai[3] " + NPC.ai[3]);
			//Main.NewText("NPC.localAI[0] " + NPC.localAI[0]);
			//Main.NewText("usedMicronWrapTime " + usedMicronWrapTime);
			//Main.NewText("usedMicronWrap " + usedMicronWrap);

			// If not moving, is not going to change their action for another 60 ticks, hasn't already healed, and is below 25% HP
			if (NPC.ai[0] == 0 && NPC.ai[1] > 60 && !usedMicronWrap && NPC.life < NPC.lifeMax * 0.25f)
			{
				usedMicronWrap = true; // Set the bool to true to indicate that they have healed.
				usedMicronWrapTime = 3660; // Set the timer to 1 minute and 1 second. This is a cool down so the NPC can't heal all the time.
				EmoteBubble.NewBubble(EmoteID.ItemLifePotion, new WorldUIAnchor(NPC), 120); // Display a emote above their head.
				NPC.netUpdate = true;
			}
			if (usedMicronWrapTime > 0)
			{
				usedMicronWrapTime -= 1; // Decrease the cool down
			}
			if (usedMicronWrap && usedMicronWrapTime == 3600) // Wait exactly one second before healing
			{
				NPC.AddBuff(BuffID.Lovestruck, 60);
				NPC.life += 150;
				NPC.netUpdate = true;
			}
			if (usedMicronWrapTime == 0) // If the cool down hits 0, set the bool to false.
			{
				usedMicronWrap = false;
				NPC.netUpdate = true;
			}
			if (!CheckIfQuestIsAvailableToTurnIn(out _, out _))
			{
				questExclamationTimer = 0;
			}
		}
		#endregion

		#region PostDraw
		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> texture1 = ModContent.Request<Texture2D>("RijamsMod/NPCs/TownNPCs/InterstellarTraveler_Arm");
		private readonly Asset<Texture2D> texture2 = ModContent.Request<Texture2D>("RijamsMod/NPCs/TownNPCs/InterstellarTraveler_Casual_Arm");
		private readonly Asset<Texture2D> texture3 = ModContent.Request<Texture2D>("RijamsMod/NPCs/TownNPCs/Shimmered/InterstellarTraveler_Arm");
		private readonly Asset<Texture2D> questIcons = ModContent.Request<Texture2D>("RijamsMod/Items/Quest/QuestIcons");
		private readonly Asset<Texture2D> questQuestion = ModContent.Request<Texture2D>("RijamsMod/Items/Quest/Question");


		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Asset<Texture2D> drawTexture = texture1;
			if (NPC.IsShimmerVariant)
			{
				drawTexture = texture3;
			}
			if (NPC.altTexture == 1 && NPCHelper.AllQuestsCompleted())
			{
				drawTexture = texture2;
			}

			Color color = NPC.GetAlpha(drawColor);

			if (NPC.frame.Y > 20 * NPC.frame.Height) //Only draw while attacking
			{
				spriteBatch.Draw(drawTexture.Value, NPC.Center - screenPos - new Vector2(0, 4), NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 1f);
			}

			// Inspired by Magic Storage
			if (CheckIfQuestIsAvailableToTurnIn(out int which, out bool secret))
			{
				questExclamationTimer++;

				Texture2D icons = questIcons.Value;
				Texture2D question = questQuestion.Value;

				Rectangle iconsSource = icons.Frame(1, 6, 0, which);
				Rectangle questionSource = icons.Frame(1, 6, 0, questExclamationTimer % 60 < 30 ? 1 : 0);

				Vector2 center = NPC.Top - new Vector2(0, 32) - Main.screenPosition;

				double sin = Math.Sin(questExclamationTimer / 30d);
				center.Y += (float)sin * 5;
				float transparency = (float)(0.75 + 0.25 * sin);

				Color questionColor = Color.Gold;
				if (secret)
				{
					questionColor = Main.DiscoColor;
				}

				spriteBatch.Draw(icons, center, iconsSource, Color.White * transparency, 0, iconsSource.Size() / 2f, 1f, SpriteEffects.None, 0);
				spriteBatch.Draw(question, center, questionSource, questionColor * transparency, 0, questionSource.Size() / 2f, 1f, SpriteEffects.None, 0);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			// Force the frame to the attacking frame if attacking an aiming straight.
			if (NPC.ai[0] == 12 && NPC.frame.Y == 17 * frameHeight)
			{
				NPC.frame.Y = 23 * frameHeight;
			}
		}

		#endregion

		#region Chat
		public override string GetChat()
		{
			WeightedRandom<string> chat = new();

			bool townNPCsCrossModSupport = ModContent.GetInstance<RijamsModConfigServer>().TownNPCsCrossModSupport;

			int interTravel = NPC.FindFirstNPC(ModContent.NPCType<InterstellarTraveler>());
			NPCHelper.GetNearbyResidentNPCs(Main.npc[interTravel], 1, out List<int> npcTypeListHouse, out List<int> npcTypeListNearBy, out List<int> npcTypeListVillage, out List<int> _);

			chat.Add("I'm pretty far from home, but this place is pretty cool.");
			chat.Add("Nice to meet you!");
			chat.Add("I'm pretty lucky to have ended up on this planet. Not only is it inhabitable, but it also contains intelligent life!");
			chat.Add("I have a few things that I can sell you if you want to take a look.");
			chat.Add("Hi there! My name is " + Main.npc[interTravel].GivenName + ".");
			chat.Add("Hey, do you know where I could get some food?", 0.5);

			if (usedMicronWrap)
			{
				chat.Add("That was a close call. I had to use a micronwrap to heal myself!", 10.0);
			}
			else if (NPC.life < NPC.lifeMax * 0.5)
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
			if (NPC.homeless)
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
			if (!NPC.downedBoss2 || !RijamsModWorld.intTravArrived) //spawn in the Interstellar Traveler before meeting the requirements
			{
				chat.Add("I'm not supposed to be here, yet. Is this your doing? Do you have some sort of divine powers that I wasn't aware of?", 2.0);
			}

			int harpy = NPC.FindFirstNPC(ModContent.NPCType<Harpy>());
			int hellTrader = NPC.FindFirstNPC(ModContent.NPCType<HellTrader>());
			int angler = NPC.FindFirstNPC(NPCID.Angler);
			if (harpy >= 0)
			{
				chat.Add(Main.npc[harpy].GivenName + " and I have surprisingly similar biology. Yet, we are different in many ways.", npcTypeListVillage.Contains(ModContent.NPCType<Harpy>()) ? 1 : 0.5);
				chat.Add(Main.npc[harpy].GivenName + "'s wings intrigue me. How much lift can she generate with them? How much energy does it take to continuously flap her wings? ...", npcTypeListVillage.Contains(ModContent.NPCType<Harpy>()) ? 1 : 0.5);
				chat.Add("I really enjoy " + Main.npc[harpy].GivenName + "'s presence. Some of the things she says makes me laugh!", npcTypeListVillage.Contains(ModContent.NPCType<Harpy>()) ? 1 : 0.5);
			}
			if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC) && townNPCsCrossModSupport)
			{
				int fisherman = NPC.FindFirstNPC(fishermanNPC.Find<ModNPC>("Fisherman").Type);
				if (fisherman >= 0)
				{
					chat.Add("Do you think you could convince " + Main.npc[fisherman].GivenName + " to give me some fish?", npcTypeListVillage.Contains(fishermanNPC.Find<ModNPC>("Fisherman").Type) ? 1 : 0.5);
				}
				if (hellTrader >= 0 && RijamsModWorld.hellTraderArrivable && fisherman >= 0)
				{
					chat.Add("So, " + Main.npc[fisherman].GivenName + " gives " + Main.npc[hellTrader].GivenName + " a bunch of fish, but not me?", npcTypeListVillage.Contains(fishermanNPC.Find<ModNPC>("Fisherman").Type) ? 1 : 0.5);
				}
				/*if (angler >= 0 && fisherman >= 0)
				{
					chat.Add("Looking for these: [i:3120] , [i:3037] , [i:3096] ? Sorry, you're going to have to get them from " + Main.npc[angler].GivenName + " or " + Main.npc[fisherman].GivenName + ".", 0.75);
				}*/
			}
			int guide = NPC.FindFirstNPC(NPCID.Guide);
			if (guide >= 0 && npcTypeListVillage.Contains(NPCID.Guide))
			{
				chat.Add(Main.npc[guide].GivenName + " seems to know a lot. Perhaps I could learn more about this planet from him.");
				if (Main.npc[guide].GivenName == "Andrew")
				{
					chat.Add("Why would " + Main.npc[guide].GivenName + " carry that hat around if he never wears it?", 0.5);
				}
			}
			int merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (merchant >= 0 && npcTypeListVillage.Contains(NPCID.Merchant))
			{
				chat.Add(Main.npc[merchant].GivenName + " refuses to sell me anything. That's fine by me, I don't need the primitive items he has on offer...");
			}
			int nurse = NPC.FindFirstNPC(NPCID.Nurse);
			if (nurse >= 0 && npcTypeListHouse.Contains(NPCID.Nurse))
			{
				chat.Add(Main.npc[nurse].GivenName + " is very experienced in her field. It's like she operates on somebody everyday!");
			}
			int demolitionist = NPC.FindFirstNPC(NPCID.Demolitionist);
			if (demolitionist >= 0 && npcTypeListVillage.Contains(NPCID.Demolitionist))
			{
				chat.Add(Main.npc[demolitionist].GivenName + " just lobs those grenades around without a care in the universe! He could seriously hurt somebody!");
			}
			int dyeTrader = NPC.FindFirstNPC(NPCID.DyeTrader);
			if (dyeTrader >= 0 && npcTypeListHouse.Contains(NPCID.DyeTrader))
			{
				chat.Add(Main.npc[dyeTrader].GivenName + " has some really strange dyes. How does he make them?");
			}
			if (angler >= 0 && npcTypeListNearBy.Contains(NPCID.Angler))
			{
				chat.Add(Main.npc[angler].GivenName + " keeps calling me names like 'Chicken Legs' or 'Bird Brain'. I hope he realizes I don't take offense to those phrases.");
				chat.Add("Do you know who " + Main.npc[angler].GivenName + "'s parents are? Where are they?");
			}
			int zoologist = NPC.FindFirstNPC(NPCID.BestiaryGirl);
			if (zoologist >= 0 && npcTypeListVillage.Contains(NPCID.BestiaryGirl))
			{
				if (Main.bloodMoon || Main.moonPhase == 0) //Blood Moon or Full Moon
				{
					chat.Add("WOAH! Have you seen " + Main.npc[zoologist].GivenName + "? Is she aware of this?");
				}
				else
				{
					chat.Add(Main.npc[zoologist].GivenName + " really likes me for some reason. Do you know why?");
				}
			}
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0 && npcTypeListVillage.Contains(NPCID.Dryad))
			{
				chat.Add("How are you? I'm doing good myself. " + Main.npc[dryad].GivenName + " is very nice to me.");
				chat.Add(Main.npc[dryad].GivenName + " has some sort of connection with nature — fascinating!");
			}
			int painter = NPC.FindFirstNPC(NPCID.Painter);
			if (painter >= 0 && npcTypeListVillage.Contains(NPCID.Painter))
			{
				chat.Add("I recognize " + Main.npc[painter].GivenName + "'s talent. That is all I have to say.");
				if (Main.npc[painter].GivenName == "Victor") //impossible in vanilla because Victor is not a name for the Painter
				{
					chat.Add("Sorry, not now. " + Main.npc[painter].GivenName + " and I are in an argument about whether something is a mouth or nose...", 0.5);
				}
			}
			int golfer = NPC.FindFirstNPC(NPCID.Golfer);
			if (golfer >= 0 && npcTypeListHouse.Contains(NPCID.Golfer))
			{
				chat.Add(Main.npc[golfer].GivenName + " challenged me to get a par score on all eighteen holes. Now the question is: do I play fair?");
			}
			int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (armsDealer >= 0 && npcTypeListHouse.Contains(NPCID.ArmsDealer))
			{
				chat.Add(Main.npc[armsDealer].GivenName + " took me to the range to show off his gun collection. He was very surprised to see how great of a shot I am.");
			}
			int tavernkeep = NPC.FindFirstNPC(NPCID.DD2Bartender);
			if (tavernkeep >= 0 && npcTypeListNearBy.Contains(NPCID.DD2Bartender))
			{
				chat.Add("So, " + Main.npc[tavernkeep].GivenName + " is from another land you say? Portals? Hm... I'm going to have to make a note of that...");
			}
			int stylist = NPC.FindFirstNPC(NPCID.Stylist);
			if (stylist >= 0 && npcTypeListHouse.Contains(NPCID.Stylist))
			{
				chat.Add("I don't have any hair for " + Main.npc[stylist].GivenName + " to style, but she is always welcome to preen the feathers on my head.");
			}
			int goblinTinkerer = NPC.FindFirstNPC(NPCID.GoblinTinkerer);
			if (goblinTinkerer >= 0 && npcTypeListHouse.Contains(NPCID.GoblinTinkerer))
			{
				chat.Add(Main.npc[goblinTinkerer].GivenName + " has some crazy weird gadgets. I might have to try some out myself.");
			}
			int witchDoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
			if (witchDoctor >= 0 && npcTypeListNearBy.Contains(NPCID.WitchDoctor))
			{
				chat.Add(Main.npc[witchDoctor].GivenName + " is of Lihzahrd species? Fascinating, another intelligent species.");
			}
			int clothier = NPC.FindFirstNPC(NPCID.Clothier);
			if (clothier >= 0 && npcTypeListHouse.Contains(NPCID.Clothier))
			{
				chat.Add(Main.npc[clothier].GivenName + " had some sort of curse? Well I'm glad he is feeling better now...");
				if (Main.npc[clothier].GivenName == "James")
				{
					chat.Add(Main.npc[clothier].GivenName + " has a very interesting couch; one that I have never seen before.", 0.5);
				}
			}
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
			if (mechanic >= 0 && npcTypeListVillage.Contains(NPCID.Mechanic))
			{
				chat.Add(Main.npc[mechanic].GivenName + " is fascinated with the technology that I have. I would be, too!");
			}
			int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
			if (partyGirl >= 0 && npcTypeListVillage.Contains(NPCID.PartyGirl))
			{
				chat.Add(Main.npc[partyGirl].GivenName + "'s dance moves are out of this world!");
			}
			int wizard = NPC.FindFirstNPC(NPCID.Wizard);
			if (wizard >= 0 && npcTypeListVillage.Contains(NPCID.Wizard))
			{
				chat.Add(Main.npc[wizard].GivenName + " keeps mistaking me for somebody else. The thing is, though, I look nothing like the other villagers here...");
			}
			int taxCollector = NPC.FindFirstNPC(NPCID.TaxCollector);
			if (taxCollector >= 0 && npcTypeListVillage.Contains(NPCID.TaxCollector))
			{
				chat.Add(Main.npc[taxCollector].GivenName + " refuses to accept my currency. Doesn't he know it's the way of the future?");
			}
			int truffle = NPC.FindFirstNPC(NPCID.Truffle);
			if (truffle >= 0 && npcTypeListHouse.Contains(NPCID.Truffle))
			{
				chat.Add(Main.npc[truffle].GivenName + " is proof that life is mysterious.");
			}
			int pirate = NPC.FindFirstNPC(NPCID.Pirate);
			if (pirate >= 0 && npcTypeListNearBy.Contains(NPCID.Pirate))
			{
				chat.Add(Main.npc[pirate].GivenName + " keeps offering me crackers. I'm not going to refuse.");
			}
			int steampunker = NPC.FindFirstNPC(NPCID.Steampunker);
			if (steampunker >= 0 && npcTypeListVillage.Contains(NPCID.Steampunker))
			{
				chat.Add("The Clentaminator that " + Main.npc[steampunker].GivenName + " invented is very powerful. I wonder if it has other uses...");
				if (Main.npc[steampunker].GivenName == "Whitney")
				{
					chat.Add("The guitar that " + Main.npc[steampunker].GivenName + " owns is nice to listen to. It reminds me of simpler times.", 0.5);
				}
			}
			int cyborg = NPC.FindFirstNPC(NPCID.Cyborg);
			if (cyborg >= 0 && npcTypeListVillage.Contains(NPCID.Cyborg))
			{
				chat.Add("I heard " + Main.npc[cyborg].GivenName + " has an invisible building material and I'm really interested in such a material. Do you know when he will sell them?");
			}
			int santa = NPC.FindFirstNPC(NPCID.SantaClaus);
			if (santa >= 0 && npcTypeListHouse.Contains(NPCID.SantaClaus))
			{
				chat.Add(Main.npc[santa].GivenName + " is such a jolly fellow. No wonder humans enjoy this holiday!");
			}
			int princess = NPC.FindFirstNPC(NPCID.Princess);
			if (princess >= 0 && npcTypeListVillage.Contains(NPCID.Princess))
			{
				chat.Add(Main.npc[princess].GivenName + " is so ecstatic! She calls me her 'friendly alien' some times. Heh, well I guess it's true!");
				if (Main.npc[princess].GivenName == "Yorai")
				{
					chat.Add(Main.npc[princess].GivenName + " seems to know a lot about technology. They claim to have 'created' most of things on this planet which doesn't make sense to me.", 0.5);
				}
			}

			if (ModLoader.TryGetMod("SGAmod", out Mod sgamod) && townNPCsCrossModSupport) //SGAmod
			{
				if (sgamod.TryFind<ModNPC>("Dergon", out ModNPC drakenModNPC))
				{
					int draken = NPC.FindFirstNPC(drakenModNPC.Type);
					if (draken >= 0 && npcTypeListVillage.Contains(sgamod.Find<ModNPC>("Dergon").Type))
					{
						chat.Add("That Draken has a lot going through his head. He's a nice guy once you get to know him, though.");
					}
				}
			}
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) && townNPCsCrossModSupport) //Calamity
			{
				if (calamity.TryFind<ModNPC>("SEAHOE", out ModNPC seaKingModNPC))
				{
					int seaKing = NPC.FindFirstNPC(seaKingModNPC.Type); //Sea King
					if (seaKing >= 0 && npcTypeListNearBy.Contains(calamity.Find<ModNPC>("SEAHOE").Type))
					{
						chat.Add("I didn't expect to see somebody like Amidias! This planet is full of surprises!");
					}
				}
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && townNPCsCrossModSupport) //Thorium
			{
				if (thorium.TryFind<ModNPC>("Cook", out ModNPC cookModNPC))
				{
					int cook = NPC.FindFirstNPC(cookModNPC.Type);
					if (cook >= 0 && npcTypeListVillage.Contains(thorium.Find<ModNPC>("Cook").Type))
					{
						chat.Add("I am thankful to see somebody like " + Main.npc[cook].GivenName + "!");
						if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
						{
							chat.Add("Whatever " + Main.npc[cook].GivenName + " is cooking smells wonderful!");
						}
					}
				}
				if (thorium.TryFind<ModNPC>("Blacksmith", out ModNPC blacksmithModNPC))
				{
					int blacksmith = NPC.FindFirstNPC(blacksmithModNPC.Type);
					if (blacksmith >= 0 && npcTypeListNearBy.Contains(thorium.Find<ModNPC>("Blacksmith").Type))
					{
						chat.Add("I'm not sure what kind of Durasteel " + Main.npc[blacksmith].GivenName + " is working with, but it's certainly not the one I'm familiar with.");
					}
				}
			}
			if (ModLoader.TryGetMod("AlchemistNPC", out Mod alchemistNPC) && townNPCsCrossModSupport) //Alchemist NPC
			{
				if (alchemistNPC.TryFind<ModNPC>("Brewer", out ModNPC brewerModNPC))
				{
					int brewer = NPC.FindFirstNPC(brewerModNPC.Type);
					if (brewer >= 0 && npcTypeListNearBy.Contains(alchemistNPC.Find<ModNPC>("Brewer").Type))
					{
						chat.Add(Main.npc[brewer].GivenName + " has all sorts of interesting potions. I might have to try some for myself.");
					}
				}
			}
			if (ModLoader.TryGetMod("AlchemistNPCLite", out Mod alchemistNPCLite) && townNPCsCrossModSupport) //Alchemist NPC Lite
			{
				if (alchemistNPCLite.TryFind<ModNPC>("Brewer", out ModNPC brewer2ModNPC))
				{
					int brewer2 = NPC.FindFirstNPC(brewer2ModNPC.Type);
					if (brewer2 >= 0 && npcTypeListNearBy.Contains(alchemistNPCLite.Find<ModNPC>("Brewer").Type))
					{
						chat.Add(Main.npc[brewer2].GivenName + " has all sorts of interesting potions. I might have to try some for myself.");
					}
				}
			}
			if (ModLoader.TryGetMod("ExampleMod", out Mod exampleMod) && townNPCsCrossModSupport) //Example Mod
			{
				if (exampleMod.TryFind<ModNPC>("ExamplePerson", out ModNPC examplePersonModNPC))
				{
					int examplePerson = NPC.FindFirstNPC(examplePersonModNPC.Type);
					if (examplePerson >= 0 && npcTypeListNearBy.Contains(exampleMod.Find<ModNPC>("ExamplePerson").Type))
					{
						chat.Add("I feel like I'm not supposed to see " + Main.npc[examplePerson].GivenName + ".");
					}
				}
			}
			if (ModLoader.TryGetMod("HappinessRemoval", out Mod _) && townNPCsCrossModSupport) //Happiness Removal
			{
				chat.Add("Thanks for removing happiness. Now, I am eternally unhappy.", 2.0);
			}
			return chat;
		}
		#endregion

		#region Buttons
		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28"); //Shop
			button2 = Language.GetTextValue("LegacyInterface.64"); //Quest
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
			{
				button2 = "Quest Checklist";
			}
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				shop = ShopName;
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
		#endregion

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
				Main.LocalPlayer.QuickSpawnItem(NPC.GetSource_FromThis(), ItemID.GoldCoin, 2);
				RijamsModWorld.intTravQuestOddDevice = true;
				if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.WorldData);
					//RijamsModWorld.SetIntTravQuestOddDevice();
					ModPacket packet = Mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestOddDevice);
					//packet.Write((byte)npc.whoAmI);
					packet.Send();
				}
				Mod.Logger.Debug("RijamsMod: Odd Device quest completed.");
				PlayCompleteQuestSound(false);
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
					ModPacket packet = Mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestBlankDisplay);
					//packet.Write((byte)npc.whoAmI);
					packet.Send();
				}
				Mod.Logger.Debug("RijamsMod: Blank Display quest completed.");
				PlayCompleteQuestSound(false);
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
					ModPacket packet = Mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestTPCore);
					//packet.Write((byte)npc.whoAmI);
					packet.Send();
				}
				Mod.Logger.Debug("RijamsMod: Teleportation Core quest completed.");
				PlayCompleteQuestSound(false);
				return;
			}
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<BreadAndJelly>()) && RijamsModWorld.intTravQuestBreadAndJelly == false)
			{
				Main.npcChatText = "You're offering me food? Well, I'll never deny food. This bread and this jelly seem to be very high quality. Let me open this jar and slice this bread.\nMmmm... Thanks!";
				Main.npcChatCornerItem = ModContent.ItemType<BreadAndJelly>();
				//int breadAndJellyItemIndex = Main.LocalPlayer.FindItem(ModContent.ItemType<BreadAndJelly>());
				//Main.LocalPlayer.inventory[breadAndJellyItemIndex].TurnToAir(); //Currently consumes all the items in a stack (instead of 1).
				Main.LocalPlayer.ConsumeItem(ModContent.ItemType<BreadAndJelly>());
				RijamsModWorld.intTravQuestBreadAndJelly = true;
				if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.WorldData);
					RijamsModWorld.intTravQuestBreadAndJelly = true;
					ModPacket packet = Mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestBreadAndJelly);
					//packet.Write((byte)npc.whoAmI);
					packet.Send();
				}
				Mod.Logger.Debug("RijamsMod: Bread and Jelly quest completed.");
				PlayCompleteQuestSound(true);
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
					ModPacket packet = Mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestMagicOxygenizer);
					packet.Send();
				}
				Mod.Logger.Debug("RijamsMod: Magic Oxygenizer quest completed.");
				PlayCompleteQuestSound(false);
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
					ModPacket packet = Mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetQuestPrimeThruster);
					packet.Send();
				}
				Mod.Logger.Debug("RijamsMod: Prime Thruster quest completed.");
				PlayCompleteQuestSound(false);
				return;
			}
			else
			{			
				if (NPCHelper.AllQuestsCompleted())
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

		public static void QuestSystemChecklist()
		{
			bool intTravQuestOddDevice = RijamsModWorld.intTravQuestOddDevice;
			bool intTravQuestBlankDisplay = RijamsModWorld.intTravQuestBlankDisplay;
			bool intTravQuestTPCore = RijamsModWorld.intTravQuestTPCore;
			bool intTravQuestBreadAndJelly = RijamsModWorld.intTravQuestBreadAndJelly;
			bool intTravQuestMagicOxygenizer = RijamsModWorld.intTravQuestMagicOxygenizer;
			bool intTravQuestPrimeThruster = RijamsModWorld.intTravQuestPrimeThruster;

			//Strings for what items are still needed
			string OddDevice = "  Could I look at that device you have?";
			string BlankDisplay = "  I could use some sort of electronic screen.";
			string TPCore = "  I need to repair my hyper-drive.";
			string BreadAndJelly = "  I'm feeling a little peckish... (heh)."; //Will never show in game
			string MagicOxygenizer = "  My oxygen supplier isn't working currently.";
			string PrimeThruster = "  My ship's thrusters are shot.";

			int numCompleted = 0;
			int numNeedTo = 0;
			bool secret = false;

			//Strings for the final message that will show up in game
			StringBuilder completed = new(NPCHelper.AllQuestsCompleted() ? "You have completed all of my quests!\n" : "Here is what you have completed so far:\n");
			StringBuilder needTo = new("Here is what is left to do:\n");
			StringBuilder completed2 = new("You have also completed:\n");
			StringBuilder finalChat = new("");
			string newLine = "\n";

			if (intTravQuestOddDevice)
			{
				//If completed, change the message to the item
				//OddDevice = $"[i:{ModContent.ItemType<OddDevice>()}] ";
				OddDevice = "[i:RijamsMod/OddDevice] ";
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
			if (intTravQuestBreadAndJelly) //Secret quest so it acts different
			{
				BreadAndJelly = $"[i:{ModContent.ItemType<BreadAndJelly>()}] ";
				completed2.Append(BreadAndJelly);
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
			if (NPCHelper.AllQuestsCompleted())
			{
				Main.npcChatCornerItem = ModContent.ItemType<QuestTrackerComplete>();
			}
			else
			{
				Main.npcChatCornerItem = ModContent.ItemType<QuestTrackerIncomplete>();
			}
		}
		public void PlayCompleteQuestSound(bool secret)
		{
			// Play the second jingle if all the quests are completed
			if (NPCHelper.AllQuestsCompleted() && !secret) 
			{
				SoundEngine.PlaySound(new($"{nameof(RijamsMod)}/Sounds/Custom/CelebrationJingle2"));
			}
			else
			{
				SoundEngine.PlaySound(new($"{nameof(RijamsMod)}/Sounds/Custom/CelebrationJingle"));
			}
		}

		public static bool CheckIfQuestIsAvailableToTurnIn(out int which, out bool secret)
		{
			secret = false;
			which = 0;
			if (NPCHelper.AllQuestsCompleted() && RijamsModWorld.intTravQuestBreadAndJelly)
			{
				return false;
			}

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player searchPlayer = Main.player[i];
				if (!searchPlayer.active)
				{
					continue;
				}

				if (searchPlayer.HasItem(ModContent.ItemType<OddDevice>()) && !RijamsModWorld.intTravQuestOddDevice) { which = 0; return true; }
				if (searchPlayer.HasItem(ModContent.ItemType<BlankDisplay>()) && !RijamsModWorld.intTravQuestBlankDisplay) { which = 1; return true; }
				if (searchPlayer.HasItem(ModContent.ItemType<TeleportationCore>()) && !RijamsModWorld.intTravQuestTPCore) { which = 2; return true; }
				if (searchPlayer.HasItem(ModContent.ItemType<BreadAndJelly>()) && !RijamsModWorld.intTravQuestBreadAndJelly) { which = 3; secret = true; return true; }
				if (searchPlayer.HasItem(ModContent.ItemType<MagicOxygenizer>()) && !RijamsModWorld.intTravQuestMagicOxygenizer) { which = 4; return true; }
				if (searchPlayer.HasItem(ModContent.ItemType<PrimeThruster>()) && !RijamsModWorld.intTravQuestPrimeThruster) { which = 5; return true; }
			}
			return false;
		}

		#endregion

		#region Shop
		public override void AddShops()
		{
			var npcShop = new NPCShop(Type, ShopName)
				.Add(ModContent.ItemType<InterstellarPistol>(), Condition.DownedSkeletron, Condition.NpcIsPresent(NPCID.ArmsDealer))
				.Add(ModContent.ItemType<PlasmaRifle>(), Condition.DownedPlantera)
				.Add(ModContent.ItemType<InterstellarSMG>(), Condition.DownedPlantera)
				.Add(ModContent.ItemType<AGMMissileLauncher>(), Condition.DownedGolem, Condition.NpcIsPresent(NPCID.Cyborg))
				.Add(ModContent.ItemType<InterstellarSniper>(), Condition.DownedCultist)
				.Add(ModContent.ItemType<InterstellarCarbine>(), Condition.DownedMoonLord)
				.Add(ModContent.ItemType<ControlGlove>(), new Condition("After defeating Deerclops or in Hardmode", () => Condition.DownedDeerclops.IsMet() || Condition.Hardmode.IsMet()));

			Condition watchRandom(int numberToCheck) => new("Swaps between Watches at random", () => Main.GameUpdateCount % 2 == numberToCheck);
			npcShop.Add(new Item(ItemID.GoldWatch) { shopCustomPrice = 10000 }, watchRandom(0));
			npcShop.Add(new Item(ItemID.PlatinumWatch) { shopCustomPrice = 10000 }, watchRandom(1));
			npcShop.Add(ItemID.DepthMeter);
			npcShop.Add(ItemID.Compass);
			npcShop.Add(ItemID.Radar);
			npcShop.Add(ItemID.MetalDetector);
			npcShop.Add(ItemID.TallyCounter, Condition.DownedSkeletron);

			// Sell more of the Traveling Merchant's info items the more Town NPCs there are.
			Item lifeformAnalyzer = new(ItemID.LifeformAnalyzer) { shopCustomPrice = 55000 };
			Item dPSMeter = new(ItemID.DPSMeter) { shopCustomPrice = 55000 };
			Item stopwatch = new(ItemID.Stopwatch) { shopCustomPrice = 55000 };

			Condition TownNPCRange57 = new(ShopConditions.TownNPCRangeS("5-7"), () => NPCHelper.CountTownNPCs() >= 5 && NPCHelper.CountTownNPCs() <= 7);
			Condition TownNPCRange811 = new(ShopConditions.TownNPCRangeS("8-11"), () => NPCHelper.CountTownNPCs() >= 8 && NPCHelper.CountTownNPCs() <= 11);
			Condition TownNPCOver12 = new(ShopConditions.CountTownNPCsS(12), ShopConditions.CountTownNPCsFb(12));

			npcShop.Add(lifeformAnalyzer, ShopConditions.MoonPhase036, TownNPCRange57);
			npcShop.Add(dPSMeter, ShopConditions.MoonPhase147, TownNPCRange57);
			npcShop.Add(stopwatch, ShopConditions.MoonPhase25, TownNPCRange57);

			npcShop.Add(lifeformAnalyzer, ShopConditions.MoonPhase036, TownNPCRange811);
			npcShop.Add(dPSMeter, ShopConditions.MoonPhase036, TownNPCRange811);

			npcShop.Add(dPSMeter, ShopConditions.MoonPhase147, TownNPCRange811);
			npcShop.Add(stopwatch, ShopConditions.MoonPhase147, TownNPCRange811);

			npcShop.Add(stopwatch, ShopConditions.MoonPhase25, TownNPCRange811);
			npcShop.Add(lifeformAnalyzer, ShopConditions.MoonPhase25, TownNPCRange811);

			npcShop.Add(lifeformAnalyzer, TownNPCOver12);
			npcShop.Add(dPSMeter, TownNPCOver12);
			npcShop.Add(stopwatch, TownNPCOver12);

			npcShop.Add(ItemID.FishermansGuide, Condition.AnglerQuestsFinishedOver(1), Condition.NpcIsPresent(NPCID.Angler));
			npcShop.Add(ItemID.Sextant, Condition.AnglerQuestsFinishedOver(1), Condition.NpcIsPresent(NPCID.Angler));
			npcShop.Add(ItemID.WeatherRadio, Condition.AnglerQuestsFinishedOver(1), Condition.NpcIsPresent(NPCID.Angler));

			string displayRandom = "Swaps between the displays at random";
			Condition displayRandom4(int numberToCheck) => new(displayRandom, () => Main.GameUpdateCount % 4 == numberToCheck);
			Condition displayRandom3(int numberToCheck) => new(displayRandom, () => Main.GameUpdateCount % 3 == numberToCheck);

			npcShop.Add(new Item(ModContent.ItemType<LifeDisplay>()) { shopCustomPrice = 1000 },
				displayRandom4(0), ShopConditions.IntTravQuestBlankDisplay);
			npcShop.Add(new Item(ModContent.ItemType<ManaDisplay>()) { shopCustomPrice = 1000 },
				displayRandom4(1), ShopConditions.IntTravQuestBlankDisplay);
			npcShop.Add(new Item(ModContent.ItemType<DefenseDisplay>()) { shopCustomPrice = 1000 },
				displayRandom4(2), ShopConditions.IntTravQuestBlankDisplay);
			npcShop.Add(new Item(ModContent.ItemType<MovementDisplay>()) { shopCustomPrice = 1000 },
				displayRandom4(3), ShopConditions.IntTravQuestBlankDisplay);

			npcShop.Add(new Item(ModContent.ItemType<DamageDisplay>()) { shopCustomPrice = 1000 },
				displayRandom3(0), ShopConditions.IntTravQuestBlankDisplay);
			npcShop.Add(new Item(ModContent.ItemType<CritDisplay>()) { shopCustomPrice = 1000 },
				displayRandom3(1), ShopConditions.IntTravQuestBlankDisplay);
			npcShop.Add(new Item(ModContent.ItemType<SummonsDisplay>()) { shopCustomPrice = 1000 },
				displayRandom3(2), ShopConditions.IntTravQuestBlankDisplay);

			npcShop.Add(new Item(ModContent.ItemType<Items.Placeable.InformationInterfaceTile>()) { shopCustomPrice = 5000 },
				Condition.PlayerCarriesItem(ModContent.ItemType<InformationInterface>()));

			npcShop.Add(ItemID.RodofDiscord, ShopConditions.IntTravQuestOddDevice);
			npcShop.Add(ModContent.ItemType<Items.Consumables.RyeJam>(), ShopConditions.IntTravQuestBreadAndJelly);
			npcShop.Add(ModContent.ItemType<BreathingPack>(), ShopConditions.IntTravQuestMagicOxygenizer);
			npcShop.Add(ModContent.ItemType<RocketBooster>(), ShopConditions.IntTravQuestPrimeThruster);
			npcShop.Add(ModContent.ItemType<Items.Consumables.ReefCola>(), Condition.DownedDukeFishron);
			npcShop.Add(ModContent.ItemType<Items.Pets.InterestingSphere>());
			npcShop.Add(ModContent.ItemType<Items.Pets.FluffaloEgg>(), Condition.Hardmode);
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.IntTrav.IntTrav_Helmet>(), ShopConditions.IsNotNpcShimmered);
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.IntTrav.IntTrav_Chestplate>(), ShopConditions.IsNotNpcShimmered);
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.IntTrav.IntTrav_Leggings>(), ShopConditions.IsNotNpcShimmered);
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.IntTrav.PeacekeeperHat>(), Condition.IsNpcShimmered);
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.IntTrav.PeacekeeperShirt>(), Condition.IsNpcShimmered);
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.IntTrav.PeacekeeperTrousers>(), Condition.IsNpcShimmered);
			npcShop.Add(ModContent.ItemType<Items.Placeable.MusicBoxOSW>());
			npcShop.Register();
		}
		#endregion

		#region Misc and Attack
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

		public override void DrawTownAttackGun(ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)
		//Allows you to customize how this town NPC's weapon is drawn when this NPC is shooting (this NPC must have an attack type of 1).
		//Scale is a multiplier for the item's drawing size, item is the ID of the item to be drawn, and closeness is how close the item should be drawn to the NPC.
		{
			Main.NewText("NPC.ai[0] " + NPC.ai[0] + " NPC.frame.Y " + NPC.frame.Y + " NPC.frame.Height " + NPC.frame.Height);

			//multiplier = 12f;
			//randomOffset = 2f;
			if (!Main.hardMode)
			{
				Main.GetItemDrawFrame(ModContent.ItemType<InterstellarPistol>(), out Texture2D itemTexture, out Rectangle itemRectangle);
				item = itemTexture;
				itemFrame = itemRectangle;
				scale = 0.75f;
				horizontalHoldoutOffset = -20;
			}
			else if (Main.hardMode && !NPC.downedMoonlord)
			{
				Main.GetItemDrawFrame(ModContent.ItemType<InterstellarSMG>(), out Texture2D itemTexture, out Rectangle itemRectangle);
				item = itemTexture;
				itemFrame = itemRectangle;
				scale = 0.75f;
				horizontalHoldoutOffset = -34;
			}
			else if (NPC.downedMoonlord)
			{
				Main.GetItemDrawFrame(ModContent.ItemType<InterstellarCarbine>(), out Texture2D itemTexture, out Rectangle itemRectangle);
				item = itemTexture;
				itemFrame = itemRectangle;
				scale = 0.75f;
				horizontalHoldoutOffset = -34;
			}
		}

		#endregion
	}
	#region ITownNPCProfile
	public class InterstellarTravelerProfile : ITownNPCProfile
	{
		private string Namespace => GetType().Namespace.Replace('.', '/');
		private string NPCName => (GetType().Name.Split("Profile")[0]).Replace('.', '/');
		private string Path => (Namespace + "/" + NPCName);

		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (ModContent.GetInstance<RijamsModConfigClient>().Ornithophobia)
			{
				return ModContent.Request<Texture2D>(Path + "_Helmet");
			}
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
			{
				return ModContent.Request<Texture2D>(Path);
			}
			if (npc.altTexture == 1 && NPCHelper.AllQuestsCompleted())
			{
				return ModContent.Request<Texture2D>(Path + "_Casual");
			}

			if (npc.IsShimmerVariant && npc.altTexture != 1)
			{
				return ModContent.Request<Texture2D>(Namespace + "/Shimmered/" + NPCName);
			}
			if (npc.IsShimmerVariant && npc.altTexture == 1)
			{
				return ModContent.Request<Texture2D>(Namespace + "/Shimmered/" + NPCName + "_Hatless");
			}

			return ModContent.Request<Texture2D>(Path);
		}

		public int GetHeadTextureIndex(NPC npc)
		{
			if (ModContent.GetInstance<RijamsModConfigClient>().Ornithophobia)
			{
				return InterstellarTraveler.HelmetHeadIndex;
			}
			if (npc.IsShimmerVariant)
			{
				return InterstellarTraveler.ShimmerHeadIndex;
			}
			return ModContent.GetModHeadSlot(Path + "_Head");
		}
	}
	#endregion
}