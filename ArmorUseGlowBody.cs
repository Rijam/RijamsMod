using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using ReLogic.Content;

namespace RijamsMod
{
	///Adapted from Clicker Class Core/BodyGlowmaskPlayer.cs
	///Usage: In the item's SetStaticDefaults(), Check for !Main.dedServ first, then add:
	///```
	///ArmorGlowmaskBody.RegisterData(Item.bodySlot, Color.White);
	///```
	///The key value is the slot. Item.bodySlot
	///The second value can be any color
	public class ArmorUseGlowBody : ModPlayer
	{
		//slot, color
		private static Dictionary<int, Color> GlowListBody { get; set; }

		public static void RegisterData(int bodySlot, Color color)
		{
			if (!GlowListBody.ContainsKey(bodySlot))
			{
				GlowListBody.Add(bodySlot, color);
			}
		}

		public override void Load()
		{
			GlowListBody = new Dictionary<int, Color>();
		}

		public override void Unload()
		{
			GlowListBody.Clear();
		}
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
		{
			if (!GlowListBody.TryGetValue(drawInfo.drawPlayer.body, out Color color))
			{
				return;
			}
			drawInfo.bodyGlowColor = color * drawInfo.drawPlayer.stealth;
			drawInfo.armGlowColor = color * drawInfo.drawPlayer.stealth;
		}
	}
}