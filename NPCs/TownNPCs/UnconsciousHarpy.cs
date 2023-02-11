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
			// DisplayName.SetDefault("Unconscious Harpy");
			//NPCID.Sets.TownCritter[npc.type] = true;
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
			//npc.townNPC = true;
			//npc.dontTakeDamage = true;
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
		/*public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return true;
		}*/

		/*public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<Harpy>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
			});
		}*/

		public override bool CanChat() //from Calamity's Vanities
		{
			return true;
		}

		public override void AI()
		{
			NPC.breath += 2;
			NPC.TargetClosest(false);
			NPC.spriteDirection = 1;//npc.direction;
			//From Spirit mod
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.homeless = true;
				NPC.homeTileX = -1;
				NPC.homeTileY = -1;
				NPC.netUpdate = true;
			}
			foreach (var player in Main.player)
			{
				if (!player.active) continue;
				if (player.talkNPC == NPC.whoAmI)
				{
					Rescue();
					return;
				}
			}
		}

		public override string GetChat()
		{
			Rescue();
			//RijamsModWorld.savedHarpy = true;
			//RijamsModWorld.UpdateWorldBool();
			//npc.Transform(ModContent.NPCType<Harpy>());
			Mod.Logger.Debug("RijamsMod: Harpy NPC rescued.");
			return "Ow, my head hurts... AHH! Don't attack me, please! I'm friendly... you too?";
		}
		
		public override bool UsesPartyHat()
		{ 
			return false;
		}

		public void Rescue()
		{
			NPC.dontTakeDamage = false;
			RijamsModWorld.savedHarpy = true;
			RijamsModWorld.UpdateWorldBool();
			NPC.Transform(ModContent.NPCType<Harpy>());
			RijamsModWorld.harpyJustRescued = true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) //from Calamity's Vanities
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
		public override void HitEffect(int hitDirection, double damage)
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