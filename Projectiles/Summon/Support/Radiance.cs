using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Support
{
	public class Radiance : ModProjectile
	{
		// Nightglow projectile clone
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Type] = 2;
			ProjectileID.Sets.TrailCacheLength[Type] = 20;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 960;
			ProjectileID.Sets.MinionShot[Type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			//Projectile.aiStyle = 171;
			Projectile.alpha = 255;
			Projectile.penetrate = 3;
			Projectile.friendly = true;
			Projectile.timeLeft = 240;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = oldVelocity.X * -1f;
			}

			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = oldVelocity.Y * -1f;
			}
			return false;
		}
		public override void AI()
		{
			// Projectile.ai[1] controls the color

			bool flag = false;
			bool canHome = false;
			float lifeTime1 = 180f;
			float lifeTime2 = 20f;
			float velSlowDown = 0.97f;
			float lerpValue1 = 0.075f;
			float lerpValue2 = 0.125f;
			float velocityMulti = 30f;
			if (Projectile.timeLeft == 238)
			{
				int num5 = Projectile.alpha;
				Projectile.alpha = 0;
				Color fairyQueenWeaponsColor = Projectile.GetFairyQueenWeaponsColor();
				Projectile.alpha = num5;
				for (int i = 0; i < 3; i++)
				{
					Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Main.rand.NextVector2CircularEdge(3f, 3f) * (Main.rand.NextFloat() * 0.5f + 0.5f), 0, fairyQueenWeaponsColor);
					dust.scale *= 1.2f;
					dust.noGravity = true;
				}
			}

			if (Projectile.timeLeft > lifeTime1)
			{
				flag = true;
			}
			else if (Projectile.timeLeft > lifeTime2)
			{
				canHome = true;
			}

			if (flag)
			{
				//Main.NewText("flag");
				float num6 = (float)Math.Cos((float)Projectile.whoAmI % 6f / 6f + Projectile.position.X / 320f + Projectile.position.Y / 160f);
				Projectile.velocity *= velSlowDown;
				Projectile.velocity = Projectile.velocity.RotatedBy(num6 * ((float)Math.PI * 2f) * 0.125f * 1f / 30f);
			}

			int newTarget = (int)Projectile.ai[0];

			if (newTarget == -1)
			{
				newTarget = Projectile.owner;
			}

			if (canHome)
			{
				Vector2 projVelocity = Projectile.velocity;
				if (Main.player.IndexInRange(newTarget))
				{
					Player player = Main.player[newTarget];
					projVelocity = Projectile.DirectionTo(player.Center) * velocityMulti;
				}
				else
				{
					Projectile.timeLeft--;
				}

				float lerpAmount = MathHelper.Lerp(lerpValue1, lerpValue2, Utils.GetLerpValue(lifeTime1, 30f, Projectile.timeLeft, clamped: true));
				Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, projVelocity, lerpAmount);
				Projectile.velocity *= MathHelper.Lerp(0.85f, 1f, Utils.GetLerpValue(0f, 90f, Projectile.timeLeft, clamped: true));
			}

			Projectile.Opacity = Utils.GetLerpValue(240f, 220f, Projectile.timeLeft, clamped: true);
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;

			if (Main.player.IndexInRange(newTarget))
			{
				Player player = Main.player[newTarget];
				if (Projectile.Hitbox.Intersects(player.Hitbox))
				{
					//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Healing " + player.whoAmI + " Name " + player.name), Color.Lime);
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						player.Heal(20);
					}
					else if (Main.netMode != NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.SpiritHeal, number: player.whoAmI, number2: 20);
					}
					//player.Heal(20);
					//player.HealEffect(20, true);
					SoundEngine.PlaySound(SoundID.Item4 with { Pitch = 0.5f }, Projectile.Center);
					Projectile.Kill();
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			Color fairyQueenWeaponsColor2 = Projectile.GetFairyQueenWeaponsColor();
			//SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			Vector2 target2 = Projectile.Center;
			Main.rand.NextFloat();
			for (int num26 = 0; num26 < Projectile.oldPos.Length; num26++)
			{
				Vector2 vector6 = Projectile.oldPos[num26];
				if (vector6 == Vector2.Zero)
					break;

				int num27 = Main.rand.Next(1, 3);
				float num28 = MathHelper.Lerp(0.3f, 1f, Utils.GetLerpValue(Projectile.oldPos.Length, 0f, num26, clamped: true));
				if ((float)num26 >= (float)Projectile.oldPos.Length * 0.3f)
					num27--;

				if ((float)num26 >= (float)Projectile.oldPos.Length * 0.75f)
					num27 -= 2;

				vector6.DirectionTo(target2).SafeNormalize(Vector2.Zero);
				target2 = vector6;
				for (float num29 = 0f; num29 < (float)num27; num29++)
				{
					int num30 = Dust.NewDust(vector6, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 0, fairyQueenWeaponsColor2);
					Dust dust2 = Main.dust[num30];
					dust2.velocity *= Main.rand.NextFloat() * 0.8f;
					Main.dust[num30].noGravity = true;
					Main.dust[num30].scale = 0.9f + Main.rand.NextFloat() * 1.2f;
					Main.dust[num30].fadeIn = Main.rand.NextFloat() * 1.2f * num28;
					dust2 = Main.dust[num30];
					dust2.scale *= num28;
					if (num30 != 6000)
					{
						Dust dust12 = Dust.CloneDust(num30);
						dust2 = dust12;
						dust2.scale /= 2f;
						dust2 = dust12;
						dust2.fadeIn *= 0.85f;
						dust12.color = new Color(255, 255, 255, 255);
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
			int startingTrailValue = 19;
			float maxScale = 0.7f;
			float scaleDiv = 20f;
			float rotationMulti = 0f;

			for (int i = startingTrailValue; (iterationAmount > 0 && i < numIs0) || (iterationAmount < 0 && i > numIs0); i += iterationAmount)
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
					colorMulti = startingTrailValue - i;
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
				Main.EntitySpriteDraw(texture, trailPos, sourceRect, trailColor, trailRotation + Projectile.rotation * rotationMulti * (float)(i - 1) * (float)(-spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt()), projOrigin, MathHelper.Lerp(Projectile.scale, maxScale, (float)i / scaleDiv), trailSpriteEffects);

			}

			Color fairyQueenWeaponColor = Projectile.GetFairyQueenWeaponsColor(0f);

			Vector2 shinyPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(texture, shinyPos, sourceRect, fairyQueenWeaponColor, Projectile.rotation, projOrigin, Projectile.scale * 0.9f, spriteEffects);
			Texture2D textureExtra98 = TextureAssets.Extra[98].Value;
			Vector2 shinyOrigin = textureExtra98.Size() / 2f;
			Color colorTopBot = fairyQueenWeaponColor * 0.5f;
			Color ColorLeftRight = fairyQueenWeaponColor;
			float scaleWarping = Utils.GetLerpValue(15f, 30f, Projectile.timeLeft, clamped: true) * Utils.GetLerpValue(240f, 200f, Projectile.timeLeft, clamped: true) * (1f + 0.2f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 30f / 0.5f * ((float)Math.PI * 2f) * 3f)) * 0.8f;
			Vector2 scale1 = new Vector2(0.5f, 5f) * scaleWarping;
			Vector2 scale2 = new Vector2(0.5f, 2f) * scaleWarping;
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

			return false;
		}
	}
}