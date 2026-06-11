using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class BeltProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// This makes the projectile use whip collision detection and allows flasks to be applied to it.
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.drawLayer = ProjectileDrawLayerID.HeldProj;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.SummonMeleeSpeed;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true; // This prevents the projectile from hitting through solid tiles.
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.WhipSettings.Segments = 20;
			Projectile.WhipSettings.RangeMultiplier = 0.5f;
		}

		private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2; // Without PiOver2, the rotation would be off by 90 degrees counterclockwise.

			Projectile.Center = Main.GetPlayerArmPosition(Projectile, owner) + Projectile.velocity * Timer;
			// Vanilla uses Vector2.Dot(Projectile.velocity, Vector2.UnitX) here. Dot Product returns the difference between two vectors, 0 meaning they are perpendicular.
			// However, the use of UnitX basically turns it into a more complicated way of checking if the projectile's velocity is above or equal to zero on the X axis.
			Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;

			Timer++; // make sure you keep this line if you remove the charging mechanic.


			Projectile.GetWhipSettings(Projectile, out float swingTime, out _, out _);

			if (Timer >= swingTime || owner.itemAnimation <= 1) // itemAnimation <= 1 allows quick swapping between whips
			{
				owner.itemAnimation = 0; // allows quick swapping between whips
				Projectile.Kill();
				return;
			}

			owner.heldProj = Projectile.whoAmI;
			owner.MatchItemTimeToItemAnimation();

			if (Timer == swingTime / 2)
			{
				// Plays a whipcrack sound at the tip of the whip.
				List<Vector2> points = Projectile.WhipPointsForCollision;
				Projectile.FillWhipControlPoints(Projectile, points, owner);
				SoundEngine.PlaySound(SoundID.Item153, points[^1]);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (int)(Projectile.damage * 0.5f);
		}

		public override bool PreDraw(Player player, ref Color lightColor)
		{
			List<Vector2> controlPoints = new();
			Projectile.FillWhipControlPoints(Projectile, controlPoints, player);

			RijamsModProjectile.DrawLineForWhips(controlPoints, Color.Brown);

			//Main.DrawWhip_WhipBland(Projectile, controlPoints);
			// The code below is for custom drawing.
			// If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
			// However, you must adhere to how they draw if you do.

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			int totalSegments = Projectile.WhipSettings.Segments; // The number of segments this whip has.

			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 pos = controlPoints[0];

			for (int i = 0; i < controlPoints.Count - 1; i++)
			{
				// These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
				// You can change them if they don't!
				Rectangle frame = new(0, 0, 10, 26); // Handle
				Vector2 origin = new(5, 8);
				float scale = 1;

				// These statements determine what part of the spritesheet to draw for the current segment.
				// They can also be changed to suit your sprite.
				if (i == controlPoints.Count - 2)
				{
					// This is the head of the whip. You need to measure the sprite to figure out these values.
					frame.Y = 74;
					frame.Height = 18;

					// For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
					Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
					float t = Timer / timeToFlyOut;
					scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
				}
				// Divide the middle of the whip (after the handle and before the head) by approximately 3 and use the middle segments in each third.
				else if (i > 2 * (totalSegments / 3)) // At 2/3 of the way across the whip, the third segment is used.
				{
					// Third segment
					frame.Y = 58;
					frame.Height = 16;
				}
				else if (i > totalSegments / 3) // At 1/3 of the way across the whip, the second segment is used.
				{
					// Second Segment
					frame.Y = 42;
					frame.Height = 16;
				}
				else // At the start of the whip after the handle, the first segment is used.
				{
					// First Segment
					frame.Y = 26;
					frame.Height = 16;
				}

				Vector2 element = controlPoints[i];
				Vector2 diff = controlPoints[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;
			}
			return false;
		}
	}
}