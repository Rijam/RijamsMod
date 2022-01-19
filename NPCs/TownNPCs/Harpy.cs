using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using RijamsMod.Items;
using RijamsMod.Projectiles;

namespace RijamsMod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Harpy : ModNPC
	{
		public override string Texture
		{
			get
			{
				return "RijamsMod/NPCs/TownNPCs/Harpy";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "RijamsMod/NPCs/TownNPCs/Harpy_Alt_1" };
			}
		}

		public override bool Autoload(ref string name)
		{
			name = "Harpy";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			DisplayName.SetDefault("Harpy");
			Main.npcFrameCount[npc.type] = 25;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 0;
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
			npc.catchItem = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtHarpy>() : (short)-1;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Harpy_Gore_Head"), 1f);
				for (int k = 0; k < 1; k++)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Harpy_Gore_Arm"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Harpy_Gore_Leg"), 1f);
				}
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

		public override string TownNPCName()
		{
			string[] names = { "Pinnia", "Anisoa", "Remexa", "Lyre", "Ornitha", "Talia", "Mythia", "Aera", "Allegora", "Faybli", "Chimerica", "Whimsica", "Disparate", "Ourana", "Aetha", "Eria" };
			return Main.rand.Next(names);
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int harpy = NPC.FindFirstNPC(ModContent.NPCType<Harpy>());
			chat.Add("Don't attack, please!");
			chat.Add("Friends? I am friendly.");
			chat.Add("I think I am different...");
			chat.Add("Buy items from me?");
			chat.Add("Hi, I'm " + Main.npc[harpy].GivenName + ".");
			chat.Add("Flying is fun and all, but I sometimes wish I had hands.");
			chat.Add("Hm? Chicken Nuggets?", 0.5);
			
			if (npc.life < npc.lifeMax * 0.5)
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
			if (npc.homeless)
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
			/*if (Main.LocalPlayer.HasItem(ItemID.BirdieRattle))
			{
				chat.Add("Aww, isn't that little harpy so cute? Wait, what do you mean pet?");
			}*/
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
			Mod joostmod = ModLoader.GetMod("Joostmod");
			if (joostmod != null) //Joostmod
            {
				if (Main.player[Main.myPlayer].HasBuff(mod.BuffType("HarpyMinion")))
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
			int fisherman = NPC.FindFirstNPC(ModContent.NPCType<Fisherman>());
			if (interTravel >= 0)
			{
				chat.Add("Me and " + Main.npc[fisherman].GivenName + " go on fishing trips sometimes! He lets me 'scout ahead', whatever that means!", 0.5);
			}
			/*int zoologist = NPC.FindFirstNPC(NPCID.Zoologist);
			if (zoologist >= 0)
			{
				chat.Add("" + Main.npc[zoologist].GivenName + " is so nice to me! I like being around her.", 0.25);
			}*/
			int angler = NPC.FindFirstNPC(NPCID.Angler);
			if (angler >= 0)
			{
				chat.Add("" + Main.npc[angler].GivenName + " is so mean! He keeps calling me names like 'Chicken Legs' or 'Bird Brain'!", 0.25);
			}
			int pirate = NPC.FindFirstNPC(NPCID.Pirate);
			if (pirate >= 0)
			{
				chat.Add("" + Main.npc[pirate].GivenName + " lets me sit in the crows nest!", 0.25);
			}
			if (ModLoader.GetMod("SGAmod") != null) //SGAmod
			{	
				int draken = NPC.FindFirstNPC(ModLoader.GetMod("SGAmod").NPCType("Dergon"));
				if (draken >= 0)
				{
					chat.Add("I asked Draken if he wants to go flying, but he said he didn't want to. Why not?", 0.25);
				}
			}
			if (ModLoader.GetMod("SacredTools") != null) //Shadows of Abaddon
			{
				chat.Add("Jensen? Raynare? Sorry, I don't know what you are talking about.", 0.5);
			}
			if (ModLoader.GetMod("AAMod") != null) //Ancients Awakened
			{
				chat.Add("Athena? Sorry, I don't know what you are talking about.", 0.5);
			}
			if (ModLoader.GetMod("Varia") != null) //Varia
			{
				chat.Add("Fallen Angel? Sorry, I don't know what you are talking about.", 0.5);
			}
			if (ModLoader.GetMod("pinkymod") != null) //Pinky Mod
			{
				chat.Add("Valdaris? Sorry, I don't know what you are talking about.", 0.5);
			}
			return chat;
		}
		
		/*
			Future happiness notes:
				Loved Biome: Sky
				Liked Biome: Forest
				Disliked Biome: Underground, Cavern, Underworld
				Loved NPCs:
					Zoologist
					Princess
					Interstellar Traveler (this mod)
					Fisherman (this mod)
				
				Liked NPCs:
					Pirate
					Dryad
					Guide
					Stylist
					Steampunker
					Golfer
					Painter
					Witch Doctor
					Party Girl
					Mechanic
					Draken (SGAmod)
				
				Disliked NPCs:
				
				Hated NPCs:
					Angler
			
			
			Other NPCs' thoughts:
				Loved by:
					Zoologist
					Princess
					Interstellar Traveler (this mod)
					
				
				Liked by:
					Dryad
					Golfer
					Steampunker
					Pirate
					Fisherman (this mod)
				
				Disliked by:
					Arms Dealer
					Nurse
					Angler
				
				Hated by:
				
		*/

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = "Shop";
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
			
			shop.item[nextSlot].SetDefaults(ItemID.Feather);
			nextSlot++;
			Mod calamityMod = ModLoader.GetMod("CalamityMod");
			if (calamityMod != null)
			{
				shop.item[nextSlot].SetDefaults(ModLoader.GetMod("CalamityMod").ItemType("DesertFeather"));
				nextSlot++;
				if ((bool)calamityMod.Call("GetBossDowned", "dragonfolly"))
                {
					shop.item[nextSlot].SetDefaults(ModLoader.GetMod("CalamityMod").ItemType("EffulgentFeather"));
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			Mod shadowsOfAbaddon = ModLoader.GetMod("SacredTools");
			if (shadowsOfAbaddon != null) //Shadows of Abaddon
			{
				shop.item[nextSlot].SetDefaults(shadowsOfAbaddon.ItemType("BirdFeather")); //White Feather
				shop.item[nextSlot].shopCustomPrice = 50;
				nextSlot++;
				
				/*if (SacredToolsDownedHarpyPreHM) //Jensen
				{
					//shop.item[nextSlot].SetDefaults(ModLoader.GetMod("SacredTools").ItemType("GrandHarpyFeather"));
					shop.item[nextSlot].SetDefaults(ModLoader.GetMod("SacredTools").ItemType("HarpyDrop")); //Grand Harpy Feather
					shop.item[nextSlot].shopCustomPrice = 10000;
					nextSlot++;
				}
				if (SacredToolsDownedHarpyHM) //Raynare
				{
					//shop.item[nextSlot].SetDefaults(ModLoader.GetMod("SacredTools").ItemType("RoyalHarpyFeather"));
					shop.item[nextSlot].SetDefaults(ModLoader.GetMod("SacredTools").ItemType("GoldenFeather")); //Royal Harpy Feather
					shop.item[nextSlot].shopCustomPrice = 20000;
					nextSlot++;
				}*/
			}
			Mod ancientsAwakened = ModLoader.GetMod("AAMod");
			if (ancientsAwakened != null) //Ancients Awakened
			{
				shop.item[nextSlot].SetDefaults(ModLoader.GetMod("AAMod").ItemType("vulture_feather"));
				shop.item[nextSlot].shopCustomPrice = 5000;
				nextSlot++;
				if (NPC.downedPlantBoss)
				{
					shop.item[nextSlot].SetDefaults(ModLoader.GetMod("AAMod").ItemType("SeraphFeather"));
					shop.item[nextSlot].shopCustomPrice = 10000;
					nextSlot++;
				}
				/*if (AAModDownedAthena) //Athena
				{
					shop.item[nextSlot].SetDefaults(ModLoader.GetMod("AAMod").ItemType("GoddessFeather"));
					shop.item[nextSlot].shopCustomPrice = 150000;
					nextSlot++;
				}
				*/
			}
			/*Mod pinkyMod = ModLoader.GetMod("pinkymod");
			if (pinkyMod != null) //Pinky Mod
			{
				if (PinkymodDownedST) //Sunlight Trader
				{
					shop.item[nextSlot].SetDefaults(ModLoader.GetMod("PinkyMod").ItemType("SunsweptFeather"));
					shop.item[nextSlot].shopCustomPrice = 100000;
					nextSlot++;
				}
			}*/
			Mod orchidMod = ModLoader.GetMod("OrchidMod");
			if (orchidMod != null) //Orchid Mod
			{
				if (NPC.downedBoss1)
				{
					shop.item[nextSlot].SetDefaults(ModLoader.GetMod("OrchidMod").ItemType("HarpyTalon"));
					shop.item[nextSlot].shopCustomPrice = 2000;
					nextSlot++;
				}
			}
			Mod thorium = ModLoader.GetMod("ThoriumMod");
			if (thorium != null) //Thorium
			{
				shop.item[nextSlot].SetDefaults(thorium.ItemType("BirdTalon")); //Talon
				shop.item[nextSlot].shopCustomPrice = 100;
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.SkyMill);
			shop.item[nextSlot].shopCustomPrice = 17500;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.LuckyHorseshoe);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Starfury);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ShinyRedBalloon);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SunplateBlock);
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
				Mod qwertysBossAndItems = ModLoader.GetMod("QwertysRandomContent");
				if (qwertysBossAndItems != null) //Qwertys Boss And Items
				{
					shop.item[nextSlot].SetDefaults(ModLoader.GetMod("QwertysRandomContent").ItemType("FortressHarpyFeather"));
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
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Accessories.GuideToProperFlightTechniques>());
				nextSlot++;
			}
			if (NPC.downedGolemBoss)
            {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Materials.SunEssence>());
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.Harpy_Vanity_Shirt>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.Harpy_Vanity_Shorts>());
			nextSlot++;
			Mod split = ModLoader.GetMod("Split");
			if (split != null) //Thorium
			{
				shop.item[nextSlot].SetDefaults(split.ItemType("PosterHarpy"));
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
}