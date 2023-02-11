using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace RijamsMod.Projectiles.Ranged
{
	public class InterstellarLaser : ModProjectile
	{
		public Color drawColor = Color.Transparent;
		public bool homing = false;
		public int homingDetectionRange = 5;
		public bool bounceOnTiles = false;
		public bool bounceOnNPCs = false;
		public int inflictBuff = -1;
		public int buffTime = 0;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Interstellar Laser");     //The English name of the projectile
			//ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;    //The length of old position to be recorded
			//ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.alpha = 0;
			Projectile.timeLeft = 600; //10 seconds
			Projectile.extraUpdates = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			//Projectile.usesIDStaticNPCImmunity = true;
			//Projectile.idStaticNPCHitCooldown = 1;

			DrawOriginOffsetY = -16;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//If collide with tile, reduce the penetrate.
			//So the projectile can reflect at most 5 times
			if (bounceOnTiles)
			{
				Projectile.penetrate--;
				if (Projectile.penetrate <= 0)
				{
					return true;
				}
				else
				{
					Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
					SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
					if (Projectile.velocity.X != oldVelocity.X)
					{
						Projectile.velocity.X = -oldVelocity.X;
					}
					if (Projectile.velocity.Y != oldVelocity.Y)
					{
						Projectile.velocity.Y = -oldVelocity.Y;
					}
				}
				return false;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (inflictBuff > 0)
			{
				target.AddBuff(inflictBuff, buffTime);
			}
			if (bounceOnNPCs)
			{
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				Projectile.velocity *= -1;
			}
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			if (inflictBuff > 0)
			{
				target.AddBuff(inflictBuff, buffTime);
			}
			if (bounceOnNPCs)
			{
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				Projectile.velocity *= -1;
			}
		}

		public override void AI()
		{
			//Projectile.velocity = Vector2.Zero;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(Projectile.Center, drawColor.ToVector3() * 0.2f);
			Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.AuraDust>(), Projectile.velocity, 0, drawColor * 0.5f, 1f);
			
			if (homing)
			{
				int newTarget = Projectile.FindTargetWithLineOfSight(16 * homingDetectionRange); // homingDetectionRange = 5 means 5 block check
				if (newTarget != -1) // Turn towards the target
				{
					Projectile.oldVelocity = Projectile.velocity;
					NPC nPC2 = Main.npc[newTarget];
					Vector2 newVelocity = Projectile.DirectionTo(nPC2.Center).SafeNormalize(-Vector2.UnitY) * Projectile.velocity.Length();
					//Main.NewText("newVelocity " + newVelocity.ToRotation() + " oldVelocity " + Projectile.oldVelocity.ToRotation());
					//Main.NewText("Rotation " + (Math.Abs(newVelocity.ToRotation() - Projectile.oldVelocity.ToRotation())));
					Projectile.velocity = newVelocity;
					Projectile.netUpdate = true;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Player owner = Main.player[Projectile.owner];

			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = ((Projectile.spriteDirection <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			if (owner.gravDir == -1f)
			{
				spriteEffects |= SpriteEffects.FlipVertically;
			}

			// Get texture of projectile
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Texture2D texture2 = ModContent.Request<Texture2D>(Mod.Name + "/Projectiles/Ranged/" + Name + "_Opaque").Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

			// The origin 
			Vector2 origin = new(sourceRectangle.Size().X / 2f, sourceRectangle.Size().Y / 2f + DrawOriginOffsetY);

			// The rotation of the projectile.
			float rotation = Projectile.rotation;

			// The position of the sprite. Not subtracting Main.player[Projectile.owner].gfxOffY will cause the sprite to bounce when walking up blocks.
			Vector2 position = new(Projectile.Center.X, Projectile.Center.Y - Main.player[Projectile.owner].gfxOffY);

			/*
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				//Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}*/

			Main.EntitySpriteDraw(texture2,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, rotation, origin, Projectile.scale, spriteEffects, 0);

			/*Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				Projectile.Hitbox, Color.Magenta * 0.25f, 0, Projectile.Hitbox.Size() / 2, Projectile.scale, SpriteEffects.None, 0);*/

			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<Dusts.AuraDust>(), 0, 0, 0, drawColor, 1f);
			}
			SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.position);
		}
	}
}