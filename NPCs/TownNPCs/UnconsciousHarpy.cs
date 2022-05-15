using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RijamsMod.NPCs.TownNPCs
{
	public class UnconsciousHarpy : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unconscious Harpy");
			//NPCID.Sets.TownCritter[npc.type] = true;
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.friendly = true;
			npc.npcSlots = 5f;
			//npc.townNPC = true;
			//npc.dontTakeDamage = true;
			npc.width = 72;
			npc.height = 22;
			npc.aiStyle = 0;
			npc.damage = 0;
			npc.defense = 15;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.1f;
			npc.rarity = 1;
		}
		/*public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return true;
		}*/
		public override bool CanChat() //from Calamity's Vanities
		{
			return true;
		}

		public override void AI()
		{
			npc.breath += 2;
			npc.TargetClosest(false);
			npc.spriteDirection = 1;//npc.direction;
			//From Spirit mod
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				npc.homeless = false;
				npc.homeTileX = -1;
				npc.homeTileY = -1;
				npc.netUpdate = true;
			}
			foreach (var player in Main.player)
			{
				if (!player.active) continue;
				if (player.talkNPC == npc.whoAmI)
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
			mod.Logger.Debug("RijamsMod: Harpy NPC rescued.");
			return "Ow, my head hurts... AHH! Don't attack me, please! I'm friendly... you too?";
		}
		
		public override bool UsesPartyHat()
		{ 
			return false;
		}

		public void Rescue()
        {
			npc.dontTakeDamage = false;
			RijamsModWorld.savedHarpy = true;
			RijamsModWorld.UpdateWorldBool();
			npc.Transform(ModContent.NPCType<Harpy>());
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) //from Calamity's Vanities
		{
			if (spawnInfo.player.ZoneSkyHeight && !RijamsModWorld.savedHarpy && !NPC.AnyNPCs(ModContent.NPCType<UnconsciousHarpy>()) && !NPC.AnyNPCs(ModContent.NPCType<Harpy>()))
			{
				if (spawnInfo.spawnTileType == TileID.Cloud || spawnInfo.spawnTileType == TileID.RainCloud || spawnInfo.spawnTileType == TileID.Grass || spawnInfo.spawnTileType == TileID.Sunplate || spawnInfo.spawnTileType == TileID.SnowCloud || spawnInfo.spawnTileType == TileID.Dirt)
				{
					return 0.75f;
				}				
			}
			return 0f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Harpy_Gore_Head"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Harpy_Gore_Arm"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Harpy_Gore_Leg"), 1f);
			}
		}
	}
}