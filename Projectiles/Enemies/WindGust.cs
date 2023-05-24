using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Enemies
{
	public class WindGust : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.hostile = true;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 20;
			Projectile.knockBack = 16f;
			Projectile.scale = 1.2f;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.frameCounter++;
			if (Projectile.frameCounter == 5)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float rotationFactor = Projectile.rotation; // The rotation of the Jousting Lance.
			float scaleFactor = 150f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
			float widthMultiplier = 50f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
			float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

			// This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
			// You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
			// Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
			// The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
			Rectangle lanceHitboxBounds = new(0, 0, 400, 400);

			// Set the position of the large rectangle.
			lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
			lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

			// This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
			Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * scaleFactor;

			// The following is for debugging the size of the hit line. This will allow you to easily see where it starts and ends.
			//Dust.NewDustPerfect(Projectile.Center, DustID.Pixie, Velocity: Vector2.Zero, Scale: 0.5f);
			//Dust.NewDustPerfect(hitLineEnd, DustID.Torch, Velocity: Vector2.Zero, Scale: 0.5f);

			// First check that our large rectangle intersects with the target hitbox.
			// Then we check to see if a line from the tip of the Jousting Lance to the "end" of the lance intersects with the target hitbox.
			if (lanceHitboxBounds.Intersects(targetHitbox)
				&& Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
			{
				return true;
			}

			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;

			// Get texture of projectile
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

			// The origin in this case is (0, 0) of our projectile because Projectile.Center is the tip of our Jousting Lance.
			Vector2 origin = sourceRectangle.Size() - new Vector2(sourceRectangle.Width, sourceRectangle.Height / 2f);

			// The rotation of the projectile.
			float rotation = Projectile.rotation;

			// If the projectile is facing right, we need to rotate it by -90 degrees, move the origin, and flip the sprite horizontally.
			// This will make it so the bottom of the sprite is correctly facing down when shot to the right.

			// The position of the sprite. Not subtracting Main.player[Projectile.owner].gfxOffY will cause the sprite to bounce when walking up blocks.
			Vector2 position = Projectile.Center;

			// Apply lighting and draw our projectile
			Color drawColor = new(255, 255, 255, 0);

			Main.EntitySpriteDraw(texture,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, rotation, origin, Projectile.scale, spriteEffects, 0);

			return false;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (Main.expertMode)
			{
				target.AddBuff(BuffID.Chilled, 120);
			}
			//target.fallStart -= 100;
			//target.fallStart2 -= 100;
			//Main.NewText(target.position + " " + target.fallStart + " " + target.fallStart2);
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			if (Condition.ForTheWorthyWorld.IsMet())
			{
				modifiers.KnockbackImmunityEffectiveness *= 0.5f;
				modifiers.Knockback.Base += 5f;
				modifiers.Knockback *= 2f;
			}
			else
			{
				modifiers.KnockbackImmunityEffectiveness *= 0.8f;
				modifiers.Knockback.Base += 2f;
				modifiers.Knockback *= 2f;
			}
		}
	}
}