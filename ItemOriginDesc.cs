using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RijamsMod
{
	//Inspired by Idglibrary

	/// <summary>
	/// Adds an additional line to the tooltip if the item is not in the player's inventory.
	/// Usage: In the item's SetStaticDefaults, add:
	/// ```
	///	ItemOriginDesc.itemList.Add(item.type, "Your string");
	///	```
	/// </summary>
	public class ItemOriginDesc : GlobalItem
	{
		public static Dictionary<int, string> itemList = new Dictionary<int, string>();

		public void ClearList() { itemList.Clear(); }

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (itemList.ContainsKey(item.type) && CheckIfNotInInventory(item) && CheckIfNotEquipped(item))
			{
				if (itemList.TryGetValue(item.type, out string description))
				{
					tooltips.Add(new TooltipLine(mod, "ItemOriginDesc", description));
				}
			}
		}
		public bool CheckIfNotInInventory(Item item)
		{
			Player player = Main.LocalPlayer;
			if (!player.HasItem(item.type))
			{
				return true;
			}
			return false;
		}
		public bool CheckIfNotEquipped(Item item)
        {
			Player player = Main.LocalPlayer;
			for (int i = 0; i <= 7 + player.extraAccessorySlots; i++)
			{
				if (player.armor[i].type == item.type)
				{
					return false;
				}
			}
			return true;
		}
	}
}