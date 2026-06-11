using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Minions
{
    public class BabyBloodEel : ModProjectile
    {
		public int NumberOfTimesSummoned
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = (int)value;
		}

		public int WaitBeforeTargettingAgainTime
		{
			get => (int)Projectile.ai[2];
			set => Projectile.ai[2] = (int)value;
		}

		public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 3;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionTargetingFeature[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Type] = 96; // 24 slots before it breaks in multiplayer
			ProjectileID.Sets.MinionSacrificable[Type] = true;
		}

        public override void SetDefaults()
        {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
			Projectile.minion = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.timeLeft = Projectile.SentryLifeTime;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}

		public override void OnSpawn(IEntitySource source)
		{
			NumberOfTimesSummoned = 1;
			WaitBeforeTargettingAgainTime = 0;
			Projectile.netUpdate = true;
		}

		public override bool? CanCutTiles() => false;
		public override bool MinionContactDamage() => true;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			// If intersecting with the target, spawn blood dust.
			Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
		}

		public override void AI()
        {
			// Main.NewText($"{ProjectileID.Sets.TrailCacheLength[Type]}");
			// ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{ProjectileID.Sets.TrailCacheLength[Type]}; {Projectile.oldPos.Length}; {NumberOfTimesSummoned + 2}"), Color.White);

            Player player = Main.player[Projectile.owner];

			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<Buffs.Minions.BabyBloodEelBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.Minions.BabyBloodEelBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			/*else
			{
				// ProjectileID.Sets.TrailCacheLength[Type] = 96;
				// Array.Resize(ref Projectile.oldPos, ProjectileID.Sets.TrailCacheLength[Projectile.type]);
				// Projectile.netUpdate = true;
				Projectile.Kill();
			}
			*/

			int target = -1; // No target
			// Main.NewText(WaitBeforeTargettingAgainTime);
			if (WaitBeforeTargettingAgainTime <= 0) // If not waiting, try to find a target
			{
				if (player.HasMinionAttackTargetNPC) // If the player is targetting an NPC, target that NPC.
				{
					float distFromTarget = (Main.npc[player.MinionAttackTargetNPC].Center - Projectile.Center).Length();
					if (distFromTarget <= 800) // Check that the targetted NPC is close enough, otherwise ignore it.
					{
						target = player.MinionAttackTargetNPC;
					}
				}
				else
				{
					// Target the closest NPC
					target = FindTargetWithLineOfSight(800); // 50 block range
				}
			}
			else
			{
				WaitBeforeTargettingAgainTime--; // Decrease the wait time.
			}

			float minVelocity = 1f;

			// Main.NewText($"{target}");

			if (target != -1) // Has a target
			{
				Entity targetEntity = Main.npc[target];

				Vector2 center = targetEntity.Center;
				float distFromTarget = (center - Projectile.Center).Length();
				float distFromPlayer = (player.Center - Projectile.Center).Length();

				// Main.NewText($"{distFromTarget} {distFromPlayer}");
				
				// If the target or player is far away, stop targeting and go to the player.
				if (distFromTarget > 1024 || distFromPlayer > 1024) // 64 blocks
				{
					target = -1;
					center = player.Center;
					targetEntity = player;
					WaitBeforeTargettingAgainTime = 120; // Wait 2 seconds before targetting something else.
				}
				
				minVelocity = 6f;
				BoostWhenNotMoving(Projectile, targetEntity);
				if (distFromTarget > 32f) // Only turn if 2 tiles away from the target (So it doesn't become glued to the target).
				{
					minVelocity += ((distFromTarget - 32) / 32f) + Math.Min(NumberOfTimesSummoned / 2f, 6f); // Speed up if further away from the target.
					float targetAngle = Projectile.AngleTo(center);
					float f = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(30f)); // 30 deg turning radius.
					Projectile.velocity = f.ToRotationVector2() * minVelocity;
				}
			}
			else
			{
				HoverAroundPlayer(Projectile, player, ref minVelocity);
			}
			DoMovement(Projectile, minVelocity);

			/*if (Main.GameUpdateCount % 5 == 0)
			{
				for (int i = 0; i < Main.rand.Next(0, 2 + (int)Projectile.velocity.Length()); i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, 1, 1, DustID.DungeonSpirit, 0, 0, 50, default, 1.5f);
					dust.noGravity = true;
				}
			}*/

			// Increase the damage based on how many times the minion was summoned.
			// Additional Stardust Dragon summons: 23% of the weapon's listed damage.
			// Additional Abigail summons: 55% of the weapon's listed damage (Pre-HM), 130% of the weapon's listed damage (HM).
			// Additional Desert Tiger summons: 40% of the weapon's listed damage.

			// Additional summons: 75% of the weapon's list damage
			// If weapon does 40 damage:
			// 1 summon  = 40 damage
			// 2 summons = 70 damage
			// 3 summons = 100 damage etc.
			Projectile.damage = (int)Math.Round(Projectile.originalDamage * (1 + 0.75f * (NumberOfTimesSummoned - 1)));
			// Projectile.damage = Projectile.originalDamage * NumberOfTimesSummoned; // Basically, originalDamage * how many time you summoned the minion
		}

		/// <summary>
		/// While idle, it will hover around the player.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="player"></param>
		/// <param name="minVelocity"></param>
		public static void HoverAroundPlayer(Projectile projectile, Player player, ref float minVelocity)
		{
			Vector2 center = player.Center;
			if (projectile.Distance(center) > 2000f) // If really far from the player, teleport to the player.
			{
				projectile.Center = center;
				projectile.velocity = Vector2.Zero;
				projectile.netUpdate = true;
			}
			float distFromPlayer = (center - projectile.Center).Length();
			minVelocity = Math.Min(12f, Math.Max(4f, player.velocity.Length())); // Get faster if the player is moving fast.
			minVelocity += (distFromPlayer / 64f); // Speed up if further away from the player.

			BoostWhenNotMoving(projectile, player);
			if (distFromPlayer > 120f) // Only turn if 7.5 tiles away from the player (So it doesn't become glued to the player).
			{
				float targetAngle = projectile.AngleTo(center);
				float f = projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(5f)); // 5 deg turning radius.
				//f = Math.Clamp(f, 0f, MathHelper.ToRadians(5f));
				projectile.velocity = f.ToRotationVector2() * minVelocity;
			}
		}

		/// <summary>
		/// Gives the minion a little boost while not moving to get it to start moving.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="target"></param>
		public static void BoostWhenNotMoving(Projectile projectile, Entity target)
		{
			if (projectile.velocity == Vector2.Zero)
			{
				projectile.velocity.X = 2f * (float)target.direction;
				Vector2 position = projectile.position;
				for (int i = 0; i < projectile.oldPos.Length; i++)
				{
					position -= projectile.velocity;
					projectile.oldPos[i] = position;
				}
			}
		}

		/// <summary>
		/// Does the movement based on the minVelocity
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="minVelocity"></param>
		public static void DoMovement(Projectile projectile, float minVelocity)
		{
			if (projectile.velocity.Length() > minVelocity)
			{
				projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * minVelocity;
			}
			if (Math.Abs(projectile.velocity.Y) < 1f)
			{
				projectile.velocity.Y -= 0.1f;
			}
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			int direction = projectile.direction;
			projectile.direction = (projectile.spriteDirection = ((projectile.velocity.X > 0f) ? 1 : (-1)));
			if (direction != projectile.direction)
			{
				projectile.netUpdate = true;
			}
			// Clamp the position to slightly inside of the world bounds.
			projectile.position.X = MathHelper.Clamp(projectile.position.X, 160f, Main.maxTilesX * 16 - 160);
			projectile.position.Y = MathHelper.Clamp(projectile.position.Y, 160f, Main.maxTilesY * 16 - 160);
		}


		/// <summary>
		/// Copied from Projection.FindTargetWithLineOfSight() with the local immunity check removed.
		/// </summary>
		/// <param name="maxRange">Defaults 800</param>
		/// <returns>Index of the NPC</returns>
		public int FindTargetWithLineOfSight(float maxRange = 800f)
		{
			int result = -1;
			foreach (NPC npc in Main.ActiveNPCs)
			{
				bool canTarget = npc.CanBeChasedBy(this);
				/*
				if (Projectile.localNPCImmunity[i] != 0)
					canTarget = false;
				*/

				if (canTarget)
				{
					float DistanceToNPC = Projectile.Distance(npc.Center);
					if (DistanceToNPC < maxRange && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
					{
						maxRange = DistanceToNPC;
						result = npc.whoAmI;
					}
				}
			}

			return result;
		}

		public override bool PreDraw(Player player, ref Color lightColor)
		{
			// Draw the oldPos
			// for (int i = 0; i < Projectile.oldPos.Length; i++)
			// {
			//	Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Projectile.oldPos[i] - Main.screenPosition, new Rectangle(0, 0, 20, 20), Color.Magenta * 0.1f, 0f, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0);
			// }

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int spaceBetweenSegments = 16;
			SpriteEffects effects = ((Projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Rectangle rectangle = texture.Frame(1, Main.projFrames[Projectile.type]);
			Vector2 origin = rectangle.Size() / 2f;
			Vector2 position = Projectile.Center - Main.screenPosition;
			Color headColor = Projectile.GetAlpha(Lighting.GetColor(Projectile.Center.ToTileCoordinates()));
			Vector2 lastSegmentPos = Projectile.Center;
			int numFrameY = 1;
			int numOfFrames = Main.projFrames[Projectile.type] - 1; // 0 == head, 1 == body, 2 == tail

			int numberOfSegmentsToDraw = NumberOfTimesSummoned + 2;
			if (Main.netMode != NetmodeID.SinglePlayer) // Stop adding new segments in multiplayer after the 23rd time summoned. I couldn't get the net code to work.
			{
				numberOfSegmentsToDraw = Math.Min(numberOfSegmentsToDraw, 24);
			}

			for (int i = 1; i < numberOfSegmentsToDraw; i++) // Draw the number of times summoned +2 for 1 body and 1 tail segment.
			{
				int frameY = numFrameY;
				if (i == numberOfSegmentsToDraw - 1) // i == tail segment
				{
					frameY = numOfFrames; // Tail
				}
				Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], 0, frameY); // Get the frame (frame 1, body)

				Vector2 currentSegment = Vector2.Zero;
				
				try
				{
					currentSegment = Projectile.oldPos[i * 4] + Projectile.Size / 2f; // Center of the old pos. i * 4 seems to be the magic number to make it look good while moving fast and slow.
				}
				catch (Exception e)
				{
					// For some reason, the very first summon after restarting the game doesn't increment the TrailCacheLength which causes an ioob error on the above oldPos[] array.
					// All subsequent summons work just fine.
					ModContent.GetInstance<RijamsMod>().Logger.Error($"BabyBloodEel Projectile index out of bounds for drawing the segments. i was {i}; i * 4 {i * 4}; Projectile.oldPos.Length {Projectile.oldPos.Length}; TrailCacheLength was {ProjectileID.Sets.TrailCacheLength[Type]}. {e}");
					Projectile.Kill();
					ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Error with the Baby Blood Eel minion. Try summoning it again."), Color.OrangeRed);
				}

				float rotation = (lastSegmentPos - currentSegment).ToRotation(); // Calculate the rotation between the last segment and the old pos.
				
				// Place the next segment after the last one and rotate it based on the last segment.
				// If the segement is the tail, it gets extra space.
				currentSegment = lastSegmentPos - new Vector2(i == numberOfSegmentsToDraw - 1 ? spaceBetweenSegments + 6 : spaceBetweenSegments, 0f).RotatedBy(rotation, Vector2.Zero);
				
				Vector2 currentSegmentPosition = currentSegment - Main.screenPosition;
				SpriteEffects effects2 = ((!(currentSegment.X < lastSegmentPos.X)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None); // Flip the sprite based on the previous segment.
				
				lastSegmentPos = currentSegment; // Save the position of the current segment to be used for the next segment.
				
				Color segmentColor = Projectile.GetAlpha(Lighting.GetColor(currentSegment.ToTileCoordinates())); // Get the color of the environment.

				Main.EntitySpriteDraw(texture, currentSegmentPosition, sourceRectangle, segmentColor, rotation + MathHelper.PiOver2, origin, Projectile.scale, effects2, 0); // Draw the body and tail.
			}
			Main.EntitySpriteDraw(texture, position, rectangle, headColor, Projectile.rotation, origin, Projectile.scale, effects, 0); // Draws the head
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			// writer.Write(ProjectileID.Sets.TrailCacheLength[Type]);
			// writer.Write(Projectile.oldPos.Length);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			// ProjectileID.Sets.TrailCacheLength[Type] = reader.ReadInt32();
			//int length = reader.ReadInt32();
			// Array.Resize(ref Projectile.oldPos, ProjectileID.Sets.TrailCacheLength[Projectile.type]);
		}

		public override void OnKill(int timeLeft)
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 96;
			Array.Resize(ref Projectile.oldPos, ProjectileID.Sets.TrailCacheLength[Projectile.type]);
		}
	}
}