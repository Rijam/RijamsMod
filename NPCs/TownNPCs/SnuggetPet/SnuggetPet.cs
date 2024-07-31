using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI;
using RijamsMod.EmoteBubbles;

namespace RijamsMod.NPCs.TownNPCs.SnuggetPet
{
	[AutoloadHead]
	public class SnuggetPet : ModNPC
	{
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
			Main.npcFrameCount[Type] = 27;
			NPCID.Sets.ExtraFramesCount[Type] = 20;
			NPCID.Sets.AttackFrameCount[Type] = 0;
			NPCID.Sets.DangerDetectRange[Type] = 250;
			NPCID.Sets.AttackType[Type] = -1;
			NPCID.Sets.AttackTime[Type] = -1;
			NPCID.Sets.AttackAverageChance[Type] = 1;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[Type] = true;
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

			NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<SnuggetEmote>();

			NPCID.Sets.IsPetSmallForPetting[Type] = true;
			NPCID.Sets.PlayerDistanceWhilePetting[Type] = 34;
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
			Main.npcCatchable[Type] = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs;
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

				bool chatting = NPC.ai[0] == 3f;
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

				if (chatting)
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

		public readonly List<string> NameList0 = new()
		{
			"Petty", "Fluffles", "Fluffy"
		};
		public readonly List<string> NameList1 = new()
		{
			"Floofy", "Puffy", "Shiny"
		};
		public readonly List<string> NameList2 = new()
		{
			"Nugget", "Snuggly", "Vibrant"
		};
		public readonly List<string> NameList3 = new()
		{
			"Snuggles", "Glowy", "Cuddles"
		};
		public readonly List<string> NameList4 = new()
		{
			"Rainbow", "Disco", "Illuminant"
		};

		public override List<string> SetNPCNameList()
		{
			return NPC.townNpcVariationIndex switch // Change the name based on the variation.
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

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return false; // Don't go to King or Queen statues. (Default is false so this technically isn't needed.)
		}

		public override Color? GetAlpha(Color drawColor)
		{
			return NPC.IsShimmerVariant ? Main.DiscoColor : Color.White; // variationType of 1 makes it shimmered, even when it isn't.
		}

		public override void EmoteBubblePosition(ref Vector2 position, ref SpriteEffects spriteEffects)
		{
			spriteEffects = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			position.X += NPC.width * 1.5f * NPC.spriteDirection;
		}

		// public override void ChatBubblePosition(ref Vector2 position, ref SpriteEffects spriteEffects)
		// {
			// position.X += NPC.width * NPC.spriteDirection;
		// }

		public override void PartyHatPosition(ref Vector2 position, ref SpriteEffects spriteEffects)
		{
			position.X += 5 * NPC.spriteDirection;
			position.Y += 2;
		}
	}

	public class SnuggetPetProfile : ITownNPCProfile
	{
		private static readonly string filePath = "RijamsMod/NPCs/TownNPCs/SnuggetPet/SnuggetPet";

		private readonly Asset<Texture2D> variant0 = ModContent.Request<Texture2D>(filePath);
		private readonly Asset<Texture2D> variant1 = ModContent.Request<Texture2D>($"{filePath}_1");
		private readonly Asset<Texture2D> variant2 = ModContent.Request<Texture2D>($"{filePath}_2");
		private readonly Asset<Texture2D> variant3 = ModContent.Request<Texture2D>($"{filePath}_3");
		private readonly Asset<Texture2D> variant4 = ModContent.Request<Texture2D>($"{filePath}_4");
		private readonly Asset<Texture2D> variant5 = ModContent.Request<Texture2D>($"{filePath}_5");
		private readonly Asset<Texture2D> variant6 = ModContent.Request<Texture2D>($"{filePath}_6");
		private readonly Asset<Texture2D> variant7 = ModContent.Request<Texture2D>($"{filePath}_7");
		private readonly Asset<Texture2D> variant8 = ModContent.Request<Texture2D>($"{filePath}_8");
		private readonly int headIndex0 = ModContent.GetModHeadSlot($"{filePath}_Head");

		public int RollVariation()
		{
			int random = Main.rand.Next(8); // 8 variants; 0 through 7.
			if (random == 1) // variationType of 1 makes it shimmered, even when it isn't.
			{
				random = 8; // So variation 1 becomes number 8.
			}
			return random;
		}

		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			return npc.townNpcVariationIndex switch
			{
				0 => variant0,
				1 => variant1, // Shimmered
				2 => variant2,
				3 => variant3,
				4 => variant4,
				5 => variant5,
				6 => variant6,
				7 => variant7,
				8 => variant8,
				_ => variant0
			};
		}

		public int GetHeadTextureIndex(NPC npc)
		{
			return npc.townNpcVariationIndex switch
			{
				0 => headIndex0,
				1 => SnuggetPet.HeadIndex1, // Shimmered
				2 => SnuggetPet.HeadIndex2,
				3 => SnuggetPet.HeadIndex3,
				4 => SnuggetPet.HeadIndex4,
				5 => SnuggetPet.HeadIndex5,
				6 => SnuggetPet.HeadIndex6,
				7 => SnuggetPet.HeadIndex7,
				8 => SnuggetPet.HeadIndex8,
				_ => headIndex0
			};
		}
	}
}