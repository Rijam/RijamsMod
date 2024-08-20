using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Items.Weapons.Melee.JoustingLances;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	// I made Example Jousting Lance so I'm going to use it!
	public class HorsemansJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.Torch;
			dustTypeRare = DustID.Pumpkin;
			offset = 4;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 130f;
			widthMultiplier = 24f;
			lanceHitboxBounds = new(0, 0, 300, 300);
		}

		public override void ModifyDrawing(ref Color drawColor)
		{
			drawColor = new Color(255, 255, 255, 200);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			int itemType = ModContent.ItemType<HorsemansJoustingLance>();
			if (target.active)
			{
				Player owner = Main.player[Projectile.owner];
				if (owner.inventory[owner.selectedItem].type == itemType && !target.immortal && !target.SpawnedFromStatue && !NPCID.Sets.CountsAsCritter[target.type])
				{
					// The Hallowed and Shadow Jousting Lance spawn dusts when the player is moving above a certain speed.
					float minimumDustVelocity = 6f;
					int spawnChance = 8;

					// This Vector2.Dot is the dot product between the projectile's velocity and the player's velocity normalized to be between -1 and 1.
					// What this means in this context is that the speed value will be closer to positive 1 if the player is moving in the same direction as the direction the lance was shot.
					// Example: if the lance is shot up and to the right, the value here will be closer to 1 if the player is also moving up and to the right.
					float movementInLanceDirection = Vector2.Dot(Projectile.velocity.SafeNormalize(Vector2.UnitX * owner.direction), owner.velocity.SafeNormalize(Vector2.UnitX * owner.direction));

					float playerVelocity = owner.velocity.Length();

					if (playerVelocity > minimumDustVelocity && movementInLanceDirection > 0.8f)
					{
						// The chance for the dust to spawn. The actual chance (see below) is 1/dustChance. We make the chance higher the faster the player is moving by making the denominator smaller.
						if (playerVelocity > minimumDustVelocity + 1f)
						{
							spawnChance = 5;
						}
						if (playerVelocity > minimumDustVelocity + 2f)
						{
							spawnChance = 2;
						}
					}

					if (Main.rand.NextBool(spawnChance)) // Better than the normal The Horseman's Blade without this
					{
						Main.player[Projectile.owner].HorsemansBlade_SpawnPumpkin(target.whoAmI, Projectile.damage, Projectile.knockBack);

						ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningHit, new ParticleOrchestraSettings
						{
							PositionInWorld = Projectile.Center,
							MovementVector = Vector2.Zero
						});
					}
				}
			}
		}
	}
}