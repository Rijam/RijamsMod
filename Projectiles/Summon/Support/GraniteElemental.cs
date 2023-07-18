using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Support
{
	public class GraniteElemental : ModProjectile
	{
		public int healAmount = 0;
		public int cooldownTime = 0;
		public int distRadius = 0;
		private int targetPlayer = -1;  // If there was no player found, it will be -1.

		public override void SetStaticDefaults()
		{
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[Projectile.type] = 1;
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
			Projectile.width = 20;
			Projectile.height = 32;
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
			// Sync this projectile if a player joins mid game.
			Projectile.netImportant = true;

			DrawOffsetX = -20;
			DrawOriginOffsetY = -14;
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
				player.ClearBuff(ModContent.BuffType<Buffs.Minions.GraniteElementalBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.Minions.GraniteElementalBuff>()))
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

			int radius = (distRadius + player.GetModPlayer<RijamsModPlayer>().supportMinionRadiusIncrease) * 16;
			if (Main.netMode != NetmodeID.Server)
			{
				RijamsModConfigClient configClient = ModContent.GetInstance<RijamsModConfigClient>();
				if (configClient.DisplayHealingSupportSummonsAura != RijamsModConfigClient.SupportSummonsAura.Off)
				{
					for (int i = 0; i < 50; i++)
					{
						Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
						int alpha = configClient.DisplayHealingSupportSummonsAura switch
						{
							RijamsModConfigClient.SupportSummonsAura.Opaque => 0,
							RijamsModConfigClient.SupportSummonsAura.Normal => 150,
							RijamsModConfigClient.SupportSummonsAura.Faded => 240,
							RijamsModConfigClient.SupportSummonsAura.Off => 255,
							_ => 150,
						};
						Dust d = Dust.NewDustPerfect(Projectile.Center + speed * radius, ModContent.DustType<Dusts.AuraDust>(), speed, alpha, Color.Purple, 0.75f);
						d.noGravity = true;
						d.noLightEmittence = true;
					}
				}
			}

			int delay = cooldownTime; // 30 seconds
			// If the projectile was spawned without using the item, it'll have a cooldownTime of 0.
			// That makes it spawn the projectile every tick.
			if (cooldownTime == 0)
			{
				delay = int.MaxValue;
			}
			Projectile.ai[0]++; // ai[0] is the counter for when to shoot the next projectile.

			if (Projectile.ai[0] >= delay) // Wait until the delay is up
			{
				// Search for each player within the radius and add them to the list.
				if (Main.netMode != NetmodeID.Server)
				{
					Dictionary<Player, int> players = new();
					for (int i = 0; i < Main.maxPlayers; i++)
					{
						Player searchPlayer = Main.player[i];
						if (HarpyIdol.SearchPlayers(player, searchPlayer))
						{
							double distance = Vector2.Distance(searchPlayer.Center, Projectile.Center);
							if (distance <= radius)
							{
								//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Added player " + searchPlayer.whoAmI + " Name " + searchPlayer.name), Color.Green);
								players.Add(searchPlayer, searchPlayer.statLife);
							}
						}
					}
					// Find the player in that list with the lowest HP.
					//int targetPlayer = -1;  // If there was no player found, it will be -1.

					// First, check that any players were found.
					if (players.Count > 0)
					{
						// Sort the dictionary in descending order by value.
						// That makes the player with the lowest HP the last pair in the dictionary.
						Player playerWithLowestHP = players.OrderByDescending(pair => pair.Value).Last().Key;

						//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("playerWithLowestHP is " + playerWithLowestHP.whoAmI + " Name " + playerWithLowestHP.name), Color.Gold);
						targetPlayer = playerWithLowestHP.whoAmI; // Set the target player as the player with the lowest HP.
					}
					players.Clear(); // Clear the list just for good measure.
				}
				if (Main.myPlayer == Projectile.owner)
				{
					/*if (targetPlayer > 0)
						ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("targetPlayer is " + targetPlayer + " Name " + Main.player[targetPlayer].name), Color.Red);*/

					// Spawn the Curative Butterfly projectile with the player with the lowest HP as its target.
					Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, new(0, -1), ModContent.ProjectileType<GraniteEnergy>(),
						Projectile.damage, Projectile.knockBack, Projectile.owner,
						targetPlayer, healAmount);
				}

				SoundEngine.PlaySound(SoundID.Item8 with { Pitch = 0.5f }, Projectile.Center);

				// Set ai[0] back to 0.
				Projectile.ai[0] = 0;
				Projectile.frame = 0;
			}

			#endregion

			#region Animation and visuals
			// So it will lean slightly towards the direction it's moving
			Projectile.rotation = Projectile.velocity.X * 0.01f;

			/*if (Math.Abs(Projectile.velocity.X) < 1f)
			{
				Projectile.spriteDirection = player.direction * -1;
			}
			else
			{
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt() * -1;
			}*/
			float cooldownDiv4 = cooldownTime / 4f;

			if (Projectile.ai[0] < cooldownDiv4) // 7.5
			{
				Projectile.ai[1] = 4;
				if (Projectile.ai[0] % 4 == 0)
				{
					Dust.NewDust(new(Projectile.position.X, Projectile.position.Y + Projectile.height), Projectile.width, 1, DustID.Granite, Scale: 0.125f);
				}
			}
			else if (Projectile.ai[0] < cooldownDiv4 * 2) // 15
			{
				Projectile.ai[1] = 0;
				if (Projectile.ai[0] % 4 == 0)
				{
					Dust.NewDust(new(Projectile.position.X, Projectile.position.Y + Projectile.height), Projectile.width, 1, DustID.Granite, Scale: 0.25f);
				}
			}
			else if (Projectile.ai[0] < cooldownDiv4 * 3) // 22.5
			{
				Projectile.ai[1] = 1;
				if (Projectile.ai[0] % 4 == 0)
				{
					Dust.NewDust(new(Projectile.position.X, Projectile.position.Y + Projectile.height), Projectile.width, 1, DustID.Granite, Scale: 0.375f);
				}
			}
			else if (Projectile.ai[0] < cooldownDiv4 * 4 - (3 * 60)) // 27
			{
				Projectile.ai[1] = 2;
				if (Projectile.ai[0] % 4 == 0)
				{
					Dust.NewDust(new(Projectile.position.X, Projectile.position.Y + Projectile.height), Projectile.width, 1, DustID.Granite, Scale: 0.5f);
				}
			}
			else // 30
			{
				Projectile.position.Y -= 4f; // Move up a lot when 3 seconds left

				Projectile.ai[1] = 3;
				if (Projectile.ai[0] % 4 == 0)
				{
					Dust.NewDust(new(Projectile.position.X, Projectile.position.Y + Projectile.height), Projectile.width, 1, DustID.Granite, Scale: 0.75f);
				}
			}

			Projectile.ai[2]++;
			if (Projectile.ai[2] > 48)
			{
				Projectile.ai[2] = 0;
			}

			Projectile.position.Y += (float)Math.Sin(Projectile.ai[0] / 10f); // Slightly move up and down.

			#endregion
		}

		private readonly Texture2D textureSmallRocks = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Summon/Support/GraniteElemental_SmallRocks").Value;
		private readonly Texture2D textureMediumRocks = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Summon/Support/GraniteElemental_MediumRocks").Value;
		private readonly Texture2D textureBigRocks = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Summon/Support/GraniteElemental_BigRocks").Value;
		private readonly Texture2D textureLight = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Summon/Support/GraniteElemental_Light").Value;


		public override void PostDraw(Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangleLight = textureLight.Frame(1, 4, frameY: (int)Projectile.ai[1]);
			
			Rectangle sourceRectangleRocks = textureSmallRocks.Frame(1, 12, frameY: (int)(Projectile.ai[2] / 4f));

			Main.EntitySpriteDraw(textureLight, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangleLight, GetAlpha(lightColor).Value, Projectile.rotation, sourceRectangleLight.Size() / 2f, Projectile.scale, spriteEffects, 0);

			float cooldownDiv4 = cooldownTime / 4f;

			if (Projectile.ai[0] >= cooldownDiv4)
			{
				Main.EntitySpriteDraw(textureSmallRocks, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
					sourceRectangleRocks, GetAlpha(lightColor).Value, Projectile.rotation, sourceRectangleRocks.Size() / 2f, Projectile.scale, spriteEffects, 0);
			}
			if (Projectile.ai[0] >= cooldownDiv4 * 2)
			{
				Main.EntitySpriteDraw(textureMediumRocks, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
					sourceRectangleRocks, GetAlpha(lightColor).Value, Projectile.rotation, sourceRectangleRocks.Size() / 2f, Projectile.scale, spriteEffects, 0);
			}
			if (Projectile.ai[0] >= cooldownDiv4 * 3)
			{
				Main.EntitySpriteDraw(textureBigRocks, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
					sourceRectangleRocks, GetAlpha(lightColor).Value, Projectile.rotation, sourceRectangleRocks.Size() / 2f, Projectile.scale, spriteEffects, 0);
			}
		}

		public float LerpValue()
		{
			if (cooldownTime > 0)
			{
				return MathHelper.Lerp(0f, 1f, Projectile.ai[0] / (float)cooldownTime);
			}
			return 0f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			int r = (int)MathHelper.Lerp(lightColor.R, 255, LerpValue());
			int g = (int)MathHelper.Lerp(lightColor.G, 255, LerpValue());
			int b = (int)MathHelper.Lerp(lightColor.B, 255, LerpValue());

			return new(r, g, b, 255);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(cooldownTime);
			writer.Write(distRadius);
			writer.Write(targetPlayer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			cooldownTime = reader.ReadInt32();
			distRadius = reader.ReadInt32();
			targetPlayer = reader.ReadInt32();
		}
	}
}