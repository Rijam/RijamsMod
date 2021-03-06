using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using RijamsMod.Items;
using RijamsMod.Items.Accessories.Movement;
using RijamsMod.Projectiles;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.Bestiary;

namespace RijamsMod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Harpy : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Harpy");
			Main.npcFrameCount[NPC.type] = 25;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
			NPCID.Sets.AttackFrameCount[NPC.type] = 4;
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;
			NPCID.Sets.AttackType[NPC.type] = 0;
			NPCID.Sets.AttackTime[NPC.type] = 90;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;

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
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<InterstellarTraveler>(), AffectionLevel.Love)
				//Love Fisherman (cross mod)
				.SetNPCAffection(NPCID.Pirate, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Stylist, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Golfer, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Painter, AffectionLevel.Like)
				.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Like)
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<HellTrader>(), AffectionLevel.Like)
				//Like Draken (cross mod)
				.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Angler, AffectionLevel.Hate)
				//Princess is automatically set
			; // < Mind the semicolon!
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;//def 15
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtHarpy>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			});
		}

		public override void OnKill()
		{
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
			for (int k = 0; k < 2; k++)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (RijamsModWorld.savedHarpy && NPC.CountNPCS(ModContent.NPCType<Harpy>()) < 1)
			{
				return true;
            }
			else
			{
				return false;
			}
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			return true;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return new HarpyProfile();
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string>()
			{
				"Pinnia", "Anisoa", "Remexa", "Lyre", "Ornitha", "Talia", "Mythia", "Aera", "Allegora", "Faybli", "Chimerica", "Whimsica", "Disparate", "Ourana", "Aetha", "Eria"
			};
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new();

			bool townNPCsCrossModSupport = ModContent.GetInstance<RijamsModConfigServer>().TownNPCsCrossModSupport;

			int harpy = NPC.FindFirstNPC(ModContent.NPCType<Harpy>());
			NPCHelper.GetNearbyResidentNPCs(Main.npc[harpy], 1, out List<int> _, out List<int> _, out List<int> npcTypeListVillage, out List<int> _);

			chat.Add("Don't attack, please!");
			chat.Add("Friends? I am friendly.");
			chat.Add("I think I am different...");
			chat.Add("Buy items from me?");
			chat.Add("Hi, I'm " + Main.npc[harpy].GivenName + ".");
			chat.Add("Flying is fun and all, but I sometimes wish I had hands.");
			chat.Add("Hm? Chicken Nuggets?", 0.5);
			
			if (NPC.life < NPC.lifeMax * 0.5)
			{
				chat.Add("Help! I'm hurt!", 10.0);
			}
			if (Main.dayTime)
			{
				chat.Add(Main.raining ? "Rain makes flying hard." : "I like sunny days!");
				chat.Add("Come bask in the sun with me?");
			}
			else
			{
				chat.Add("Scary monsters at night. Am I a monster?");
				chat.Add("Protect me from monsters?");
				chat.Add(Main.raining ? "I am afraid of thunder." : "No sun shining makes it hard to see.");
			}
			if (NPC.homeless)
			{
				chat.Add("Can I get a nest of my own?");
			}
			else
			{
				chat.Add("This nest is very nice. Thank you!");
			}
			if (Main.LocalPlayer.wingTimeMax > 0)
			{
				chat.Add("Wings? Now you can soar through the sky like me!");
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
				chat.Add("I'm not used to wearing anything on my head!", 2.0);
				chat.Add("Parties are fun! We should do one every day!", 2.0);
			}
			if (Main.bloodMoon || Main.eclipse)
			{
				chat.Add("AH! Stay back! Oh, its just you.", 2.0);
				chat.Add("Is it safe? I am very scared!", 2.0);
			}
			if (Main.LocalPlayer.HasItem(ItemID.BirdieRattle))
			{
				chat.Add("Aww, isn't that little harpy so cute? Wait, what do you mean pet?");
			}
			if (Main.LocalPlayer.HasItem(ItemID.HarpyBanner))
			{
				chat.Add(Language.GetTextValue("That's so cool! You made a banner just for me?"), 5.0);
			}
			if ((Main.LocalPlayer.HasItem(ItemID.TallyCounter) || Main.LocalPlayer.HasItem(ItemID.REK) || Main.LocalPlayer.HasItem(ItemID.PDA) || Main.LocalPlayer.HasItem(ItemID.CellPhone)) && Main.player[Main.myPlayer].lastCreatureHit == Item.NPCtoBanner(NPCID.Harpy))
			//The player has the Tally Counter, R.E.K. 3000, PDA, or Cellphone in their inventory. The last enemy they hit was a Harpy and the kill count for Harpies is more than 0.
			//Item.NPCtoBanner(NPCID.Harpy) == 44
			{
				if (NPC.killCount[Item.NPCtoBanner(NPCID.Harpy)] > 0)
				{
					chat.Add(Language.GetTextValue("Why does that device that you have say \"Harpy: " + NPC.killCount[Item.NPCtoBanner(NPCID.Harpy)] + "\"?"), 20.0);
				}
			}
			if (ModLoader.TryGetMod("Joostmod", out Mod joostMod) && townNPCsCrossModSupport) //Joostmod
            {
				if (Main.LocalPlayer.HasBuff(joostMod.Find<ModBuff>("HarpyMinion").Type))
				{
					chat.Add(Language.GetTextValue("Aww, isn't that little harpy so cute? Wait, what do you mean minion?"));
				}
			}
			if (RijamsModWorld.savedHarpy == false) //spawn in the Harpy before finding her
			{
				chat.Add("Wait, how did I get here? Where did I come from?", 2.0);
			}
			int interTravel = NPC.FindFirstNPC(ModContent.NPCType<InterstellarTraveler>());
			if (interTravel >= 0)
			{
				chat.Add("Me and " + Main.npc[interTravel].GivenName + " are like each other, but also not.", 0.5);
				chat.Add("" + Main.npc[interTravel].GivenName + " really likes my wings. Why doesn't she have any if she is a bird?", 0.5);
			}
			if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC) && townNPCsCrossModSupport)
			{
				int fisherman = NPC.FindFirstNPC(fishermanNPC.Find<ModNPC>("Fisherman").Type);
				if (fisherman >= 0)
				{
					chat.Add("Me and " + Main.npc[fisherman].GivenName + " go on fishing trips sometimes! He lets me 'scout ahead', whatever that means!", 0.5);
				}
			}
			int hellTrader = NPC.FindFirstNPC(ModContent.NPCType<HellTrader>());
			if (hellTrader >= 0 && RijamsModWorld.hellTraderArrivable)

			{
				chat.Add("Me and " + Main.npc[hellTrader].GivenName + " come from opposite heights of the world! Isn't that cool!", 0.5);
			}
			int zoologist = NPC.FindFirstNPC(NPCID.BestiaryGirl);
			if (zoologist >= 0 && npcTypeListVillage.Contains(NPCID.BestiaryGirl))
			{
				chat.Add("" + Main.npc[zoologist].GivenName + " is so nice to me! I like being around her.");
			}
			int angler = NPC.FindFirstNPC(NPCID.Angler);
			if (angler >= 0 && npcTypeListVillage.Contains(NPCID.Angler))
			{
				chat.Add("" + Main.npc[angler].GivenName + " is so mean! He keeps calling me names like 'Chicken Legs' or 'Bird Brain'!");
			}
			int pirate = NPC.FindFirstNPC(NPCID.Pirate);
			if (pirate >= 0 && npcTypeListVillage.Contains(NPCID.Pirate))
			{
				chat.Add("" + Main.npc[pirate].GivenName + " lets me sit in the crows nest!");
			}
			if (ModLoader.TryGetMod("SGAmod", out Mod sgamod) && townNPCsCrossModSupport) //SGAmod
			{	
				int draken = NPC.FindFirstNPC(sgamod.Find<ModNPC>("Dergon").Type);
				if (draken >= 0 && npcTypeListVillage.Contains(sgamod.Find<ModNPC>("Dergon").Type))
				{
					chat.Add("I asked Draken if he wants to go flying, but he said he didn't want to. Why not?", 0.5);
				}
			}
			if (ModLoader.TryGetMod("SacredTools", out Mod _) && townNPCsCrossModSupport) //Shadows of Abaddon
			{
				chat.Add("Jensen? Raynare? Sorry, I don't know what you are talking about.", 0.5);
			}
			if (ModLoader.TryGetMod("AAMod", out Mod _) && townNPCsCrossModSupport) //Ancients Awakened
			{
				chat.Add("Athena? Sorry, I don't know what you are talking about.", 0.5);
			}
			if (ModLoader.TryGetMod("Varia", out Mod _) && townNPCsCrossModSupport) //Varia
			{
				chat.Add("Fallen Angel? Sorry, I don't know what you are talking about.", 0.5);
			}
			if (ModLoader.TryGetMod("pinkymod", out Mod _) && townNPCsCrossModSupport) //Pinky Mod
			{
				chat.Add("Valdaris? Sorry, I don't know what you are talking about.", 0.5);
			}
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28"); //Shop
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
			}
		}

		/*public bool SacredToolsDownedHarpyPreHM
		{
			get { return SacredTools.ModdedWorld.downedHarpy; }
		}
		public bool SacredToolsDownedHarpyHM
		{
			get { return SacredTools.ModdedWorld.downedRaynare; }
		}
		public bool AAModDownedAthena
		{
			get { return AAMod.AAWorld.downedAthena; }
		}
		public bool PinkymodDownedST
		{
			get { return pinkymod.Global.Pinkyworld.downedSunlightTrader; }
		}*/ //from Alchemist NPC

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<RijamsModConfigServer>().TownNPCsCrossModSupport;
			NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Harpy>())], 3, out List<int> _, out List<int> _, out List<int> _, out List<int> npcTypeListAll);

			shop.item[nextSlot].SetDefaults(ItemID.Feather);
			nextSlot++;
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && townNPCsCrossModSupport)
			{
				shop.item[nextSlot].SetDefaults(calamityMod.Find<ModItem>("DesertFeather").Type);
				nextSlot++;
				if ((bool)calamityMod.Call("GetBossDowned", "dragonfolly"))
                {
					shop.item[nextSlot].SetDefaults(calamityMod.Find<ModItem>("EffulgentFeather").Type);
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (ModLoader.TryGetMod("SacredTools", out Mod shadowsOfAbaddon) && townNPCsCrossModSupport) //Shadows of Abaddon
			{
				shop.item[nextSlot].SetDefaults(shadowsOfAbaddon.Find<ModItem>("BirdFeather").Type); //White Feather
				shop.item[nextSlot].shopCustomPrice = 50;
				nextSlot++;
				
				/*if (SacredToolsDownedHarpyPreHM) //Jensen
				{
					//shop.item[nextSlot].SetDefaults(ModLoader.TryGetMod("SacredTools").ItemType("GrandHarpyFeather"));
					shop.item[nextSlot].SetDefaults(ModLoader.TryGetMod("SacredTools").ItemType("HarpyDrop")); //Grand Harpy Feather
					shop.item[nextSlot].shopCustomPrice = 10000;
					nextSlot++;
				}
				if (SacredToolsDownedHarpyHM) //Raynare
				{
					//shop.item[nextSlot].SetDefaults(ModLoader.TryGetMod("SacredTools").ItemType("RoyalHarpyFeather"));
					shop.item[nextSlot].SetDefaults(ModLoader.TryGetMod("SacredTools").ItemType("GoldenFeather")); //Royal Harpy Feather
					shop.item[nextSlot].shopCustomPrice = 20000;
					nextSlot++;
				}*/
			}
			if (ModLoader.TryGetMod("AAMod", out Mod ancientsAwakened) && townNPCsCrossModSupport) //Ancients Awakened
			{
				shop.item[nextSlot].SetDefaults(ancientsAwakened.Find<ModItem>("vulture_feather").Type);
				shop.item[nextSlot].shopCustomPrice = 5000;
				nextSlot++;
				if (NPC.downedPlantBoss)
				{
					shop.item[nextSlot].SetDefaults(ancientsAwakened.Find<ModItem>("SeraphFeather").Type);
					shop.item[nextSlot].shopCustomPrice = 10000;
					nextSlot++;
				}
				/*if (AAModDownedAthena) //Athena
				{
					shop.item[nextSlot].SetDefaults(ModLoader.TryGetMod("AAMod").ItemType("GoddessFeather"));
					shop.item[nextSlot].shopCustomPrice = 150000;
					nextSlot++;
				}
				*/
			}
			/*Mod pinkyMod = ModLoader.TryGetMod("pinkymod");
			if (pinkyMod != null) //Pinky Mod
			{
				if (PinkymodDownedST) //Sunlight Trader
				{
					shop.item[nextSlot].SetDefaults(ModLoader.TryGetMod("PinkyMod").ItemType("SunsweptFeather"));
					shop.item[nextSlot].shopCustomPrice = 100000;
					nextSlot++;
				}
			}*/
			if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && townNPCsCrossModSupport) //Orchid Mod
			{
				if (NPC.downedBoss1)
				{
					shop.item[nextSlot].SetDefaults(orchidMod.Find<ModItem>("HarpyTalon").Type);
					shop.item[nextSlot].shopCustomPrice = 2000;
					nextSlot++;
				}
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && townNPCsCrossModSupport) //Thorium
			{
				shop.item[nextSlot].SetDefaults(thorium.Find<ModItem>("BirdTalon").Type); //Talon
				shop.item[nextSlot].shopCustomPrice = 100;
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.SkyMill);
			shop.item[nextSlot].shopCustomPrice = 17500;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.LuckyHorseshoe);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.CreativeWings); //Fledgling Wings
			shop.item[nextSlot].shopCustomPrice = 80000;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Starfury);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ShinyRedBalloon);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SunplateBlock);
			shop.item[nextSlot].shopCustomPrice = 20;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.SunplatePillarBlock>());
			shop.item[nextSlot].shopCustomPrice = 20;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Cloud);
			shop.item[nextSlot].shopCustomPrice = 90;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.RainCloud);
			shop.item[nextSlot].shopCustomPrice = 90;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SnowCloudBlock);
			shop.item[nextSlot].shopCustomPrice = 90;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.GiantHarpyFeather);
			nextSlot++;
			if (Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.IceFeather);
				nextSlot++;
				if (ModLoader.TryGetMod("QwertysRandomContent", out Mod qwertysBossAndItems) && townNPCsCrossModSupport) //Qwertys Boss And Items
				{
					shop.item[nextSlot].SetDefaults(qwertysBossAndItems.Find<ModItem>("FortressHarpyFeather").Type);
					nextSlot++;
				}
				if (NPC.downedMechBossAny)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FireFeather);
					nextSlot++;
				}
				if (NPC.downedPlantBoss)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BoneFeather);
					nextSlot++;
				}
				if (NPC.downedGolemBoss)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Materials.GiantRedHarpyFeather>());
					nextSlot++;
				}
			}
			if (Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.SoulofFlight);
				shop.item[nextSlot].shopCustomPrice = 15000;
				nextSlot++;
			}
			if (NPC.downedMechBossAny)
			{
				shop.item[nextSlot].SetDefaults(ItemID.HarpyWings);
				nextSlot++;
			}
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<GuideToProperFlightTechniques>());
				nextSlot++;
			}
			if (NPC.downedGolemBoss)
            {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Materials.SunEssence>());
				nextSlot++;
			}
			if (npcTypeListAll.Count >= 20)
			{
				shop.item[nextSlot].SetDefaults(ItemID.BirdieRattle);
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.Harpy.Harpy_Shirt>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.Harpy.Harpy_Shorts>());
			nextSlot++;
			if (ModLoader.TryGetMod("Split", out Mod split) && townNPCsCrossModSupport) //Split Mod
			{
				shop.item[nextSlot].SetDefaults(split.Find<ModItem>("PosterHarpy").Type);
				shop.item[nextSlot].shopCustomPrice = 10000;
				nextSlot++;
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return !toKingStatue;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			if (!Main.hardMode)
			{
			damage = 10;
			}
			if (Main.hardMode && !NPC.downedMoonlord)
			{
			damage = 20;
			}
			if (NPC.downedMoonlord)
			{
			damage = 30;
			}
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			if(NPC.downedMoonlord)
            {
				projType = ModContent.ProjectileType<FriendlyHarpyFeatherRed>();
			}
			else
            {
				projType = ModContent.ProjectileType<FriendlyHarpyFeather>();
			}
			attackDelay = 2;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 0.15f;
		}
	}
	public class HarpyProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;

		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}