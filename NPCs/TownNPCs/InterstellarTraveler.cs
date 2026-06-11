using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RijamsMod.EmoteBubbles;
using RijamsMod.Items.Accessories.Misc;
using RijamsMod.Items.Accessories.Ranger;
using RijamsMod.Items.Accessories.Summoner;
using RijamsMod.Items.Information;
using RijamsMod.Items.Quest;
using RijamsMod.Items.Tools;
using RijamsMod.Items.Weapons.Magic;
using RijamsMod.Items.Weapons.Ranged;

namespace RijamsMod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class InterstellarTraveler : ModNPC
	{
		private bool usedMicronWrap = false;
		private short usedMicronWrapTime = 0;
		internal ChatWithPortrait chatEmotion;
		internal List<ChatWithPortrait.DialogWithEmotion> questChatEmotion = new();

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

		public override void Unload()
		{
			chatEmotion = null;
			questChatEmotion = null;
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 26;
			NPCID.Sets.ExtraFramesCount[Type] = 10;
			NPCID.Sets.AttackFrameCount[Type] = 5;
			NPCID.Sets.DangerDetectRange[Type] = 1000;
			NPCID.Sets.AttackType[Type] = 1;
			NPCID.Sets.AttackTime[Type] = 30; 
			NPCID.Sets.AttackAverageChance[Type] = 1; // Lower numbers actually make the NPC more likely to attack
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
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

			NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<InterstellarTravelerEmote>();

			// Here we define which portrait to use for the Town NPC when the portrait style setting is set to detailed.
			NPCID.Sets.NPCPortraits.Add(Type, NPCID.Sets.PrioritizedPortrait()
				.With(() => ModContent.GetInstance<RijamsModConfigClient>().Ornithophobia, NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Helmet", ""))) // This is the portrait to use while the Town NPC is shimmered.

				// .With(() => NPCHelper.PartyPortraitCondition() && NPCHelper.AllQuestsCompleted(), NPCID.Sets.BasicPortrait($"{Texture}_Casual_Portrait")) // This is the portrait to use while the Town NPC is shimmered.
				// .With(() => NPCID.Sets.ShimmeredPortraitCondition() && NPCHelper.PartyPortraitCondition(), NPCID.Sets.BasicPortrait($"{Texture}_Shimmer_Alt_Portrait")) // This is the portrait to use while the Town NPC is shimmered.
				// .With(NPCID.Sets.ShimmeredPortraitCondition, NPCID.Sets.BasicPortrait($"{Texture}_Shimmer_Portrait")) // This is the portrait to use while the Town NPC is shimmered.

				// Happiness button
				.With(() => CasualShowingHappinessText(0f, 0.82f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "VeryHappy")))
				.With(() => ChatWithPortrait.ShimmerPartyShowingHappinessText(0f, 0.82f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "VeryHappy")))
				.With(() => ChatWithPortrait.ShimmerShowingHappinessText(0f, 0.82f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "VeryHappy")))
				.With(() => ChatWithPortrait.ShowingHappinessText(0f, 0.82f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "VeryHappy")))
				.With(() => CasualShowingHappinessText(0.82f, 1f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Happy")))
				.With(() => ChatWithPortrait.ShimmerPartyShowingHappinessText(0.82f, 1f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Happy")))
				.With(() => ChatWithPortrait.ShimmerShowingHappinessText(0.82f, 1f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Happy")))
				.With(() => ChatWithPortrait.ShowingHappinessText(0.82f, 1f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Happy")))
				.With(() => CasualShowingHappinessText(1f, 1.1f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Neutral")))
				.With(() => ChatWithPortrait.ShimmerPartyShowingHappinessText(1f, 1.1f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Neutral")))
				.With(() => ChatWithPortrait.ShimmerShowingHappinessText(1f, 1.1f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Neutral")))
				.With(() => ChatWithPortrait.ShowingHappinessText(1f, 1.1f), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Neutral")))
				.With(() => CasualShowingHappinessText(1.1f, float.MaxValue), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Sad")))
				.With(() => ChatWithPortrait.ShimmerPartyShowingHappinessText(1.1f, float.MaxValue), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Sad")))
				.With(() => ChatWithPortrait.ShimmerShowingHappinessText(1.1f, float.MaxValue), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Sad")))
				.With(() => ChatWithPortrait.ShowingHappinessText(1.1f, float.MaxValue), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Sad")))
				// Housing button
				.With(CasualShowingHousingText, NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Sad")))
				.With(ChatWithPortrait.ShimmerPartyShowingHousingText, NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Sad")))
				.With(ChatWithPortrait.ShimmerShowingHousingText, NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Sad")))
				.With(ChatWithPortrait.ShowingHousingText, NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Sad")))

				// Normal Chat
				// Casual outfit during a party after completing all quests
				.With(() => CasualPortraitEmotionCondtion(PortraitEmotion.Angry), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Angry")))
				.With(() => CasualPortraitEmotionCondtion(PortraitEmotion.Blushing), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Blushing")))
				.With(() => CasualPortraitEmotionCondtion(PortraitEmotion.Happy), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Happy")))
				.With(() => CasualPortraitEmotionCondtion(PortraitEmotion.Sad), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Sad")))
				.With(() => CasualPortraitEmotionCondtion(PortraitEmotion.Shocked), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Shocked")))
				.With(() => CasualPortraitEmotionCondtion(PortraitEmotion.Smirk), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Smirk")))
				.With(() => CasualPortraitEmotionCondtion(PortraitEmotion.Thinking), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Thinking")))
				.With(() => CasualPortraitEmotionCondtion(PortraitEmotion.VeryHappy), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "VeryHappy")))
				.With(() => CasualPortraitEmotionCondtion(PortraitEmotion.Worried), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Worried")))
				.With(() => NPCHelper.PartyPortraitCondition() && NPCHelper.AllQuestsCompleted(), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Casual", "Neutral")))
				// Shimmered Party
				.With(() => ChatWithPortrait.ShimmerPartyPortraitEmotionCondtion(PortraitEmotion.Angry), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Angry")))
				.With(() => ChatWithPortrait.ShimmerPartyPortraitEmotionCondtion(PortraitEmotion.Blushing), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Blushing")))
				.With(() => ChatWithPortrait.ShimmerPartyPortraitEmotionCondtion(PortraitEmotion.Happy), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Happy")))
				.With(() => ChatWithPortrait.ShimmerPartyPortraitEmotionCondtion(PortraitEmotion.Sad), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Sad")))
				.With(() => ChatWithPortrait.ShimmerPartyPortraitEmotionCondtion(PortraitEmotion.Shocked), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Shocked")))
				.With(() => ChatWithPortrait.ShimmerPartyPortraitEmotionCondtion(PortraitEmotion.Smirk), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Smirk")))
				.With(() => ChatWithPortrait.ShimmerPartyPortraitEmotionCondtion(PortraitEmotion.Thinking), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Thinking")))
				.With(() => ChatWithPortrait.ShimmerPartyPortraitEmotionCondtion(PortraitEmotion.VeryHappy), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "VeryHappy")))
				.With(() => ChatWithPortrait.ShimmerPartyPortraitEmotionCondtion(PortraitEmotion.Worried), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Worried")))
				.With(() => NPCID.Sets.ShimmeredPortraitCondition() && NPCHelper.PartyPortraitCondition(), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer_Hatless", "Neutral")))
				// Shimmer
				.With(() => ChatWithPortrait.ShimmerPortraitEmotionCondtion(PortraitEmotion.Angry), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Angry")))
				.With(() => ChatWithPortrait.ShimmerPortraitEmotionCondtion(PortraitEmotion.Blushing), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Blushing")))
				.With(() => ChatWithPortrait.ShimmerPortraitEmotionCondtion(PortraitEmotion.Happy), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Happy")))
				.With(() => ChatWithPortrait.ShimmerPortraitEmotionCondtion(PortraitEmotion.Sad), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Sad")))
				.With(() => ChatWithPortrait.ShimmerPortraitEmotionCondtion(PortraitEmotion.Shocked), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Shocked")))
				.With(() => ChatWithPortrait.ShimmerPortraitEmotionCondtion(PortraitEmotion.Smirk), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Smirk")))
				.With(() => ChatWithPortrait.ShimmerPortraitEmotionCondtion(PortraitEmotion.Thinking), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Thinking")))
				.With(() => ChatWithPortrait.ShimmerPortraitEmotionCondtion(PortraitEmotion.VeryHappy), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "VeryHappy")))
				.With(() => ChatWithPortrait.ShimmerPortraitEmotionCondtion(PortraitEmotion.Worried), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Worried")))
				.With(NPCID.Sets.ShimmeredPortraitCondition, NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Shimmer", "Neutral")))
				// Default
				.With(() => ChatWithPortrait.PortraitEmotionCondtion(PortraitEmotion.Angry), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Angry")))
				.With(() => ChatWithPortrait.PortraitEmotionCondtion(PortraitEmotion.Blushing), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Blushing")))
				.With(() => ChatWithPortrait.PortraitEmotionCondtion(PortraitEmotion.Happy), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Happy")))
				.With(() => ChatWithPortrait.PortraitEmotionCondtion(PortraitEmotion.Sad), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Sad")))
				.With(() => ChatWithPortrait.PortraitEmotionCondtion(PortraitEmotion.Shocked), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Shocked")))
				.With(() => ChatWithPortrait.PortraitEmotionCondtion(PortraitEmotion.Smirk), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Smirk")))
				.With(() => ChatWithPortrait.PortraitEmotionCondtion(PortraitEmotion.Thinking), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Thinking")))
				.With(() => ChatWithPortrait.PortraitEmotionCondtion(PortraitEmotion.VeryHappy), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "VeryHappy")))
				.With(() => ChatWithPortrait.PortraitEmotionCondtion(PortraitEmotion.Worried), NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Worried")))
				.Default(NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath(this, "Default", "Neutral")))); // Default portrait to use (not shimmered).
			NPCID.Sets.NPCPortraitsCloseUpOffsets.Add(Type, new Vector2(-2f, 0f)); // Here we can change the offsets of Town NPC when the portrait style setting is set to profile.
			//NPCID.Sets.NPCPortraitsFullBodyRetroOffsets.Add(Type, new Vector2(0f, 0f)); // Here we can change the offsets of Town NPC when the portrait style setting is set to retro.
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = NPCAIStyleID.Passive;
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
			bestiaryEntry.Info.AddRange(
			[
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				// new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			]);
			if (NPCHelper.ShouldAddHappinessInfoBox())
			{
				bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name)));
			}
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
					foreach (Player player in Main.ActivePlayers)
					{
						if (player.HasItem(ModContent.ItemType<OddDevice>())) //check if the player has the Odd Device in their inventory
						{
							return true;
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
				RijamsModWorld.NetMessageSendWorldData();
			}

			return
			[
				"Tlani", "Cuia", "Cuemal", "Teztlal", "Nezal", "Zelelli", "Matlin", "Xoco", "Zillin", "Centia", "Citzil", "Malxoc", "Izta", "Xical", "Mazalch", "Tlazoh", "Checa", "Acnopan", "Uetlac", "Illi", "Zina"
			];
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
				int healAmount = Math.Clamp(150, 0, NPC.lifeMax);
				NPC.life += healAmount;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.HealEffect(healAmount);
				}
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

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(usedMicronWrap);
			writer.Write(usedMicronWrapTime);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			usedMicronWrap = reader.ReadBoolean();
			usedMicronWrapTime = reader.ReadInt16();
		}
		#endregion

		#region PostDraw
		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> textureArm = ModContent.Request<Texture2D>("RijamsMod/NPCs/TownNPCs/InterstellarTraveler_Arm");
		private readonly Asset<Texture2D> textureCasualArm = ModContent.Request<Texture2D>("RijamsMod/NPCs/TownNPCs/InterstellarTraveler_Casual_Arm");
		private readonly Asset<Texture2D> textureShimmeredArm = ModContent.Request<Texture2D>("RijamsMod/NPCs/TownNPCs/Shimmered/InterstellarTraveler_Arm");
		private readonly Asset<Texture2D> questIcons = ModContent.Request<Texture2D>("RijamsMod/Items/Quest/QuestIcons");
		private readonly Asset<Texture2D> questQuestion = ModContent.Request<Texture2D>("RijamsMod/Items/Quest/Question");

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Asset<Texture2D> drawTexture = textureArm;
			if (NPC.IsShimmerVariant)
			{
				drawTexture = textureShimmeredArm;
			}
			if (NPC.altTexture == 1 && NPCHelper.AllQuestsCompleted())
			{
				drawTexture = textureCasualArm;
			}

			Color color = NPC.GetAlpha(drawColor);

			if (NPC.frame.Y > 20 * NPC.frame.Height) //Only draw while attacking
			{
				spriteBatch.Draw(drawTexture.Value, NPC.Center - screenPos + NPCHelper.DrawingOffsets(NPC), NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 1f);
			}

			// Inspired by Magic Storage
			if (!Main.hideUI && CheckIfQuestIsAvailableToTurnIn(out int which, out bool secret))
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
			bool townNPCsCrossModSupport = ModContent.GetInstance<RijamsModConfigServer>().TownNPCsCrossModSupport;

			NPCHelper.GetNearbyResidentNPCs(Main.npc[NPC.whoAmI], 1, out List<int> npcTypeListHouse, out List<int> npcTypeListNearBy, out List<int> npcTypeListVillage, out List<int> _);

			chatEmotion = new();

			chatEmotion.Add("I'm pretty far from home, but this place is pretty cool.", emotion: PortraitEmotion.Happy);
			chatEmotion.Add("Nice to meet you!", emotion: PortraitEmotion.Happy);
			chatEmotion.Add("I'm pretty lucky to have ended up on this planet. Not only is it inhabitable, but it also contains intelligent life!", emotion: PortraitEmotion.Happy);
			chatEmotion.Add("I have a few things that I can sell you if you want to take a look.");
			chatEmotion.Add($"Hi there! My name is {Main.npc[NPC.whoAmI].GivenName}.", emotion: PortraitEmotion.VeryHappy);
			chatEmotion.Add("Hey, do you know where I could get some food?", 0.5, emotion: PortraitEmotion.Thinking);

			if (usedMicronWrap)
			{
				chatEmotion.Add("That was a close call. I had to use a micronwrap to heal myself!", 10.0);
			}
			else if (NPC.life < NPC.lifeMax * 0.5)
			{
				chatEmotion.Add("Ouch! I better apply some micronwraps...", 10.0, emotion: PortraitEmotion.Shocked);
			}
			if (Main.dayTime)
			{
				if (Main.raining)
				{
					chatEmotion.Add("The rain is pleasant to watch; maybe not to stand in, though.");
				}
				else
				{
					chatEmotion.Add("Nice day today, isn't it?", emotion: PortraitEmotion.Happy);
				}
				chatEmotion.Add("This planet is very interesting.", emotion: PortraitEmotion.Happy);
			}
			else
			{
				if (Main.bloodMoon)
				{
					chatEmotion.Add("You better take shelter. There are some very strange creatures tonight.");
				}
				else
				{
					chatEmotion.Add("You better take shelter. I've seen strange creatures at night.");
				}
				chatEmotion.Add("Well, time to relax inside.", emotion: PortraitEmotion.Happy);
				chatEmotion.Add("I hope you have a weapon to defend yourself with.");
			}
			if (NPC.homeless)
			{
				chatEmotion.Add("It's dangerous out here. Do you have a place where I could stay?", emotion: PortraitEmotion.Worried);
			}
			else
			{
				chatEmotion.Add("Thanks for letting me stay here.", emotion: PortraitEmotion.Happy);
			}
			if (Main.LocalPlayer.wingTimeMax > 0)
			{
				chatEmotion.Add("Woah! You have wings? And they work - like function? How did you get those? Are they heavy? How exhausting are they to use? Could I get a pair for myself? ...", emotion: PortraitEmotion.Shocked);
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chatEmotion.Add("Don't mind me, I'm just waiting for my next slice of cake.", 2.0, emotion: PortraitEmotion.VeryHappy);
				chatEmotion.Add("What's the occasion? Ah, it doesn't matter. Parties are fun!", 2.0, emotion: PortraitEmotion.VeryHappy);
			}

			if (NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) > 1) //more than one Interstellar Traveler
			{
				chatEmotion.Add("What? There two of me!? I have a lot of questions now. Is this your doing? Do you have some sort of divine powers that I wasn't aware of?", 5.0, emotion: PortraitEmotion.Angry);
			}
			if (!NPC.downedBoss2 || !RijamsModWorld.intTravArrived) //spawn in the Interstellar Traveler before meeting the requirements
			{
				chatEmotion.Add("I'm not supposed to be here, yet. Is this your doing? Do you have some sort of divine powers that I wasn't aware of?", 2.0, emotion: PortraitEmotion.Angry);
			}

			int harpy = NPC.FindFirstNPC(ModContent.NPCType<Harpy>());
			int hellTrader = NPC.FindFirstNPC(ModContent.NPCType<HellTrader>());
			int angler = NPC.FindFirstNPC(NPCID.Angler);
			if (harpy >= 0)
			{
				chatEmotion.Add($"{Main.npc[harpy].GivenName} and I have surprisingly similar biology. Yet, we are different in many ways.", npcTypeListVillage.Contains(ModContent.NPCType<Harpy>()) ? 1 : 0.5, emotion: PortraitEmotion.Thinking);
				chatEmotion.Add($"{Main.npc[harpy].GivenName}'s wings intrigue me. How much lift can she generate with them? How much energy does it take to continuously flap her wings? ...", npcTypeListVillage.Contains(ModContent.NPCType<Harpy>()) ? 1 : 0.5, emotion: PortraitEmotion.Thinking);
				chatEmotion.Add($"I really enjoy {Main.npc[harpy].GivenName}'s presence. Some of the things she says makes me laugh!", npcTypeListVillage.Contains(ModContent.NPCType<Harpy>()) ? 1 : 0.5, emotion: PortraitEmotion.VeryHappy);
			}
			if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC) && townNPCsCrossModSupport)
			{
				int fisherman = NPC.FindFirstNPC(fishermanNPC.Find<ModNPC>("Fisherman").Type);
				if (fisherman >= 0)
				{
					chatEmotion.Add($"Do you think you could convince {Main.npc[fisherman].GivenName} to give me some fish?", npcTypeListVillage.Contains(fishermanNPC.Find<ModNPC>("Fisherman").Type) ? 1 : 0.5, emotion: PortraitEmotion.Thinking);
				}
				if (hellTrader >= 0 && RijamsModWorld.hellTraderArrivable && fisherman >= 0)
				{
					chatEmotion.Add($"So, {Main.npc[fisherman].GivenName} gives {Main.npc[hellTrader].GivenName} a bunch of fish, but not me?", npcTypeListVillage.Contains(fishermanNPC.Find<ModNPC>("Fisherman").Type) ? 1 : 0.5, emotion: PortraitEmotion.Angry);
				}
				/*if (angler >= 0 && fisherman >= 0)
				{
					chatEmotion.Add("Looking for these: [i:3120] , [i:3037] , [i:3096] ? Sorry, you're going to have to get them from " + Main.npc[angler].GivenName + " or " + Main.npc[fisherman].GivenName + ".", 0.75);
				}*/
			}
			int guide = NPC.FindFirstNPC(NPCID.Guide);
			if (guide >= 0 && npcTypeListVillage.Contains(NPCID.Guide))
			{
				chatEmotion.Add($"{Main.npc[guide].GivenName} seems to know a lot. Perhaps I could learn more about this planet from him.", emotion: PortraitEmotion.Thinking);
				if (Main.npc[guide].GivenName == "Andrew")
				{
					chatEmotion.Add($"Why would {Main.npc[guide].GivenName} carry that hat around if he never wears it?", 0.5);
				}
			}
			int merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (merchant >= 0 && npcTypeListVillage.Contains(NPCID.Merchant))
			{
				chatEmotion.Add($"{Main.npc[merchant].GivenName} refuses to sell me anything. That's fine by me, I don't need the primitive items he has on offer...", emotion: PortraitEmotion.Angry);
			}
			int nurse = NPC.FindFirstNPC(NPCID.Nurse);
			if (nurse >= 0 && npcTypeListHouse.Contains(NPCID.Nurse))
			{
				chatEmotion.Add($"{Main.npc[nurse].GivenName} is very experienced in her field. It's like she operates on somebody everyday!", emotion: PortraitEmotion.Smirk);
			}
			int demolitionist = NPC.FindFirstNPC(NPCID.Demolitionist);
			if (demolitionist >= 0 && npcTypeListVillage.Contains(NPCID.Demolitionist))
			{
				chatEmotion.Add($"{Main.npc[demolitionist].GivenName} just lobs those grenades around without a care in the universe! He could seriously hurt somebody!", emotion: PortraitEmotion.Angry);
			}
			int dyeTrader = NPC.FindFirstNPC(NPCID.DyeTrader);
			if (dyeTrader >= 0 && npcTypeListHouse.Contains(NPCID.DyeTrader))
			{
				chatEmotion.Add($"{Main.npc[dyeTrader].GivenName} has some really strange dyes. How does he make them?", emotion: PortraitEmotion.Thinking);
			}
			if (angler >= 0 && npcTypeListNearBy.Contains(NPCID.Angler))
			{
				chatEmotion.Add($"{Main.npc[angler].GivenName} keeps calling me names like 'Chicken Legs' or 'Bird Brain'. I hope he realizes I don't take offense to those phrases.");
				chatEmotion.Add($"Do you know who {Main.npc[angler].GivenName}'s parents are? Where are they?");
			}
			int zoologist = NPC.FindFirstNPC(NPCID.BestiaryGirl);
			if (zoologist >= 0 && npcTypeListVillage.Contains(NPCID.BestiaryGirl))
			{
				if (NPC.ShouldBestiaryGirlBeLycantrope()) //Blood Moon or Full Moon
				{
					chatEmotion.Add($"WOAH! Have you seen {Main.npc[zoologist].GivenName}? Is she aware of this?", emotion: PortraitEmotion.Shocked);
				}
				else
				{
					chatEmotion.Add($"{Main.npc[zoologist].GivenName} really likes me for some reason. Do you know why?", emotion: PortraitEmotion.Thinking);
				}
			}
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0 && npcTypeListVillage.Contains(NPCID.Dryad))
			{
				chatEmotion.Add($"How are you? I'm doing good myself. {Main.npc[dryad].GivenName} is very nice to me.", emotion: PortraitEmotion.Happy);
				chatEmotion.Add($"{Main.npc[dryad].GivenName} has some sort of connection with nature — fascinating!", emotion: PortraitEmotion.Thinking);
			}
			int painter = NPC.FindFirstNPC(NPCID.Painter);
			if (painter >= 0 && npcTypeListVillage.Contains(NPCID.Painter))
			{
				chatEmotion.Add($"I recognize {Main.npc[painter].GivenName}'s talent. That is all I have to say.");
				if (Main.npc[painter].GivenName == "Victor") //impossible in vanilla because Victor is not a name for the Painter
				{
					chatEmotion.Add($"Sorry, not now. {Main.npc[painter].GivenName} and I are in an argument about whether something is a mouth or nose...", 0.5, emotion: PortraitEmotion.Angry);
				}
			}
			int golfer = NPC.FindFirstNPC(NPCID.Golfer);
			if (golfer >= 0 && npcTypeListHouse.Contains(NPCID.Golfer))
			{
				chatEmotion.Add($"{Main.npc[golfer].GivenName} challenged me to get a par score on all eighteen holes. Now the question is: do I play fair?", emotion: PortraitEmotion.Thinking);
			}
			int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (armsDealer >= 0 && npcTypeListHouse.Contains(NPCID.ArmsDealer))
			{
				chatEmotion.Add($"{Main.npc[armsDealer].GivenName} took me to the range to show off his gun collection. He was very surprised to see how great of a shot I am.", emotion: PortraitEmotion.VeryHappy);
			}
			int tavernkeep = NPC.FindFirstNPC(NPCID.DD2Bartender);
			if (tavernkeep >= 0 && npcTypeListNearBy.Contains(NPCID.DD2Bartender))
			{
				chatEmotion.Add($"So, {Main.npc[tavernkeep].GivenName} is from another land you say? Portals? Hm... I'm going to have to make a note of that...", emotion: PortraitEmotion.Thinking);
			}
			int stylist = NPC.FindFirstNPC(NPCID.Stylist);
			if (stylist >= 0 && npcTypeListHouse.Contains(NPCID.Stylist))
			{
				chatEmotion.Add($"I don't have any hair for {Main.npc[stylist].GivenName} to style, but she is always welcome to preen the feathers on my head.", emotion: PortraitEmotion.Happy);
			}
			int goblinTinkerer = NPC.FindFirstNPC(NPCID.GoblinTinkerer);
			if (goblinTinkerer >= 0 && npcTypeListHouse.Contains(NPCID.GoblinTinkerer))
			{
				chatEmotion.Add($"{Main.npc[goblinTinkerer].GivenName} has some crazy weird gadgets. I might have to try some out myself.", emotion: PortraitEmotion.Happy);
			}
			int witchDoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
			if (witchDoctor >= 0 && npcTypeListNearBy.Contains(NPCID.WitchDoctor))
			{
				chatEmotion.Add($"{Main.npc[witchDoctor].GivenName} is of Lihzahrd species? Fascinating, another intelligent species.", emotion: PortraitEmotion.Thinking);
			}
			int clothier = NPC.FindFirstNPC(NPCID.Clothier);
			if (clothier >= 0 && npcTypeListHouse.Contains(NPCID.Clothier))
			{
				chatEmotion.Add($"{Main.npc[clothier].GivenName} had some sort of curse? Well I'm glad he is feeling better now...", emotion: PortraitEmotion.Thinking);
				if (Main.npc[clothier].GivenName == "James")
				{
					chatEmotion.Add($"{Main.npc[clothier].GivenName} has a very interesting couch; one that I have never seen before.", 0.5, emotion: PortraitEmotion.Thinking);
				}
			}
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
			if (mechanic >= 0 && npcTypeListVillage.Contains(NPCID.Mechanic))
			{
				chatEmotion.Add($"{Main.npc[mechanic].GivenName} is fascinated with the technology that I have. I would be, too!", emotion: PortraitEmotion.Happy);
			}
			int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
			if (partyGirl >= 0 && npcTypeListVillage.Contains(NPCID.PartyGirl))
			{
				chatEmotion.Add($"{Main.npc[partyGirl].GivenName}'s dance moves are out of this world!", emotion: PortraitEmotion.Shocked);
			}
			int wizard = NPC.FindFirstNPC(NPCID.Wizard);
			if (wizard >= 0 && npcTypeListVillage.Contains(NPCID.Wizard))
			{
				chatEmotion.Add($"{Main.npc[wizard].GivenName} keeps mistaking me for somebody else. The thing is, though, I look nothing like the other villagers here...");
			}
			int taxCollector = NPC.FindFirstNPC(NPCID.TaxCollector);
			if (taxCollector >= 0 && npcTypeListVillage.Contains(NPCID.TaxCollector))
			{
				chatEmotion.Add($"{Main.npc[taxCollector].GivenName} refuses to accept my currency. Doesn't he know it's the way of the future?", emotion: PortraitEmotion.Angry);
			}
			int truffle = NPC.FindFirstNPC(NPCID.Truffle);
			if (truffle >= 0 && npcTypeListHouse.Contains(NPCID.Truffle))
			{
				chatEmotion.Add($"{Main.npc[truffle].GivenName} is proof that life is mysterious.", emotion: PortraitEmotion.Thinking);
			}
			int pirate = NPC.FindFirstNPC(NPCID.Pirate);
			if (pirate >= 0 && npcTypeListNearBy.Contains(NPCID.Pirate))
			{
				chatEmotion.Add($"{Main.npc[pirate].GivenName} keeps offering me crackers. I'm not going to refuse.", emotion: PortraitEmotion.Smirk);
			}
			int steampunker = NPC.FindFirstNPC(NPCID.Steampunker);
			if (steampunker >= 0 && npcTypeListVillage.Contains(NPCID.Steampunker))
			{
				chatEmotion.Add($"The Clentaminator that {Main.npc[steampunker].GivenName} invented is very powerful. I wonder if it has other uses...", emotion: PortraitEmotion.Thinking);
				if (Main.npc[steampunker].GivenName == "Whitney")
				{
					chatEmotion.Add($"The guitar that {Main.npc[steampunker].GivenName} owns is nice to listen to. It reminds me of simpler times.", 0.5, emotion: PortraitEmotion.Happy);
				}
			}
			int cyborg = NPC.FindFirstNPC(NPCID.Cyborg);
			if (cyborg >= 0 && npcTypeListVillage.Contains(NPCID.Cyborg))
			{
				chatEmotion.Add($"I heard {Main.npc[cyborg].GivenName} has an invisible building material and I'm really interested in such a material. Do you know when he will sell them?", emotion: PortraitEmotion.Thinking);
			}
			int santa = NPC.FindFirstNPC(NPCID.SantaClaus);
			if (santa >= 0 && npcTypeListHouse.Contains(NPCID.SantaClaus))
			{
				chatEmotion.Add($"{Main.npc[santa].GivenName} is such a jolly fellow. No wonder humans enjoy this holiday!", emotion: PortraitEmotion.VeryHappy);
			}
			int princess = NPC.FindFirstNPC(NPCID.Princess);
			if (princess >= 0 && npcTypeListVillage.Contains(NPCID.Princess))
			{
				chatEmotion.Add($"{Main.npc[princess].GivenName} is so ecstatic! She calls me her 'friendly alien' some times. Heh, well I guess it's true!", emotion: PortraitEmotion.VeryHappy);
				if (Main.npc[princess].GivenName == "Yorai")
				{
					chatEmotion.Add($"{Main.npc[princess].GivenName} seems to know a lot about technology. They claim to have 'created' most of things on this planet which doesn't make sense to me.", 0.5, emotion: PortraitEmotion.Worried);
				}
			}

			if (ModLoader.TryGetMod("SGAmod", out Mod sgamod) && townNPCsCrossModSupport) //SGAmod
			{
				if (sgamod.TryFind<ModNPC>("Dergon", out ModNPC drakenModNPC))
				{
					int draken = NPC.FindFirstNPC(drakenModNPC.Type);
					if (draken >= 0 && npcTypeListVillage.Contains(sgamod.Find<ModNPC>("Dergon").Type))
					{
						chatEmotion.Add("That Draken has a lot going through his head. He's a nice guy once you get to know him, though.", emotion: PortraitEmotion.Worried);
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
						chatEmotion.Add("I didn't expect to see somebody like Amidias! This planet is full of surprises!", emotion: PortraitEmotion.Shocked);
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
						chatEmotion.Add($"I am thankful to see somebody like {Main.npc[cook].GivenName}!", emotion: PortraitEmotion.Happy);
						if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
						{
							chatEmotion.Add($"Whatever {Main.npc[cook].GivenName} is cooking smells wonderful!", emotion: PortraitEmotion.VeryHappy);
						}
					}
				}
				if (thorium.TryFind<ModNPC>("Blacksmith", out ModNPC blacksmithModNPC))
				{
					int blacksmith = NPC.FindFirstNPC(blacksmithModNPC.Type);
					if (blacksmith >= 0 && npcTypeListNearBy.Contains(thorium.Find<ModNPC>("Blacksmith").Type))
					{
						chatEmotion.Add($"I'm not sure what kind of Durasteel {Main.npc[blacksmith].GivenName} is working with, but it's certainly not the one I'm familiar with.");
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
						chatEmotion.Add($"{Main.npc[brewer].GivenName} has all sorts of interesting potions. I might have to try some for myself.");
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
						chatEmotion.Add($"{Main.npc[brewer2].GivenName} has all sorts of interesting potions. I might have to try some for myself.");
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
						chatEmotion.Add($"I feel like I'm not supposed to see {Main.npc[examplePerson].GivenName}.", emotion: PortraitEmotion.Worried);
					}
				}
			}
			if (ModLoader.TryGetMod("NoNPCHappinessReborn", out Mod _) && townNPCsCrossModSupport) // Happiness Removal Reborn
			{
				chatEmotion.Add("Thanks for removing happiness. Now, I am eternally unhappy.", 2.0, emotion: PortraitEmotion.Sad);
			}
			if (ModLoader.TryGetMod("StarlightRiver", out Mod starlightRiver) && townNPCsCrossModSupport) // Starlight River
			{
				// Only add this chatEmotion message if the player has unlocked Starlight, which is unlocked after the Crow (Alican) cut scene happens.
				if (starlightRiver.TryFind<ModPlayer>("AbilityHandler", out ModPlayer abilityHandler))
				{
					// Try to get the value of the AnyUnlocked which is true if the player has unlocked the abilities.
					// https://github.com/ProjectStarlight/StarlightRiver/blob/master/Content/Abilities/AbilityHandler.cs#L63
					// I suck at reflection.
					PropertyInfo anyUnlocked = abilityHandler.GetType().GetProperty("AnyUnlocked", BindingFlags.Public | BindingFlags.Instance);
					object anyUnlocks = anyUnlocked?.GetValue(Main.LocalPlayer.GetModPlayer(abilityHandler));
					if (anyUnlocks?.ToString() == "True")
					{
						chatEmotion.Add("Did you see that Starlight person who came through that portal? You mentioned their name was Alican? I'm very interested in who they are; if only I were able to meet them before they went back through that portal.", emotion: PortraitEmotion.Shocked);
					}
				}
			}
			return chatEmotion.GetRandomChat();
		}
		#endregion

		#region Buttons

		public override void RegisterChatButtons(NPCInteractionList interactions)
		{
			interactions.Prepend(NPCInteractions.Shop(ShopName));
			NPCInteractionList.Entry questButton = interactions.InsertAfter(new QuestButton(), NPCInteractionDatabase.CloseButton);
			interactions.InsertAfter(new QuestChecklistButton(), questButton);
		}

		public class QuestButton : NPCInteraction
		{
			public override string GetText() => Language.GetTextValue("LegacyInterface.64"); //Quest
			public override bool Condition() => true;
			public override void Interact()
			{
				InterstellarTraveler.QuestSystem(TalkNPC);
				Main.DoNPCPortraitHop();
			}
			public override bool ShowExcalmation => CheckIfQuestIsAvailableToTurnIn(out _, out _);
			public override void TextColor(ref Color chatColor, ref Color chatColorShadow, bool hoveringOverButton)
			{
				if (CheckIfQuestIsAvailableToTurnIn(out _, out _))
				{
					chatColor = Color.Orange * (Main.mouseTextColor / 255f);
				}
			}
		}

		public class QuestChecklistButton : NPCInteraction
		{
			public override string GetText() => "Quest Checklist";
			public override bool Condition() => true;
			public override void Interact()
			{
				InterstellarTraveler.QuestSystemChecklist();
				Main.DoNPCPortraitHop();
			}
		}
		#endregion

		#region Quest System
		/// <summary>
		/// Turns in a quest if the player is able to do that, otherwise it gives hints.
		/// </summary>
		/// <param name="npc"></param>
		public static void QuestSystem(NPC npc)
		{
			Mod mod = ModContent.GetInstance<RijamsMod>();
			if (
				TurnInQuestItem_Outer(mod, npc, ModContent.ItemType<OddDevice>(), ref RijamsModWorld.intTravQuestOddDevice, RijamsModMessageType.SetQuestOddDevice)
				|| TurnInQuestItem_Outer(mod, npc, ModContent.ItemType<BlankDisplay>(), ref RijamsModWorld.intTravQuestBlankDisplay, RijamsModMessageType.SetQuestBlankDisplay)
				|| TurnInQuestItem_Outer(mod, npc, ModContent.ItemType<TeleportationCore>(), ref RijamsModWorld.intTravQuestTPCore, RijamsModMessageType.SetQuestTPCore)
				|| TurnInQuestItem_Outer(mod, npc, ModContent.ItemType<BreadAndJelly>(), ref RijamsModWorld.intTravQuestBreadAndJelly, RijamsModMessageType.SetQuestBreadAndJelly, secret: true)
				|| TurnInQuestItem_Outer(mod, npc, ModContent.ItemType<MagicOxygenizer>(), ref RijamsModWorld.intTravQuestMagicOxygenizer, RijamsModMessageType.SetQuestMagicOxygenizer)
				|| TurnInQuestItem_Outer(mod, npc, ModContent.ItemType<PrimeThruster>(), ref RijamsModWorld.intTravQuestPrimeThruster, RijamsModMessageType.SetQuestPrimeThruster)
				)
			{
				return;
			}
			else
			{
				QuestItemHints();
			}
		}

		public static bool TurnInQuestItem_Outer(Mod mod, NPC npc, int item, ref bool worldFlag, RijamsModMessageType packetEnum, bool secret = false)
		{
			if (Main.LocalPlayer.HasItem(item) && worldFlag == false)
			{
				switch (packetEnum)
				{
					case RijamsModMessageType.SetQuestOddDevice:
						TurnInQuestItem_Inner_OddDevice(npc);
						break;
					case RijamsModMessageType.SetQuestBlankDisplay:
						TurnInQuestItem_Inner_BlankDisplay();
						break;
					case RijamsModMessageType.SetQuestTPCore:
						TurnInQuestItem_Inner_TeleportationCore();
						break;
					case RijamsModMessageType.SetQuestBreadAndJelly:
						TurnInQuestItem_Inner_BreadAndJelly();
						break;
					case RijamsModMessageType.SetQuestMagicOxygenizer:
						TurnInQuestItem_Inner_MagicOxygenizer();
						break;
					case RijamsModMessageType.SetQuestPrimeThruster:
						TurnInQuestItem_Inner_PrimeThruster();
						break;
					default:
						break;
				}
				Main.npcChatCornerItem = item;
				Main.LocalPlayer.ConsumeItem(item);
				
				worldFlag = true;
				if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
				{
					// NetMessage.SendData(MessageID.WorldData);
					//RijamsModWorld.SetIntTravQuestOddDevice();
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)packetEnum);
					//packet.Write((byte)npc.whoAmI);
					packet.Send();
				}

				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ItemTransfer, new ParticleOrchestraSettings
				{
					PositionInWorld = Main.LocalPlayer.Center,
					MovementVector = npc.Center - Main.LocalPlayer.Center,
					UniqueInfoPiece = item
				});
				mod.Logger.Debug($"RijamsMod: {packetEnum} quest completed.");
				PlayCompleteQuestSound(secret);
				return true;
			}
			return false;
		}

		public static void TurnInQuestItem_Inner_OddDevice(NPC npc)
		{
			Main.npcChatText = "I was tracking that device you have. Where did you get it? Yeah... it's irresponsible of me to enter an uncontacted planet, but I didn't have much choice. Anyway, I'll buy that device from you for 2[i:73]."; ;
			Main.npcChatPortrait = ChooseTheCorrectVariantPortrait("Thinking");
			Main.LocalPlayer.QuickSpawnItem(npc.GetSource_FromThis(), ItemID.GoldCoin, 2);
		}

		public static void TurnInQuestItem_Inner_BlankDisplay()
		{
			Main.npcChatText = "Ah, I could program this device to display certain useful aspects about yourself. Take a look at my shop if you would like to have one. I will randomly offer two of these displays every time you talk to me. And don't worry, I won't be selling your data or anything.";
			Main.npcChatPortrait = ChooseTheCorrectVariantPortrait("Smirk");
		}

		public static void TurnInQuestItem_Inner_TeleportationCore()
		{
			Main.npcChatText = "This looks interesting! I bet I could use this to repair the hyper-drive on my ship. Its magical properties could let me travel even faster than before! I might even be able to create a device that can let you utilize its teleporting capabilities, too.";
			Main.npcChatPortrait = ChooseTheCorrectVariantPortrait("Thinking");
		}
		public static void TurnInQuestItem_Inner_BreadAndJelly()
		{
			Main.npcChatText = "You're offering me food? Well, I'll never deny food. This bread and this jelly seem to be very high quality. Let me open this jar and slice this bread.\nMmmm... Thanks!";
			Main.npcChatPortrait = ChooseTheCorrectVariantPortrait("Blushing");
		}
		public static void TurnInQuestItem_Inner_MagicOxygenizer()
		{
			Main.npcChatText = "This machine seems to create oxygen from only electricity. How does it do that? Well it is magic, I guess. Anyway, I could use this on my ship and to create a personal breathing device!";
			Main.npcChatPortrait = ChooseTheCorrectVariantPortrait("Shocked");
		}
		public static void TurnInQuestItem_Inner_PrimeThruster()
		{
			Main.npcChatText = "This is the perfect replacement for my ship's thrusters. It seems like the magic from the this planet has been very beneficial for repairing my ship. Oh, and you of course! Thanks!";
			Main.npcChatPortrait = ChooseTheCorrectVariantPortrait("VeryHappy");
		}

		public static void QuestItemHints()
		{
			ChatWithPortrait lines = new();
			if (!RijamsModWorld.intTravQuestBreadAndJelly)
			{
				lines.Add("I'd be happy to take a look at other items, too; if you think I could use them for something.", emotion: PortraitEmotion.Happy);
			}
			if (NPCHelper.AllQuestsCompleted())
			{
				Main.npcChatCornerItem = ModContent.ItemType<QuestTrackerComplete>();
				lines.Add("It looks like you've found everything I needed, thanks!", emotion: PortraitEmotion.VeryHappy);
				lines.Add("Nice job! You have collected and turned in everything I needed.", emotion: PortraitEmotion.VeryHappy);
				lines.Add("With your help, I have everything I need to repair my ship! I quite like it here, though. I might stay a little longer!", emotion: PortraitEmotion.Smirk);
				lines.Add("I'm going to be in so much trouble when I get back home. Not only have I been MIA for a long time, but when I do show up, how do I explain all of the magical parts in my ship? Sorry, none of this is your fault. You were great help!", emotion: PortraitEmotion.Worried);
			}
			else
			{
				Main.npcChatCornerItem = ModContent.ItemType<QuestTrackerIncomplete>();
				lines.Add("I'm looking for some specific items to repair my space ship. If you think have anything I'd be interested in, then feel free to talk to me.");
				lines.Add("I need some items to repair my space ship. Do you think you could help me out?");
				lines.Add("Look at my checklist to see which quests you have completed and what I still need.");

				if (!RijamsModWorld.intTravQuestOddDevice)
				{
					lines.Add("I was tracking that [c/FFFF00:device] that you have. Could I take a look at it?", emotion: PortraitEmotion.Thinking);
					lines.Add("Would you let me take a look at the [c/FFFF00:device] that you were carrying around?", emotion: PortraitEmotion.Thinking);
				}
				if (!RijamsModWorld.intTravQuestBlankDisplay)
				{
					lines.Add("I could use some sort of electronic screen. Something to [c/FFFF00:display] information on. It shouldn't be too hard for you to craft.", emotion: PortraitEmotion.Smirk);
					lines.Add("Just some glass, a lens, and some metal would be all that is required to craft something to [c/FFFF00:display] information on.", emotion: PortraitEmotion.Happy);
				}
				if (!RijamsModWorld.intTravQuestTPCore && Main.hardMode)
				{
					lines.Add("My hyper-drive needs to be repaired. There seems to be new creatures in this world who have the ability to [c/FFFF00:teleport].", emotion: PortraitEmotion.Thinking);
					lines.Add("Several creatures have the ability to [c/FFFF00:teleport]. Harnessing that ability would be perfect for repairing my hyper-drive.", emotion: PortraitEmotion.Happy);
				}
				if (!RijamsModWorld.intTravQuestMagicOxygenizer && NPC.downedMechBossAny)
				{
					lines.Add("My ship's oxygen supplier isn't working anymore, which makes it inconvenient to repair my ship. If you were able to create a device that can [c/FFFF00:create oxygen], that would be very helpful.", emotion: PortraitEmotion.Smirk);
					lines.Add("The magic in this world is fascinating! If you were able to create a device that can [c/FFFF00:create oxygen], I could use that to repair the oxygen supplier on my ship.", emotion: PortraitEmotion.Happy);
				}
				if (!RijamsModWorld.intTravQuestPrimeThruster && NPC.downedPlantBoss)
				{
					lines.Add("Without thrusters, my ship isn't going to move anywhere! A new [c/FFFF00:thruster] should solve that, of course!", emotion: PortraitEmotion.Smirk);
					int cyborg = NPC.FindFirstNPC(NPCID.Cyborg);
					if (cyborg >= 0)
					{
						lines.Add($"{Main.npc[cyborg].FullName} has several rockets available. I bet you could use those to craft a new [c/FFFF00:thruster] for my ship.", emotion: PortraitEmotion.Thinking);
					}
				}
			}
			ChatWithPortrait.DialogWithEmotion chosenLine = lines.ChatDB.Get();
			Main.npcChatText = chosenLine.Chat;
			Main.npcChatPortrait = ChooseTheCorrectVariantPortrait(chosenLine.Emotion.ToString());
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
				Main.npcChatPortrait = ChooseTheCorrectVariantPortrait("VeryHappy");
			}
			else
			{
				Main.npcChatCornerItem = ModContent.ItemType<QuestTrackerIncomplete>();
				Main.npcChatPortrait = ChooseTheCorrectVariantPortrait("Thinking");
			}
		}
		public static void PlayCompleteQuestSound(bool secret)
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

			foreach (Player searchPlayer in Main.ActivePlayers)
			{
				if (!RijamsModWorld.intTravQuestOddDevice && searchPlayer.HasItem(ModContent.ItemType<OddDevice>())) { which = 0; return true; }
				if (!RijamsModWorld.intTravQuestBlankDisplay && searchPlayer.HasItem(ModContent.ItemType<BlankDisplay>())) { which = 1; return true; }
				if (!RijamsModWorld.intTravQuestTPCore && searchPlayer.HasItem(ModContent.ItemType<TeleportationCore>())) { which = 2; return true; }
				if (!RijamsModWorld.intTravQuestBreadAndJelly && searchPlayer.HasItem(ModContent.ItemType<BreadAndJelly>())) { which = 3; secret = true; return true; }
				if (!RijamsModWorld.intTravQuestMagicOxygenizer && searchPlayer.HasItem(ModContent.ItemType<MagicOxygenizer>())) { which = 4; return true; }
				if (!RijamsModWorld.intTravQuestPrimeThruster && searchPlayer.HasItem(ModContent.ItemType<PrimeThruster>())) { which = 5; return true; }
			}
			return false;
		}

		#endregion

		#region Shop
		public override void AddShops()
		{
			var npcShop = new NPCShop(Type, ShopName)
				.Add(ModContent.ItemType<InterstellarPistol>(), Condition.DownedSkeletron, Condition.NpcIsPresent(NPCID.ArmsDealer))
				.Add(ModContent.ItemType<InterstellarCrossbow>(), Condition.DownedMechBossAny)
				.Add(ModContent.ItemType<PlasmaRifle>(), Condition.DownedPlantera)
				.Add(ModContent.ItemType<InterstellarSMG>(), Condition.DownedPlantera)
				.Add(ModContent.ItemType<AGMMissileLauncher>(), Condition.DownedGolem, Condition.NpcIsPresent(NPCID.Cyborg))
				.Add(ModContent.ItemType<InterstellarSniper>(), Condition.DownedCultist)
				.Add(ModContent.ItemType<InterstellarCarbine>(), Condition.DownedMoonLord)
				// .Add(ModContent.ItemType<ControlGlove>(), new Condition("After defeating Deerclops or in Hardmode", () => Condition.DownedDeerclops.IsMet() || Condition.Hardmode.IsMet()));
				.Add(ModContent.ItemType<ControlGlove>(), ShopConditions.OrConditions(Condition.DownedDeerclops, Condition.Hardmode));

			Condition watchRandom(int numberToCheck) => new("Swaps between Watches at random", () => Main.GameUpdateCount % 2 == numberToCheck);
			npcShop.Add(new Item(ItemID.GoldWatch) { shopCustomPrice = 10000 }, watchRandom(0));
			npcShop.Add(new Item(ItemID.PlatinumWatch) { shopCustomPrice = 10000 }, watchRandom(1));
			npcShop.Add(ItemID.DepthMeter);
			npcShop.Add(ItemID.Compass);
			npcShop.Add(ItemID.Radar);
			npcShop.Add(ItemID.MetalDetector);
			npcShop.Add(ItemID.TallyCounter, Condition.DownedSkeletron);

			// Sell more of the Traveling Merchant's info items the more Town NPCs there are.
			Item lifeformAnalyzer = new(ItemID.LifeformAnalyzer) { shopCustomPrice = 75000 };
			Item dPSMeter = new(ItemID.DPSMeter) { shopCustomPrice = 75000 };
			Item stopwatch = new(ItemID.Stopwatch) { shopCustomPrice = 75000 };

			Condition TownNPCRange1013 = ShopConditions.TownNPCRange(10, 13);
			Condition TownNPCRange1419 = ShopConditions.TownNPCRange(14, 19);
			Condition TownNPCOver20 = ShopConditions.CountTownNPCs(20);

			npcShop.Add(lifeformAnalyzer, ShopConditions.MoonPhase036, TownNPCRange1013);
			npcShop.Add(dPSMeter, ShopConditions.MoonPhase147, TownNPCRange1013);
			npcShop.Add(stopwatch, ShopConditions.MoonPhase25, TownNPCRange1013);

			npcShop.Add(lifeformAnalyzer, ShopConditions.MoonPhase036, TownNPCRange1419);
			npcShop.Add(dPSMeter, ShopConditions.MoonPhase036, TownNPCRange1419);

			npcShop.Add(dPSMeter, ShopConditions.MoonPhase147, TownNPCRange1419);
			npcShop.Add(stopwatch, ShopConditions.MoonPhase147, TownNPCRange1419);

			npcShop.Add(stopwatch, ShopConditions.MoonPhase25, TownNPCRange1419);
			npcShop.Add(lifeformAnalyzer, ShopConditions.MoonPhase25, TownNPCRange1419);

			npcShop.Add(lifeformAnalyzer, TownNPCOver20);
			npcShop.Add(dPSMeter, TownNPCOver20);
			npcShop.Add(stopwatch, TownNPCOver20);

			// Sell more of the Angler info items the more quests have been completed.
			Item fishermansGuide = new(ItemID.FishermansGuide) { shopCustomPrice = 75000 };
			Item sextant = new(ItemID.Sextant) { shopCustomPrice = 75000 };
			Item weatherRadio = new(ItemID.WeatherRadio) { shopCustomPrice = 75000 };

			Condition AnglerQuestsFinishedRange13 = ShopConditions.AnglerQuestsFinishedRange(1, 3);
			Condition AnglerQuestsFinishedRange45 = ShopConditions.AnglerQuestsFinishedRange(4, 5);

			npcShop.Add(fishermansGuide, ShopConditions.MoonPhase036, AnglerQuestsFinishedRange13, Condition.NpcIsPresent(NPCID.Angler));
			npcShop.Add(sextant, ShopConditions.MoonPhase147, AnglerQuestsFinishedRange13, Condition.NpcIsPresent(NPCID.Angler));
			npcShop.Add(weatherRadio, ShopConditions.MoonPhase25, AnglerQuestsFinishedRange13, Condition.NpcIsPresent(NPCID.Angler));

			npcShop.Add(fishermansGuide, ShopConditions.MoonPhase036, AnglerQuestsFinishedRange45, Condition.NpcIsPresent(NPCID.Angler));
			npcShop.Add(sextant, ShopConditions.MoonPhase036, AnglerQuestsFinishedRange45, Condition.NpcIsPresent(NPCID.Angler));

			npcShop.Add(sextant, ShopConditions.MoonPhase147, AnglerQuestsFinishedRange45, Condition.NpcIsPresent(NPCID.Angler));
			npcShop.Add(weatherRadio, ShopConditions.MoonPhase147, AnglerQuestsFinishedRange45, Condition.NpcIsPresent(NPCID.Angler));

			npcShop.Add(stopwatch, ShopConditions.MoonPhase25, AnglerQuestsFinishedRange45, Condition.NpcIsPresent(NPCID.Angler));
			npcShop.Add(fishermansGuide, ShopConditions.MoonPhase25, AnglerQuestsFinishedRange45, Condition.NpcIsPresent(NPCID.Angler));

			npcShop.Add(fishermansGuide, Condition.AnglerQuestsFinishedOver(6), Condition.NpcIsPresent(NPCID.Angler));
			npcShop.Add(sextant, Condition.AnglerQuestsFinishedOver(6), Condition.NpcIsPresent(NPCID.Angler));
			npcShop.Add(weatherRadio, Condition.AnglerQuestsFinishedOver(6), Condition.NpcIsPresent(NPCID.Angler));

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

			npcShop.Add(ItemID.RodofDiscord, ShopConditions.IntTravQuestTPCore, Condition.Hardmode);
			npcShop.Add(ModContent.ItemType<Items.Consumables.RyeJam>(), ShopConditions.IntTravQuestBreadAndJelly);
			npcShop.Add(ModContent.ItemType<BreathingPack>(), ShopConditions.IntTravQuestMagicOxygenizer);
			npcShop.Add(ModContent.ItemType<RocketBooster>(), ShopConditions.IntTravQuestPrimeThruster);
			npcShop.Add(ModContent.ItemType<MatterManipulator>(), ShopConditions.IntTravQuestAllComplete, Condition.DownedMoonLord);
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
			//Main.NewText("NPC.ai[0] " + NPC.ai[0] + " NPC.frame.Y " + NPC.frame.Y + " NPC.frame.Height " + NPC.frame.Height);

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

		private static bool CasualPortraitEmotionCondtion(PortraitEmotion emotion)
		{
			if (NPCHelper.AllQuestsCompleted())
			{
				return ChatWithPortrait.PartyPortraitEmotionCondtion(emotion);
			}
			return false;
		}

		private static bool CasualShowingHappinessText(float minHappiness, float maxHappiness)
		{
			if (NPCHelper.AllQuestsCompleted())
			{
				return ChatWithPortrait.PartyShowingHappinessText(minHappiness, maxHappiness);
			}
			return false;
		}

		private static bool CasualShowingHousingText()
		{
			if (NPCHelper.AllQuestsCompleted())
			{
				return ChatWithPortrait.PartyShowingHousingText();
			}
			return false;
		}

		public static NPCID.Sets.BasicNPCPortrait ChooseTheCorrectVariantPortrait(string mood)
		{
			if (ModContent.GetInstance<RijamsModConfigClient>().Ornithophobia)
			{
				return NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath("RijamsMod/NPCs/TownNPCs", "InterstellarTraveler", "Helmet", ""));
			}
			else if (NPCHelper.PartyPortraitCondition() && NPCHelper.AllQuestsCompleted())
			{
				return NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath("RijamsMod/NPCs/TownNPCs", "InterstellarTraveler", "Casual", mood));
			}
			else if (NPCID.Sets.ShimmeredPortraitCondition() && NPCHelper.PartyPortraitCondition())
			{
				return NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath("RijamsMod/NPCs/TownNPCs", "InterstellarTraveler", "Shimmer_Hatless", mood));
			}
			else if (NPCID.Sets.ShimmeredPortraitCondition())
			{
				return NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath("RijamsMod/NPCs/TownNPCs", "InterstellarTraveler", "Shimmer", mood));
			}
			return NPCID.Sets.BasicPortrait(ChatWithPortrait.PortraitPath("RijamsMod/NPCs/TownNPCs", "InterstellarTraveler", "Default", mood));
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

		private Asset<Texture2D> helmet;
		private Asset<Texture2D> normal;
		private Asset<Texture2D> casual;
		private Asset<Texture2D> shimmered;
		private Asset<Texture2D> shimmeredParty;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (ModContent.GetInstance<RijamsModConfigClient>().Ornithophobia)
			{
				return helmet ??= ModContent.Request<Texture2D>(Path + "_Helmet");
			}
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
			{
				return normal ??= ModContent.Request<Texture2D>(Path);
			}
			if (npc.altTexture == 1 && NPCHelper.AllQuestsCompleted())
			{
				return casual ??= ModContent.Request<Texture2D>(Path + "_Casual");
			}

			if (npc.IsShimmerVariant && npc.altTexture != 1)
			{
				return shimmered ??= ModContent.Request<Texture2D>(Namespace + "/Shimmered/" + NPCName);
			}
			if (npc.IsShimmerVariant && npc.altTexture == 1)
			{
				return shimmeredParty ??= ModContent.Request<Texture2D>(Namespace + "/Shimmered/" + NPCName + "_Hatless");
			}

			return normal ??= ModContent.Request<Texture2D>(Path);
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
