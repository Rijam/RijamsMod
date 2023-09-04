using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using RijamsMod.Items;
using RijamsMod.Items.Accessories.Movement;
using RijamsMod.Projectiles.Magic;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.IO;
using RijamsMod.Items.Placeable;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace RijamsMod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Harpy : ModNPC
	{
		private const string ShopName = "Shop";
		internal static int ShimmerHeadIndex;
		private static ITownNPCProfile NPCProfile;

		public override void Load()
		{
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, GetType().Namespace.Replace('.', '/') + "/Shimmered/" + Name + "_Head");
		}

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

			NPCProfile = new HarpyProfile();
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.homeless = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = NPCAIStyleID.Passive;
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

		public override void PostAI()
		{
			if (RijamsModWorld.harpyJustRescued > 0 && !NPC.homeless && !NPCHelper.IsFarFromHome(Main.npc[NPC.FindFirstNPC(Type)]))
			{
				RijamsModWorld.harpyJustRescued = 0;
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerTownNPCSend, new ParticleOrchestraSettings
				{
					PositionInWorld = NPC.Center,
					MovementVector = Vector2.Zero
				});
			}
			if (RijamsModWorld.harpyJustRescued > 0)
			{
				RijamsModWorld.harpyJustRescued--;
				if (RijamsModWorld.harpyJustRescued == 0)
				{
					ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerTownNPCSend, new ParticleOrchestraSettings
					{
						PositionInWorld = NPC.Center,
						MovementVector = Vector2.Zero
					});
				}
			}
			/*if (isShimmered && !NPC.IsShimmerVariant)
			{
				NPC.townNpcVariationIndex = 1;
			}
			if (NPC.IsShimmerVariant && !isShimmered)
			{
				isShimmered = true;
			}*/
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			if (RijamsModWorld.savedHarpy && NPC.CountNPCS(ModContent.NPCType<Harpy>()) < 1)
			{
				return true;
			}
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			if (RijamsModWorld.harpyJustRescued > 0)
			{
				RijamsModWorld.harpyJustRescued--;
			}
			if (RijamsModWorld.harpyJustRescued > 0 && NPC.homeless)
			{
				return false;
			}
			return true;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
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
				chat.Add("That's so cool! You made a banner just for me?", 5.0);
			}
			if ((Main.LocalPlayer.HasItem(ItemID.TallyCounter) || Main.LocalPlayer.HasItem(ItemID.REK) || Main.LocalPlayer.HasItem(ItemID.PDA) || Main.LocalPlayer.HasItem(ItemID.CellPhone)) && Main.player[Main.myPlayer].lastCreatureHit == Item.NPCtoBanner(NPCID.Harpy))
			//The player has the Tally Counter, R.E.K. 3000, PDA, or Cellphone in their inventory. The last enemy they hit was a Harpy and the kill count for Harpies is more than 0.
			//Item.NPCtoBanner(NPCID.Harpy) == 44
			{
				if (NPC.killCount[Item.NPCtoBanner(NPCID.Harpy)] > 0)
				{
					chat.Add("Why does that device that you have say \"Harpy: " + NPC.killCount[Item.NPCtoBanner(NPCID.Harpy)] + "\"?", 20.0);
				}
			}
			if (ModLoader.TryGetMod("Joostmod", out Mod joostMod) && townNPCsCrossModSupport) //Joostmod
			{
				if (Main.LocalPlayer.HasBuff(joostMod.Find<ModBuff>("HarpyMinion").Type))
				{
					chat.Add("Aww, isn't that little harpy so cute? Wait, what do you mean minion?");
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
				if (fishermanNPC.TryFind<ModNPC>("Fisherman", out ModNPC fishermanModNPC))
				{
					int fisherman = NPC.FindFirstNPC(fishermanModNPC.Type);
					if (fisherman >= 0)
					{
						chat.Add("Me and " + Main.npc[fisherman].GivenName + " go on fishing trips sometimes! He lets me 'scout ahead', whatever that means!", 0.5);
					}
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
			if (ModLoader.TryGetMod("LivingWorldMod", out Mod livingWorldMod) && townNPCsCrossModSupport) // Living World Mod
			{
				if (livingWorldMod.TryFind<ModNPC>("HarpyVillager", out ModNPC harpyVillagerModNPC))
				{
					int harpyVillagerType = harpyVillagerModNPC.Type;
					int harpyVillager = NPC.FindFirstNPC(harpyVillagerType);

					if (harpyVillager > 0)
					{
						chat.Add("Other harpies? I hope they welcome me. I would love to be with them!");
					}
				}
			}
			if (NPC.killCount[Item.NPCtoBanner(NPCID.Pinky)] > 0)
			{
				// Harpy Raiders reference.
				chat.Add("Slimes don't really like Harpies. I don't know why.", 0.1f);
			}
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28"); //Shop
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				shop = ShopName;
			}
		}

		public override void AddShops()
		{
			var npcShop = new NPCShop(Type, ShopName)
				.Add(new Item(ItemID.Feather) { shopCustomPrice = 10 * 5 * 2});
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && ShopConditions.TownNPCsCrossModSupport.IsMet())
			{
				npcShop.Add(NPCHelper.SafelyGetCrossModItemWithPrice(calamityMod, "CalamityMod/EffulgentFeather", 1f, 5f),
					new Condition("After defeating Dragonfolly", () => (bool)calamityMod.Call("GetBossDowned", "dragonfolly")));
			}
			if (ModLoader.TryGetMod("SacredTools", out Mod shadowsOfAbaddon) && ShopConditions.TownNPCsCrossModSupport.IsMet()) //Shadows of Abaddon
			{
				npcShop.Add(NPCHelper.SafelyGetCrossModItem(shadowsOfAbaddon, "SacredTools/BirdFeather")); //White Feather
			}
			if (ModLoader.TryGetMod("AAMod", out Mod ancientsAwakened) && ShopConditions.TownNPCsCrossModSupport.IsMet()) //Ancients Awakened
			{
				npcShop.Add(NPCHelper.SafelyGetCrossModItemWithPrice(ancientsAwakened, "AAMod/vulture_feather", 1f, 2f));
				npcShop.Add(NPCHelper.SafelyGetCrossModItemWithPrice(ancientsAwakened, "AAMod/SeraphFeather", 1f, 2f), Condition.DownedPlantera);
			}
			if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && ShopConditions.TownNPCsCrossModSupport.IsMet()) //Orchid Mod
			{
				npcShop.Add(NPCHelper.SafelyGetCrossModItemWithPrice(orchidMod, "OrchidMod/HarpyTalon", 1f, 2f), Condition.DownedEyeOfCthulhu);
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ShopConditions.TownNPCsCrossModSupport.IsMet()) //Thorium
			{
				npcShop.Add(NPCHelper.SafelyGetCrossModItemWithPrice(thorium, "ThoriumMod/BirdTalon", 1f, 50f)); //Talon
			}
			npcShop.Add(ItemID.SkyMill);
			npcShop.Add(new Item(ItemID.LuckyHorseshoe) { shopCustomPrice = 5400 * 5 * 2});
			npcShop.Add(ItemID.CelestialMagnet);
			npcShop.Add(new Item(ItemID.Starfury) { shopCustomPrice = 10000 * 5 * 2 });
			npcShop.Add(new Item(ItemID.ShinyRedBalloon) { shopCustomPrice = 15000 * 5 * 2 });
			npcShop.Add(new Item(ItemID.CreativeWings) { shopCustomPrice = 160000 }, Condition.DownedEarlygameBoss); //Fledgling Wings
			npcShop.Add(new Item(ItemID.SunplateBlock) { shopCustomPrice = 20 });
			npcShop.Add(new Item(ModContent.ItemType<SunplatePillarBlock>()) { shopCustomPrice = 20 });
			npcShop.Add(new Item(ItemID.Cloud) { shopCustomPrice = 90 });
			npcShop.Add(new Item(ItemID.RainCloud) { shopCustomPrice = 90 });
			npcShop.Add(new Item(ItemID.SnowCloudBlock) { shopCustomPrice = 90 });
			npcShop.Add(new Item(ItemID.GiantHarpyFeather) { shopCustomPrice = 25000 * 5 * 2 });
			npcShop.Add(new Item(ItemID.IceFeather) { shopCustomPrice = 25000 * 5 * 2 }, Condition.Hardmode);
			if (ModLoader.TryGetMod("QwertysRandomContent", out Mod qwertysBossAndItems) && ShopConditions.TownNPCsCrossModSupport.IsMet()) //Qwertys Boss And Items
			{
				npcShop.Add(NPCHelper.SafelyGetCrossModItem(qwertysBossAndItems, "QwertysRandomContent/FortressHarpyFeather"));
			}
			npcShop.Add(new Item(ItemID.FireFeather) { shopCustomPrice = 25000 * 5 * 2}, Condition.Hardmode, Condition.DownedMechBossAny);
			npcShop.Add(new Item(ItemID.BoneFeather) { shopCustomPrice = 25000 * 5 * 2 }, Condition.Hardmode, Condition.DownedPlantera);
			npcShop.Add(new Item(ModContent.ItemType<Items.Materials.GiantRedHarpyFeather>()) { shopCustomPrice = 25000 * 5 * 2 }, Condition.Hardmode, Condition.DownedGolem);
			npcShop.Add(new Item(ItemID.SoulofFlight) { shopCustomPrice = 200 * 5 * 2}, Condition.Hardmode, Condition.DownedMechBossAll);
			npcShop.Add(ItemID.HarpyWings, Condition.Hardmode, Condition.DownedPlantera);
			npcShop.Add(ModContent.ItemType<GuideToProperFlightTechniques>(), Condition.Hardmode, Condition.DownedMechBossAll);
			npcShop.Add(ModContent.ItemType<Items.Materials.SunEssence>(), Condition.Hardmode, Condition.DownedGolem);
			npcShop.Add(ItemID.BirdieRattle, new Condition(ShopConditions.CountTownNPCsS(20), ShopConditions.CountTownNPCsFb(20)));
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.Harpy.Harpy_Shirt>());
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.Harpy.Harpy_Shorts>());
			if (ModLoader.TryGetMod("Split", out Mod split) && ShopConditions.TownNPCsCrossModSupport.IsMet()) //Split Mod
			{
				npcShop.Add(new Item(NPCHelper.SafelyGetCrossModItem(split, "Split/PosterHarpy")) { shopCustomPrice = 10000 });
			}
			npcShop.Add(ModContent.ItemType<GroupHologramItem>(),
				new Condition(ShopConditions.IntTravQuestsS(3), ShopConditions.IntTravQuestsFb(3)),
				Condition.NpcIsPresent(ModContent.NPCType<Harpy>()),
				Condition.NpcIsPresent(ModContent.NPCType<InterstellarTraveler>()),
				Condition.NpcIsPresent(ModContent.NPCType<HellTrader>()),
				ShopConditions.HellTraderMovedIn);
			npcShop.Register();
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
			if (NPC.downedMoonlord)
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
		private string Namespace => GetType().Namespace.Replace('.', '/');
		private string NPCName => (GetType().Name.Split("Profile")[0]).Replace('.', '/');
		private string Path => (Namespace + "/" + NPCName);

		public int RollVariation() => 0;

		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (RijamsModWorld.harpyJustRescued > 0)
			{
				if (npc.IsShimmerVariant)
				{
					return ModContent.Request<Texture2D>(Namespace + "/Shimmered/" + NPCName + "_Alt");
				}
				return ModContent.Request<Texture2D>(Path + "_Alt");
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
			if (npc.IsShimmerVariant)
			{
				return Harpy.ShimmerHeadIndex;
			}
			return ModContent.GetModHeadSlot(Path + "_Head");
		}
	}
}