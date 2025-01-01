using Microsoft.Xna.Framework;
using RijamsMod.Items.Weapons.Melee.JoustingLances;
using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	// I made Example Jousting Lance so I'm going to use it!
	public class CandyCaneJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.WhiteTorch;
			dustTypeRare = DustID.RedTorch;
			offset = 4;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 130f;
			widthMultiplier = 24f;
			lanceHitboxBounds = new(0, 0, 300, 300);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			int itemType = ModContent.ItemType<CandyCaneJoustingLance>();
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
							spawnChance = 3;
						}
						if (playerVelocity > minimumDustVelocity + 2f)
						{
							spawnChance = 1;
						}
					}

					if (Main.rand.NextBool(spawnChance))
					{
						if (Projectile.owner == Main.myPlayer)
						{
							Projectile.NewProjectile(Entity.GetSource_FromThis(), new Vector2(target.position.X, target.position.Y - Main.screenHeight - 100 - (target.position.Y - owner.position.Y)), new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.Next(10, 14)), ModContent.ProjectileType<FestiveOrnament>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, -1, Main.rand.Next(0, 4));
						}

						ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.PaladinsHammer, new ParticleOrchestraSettings
						{
							PositionInWorld = Projectile.Center,
							MovementVector = Projectile.velocity
						});
					}
				}
			}
		}
	}
}