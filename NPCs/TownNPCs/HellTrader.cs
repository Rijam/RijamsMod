using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using RijamsMod.Items;
using RijamsMod.Projectiles;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.Bestiary;
using Terraria.UI.Gamepad;
using Terraria.ModLoader.IO;

namespace RijamsMod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class HellTrader : ModNPC
	{
		public override void BossHeadSlot(ref int index)
		{
			if (!RijamsModWorld.hellTraderArrivable)
			{
				index = -1;
			}
		}

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
			// DisplayName.SetDefault("Hell Trader");
			Main.npcFrameCount[NPC.type] = 23;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 7;
			NPCID.Sets.AttackFrameCount[NPC.type] = 2;
			NPCID.Sets.DangerDetectRange[NPC.type] = 1000;
			NPCID.Sets.AttackType[NPC.type] = 2;
			NPCID.Sets.AttackTime[NPC.type] = 40;
			NPCID.Sets.AttackAverageChance[NPC.type] = 10;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;

			NPCID.Sets.MagicAuraColor[Type] = Color.Yellow;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Love)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Love)
				//Love Weapon Master (cross mod)
				.SetNPCAffection(ModContent.NPCType<Harpy>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.DD2Bartender, AffectionLevel.Like)
				.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Like)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Clothier, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Truffle, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Like)
				//Like Fisherman (cross mod)
				//Like Goat (cross mod)
				.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Stylist, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Cyborg, AffectionLevel.Hate)
				//Princess is automatically set
			; // < Mind the semicolon!

			NPCProfile = new HellTraderProfile();
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 30;//def 15
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			NPC.lavaImmune = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>()] = true;
			NPC.homeless = true;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs ? ModContent.ItemType<CaughtHellTrader>() : -1;
			NPC.rarity = RijamsModWorld.hellTraderArrivable ? 0 : 1;
			NPC.npcSlots = RijamsModWorld.hellTraderArrivable ? 1 : 0.25f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
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
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head_Alt_1_Shimmered").Type, 1f);
					}
					else if (RijamsModWorld.hellTraderArrivable)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head_Alt_2_Shimmered").Type, 1f);
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
					if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head_Alt_1").Type, 1f);
					}
					else if (RijamsModWorld.hellTraderArrivable)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head_Alt_2").Type, 1f);
					}
					else
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head").Type, 1f);
					}
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
			if (Main.hardMode && RijamsModWorld.hellTraderArrivable && NPC.CountNPCS(ModContent.NPCType<HellTrader>()) < 1)
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
			return RijamsModWorld.hellTraderArrivable;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}
		public override List<string> SetNPCNameList()
		{
			return new List<string>()
			{ 
				"Mixi", "Brima", "Sulfura", "Leh", "Inferna", "Purgator", "Haidess", "Blaiz", "Agoni", "Flaima", "Nethi", "Perdition", "Do\'om", "Braz", "Grihmos", "Da\'nur"
			};
			
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (!RijamsModWorld.hellTraderArrivable && !NPC.AnyNPCs(NPC.type) && spawnInfo.Player.ZoneUnderworldHeight)
			{
				if (spawnInfo.SpawnTileType == TileID.ObsidianBrick || spawnInfo.SpawnTileType == TileID.HellstoneBrick || spawnInfo.SpawnTileType == TileID.Platforms || spawnInfo.SpawnTileType == TileID.AshGrass)
				{
					if (!NPC.downedBoss1) //Haven't killed EoC
					{
						return 0.05f;
					}
					if (!NPC.downedBoss3) //Have killed EoC but not Skeletron
					{
						return 0.075f;
					}
					return 0.1f; //Else
				}
			}
			return 0f;
		}
		public override bool UsesPartyHat()
		{
			return RijamsModWorld.hellTraderArrivable;
		}
		public override void AI()
		{
			if (!RijamsModWorld.hellTraderArrivable)
			{
				NPC.rarity = 1;
				float distance = Math.Abs(NPC.position.X - Main.player[NPC.FindClosestPlayer()].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.FindClosestPlayer()].position.Y);
				if (distance >= 4000f && NPC.homeless)
				{
					NPC.active = false;
					NPC.netSkip = -1;
					NPC.life = 0;
				}
			}
			if (RijamsModWorld.hellTraderArrivable)
			{
				NPC.rarity = 0;
			}
		}

		// Sitting frame height is corrected here.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("RijamsMod/NPCs/TownNPCs/HellTrader_Glow");
		private readonly Asset<Texture2D> shimmerGlowmask1 = ModContent.Request<Texture2D>("RijamsMod/NPCs/TownNPCs/Shimmered/HellTrader_Glow");
		private readonly Asset<Texture2D> shimmerGlowmask2 = ModContent.Request<Texture2D>("RijamsMod/NPCs/TownNPCs/Shimmered/HellTrader_Glow_Alt");
		private readonly Asset<Texture2D> shimmerWings = ModContent.Request<Texture2D>("RijamsMod/NPCs/TownNPCs/Shimmered/HellTrader_Wings");
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Asset<Texture2D> drawTexture = glowmask;
			SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color color = new(255, 255, 255, 255);
			Vector2 verticalOffset = new(0, 4 - NPC.gfxOffY);
			if (NPC.frame.Y == 18 * NPC.frame.Height) // Sitting, move up 4 pixels
			{
				verticalOffset.Y += 4;
			}

			if (NPC.IsShimmerVariant)
			{
				if (NPC.altTexture == 1)
				{
					drawTexture = shimmerGlowmask2;
				}
				else
				{
					drawTexture = shimmerGlowmask1;
				}
			}

			spriteBatch.Draw(drawTexture.Value, NPC.Center - screenPos - verticalOffset, NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 1f);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.IsShimmerVariant)
			{
				SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Vector2 verticalOffset = new(8, 4 - NPC.gfxOffY);
				if (NPC.frame.Y == 18 * NPC.frame.Height) // Sitting, move up 4 pixels
				{
					verticalOffset.Y += 4;
				}
				spriteBatch.Draw(shimmerWings.Value, NPC.Center - screenPos - verticalOffset, new Rectangle(NPC.frame.X, NPC.frame.Y, shimmerWings.Width(), NPC.frame.Height), drawColor, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 1f);
			}
			return true;
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new();

			bool townNPCsCrossModSupport = ModContent.GetInstance<RijamsModConfigServer>().TownNPCsCrossModSupport;

			int hellTrader = NPC.FindFirstNPC(ModContent.NPCType<HellTrader>());
			NPCHelper.GetNearbyResidentNPCs(Main.npc[hellTrader], 1, out List<int> _, out List<int> _, out List<int> npcTypeListVillage, out List<int> _);

			if (!RijamsModWorld.hellTraderArrivable)
			{
				Main.LocalPlayer.currentShoppingSettings.HappinessReport = ""; // Removes the Happiness chat option.

				chat.Add("Hello, human. An unexpected confrontation, for sure.");
				chat.Add("My robes are made from a special fabric. They are fire proof, just like me.");
				chat.Add("You shall know me by " + Main.npc[hellTrader].GivenName + ". I look forward to trading with you, " + Main.LocalPlayer.name + ".");
				chat.Add("I have some fine goods to trade with you if you have the money.");
				chat.Add("Are you here to trade? Good, I have something that might interest you.");
				chat.Add("The Underworld is a dangerous place. You are brave to venture down here.");
				chat.Add("The most dangerous demons are not the ones who fight, but who speak.");
				chat.Add("Who is this \"Slayer\" that you speak of?", 0.1);
				if (!Main.hardMode)
				{
					chat.Add("I see some of the greaters carrying around a small human figure. It must have some sort of significance...");
				}
			}

			if (RijamsModWorld.hellTraderArrivable)
			{
				chat.Add("Hey, human! Good to see you again.");
				chat.Add("I've got plenty of that special fabric if you want to purchase more of it!");
				chat.Add("I'm starting to get used to living with others. It's fun!");
				chat.Add("Ever notice how all of the other Imps wear chokers around their neck?");
				chat.Add("Interesting fact: Imps have blueish skin color as an adolescent. Yes, Imps have to grow up, too!");
				chat.Add("That Wall of Flesh was the biggest demon I'd ever seen! How does something like that even move forward?");
				chat.Add("Do you not like the smell of sulfur? It smells fine to me.");
				chat.Add("Who is this \"Slayer\" that you speak of?", 0.2);

				if (NPC.life < NPC.lifeMax * 0.2)
				{
					chat.Add("Wait! Help! I'm not just another expendable Imp am I?", 10.0);
				}

				if (NPC.homeless)
				{
					chat.Add("I accepted your request to move in expecting to receive and actual home! Can you deliver on your promise?");
				}
				else
				{
					chat.Add("Thanks again, " + Main.LocalPlayer.name + ", for asking me to live in your town. Much better than living alone in the Underworld!");
				}

				if ((Main.LocalPlayer.HasItem(ItemID.TallyCounter) || Main.LocalPlayer.HasItem(ItemID.REK) || Main.LocalPlayer.HasItem(ItemID.PDA) || Main.LocalPlayer.HasItem(ItemID.CellPhone)) && Main.LocalPlayer.lastCreatureHit == Item.NPCtoBanner(NPCID.FireImp))
				//The player has the Tally Counter, R.E.K. 3000, PDA, or Cellphone in their inventory. The last enemy they hit was a Fire Imp and the kill count for Fire Imps is more than 0.
				//Item.NPCtoBanner(NPCID.FireImp) == 24
				{
					if (NPC.killCount[Item.NPCtoBanner(NPCID.FireImp)] > 0)
					{
						chat.Add("You've slain " + NPC.killCount[Item.NPCtoBanner(NPCID.FireImp)] + " Fire Imps, hm? Interesting.", 20.0);
					}
				}

				int harpy = NPC.FindFirstNPC(ModContent.NPCType<Harpy>());
				int interTravel = NPC.FindFirstNPC(ModContent.NPCType<InterstellarTraveler>());
				if (harpy >= 0)
				{
					chat.Add("I've always wanted to fly like " + Main.npc[harpy].GivenName + " or like one of the greaters.", 0.5);
				}
				if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC))
				{
					int fisherman = NPC.FindFirstNPC(fishermanNPC.Find<ModNPC>("Fisherman").Type);
					if (fisherman >= 0)
					{
						chat.Add(Main.npc[fisherman].GivenName + " let me eat some other kinds of fish. I was getting tired of eating Flarefin Koi and Obsidifish all of the time!", 0.5);
					}
				}
				if (interTravel >= 0)
				{
					chat.Add("So, " + Main.npc[interTravel].GivenName + " is from a different planet? I have a lot to learn...", 0.5);
				}
				int taxCollector = NPC.FindFirstNPC(NPCID.TaxCollector);
				if (taxCollector >= 0 && npcTypeListVillage.Contains(NPCID.TaxCollector))
				{
					chat.Add("Uh yeah, I see why " + Main.npc[taxCollector].GivenName + " was banished to the Underworld.");
				}

				if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
				{
					chat.Add("I've never tasted food so sweet!");
					chat.Add("What? Surprised to see me without my hood on?");
				}
				Player player = Main.player[Main.myPlayer];
				if (player.ZoneSkyHeight || player.ZoneSnow)
				{
					chat.Add("Brr! I'm not used to it being so cold!");
				}
				if (!player.ZoneSkyHeight && !player.ZoneDirtLayerHeight && !player.ZoneRockLayerHeight && !player.ZoneUnderworldHeight)
				{
					chat.Add("It's so vibrant on the surface! I'm used to nothing but reds and grays.");
					if (Main.dayTime)
					{
						chat.Add(Main.raining ? "Water is falling from the sky? How does that even work?" : "I had read about the sun! It's a lot brighter than I expected! Brighter than magma!");
					}
					if (!Main.dayTime)
					{
						chat.Add("It gets a little chilly at night. Luckily, my robes keep me warm.");
					}
					if (Main.bloodMoon)
					{
						chat.Add("Woah. What's going on tonight? Why is the moon red?", 2.0);
					}
					if (Main.eclipse)
					{
						chat.Add("Wait, where did the sun go?", 2.0);
					}
				}
				if (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight)
				{
					chat.Add("The Underground is the perfect place for me.");
				}
				if (player.ZoneUnderworldHeight)
				{
					chat.Add("Y'know, the Underworld isn't that friendly of a place. I still like it from nostalgia, though.");
					chat.Add("I read that there was once a great civilization down here. They are the ones who built all of these buildings. I never did figure out what happened to them, though.");
				}
				if (player.ZoneDesert || player.ZoneJungle || player.ZoneUnderworldHeight)
				{
					chat.Add("I enjoy the heat from this environment.");
				}
				if (player.ZoneBeach)
				{
					chat.Add("The ocean is huge! I'd never seen so much water in one spot before!");
				}
				if (player.HasBuff(BuffID.ImpMinion))
				{
					chat.Add("I'm honestly surprised you were able to control those Imps. It's not easy getting hell spawn to do what you want, I should know!");
				}
				if (player.HasItem(ModContent.ItemType<Items.Weapons.Magic.PlasmaRifle>()))
				{
					chat.Add("I feel like I've seen that energy gun you have, but I'm not sure where.", 0.5);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && townNPCsCrossModSupport) //Thorium
				{
					if (thorium.TryFind<ModNPC>("WeaponMaster", out ModNPC weaponMasterModNPC))
					{
						int weaponMaster = NPC.FindFirstNPC(weaponMasterModNPC.Type);
						if (weaponMaster >= 0 && npcTypeListVillage.Contains(thorium.Find<ModNPC>("WeaponMaster").Type))
						{
							chat.Add("Hmm...\nHuh? No, I wasn't staring at " + Main.npc[weaponMaster].GivenName + "!");
							chat.Add("So... heard anything new about " + Main.npc[weaponMaster].GivenName + "? I'm just asking...");
							chat.Add(Main.npc[weaponMaster].GivenName + "'s helmet is very nice.\nNothing! It's just a casual observation.");
						}
					}
					if (thorium.TryFind<ModItem>("PLG8999", out ModItem plg))
					{
						if (player.HasItem(plg.Type))
						{
							chat.Add("That big gun you have scares me! I don't even know why!", 0.5);
						}
					}
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) && townNPCsCrossModSupport) //Calamity
				{
					if (calamity.TryFind<ModNPC>("WITCH", out ModNPC brimstoneWitchModNPC))
					{
						int brimstoneWitch = NPC.FindFirstNPC(brimstoneWitchModNPC.Type); //Brimstone Witch
						if (brimstoneWitch >= 0 && npcTypeListVillage.Contains(calamity.Find<ModNPC>("WITCH").Type))
						{
							chat.Add("Calamitas is so powerful! I wish my magic were as good as hers!");
						}
					}
					if (calamity.TryFind<ModNPC>("DILF", out ModNPC archmageModNPC))
					{
						int archmage = NPC.FindFirstNPC(archmageModNPC.Type); //Archmage
						if (archmage >= 0 && npcTypeListVillage.Contains(calamity.Find<ModNPC>("DILF").Type))
						{
							chat.Add("I'm more into fire magic, but Permafrost's ice magic is still super impressive!");
						}
					}
				}
			}
			
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28"); //Shop
			if (Main.hardMode && !RijamsModWorld.hellTraderArrivable)
			{
				button2 = "Ask to move in";
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
				MoveIn();
			}
		}

		public void MoveIn()
		{
			if (Main.hardMode && !RijamsModWorld.hellTraderArrivable)
			{
				Main.npcChatText = "You want me to move into one of the homes you built? Other humans live there right? What would they think of me?\nWell, to be honest, it is lonely living down here. Sure! Why the hell not! I'd love to see what else this world has to offer.";
				RijamsModWorld.hellTraderArrivable = true;
				if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.WorldData);
					ModPacket packet = Mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetHellTraderArrivable);
					packet.Send();
				}
				NPC.rarity = 0;
				NPC.netUpdate = true;
				Mod.Logger.Debug("RijamsMod: Hell Trader Arrivable.");
				return;
			}
		}

		public override void AddShops()
		{
			var npcShop = new NPCShop(Type, ShopName)
				.Add(new Item(ItemID.ObsidianRose) { shopCustomPrice = 50000 })
				.Add(ItemID.MagmaStone)
				.Add(new Item(ItemID.DemonScythe) { shopCustomPrice = 250000 }, Condition.DownedEyeOfCthulhu)
				.Add(ItemID.ShadowKey, Condition.DownedSkeletron)
				.Add(ItemID.Cascade, Condition.DownedSkeletron)
				.Add(ModContent.ItemType<Items.Weapons.Melee.TimonsAxe>(), Condition.DownedSkeletron)
				.Add(ItemID.HelFire, Condition.Hardmode)
				.Add(ItemID.Pwnhammer, Condition.Hardmode)
				.Add(ItemID.LavaCharm, Condition.Hardmode)
				.Add(ItemID.UnholyTrident, Condition.Hardmode, Condition.DownedMechBossAny)
				.Add(ModContent.ItemType<Items.Weapons.Melee.HammerOfRetribution>(), Condition.Hardmode, Condition.DownedMechBossAny)
				.Add(ModContent.ItemType<Items.Weapons.Melee.Quietus>(), Condition.Hardmode, Condition.DownedGolem)
				.Add(new Item(ItemID.AshBlock) { shopCustomPrice = 70 })
				.Add(new Item(ItemID.ObsidianBrick) { shopCustomPrice = 100 })
				.Add(new Item(ItemID.Hellstone) { shopCustomPrice = 150 }, Condition.Hardmode)
				.Add(new Item(ItemID.HellstoneBrick) { shopCustomPrice = 200 }, Condition.Hardmode)
				.Add(new Item(ItemID.LivingFireBlock) { shopCustomPrice = 50 }, Condition.Hardmode)
				.Add(new Item(ItemID.DemonTorch) { shopCustomPrice = 10 })
				.Add(new Item(ItemID.Fireblossom) { shopCustomPrice = 10000 })
				.Add(new Item(ItemID.Hellforge) { shopCustomPrice = 75000 }, Condition.DownedEowOrBoc);
			if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC))
			{
				if (fishermanNPC.TryFind<ModNPC>("Fisherman", out ModNPC fisherman))
				{
					npcShop.Add(new Item(ItemID.ObsidianLockbox) { shopCustomPrice = 50000 }, ShopConditions.HellTraderMovedIn, Condition.NpcIsPresent(fisherman.Type));
				}
			}
			npcShop.Add(ModContent.ItemType<Items.Accessories.Misc.LifeSapperRing>());
			npcShop.Add(ModContent.ItemType<Items.Accessories.Misc.ManaSapperRing>());
			npcShop.Add(ModContent.ItemType<Items.Materials.InfernicFabric>(), Condition.DownedEarlygameBoss);
			npcShop.Add(ModContent.ItemType<Items.Materials.Sulfur>(), Condition.Hardmode);
			npcShop.Add(new Item(ItemID.PlumbersHat) { shopCustomPrice = 400000 }, ShopConditions.HellTraderMovedIn);
			npcShop.Add(new Item(ItemID.KiteBoneSerpent) { shopCustomPrice = 50000 }, Condition.HappyWindyDay, ShopConditions.HellTraderMovedIn);
			npcShop.Add(ItemID.HellMinecart,
				ShopConditions.MoonPhase036, Condition.DownedSkeletron, ShopConditions.HellTraderMovedIn);
			npcShop.Add(ItemID.OrnateShadowKey,
				ShopConditions.MoonPhase147, Condition.DownedSkeletron, ShopConditions.HellTraderMovedIn);
			npcShop.Add(ItemID.OrnateShadowKey,
				ShopConditions.MoonPhase25, Condition.DownedSkeletron, ShopConditions.HellTraderMovedIn);
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.HellTrader.HellTrader_Hood>(), ShopConditions.HellTraderMovedIn);
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.HellTrader.HellTrader_Robes>(), ShopConditions.HellTraderMovedIn);
			npcShop.Add(ModContent.ItemType<Items.Armor.Vanity.HellTrader.HellTrader_Trousers>(), ShopConditions.HellTraderMovedIn);
			npcShop.Register();
		}

		/*
		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			shop.item[nextSlot].SetDefaults(ItemID.ObsidianRose);
			shop.item[nextSlot].shopCustomPrice = 50000;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MagmaStone); //10 gold
			nextSlot++;
			if (NPC.downedBoss1)
			{
				shop.item[nextSlot].SetDefaults(ItemID.DemonScythe);
				shop.item[nextSlot].shopCustomPrice = 250000;
				nextSlot++;
			}
			if (NPC.downedBoss3)
			{
				shop.item[nextSlot].SetDefaults(ItemID.ShadowKey);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Cascade);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.TimonsAxe>());
				nextSlot++;
			}
			if (Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.HelFire);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Pwnhammer);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LavaCharm);
				nextSlot++;
			}
			if (NPC.downedMechBossAny)
			{
				shop.item[nextSlot].SetDefaults(ItemID.UnholyTrident);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.HammerOfRetribution>());
				nextSlot++;
			}
			if (NPC.downedGolemBoss)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.Quietus>());
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.AshBlock);
			shop.item[nextSlot].shopCustomPrice = 70;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ObsidianBrick);
			shop.item[nextSlot].shopCustomPrice = 100;
			nextSlot++;
			if (Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.Hellstone);
				shop.item[nextSlot].shopCustomPrice = 150;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HellstoneBrick);
				shop.item[nextSlot].shopCustomPrice = 200;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LivingFireBlock);
				shop.item[nextSlot].shopCustomPrice = 50;
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.DemonTorch);
			shop.item[nextSlot].shopCustomPrice = 10;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Fireblossom);
			shop.item[nextSlot].shopCustomPrice = 10000;
			nextSlot++;
			if (NPC.downedBoss2)
			{
				shop.item[nextSlot].SetDefaults(ItemID.Hellforge);
				shop.item[nextSlot].shopCustomPrice = 7500;
				nextSlot++;
			}
			if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC) && RijamsModWorld.hellTraderArrivable)
			{
				int fisherman = NPC.FindFirstNPC(fishermanNPC.Find<ModNPC>("Fisherman").Type);
				if (fisherman >= 0)
				{
					shop.item[nextSlot].SetDefaults(ItemID.ObsidianLockbox);
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Accessories.Misc.LifeSapperRing>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Accessories.Misc.ManaSapperRing>());
			nextSlot++;
			if (NPC.downedBoss1)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Materials.InfernicFabric>());
				nextSlot++;
			}
			if (Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Materials.Sulfur>());
				nextSlot++;
			}
			if (RijamsModWorld.hellTraderArrivable)
			{
				shop.item[nextSlot].SetDefaults(ItemID.PlumbersHat);
				shop.item[nextSlot].shopCustomPrice = 400000;
				nextSlot++;
				if (Main.WindyEnoughForKiteDrops)
				{
					shop.item[nextSlot].SetDefaults(ItemID.KiteBoneSerpent);
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
				if (NPC.downedBoss3)
				{
					if (Main.moonPhase == 0 || Main.moonPhase == 3 || Main.moonPhase == 6)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HellMinecart);
						nextSlot++;
					}
					if (Main.moonPhase == 1 || Main.moonPhase == 4 || Main.moonPhase == 7)
					{
						shop.item[nextSlot].SetDefaults(ItemID.OrnateShadowKey);
						nextSlot++;
					}
					if (Main.moonPhase == 2 || Main.moonPhase == 5)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HellCake);
						nextSlot++;
					}
				}
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.HellTrader.HellTrader_Hood>());
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.HellTrader.HellTrader_Robes>());
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.HellTrader.HellTrader_Trousers>());
				nextSlot++;
			}
		}
		*/

		public override bool CanGoToStatue(bool toKingStatue)
		{
			if (RijamsModWorld.hellTraderArrivable)
			{
				return !toKingStatue;
			}
			return false;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			if (!Main.hardMode)
			{
				damage = 50;
			}
			if (Main.hardMode && !NPC.downedMoonlord)
			{
				damage = 80;
			}
			if (NPC.downedMoonlord)
			{
				damage = 110;
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
			SoundEngine.PlaySound(SoundID.Item8, NPC.position);
			projType = ModContent.ProjectileType<Projectiles.Magic.SulfurSphere>();
			attackDelay = 2;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 6f;
			randomOffset = 0.15f;
		}
	}
	public class HellTraderProfile : ITownNPCProfile
	{
		private string Namespace => GetType().Namespace.Replace('.', '/');
		private string NPCName => (GetType().Name.Split("Profile")[0]).Replace('.', '/');
		private string Path => (Namespace + "/" + NPCName);

		public int RollVariation() => 0;

		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
			{
				if (RijamsModWorld.hellTraderArrivable)
				{
					return ModContent.Request<Texture2D>(Path + "Town");
				}
				return ModContent.Request<Texture2D>(Path);
			}

			if (npc.altTexture == 1)
			{
				if (npc.IsShimmerVariant)
				{
					return ModContent.Request<Texture2D>(Namespace + "/Shimmered/" + NPCName + "Town_Alt");
				}
				return ModContent.Request<Texture2D>(Path + "Town_Alt");
			}

			if (RijamsModWorld.hellTraderArrivable && !(npc.homeless || NPCHelper.IsFarFromHome(npc)))
			{
				if (npc.IsShimmerVariant)
				{
					return ModContent.Request<Texture2D>(Namespace + "/Shimmered/" + NPCName + "Town");
				}
				return ModContent.Request<Texture2D>(Path + "Town");
			}

			if (npc.IsShimmerVariant)
			{
				return ModContent.Request<Texture2D>(Namespace + "/Shimmered/" + NPCName);
			}

			return ModContent.Request<Texture2D>(Path);
		}

		public int GetHeadTextureIndex(NPC npc)
		{
			if (npc.IsShimmerVariant)
			{
				return HellTrader.ShimmerHeadIndex;
			}
			return ModContent.GetModHeadSlot(Path + "_Head");
		}
	}
}