using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;

namespace RijamsMod.Projectiles.Melee.Clubs
{
	// Shortsword projectiles are handled in a special way with how they draw and damage things
	// The "hitbox" itself is closer to the player, the sprite is centered on it
	// However the interactions with the world will occur offset from this hitbox, closer to the sword's tip (CutTiles, Colliding)
	// Values chosen mostly correspond to Iron Shortword
	public class TestClubSwingProj : ModProjectile
	{
		public bool useTurn;
		public int firingAnimation;
		public int firingTime;

		public int Timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		public int Timer2
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Test Club");
			Main.projFrames[Projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(22); // This sets width and height to the same value (important when projectiles can rotate)
			Projectile.aiStyle = -1; // Use our own AI to customize how it behaves, if you don't want that, keep this at ProjAIStyleID.ShortSword. You would still need to use the code in SetVisualOffsets() though
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true; // Prevents hits through tiles. Most melee weapons that use projectiles have this
			Projectile.extraUpdates = 1; // Update 1+extraUpdates times per tick
			Projectile.timeLeft = 360; // This value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
			Projectile.hide = false; // Important when used alongside player.heldProj. "Hidden" projectiles have special draw conditions
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			// Main.NewText("Timer " + Timer);
			// Main.NewText("Projectile.Center " + Projectile.Center);
			// Main.NewText("Timer2 " + Timer2);
			// Main.NewText("Velocity " + Projectile.velocity);
			/*if (Timer == 0)
			{
				Main.NewText("firingAnimation " + firingAnimation);
			}*/

			if (useTurn) // Still wouldn't recommend using useTurn
			{
				Projectile.direction = player.direction;
			}

			Timer += 1;
			if (Timer >= firingAnimation)
			{
				// Kill the projectile if it reaches it's intended lifetime
				Projectile.Kill();
				return;
			}
			else
			{
				// Important so that the sprite draws "in" the player's hand and not fully infront or behind the player
				player.heldProj = Projectile.whoAmI;
			}

			Projectile.Center = CalcFirstPoint(player);
			float velocityRotation = Projectile.velocity.ToRotation();
			// Main.NewText("velocityRotation " + velocityRotation * 57.2957795);
			// Main.NewText("direction " + Projectile.direction);
			float rotateMin = -30f;
			float rotateMax = 30f;
			if (Projectile.direction < 0)
			{
				// Shot to the left.
				// Top is -90
				// Left is -180/180
				// Bottom is 90

				if (velocityRotation < 0) // Up
				{
					Projectile.rotation = Math.Clamp(velocityRotation, MathHelper.ToRadians(-180), MathHelper.ToRadians(-180 + rotateMax)); //-180 to -150
					Projectile.rotation += MathHelper.ToRadians(180);
				}
				else // Down
				{
					Projectile.rotation = Math.Clamp(velocityRotation, MathHelper.ToRadians(180 + rotateMin), MathHelper.ToRadians(180)); // 150 to 180
					Projectile.rotation += MathHelper.ToRadians(-180);
				}
			}
			else
			{
				// Shot to the right.
				// Top is -90
				// Right is 0
				// Bottom is 90
				Projectile.rotation = Math.Clamp(velocityRotation, MathHelper.ToRadians(rotateMin), MathHelper.ToRadians(rotateMax));
			}
			// Main.NewText("Projectile.rotation " + Projectile.rotation * 57.2957795);
			
			
			Projectile.spriteDirection = Projectile.direction;

			// Bug? Can miss targets between points when melee speed is super high.
			if (Timer < firingAnimation / 3)
			{
				
				Projectile.frame = 0;
				Timer2++;
				Vector2 center = Projectile.Center;
				center.X = MathHelper.Lerp(CalcFirstPoint(player).X, CalcSecondPoint(player).X, Timer2 / (firingTime / 3));
				center.Y = MathHelper.Lerp(CalcFirstPoint(player).Y, CalcSecondPoint(player).Y, Timer2 / (firingTime / 3));
				// Main.NewText("Timer2 out of given time: " + Timer2 + " " + (firingTime / 3));
				Projectile.Center = center;
			}
			else if (Timer == firingAnimation / 3)
			{
				Timer2 = 0;
				Projectile.Center = CalcSecondPoint(player);
				// Main.NewText("----");
			}
			if (Timer > firingAnimation / 3 && Timer < firingAnimation / 2)
			{
				Projectile.frame = 1;
				Timer2++;
				Vector2 center = Projectile.Center;
				center.X = MathHelper.Lerp(CalcSecondPoint(player).X, CalcThirdPoint(player).X, Timer2 / (firingTime / 5));
				center.Y = MathHelper.Lerp(CalcSecondPoint(player).Y, CalcThirdPoint(player).Y, Timer2 / (firingTime / 5));
				// Main.NewText("Timer2 out of given time: " + Timer2 + " " + (firingTime / 5));
				Projectile.Center = center;
			}
			else if (Timer == firingAnimation / 2)
			{
				Timer2 = 0;
				Projectile.Center = CalcThirdPoint(player);
				// Main.NewText("----");
			}
			if (Timer > firingAnimation / 2 && Timer < (firingAnimation * 4 / 5))
			{
				Projectile.frame = 2;
				Timer2++;
				Vector2 center = Projectile.Center;
				center.X = MathHelper.Lerp(CalcThirdPoint(player).X, CalcFourthPoint(player).X, Timer2 / (firingTime / 3));
				center.Y = MathHelper.Lerp(CalcThirdPoint(player).Y, CalcFourthPoint(player).Y, Timer2 / (firingTime / 3));
				// Main.NewText("Timer2 out of given time: " + Timer2 + " " + (firingTime / 3));
				Projectile.Center = center;
			}
			else if (Timer > firingAnimation * 4 / 5)
			{
				Timer2 = 0;
				Projectile.Center = CalcFourthPoint(player);
			}
		}

