using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class WhipProjBase : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(WhipProjBase);
		public override string Texture => Projectile.type == ModContent.ProjectileType<WhipProjBase>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');


		public virtual bool OverrideDustSpawning(float swingProgress, float dustChance)
		{
			return false;
		}
		public virtual int DustType()
		{
			return 0;
		}
		public virtual SoundStyle WhipCrackSound()
		{
			return SoundID.Item153;
		}
		public virtual float MultiHitPenatly()
		{
			return 1f;
		}
		public virtual WhipDrawData DrawPositions(ref bool decrementLineCount, ref Color lineColor)
		{
			Rectangle handle = new(0, 0, 10, 26);
			Vector2 handleOffset = new(5, 8);
			WhipDrawData.SegmentData head = new(72, 18);
			WhipDrawData.SegmentData third = new(58, 18);
			WhipDrawData.SegmentData second = new(42, 18);
			WhipDrawData.SegmentData first = new(26, 18);
			return new WhipDrawData(handle, handleOffset, head, third, second, first);
		}

		public virtual void InsertDrawingAfterFrameIsPicked(int i, List<Vector2> controlPoints, ref float scale)
		{

		}

		public virtual Color DrawColor(int i, Vector2 currentElement, List<Vector2> controlPoints)
		{
			return Lighting.GetColor(currentElement.ToTileCoordinates());
		}

		public virtual bool OverrideDrawing(Asset<Texture2D> texture, WhipDrawData whipDrawData, ref Vector2 pos, ref List<Vector2> controlPoints, int totalSegments, SpriteEffects flip)
		{
			return false;
		}

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
			Projectile.WhipSettings.RangeMultiplier = 1f;
		}

		internal float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2; // Without PiOver2, the rotation would be off by 90 degrees counterclockwise.

			Projectile.Center = owner.GetArmPosition() + Projectile.velocity * Timer;
			// Vanilla uses Vector2.Dot(Projectile.velocity, Vector2.UnitX) here. Dot Product returns the difference between two vectors, 0 meaning they are perpendicular.
			// However, the use of UnitX basically turns it into a more complicated way of checking if the projectile's velocity is above or equal to zero on the X axis.
			Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;

			Timer++; // make sure you keep this line if you remove the charging mechanic.

			Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out _, out _);

			if (Timer >= timeToFlyOut || owner.itemAnimation <= 1) // itemAnimation <= 1 allows quick swapping between whips
			{
				owner.itemAnimation = 0; // allows quick swapping between whips
				Projectile.Kill();
				return;
			}

			owner.heldProj = Projectile.whoAmI;
			owner.MatchItemTimeToItemAnimation();

			if (Timer == timeToFlyOut / 2)
			{
				// Plays a whipcrack sound at the tip of the whip.
				List<Vector2> points = Projectile.WhipPointsForCollision;
				Projectile.FillWhipControlPoints(Projectile, points);
				SoundEngine.PlaySound(WhipCrackSound(), points[^1]);
			}

			float swingProgress = Timer / timeToFlyOut;
			float dustChance = Utils.GetLerpValue(0.1f, 0.7f, swingProgress, clamped: true) * Utils.GetLerpValue(0.9f, 0.7f, swingProgress, clamped: true);
			if (!OverrideDustSpawning(swingProgress, dustChance) && dustChance > 0.1f && Main.rand.NextFloat() < dustChance / 2f)
			{
				Projectile.WhipPointsForCollision.Clear();
				Projectile.FillWhipControlPoints(Projectile, Projectile.WhipPointsForCollision);
				Rectangle spawnArea = Utils.CenteredRectangle(Projectile.WhipPointsForCollision[^1], new Vector2(30f, 30f));
				Dust dust = Dust.NewDustDirect(spawnArea.TopLeft(), spawnArea.Width, spawnArea.Height, DustType(), 0f, 0f, 100, default, 1.0f);
				dust.noGravity = true;
				dust.velocity.X /= 2f;
				dust.velocity.Y /= 2f;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (int)(Projectile.damage * MultiHitPenatly());
		}

		public static void DrawLineForWhips(List<Vector2> controlPoints, Color lineColor, bool decrementCount = false)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new(frame.Width / 2, 2);

			Vector2 pos = controlPoints[0];
			int maxCount = controlPoints.Count - 1;
			if (decrementCount)
			{
				maxCount--;
			}
			// If you whip has a long range and this line is poking out of the front, use list.Count - 2 instead of list.Count - 1.
			for (int i = 0; i < maxCount; i++)
			{
				Vector2 element = controlPoints[i];
				Vector2 diff = controlPoints[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates(), lineColor);
				Vector2 scale = new(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}

		public override bool PreDraw(Player player, ref Color lightColor)
		{
			List<Vector2> controlPoints = new();
			Projectile.FillWhipControlPoints(Projectile, controlPoints, player);

			bool decrementLineCount = false;
			Color lineColor = Color.White;
			WhipDrawData whipDrawData = DrawPositions(ref decrementLineCount, ref lineColor);

			DrawLineForWhips(controlPoints, lineColor, decrementLineCount);

			//Main.DrawWhip_WhipBland(Projectile, list);
			// The code below is for custom drawing.
			// If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
			// However, you must adhere to how they draw if you do.

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			int totalSegments = Projectile.WhipSettings.Segments; // The number of segments this whip has.

			Asset<Texture2D> texture = TextureAssets.Projectile[Type];

			Vector2 pos = controlPoints[0];

			if (OverrideDrawing(texture, whipDrawData, ref pos, ref controlPoints, totalSegments, flip))
			{
				return false;
			}

			for (int i = 0; i < controlPoints.Count - 1; i++)
			{
				// These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
				// You can change them if they don't!
				Rectangle frame = whipDrawData.HandleFrame; // Handle
				Vector2 origin = whipDrawData.HandleOffset;
				float scale = 1;

				// These statements determine what part of the spritesheet to draw for the current segment.
				// They can also be changed to suit your sprite.
				if (i == controlPoints.Count - 2)
				{
					// This is the head of the whip. You need to measure the sprite to figure out these values.
					frame.Y = whipDrawData.HeadSegment.Y;
					frame.Height = whipDrawData.HeadSegment.Height;

					// For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
					Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
					float t = Timer / timeToFlyOut;
					scale = MathHelper.Lerp(0.7f, 1.2f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
				}
				// Divide the middle of the whip (after the handle and before the head) by approximately 3 and use the middle segments in each third.
				else if (i > 2 * (totalSegments / 3)) // At 2/3 of the way across the whip, the third segment is used.
				{
					// Third segment
					frame.Y = whipDrawData.ThirdSegment.Y;
					frame.Height = whipDrawData.ThirdSegment.Height;
				}
				else if (i > totalSegments / 3) // At 1/3 of the way across the whip, the second segment is used.
				{
					// Second Segment
					frame.Y = whipDrawData.SecondSegment.Y;
					frame.Height = whipDrawData.SecondSegment.Height;
				}
				else if (i != 0) // At the start of the whip after the handle, the first segment is used.
				{
					// First Segment
					frame.Y = whipDrawData.FirstSegment.Y;
					frame.Height = whipDrawData.FirstSegment.Height;
				}

				InsertDrawingAfterFrameIsPicked(i, controlPoints, ref scale);

				Vector2 element = controlPoints[i];
				Vector2 diff = controlPoints[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
				Color color = DrawColor(i, element, controlPoints);

				Main.EntitySpriteDraw(texture.Value, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;
			}
			return false;
		}

		/*
		// This hook lets us change how the held projectile looks while a mannequin is holding it.
		// The following code is adapted from vanilla's Projectile.AI_DisplayDoll for aiStyle 165 (Whip)
		public override bool DisplayDollSettings(Player doll, TEDisplayDoll.DisplayDollPose pose, , ref int aiStyle)
		{
			Timer = 12f; // How far into the animation the whip is. 12 matches vanilla and is just starting the swing. 71.265 is straight ahead for ExampleWhipProjectile. 
			Projectile.ai[1] = 0.4f; // How curled the whip is. 0.4f matches vanilla. 0 for no curl.

			// Note: Projectile.GetWhipSettings() in the PreDraw code will always return 60 for the timeToFlyOut if the projectile isAPreviewDisplayDoll.
			// This means the head of the whip will be scaled down, if your code does that.

			// Unlike other DisplayDoll code, this doesn't shift the position of the projectile.
			// Instead it sets the velocity which determines how stretched out the whip is (in addition to what Timer does). 1f matches vanilla.
			Vector2 projectileVelocity = Vector2.UnitX * 1f;
			float armRotation = 0f;
			if (pose.ItemAimRadians.HasValue)
				armRotation = pose.ItemAimRadians.Value; // The rotation of the mannequin's hand.

			projectileVelocity = projectileVelocity.RotatedBy(armRotation); // Rotate the projectile based on the rotation of the mannequin's hand.
			if (Projectile.direction == -1)
				projectileVelocity.X *= -1f;

			Projectile.velocity = projectileVelocity; // The velocity of the whip affects how stretched out it will be.
			Projectile.Center = Main.GetPlayerArmPosition(Projectile, doll) + Projectile.velocity * (Timer - 1f); // Set the projectile to be in the mannequin's hand.
			Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt(); // Set the direction to the direction the whip is facing.

			return false;
		}
		*/

		// This hook lets us change how the held projectile looks while a mannequin is holding it.
		// We set aiStyle to Whip to draw the projectile as if it had the vanilla whip aiStyle.
		public override bool DisplayDollSettings(Player doll, TEDisplayDoll.DisplayDollPose pose, ref int aiStyle, ref int aiType)
		{
			aiStyle = ProjAIStyleID.Whip;
			return true;
		}

		public struct WhipDrawData(Rectangle handleFrame, Vector2 handOffset, WhipDrawData.SegmentData headSegment, WhipDrawData.SegmentData thirdSegment, WhipDrawData.SegmentData secondSegment, WhipDrawData.SegmentData firstSegment)
		{
			public Rectangle HandleFrame = handleFrame;
			public Vector2 HandleOffset = handOffset;
			public SegmentData HeadSegment = headSegment;
			public SegmentData ThirdSegment = thirdSegment;
			public SegmentData SecondSegment = secondSegment;
			public SegmentData FirstSegment = firstSegment;

			public struct SegmentData(int y, int height)
			{
				public int Y = y;
				public int Height = height;
			}
		}
	}
}