using Microsoft.Xna.Framework;
using RijamsMod.Items.Weapons;
using RijamsMod.NPCs;
using RijamsMod.NPCs.TownNPCs;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod
{
	public class RijamsMod : Mod
	{
		public static RijamsMod Instance;
		internal static RijamsModConfigClient ConfigClient;
		internal static RijamsModConfigServer ConfigServer;
		internal static RijamsModNPCs RijamsModNPCs;
		internal static ItemUseGlow ItemUseGlow;
		internal static ArmorUseGlowHead ArmorUseGlowHead;
		internal static ArmorUseGlowBody ArmorUseGlowBody;
		internal static ArmorUseGlowLegs ArmorUseGlowLegs;
		internal static WeaponAttackFlash WeaponAttackFlash;

		public override void Load()
		{
			
		}
		
		public override void Unload()
		{
			ItemOriginDesc.itemList = null;
			Items.GlobalItems.isWhip = null;
			Items.GlobalItems.isJoustingLance = null;
			Items.GlobalItems.isLanternWeapon = null;
			Items.SupportMinionCanUseCheck.SupportMinionsDefenseBuffs = null;
			Items.SupportMinionCanUseCheck.SupportMinionsHealingBuffs = null;
			Tiles.GlobalTiles.isPiano = null;
			Projectiles.RijamsModProjectile.RocketsAffectedByRocketBoosterExtraUpdates = null;
			Instance = null;
			ConfigClient = null;
			ConfigServer = null;
			RijamsModNPCs = null;
			ItemUseGlow = null;
			ArmorUseGlowHead = null;
			ArmorUseGlowBody = null;
			ArmorUseGlowLegs = null;
			WeaponAttackFlash = null;
		}

		public override void PostSetupContent()
		{
			/*if (ModLoader.TryGetMod("Census", out Mod censusMod))
			{
				// Here I am using Chat Tags to make my condition even more interesting.
				// If you localize your mod, pass in a localized string instead of just English.
				//censusMod.Call("TownNPCCondition", NPCType("Example Person"), $"Have [i:{ItemType<Items.ExampleItem>()}] or [i:{ItemType<Items.Placeable.ExampleBlock>()}] in inventory and build a house out of [i:{ItemType<Items.Placeable.ExampleBlock>()}] and [i:{ItemType<Items.Placeable.ExampleWall>()}]");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.TownNPCs.InterstellarTraveler>(), $"Defeat EoW or BoW and have [i:RijamsMod/OddDevice] Odd Device in your inventory");
				// Additional lines for additional town npc that your mod adds
				// Simpler example:
				// censusMod.Call("TownNPCCondition", NPCType("Simple"), "Defeat Duke Fishron");
				//censusMod.Call("TownNPCCondition", Find<ModNPC>("Fisherman").Type, "Rescue the Angler and have at least 5 Town NPCs");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.TownNPCs.Harpy>(), "Rescue her in space");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.TownNPCs.HellTrader>(), "Found in Hell. Can move in in Hardmode");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.TownNPCs.SnuggetPet.SnuggetPet>(), "License sold after 60% Bestiary completion and Interstellar Traveler is present");
			}*/
			if (ModLoader.TryGetMod("PboneUtils", out Mod pboneUtils))
			{
				//Something must be wrong, I can't get it to work.
				byte rarity = 1;
				Func<bool> condition = () => NPC.downedBoss1;
				pboneUtils.Call("MysteriousTraderItem", ModLoader.GetMod("RijamsMod"), ModContent.ItemType<Items.Consumables.StrangeRoll>(), rarity, condition);
				//rarity = 0;
				//condition = () => true;
				//pboneUtils.Call("MysteriousTraderItem", ModLoader.GetMod("RijamsMod"), ModContent.ItemType<Items.Consumables.StrangeRoll>(), rarity, condition);
			}
			if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs))
			{
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "GoblinTinkerer", ModContent.ItemType<Items.Weapons.Summon.Minions.ShadowflameStaff>(), new List<Condition>() { (Condition)bossesAsNPCs.Call("GetCondition", "DownedGoblinWarlock") });
				bossesAsNPCs.Call("AddToShop", "WithDiv", "Pumpking", ModContent.ItemType<Items.Weapons.Melee.HorsemansJoustingLance>(), new List<Condition>() { }, 0.1f);
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "IceQueen", ModContent.ItemType<Items.Materials.FestivePlating>(), new List<Condition>() { Condition.DownedSantaNK1 });
				bossesAsNPCs.Call("AddToShop", "WithDiv", "IceQueen", ModContent.ItemType<Items.Accessories.Summoner.NaughtyList>(), new List<Condition>() { Condition.DownedSantaNK1 }, 0.1f);
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "IceQueen", ModContent.ItemType<Items.Weapons.Summon.Whips.FestiveWhip>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "BrainOfCthulhu", ModContent.ItemType<Items.Materials.CrawlerChelicera>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "QueenSlime", ModContent.ItemType<Items.Weapons.Summon.Cudgels.CrystalClusterCudgel>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "EmpressOfLight", ModContent.ItemType<Items.Weapons.Summon.Cudgels.RadiantLanternCudgel>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "Deerclops", ModContent.ItemType<Items.Pets.StarCallerStaff>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "Deerclops", ModContent.ItemType<Items.Weapons.Summon.Cudgels.SanityFlowerCudgel>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "CustomPrice", "EyeOfCthulhu", ModContent.ItemType<Items.Weapons.Ranged.Ammo.BloodyArrow>(), new List<Condition>() { (Condition)bossesAsNPCs.Call("GetCondition", "CrimsonOrHardmode") }, 40);
				bossesAsNPCs.Call("AddToShop", "WithDiv", "KingSlime", ModContent.ItemType<Items.Accessories.Misc.MorphasRing>(), new List<Condition>() { }, 0.17f);
			}
			if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC))
			{
				fishermanNPC.Call("AddToShop", "DefaultPrice", "Fish", ModContent.ItemType<Items.Fishing.HornetTail>(), new List<Condition>() { });
				fishermanNPC.Call("AddToShop", "DefaultPrice", "Fish", ModContent.ItemType<Items.Fishing.FungiEel>(), new List<Condition>() { });
				fishermanNPC.Call("AddToShop", "CustomPrice", "Bait", ModContent.ItemType<Items.Fishing.WildBait>(), new List<Condition>() { Condition.AnglerQuestsFinishedOver(3) }, 5000);
			}

			if (ModLoader.TryGetMod("DialogueTweak", out Mod dialogueTweak))
			{
				Func<Rectangle> frame = () => new(0, 0, 44, 44);

				dialogueTweak.Call("ReplaceExtraButtonIcon", 
					ModContent.NPCType<InterstellarTraveler>(),
					"RijamsMod/Items/Quest/Icon_QuestAvailable",
					() => NPCHelper.NumberOfQuestsCompleted() < NPCHelper.NUMBEROFQUESTS && !InterstellarTraveler.showingQuestChecklistButton,
					frame);

				dialogueTweak.Call("ReplaceExtraButtonIcon",
					ModContent.NPCType<InterstellarTraveler>(),
					"RijamsMod/Items/Quest/Icon_QuestComplete",
					() => NPCHelper.NumberOfQuestsCompleted() >= NPCHelper.NUMBEROFQUESTS && !InterstellarTraveler.showingQuestChecklistButton,
					frame);

				dialogueTweak.Call("ReplaceExtraButtonIcon",
					ModContent.NPCType<InterstellarTraveler>(),
					"RijamsMod/Items/Quest/Icon_QuestChecklistNone",
					() => NPCHelper.NumberOfQuestsCompleted() == 0 && InterstellarTraveler.showingQuestChecklistButton,
					frame);

				dialogueTweak.Call("ReplaceExtraButtonIcon",
					ModContent.NPCType<InterstellarTraveler>(),
					"RijamsMod/Items/Quest/Icon_QuestChecklistPartial",
					() => NPCHelper.NumberOfQuestsCompleted() > 0 && NPCHelper.NumberOfQuestsCompleted() < NPCHelper.NUMBEROFQUESTS && InterstellarTraveler.showingQuestChecklistButton,
					frame);

				dialogueTweak.Call("ReplaceExtraButtonIcon",
					ModContent.NPCType<InterstellarTraveler>(),
					"RijamsMod/Items/Quest/Icon_QuestChecklistAll",
					() => NPCHelper.NumberOfQuestsCompleted() >= NPCHelper.NUMBEROFQUESTS && InterstellarTraveler.showingQuestChecklistButton,
					frame);
			}
		}

		//Adapted from absoluteAquarian's GraphicsLib
		public override object Call(params object[] args)
		{
			if (args is null)
				throw new ArgumentNullException(nameof(args));

			if (args[0] is not string function)
				throw new ArgumentException("Expected a function name for the first argument");

			void CheckArgsLength(int expected, params string[] argNames)
			{
				if (args.Length != expected)
					throw new ArgumentOutOfRangeException($"Expected {expected} arguments for Mod.Call(\"{function}\", {string.Join(",", argNames)}), got {args.Length} arguments instead");
			}

			switch (function)
			{
				case "VanillaVanityToArmor":
					return ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;
				case "TownNPCsCrossModSupport":
					return ModContent.GetInstance<RijamsModConfigServer>().TownNPCsCrossModSupport;
				case "LoadDebugItems":
					return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
				case "CatchNPCs":
					return ModContent.GetInstance<RijamsModConfigServer>().CatchNPCs;
				case "savedHarpy":
					return RijamsModWorld.savedHarpy;
				case "intTravArrived":
					return RijamsModWorld.intTravArrived;
				case "hellTraderArrivable":
					return RijamsModWorld.hellTraderArrivable;
				case "boughtSnuggetPet":
					return RijamsModWorld.boughtSnuggetPet;
				case "intTravQuest":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
					return args[1].ToString() switch
					{
						"OddDevice" => RijamsModWorld.intTravQuestOddDevice,
						"BlankDisplay" => RijamsModWorld.intTravQuestBlankDisplay,
						"TPCore" => RijamsModWorld.intTravQuestTPCore,
						"BreadAndJelly" => RijamsModWorld.intTravQuestBreadAndJelly,
						"MagicOxygenizer" => RijamsModWorld.intTravQuestMagicOxygenizer,
						"PrimeThruster" => RijamsModWorld.intTravQuestPrimeThruster,
						_ => throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Rijam's Mod"),
					};
				case "GetNearbyResidentNPCs":
					CheckArgsLength(3, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString() });
					return NPCs.NPCHelper.GetNearbyResidentNPCs((NPC)args[1], (int)args[2], out List<int> _, out List<int> _, out List<int> _, out List<int> _);
				case "GetStatusShop1":
					return NPCs.NPCHelper.StatusShop1();
				case "GetStatusShop2":
					return NPCs.NPCHelper.StatusShop2();
				case "AddItemToIsJoustingLance":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
					Items.GlobalItems.isJoustingLance.Add((int)args[1]);
					return Items.GlobalItems.isJoustingLance.Contains((int)args[1]);
				case "AddItemToIsLanternWeapon":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
					Items.GlobalItems.isLanternWeapon.Add((int)args[1]);
					return Items.GlobalItems.isLanternWeapon.Contains((int)args[1]);
				case "AddItemToIsWhip":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
					Items.GlobalItems.isWhip.Add((int)args[1]);
					return Items.GlobalItems.isWhip.Contains((int)args[1]);
				case "AddTileToPianos":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
					Tiles.GlobalTiles.isPiano.Add((int)args[1]);
					return Tiles.GlobalTiles.isPiano.Contains((int)args[1]);
				case "AddProjectileToRocketBoosterExtraUpdates":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
					Projectiles.RijamsModProjectile.RocketsAffectedByRocketBoosterExtraUpdates.Add((int)args[1]);
					return Projectiles.RijamsModProjectile.RocketsAffectedByRocketBoosterExtraUpdates.Contains((int)args[1]);
				case "AddProjectileToRocketBoosterExtraUpdatesBlackList":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
					Projectiles.RijamsModProjectile.RocketBoosterExtraUpdatesBlackList.Add((int)args[1]);
					return Projectiles.RijamsModProjectile.RocketBoosterExtraUpdatesBlackList.Contains((int)args[1]);
				default:
					throw new ArgumentException($"Function \"{function}\" is not defined by Rijam's Mod");
			}
		}

		// Adapted from Thorium Mod
		/// <summary>
		/// Attempts to play a sound across the network. Only supports Volume and Pitch modifiers.
		/// </summary>
		/// <param name="soundStyle"> The SoundStyle of the sound. Can include Volume and Pitch modifiers. </param>
		/// <param name="position"> The position of the sound. </param>
		/// <param name="player"> The player who is creating the sound. </param>
		/// <returns>True if multiplayer, false if single player.</returns>
		public bool PlayNetworkSound(SoundStyle soundStyle, Vector2 position, Player player)
		{
			PlaySound(soundStyle, player);

			if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
			{
				// Create a packet to send.
				ModPacket packet = GetPacket();
				packet.Write((byte)RijamsModMessageType.PlayNetworkSound); // Message type
				packet.Write(soundStyle.SoundPath); // Sound path
				packet.Write(soundStyle.Volume); // Volume
				packet.Write(soundStyle.Pitch); // Pitch
				packet.WriteVector2(position); // Position
				packet.Write((byte)player.whoAmI); // Who created the sound
				packet.Send(-1, player.whoAmI);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Attempts to play a sound across the network. Only supports Volume and Pitch modifiers.
		/// </summary>
		/// <param name="soundPath"> The path to the sound to be played. </param>
		/// <param name="volume"> The volume of the sound. </param>
		/// <param name="pitch"> The pitch of the sound. 0 is normal pitch. </param>
		/// <param name="position"> The position of the sound. </param>
		/// <param name="player"> The player who is creating the sound. </param>
		/// <returns>True if multiplayer, false if single player.</returns>
		public bool PlayNetworkSound(string soundPath, float volume, float pitch, Vector2 position, Player player)
		{
			PlaySound(new SoundStyle(soundPath) with { Volume = volume, Pitch = pitch }, player);

			if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
			{
				// Create a packet to send.
				ModPacket packet = GetPacket();
				packet.Write((byte)RijamsModMessageType.PlayNetworkSound); // Message type
				packet.Write(soundPath); // Sound path
				packet.Write(volume); // Volume
				packet.Write(pitch); // Pitch
				packet.WriteVector2(position); // Position
				packet.Write((byte)player.whoAmI); // Who created the sound
				packet.Send(-1, player.whoAmI);

				return true;
			}
			return false;
		}

		/// <summary>
		/// Receives the packet and requests to play the sound.
		/// </summary>
		/// <param name="reader"></param>
		internal void PlayNetworkSoundReceive(BinaryReader reader)
		{
			string soundPath = reader.ReadString();
			float volume = reader.ReadSingle();
			float pitch = reader.ReadSingle();
			Vector2 position = reader.ReadVector2();
			int playerIndex = reader.ReadByte();

			Player player = Main.player[playerIndex];
			PlayNetworkSound(new SoundStyle(soundPath) with { Volume = volume, Pitch = pitch }, position, player);
		}
		/// <summary>
		/// Plays the sound at the player who created the sound's center.
		/// </summary>
		/// <param name="soundStyle"> The sound. </param>
		/// <param name="player"> The player who created the sound. </param>
		internal static void PlaySound(SoundStyle soundStyle, Player player)
		{
			SoundEngine.PlaySound(soundStyle, player.Center);
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			RijamsModMessageType msgType = (RijamsModMessageType)reader.ReadByte();
			switch (msgType)
			{
				case RijamsModMessageType.DummyPacket: 
					NetMessage.SendData(MessageID.WorldData);
					Logger.Debug("RijamsMod: Dummy Packet (Multiplayer packet).");
					break;
				case RijamsModMessageType.SetQuestOddDevice:
					RijamsModWorld.intTravQuestOddDevice = true;
					NetMessage.SendData(MessageID.WorldData);
					Logger.Debug("RijamsMod: Odd Device quest completed (Multiplayer packet).");
					break;
				case RijamsModMessageType.SetQuestBlankDisplay:
					RijamsModWorld.intTravQuestBlankDisplay = true;
					NetMessage.SendData(MessageID.WorldData);
					Logger.Debug("RijamsMod: Blank Display quest completed (Multiplayer packet).");
					break;
				case RijamsModMessageType.SetQuestTPCore:
					RijamsModWorld.intTravQuestTPCore = true;
					NetMessage.SendData(MessageID.WorldData);
					Logger.Debug("RijamsMod: Teleportation Core quest completed (Multiplayer packet).");
					break;
				case RijamsModMessageType.SetQuestBreadAndJelly:
					RijamsModWorld.intTravQuestBreadAndJelly = true;
					NetMessage.SendData(MessageID.WorldData);
					Logger.Debug("RijamsMod: Rye Jam quest completed (Multiplayer packet).");
					break;
				case RijamsModMessageType.SetQuestMagicOxygenizer:
					RijamsModWorld.intTravQuestMagicOxygenizer = true;
					NetMessage.SendData(MessageID.WorldData);
					Logger.Debug("RijamsMod: Magic Oxygenizer quest completed (Multiplayer packet).");
					break;
				case RijamsModMessageType.SetQuestPrimeThruster:
					RijamsModWorld.intTravQuestPrimeThruster = true;
					NetMessage.SendData(MessageID.WorldData);
					Logger.Debug("RijamsMod: Prime Thruster quest completed (Multiplayer packet).");
					break;
				case RijamsModMessageType.SetHellTraderArrivable:
					RijamsModWorld.hellTraderArrivable = true;
					NetMessage.SendData(MessageID.WorldData);
					Logger.Debug("RijamsMod: Hell Trader Arrivable (Multiplayer packet).");
					break;
				case RijamsModMessageType.SetSnuggetTownPetArrivable:
					RijamsModWorld.boughtSnuggetPet = true;
					NetMessage.SendData(MessageID.WorldData);
					Logger.Debug("RijamsMod: Snugget Town Pet Arrivable (Multiplayer packet).");
					break;
				case RijamsModMessageType.PlayNetworkSound:
					PlayNetworkSoundReceive(reader);
					break;
				default:
					Logger.WarnFormat("RijamsMod: Unknown Message type: {0}", msgType);
					break;
			}
		}
	}
	internal enum RijamsModMessageType : byte
	{
		DummyPacket, //I'm not sure why, but the first packet gets triggered when entering the world (in multiplayer)
		SetQuestOddDevice,
		SetQuestBlankDisplay,
		SetQuestTPCore,
		SetQuestBreadAndJelly,
		SetQuestMagicOxygenizer,
		SetQuestPrimeThruster,
		SetHellTraderArrivable,
		SetSnuggetTownPetArrivable,
		PlayNetworkSound
	}
}