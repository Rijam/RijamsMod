using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RijamsMod.Items.Consumables;
using RijamsMod.Items.Weapons;
using RijamsMod.NPCs;
using RijamsMod.NPCs.TownNPCs;
using RijamsMod.NPCs.TownNPCs.SnuggetPet;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod
{
	public class RijamsMod : Mod
	{
		public static RijamsMod Instance;
		/*
		internal static RijamsModConfigClient ConfigClient;
		internal static RijamsModConfigServer ConfigServer;
		internal static RijamsModNPCs RijamsModNPCs;
		internal static ItemUseGlow ItemUseGlow;
		internal static ArmorUseGlowHead ArmorUseGlowHead;
		internal static ArmorUseGlowBody ArmorUseGlowBody;
		internal static ArmorUseGlowLegs ArmorUseGlowLegs;
		internal static WeaponAttackFlash WeaponAttackFlash;
		*/

		public override void Load()
		{
			Instance = this;
			// I could do this, but I already have my own versions in the mod
			// if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutantMod))
			// {
			// 	fargosMutantMod.Call("AddCaughtNPC", "InterstellarTraveler", ModContent.NPCType<InterstellarTraveler>(), "'I'm pretty far from home, but this place is pretty cool.'", "RijamsMod");
			// }
			Asset<Effect> inlineShader = this.Assets.Request<Effect>("Effects/Inline");
			GameShaders.Misc["RijamsMod:Inline"] = new MiscShaderData(inlineShader, "InlinePass");
		}
		
		public override void Unload()
		{
			ItemOriginDesc.itemList = null;
			// Items.GlobalItems.isWhip = null;
			// Items.GlobalItems.isJoustingLance = null;
			// Items.GlobalItems.isLanternWeapon = null;
			// Items.GlobalItems.isCombatFlareGun = null;
			Items.SupportMinionCanUseCheck.SupportMinionsDefenseBuffs = null;
			Items.SupportMinionCanUseCheck.SupportMinionsHealingBuffs = null;
			// Tiles.GlobalTiles.isPiano = null;
			// Projectiles.RijamsModProjectile.RocketsAffectedByRocketBoosterExtraUpdates = null;
			Instance = null;
			/*
			ConfigClient = null;
			ConfigServer = null;
			RijamsModNPCs = null;
			ItemUseGlow = null;
			ArmorUseGlowHead = null;
			ArmorUseGlowBody = null;
			ArmorUseGlowLegs = null;
			WeaponAttackFlash = null;
			*/
		}

		public override void PostSetupContent()
		{
			if (ModLoader.TryGetMod("PboneUtils", out Mod pboneUtils))
			{
				//Something must be wrong, I can't get it to work.
				byte rarity = 1;
				Func<bool> condition = () => NPC.downedBoss1;
				pboneUtils.Call("MysteriousTraderItem", this, ModContent.ItemType<StrangeRoll>(), rarity, condition);
				//rarity = 0;
				//condition = () => true;
				//pboneUtils.Call("MysteriousTraderItem", ModLoader.GetMod("RijamsMod"), ModContent.ItemType<Items.Consumables.StrangeRoll>(), rarity, condition);
			}
			if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs))
			{
				bossesAsNPCs.Call("AddToShop", "WithDiv", "KingSlime", ModContent.ItemType<Items.Accessories.Misc.MorphasRing>(), new List<Condition>() { }, 0.17f);
				bossesAsNPCs.Call("AddToShop", "CustomPrice", "EyeOfCthulhu", ModContent.ItemType<Items.Weapons.Ranged.Ammo.BloodyArrow>(), new List<Condition>() { Condition.CrimsonWorld, Condition.Hardmode, Condition.DownedEowOrBoc }, 40);
				bossesAsNPCs.Call("AddToShop", "CustomPrice", "EyeOfCthulhu", ModContent.ItemType<Items.Weapons.Ranged.Ammo.BloodyArrow>(), new List<Condition>() { (Condition)bossesAsNPCs.Call("GetCondition", "CrimsonOrHardmode"), Condition.NotDownedEowOrBoc }, 40 * 5);
				bossesAsNPCs.Call("AddToShop", "CustomPrice", "EyeOfCthulhu", ModContent.ItemType<Items.Weapons.Ranged.Ammo.BloodyArrow>(), new List<Condition>() { Condition.CrimsonWorld, Condition.PreHardmode, Condition.DownedEowOrBoc }, 40 * 2);
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "BrainOfCthulhu", ModContent.ItemType<Items.Materials.CrawlerChelicera>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "GoblinTinkerer", ModContent.ItemType<Items.Weapons.Summon.Minions.ShadowflameStaff>(), new List<Condition>() { (Condition)bossesAsNPCs.Call("GetCondition", "DownedGoblinWarlock") });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "Deerclops", ModContent.ItemType<Items.Pets.StarCallerStaff>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "Deerclops", ModContent.ItemType<Items.Weapons.Summon.Cudgels.SanityFlowerCudgel>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "Deerclops", ModContent.ItemType<Items.Weapons.Summon.Whips.TailoThreeCats>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "QueenSlime", ModContent.ItemType<Items.Weapons.Summon.Cudgels.CrystalClusterCudgel>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "IceQueen", ModContent.ItemType<Items.Weapons.Summon.Whips.FestiveWhip>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "EmpressOfLight", ModContent.ItemType<Items.Weapons.Summon.Cudgels.RadiantLanternCudgel>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "EmpressOfLight", ModContent.ItemType<Items.Weapons.Melee.JoustingLances.EtherealJoustingLance>(), new List<Condition>() { });
				bossesAsNPCs.Call("AddToShop", "WithDiv", "Pumpking", ModContent.ItemType<Items.Weapons.Melee.JoustingLances.HorsemansJoustingLance>(), new List<Condition>() { }, 0.1f);
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "IceQueen", ModContent.ItemType<Items.Materials.FestivePlating>(), new List<Condition>() { Condition.DownedSantaNK1 });
				bossesAsNPCs.Call("AddToShop", "WithDiv", "IceQueen", ModContent.ItemType<Items.Accessories.Summoner.NaughtyList>(), new List<Condition>() { Condition.DownedSantaNK1 }, 0.1f);
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "Dreadnautilus", ModContent.ItemType<Items.Weapons.Summon.Minions.BabyBloodEelStaff>(), new List<Condition>() { (Condition)bossesAsNPCs.Call("GetCondition", "DownedDreadnautilus") });
			}
			if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC))
			{
				fishermanNPC.Call("AddToShop", "DefaultPrice", "Fish", ModContent.ItemType<Items.Fishing.HornetTail>(), new List<Condition>() { });
				fishermanNPC.Call("AddToShop", "DefaultPrice", "Fish", ModContent.ItemType<Items.Fishing.FungiEel>(), new List<Condition>() { });
				fishermanNPC.Call("AddToShop", "CustomPrice", "Bait", ModContent.ItemType<Items.Fishing.WildBait>(), new List<Condition>() { Condition.AnglerQuestsFinishedOver(3) }, 5000);
				fishermanNPC.Call("AddToShop", "DefaultPrice", "Extra", ModContent.ItemType<Items.Accessories.Misc.CuriosityLure>(), new List<Condition>() { Condition.AnglerQuestsFinishedOver(3) });
				fishermanNPC.Call("AddToShop", "DefaultPrice", "Extra", ModContent.ItemType<Items.Accessories.Misc.TrapBobber>(), new List<Condition>() { Condition.AnglerQuestsFinishedOver(1) });
				fishermanNPC.Call("AddToShop", "DefaultPrice", "Extra", ModContent.ItemType<Items.Accessories.Misc.SpinnerBobber>(), new List<Condition>() { Condition.AnglerQuestsFinishedOver(1) });
			}
			
			/*
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
			*/
		}

		//Adapted from absoluteAquarian's GraphicsLib
		public override object Call(params object[] args)
		{
			ArgumentNullException.ThrowIfNull(args);

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
				case "JoustingLanceStaticInvincibility":
					return ModContent.GetInstance<RijamsModConfigServer>().JoustingLanceStaticInvincibility;
				case "savedHarpy":
					return RijamsModWorld.savedHarpy;
				case "intTravArrived":
					return RijamsModWorld.intTravArrived;
				case "hellTraderArrivable":
					return RijamsModWorld.hellTraderArrivable;
				case "boughtSnuggetPet":
					return RijamsModWorld.boughtSnuggetPet;
				case "intTravQuest":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
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
					CheckArgsLength(3, [args[0].ToString(), args[1].ToString(), args[2].ToString()]);
					return NPCs.NPCHelper.GetNearbyResidentNPCs((NPC)args[1], (int)args[2], out List<int> _, out List<int> _, out List<int> _, out List<int> _);
				case "GetStatusShop1":
					return NPCs.NPCHelper.StatusShop1();
				case "GetStatusShop2":
					return NPCs.NPCHelper.StatusShop2();
				case "AddItemToIsJoustingLance":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					int projectileType = ContentSamples.ItemsByType[(int)args[1]]?.shoot ?? ProjectileID.None;
					if (projectileType == ProjectileID.None)
					{
						Logger.WarnFormat("Call Warning: An item ({0} {1}) was added to the list of Jousting Lances but it has no associated projectile! The item needs to have Item.shoot defined.", (int)args[1], ContentSamples.ItemsByType[(int)args[1]].Name);
					}
					// Items.GlobalItems.isJoustingLance.Add((int)args[1], projectileType);
					// return Items.GlobalItems.isJoustingLance.ContainsKey((int)args[1]);
					Items.CustomItemIDSets.IsJoustingLance[(int)args[1]] = true;
					Projectiles.CustomProjectileIDSets.IsJoustingLanceProjectile[projectileType] = true;
					return Items.CustomItemIDSets.IsJoustingLance[(int)args[1]] == true;
				case "AddItemToIsLanternWeapon":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					// Items.GlobalItems.isLanternWeapon.Add((int)args[1]);
					// return Items.GlobalItems.isLanternWeapon.Contains((int)args[1]);
					Items.CustomItemIDSets.IsLanternWeapon[(int)args[1]] = true;
					return Items.CustomItemIDSets.IsLanternWeapon[(int)args[1]];
				case "AddItemToIsWhip":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					// Items.GlobalItems.isWhip.Add((int)args[1]);
					// return Items.GlobalItems.isWhip.Contains((int)args[1]);
					Items.CustomItemIDSets.IsWhip[(int)args[1]] = true;
					return Items.CustomItemIDSets.IsWhip[(int)args[1]];
				case "AddTileToPianos":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					// Tiles.GlobalTiles.isPiano.Add((int)args[1]);
					// return Tiles.GlobalTiles.isPiano.Contains((int)args[1]);
					Tiles.CustomTileIDSets.IsPiano[(int)args[1]] = true;
					return Tiles.CustomTileIDSets.IsPiano[(int)args[1]];
				case "AddProjectileToRocketBoosterExtraUpdates":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					// Projectiles.RijamsModProjectile.RocketsAffectedByRocketBoosterExtraUpdates.Add((int)args[1]);
					// return Projectiles.RijamsModProjectile.RocketsAffectedByRocketBoosterExtraUpdates.Contains((int)args[1]);
					Projectiles.CustomProjectileIDSets.RocketsAffectedByRocketBoosterExtraUpdates[(int)args[1]] = true;
					return Projectiles.CustomProjectileIDSets.RocketsAffectedByRocketBoosterExtraUpdates[(int)args[1]];
				case "AddProjectileToRocketBoosterExtraUpdatesBlackList":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					// Projectiles.RijamsModProjectile.RocketBoosterExtraUpdatesBlackList.Add((int)args[1]);
					// return Projectiles.RijamsModProjectile.RocketBoosterExtraUpdatesBlackList.Contains((int)args[1]);
					Projectiles.CustomProjectileIDSets.RocketBoosterExtraUpdatesBlackList[(int)args[1]] = true;
					return Projectiles.CustomProjectileIDSets.RocketBoosterExtraUpdatesBlackList[(int)args[1]];
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

		/// <summary>
		/// Use to increase (or decrease) the player's mana and sync it across the network.
		/// </summary>
		/// <param name="playerWhoAmI">The index of the player.</param>
		/// <param name="healAmount">The amount to increase (or decrease) the mana by.</param>
		/// <param name="showCombatText">If the combat text should be shown.</param>
		public void HealMana(int playerWhoAmI, int healAmount, bool showCombatText = true)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Player manaPlayer = Main.player[playerWhoAmI];
				manaPlayer.statMana += healAmount;
				if (manaPlayer.statMana > manaPlayer.statManaMax2)
				{
					manaPlayer.statMana = manaPlayer.statManaMax2;
				}
				if (showCombatText)
				{
					manaPlayer.ManaEffect(healAmount);
				}
			}
			else
			{
				ModPacket packet = GetPacket();
				packet.Write((byte)RijamsModMessageType.HealMana);
				packet.Write((byte)playerWhoAmI);
				packet.Write((short)healAmount);
				packet.Write((bool)showCombatText);
				packet.Send();
			}
		}

		/// <summary>
		/// Receives the packet and changes the player's mana.
		/// </summary>
		/// <param name="reader"></param>
		internal void HealManaReceive(BinaryReader reader)
		{
			byte playerIndex = reader.ReadByte();
			short healAmount = reader.ReadInt16();
			bool showCombatText = reader.ReadBoolean();

			if (healAmount > 0)
			{
				Player manaPlayer = Main.player[playerIndex];
				manaPlayer.statMana += healAmount;
				if (manaPlayer.statMana > manaPlayer.statManaMax2)
				{
					manaPlayer.statMana = manaPlayer.statManaMax2;
				}
				NetMessage.SendData(MessageID.PlayerMana, number: playerIndex);

				if (showCombatText)
				{
					manaPlayer.ManaEffect(healAmount);
				}

				if (Main.netMode == NetmodeID.Server)
				{
					HealMana(playerIndex, healAmount, showCombatText);
				}
			}
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			RijamsModMessageType msgType = (RijamsModMessageType)reader.ReadByte();
			switch (msgType)
			{
				case RijamsModMessageType.DummyPacket: 
					// NetMessage.SendData(MessageID.WorldData);
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
				case RijamsModMessageType.PlayNetworkSound:
					PlayNetworkSoundReceive(reader);
					break;
				case RijamsModMessageType.SnuggetUnlockOrExchange:
					SnuggetPetLicense.SnuggetUnlockOrExchangePet(ref RijamsModWorld.boughtSnuggetPet, ModContent.NPCType<SnuggetPet>(), "Mods.RijamsMod.UI.LicenseSnuggetUse");
					Logger.Debug("RijamsMod: Snugget Town Pet Unlock or Exchange (Multiplayer packet).");
					break;
				case RijamsModMessageType.HealMana:
					HealManaReceive(reader);
					break;
				default:
					Logger.WarnFormat("RijamsMod: Unknown Message type: {0}", msgType);
					break;
			}
		}
	}
	public enum RijamsModMessageType : byte
	{
		DummyPacket, //I'm not sure why, but the first packet gets triggered when entering the world (in multiplayer)
		SetQuestOddDevice,
		SetQuestBlankDisplay,
		SetQuestTPCore,
		SetQuestBreadAndJelly,
		SetQuestMagicOxygenizer,
		SetQuestPrimeThruster,
		SetHellTraderArrivable,
		PlayNetworkSound,
		SnuggetUnlockOrExchange,
		HealMana
	}
}