		private Vector2 CalcFirstPoint(Player player)
		{
			Vector2 point = player.Center + new Vector2(5 * player.direction, 20);
			Vector2 origin = player.Center;
			return Vector2.Transform(point - origin, Matrix.CreateRotationZ(Projectile.rotation)) + origin;
		}
		private Vector2 CalcSecondPoint(Player player)
		{
			Vector2 point = player.Center + new Vector2(4 * 16 * player.direction, 15);
			Vector2 origin = player.Center;
			return Vector2.Transform(point - origin, Matrix.CreateRotationZ(Projectile.rotation)) + origin;
		}
		private Vector2 CalcThirdPoint(Player player)
		{
			Vector2 point = player.Center + new Vector2(6 * 16 * player.direction, -2);
			Vector2 origin = player.Center;
			return Vector2.Transform(point - origin, Matrix.CreateRotationZ(Projectile.rotation)) + origin;
		}
		private Vector2 CalcFourthPoint(Player player)
		{
			Vector2 point = player.Center + new Vector2(3.15f * 16 * player.direction, -3.75f * 16);
			Vector2 origin = player.Center;
			return Vector2.Transform(point - origin, Matrix.CreateRotationZ(Projectile.rotation)) + origin;
		}


		public override bool ShouldUpdatePosition() => false;

		public override bool? CanCutTiles() => true;

		// We need to draw the projectile manually. If you don't include this, the Jousting Lance will not be aligned with the player.
		public override bool PreDraw(ref Color lightColor)
		{
			bool drawExtra = false;

			Player owner = Main.player[Projectile.owner];

			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = ((Projectile.spriteDirection <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

			// Get texture of projectile
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Texture2D texture2 = ModContent.Request<Texture2D>(Mod.Name + "/Projectiles/Melee/Clubs/" + Name + "Background").Value;
			Texture2D texture3 = ModContent.Request<Texture2D>(Mod.Name + "/Projectiles/Melee/Clubs/" + Name + "SwooshUp").Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

			// The origin in this case is (0, 0) of our projectile because Projectile.Center is the tip of our Jousting Lance.
			Vector2 origin = sourceRectangle.Size() / 2f;

			// The rotation of the projectile.
			float rotation = Projectile.rotation;

			// The position of the sprite. Not subtracting Main.player[Projectile.owner].gfxOffY will cause the sprite to bounce when walking up blocks.
			Vector2 position = new(owner.Center.X, owner.Center.Y - Main.player[Projectile.owner].gfxOffY);

			// Apply lighting and draw our projectile
			Color drawColor = lightColor;

			if (drawExtra)
			{
				Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value,
				CalcFirstPoint(owner) - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				Projectile.Hitbox, Color.Magenta * 0.25f, 0, Projectile.Hitbox.Size() / 2, Projectile.scale, spriteEffects, 0);

				Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value,
					CalcSecondPoint(owner) - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
					Projectile.Hitbox, Color.Magenta * 0.25f, 0, Projectile.Hitbox.Size() / 2, Projectile.scale, spriteEffects, 0);

				Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value,
					CalcThirdPoint(owner) - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
					Projectile.Hitbox, Color.Magenta * 0.25f, 0, Projectile.Hitbox.Size() / 2, Projectile.scale, spriteEffects, 0);

				Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value,
					CalcFourthPoint(owner) - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
					Projectile.Hitbox, Color.Magenta * 0.25f, 0, Projectile.Hitbox.Size() / 2, Projectile.scale, spriteEffects, 0);

				Main.EntitySpriteDraw(texture2,
					position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
					sourceRectangle, drawColor, rotation, origin, Projectile.scale, spriteEffects, 0);
			}

			Main.EntitySpriteDraw(texture,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, rotation, origin, Projectile.scale, spriteEffects, 0);

			Main.EntitySpriteDraw(texture3,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, rotation, origin, Projectile.scale, spriteEffects, 0);

			if (drawExtra)
			{
				Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				Projectile.Hitbox, Color.Orange * 0.5f, 0, Projectile.Hitbox.Size() / 2, Projectile.scale, spriteEffects, 0);
			}
			
			// It's important to return false, otherwise we also draw the original texture.
			return false;
		}
	}
}