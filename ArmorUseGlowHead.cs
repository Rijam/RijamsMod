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
	///ArmorUseGlowHead.RegisterData(Item.headSlot, new string[] { Texture + "_Head_Glowmask", "R", "G", "B", "Special Effect" });
	///```
	///The key value is the slot. Item.headSlot
	///For the string[]:
	///		The texture of the glowmask
	///		R, G, and B must 0 to 255
	///		"flame" and "lerpOnOff" is the only special effect supported (anything else will just draw as normal).
	public class ArmorUseGlowHead : PlayerDrawLayer
	{
		//slot, string[texture path, r, g, b, special effect]
		private static Dictionary<int, string[]> GlowListHead { get; set; }

		public static void RegisterData(int headSlot, string[] values)
		{
			if (!GlowListHead.ContainsKey(headSlot))
			{
				GlowListHead.Add(headSlot, values);
			}
		}

		// Returning true in this property makes this layer appear on the minimap player head icon.
		public override bool IsHeadLayer => false;

		public override void Load()
		{
			GlowListHead = new Dictionary<int, string[]>();
		}

		public override void Unload()
		{
			GlowListHead.Clear();
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawPlayer.head == -1)
			{
				return false;
			}

			return true;
		}

		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Head);
		}

		private int fadeInOrOut = 0;
		private int lerpTimer = 0;
		private Color lerpColor = Color.White;
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!GlowListHead.TryGetValue(drawPlayer.head, out string[] values))
			{
				return;
			}
			Asset<Texture2D> glowmask = ModContent.Request<Texture2D>(values[0]);

			int numTimesToDraw = 1;

			ulong seed = 0;

			// "flame" will draw the glowmask 5 times each with a slightly different position giving it a flickering sort of look.
			// "lerpOnOff" will fade the glowmask in and out.

			if (values[4] == "flame")
			{
				numTimesToDraw = 5;
				seed = Main.TileFrameSeed ^ (ulong)(((long)drawPlayer.position.Y << 32) | (uint)drawPlayer.position.X);
			}

			for (int i = 0; i < numTimesToDraw; i++)
			{
				Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.bodyFrame.Width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition;
				Vector2 headVect = drawInfo.headVect;

				Color color = drawPlayer.GetImmuneAlphaPure(new Color(int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3])), drawInfo.shadow);

				if (values[4] == "flame")
				{
					float random1 = Utils.RandomInt(ref seed, -5, 6) * 0.05f;
					float random2 = Utils.RandomInt(ref seed, -5, 1) * 0.15f;
					drawPos += new Vector2(random1, random2);
				}
				if (values[4] == "lerpOnOff")
				{
					if (fadeInOrOut == 0)
					{
						lerpColor = Color.Lerp(new Color(int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), 255), new Color(0, 0, 0, 0), lerpTimer / 600f);
					}
					if (fadeInOrOut == 1)
					{
						lerpColor = Color.Lerp(new Color(0, 0, 0, 0), new Color(int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), 255), lerpTimer / 600f);
					}
					color = drawPlayer.GetImmuneAlphaPure(lerpColor, drawInfo.shadow);
					lerpTimer++;
				}

				DrawData drawData = new(
					glowmask.Value, // The texture to render.
					drawPos.Floor() + headVect, // Position to render at.
					new Rectangle(drawPlayer.bodyFrame.X, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), // Source rectangle.
					color * drawPlayer.stealth, // Color.
					drawPlayer.headRotation, // Rotation.
					headVect, // Origin. Uses the texture's center.
					1f, // Scale.
					drawInfo.playerEffect, // SpriteEffects.
					0) // 'Layer'. This is always 0 in Terraria.
					{
						shader = drawInfo.cHead //Shader applied aka dyes
					};

				// Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
				drawInfo.DrawDataCache.Add(drawData);

				if (values[4] == "lerpOnOff")
				{
					if (lerpTimer > 600)
					{
						fadeInOrOut++;
						lerpTimer = 0;
						if (fadeInOrOut > 1)
						{
							fadeInOrOut = 0;
						}
					}
				}
			}
		}
	}
}