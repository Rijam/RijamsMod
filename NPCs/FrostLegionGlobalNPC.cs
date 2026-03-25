using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using static RijamsMod.RijamsModConfigServer;

namespace RijamsMod.NPCs
{
	public class FrostLegionGlobalNPC : GlobalNPC
	{
		// Loot set in RijamsModNPCs.cs
	}

	public class FrostLegionSystem : ModSystem
	{
		public override void PostUpdateEverything()
		{
			if (Main.invasionType == InvasionID.SnowLegion && Main.invasionProgressAlpha > 0)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					try
					{
						SnowingForFrostLegion();
					}
					catch
					{
						if (!Main.ignoreErrors)
							throw;
					}
				}
			}
		}
		// Copied from Main.snowing()
		internal static void SnowingForFrostLegion()
		{
			if (!ModContent.GetInstance<RijamsModConfigClient>().SnowDuringFrostLegion)
			{
				return;
			}

			if (Main.remixWorld)
			{
				return;
			}

			Vector2 scaledSize = Main.Camera.ScaledSize;
			Vector2 scaledPosition = Main.Camera.ScaledPosition;
			if (Main.gamePaused /* || Main.SceneMetrics.SnowTileCount <= 0 */ || (!((double)Main.LocalPlayer.position.Y < Main.worldSurface * 16.0) && (!Main.remixWorld || !((double)Main.player[Main.myPlayer].position.Y > Main.worldSurface * 16.0))))
			{
				return;
			}

			// float SnowBiomePercentage = (float)Main.SceneMetrics.SnowTileCount / (float)SceneMetrics.SnowTileMax;
			float SnowBiomePercentage = 1f;
			// SnowBiomePercentage *= SnowBiomePercentage;
			// SnowBiomePercentage *= SnowBiomePercentage;
			float cameraX = Main.Camera.ScaledSize.X / (float)Main.maxScreenW;
			int cameraDustAmount = (int)(500f * cameraX);
			cameraDustAmount *= (int)(1f + 2f * Main.cloudAlpha);
			float rainAmoutish = 1f + 50f * Main.cloudAlpha;
			rainAmoutish = MathHelper.Clamp(rainAmoutish, 3f, 10f); // Clamp so that it is more snowy when not raining and not a blizzard while raining.

			for (int i = 0; i < rainAmoutish; i++)
			{
				try
				{
					if (!(Main.snowDust < cameraDustAmount * (Main.gfxQuality / 2f + 0.5f) + cameraDustAmount * 0.1f))
					{
						break;
					}

					if (!(Main.rand.NextFloat() < SnowBiomePercentage))
					{
						continue;
					}

					int dustPosX = Main.rand.Next((int)scaledSize.X + 1500) - 750;
					int dustPosY = (int)scaledPosition.Y - Main.rand.Next(50);
					if (Main.LocalPlayer.velocity.Y > 0f)
					{
						dustPosY -= (int)Main.LocalPlayer.velocity.Y;
					}
					if (Main.rand.NextBool(5))
					{
						dustPosX = Main.rand.Next(500) - 500;
					}
					else if (Main.rand.NextBool(5))
					{
						dustPosX = Main.rand.Next(500) + (int)scaledSize.X;
					}
					if (dustPosX < 0 || dustPosX > scaledSize.X)
					{
						dustPosY += Main.rand.Next((int)(scaledSize.Y * 0.8)) + (int)(scaledSize.Y * 0.1);
					}
					dustPosX += (int)scaledPosition.X;
					int dustPosXWorld = dustPosX / 16;
					int dustPosYWorld = dustPosY / 16;
					if (WorldGen.InWorld(dustPosXWorld, dustPosYWorld) && Main.tile[dustPosXWorld, dustPosYWorld] != null && !Main.tile[dustPosXWorld, dustPosYWorld].HasUnactuatedTile && Main.tile[dustPosXWorld, dustPosYWorld].WallType == 0)
					{
						int dustIndex = Dust.NewDust(new Vector2(dustPosX, dustPosY), 10, 10, DustID.Snow);
						Main.dust[dustIndex].scale += Main.cloudAlpha * 0.2f;
						Main.dust[dustIndex].velocity.Y = 3f + Main.rand.Next(30) * 0.1f;
						Main.dust[dustIndex].velocity.Y *= Main.dust[dustIndex].scale;
						if (!Main.raining)
						{
							Main.dust[dustIndex].velocity.X = Main.windSpeedCurrent + Main.rand.Next(-10, 10) * 0.1f;
							Main.dust[dustIndex].velocity.X += Main.windSpeedCurrent * 15f;
						}
						else
						{
							Main.dust[dustIndex].velocity.X = (float)Math.Sqrt(Math.Abs(Main.windSpeedCurrent)) * Math.Sign(Main.windSpeedCurrent) * (Main.cloudAlpha + 0.5f) * 10f + Main.rand.NextFloat() * 0.2f - 0.1f;
							Main.dust[dustIndex].velocity.Y *= 0.5f;
						}

						Main.dust[dustIndex].velocity.Y *= 1f + 0.3f * Main.cloudAlpha;
						Main.dust[dustIndex].scale += Main.cloudAlpha * 0.2f;

						Main.dust[dustIndex].velocity *= 1f + Main.cloudAlpha * 0.5f;
					}

					continue;
				}
				catch
				{
				}
			}
		}

		public override void Load()
		{
			// Terraria.On_Main.StartInvasion += DetourMainStartInvasion;
		}

		// WIP for the future. Might also need to Detour Main.FakeLoadInvasionStart().
		private static void DetourMainStartInvasion(On_Main.orig_StartInvasion orig, int type = 1)
		{
			// type == 2 for Frost Legion
			orig(type);

			if (true)
			{
				int players = 0;
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					if (Main.player[i].active && Main.player[i].ConsumedLifeCrystals >= 5)
						players++;
				}
				if (players > 0)
				{
					if (type == InvasionID.SnowLegion)
					{
						// Main.invasionType = type; // Should already be set.

						// Original size is 80 + (40 * players) kills.
						Main.invasionSize = 80 + 40 * players;
						Main.invasionSizeStart = Main.invasionSize;
						Main.invasionProgressMax = Main.invasionSizeStart;
					}
				}
			}
		}
	}

	public class SnowBallHostileGlobalProjectile : GlobalProjectile
	{
		public override void SetDefaults(Projectile entity)
		{
			// Config is set to DropAsItem or Off
			if (ModContent.GetInstance<RijamsModConfigServer>().SnowBallaGriefing >= SnowBallaGriefingOptions.DropAsItem && entity.type == ProjectileID.SnowBallHostile)
			{
				entity.noDropItem = true;
			}
		}
		public override void OnKill(Projectile projectile, int timeLeft)
		{
			if (ModContent.GetInstance<RijamsModConfigServer>().SnowBallaGriefing == SnowBallaGriefingOptions.DropAsItem && projectile.type == ProjectileID.SnowBallHostile)
			{
				int newItem = Item.NewItem(projectile.GetSource_DropAsItem(), projectile.Hitbox, ItemID.SnowBlock);

				// Here we need to make sure the item is synced in multiplayer games.
				if (Main.netMode == NetmodeID.MultiplayerClient && newItem >= 0)
				{
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, newItem, 1f);
				}
			}
		}
	}
}