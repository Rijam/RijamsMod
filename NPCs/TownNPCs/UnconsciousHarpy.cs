using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;

namespace RijamsMod.NPCs.TownNPCs
{
	public class UnconsciousHarpy : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new(0)
			{
				Hide = true // Hides this NPC from the bestiary
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
		}

		public override void SetDefaults()
		{
			NPC.friendly = true;
			NPC.npcSlots = 5f;
			NPC.width = 72;
			NPC.height = 22;
			NPC.aiStyle = 0;
			NPC.damage = 0;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.1f;
			NPC.rarity = 1;
		}

		public override bool CanChat()
		{
			return true;
		}

		public override void AI()
		{
			NPC.spriteDirection = 1;

			// This is where we check to see if a player has clicked on our NPC.
			// First, don't run this code if it is a multiplayer client.
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				// Loop through every player on the server.
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					// If the player is active (on the server) and are talking to this NPC...
					if (Main.player[i].active && Main.player[i].talkNPC == NPC.whoAmI)
					{
						NPC.Transform(ModContent.NPCType<Harpy>()); // Transform to our real Town NPC.																  
						Main.BestiaryTracker.Chats.RegisterChatStartWith(NPC); // Unlock the Town NPC in the Bestiary.																  
						Main.player[i].SetTalkNPC(NPC.whoAmI);  // Change who the player is talking to to the new Town NPC. 
						RijamsModWorld.savedHarpy = true; // Set our rescue bool to true.
						RijamsModWorld.harpyJustRescued = true;
						Mod.Logger.Debug("RijamsMod: Harpy NPC rescued.");

						// We need to sync these changes in multiplayer.
						if (Main.netMode == NetmodeID.Server)
						{
							NetMessage.SendData(MessageID.SyncTalkNPC, -1, -1, null, i);
							NetMessage.SendData(MessageID.WorldData);
						}
					}
				}
			}
		}

		public override string GetChat()
		{
			return "Ow, my head hurts... AHH! Don't attack me, please! I'm friendly... you too?";
		}
		
		public override bool UsesPartyHat()
		{ 
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneSkyHeight && !RijamsModWorld.savedHarpy && !NPC.AnyNPCs(ModContent.NPCType<UnconsciousHarpy>()) && !NPC.AnyNPCs(ModContent.NPCType<Harpy>()))
			{
				if (spawnInfo.SpawnTileType == TileID.Cloud || spawnInfo.SpawnTileType == TileID.RainCloud || spawnInfo.SpawnTileType == TileID.Grass || spawnInfo.SpawnTileType == TileID.Sunplate || spawnInfo.SpawnTileType == TileID.SnowCloud || spawnInfo.SpawnTileType == TileID.Dirt)
				{
					return 0.75f;
				}				
			}
			return 0f;
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + "Harpy_Gore_Head").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + "Harpy_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + "Harpy_Gore_Leg").Type, 1f);
				}
			}
		}
	}
}