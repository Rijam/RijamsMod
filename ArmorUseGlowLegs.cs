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
	///Adapted from Clicker Class DrawLayers/HeadLayer.cs
	///Usage: In the item's SetStaticDefaults(), Check for !Main.dedServ first, then add:
	///```
	///ArmorUseGlowLegs.RegisterData(Item.legSlot, new string[] { Texture + "_Legs_Glowmask", "R", "G", "B", "Special Effect" });
	///```
	///The key value is the slot. Item.legSlot
	///For the string[]:
	///		The texture of the glowmask
	///		R, G, and B must 0 to 255
	///		"flame" is the only special effect supported (anything else will just draw as normal).
	public class ArmorUseGlowLegs : PlayerDrawLayer
	{
		//slot, string[texture path, r, g, b, special effect]
		private static Dictionary<int, string[]> GlowListLegs { get; set; }

		public static void RegisterData(int legSlot, string[] values)
		{
			if (!GlowListLegs.ContainsKey(legSlot))
			{
				GlowListLegs.Add(legSlot, values);
			}
		}

		public override void Load()
		{
			GlowListLegs = new Dictionary<int, string[]>();
		}

		public override void Unload()
		{
			GlowListLegs.Clear();
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawPlayer.legs == -1)
			{
				return false;
			}

			return true;
		}

		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Leggings);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!GlowListLegs.TryGetValue(drawPlayer.legs, out string[] values))
			{
				return;
			}
			Asset<Texture2D> glowmask = ModContent.Request<Texture2D>(values[0]);

			int numTimesToDraw = 1;

			ulong seed = 0;

			if (values[4] == "flame")
			{
				numTimesToDraw = 5;
				seed = Main.TileFrameSeed ^ (ulong)(((long)drawPlayer.position.Y << 32) | (uint)drawPlayer.position.X);
			}

			for (int i = 0; i < numTimesToDraw; i++)
			{
				Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.legFrame.Width / 2, drawPlayer.height - drawPlayer.legFrame.Height + 4f) + drawPlayer.legPosition;
				Vector2 legsOffset = drawInfo.legsOffset;

				if (values[4] == "flame")
				{
					float random1 = Utils.RandomInt(ref seed, -5, 6) * 0.05f;
					float random2 = Utils.RandomInt(ref seed, -5, 1) * 0.15f;
					drawPos += new Vector2(random1, random2);
				}

				Color color = drawPlayer.GetImmuneAlphaPure(new Color(int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3])), drawInfo.shadow);

				DrawData drawData = new(
					glowmask.Value, // The texture to render.
					drawPos.Floor() + legsOffset, // Position to render at.
					drawPlayer.legFrame, // Source rectangle.
					color * drawPlayer.stealth, // Color.
					drawPlayer.legRotation, // Rotation.
					legsOffset, // Origin. Uses the texture's center.
					1f, // Scale.
					drawInfo.playerEffect, // SpriteEffects.
					0) // 'Layer'. This is always 0 in Terraria.
				{
					shader = drawInfo.cLegs //Shader applied aka dyes
				};

				// Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}