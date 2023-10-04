using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.IO;
using System.Xml;
using System;
using Terraria.GameContent.UI;

namespace RijamsMod.NPCs.TownNPCs.SnuggetPet
{
	[AutoloadHead]
	public class SnuggetPet : ModNPC
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return true;
		}

		internal static int HeadIndex1;
		internal static int HeadIndex2;
		internal static int HeadIndex3;
		internal static int HeadIndex4;
		internal static int HeadIndex5;
		internal static int HeadIndex6;
		internal static int HeadIndex7;
		internal static int HeadIndex8;
		private static ITownNPCProfile NPCProfile;

		public override void Load()
		{
			// Adds our Shimmer Head to the NPCHeadLoader.
			HeadIndex1 = Mod.AddNPCHeadTexture(Type, Texture + "_1_Head");
			HeadIndex2 = Mod.AddNPCHeadTexture(Type, Texture + "_2_Head");
			HeadIndex3 = Mod.AddNPCHeadTexture(Type, Texture + "_3_Head");
			HeadIndex4 = Mod.AddNPCHeadTexture(Type, Texture + "_4_Head");
			HeadIndex5 = Mod.AddNPCHeadTexture(Type, Texture + "_5_Head");
			HeadIndex6 = Mod.AddNPCHeadTexture(Type, Texture + "_6_Head");
			HeadIndex7 = Mod.AddNPCHeadTexture(Type, Texture + "_7_Head");
			HeadIndex8 = Mod.AddNPCHeadTexture(Type, Texture + "_8_Head");
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Snugget");
			Main.npcFrameCount[NPC.type] = 27;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 20;
			NPCID.Sets.AttackFrameCount[NPC.type] = 0;
			NPCID.Sets.DangerDetectRange[NPC.type] = 250;
			NPCID.Sets.AttackType[NPC.type] = -1;
			NPCID.Sets.AttackTime[NPC.type] = -1;
			NPCID.Sets.AttackAverageChance[NPC.type] = 1;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = false;
			NPCID.Sets.ExtraTextureCount[Type] = 0;
			NPCID.Sets.NPCFramingGroup[Type] = 6;

			NPCID.Sets.IsTownPet[Type] = true;
			NPCID.Sets.CannotSitOnFurniture[Type] = true;
			NPCID.Sets.TownNPCBestiaryPriority.Add(Type);

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
							   //Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPCProfile = new SnuggetPetProfile();
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 30;
			NPC.height = 30;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.knockBackResist = 0.5f;
			NPC.housingCategory = 1;
			AnimationType = NPCID.TownBunny;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtSnugget>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name))
			});
		}

		public override void PostAI()
		{
			// Make it so the Snugget can show emotes!

			// Talking to another Town NPC
			if (NPC.ai[0] == 3f || NPC.ai[0] == 4f)
			{
				NPC.localAI[0]++;

				bool flag12 = NPC.ai[0] == 3f;
				int time1 = -1;
				int time2 = -1;

				if (NPC.localAI[0] == 216f && Main.netMode != NetmodeID.MultiplayerClient)
				{
					time1 = 70;
				}
				else if (NPC.localAI[0] == 320f)
				{
					time1 = 100;
				}

				if (NPC.localAI[0] == 70f)
				{
					time2 = 90;
				}

				if (flag12)
				{
					NPC nPC = Main.npc[(int)NPC.ai[2]];
					if (time1 != -1)
					{
						EmoteBubble.NewBubbleNPC(new WorldUIAnchor(NPC), time1, new WorldUIAnchor(nPC));
					}

					if (time2 != -1)
					{
						EmoteBubble.NewBubbleNPC(new WorldUIAnchor(nPC), time2, new WorldUIAnchor(NPC));
					}
				}

				if (NPC.localAI[0] >= 420f)
				{
					NPC.localAI[0] = 0f;
					NPC.ai[0] = 3f; // Added
				}

			}

			// Talking to the player
			if (NPC.ai[0] == 7f || NPC.ai[0] == 19f)
			{
				NPC.localAI[0]++;

				if (NPC.localAI[0] == 16f)
				{
					EmoteBubble.NewBubbleNPC(new WorldUIAnchor(NPC), 112);
				}
				else if (NPC.localAI[0] == 160f)
				{
					EmoteBubble.NewBubbleNPC(new WorldUIAnchor(NPC), 60);
				}

				if (NPC.localAI[0] >= 220f)
				{
					NPC.frameCounter = 0f;
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			if (RijamsModWorld.boughtSnuggetPet)
			{
				return true;
			}
			return false;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			// Profiles.VariantNPCProfile("RijamsMod/NPCs/TownNPCs/TestPet", "TestPet", TestPetHeadIDs, "1", "2"); doesn't work because
			// It uses Main.Assets.Request<>() which won't find mod assets (ModContent.Request<>() is needed instead).
			return NPCProfile;
		}

		public int variationType = 0;
		public override List<string> SetNPCNameList()
		{
			//variationType = Main.rand.Next(2);

			List<string> NameList0 = new()
			{
				"Petty", "Fluffles", "Fluffy"
			};
			List<string> NameList1 = new()
			{
				"Floofy", "Puffy", "Shiny"
			};
			List<string> NameList2 = new()
			{
				"Nugget", "Snuggly", "Vibrant"
			};
			List<string> NameList3 = new()
			{
				"Snuggles", "Glowy", "Cuddles"
			};
			List<string> NameList4 = new()
			{
				"Rainbow", "Disco", "Illuminant"
			};

			return variationType switch
			{
				0 => NameList0,
				1 => NameList4, // Shimmered?
				2 => NameList2,
				3 => NameList3,
				4 => NameList0,
				5 => NameList1,
				6 => NameList2,
				7 => NameList3,
				8 => NameList1,
				_ => NameList1
			};
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new();

			chat.Add("Squeak!");
			chat.Add("Honk!");
			chat.Add("Chatter...");
			chat.Add("Mew.");

			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("UI.PetTheAnimal"); // Pet
		}

		/*public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				shop = false;
			}
		}*/

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return false;
		}

		public override Color? GetAlpha(Color drawColor)
		{
			return NPC.IsShimmerVariant ? Main.DiscoColor : Color.White; // variationType of 1 makes it shimmered, even when it isn't.
			// return Color.White;
		}

		public override void SaveData(TagCompound tag)
		{
			tag["SnuggetVariationType"] = variationType;
		}

		public override void LoadData(TagCompound tag)
		{
			variationType = tag.GetInt("SnuggetVariationType");
		}
	}

	public class SnuggetPetProfile : ITownNPCProfile
	{
		private string Namespace => GetType().Namespace.Replace('.', '/');
		private string NPCName => (GetType().Name.Split("Profile")[0]).Replace('.', '/');
		private string FilePath => (Namespace + "/" + NPCName);

		private static SnuggetPet GetModNPCForPet() // bit of a hack
		{
			int index = NPC.FindFirstNPC(ModContent.NPCType<SnuggetPet>());
			if (index > -1)
			{
				ModNPC thePet = Main.npc[index].ModNPC;
				if (thePet is SnuggetPet snuggetPet)
				{
					return snuggetPet;
				}
			}
			return null;
		}
		public int RollVariation()
		{
			int random = Main.rand.Next(8); // 8 variants
			if (random == 1) // variationType of 1 makes it shimmered, even when it isn't.
			{
				random = 8;
			}
			if (GetModNPCForPet() is not null)
			{
				GetModNPCForPet().variationType = random;
			}
			else
			{
				return 0;
			}
			return random;
		}

		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (GetModNPCForPet() is not null)
			{
				return ModContent.Request<Texture2D>(FilePath + "_" + GetModNPCForPet().variationType);
			}
			return ModContent.Request<Texture2D>(FilePath + "_" + RollVariation());
		}

		public int GetHeadTextureIndex(NPC npc)
		{
			if (GetModNPCForPet() is not null)
			{
				return GetModNPCForPet().variationType switch
				{
					0 => ModContent.GetModHeadSlot(FilePath + "_Head"),
					1 => SnuggetPet.HeadIndex1, // Shimmered?
					2 => SnuggetPet.HeadIndex2,
					3 => SnuggetPet.HeadIndex3,
					4 => SnuggetPet.HeadIndex4,
					5 => SnuggetPet.HeadIndex5,
					6 => SnuggetPet.HeadIndex6,
					7 => SnuggetPet.HeadIndex7,
					8 => SnuggetPet.HeadIndex8,
					_ => ModContent.GetModHeadSlot(FilePath + "_Head")
				};
			}
			return ModContent.GetModHeadSlot(FilePath + "_Head");
		}
	}
}