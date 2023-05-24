using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Support
{
	public class CurativeButterfly : ModProjectile
	{
		// Some AI taken from Nightglow projectile
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Type] = 0;
			ProjectileID.Sets.TrailCacheLength[Type] = 3;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
			ProjectileID.Sets.MinionShot[Type] = true;
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
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
			bool flag = false;
			bool canHome = false;
			float lifeTime1 = 520f;
			float lifeTime2 = 40f;
			float lifeTime3 = 480f;
			float velSlowDown = 0.97f;
			float lerpValue1 = 0.075f;
			float lerpValue2 = 0.125f;
			float velocityMulti = 10f;
			if (Projectile.timeLeft == Projectile.timeLeft - 2)
			{
				for (int i = 0; i < 3; i++)
				{
					Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Grass, Main.rand.NextVector2CircularEdge(3f, 3f) * (Main.rand.NextFloat() * 0.5f + 0.5f));
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
			else
			{
				Projectile.alpha++;
			}

			if (Projectile.timeLeft < lifeTime3)
			{
				Projectile.tileCollide = true;
			}

			if (flag)
			{
				float num6 = (float)Math.Cos((float)Projectile.whoAmI % 6f / 6f + Projectile.position.X / 320f + Projectile.position.Y / 160f);
				Projectile.velocity *= velSlowDown;
				Projectile.velocity = Projectile.velocity.RotatedBy(num6 * ((float)Math.PI * 2f) * 0.125f * 1f / 30f);
				Projectile.tileCollide = false;
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

			// So it will lean slightly towards the direction it's moving
			Projectile.rotation = Projectile.velocity.X * 0.05f;

			if (Main.player.IndexInRange(newTarget))
			{
				Player player = Main.player[newTarget];
				if (Projectile.Hitbox.Intersects(player.Hitbox))
				{
					player.Heal(20);
					SoundEngine.PlaySound(SoundID.Item4 with { Pitch = 0.5f }, Projectile.Center);
					Projectile.Kill();
				}
			}

			if (Math.Abs(Projectile.velocity.X) < 1f)
			{
				Projectile.spriteDirection = Main.player[Projectile.owner].direction * -1;
			}
			else
			{
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt() * -1;
			}

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 8;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Grass, Main.rand.NextVector2CircularEdge(3f, 3f) * (Main.rand.NextFloat() * 0.5f + 0.5f));
				dust.noGravity = true;
			}
		}

		// Fade from bright white to the light color
		public Color FadingBrightness(Color lightColor, out float lerp, int alpha = 255)
		{
			float lerpValue = MathHelper.Lerp(0f, 1f, Projectile.timeLeft / 600f);

			int r = (int)MathHelper.Lerp(lightColor.R, 255, lerpValue);
			int g = (int)MathHelper.Lerp(lightColor.G, 255, lerpValue);
			int b = (int)MathHelper.Lerp(lightColor.B, 255, lerpValue);

			lerp = lerpValue;

			return new(r, g, b, alpha);
		}


		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			// Get texture of projectile
			Texture2D texture = ModContent.Request<Texture2D>((GetType().Namespace + "." + Name + "Outline").Replace('.', '/')).Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

			FadingBrightness(lightColor, out float fadeAmount);

			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(1f, Projectile.gfxOffY),
				sourceRectangle, new Color(0, 100, 0, 0) * fadeAmount, Projectile.rotation, sourceRectangle.Size() / 2f, Projectile.scale, spriteEffects, 0);

			Main.EntitySpriteDraw(texture, Projectile.oldPos[1] + new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition + new Vector2(1f, Projectile.gfxOffY),
				sourceRectangle, new Color(0, 50, 0, 0) * fadeAmount, Projectile.rotation, sourceRectangle.Size() / 2f, Projectile.scale, spriteEffects, 0);

			return true;
		}

		public override void PostDraw(Color lightColor)
		{
			SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			// Get texture of projectile
			Texture2D texture = ModContent.Request<Texture2D>((GetType().Namespace + "." + Name + "Glow").Replace('.', '/')).Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(1f, Projectile.gfxOffY),
				sourceRectangle, FadingBrightness(lightColor, out _), Projectile.rotation, sourceRectangle.Size() / 2f, Projectile.scale, spriteEffects, 0);
		}
	}
}