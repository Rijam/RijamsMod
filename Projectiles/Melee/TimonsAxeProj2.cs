using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee
{
	public class TimonsAxeProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// If a Jellyfish is zapping and we attack it with this projectile, it will deal damage to us.
			// This set has the projectiles for the Night's Edge, Excalibur, Terra Blade (close range), and The Horseman's Blade (close range).
			// This set does not have the True Night's Edge, True Excalibur, or the long range Terra Beam projectiles.
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
			Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.alpha = 0;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 300;
			Projectile.extraUpdates = 0;
			Projectile.usesOwnerMeleeHitCD = true;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.ownerHitCheck = true;
			Projectile.ownerHitCheckDistance = 20 * 16f;
			Projectile.localNPCHitCooldown = -1;

			//Projectile.aiStyle = 190;
			//AIType = ProjectileID.NightsEdge;

			Projectile.noEnchantmentVisuals = true;
		}

		public override void AI()
		{
			/*if (Projectile.localAI[0] == 0f)
			{
				SoundEngine.PlaySound(SoundID.Item60 with { Volume = 0.65f }, Projectile.position);
			}*/

			// Projectile.ai[0] == direction
			// Projectile.ai[1] == max time
			// Projectile.ai[2] == scale
			// Projectile.localAI[0] == current time

			Projectile.localAI[0] += 1f;
			Player player = Main.player[Projectile.owner];
			float currentTimeOverMax = Projectile.localAI[0] / Projectile.ai[1];
			float direction = Projectile.ai[0];
			float velocityRotation = Projectile.velocity.ToRotation();
			float adjustedRotation = (float)Math.PI * direction * currentTimeOverMax + velocityRotation + direction * (float)Math.PI + player.fullRotation;
			Projectile.rotation = adjustedRotation;
			float scaleMulti = 0.3f;
			float scaleAdder = 1f;

			Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
			Projectile.scale = scaleAdder + currentTimeOverMax * scaleMulti;

			if (Math.Abs(direction) < 0.2f)
			{
				Projectile.rotation += (float)Math.PI * 4f * direction * 10f * currentTimeOverMax;
				float lerpTime = Utils.Remap(Projectile.localAI[0], 10f, Projectile.ai[1] - 5f, 0f, 1f);
				Projectile.position += Projectile.velocity.SafeNormalize(Vector2.Zero) * (45f * lerpTime);
				Projectile.scale += lerpTime * 0.4f;
			}

			/*if (Main.rand.NextBool(2))
			{
				float dustRotation = Projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.7f;
				Vector2 dustPos = Projectile.Center + dustRotation.ToRotationVector2() * 84f * Projectile.scale;
				if (Main.rand.NextBool(10))
				{
					Dust dust = Dust.NewDustPerfect(dustPos, DustID.WaterCandle, null, 150, default, 1.4f);
					dust.noLight = dust.noLightEmittence = true;
				}

				if (Main.rand.NextBool(5))
				{
					Dust.NewDustPerfect(dustPos, DustID.Clentaminator_Blue, new Vector2(player.velocity.X * 0.2f + (player.direction * 3), player.velocity.Y * 0.2f), 100, default, 1.4f).noGravity = true;
				}
			}*/

			Projectile.scale *= Projectile.ai[2];
			if (Projectile.localAI[0] >= Projectile.ai[1])
			{
				Projectile.Kill();
			}

			// This for loop spawns the visuals when using Flasks (weapon imbues)
			for (float i = -MathHelper.PiOver4; i <= MathHelper.PiOver4; i += MathHelper.PiOver2)
			{
				Rectangle rectangle = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + i).ToRotationVector2() * 70f * Projectile.scale, new Vector2(60f * Projectile.scale, 60f * Projectile.scale));
				Projectile.EmitEnchantmentVisualsAt(rectangle.TopLeft(), rectangle.Width, rectangle.Height);
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float coneLength = 94f * Projectile.scale;
			float projRotation = (float)Math.PI * 2f / 25f * Projectile.ai[0];
			float maximumAngle = (float)Math.PI / 4f;
			float coneRotation = Projectile.rotation + projRotation;

			// Imperfect dusts, but it gives a general idea.
			// Dust.NewDustPerfect(Projectile.Center + coneRotation.ToRotationVector2() * coneLength, DustID.Pixie, Vector2.Zero);


			if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation, maximumAngle))
				return true;

			float backOfTheSwing = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);
			if (backOfTheSwing > 0f)
			{
				float coneRotation2 = coneRotation - (float)Math.PI / 4f * Projectile.ai[0] * backOfTheSwing;

				// Imperfect dusts, but it gives a general idea.
				// Dust.NewDustPerfect(Projectile.Center + coneRotation2.ToRotationVector2() * coneLength, DustID.Pixie, Vector2.Zero);

				if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation2, maximumAngle))
				{
					return true;
				}
			}

			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
			{
				PositionInWorld = target.Center,
				MovementVector = new Vector2(hit.HitDirection * 10, target.Center.Y - Projectile.Center.Y).SafeNormalize(Vector2.Zero)
			});

			// Set the target's hit direction to away from the player so the knockback is in the correct direction.
			hit.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
			{
				PositionInWorld = target.Center,
				MovementVector = new Vector2(info.HitDirection * 10, target.Center.Y - Projectile.Center.Y).SafeNormalize(Vector2.Zero)
			});

			info.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 position = Projectile.Center - Main.screenPosition;
			//Asset<Texture2D> asset = TextureAssets.Projectile[ProjectileID.NightsEdge];
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Rectangle sourceRectangle = texture.Frame(1, 4);
			Vector2 origin = sourceRectangle.Size() / 2f;
			float scale = Projectile.scale * 1.1f;
			SpriteEffects spriteEffects = ((!(Projectile.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None);
			float currentTimeOverMax = Projectile.localAI[0] / Projectile.ai[1];
			float lerpTime = Utils.Remap(currentTimeOverMax, 0f, 0.6f, 0f, 1f) * Utils.Remap(currentTimeOverMax, 0.6f, 1f, 1f, 0f);
			float backAndFrontScale = 0.975f;
			float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
			lightingColor = Utils.Remap(lightingColor, 0.2f, 1f, 0f, 1f);
			Color backDarkColor = new(20, 50, 127);

			Color middleMediumColor = new(30, 67, 160);
			Color frontLightColor = new(40, 120, 255);
			Color whiteTimesLerpTime = Color.White * lerpTime * 0.5f;
			whiteTimesLerpTime.A = (byte)(whiteTimesLerpTime.A * (1f - lightingColor));
			Color faintLightingColor = whiteTimesLerpTime * lightingColor * 0.5f;
			faintLightingColor.G = (byte)(faintLightingColor.G * lightingColor);
			faintLightingColor.R = (byte)(faintLightingColor.R * (0.25f + lightingColor * 0.75f));
			
			// Back part
			Main.EntitySpriteDraw(texture, position, sourceRectangle, backDarkColor * lightingColor * lerpTime, Projectile.rotation + Projectile.ai[0] * ((float)Math.PI / 4f) * -1f * (1f - currentTimeOverMax), origin, scale * backAndFrontScale, spriteEffects, 0f);
			// Very faint part affected by the light color
			Main.EntitySpriteDraw(texture, position, sourceRectangle, faintLightingColor * 0.15f, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, scale, spriteEffects, 0f);
			// Middle part
			Main.EntitySpriteDraw(texture, position, sourceRectangle, middleMediumColor * lightingColor * lerpTime * 0.3f, Projectile.rotation, origin, scale * 0.8f, spriteEffects, 0f);
			// Front part
			Main.EntitySpriteDraw(texture, position, sourceRectangle, frontLightColor * lightingColor * lerpTime * 0.7f, Projectile.rotation, origin, scale * backAndFrontScale, spriteEffects, 0f);
			// Thin top line (final frame)
			Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.3f * lerpTime * (1f - lightingColor * 0.7f), Projectile.rotation + Projectile.ai[0] * 0.01f, origin, scale, spriteEffects, 0f);
			
			Vector2 drawpos = position + (Projectile.rotation + Utils.Remap(currentTimeOverMax, 0f, 1f, 0f, (float)Math.PI / 2f) * Projectile.ai[0]).ToRotationVector2() * ((float)texture.Width * 0.5f - 4f) * scale;
			DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawpos, new Color(255, 255, 255, 0) * lerpTime * 0.5f, frontLightColor, currentTimeOverMax, 0f, 0.5f, 0.5f, 1f, (float)Math.PI / 4f, new Vector2(2f, 2f), Vector2.One);

			//Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, position, sourceRectangle, Color.Orange * 0.75f, 0f, origin, scale, spriteEffects);

			return false;
		}

		private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
		{
			Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
			Color color = shineColor * opacity * 0.5f;
			color.A = 0;
			Vector2 origin = sparkleTexture.Size() / 2f;
			Color color2 = drawColor * 0.5f;
			float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
			Vector2 vector = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
			Vector2 vector2 = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
			color *= lerpValue;
			color2 *= lerpValue;
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, color, (float)Math.PI / 2f + rotation, origin, vector, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, color, 0f + rotation, origin, vector2, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, color2, (float)Math.PI / 2f + rotation, origin, vector * 0.6f, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, color2, 0f + rotation, origin, vector2 * 0.6f, dir);
		}
	}
}