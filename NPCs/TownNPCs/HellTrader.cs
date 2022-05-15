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

namespace RijamsMod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class HellTrader : ModNPC
	{
		public override string Texture
		{
			get
			{
				return "RijamsMod/NPCs/TownNPCs/HellTrader";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "RijamsMod/NPCs/TownNPCs/HellTraderTown_Alt_1" };
			}
		}

		public override bool Autoload(ref string name)
		{
			name = "Hell Trader";
			return mod.Properties.Autoload;
		}

		public override void BossHeadSlot(ref int index)
		{
			if (!RijamsModWorld.hellTraderArrivable)
            {
				index = -1;
			}
		}
		//public override string HeadTexture => "RijamsMod/NPCs/TownNPCs/HellTrader_Head";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hell Trader");
			Main.npcFrameCount[npc.type] = 23;
			NPCID.Sets.ExtraFramesCount[npc.type] = 7;
			NPCID.Sets.AttackFrameCount[npc.type] = 2;
			NPCID.Sets.DangerDetectRange[npc.type] = 1000;
			NPCID.Sets.AttackType[npc.type] = 2;
			NPCID.Sets.AttackTime[npc.type] = 40;
			NPCID.Sets.AttackAverageChance[npc.type] = 10;
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
			npc.defense = 30;//def 15
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			animationType = NPCID.Guide;
			npc.lavaImmune = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.homeless = true;
			Main.npcCatchable[npc.type] = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs;
			npc.catchItem = ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs ? (short)ModContent.ItemType<CaughtHellTrader>() : (short)-1;
			npc.rarity = RijamsModWorld.hellTraderArrivable ? 0 : 1;
			npc.npcSlots = RijamsModWorld.hellTraderArrivable ? 1 : 0.25f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HellTrader_Gore_Head_Alt_1"), 1f);
				}
				else
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HellTrader_Gore_Head"), 1f);
				}
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HellTrader_Gore_Arm"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HellTrader_Gore_Leg"), 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
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

		public override string TownNPCName()
		{
			string[] names = { "Mixi", "Brima", "Sulfura", "Leh", "Inferna", "Purgator", "Haidess", "Blaiz", "Agoni", "Flaima", "Nethi", "Perdition", "Do\'om", "Braz", "Grihmos", "Da\'nur" };
			return Main.rand.Next(names);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (!RijamsModWorld.hellTraderArrivable && !NPC.AnyNPCs(npc.type) && spawnInfo.player.ZoneUnderworldHeight)
            {
				if (spawnInfo.spawnTileType == TileID.ObsidianBrick || spawnInfo.spawnTileType == TileID.HellstoneBrick || spawnInfo.spawnTileType == TileID.Platforms)
                {
					if (!NPC.downedBoss1) //Haven't killed EoC
                    {
						return 0.05f;
                    }
					else if (!NPC.downedBoss3) //Have killed EoC but not Skeletron
                    {
						return 0.075f;
					}
					else
                    {
						return 0.1f; //Else
					}
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
				npc.rarity = 1;
				float distance = Math.Abs(npc.position.X - Main.player[npc.FindClosestPlayer()].position.X) + Math.Abs(npc.position.Y - Main.player[npc.FindClosestPlayer()].position.Y);
				if (distance >= 4000f && npc.homeless)
				{
					npc.active = false;
					npc.netSkip = -1;
					npc.life = 0;
				}
			}
			if (RijamsModWorld.hellTraderArrivable)
            {
				npc.rarity = 0;
			}
		}
        /*public override bool PreAI()
        {
			if (!RijamsModWorld.hellTraderArrivable)
			{
				npc.altTexture = 0; //works, but then there is an infinite amount of confetti
				//Main.npcTexture[npc.type] = Main.npcAltTextures[npc.type][npc.altTexture];
			}
			return true;
        }*/

        public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int hellTrader = NPC.FindFirstNPC(ModContent.NPCType<HellTrader>());
			if (!RijamsModWorld.hellTraderArrivable)
            {
				chat.Add("Hello, human. An unexpected confrontation, for sure.");
				chat.Add("My robes are made from a special fabric. They are fire proof, just like me.");
				chat.Add("You shall know me by " + Main.npc[hellTrader].GivenName + ". I look forward to trading with you, " + Main.LocalPlayer.name + ".");
				chat.Add("I have some fine goods to trade with you if you have the money.");
				chat.Add("Are you here to trade? Good, I have something that might interest you.");
				chat.Add("The Underworld is a dangerous place. You are brave to venture down here.");
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
				chat.Add("Who is this \"Slayer\" that you speak of?", 0.2);

				if (npc.life < npc.lifeMax * 0.2)
				{
					chat.Add("Wait! Help! I'm not just another expendable Imp am I?", 10.0);
				}

				if (npc.homeless)
				{
					chat.Add("I accepted your request to move in expecting to receive and actual home! Can you deliver on your promise?");
				}
				else
				{
					chat.Add("Thanks again, " + Main.LocalPlayer.name + ", for asking me to live in your town. Much better than living alone in the Underworld!");
				}

				if ((Main.LocalPlayer.HasItem(ItemID.TallyCounter) || Main.LocalPlayer.HasItem(ItemID.REK) || Main.LocalPlayer.HasItem(ItemID.PDA) || Main.LocalPlayer.HasItem(ItemID.CellPhone)) && Main.player[Main.myPlayer].lastCreatureHit == Item.NPCtoBanner(NPCID.FireImp))
				//The player has the Tally Counter, R.E.K. 3000, PDA, or Cellphone in their inventory. The last enemy they hit was a Harpy and the kill count for Fire Imps is more than 0.
				//Item.NPCtoBanner(NPCID.FireImp) == 24
				{
					if (NPC.killCount[Item.NPCtoBanner(NPCID.FireImp)] > 0)
					{
						chat.Add("You've slain " + NPC.killCount[Item.NPCtoBanner(NPCID.FireImp)] + " Fire Imps, hm? Interesting.", 20.0);
					}
				}

				int harpy = NPC.FindFirstNPC(ModContent.NPCType<Harpy>());
				int fisherman = NPC.FindFirstNPC(ModContent.NPCType<Fisherman>());
				int interTravel = NPC.FindFirstNPC(ModContent.NPCType<InterstellarTraveler>());
				if (harpy >= 0)
				{
					chat.Add("I've always wanted to fly like " + Main.npc[harpy].GivenName + " or one of the greaters.", 0.5);
				}
				if (fisherman >= 0)
				{
					chat.Add(Main.npc[fisherman].GivenName + " let me eat some other kinds of fish. I was getting tired of eating Flarefin Koi and Obsidifish all of the time!", 0.5);
				}
				if (interTravel >= 0)
				{
					chat.Add("So, " + Main.npc[interTravel].GivenName + " is from a different planet? I have a lot to learn...", 0.5);
				}
				int taxCollector = NPC.FindFirstNPC(NPCID.TaxCollector);
				if (taxCollector >= 0)
				{
					chat.Add("Uh yeah, I see why " + Main.npc[taxCollector].GivenName + " was banished to the Underworld.", 0.25);
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
				Mod thorium = ModLoader.GetMod("ThoriumMod");
				if (thorium != null) //Thorium
				{
					int weaponMaster = NPC.FindFirstNPC(thorium.NPCType("WeaponMaster"));
					if (weaponMaster >= 0)
					{
						chat.Add("Hmm...\nHuh? No, I wasn't staring at " + Main.npc[weaponMaster].GivenName + "!", 0.5);
						chat.Add("So... heard anything new about " + Main.npc[weaponMaster].GivenName + "? I'm just asking...", 0.5);
						chat.Add(Main.npc[weaponMaster].GivenName + "'s helmet is very nice.\nNothing! It's just a casual observation.", 0.5);
					}
					if (player.HasItem(thorium.ItemType("PLG8999")))
                    {
						chat.Add("That big gun you have scares me! I don't even know why!", 0.5);
					}
				}
				if (player.HasItem(ModContent.ItemType<Items.Weapons.PlasmaRifle>()))
				{
					chat.Add("I feel like I've seen that energy gun you have, but I'm not sure where.", 0.5);
				}
				Mod calamity = ModLoader.GetMod("CalamityMod");
				if (calamity != null) //Calamity
				{
					int brimestoneWitch = NPC.FindFirstNPC(calamity.NPCType("WITCH")); //Brimstone Witch
					if (brimestoneWitch >= 0)
					{
						chat.Add("Calamitas is so powerful! I wish my magic were as good as hers!", 0.5);
					}
				}
			}
			
			return chat;
		}

		/*
			Future happiness notes:
				Loved Biome: Underground, Cavern, Underworld
				Liked Biome:
				Disliked Biome: 
				Loved NPCs:
				
				Liked NPCs:
				
				Disliked NPCs:
				
				Hated NPCs:
			
			
			Other NPCs' thoughts:
				Loved by:
				
				Liked by:
				
				Disliked by:
				
				Hated by:
				
		*/

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28"); //Shop
			if (Main.hardMode && !RijamsModWorld.hellTraderArrivable)
            {
				button2 = "Ask to move in";
			}
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
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
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)RijamsModMessageType.SetHellTraderArrivable);
					packet.Send();
				}
				npc.rarity = 0;
				npc.netUpdate = true;
				mod.Logger.Debug("RijamsMod: Hell Trader Arrivable.");
				return;
			}
		}

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
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.TimonsAxe>());
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
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.HammerOfRetribution>());
				nextSlot++;
			}
			if (NPC.downedGolemBoss)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Quietus>());
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
			shop.item[nextSlot].SetDefaults(ItemID.Hellforge);
			shop.item[nextSlot].shopCustomPrice = 7500;
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
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.HellTrader_Vanity_Hood>());
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.HellTrader_Vanity_Robes>());
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Armor.Vanity.HellTrader_Vanity_Trousers>());
				nextSlot++;
			}
		}

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
			Main.PlaySound(SoundID.Item, npc.position, 8);
			projType = ModContent.ProjectileType<Projectiles.SulfurSphere>();
			attackDelay = 2;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 6f;
			randomOffset = 0.15f;
		}
	}
}