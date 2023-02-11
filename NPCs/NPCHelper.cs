using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using RijamsMod.NPCs.TownNPCs;
using Terraria.Localization;

namespace RijamsMod.NPCs
{
	/// <summary>
	/// NPCHelper is a small class that "automates" many repeated things for the Town NPCs.
	/// </summary>
	public class NPCHelper
	{
		/// Mod name
		private static readonly string mod = ModContent.GetInstance<RijamsMod>().Name;

		/// <summary>
		/// Automatically gets the localized Loved text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string LoveText(string npc)
		{
			return "[c/b3f2b3:" + Language.GetTextValue("RandomWorldName_Noun.Love") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Love") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Liked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string LikeText(string npc)
		{
			return "[c/ddf2b3:" + Language.GetTextValue("Mods." + mod + ".UI.Like") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Like") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Disliked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string DislikeText(string npc)
		{
			return "[c/f2e0b3:" + Language.GetTextValue("Mods." + mod + ".UI.Dislike") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Dislike") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Hated text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string HateText(string npc)
		{
			return "[c/f2b5b3:" + Language.GetTextValue("RandomWorldName_Noun.Hate") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Hate");
		}

		/// <summary>
		/// Automatically gets the path to the localized Bestiary Description.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string BestiaryPath(string npc)
		{
			return "Mods." + mod + ".Bestiary.Description." + npc;
		}

		/// <summary>
		/// Automatically gets the base path to the localized dialog. Add `+ "Key"` to get the dialog.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string DialogPath(string npc)
		{
			return "Mods." + mod + ".NPCDialog." + npc + ".";
		}

		/// <summary>
		/// Gets if the player has unlocked the Otherworldly music. Doesn't actually check for the player, but the shop runs client side so it doesn't matter.
		/// </summary>
		/// <returns>bool</returns>
		public static bool UnlockOWMusic()
		{
			return Main.Configuration.Get("UnlockMusicSwap", false);
		}

		private static bool shop1;
		private static bool shop2;

		/// <summary>
		/// Sets the shop1 bool. Set it to the opposite of the SetShop2()
		/// </summary>
		public static void SetShop1(bool tOrF)
		{
			shop1 = tOrF;
		}

		/// <summary>
		/// Sets the shop2 bool. Set it to the opposite of the SetShop1()
		/// </summary>
		public static void SetShop2(bool tOrF)
		{
			shop2 = tOrF;
		}

		/// <summary>
		/// Gets if shop1 is open.
		/// </summary>
		/// <returns>bool</returns>
		public static bool StatusShop1()
		{
			return shop1;
		}

		/// <summary>
		/// Gets if shop2 is open.
		/// </summary>
		/// <returns>bool</returns>
		public static bool StatusShop2()
		{
			return shop2;
		}

		/// <summary>
		/// Returns true if the world has completed all of the Interstellar Traveler's quests.
		/// </summary>
		/// <returns>bool</returns>
		public static bool AllQuestsCompleted()
		{
			if (RijamsModWorld.intTravQuestOddDevice == true && RijamsModWorld.intTravQuestBlankDisplay == true && RijamsModWorld.intTravQuestTPCore == true
				&& RijamsModWorld.intTravQuestMagicOxygenizer == true && RijamsModWorld.intTravQuestPrimeThruster == true) //Rye Jam quest is not needed
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns true if Town NPC is far from their home (> 120 tiles).
		/// Adapted from vanilla
		/// </summary>
		/// <returns>True if the Town NPC is > 120 tiles from their home</returns>
		public static bool IsFarFromHome(NPC npc)
		{
			Vector2 value = new(npc.homeTileX, npc.homeTileY);
			Vector2 value2 = new(npc.Center.X / 16f, npc.Center.Y / 16f);
			if (Vector2.Distance(value, value2) > 120f)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// <br>Returns a list of all of the Town NPCs within 25 tiles.</br>
		/// <br>searchMode = 1: Everything that could be a Town NPC, including Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.</br>
		/// <br>searchMode = 2: Town NPCs and Town Pets. Excludes Old Man, Traveling Merchant, and Skeleton Merchant.</br>
		/// <br>searchMode = 3: Only real Town NPCs. Excludes Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.</br>
		/// <br>npcTypeListHouse is a list of the npc.type for all of the Town NPCs within 25 tiles.</br>
		/// <br>npcTypeListNearBy is a list of the npc.type for all of the Town NPCs within 50 tiles.</br>
		/// <br>npcTypeListVillage is a list of the npc.type for all of the Town NPCs within 120 tiles.</br>
		/// <br>npcTypeListAll is a list of the npc.type for all of the Town NPCs in the world.</br>
		/// <br>Use .Count if you want the total number of Town NPCs for the given list.</br>
		/// <br>Adapted from vanilla</br>
		/// </summary>
		/// <returns>List NPC of all the Town NPCs within 25 tiles.</returns>
		public static List<NPC> GetNearbyResidentNPCs(NPC npc, int searchMode, out List<int> npcTypeListHouse, out List<int> npcTypeListNearBy, out List<int> npcTypeListVillage, out List<int> npcTypeListAll)
		{
			List<NPC> list = new();
			npcTypeListHouse = new();
			npcTypeListNearBy = new();
			npcTypeListVillage = new();
			npcTypeListAll = new();
			Vector2 npc1Home = new(npc.homeTileX, npc.homeTileY);
			if (npc.homeless)
			{
				npc1Home = new Vector2(npc.Center.X / 16f, npc.Center.Y / 16f);
			}
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (i == npc.whoAmI)
				{
					continue;
				}
				NPC nPC = Main.npc[i];
				if (nPC.active && nPC.townNPC && NearbyResidentSearchMode(npc, nPC, searchMode))
				{
					Vector2 npc2Home = new(nPC.homeTileX, nPC.homeTileY);
					if (nPC.homeless)
					{
						npc2Home = nPC.Center / 16f;
					}
					float distance = Vector2.Distance(npc1Home, npc2Home);
					if (distance < 25f)
					{
						list.Add(nPC);
						npcTypeListHouse.Add(nPC.type);
					}
					if (distance < 50f)
					{
						npcTypeListNearBy.Add(nPC.type);
					}
					if (distance < 120f)
					{
						npcTypeListVillage.Add(nPC.type);
					}
					npcTypeListAll.Add(nPC.type);
				}
			}
			return list;
		}

		/// <summary>
		/// Used by NPCHelper.GetNearbyResidentNPCs().
		/// Returns true or false based on the search mode. See the method for info on the searchMode.
		/// </summary>
		/// <returns>True if the NPC fits the search mode requirements</returns>
		public static bool NearbyResidentSearchMode(NPC npc1, NPC npc2, int searchMode)
		{
			switch (searchMode)
			{
				case 1: // Everything that could be a Town NPC, including Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.
					if (NPCID.Sets.ActsLikeTownNPC[npc2.type] || npc2.housingCategory >= 1)
					{
						return true;
					}
					return false;
				case 2: // Town NPCs and Town Pets. Excludes Old Man, Traveling Merchant, and Skeleton Merchant.
					if (npc2.type != NPCID.OldMan || npc2.type != NPCID.TravellingMerchant || npc2.type != NPCID.SkeletonMerchant || !NPCID.Sets.ActsLikeTownNPC[npc2.type])
					{
						return true;
					}
					return false;
				case 3: // Only real Town NPCs. Excludes Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.
					if (npc2.type != NPCID.OldMan || npc2.type != NPCID.TravellingMerchant || npc2.type != NPCID.SkeletonMerchant || !NPCID.Sets.ActsLikeTownNPC[npc2.type] && !WorldGen.TownManager.CanNPCsLiveWithEachOther(npc1, npc2))
					{
						return true;
					}
					return false;
				default:
					return false;
			}
		}
		/// <summary>
		/// Searches the shop (or chest) to see if an item is in it. slotNumber is the slot the item is in.
		/// See ItemOriginDesc.CheckIfInShop() for a player version.
		/// </summary>
		/// <returns>True if the item is found</returns>
		public static bool FindItemInShop(int[] shop, int item, out int? slotNumber)
		{
			slotNumber = null;
			for (int i = 0; i < Chest.maxItems; i++)
			{
				if (shop[i] == item)
				{
					slotNumber = i;
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Searches the shop (or chest) to see if an item is in it. slotNumber is the slot the item is in.
		/// See ItemOriginDesc.CheckIfInShop() for a player version.
		/// </summary>
		/// <returns>True if the item is found</returns>
		public static bool FindItemInShop(Chest shop, int item, out int? slotNumber)
		{
			slotNumber = null;
			for (int i = 0; i < Chest.maxItems; i++)
			{
				if (shop.item[i].type == item)
				{
					slotNumber = i;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns true if all mechanical bosses have been defeated.
		/// </summary>
		public static bool DownedMechBossAll()
		{
			if (NPC.downedMechBoss1 && NPC.downedBoss2 && NPC.downedBoss3)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Safely returns the integer of the ModItem from the given mod.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <returns>int if found, 0 if not found.</returns>
		public static int SafelyGetCrossModItem(Mod mod, string itemString)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				return outItem.Type;
			}
			ModContent.GetInstance<RijamsMod>().Logger.WarnFormat("SafelyGetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			return 0;
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the customPrice.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		/// <param name="customPrice">The custom price of the item.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, Chest shop, ref int nextSlot, int customPrice)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				shop.item[nextSlot].SetDefaults(outItem.Type);
				shop.item[nextSlot].shopCustomPrice = customPrice;
				nextSlot++;
			}
			else
			{
				ModContent.GetInstance<RijamsMod>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the item's value / 5 / priceDiv.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		/// <param name="priceDiv">The price will be divided by this amount.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, Chest shop, ref int nextSlot, float priceDiv)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				shop.item[nextSlot].SetDefaults(outItem.Type);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value / 5 / priceDiv);
				nextSlot++;
			}
			else
			{
				ModContent.GetInstance<RijamsMod>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the item's (value / priceDiv) * priceMulti.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		/// <param name="priceDiv">The price will be divided by this amount.</param>
		/// <param name="priceMulti">The price will be multiplied by this amount after the priceDiv.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, Chest shop, ref int nextSlot, float priceDiv = 1f, float priceMulti = 1f)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				shop.item[nextSlot].SetDefaults(outItem.Type);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value / priceDiv * priceMulti);
				nextSlot++;
			}
			else
			{
				ModContent.GetInstance<RijamsMod>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}
	}
}