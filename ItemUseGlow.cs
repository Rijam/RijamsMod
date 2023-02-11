using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace RijamsMod
{
	///Adapted from Qwerty's random content mod
	///https://github.com/qwerty3-14/QwertyMod/blob/main/Common/Playerlayers/ItemUseGlow.cs github source
	///Usage: In the item's SetDefaults(), Check for !Main.dedServ first, then add:
	///```
	///Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
	///```
	///Additionally,
	///Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = int
	///Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = int
	///Item.GetGlobalItem<ItemUseGlow>().angleAdd = float
	///Item.GetGlobalItem<ItemUseGlow>().blendAlpha = bool
	///Can be set as well.
	public class ItemUseGlow : GlobalItem
	{
		public Texture2D glowTexture = null;
		public int glowOffsetY = 0;
		public int glowOffsetX = 0;
		public float angleAdd = 0f;
		public bool blendAlpha = false;
		public bool drawOnGround = true;
		public bool drawOnPlayer = true;
		public Color drawColor = Color.White;
		public bool flameFlicker = false;

		public override bool InstancePerEntity => true;
		public override GlobalItem Clone(Item item, Item itemClone)
		{
			return base.Clone(item, itemClone);
		}
		public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (glowTexture != null && drawOnGround)
			{
				Texture2D texture = glowTexture;
				Vector2 pos = new
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f - glowOffsetX,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f - glowOffsetY
				);
				int numTimesToDraw = 1;
				ulong seed = 0;

				if (flameFlicker)
				{
					numTimesToDraw = 7;
					seed = Main.TileFrameSeed ^ (ulong)(((long)item.position.Y << 32) | (uint)item.position.X);
				}
				for (int i = 0; i < numTimesToDraw; i++)
				{
					if (flameFlicker)
					{
						pos += new Vector2(Utils.RandomInt(ref seed, -5, 6) * 0.05f, Utils.RandomInt(ref seed, -8, 6) * 0.15f);
					}
					spriteBatch.Draw
					(
						texture,
						pos,
						new Rectangle(0, 0, texture.Width, texture.Height),
						drawColor,
						rotation + angleAdd,
						texture.Size() * 0.5f,
						scale,
						SpriteEffects.None,
						0f
					);
				}
			}
		}
	}
	class ItemGlowLayer : PlayerDrawLayer
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return true;
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.JustDroppedAnItem)
			{
				return;
			}
			if (drawInfo.drawPlayer.heldProj >= 0 && drawInfo.shadow == 0f && !drawInfo.heldProjOverHand)
			{
				drawInfo.projectileDrawPosition = drawInfo.DrawDataCache.Count;
			}
			Item heldItem = drawInfo.heldItem;
			int itemID = heldItem.type;
			float adjustedItemScale = drawInfo.drawPlayer.GetAdjustedItemScale(heldItem);
			

			Main.instance.LoadItem(itemID);

			if (heldItem.TryGetGlobalItem(out ItemUseGlow result))
			{
				Texture2D glowTexture = result.glowTexture;
				int glowOffsetY = result.glowOffsetY;
				int glowOffsetX = result.glowOffsetX;
				bool blendAlpha = result.blendAlpha;
				float angleAdd = result.angleAdd;
				Color drawColor = result.drawColor;

				if (glowTexture != null && result.drawOnPlayer)
				{
					Texture2D itemTexture = TextureAssets.Item[itemID].Value;
					Vector2 position = new((int)(drawInfo.ItemLocation.X - Main.screenPosition.X - glowOffsetX), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y - glowOffsetY));
					Rectangle? sourceRect = new(0, 0, itemTexture.Width, itemTexture.Height);

					if (ItemID.Sets.IsFood[itemID])
					{
						sourceRect = itemTexture.Frame(1, 3, 0, 1);
					}
					drawInfo.itemColor = Lighting.GetColor((int)(drawInfo.Position.X + drawInfo.drawPlayer.width * 0.5) / 16, (int)((drawInfo.Position.Y + drawInfo.drawPlayer.height * 0.5) / 16.0));
					if (drawInfo.drawPlayer.shroomiteStealth && heldItem.CountsAsClass(DamageClass.Ranged))
					{
						float num2 = drawInfo.drawPlayer.stealth;
						if (num2 < 0.03)
						{
							num2 = 0.03f;
						}
						float num3 = (1f + num2 * 10f) / 11f;
						drawInfo.itemColor = new Color((byte)(drawInfo.itemColor.R * num2), (byte)(drawInfo.itemColor.G * num2), (byte)(drawInfo.itemColor.B * num3), (byte)(drawInfo.itemColor.A * num2));
					}
					if (drawInfo.drawPlayer.setVortex && heldItem.CountsAsClass(DamageClass.Ranged))
					{
						float num4 = drawInfo.drawPlayer.stealth;
						if (num4 < 0.03)
						{
							num4 = 0.03f;
						}
						//_ = (1f + num4 * 10f) / 11f;
						drawInfo.itemColor = drawInfo.itemColor.MultiplyRGBA(new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num4)));
					}
					bool flag = drawInfo.drawPlayer.itemAnimation > 0 && heldItem.useStyle != ItemUseStyleID.None;
					bool flag2 = heldItem.holdStyle != 0 && !drawInfo.drawPlayer.pulley;
					if (!drawInfo.drawPlayer.CanVisuallyHoldItem(heldItem))
					{
						flag2 = false;
					}
					if (drawInfo.shadow != 0f || drawInfo.drawPlayer.frozen || !(flag || flag2) || itemID <= 0 || drawInfo.drawPlayer.dead || heldItem.noUseGraphic || (drawInfo.drawPlayer.wet && heldItem.noWet) || (drawInfo.drawPlayer.happyFunTorchTime && drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].createTile == TileID.Torches && drawInfo.drawPlayer.itemAnimation == 0))
					{
						return;
					}
					//Special check for the Timon's Axe, Hammer of Retribution, Quietus. Only draw the glowmask if the player has enough mana.
					if (heldItem.ModItem != null && heldItem.ModItem is Items.MagicMeleeGlow && !drawInfo.drawPlayer.CheckMana(20, false))
					{
						return;
					}

					if (blendAlpha)
					{
						drawColor = new(drawColor.R, drawColor.G, drawColor.B, heldItem.alpha);
					}

					Vector2 origin = new((float)sourceRect.Value.Width * 0.5f - (float)sourceRect.Value.Width * 0.5f * (float)drawInfo.drawPlayer.direction, sourceRect.Value.Height);
					if (heldItem.useStyle == ItemUseStyleID.DrinkLiquid && drawInfo.drawPlayer.itemAnimation > 0)
					{
						Vector2 value2 = new(0.5f, 0.4f);
						origin = sourceRect.Value.Size() * value2;
					}
					if (drawInfo.drawPlayer.gravDir == -1f)
					{
						origin.Y = sourceRect.Value.Height - origin.Y;
					}
					//origin += vector;
					float itemRotation = drawInfo.drawPlayer.itemRotation + angleAdd;
					if (heldItem.useStyle == ItemUseStyleID.GolfPlay)
					{
						ref float x = ref position.X;
						float num6 = x;
						//_ = drawInfo.drawPlayer.direction;
						x = num6 - 0f;
						itemRotation -= (float)Math.PI / 2f * (float)drawInfo.drawPlayer.direction;
						origin.Y = 2f;
						origin.X += 2 * drawInfo.drawPlayer.direction;
					}
					ItemSlot.GetItemLight(ref drawInfo.itemColor, heldItem);
					DrawData drawData;

					int numTimesToDraw = 1;
					ulong seed = 0;

					if (result.flameFlicker)
					{
						numTimesToDraw = 7;
						seed = Main.TileFrameSeed ^ (ulong)(((long)drawInfo.drawPlayer.position.Y << 32) | (uint)drawInfo.drawPlayer.position.X);
					}

					for (int i = 0; i < numTimesToDraw; i++)
					{
						Vector2 flameRandom = Vector2.Zero;
						if (result.flameFlicker)
						{
							flameRandom = new Vector2(Utils.RandomInt(ref seed, -10, 11) * 0.15f, Utils.RandomInt(ref seed, -10, 6) * 0.35f);
						}

						if (heldItem.useStyle == ItemUseStyleID.Shoot)
						{
							if (Item.staff[itemID])
							{
								float rotation = drawInfo.drawPlayer.itemRotation + 0.785f * (float)drawInfo.drawPlayer.direction;
								int posX = 0;
								int posY = 0;
								Vector2 origin5 = new(0f, itemTexture.Height);
								if (drawInfo.drawPlayer.gravDir == -1f)
								{
									if (drawInfo.drawPlayer.direction == -1)
									{
										rotation += 1.57f;
										origin5 = new Vector2(itemTexture.Width, 0f);
										posX -= itemTexture.Width;
									}
									else
									{
										rotation -= 1.57f;
										origin5 = Vector2.Zero;
									}
								}
								else if (drawInfo.drawPlayer.direction == -1)
								{
									origin5 = new Vector2(itemTexture.Width, itemTexture.Height);
									posX -= itemTexture.Width;
								}
								drawData = new DrawData(glowTexture, new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X + origin5.X + (float)posX), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y + (float)posY)) + flameRandom, sourceRect, drawColor, rotation, origin5, adjustedItemScale, drawInfo.itemEffect, 0);
								drawInfo.DrawDataCache.Add(drawData);
								return;
							}
							int vector4X;
							Vector2 vector3 = new(itemTexture.Width / 2, itemTexture.Height / 2);
							Vector2 vector4 = Main.DrawPlayerItemPos(drawInfo.drawPlayer.gravDir, itemID);
							vector4X = (int)vector4.X;
							vector3.Y = vector4.Y;
							Vector2 origin6 = new(-vector4X, itemTexture.Height / 2);
							if (drawInfo.drawPlayer.direction == -1)
							{
								origin6 = new Vector2(itemTexture.Width + vector4X, itemTexture.Height / 2);
							}
							drawData = new DrawData(glowTexture, new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X + vector3.X), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y + vector3.Y)), sourceRect, drawColor, drawInfo.drawPlayer.itemRotation, origin6, adjustedItemScale, drawInfo.itemEffect, 0);
							drawInfo.DrawDataCache.Add(drawData);
							if (heldItem.color != default)
							{
								drawData = new DrawData(glowTexture, new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X + vector3.X), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y + vector3.Y)) + flameRandom, sourceRect, drawColor, drawInfo.drawPlayer.itemRotation, origin6, adjustedItemScale, drawInfo.itemEffect, 0);
								drawInfo.DrawDataCache.Add(drawData);
							}
							return;
						}
						if (drawInfo.drawPlayer.gravDir == -1f)
						{
							drawData = new DrawData(glowTexture, position + flameRandom, sourceRect, drawColor, itemRotation, origin, adjustedItemScale, drawInfo.itemEffect, 0);
							drawInfo.DrawDataCache.Add(drawData);
							if (heldItem.color != default)
							{
								drawData = new DrawData(glowTexture, position + flameRandom, sourceRect, drawColor, itemRotation, origin, adjustedItemScale, drawInfo.itemEffect, 0);
								drawInfo.DrawDataCache.Add(drawData);
							}
							return;
						}
						drawData = new DrawData(glowTexture, position + flameRandom, sourceRect, drawColor, itemRotation, origin, adjustedItemScale, drawInfo.itemEffect, 0);
						drawInfo.DrawDataCache.Add(drawData);
						if (heldItem.color != default)
						{
							drawData = new DrawData(glowTexture, position + flameRandom, sourceRect, drawColor, itemRotation, origin, adjustedItemScale, drawInfo.itemEffect, 0);
							drawInfo.DrawDataCache.Add(drawData);
						}
					}
				}
			}
		}
	}
}