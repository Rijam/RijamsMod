using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Magic
{
	public class LanternLightUnused : ModProjectile // Legacy
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return false;
		}

		public float vecolityMultiplier = 30f;
		public int homingRange = 800;
		public int timeLeftMax = 300;
		public float timeBeforeItCanStartHoming = 180f;
		public float timeLeftBeforeItStopsHoming = 60f;
		public int trailLength = 19;
		public float shineScale = 1f;
		public bool bounceOnTiles = true;
		public bool homingNeedsLineOfSight = true;
		public Color overrideColor = Color.Black;
		public int buffType = 0;
		public int buffTime = 60;
		public int buffChance = 1;
		public bool orgTileCollide = true;
		public bool orgIgnoreWater = false;
		public int orgPenetrate = 1;

		// Nightglow projectile clone
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Type] = 2;
			ProjectileID.Sets.TrailCacheLength[Type] = 20;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 960;
		}
		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			//Projectile.aiStyle = 171;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
		public override void OnSpawn(IEntitySource source)
		{
			/*Projectile.timeLeft = timeLeftMax;
			Projectile.tileCollide = orgTileCollide;
			Projectile.ignoreWater = orgIgnoreWater;
			Projectile.maxPenetrate = orgPenetrate;
			Projectile.penetrate = orgPenetrate;*/
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (bounceOnTiles)
			{
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = oldVelocity.X * -1f;
				}

				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = oldVelocity.Y * -1f;
				}
			}
			return !bounceOnTiles;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (buffType > 0 && Main.rand.NextBool(buffChance))
			{
				target.AddBuff(buffType, buffTime);
			}
		}

		public override bool PreAI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.timeLeft = timeLeftMax;
				Projectile.maxPenetrate = orgPenetrate;
				Projectile.penetrate = orgPenetrate;
			}
			Projectile.tileCollide = orgTileCollide;
			Projectile.ignoreWater = orgIgnoreWater;
			return true;
		}

		public override void AI()
		{
			/*
			if (Projectile.ai[2] == 0 || Projectile.ai[2] == timeBeforeItCanStartHoming || Projectile.ai[2] == timeLeftBeforeItStopsHoming || Projectile.ai[2] == Projectile.timeLeft)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(
					"-> vecolityMultiplier " + vecolityMultiplier + " homingRange " + homingRange + " timeLeftMax " + timeLeftMax
					+ " timeBeforeItCanStartHoming " + timeBeforeItCanStartHoming + " timeLeftBeforeItStopsHoming " + timeLeftBeforeItStopsHoming
					+ " trailLength " + trailLength + " shineScale " + shineScale + " bounceOnTiles " + bounceOnTiles + " homingNeedsLineOfSight " + homingNeedsLineOfSight
					+ " overrideColor " + overrideColor + " buffType " + buffType + " buffTime " + buffTime + " buffChance " + buffChance
					+ " Projectile.Center " + Projectile.Center), new Color(100, 100, 100));
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(
					"-> Projectile.timeLeft " + Projectile.timeLeft + " Projectile.tileCollide " + Projectile.tileCollide + " Projectile.ignoreWater " + Projectile.ignoreWater
					+ " Projectile.penetrate " + Projectile.penetrate), new Color(150, 150, 150));
			}
			*/
			Projectile.ai[2]++;

			// Projectile.ai[1] controls the color

			bool beforeHoming = false;
			bool canHome = false;
			float velSlowDown = 0.97f;
			float lerpValue1 = 0.075f;
			float lerpValue2 = 0.125f;

			int savedAlpha = Projectile.alpha;
			Projectile.alpha = 0;
			Color fairyQueenWeaponsColor = Projectile.GetFairyQueenWeaponsColor();
			Projectile.alpha = savedAlpha;

			Color colorToUse = overrideColor != Color.Black ? overrideColor : fairyQueenWeaponsColor;

			if (Projectile.timeLeft >= timeLeftMax - 2)
			{
				/* ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(
					"-> Projectile.timeLeft >= timeLeftMax - 2, Projectile.timeLeft " + Projectile.timeLeft + " timeLeftMax - 2 " + (timeLeftMax - 2)), new Color(150, 50, 50)); */
				for (int i = 0; i < 3; i++)
				{
					Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Main.rand.NextVector2CircularEdge(3f, 3f) * (Main.rand.NextFloat() * 0.5f + 0.5f), 0, colorToUse);
					dust.scale *= 1.2f;
					dust.noGravity = true;
				}
			}
			Lighting.AddLight(Projectile.Center, colorToUse.ToVector3());

			if (Projectile.timeLeft > timeBeforeItCanStartHoming)
			{
				//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(
				//	"-> beforeHoming, Projectile.timeLeft " + Projectile.timeLeft + " timeBeforeItCanStartHoming " + timeBeforeItCanStartHoming), new Color(50, 150, 50));
				beforeHoming = true;
			}
			else if (Projectile.timeLeft > timeLeftBeforeItStopsHoming)
			{
				//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(
				//	"-> canHome, Projectile.timeLeft " + Projectile.timeLeft + " timeLeftBeforeItStopsHoming " + timeLeftBeforeItStopsHoming), new Color(50, 50, 150));
				canHome = true;
			}

			if (beforeHoming)
			{
				float num6 = (float)Math.Cos(Projectile.whoAmI % 6f / 6f + Projectile.position.X / 320f + Projectile.position.Y / 160f);
				Projectile.velocity *= velSlowDown;
				Projectile.velocity = Projectile.velocity.RotatedBy(num6 * (MathHelper.TwoPi * -Projectile.direction) * 0.125f * 1f / 30f);
			}

			int newTarget;
			if (homingNeedsLineOfSight)
			{
				newTarget = Projectile.FindTargetWithLineOfSight(homingRange);
			}
			else
			{
				NPC maybeTarget = Projectile.FindTargetWithinRange(homingRange);
				newTarget = maybeTarget != null ? maybeTarget.whoAmI : -1;
			}
			//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(
			//	"-> newTarget " + newTarget), new Color(50, 50, 150));

			if (canHome)
			{
				Vector2 projVelocity = Projectile.velocity;
				if (Main.npc.IndexInRange(newTarget))
				{
					NPC npc = Main.npc[newTarget];
					projVelocity = Projectile.DirectionTo(npc.Center) * vecolityMultiplier;
				}
				else
				{
					Projectile.timeLeft--;
				}

				float lerpAmount = MathHelper.Lerp(lerpValue1, lerpValue2, Utils.GetLerpValue(timeBeforeItCanStartHoming, 30f, Projectile.timeLeft, clamped: true));
				Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, projVelocity, lerpAmount);
				Projectile.velocity *= MathHelper.Lerp(0.85f, 1f, Utils.GetLerpValue(0f, 90f, Projectile.timeLeft, clamped: true));
			}
			Projectile.Opacity = Utils.GetLerpValue(timeLeftMax, timeLeftMax - 20f, Projectile.timeLeft, clamped: true);
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
		}
		public override void Kill(int timeLeft)
		{
			Color fairyQueenWeaponsColor2 = Projectile.GetFairyQueenWeaponsColor();

			Color colorToUse = overrideColor != Color.Black ? overrideColor : fairyQueenWeaponsColor2;

			Vector2 target2 = Projectile.Center;
			Main.rand.NextFloat();
			for (int j = 0; j < Projectile.oldPos.Length; j++)
			{
				Vector2 projOldPos = Projectile.oldPos[j];
				if (projOldPos == Vector2.Zero)
					break;

				int random = Main.rand.Next(1, 3);
				float lerp = MathHelper.Lerp(0.3f, 1f, Utils.GetLerpValue(Projectile.oldPos.Length, 0f, j, clamped: true));
				if (j >= Projectile.oldPos.Length * 0.3f)
					random--;

				if (j >= Projectile.oldPos.Length * 0.75f)
					random -= 2;

				projOldPos.DirectionTo(target2).SafeNormalize(Vector2.Zero);
				target2 = projOldPos;
				for (int i = 0; i < random; i++)
				{
					Dust dust = Dust.NewDustDirect(projOldPos, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 0, colorToUse);
					dust.velocity *= Main.rand.NextFloat() * 0.8f;
					dust.noGravity = true;
					dust.scale = 0.9f + Main.rand.NextFloat() * 1.2f;
					dust.fadeIn = Main.rand.NextFloat() * 1.2f * lerp;
					dust.scale *= lerp;
					if (dust.dustIndex != 6000)
					{
						Dust dust12 = Dust.CloneDust(dust.dustIndex);
						dust12.scale /= 2f;
						dust12.fadeIn *= 0.85f;
						dust12.color = new Color(255, 255, 255, 255);
						dust12.noLightEmittence = true;
					}
				}
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * Projectile.Opacity;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = TextureAssets.Projectile[Projectile.type].Height() / Main.projFrames[Projectile.type];
			int frameY = frameHeight * Projectile.frame;
			Rectangle sourceRect = new(0, frameY, texture.Width, frameHeight);
			Vector2 projOrigin = sourceRect.Size() / 2f;

			int numIs0 = 0;
			int iterationAmount = -1;
			float maxScale = 0.7f;
			float scaleDiv = 20f;
			float rotationMulti = 0f;

			for (int i = trailLength; (iterationAmount > 0 && i < numIs0) || (iterationAmount < 0 && i > numIs0); i += iterationAmount)
			{
				if (i >= Projectile.oldPos.Length)
				{
					continue;
				}

				Color trailColor = Projectile.GetFairyQueenWeaponsColor(0.5f);
				trailColor *= Utils.GetLerpValue(0f, 20f, Projectile.timeLeft, clamped: true);

				float colorMulti = numIs0 - i;
				if (iterationAmount < 0)
				{
					colorMulti = trailLength - i;
				}

				trailColor *= colorMulti / ((float)ProjectileID.Sets.TrailCacheLength[Projectile.type] * 1.5f);
				Vector2 trailOldPos = Projectile.oldPos[i];

				float trailRotation = Projectile.oldRot[i];
				SpriteEffects trailSpriteEffects = ((Projectile.oldSpriteDirection[i] == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

				if (trailOldPos == Vector2.Zero)
				{
					continue;
				}

				Vector2 trailPos = trailOldPos + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
				Main.EntitySpriteDraw(texture, trailPos, sourceRect, trailColor, trailRotation + Projectile.rotation * rotationMulti * (i - 1) * (-spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt()), projOrigin, MathHelper.Lerp(Projectile.scale, maxScale, (float)i / scaleDiv), trailSpriteEffects);

			}

			Color fairyQueenWeaponColor = Projectile.GetFairyQueenWeaponsColor(0f);

			Vector2 shinyPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(texture, shinyPos, sourceRect, fairyQueenWeaponColor, Projectile.rotation, projOrigin, Projectile.scale * 0.9f, spriteEffects);
			Texture2D textureExtra98 = TextureAssets.Extra[98].Value;
			Vector2 shinyOrigin = textureExtra98.Size() / 2f;
			Color colorTopBot = fairyQueenWeaponColor * 0.5f;
			Color ColorLeftRight = fairyQueenWeaponColor;
			float scaleWarping = Utils.GetLerpValue(15f, 30f, Projectile.timeLeft, clamped: true) * Utils.GetLerpValue(timeLeftMax, timeLeftMax - 40f, Projectile.timeLeft, clamped: true) * (1f + 0.2f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 30f / 0.5f * ((float)Math.PI * 2f) * 3f)) * 0.8f;
			Vector2 scale1 = new Vector2(0.5f, 5f) * scaleWarping * shineScale;
			Vector2 scale2 = new Vector2(0.5f, 2f) * scaleWarping * shineScale;
			colorTopBot *= scaleWarping;
			ColorLeftRight *= scaleWarping;

			scale1 *= 0.4f;
			scale2 *= 0.4f;

			// Flashy star bits
			Main.EntitySpriteDraw(textureExtra98, shinyPos, null, ColorLeftRight, MathHelper.PiOver2, shinyOrigin, scale1, spriteEffects);
			Main.EntitySpriteDraw(textureExtra98, shinyPos, null, ColorLeftRight, 0f, shinyOrigin, scale2, spriteEffects);
			Main.EntitySpriteDraw(textureExtra98, shinyPos, null, colorTopBot, MathHelper.PiOver2, shinyOrigin, scale1 * 0.6f, spriteEffects);
			Main.EntitySpriteDraw(textureExtra98, shinyPos, null, colorTopBot, 0f, shinyOrigin, scale2 * 0.6f, spriteEffects);

			Color projColor = Projectile.GetAlpha(lightColor);
			float projScale = Projectile.scale;
			float projRotation = Projectile.rotation;

			projColor.A /= 2;

			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRect, projColor, projRotation, projOrigin, projScale, spriteEffects);

			/*
			Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				Projectile.Hitbox, Color.Orange * 0.5f, 0, Projectile.Hitbox.Size() / 2, Projectile.scale, spriteEffects, 0);
			*/

			return false;
		}

		// I hope you have good internet D:
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(vecolityMultiplier);
			writer.Write(homingRange);
			writer.Write(timeLeftMax);
			writer.Write(timeBeforeItCanStartHoming);
			writer.Write(timeLeftBeforeItStopsHoming);
			writer.Write(trailLength);
			writer.Write(shineScale);
			writer.Write(bounceOnTiles);
			writer.Write(homingNeedsLineOfSight);
			writer.WriteRGB(overrideColor);
			writer.Write(overrideColor.A);
			writer.Write(buffType);
			writer.Write(buffTime);
			writer.Write(buffChance);
			writer.Write(orgTileCollide);
			writer.Write(orgIgnoreWater);
			writer.Write(orgPenetrate);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			vecolityMultiplier = reader.ReadSingle();
			homingRange = reader.ReadInt32();
			timeLeftMax = reader.ReadInt32();
			timeBeforeItCanStartHoming = reader.ReadSingle();
			timeLeftBeforeItStopsHoming = reader.ReadSingle();
			trailLength = reader.ReadInt32();
			shineScale = reader.ReadSingle();
			bounceOnTiles = reader.ReadBoolean();
			homingNeedsLineOfSight = reader.ReadBoolean();
			overrideColor = reader.ReadRGB();
			overrideColor.A = reader.ReadByte();
			buffType = reader.ReadInt32();
			buffTime = reader.ReadInt32();
			buffChance = reader.ReadInt32();
			orgTileCollide = reader.ReadBoolean();
			orgIgnoreWater = reader.ReadBoolean();
			orgPenetrate = reader.ReadInt32();
		}
	}
}