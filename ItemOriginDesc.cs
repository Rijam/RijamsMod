using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod
{
	// Inspired by Idglibrary

	/// <summary>
	/// Adds an additional line to the tooltip if the item is not in the player's inventory.
	/// Supports up to three additional lines.
	/// Usage: In the item's SetStaticDefaults, add:
	/// ```
	///	ItemOriginDesc.itemList.Add(Item.type, new string[] { "Your string", "Your string 2", null } );
	///	```
	/// </summary>
	public class ItemOriginDesc : GlobalItem
	{
		public static Dictionary<int, string[]> itemList = new();

		public static void ClearList() { itemList.Clear(); }

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (itemList.ContainsKey(item.type) && 
				!CheckIfInInventory(item.type) && !CheckIfEquipped(item.type) && !CheckIfEquippedVanity(item.type) &&
				!CheckIfEquippedMisc(item.type) && !CheckIfEquippedDye(item.type) && !CheckIfEquippedDyeMisc(item.type) &&
				!CheckIfInChest(item.type) && !CheckIfInShop(item.type) && !CheckIfInPortableStorage(item.type))
			{
				if (itemList.TryGetValue(item.type, out string[] description))
				{
					tooltips.Add(new TooltipLine(Mod, "ItemOriginDesc", description[0]));
					if (description[1] != null)
					{
						tooltips.Add(new TooltipLine(Mod, "ItemOriginDesc2", description[1]));
					}
					if (description[2] != null)
					{
						tooltips.Add(new TooltipLine(Mod, "ItemOriginDesc3", description[2]));
					}
				}
			}
		}
		/// <summary>
		/// Returns true if the item is in the player's main inventory. This includes the coin and ammo slots.
		/// It does not include the trash slot.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>bool</returns>
		public static bool CheckIfInInventory(int item)
		{
			Player player = Main.LocalPlayer;
			if (player.HasItem(item))
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Returns true if the item is equipped in the armor or accessory slots.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>bool</returns>
		public static bool CheckIfEquipped(int item)
        {
			Player player = Main.LocalPlayer;
			for (int i = 0; i <= 7 + player.GetAmountOfExtraAccessorySlotsToShow(); i++)
			{
				if (player.armor[i].type == item)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Returns true if the item is equipped in the social armor or social accessory slots
		/// </summary>
		/// <param name="item"></param>
		/// <returns>bool</returns>
		public static bool CheckIfEquippedVanity(int item)
		{
			Player player = Main.LocalPlayer;
			for (int i = 10; i <= 17 + player.GetAmountOfExtraAccessorySlotsToShow(); i++)
			{
				if (player.armor[i].type == item)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Returns true if the item is equipped in the extra equip slots. These are the:
		/// pet, light pet, minecart, mount, and grappling hook.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>bool</returns>
		public static bool CheckIfEquippedMisc(int item)
		{
			Player player = Main.LocalPlayer;
			for (int i = 0; i <= 4; i++)
			{
				if (player.miscEquips[i].type == item)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Returns true if the item is equipped in the armor or accessory dye slots.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>bool</returns>
		public static bool CheckIfEquippedDye(int item)
		{
			Player player = Main.LocalPlayer;
			for (int i = 0; i <= 7 + player.extraAccessorySlots; i++)
			{
				if (player.dye[i].type == item)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Returns true if the item is equipped in the extra equip dye slots.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>bool</returns>
		public static bool CheckIfEquippedDyeMisc(int item)
		{
			Player player = Main.LocalPlayer;
			for (int i = 0; i <= 4; i++)
			{
				if (player.miscDyes[i].type == item)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Returns true if the item is in the chest that the player is currently looking in.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>bool</returns>
		public static bool CheckIfInChest(int item)
		{
			Player player = Main.LocalPlayer;
			if (player.chest >= 0)
			{
				for (int i = 0; i <= 39; i++)
				{
					if (Main.chest[player.chest].item[i].type == item)
					{
						return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Returns true if the item is in the shop that the player is currently looking at.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>bool</returns>
		public static bool CheckIfInShop(int item)
		{
			if (Main.npcShop != 0)
			{
				for (int i = 0; i <= 39; i++)
				{
					if (Main.instance.shop[Main.npcShop].item[i].type == item)
					{
						return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Returns true if the item is in the portable storage that the player is currently looking in.
		/// Portable storage is: Piggy Bank, Safe, Defender's Forge, and Void Bag/Vault.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>bool</returns>
		public static bool CheckIfInPortableStorage(int item)
		{
			Player player = Main.LocalPlayer;
			// -2	bank	Piggy Bank
			// -3	bank2	Safe
			// -4	bank3	Defender's Forge
			// -5	bank4	Void Bag/Vault
			if (player.chest <= -2 && player.chest >= -5)
			{
				for (int i = 0; i <= 39; i++)
				{
					switch ((player.chest + 1) * -1)
					{
						case 1: // Piggy Bank
							if (player.bank.item[i].type == item)
							{
								return true;
							}
							continue;
						case 2: // Safe
							if (player.bank2.item[i].type == item)
							{
								return true;
							}
							continue;
						case 3: // Defender's Forge
							if (player.bank3.item[i].type == item)
							{
								return true;
							}
							continue;
						case 4: // Void Bag/Vault
							if (player.bank4.item[i].type == item)
							{
								return true;
							}
							continue;
						default:
							break;
					}
				}
			}
			return false;
		}
	}
}