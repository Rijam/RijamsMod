using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Whips
{
	public class SupernovaWhipProj : ModProjectile
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
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.SummonMeleeSpeed;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = false; // This prevents the projectile from hitting through solid tiles.
			Projectile.extraUpdates = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.WhipSettings.Segments = 20;
			Projectile.WhipSettings.RangeMultiplier = 3f;
		}

		private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private float Timer2
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2; // Without PiOver2, the rotation would be off by 90 degrees counterclockwise.

			Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * Timer;
			// Vanilla uses Vector2.Dot(Projectile.velocity, Vector2.UnitX) here. Dot Product returns the difference between two vectors, 0 meaning they are perpendicular.
			// However, the use of UnitX basically turns it into a more complicated way of checking if the projectile's velocity is above or equal to zero on the X axis.
			Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;

			Timer2 = 1; // Each segment can only spawn one Stardust Explosion
			Timer++; // make sure you keep this line if you remove the charging mechanic.

			float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;

			if (Timer >= swingTime || owner.itemAnimation <= 1) // itemAnimation <= 1 allows quick swapping between whips
			{
				owner.itemAnimation = 0; // allows quick swapping between whips
				Projectile.Kill();
				return;
			}

			owner.heldProj = Projectile.whoAmI;

			// If the swingTime is an odd number, the whipcrack sound will not play.
			float evenSwingTime = swingTime;
			if (swingTime % 2 == 1)
			{
				evenSwingTime -= 1;
			}
			if (Timer == evenSwingTime / 2)
			{
				// Plays a whipcrack sound at the tip of the whip.
				List<Vector2> points = Projectile.WhipPointsForCollision;
				Projectile.FillWhipControlPoints(Projectile, points);
				SoundEngine.PlaySound(SoundID.Item153, points[^1]);
			}

			float t3 = Timer / swingTime;
			float num5 = Utils.GetLerpValue(0.1f, 0.7f, t3, clamped: true) * Utils.GetLerpValue(0.9f, 0.7f, t3, clamped: true);
			if (num5 > 0.1f && Main.rand.NextFloat() < num5 / 2f)
			{
				Projectile.WhipPointsForCollision.Clear();
				Projectile.FillWhipControlPoints(Projectile, Projectile.WhipPointsForCollision);
				Rectangle r4 = Utils.CenteredRectangle(Projectile.WhipPointsForCollision[^1], new Vector2(30f, 30f));
				Lighting.AddLight(r4.Center(), Color.Yellow.ToVector3() * 0.2f);
				int selectRand = Utils.SelectRandom(Main.rand, DustID.YellowTorch, DustID.BlueTorch);
				for (int i = 0; i < 5; i++)
				{
					int num6 = Dust.NewDust(r4.TopLeft(), r4.Width, r4.Height, selectRand, 0f, 0f, 0, Color.White, 2f);
					Main.dust[num6].noGravity = true;
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SupernovaWhipDebuff>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (int)(Projectile.damage * 0.95f);

			Player owner = Main.player[Projectile.owner];
			float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;

			//Solar Eruption source
			target.AddBuff(BuffID.StardustMinionBleed, 300);
			if (Projectile.owner == Main.myPlayer)
			{
				if (Projectile.damage > Projectile.originalDamage * 0.5f)
				{
					// Main.NewText("Timer: " + Timer);
					// Main.NewText("swingTime: " + swingTime);
					// Main.NewText("Timer2: " + Timer2);
					if (Timer2 > 0 && Timer < swingTime)
					{
						// Main.NewText("Spawn StardustExplosion");
						Projectile.NewProjectile(Entity.GetSource_FromThis(), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<StardustExplosion>(), Projectile.damage, 0, Projectile.owner);
						Timer2 -= 1;
					}
				}
			}
		}

		// This method draws a line between all points of the whip, in case there's empty space between the sprites.
		private void DrawLine(List<Vector2> list)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new(frame.Width / 2, 2);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count - 1; i++)
			{
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
				float t = Timer / timeToFlyOut;
				Color color = Color.Lerp(Color.Blue, Color.Yellow, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
				Vector2 scale = new(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> list = new();
			Projectile.FillWhipControlPoints(Projectile, list);

			DrawLine(list);

			//Main.DrawWhip_WhipBland(Projectile, list);
			// The code below is for custom drawing.
			// If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
			// However, you must adhere to how they draw if you do.

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.instance.LoadProjectile(Type);
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 pos = list[0];

			for (int i = 0; i < list.Count - 1; i++)
			{
				// These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
				// You can change them if they don't!
				Rectangle frame = new(0, 0, 22, 34); // Handle
				Vector2 origin = new(11, 10);
				float scale = 1;

				// These statements determine what part of the spritesheet to draw for the current segment.
				// They can also be changed to suit your sprite.
				if (i == list.Count - 2)
				{
					// This is the head of the whip. You need to measure the sprite to figure out these values.
					frame.Y = 112;
					frame.Height = 30;

					// For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
					Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
					float t = Timer / timeToFlyOut;
					scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
				}
				else if (i > 10)
				{
					// Third segment
					frame.Y = 86;
					frame.Height = 26;
				}
				else if (i > 5)
				{
					// Second Segment
					frame.Y = 60;
					frame.Height = 26;
				}
				else if (i > 0)
				{
					// First Segment
					frame.Y = 34;
					frame.Height = 26;
				}

				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
				Color color = Color.White;

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;
			}
			return false;
		}
	}
}