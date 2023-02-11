using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Support
{
	public class SanityFlower : ModProjectile
	{
		public int cooldownTime = 0;
		public int distRadius = 0;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sanity Flower");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[Projectile.type] = 3;
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[Projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 36;
			// Makes the minion go through tiles freely
			Projectile.tileCollide = false;

			// These below are needed for a minion weapon
			// Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.friendly = true;
			// Only determines the damage type
			Projectile.minion = true;
			// Declares the damage type (needed for it to deal damage)
			Projectile.DamageType = DamageClass.Summon;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.minionSlots = 1f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.penetrate = -1;
		}

		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return false;
		}

        public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			#region Active check
			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<Buffs.Minions.SanityFlowerBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.Minions.SanityFlowerBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			#endregion

			#region AI

			Vector2 yOffset = new(-60 * player.direction, -60f);
			Vector2 mountedPosition = player.MountedCenter + yOffset;
			float distProjCenterMountPos = Vector2.Distance(Projectile.Center, mountedPosition);
			if (distProjCenterMountPos > 1000f)
			{
				Projectile.Center = player.Center + yOffset;
			}
			Vector2 difMountPosProjCenter = mountedPosition - Projectile.Center;
			float numIs4f = 4f;
			if (distProjCenterMountPos < numIs4f)
			{
				Projectile.velocity *= 0.25f;
			}
			if (difMountPosProjCenter != Vector2.Zero)
			{
				if (difMountPosProjCenter.Length() < numIs4f)
				{
					Projectile.velocity = difMountPosProjCenter;
				}
				else
				{
					Projectile.velocity = difMountPosProjCenter * 0.1f;
				}
			}

			int radius = (distRadius + player.GetModPlayer<RijamsModPlayer>().supportMinionRadiusIncrease) * 16; // 20 tiles
			if (Main.netMode != NetmodeID.Server)
			{
				if (ModContent.GetInstance<RijamsModConfigClient>().DisplayHealingSupportSummonsAura)
				{
					for (int i = 0; i < 50; i++)
					{
						Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
						Dust d = Dust.NewDustPerfect(Projectile.Center + speed * radius, ModContent.DustType<Dusts.AuraDust>(), speed, 150, Color.Green, 0.75f);
						d.noGravity = true;
						d.noLightEmittence = true;
					}
				}
			}

			int delay = cooldownTime; // 30 seconds
			Projectile.ai[0]++; // ai[0] is the counter for when to shoot the next projectile.

			if (Projectile.ai[0] >= delay) // Wait until the delay is up
			{
				// Search for each player within the radius and add them to the list.
				List<Player> players = new();
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Player searchPlayer = Main.player[i];
					if (searchPlayer.active || !searchPlayer.dead || !searchPlayer.hostile || (searchPlayer.team == player.team && searchPlayer.team != 0))
					{
						double distance = Vector2.Distance(searchPlayer.Center, Projectile.Center);
						if (distance <= radius)
						{
							players.Add(searchPlayer);
						}
					}
				}
				// Find the player in that list with the lowest HP.
				int targetPlayer = -1;  // If there was no player found, it will be -1.

				// First, check that any players were found.
				if (players.Count > 0)
				{
					Player playerWithLowestHP = players[0]; // Assign the player with the lowest HP as the first player.
					// Go through all of the found players and find which one has the lowest HP.
					foreach (Player foundPlayer in players)
					{
						if (foundPlayer.statLife < playerWithLowestHP.statLife)
						{
							playerWithLowestHP = foundPlayer;
						}
					}
					targetPlayer = playerWithLowestHP.whoAmI; // Set the target player as the player with the lowest HP.
				}
				// Spawn the Curative Butterfly projectile with the player with the lowest HP as its target.
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, new(0, -1), ModContent.ProjectileType<CurativeButterfly>(),
					Projectile.damage, Projectile.knockBack, Projectile.owner,
					targetPlayer);

				SoundEngine.PlaySound(SoundID.Item8 with { Pitch = 0.5f }, Projectile.Center);

				// Set ai[0] back to 0.
				Projectile.ai[0] = 0;
				players.Clear(); // Clear the list just for good measure.

				Projectile.frame = 0;
			}

			#endregion

			#region Animation and visuals
			// So it will lean slightly towards the direction it's moving
			Projectile.rotation = Projectile.velocity.X * 0.01f;

			if (Math.Abs(Projectile.velocity.X) < 1f)
			{
				Projectile.spriteDirection = player.direction * -1;
			}
			else
			{
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt() * -1;
			}

			if (Projectile.ai[0] > cooldownTime / 3f)
			{
				if (Projectile.ai[0] >= cooldownTime - (3 * 60)) // 27 seconds
				{
					Projectile.position.Y -= 4f; // Move up a lot when 3 seconds left
					Projectile.frame = 2;
					if (Projectile.ai[0] % 2 == 0)
					{ 
						Dust.NewDust(new (Projectile.position.X, Projectile.position.Y + Projectile.height), Projectile.width, 1, DustID.Grass, Scale: 0.75f);
					}
				}
				else
				{
					Projectile.frame = 1;
					if (Projectile.ai[0] % 2 == 0)
					{
						Dust.NewDust(new(Projectile.position.X, Projectile.position.Y + Projectile.height), Projectile.width, 1, DustID.Grass, Scale: 0.5f);
					}
				}
			}
			else
			{
				if (Projectile.ai[0] % 2 == 0)
				{
					Dust.NewDust(new(Projectile.position.X, Projectile.position.Y + Projectile.height), Projectile.width, 1, DustID.Grass, Scale: 0.25f);
				}
			}

			Projectile.position.Y += (float)Math.Sin(Projectile.ai[0] / 10f); // Slightly move up and down.
			
			#endregion
		}

		public float LerpValue()
		{
			return MathHelper.Lerp(0f, 1f, Projectile.ai[0] / (float)cooldownTime);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			int r = (int)MathHelper.Lerp(lightColor.R, 255, LerpValue());
			int g = (int)MathHelper.Lerp(lightColor.G, 255, LerpValue());
			int b = (int)MathHelper.Lerp(lightColor.B, 255, LerpValue());

			return new(r, g, b, 255);
		}
	}
}