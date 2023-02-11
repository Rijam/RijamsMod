using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons
{
	// Probably some bugs in this that I haven't discovered.
	
	/// <summary>
	/// Usage: In the item's SetDefaults(), Check for !Main.dedServ first, then add:
	/// var flash = Item.GetGlobalItem<WeaponAttackFlash>().flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash").Value;
	/// flash.firstProperty = 1;
	/// </summary>
	public class WeaponAttackFlash : GlobalItem
	{
		/// <summary> The texture to be used. The texture should face right. </summary>
		public Texture2D flashTexture = null;
		/// <summary> Positive numbers moves the origin down. </summary>
		public int posOffsetY = 0;
		/// <summary> Moves the origin away from the player. This value is for when facing left. </summary>
		public int posOffsetXLeft = 0;
		/// <summary> Moves the origin away from the player. This value is for when facing right. </summary>
		public int posOffsetXRight = 0;
		/// <summary> Draw alpha </summary>
		public int alpha = 255;
		/// <summary> Rotates the drawing </summary>
		public float angleAdd = 0f;
		/// <summary> Scales the drawing. The scale of the item is already taken into account. This will scale it beyond that. </summary>
		public float scale = 1f;
		/// <summary> The color of the drawing. No alpha is included, set that alpha property instead. </summary>
		public Color colorNoAlpha = new(255, 255, 255);
		/// <summary> How many frames are in the texture. Defaults to 1. </summary>
		public int frameCount = 1;
		/// <summary> How fast should the frames update (in ticks). </summary>
		public int frameRate = 0;
		/// <summary> If true, it will select a random frame every frame update. If false, it will cycle through from 0 to the frameCount. </summary>
		public bool useRandomFrame = false;
		/// <summary> The animation will always start on the first frame. False by default. </summary>
		public bool forceFirstFrame = false;
		/// <summary> The animation will loop. True by default. </summary>
		public bool animationLoop = true;
		/// <summary> A specific condition that the flash will play. Default is true (always). </summary>
		public Func<bool> flashCondition = () => true;
		/// <summary> If true, the flash will only draw if the item is being used. Aka, it won't draw when it's just being held. </summary>
		public bool onlyDrawInUse = true;

		public override bool InstancePerEntity => true;
		public override GlobalItem Clone(Item item, Item itemClone)
		{
			return base.Clone(item, itemClone);
		}
	}
	public class WeaponAttackFlashLayer : PlayerDrawLayer
	{
		public int frame = 0; // The frame of the texture.
		public int time = 0; // time for the Timer.

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return true;
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem); // Draw after the item that the player is holding.

		public int Timer
		{
			get => time;
			set => time = value;
		}

		public void CalcFrame(WeaponAttackFlash item)
		{
			Timer++;
			if (Timer >= item.frameRate)
			{
				if (item.useRandomFrame)
				{
					frame = Main.rand.Next(0, item.frameCount); // Select a random frame.
				}
				else
				{
					frame++; // Cycle through the animation.
				}

				Timer = 0;
				if (frame >= item.frameCount && item.animationLoop)
				{
					frame = 0;
				}
				// If animationLoop is false, the frame will grow continuously. Frames above the max frames will just be invisible.
				// The texture needs padding at bottom for it to be invisible. If there is no padding, it'll stretch the last row of pixels to fill the size of the frame.
			}
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.JustDroppedAnItem)
			{
				return;
			}
			Item heldItem = drawInfo.heldItem;
			int itemID = heldItem.type;
			float adjustedItemScale = drawInfo.drawPlayer.GetAdjustedItemScale(heldItem);

			Main.instance.LoadItem(itemID);

			if (heldItem.TryGetGlobalItem(out WeaponAttackFlash result))
			{
				Player drawPlayer = drawInfo.drawPlayer;
				Texture2D flashTexture = result.flashTexture;
				int posOffsetXLeft = result.posOffsetXLeft;
				int posOffsetXRight = result.posOffsetXRight;
				int posOffsetY = result.posOffsetY;
				int alpha = result.alpha;
				Color colorNoAlpha = result.colorNoAlpha;
				float angleAdd = result.angleAdd;
				float scale = result.scale;
				int frameCount = result.frameCount;

				if (flashTexture != null && result.flashCondition.Invoke()) // If a flash texture for the weapon exists and the flashCondition is true
				{
					bool flag = drawPlayer.itemAnimation > 0 && heldItem.useStyle != ItemUseStyleID.None;
					bool flag2 = heldItem.holdStyle != 0 && !drawPlayer.pulley;
					if (drawInfo.shadow != 0f || drawPlayer.frozen || !(flag || flag2) || itemID <= 0 || drawPlayer.dead || heldItem.noUseGraphic || (drawPlayer.wet && heldItem.noWet) || (drawPlayer.happyFunTorchTime && drawPlayer.inventory[drawPlayer.selectedItem].createTile == TileID.Torches && drawPlayer.itemAnimation == 0))
					{
						return;
					}
					
					// Stop the flash from drawing if the item isn't being used (but is being held)
					if (result.onlyDrawInUse && !drawPlayer.ItemAnimationActive)
					{
						return;
					}

					// Make it the first frame when the weapon has just started to be used
					if (result.forceFirstFrame && drawPlayer.ItemAnimationJustStarted)
					{
						frame = 0;
						Timer = 0;
					}

					Texture2D itemTexture = TextureAssets.Item[itemID].Value;

					Vector2 position = new((int)(drawInfo.ItemLocation.X - Main.screenPosition.X), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y));

					CalcFrame(result); // Change the frame.
					if (frameCount <= 0) // Divide by zero check
					{
						frameCount = 1;
					}
					Rectangle sourceRect = flashTexture.Frame(1, frameCount, 0, frame, 0, 0);

					if ((drawPlayer.shroomiteStealth || drawPlayer.setVortex) && heldItem.CountsAsClass(DamageClass.Ranged)) // Decrease the alpha if stealthy
					{
						float alphaMulti = drawPlayer.stealth;
						if (alphaMulti < 0.03)
						{
							alphaMulti = 0.03f;
						}
						alpha = (int)(alpha * alphaMulti);
						colorNoAlpha *= alphaMulti;
					}

					Vector2 halfTextureSize = itemTexture.Size() / 2f;
					Vector2 itemPos = Main.DrawPlayerItemPos(drawPlayer.gravDir, itemID);
					halfTextureSize.Y = itemPos.Y;
					Vector2 origin = new(-itemTexture.Width + (int)itemPos.X - posOffsetXRight, itemTexture.Height / 2 - posOffsetY); // facing right

					if (drawPlayer.direction == -1) // If facing left
					{
						origin = new Vector2(itemTexture.Width + (int)itemPos.X + posOffsetXLeft, itemTexture.Height / 2 - posOffsetY);
					}

					if (drawPlayer.gravDir == -1f) // If up-side-down
					{
						origin.Y = sourceRect.Height - origin.Y;
					}

					float itemRotation = drawPlayer.itemRotation + angleAdd;

					DrawData drawData = new(flashTexture, position + halfTextureSize, sourceRect, new(colorNoAlpha.R, colorNoAlpha.G, colorNoAlpha.B, alpha), itemRotation, origin, adjustedItemScale * scale, drawInfo.itemEffect, 0);
					drawInfo.DrawDataCache.Add(drawData);
				}
			}
		}
	}
}