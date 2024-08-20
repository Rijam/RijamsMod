using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Items.Weapons.Melee.JoustingLances;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	public class EtherealJoustingLanceProj : JoustingLanceProjBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.Enchanted_Gold;
			dustTypeRare = DustID.UnusedWhiteBluePurple;
			offset = 4;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 145f;
			widthMultiplier = 24f;
			lanceHitboxBounds = new(0, 0, 300, 300);
		}

		public override void ModifyDrawing(ref Color drawColor)
		{
			Color lightColor = drawColor;
			Color minColor = new(127, 127, 127, 0);
			drawColor.R = Math.Max(lightColor.R, minColor.R);
			drawColor.G = Math.Max(lightColor.G, minColor.G);
			drawColor.B = Math.Max(lightColor.B, minColor.B);
			drawColor.A = lightColor.A;
		}

		private readonly float minimumDustVelocity = 6f;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			int itemType = ModContent.ItemType<EtherealJoustingLance>();
			if (target.active)
			{
				Player owner = Main.player[Projectile.owner];
				if (owner.inventory[owner.selectedItem].type == itemType && !target.immortal && !target.SpawnedFromStatue && !NPCID.Sets.CountsAsCritter[target.type])
				{
					// The Hallowed and Shadow Jousting Lance spawn dusts when the player is moving above a certain speed.

					// This Vector2.Dot is the dot product between the projectile's velocity and the player's velocity normalized to be between -1 and 1.
					// What this means in this context is that the speed value will be closer to positive 1 if the player is moving in the same direction as the direction the lance was shot.
					// Example: if the lance is shot up and to the right, the value here will be closer to 1 if the player is also moving up and to the right.
					float movementInLanceDirection = Vector2.Dot(Projectile.velocity.SafeNormalize(Vector2.UnitX * owner.direction), owner.velocity.SafeNormalize(Vector2.UnitX * owner.direction));

					float playerVelocity = owner.velocity.Length();

					if (playerVelocity > minimumDustVelocity + 2f && movementInLanceDirection > 0.8f)
					{
						Projectile lance = SpawnProjectileOnEdgeOfScreenTargettingEnemy(owner, ProjectileID.FairyQueenRangedItemShot, target.whoAmI, Projectile.damage, Projectile.knockBack, 30f);
						lance.tileCollide = false;
						lance.timeLeft = 600;
						lance.penetrate = 1;
						// Using local NPC immunity allows each to strike independently from one another.
						lance.usesIDStaticNPCImmunity = true;
						lance.idStaticNPCHitCooldown = 10;

						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, lance.whoAmI);
						}

						ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.StellarTune, new ParticleOrchestraSettings
						{
							PositionInWorld = target.Center,
							MovementVector = target.velocity
						});
					}
				}
			}
		}

		public static Projectile SpawnProjectileOnEdgeOfScreenTargettingEnemy(Player player, int projectileType, int targetIndex, int damage, float knockback, float speed, int screenFluff = 300)
		{
			Vector2 targetCenter = Main.npc[targetIndex].Center;
			Vector2 position = player.Center;
			int screenOffsetX = Main.rand.Next((Main.LogicCheckScreenWidth / 2) + screenFluff, Main.LogicCheckScreenWidth + screenFluff);
			int screenOffsetY = Main.rand.Next((Main.LogicCheckScreenHeight / 2) + screenFluff, Main.LogicCheckScreenHeight + screenFluff);
			if (Main.rand.NextBool())
			{
				screenOffsetX *= -1;
			}
			if (Main.rand.NextBool())
			{
				screenOffsetY *= -1;
			}
			position += new Vector2(screenOffsetX, screenOffsetY);

			// The same as Projectile.DirectionTo but using the new projectile's position instead of the Jousting Lance's position.
			Vector2 velocity = Vector2.Normalize(targetCenter - position) * speed;
			return Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), position, velocity, projectileType, damage, knockback, player.whoAmI, targetIndex);
		}

		Asset<Texture2D> glowTexture;

		public override void PostDraw(Color lightColor)
		{
			Player owner = Main.player[Projectile.owner];
			float movementInLanceDirection = Vector2.Dot(Projectile.velocity.SafeNormalize(Vector2.UnitX * owner.direction), owner.velocity.SafeNormalize(Vector2.UnitX * owner.direction));

			float playerVelocity = owner.velocity.Length();

			if (playerVelocity > minimumDustVelocity && movementInLanceDirection > 0.8f)
			{
				// SpriteEffects change which direction the sprite is drawn.
				SpriteEffects spriteEffects = SpriteEffects.None;

				// Get texture of projectile
				glowTexture ??= ModContent.Request<Texture2D>("RijamsMod/Projectiles/Melee/JoustingLances/EtherealJoustingLanceProj_Glow");

				// Get the currently selected frame on the texture.
				Rectangle sourceRectangle = glowTexture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

				// The origin in this case is (0, 0) of our projectile because Projectile.Center is the tip of our Jousting Lance.
				Vector2 origin = Vector2.Zero;

				// The rotation of the projectile.
				float rotation = Projectile.rotation;

				// If the projectile is facing right, we need to rotate it by -90 degrees, move the origin, and flip the sprite horizontally.
				// This will make it so the bottom of the sprite is correctly facing down when shot to the right.
				if (Projectile.direction > 0)
				{
					rotation -= (float)Math.PI / 2f;
					origin.X += sourceRectangle.Width;
					spriteEffects = SpriteEffects.FlipHorizontally;
				}

				// The position of the sprite. Not subtracting Main.player[Projectile.owner].gfxOffY will cause the sprite to bounce when walking up blocks.
				Vector2 position = new(Projectile.Center.X, Projectile.Center.Y - Main.player[Projectile.owner].gfxOffY);

				Color glowColor = Color.Gold;
				glowColor.A = 255;

				if (playerVelocity > minimumDustVelocity + 1f)
				{
					glowColor.A = 150;
				}
				if (playerVelocity > minimumDustVelocity + 2f)
				{
					glowColor.A = 50;
				}

				Main.EntitySpriteDraw(glowTexture.Value,
					position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
					sourceRectangle, glowColor, rotation, origin, Projectile.scale, spriteEffects, 0);
			}
		}
	}
}