using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ReLogic.Content;

namespace RijamsMod
{
	///	<summary>
	/// <br>Adapted from Clicker Class DrawLayers/HeadLayer.cs</br>
	/// <br>Usage: In the item's SetStaticDefaults(), Check for !Main.dedServ first, then add:</br>
	/// <br><code>ArmorUseGlowHead.RegisterData(Item.legSlot, new ArmorHeadLegsOptions(Texture + "_Legs_Glowmask", Color.White, GlowMaskEffects.LerpOnOff));</code></br>
	/// <br>The key value is the slot. Item.legSlot</br>
	/// <br>ArmorHeadLegsOptions Texture is the texture of the glowmask</br>
	/// <br>ArmorHeadLegsOptions Color is the draw color. This can be omitted and it will draw White.</br>
	/// <br>ArmorHeadLegsOptions Effects is the special effect. This can be omitted and defaults to None.</br>
	/// </summary>
	public class ArmorUseGlowLegs : PlayerDrawLayer
	{
		// slot, options
		private static Dictionary<int, ArmorHeadLegsOptions> GlowListLegs { get; set; }

		/// <summary>
		/// Register the leg piece to have a glow mask.
		/// </summary>
		/// <param name="legSlot">The key value is the slot. Item.legSlot</param>
		/// <param name="values"><br>ArmorHeadLegsOptions Texture is the texture of the glowmask</br>
		/// <br>ArmorHeadLegsOptions Color is the draw color. This can be omitted and it will draw White.</br>
		/// <br>ArmorHeadLegsOptions Effects is the special effect. This can be omitted and defaults to None.</br></param>
		public static void RegisterData(int legSlot, ArmorHeadLegsOptions values)
		{
			if (!GlowListLegs.ContainsKey(legSlot))
			{
				GlowListLegs.Add(legSlot, values);
			}
		}

		public override void Load()
		{
			GlowListLegs = new Dictionary<int, ArmorHeadLegsOptions>();
		}

		public override void Unload()
		{
			GlowListLegs.Clear();
			GlowListLegs = null;
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

		private int fadeInOrOut = 0;
		private int lerpTimer = 0;
		private Color lerpColor = Color.White;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!GlowListLegs.TryGetValue(drawPlayer.legs, out ArmorHeadLegsOptions values))
			{
				return;
			}
			Asset<Texture2D> glowmask = ModContent.Request<Texture2D>(values.Texture);

			int numTimesToDraw = 1;

			ulong seed = 0;

			if (values.Effects == GlowMaskEffects.Flame)
			{
				numTimesToDraw = 5;
				seed = Main.TileFrameSeed ^ (ulong)(((long)drawPlayer.position.Y << 32) | (uint)drawPlayer.position.X);
			}

			for (int i = 0; i < numTimesToDraw; i++)
			{
				Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.legFrame.Width / 2, drawPlayer.height - drawPlayer.legFrame.Height + 4f) + drawPlayer.legPosition;
				Vector2 legsOffset = drawInfo.legsOffset;

				if (values.Effects == GlowMaskEffects.Flame)
				{
					float random1 = Utils.RandomInt(ref seed, -5, 6) * 0.05f;
					float random2 = Utils.RandomInt(ref seed, -5, 1) * 0.15f;
					drawPos += new Vector2(random1, random2);
				}

				Color color = drawPlayer.GetImmuneAlphaPure(values.Color, drawInfo.shadow);

				if (values.Effects == GlowMaskEffects.LerpOnOff)
				{
					if (fadeInOrOut == 0)
					{
						lerpColor = Color.Lerp(new Color(values.Color.R, values.Color.G, values.Color.B, 255), new Color(0, 0, 0, 0), lerpTimer / 600f);
					}
					if (fadeInOrOut == 1)
					{
						lerpColor = Color.Lerp(new Color(0, 0, 0, 0), new Color(values.Color.R, values.Color.G, values.Color.B, 255), lerpTimer / 600f);
					}
					color = drawPlayer.GetImmuneAlphaPure(lerpColor, drawInfo.shadow);
					lerpTimer++;
				}

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

			if (values.Effects == GlowMaskEffects.LerpOnOff)
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