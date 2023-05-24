using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Pets
{
    public class BabyStardustDragon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 3;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 51;
		}

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.DestroyerPet);
            Projectile.aiStyle = -1;
		}

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
            if (player.dead)
            {
                modPlayer.babyStardustDragonPet = false;
            }
            if (modPlayer.babyStardustDragonPet)
            {
                Projectile.timeLeft = 2;
            }

			Vector2 center = player.Center;
			if (Projectile.Distance(center) > 2000f)
			{
				Projectile.Center = center;
				Projectile.velocity = Vector2.Zero;
				Projectile.netUpdate = true;
			}
			float distFromPlayer = (center - Projectile.Center).Length();
			float minVelocity = Math.Min(12f, Math.Max(4f, player.velocity.Length()));
			Projectile.velocity.Length();
			if (Projectile.velocity == Vector2.Zero)
			{
				Projectile.velocity.X = 2f * (float)player.direction;
				Vector2 position = Projectile.position;
				for (int i = 0; i < Projectile.oldPos.Length; i++)
				{
					position -= Projectile.velocity;
					Projectile.oldPos[i] = position;
				}
			}
			if (!(distFromPlayer < 120f))
			{
				float targetAngle = Projectile.AngleTo(center);
				float f = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(5f));
				Projectile.velocity = f.ToRotationVector2() * minVelocity;
			}
			if (Projectile.velocity.Length() > minVelocity)
			{
				Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * minVelocity;
			}
			if (Math.Abs(Projectile.velocity.Y) < 1f)
			{
				Projectile.velocity.Y -= 0.1f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
			int direction = Projectile.direction;
			Projectile.direction = (Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? 1 : (-1)));
			if (direction != Projectile.direction)
			{
				Projectile.netUpdate = true;
			}
			Projectile.position.X = MathHelper.Clamp(Projectile.position.X, 160f, Main.maxTilesX * 16 - 160);
			Projectile.position.Y = MathHelper.Clamp(Projectile.position.Y, 160f, Main.maxTilesY * 16 - 160);

			if (Main.GameUpdateCount % 5 == 0)
			{
				for (int i = 0; i < Main.rand.Next(0, 2 + (int)Projectile.velocity.Length()); i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, 1, 1, DustID.DungeonSpirit, 0, 0, 50, default, 1.5f);
					dust.noGravity = true;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int numOfSegments = 6; // Also change the TrailCacheLength
			int spaceBetweenSegments = 16;
			SpriteEffects effects = ((Projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Rectangle rectangle = texture.Frame(1, Main.projFrames[Projectile.type]);
			Vector2 origin = rectangle.Size() / 2f;
			Vector2 position = Projectile.Center - Main.screenPosition;
			//Color headColor = Projectile.GetAlpha(Lighting.GetColor(Projectile.Center.ToTileCoordinates()));
			Color headColor = Color.White;
			headColor.A = 150;
			Vector2 value2 = Projectile.Center;
			int numFrameY = 1;
			int numOfFrames = Main.projFrames[Projectile.type] - 1;
			for (int i = 1; i < numOfSegments; i++)
			{
				int frameY = numFrameY;
				if (i == numOfSegments - 1)
				{
					frameY = numOfFrames;
				}
				Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], 0, frameY);
				 Vector2 currentSegment = Projectile.oldPos[i * 10] + Projectile.Size / 2f;
				float rotation = (value2 - currentSegment).ToRotation();
				currentSegment = value2 - new Vector2(i == numOfSegments - 1 ? spaceBetweenSegments + 6 : spaceBetweenSegments, 0f).RotatedBy(rotation, Vector2.Zero);
				rotation = (value2 - currentSegment).ToRotation() + (float)Math.PI / 2f;
				Vector2 position2 = currentSegment - Main.screenPosition;
				SpriteEffects effects2 = ((!(currentSegment.X < value2.X)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				value2 = currentSegment;
				//Color segmentColor = Projectile.GetAlpha(Lighting.GetColor(currentSegment.ToTileCoordinates()));
				Color segmentColor = Color.White;
				segmentColor.A = 150;
				Main.EntitySpriteDraw(texture, position2, sourceRectangle, segmentColor, rotation, origin, Projectile.scale, effects2, 0);
			}
			Main.EntitySpriteDraw(texture, position, rectangle, headColor, Projectile.rotation, origin, Projectile.scale, effects, 0);
			return false;
		}
	}
}