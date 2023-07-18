using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Tools
{
	public class MMProj : ModProjectile
	{
		readonly private Texture2D textureNone = ModContent.Request<Texture2D>("RijamsMod/Items/Tools/MatterManipulator").Value;
		readonly private Texture2D texturePick = ModContent.Request<Texture2D>("RijamsMod/Items/Tools/MMPickaxe").Value;
		readonly private Texture2D textureAxe = ModContent.Request<Texture2D>("RijamsMod/Items/Tools/MMAxe").Value;
		readonly private Texture2D textureHammer = ModContent.Request<Texture2D>("RijamsMod/Items/Tools/MMHammer").Value;
		readonly private Texture2D textureNoneGlow = ModContent.Request<Texture2D>("RijamsMod/Items/Tools/MatterManipulator_Glow").Value;
		readonly private Texture2D texturePickGlow = ModContent.Request<Texture2D>("RijamsMod/Items/Tools/MMPickaxe_Glow").Value;
		readonly private Texture2D textureAxeGlow = ModContent.Request<Texture2D>("RijamsMod/Items/Tools/MMAxe_Glow").Value;
		readonly private Texture2D textureHammerGlow = ModContent.Request<Texture2D>("RijamsMod/Items/Tools/MMHammer_Glow").Value;
		readonly private Texture2D lightning = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Tools/MMLightning").Value;
		public bool pick;
		public bool axe;
		public bool hammer;

		private static Color SetColorType(int mode)
		{
			switch (mode)
			{
				case 0:
					return new Color(127, 127, 127, 255);
				case 1:
					return Color.CornflowerBlue;
				case 2:
					return Color.LimeGreen;
				case 3:
					return Color.Yellow;
				default:
					break;
			}
			return Color.White;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Matter Manipulator");
		}
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = false;
			Projectile.extraUpdates = 1;
		}

		public override bool? CanCutTiles()
		{
			if (!pick && !axe && !hammer)
			{
				return false;
			}
			return base.CanCutTiles();
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			float num = (float)Math.PI / 2f;
			Vector2 playerRotation = player.RotatedRelativePoint(player.MountedCenter);
			int num12 = 2;
			float num23 = 0f;


			if (Projectile.soundDelay <= 0)
			{
				SoundEngine.PlaySound(SoundID.Item24 with { Pitch = 0.5f }, Projectile.position); //132, 24 @ 0.5f, 39 @ 2.0f, 
				Projectile.soundDelay = 40;
			}
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 60f)
			{
				Projectile.localAI[0] = 0f;
			}
			if (Vector2.Distance(playerRotation, Projectile.Center) >= 5f)
			{
				if (pick)
				{
					Vector3 lightColor1 = SetColorType(1).ToVector3();
					if (Vector2.Distance(playerRotation, Projectile.Center) >= 30f)
					{
						Vector2 projCenterMinusPlayerRot = Projectile.Center - playerRotation;
						projCenterMinusPlayerRot.Normalize();
						projCenterMinusPlayerRot *= Vector2.Distance(playerRotation, Projectile.Center) - 30f;
						DelegateMethods.v3_1 = lightColor1 * 0.3f;
						Utils.PlotTileLine(Projectile.Center - projCenterMinusPlayerRot, Projectile.Center, 8f, DelegateMethods.CastLightOpen);
					}
				}
				if (axe)
				{
					Vector3 lightColor2 = SetColorType(2).ToVector3();
					if (Vector2.Distance(playerRotation, Projectile.Center) >= 30f)
					{
						Vector2 projCenterMinusPlayerRot = Projectile.Center - playerRotation;
						projCenterMinusPlayerRot.Normalize();
						projCenterMinusPlayerRot *= Vector2.Distance(playerRotation, Projectile.Center) - 30f;
						DelegateMethods.v3_1 = lightColor2 * 0.3f;
						Utils.PlotTileLine(Projectile.Center - projCenterMinusPlayerRot, Projectile.Center, 8f, DelegateMethods.CastLightOpen);
					}
				}
				if (hammer)
				{
					Vector3 lightColor3 = SetColorType(3).ToVector3();
					if (Vector2.Distance(playerRotation, Projectile.Center) >= 30f)
					{
						Vector2 projCenterMinusPlayerRot = Projectile.Center - playerRotation;
						projCenterMinusPlayerRot.Normalize();
						projCenterMinusPlayerRot *= Vector2.Distance(playerRotation, Projectile.Center) - 30f;
						DelegateMethods.v3_1 = lightColor3 * 0.3f;
						Utils.PlotTileLine(Projectile.Center - projCenterMinusPlayerRot, Projectile.Center, 8f, DelegateMethods.CastLightOpen);
					}
				}
				if (!pick && !axe && !hammer)
				{
					Vector3 lightColor0 = SetColorType(0).ToVector3();
					if (Vector2.Distance(playerRotation, Projectile.Center) >= 30f)
					{
						Vector2 projCenterMinusPlayerRot = Projectile.Center - playerRotation;
						projCenterMinusPlayerRot.Normalize();
						projCenterMinusPlayerRot *= Vector2.Distance(playerRotation, Projectile.Center) - 30f;
						DelegateMethods.v3_1 = lightColor0 * 0.3f;
						Utils.PlotTileLine(Projectile.Center - projCenterMinusPlayerRot, Projectile.Center, 8f, DelegateMethods.CastLightOpen);
					}
				}
			}
			if (Main.myPlayer == Projectile.owner)
			{
				if (Projectile.localAI[1] > 0f)
				{
					Projectile.localAI[1] -= 1f;
				}
				if (!player.channel || player.noItems || player.CCed)
				{
					Projectile.Kill();
				}
				else if (Projectile.localAI[1] == 0f)
				{
					Vector2 value26 = playerRotation;
					Vector2 projPos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - value26;
					if (player.gravDir == -1f)
					{
						projPos.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - value26.Y;
					}
					if (Main.tile[Player.tileTargetX, Player.tileTargetY].HasTile)
					{
						projPos = new Vector2(Player.tileTargetX, Player.tileTargetY) * 16f + Vector2.One * 8f - value26;
						Projectile.localAI[1] = 2f;
					}
					projPos = Vector2.Lerp(projPos, Projectile.velocity, 0.7f);
					if (float.IsNaN(projPos.X) || float.IsNaN(projPos.Y))
					{
						projPos = -Vector2.UnitY;
					}
					float numIs30f = 30f;
					if (projPos.Length() < numIs30f)
					{
						projPos = Vector2.Normalize(projPos) * numIs30f;
					}
					int tileBoost = player.inventory[player.selectedItem].tileBoost;
					int num5 = -Player.tileRangeX - tileBoost + 1;
					int num6 = Player.tileRangeX + tileBoost - 1;
					int num7 = -Player.tileRangeY - tileBoost;
					int num8 = Player.tileRangeY + tileBoost - 1;
					int num9 = 12;
					bool flag7 = false;
					if (projPos.X < (float)(num5 * 16 - num9))
					{
						flag7 = true;
					}
					if (projPos.Y < (float)(num7 * 16 - num9))
					{
						flag7 = true;
					}
					if (projPos.X > (float)(num6 * 16 + num9))
					{
						flag7 = true;
					}
					if (projPos.Y > (float)(num8 * 16 + num9))
					{
						flag7 = true;
					}
					if (flag7)
					{
						Vector2 value27 = Vector2.Normalize(projPos);
						float num10 = -1f;
						if (value27.X < 0f && ((float)(num5 * 16 - num9) / value27.X < num10 || num10 == -1f))
						{
							num10 = (float)(num5 * 16 - num9) / value27.X;
						}
						if (value27.X > 0f && ((float)(num6 * 16 + num9) / value27.X < num10 || num10 == -1f))
						{
							num10 = (float)(num6 * 16 + num9) / value27.X;
						}
						if (value27.Y < 0f && ((float)(num7 * 16 - num9) / value27.Y < num10 || num10 == -1f))
						{
							num10 = (float)(num7 * 16 - num9) / value27.Y;
						}
						if (value27.Y > 0f && ((float)(num8 * 16 + num9) / value27.Y < num10 || num10 == -1f))
						{
							num10 = (float)(num8 * 16 + num9) / value27.Y;
						}
						projPos = value27 * num10;
					}
					if (projPos.X != Projectile.velocity.X || projPos.Y != Projectile.velocity.Y)
					{
						Projectile.netUpdate = true;
					}
					Projectile.velocity = projPos;
				}
			}
			Projectile.position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) - Projectile.Size / 2f;
			Projectile.rotation = Projectile.velocity.ToRotation() + num;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.timeLeft = 2;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(num12);
			player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(Projectile.velocity.Y * (float)Projectile.direction, Projectile.velocity.X * (float)Projectile.direction) + num23);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player owner = Main.player[Projectile.owner];
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}

			if (owner.gravDir == -1f)
			{
				spriteEffects |= SpriteEffects.FlipVertically;
			}
			Vector2 projPos = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;
			//Color projAlpha = Projectile.GetAlpha(lightColor);
			Vector2 playerPos = owner.RotatedRelativePoint(owner.MountedCenter) + Vector2.UnitY;// * owner.gfxOffY;
			Vector2 worldPos = projPos + Main.screenPosition - playerPos;
			Vector2 normalWorldPos = Vector2.Normalize(worldPos);
			float worldPosLength = worldPos.Length();
			//float worldPosRotation = worldPos.ToRotation() + (float)Math.PI / 2f;
			float numIsNeg5f = -5f;
			float tipInset = numIsNeg5f + 30f;
			// new Vector2(2f, worldPosLength - numIs25f);
			Vector2 beamPos = Vector2.Lerp(projPos + Main.screenPosition, playerPos + normalWorldPos * tipInset, 0.5f);
			//Vector2 beamRotate = -Vector2.UnitY.RotatedBy(Projectile.localAI[0] / 60f * (float)Math.PI);
			/*Vector2[] beamRotations = new Vector2[4]
			{
					beamRotate,
					beamRotate.RotatedBy(Math.PI / 2f),
					beamRotate.RotatedBy(Math.PI),
					beamRotate.RotatedBy((3 * Math.PI) / 2f)
			};*/
			/*if (worldPosLength > tipInset)
			{
				for (int i = 0; i < 2; i++)
				{
					Color beamColor1;
					if (i % 2 == 0)
					{
						beamColor1 = SetColorType(colorMode[0]);
						beamColor1.A = 128;
						beamColor1 *= 0.5f;
					}
					else
					{
						beamColor1 = SetColorType(colorMode[1]);
						beamColor1.A = 128;
						beamColor1 *= 0.5f;
					}
					Vector2 value150 = new Vector2(beamRotations[i].X, 0f).RotatedBy(worldPosRotation) * 4f;
					Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, beamPos - Main.screenPosition + value150, new Rectangle(0, 0, 1, 1), beamColor1, worldPosRotation, Vector2.One / 2f, new Vector2(2f, worldPosLength - tipInset), spriteEffects, 0);
				}
			}*/
			// int type2 = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].type;
			// Main.instance.LoadItem(type2);
			// Texture2D value151 = TextureAssets.Item[type2].Value;

			Texture2D itemTexture = texturePick;
			Texture2D glowTexture = texturePickGlow;
			if (pick)
			{
				itemTexture = texturePick;
				glowTexture = texturePickGlow;
			}
			else if (axe)
			{
				itemTexture = textureAxe;
				glowTexture = textureAxeGlow;
			}
			else if (hammer)
			{
				itemTexture = textureHammer;
				glowTexture = textureHammerGlow;
			}
			else
			{
				itemTexture = textureNone;
				glowTexture = textureNoneGlow;
			}
			
			Color color94 = Lighting.GetColor((int)playerPos.X / 16, (int)playerPos.Y / 16);
			Main.EntitySpriteDraw(itemTexture, playerPos - Main.screenPosition + normalWorldPos * numIsNeg5f, null,
				color94, Projectile.rotation + (float)Math.PI / 2f + ((spriteEffects == SpriteEffects.None || spriteEffects == SpriteEffects.FlipVertically) ? ((float)Math.PI) : 0f),
				new Vector2((spriteEffects != 0 && spriteEffects != SpriteEffects.FlipVertically) ? itemTexture.Width : 0, (float)itemTexture.Height / 2f) + Vector2.UnitY * 1f, owner.inventory[owner.selectedItem].scale, spriteEffects, 0);
			Main.EntitySpriteDraw(glowTexture, playerPos - Main.screenPosition + normalWorldPos * numIsNeg5f, null,
				new Color(255, 255, 255, 0), Projectile.rotation + (float)Math.PI / 2f + ((spriteEffects == SpriteEffects.None || spriteEffects == SpriteEffects.FlipVertically) ? ((float)Math.PI) : 0f),
				new Vector2((spriteEffects != 0 && spriteEffects != SpriteEffects.FlipVertically) ? glowTexture.Width : 0, (float)glowTexture.Height / 2f) + Vector2.UnitY * 1f, owner.inventory[owner.selectedItem].scale, spriteEffects, 0);
			/*if (worldPosLength > tipInset)
			{
				for (int j = 2; j < 4; j++)
				{
					Color beamColor2;
					if (j % 2 == 0)
					{
						beamColor2 = SetColorType(colorMode[0]);
						beamColor2.A = 128;
						beamColor2 *= 0.5f;
					}
					else
					{
						beamColor2 = SetColorType(colorMode[1]);
						beamColor2.A = 128;
						beamColor2 *= 0.5f;
					}
					Vector2 value152 = new Vector2(beamRotations[j].X, 0f).RotatedBy(worldPosRotation) * 4f;
					Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, beamPos - Main.screenPosition + value152, new Rectangle(0, 0, 1, 1), beamColor2, worldPosRotation, Vector2.One / 2f, new Vector2(2f, worldPosLength - tipInset), spriteEffects, 0);
				}
			}*/

			if (worldPosLength > tipInset)
			{
				float length = (worldPosLength - tipInset) / 384f;
				float thinkness = MathHelper.Clamp(length, 0.5f, 3f);
				float worldPosRotation2 = worldPos.ToRotation();

				SpriteEffects laserSpriteEffects = SpriteEffects.None;
				if (Main.rand.NextBool())
				{
					laserSpriteEffects |= SpriteEffects.FlipHorizontally;
				}
				if (Main.rand.NextBool())
				{
					laserSpriteEffects |= SpriteEffects.FlipVertically;
				}

				if (pick)
				{
					Color drawColor = SetColorType(1);
					drawColor.A = 0;
					Rectangle sourceRectangle = lightning.Frame(1, 8, 0, Main.rand.Next(0, 7));
					Main.EntitySpriteDraw(lightning, beamPos - Main.screenPosition, sourceRectangle, drawColor, worldPosRotation2, sourceRectangle.Size() / 2f, new Vector2(length, thinkness), laserSpriteEffects, 0);
					Main.EntitySpriteDraw(projTexture, projPos, null, drawColor, Projectile.rotation, new Vector2(projTexture.Width, projTexture.Height) / 2f, Projectile.scale, SpriteEffects.None, 0);
				}
				if (axe)
				{
					Color drawColor = SetColorType(2);
					drawColor.A = 0;
					Rectangle sourceRectangle = lightning.Frame(1, 8, 0, Main.rand.Next(0, 7));
					Main.EntitySpriteDraw(lightning, beamPos - Main.screenPosition, sourceRectangle, drawColor, worldPosRotation2, sourceRectangle.Size() / 2f, new Vector2(length, thinkness), laserSpriteEffects, 0);
					Main.EntitySpriteDraw(projTexture, projPos, null, drawColor, Projectile.rotation, new Vector2(projTexture.Width, projTexture.Height) / 2f, Projectile.scale, SpriteEffects.None, 0);
				}
				if (hammer)
				{
					Color drawColor = SetColorType(3);
					drawColor.A = 0;
					Rectangle sourceRectangle = lightning.Frame(1, 8, 0, Main.rand.Next(0, 7));
					Main.EntitySpriteDraw(lightning, beamPos - Main.screenPosition, sourceRectangle, drawColor, worldPosRotation2, sourceRectangle.Size() / 2f, new Vector2(length, thinkness), laserSpriteEffects, 0);
					Main.EntitySpriteDraw(projTexture, projPos, null, drawColor, Projectile.rotation, new Vector2(projTexture.Width, projTexture.Height) / 2f, Projectile.scale, SpriteEffects.None, 0);
				}

				if (!pick && !axe && !hammer)
				{
					Color drawColor = SetColorType(0);
					drawColor.A = 0;
					Rectangle sourceRectangle = lightning.Frame(1, 8, 0, Main.rand.Next(0, 7));
					Main.EntitySpriteDraw(lightning, beamPos - Main.screenPosition, sourceRectangle, drawColor, worldPosRotation2, sourceRectangle.Size() / 2f, new Vector2(length, thinkness), laserSpriteEffects, 0);
					Main.EntitySpriteDraw(projTexture, projPos, null, drawColor, Projectile.rotation, new Vector2(projTexture.Width, projTexture.Height) / 2f, Projectile.scale, SpriteEffects.None, 0);
				}
			}

			/*float tipAlpha = Projectile.localAI[0] / 60f;
			if (tipAlpha > 0.5f)
			{
				tipAlpha = 1f - tipAlpha;
			}
			// projAlpha * tipAlpha * 2f

			Color tipColor1 = SetColorType(colorMode[0]);
			Color tipColor2 = SetColorType(colorMode[1]);

			Color tipColorLerp = Color.Lerp(tipColor1, tipColor2, tipAlpha);
			tipColorLerp.A = 0;

			//Main.EntitySpriteDraw(projTexture, projPos, null, tipColorLerp, Projectile.rotation, new Vector2(projTexture.Width, projTexture.Height) / 2f, Projectile.scale, spriteEffects, 0);
			*/
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(pick);
			writer.Write(axe);
			writer.Write(hammer);
			//writer.Write(colorMode[0]);
			//writer.Write(colorMode[1]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			pick = reader.ReadBoolean();
			axe = reader.ReadBoolean();
			hammer = reader.ReadBoolean();
			//colorMode[0] = reader.ReadInt32();
			//colorMode[1] = reader.ReadInt32();
		}
	}
}