using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Magic
{
	public class MiniWindGust : ModProjectile
	{
		public override string Texture => "RijamsMod/Projectiles/Enemies/WindGust";
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
		}
		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 5)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame > Main.projFrames[Type])
				{
					Projectile.frame = 5; // Go invisible
				}
			}

			//Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SnowBlock);

			float livingTime = 20f;
			Projectile.localAI[0]++;
			//Projectile.alpha = (int)MathHelper.Lerp(0f, 255f, Projectile.localAI[0] / numIs20f);
			//Projectile.alpha = 255;
			int ai0 = (int)Projectile.ai[0];
			int parentProjType = ModContent.ProjectileType<MiniGusterProj>();

			if (Projectile.localAI[0] >= livingTime || ai0 < 0 || ai0 > 1000 || !Main.projectile[ai0].active || Main.projectile[ai0].type != parentProjType)
			{
				Projectile.Kill();
				return;
			}
			Projectile.Center = Main.projectile[ai0].Center - Projectile.velocity;

			Projectile.rotation = Projectile.velocity.ToRotation();

			Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
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
	}
}