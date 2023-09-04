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
	public class RadiantLantern : ModProjectile
	{
		public int healAmount = 0;
		public int cooldownTime = 0;
		public int distRadius = 0;
		private int targetPlayer = -1;  // If there was no player found, it will be -1.
		private int targetPlayer2 = -1;  // If there was no player found, it will be -1.

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Radiant Lantern");
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
			Projectile.width = 22;
			Projectile.height = 42;
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
				player.ClearBuff(ModContent.BuffType<Buffs.Minions.RadiantLanternBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.Minions.RadiantLanternBuff>()))
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
				RijamsModConfigClient configClient = ModContent.GetInstance<RijamsModConfigClient>();
				if (configClient.DisplayHealingSupportSummonsAura != RijamsModConfigClient.SupportSummonsAura.Off)
				{
					for (int i = 0; i < 70; i++)
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
						Dust d = Dust.NewDustPerfect(Projectile.Center + speed * radius, ModContent.DustType<Dusts.AuraDust>(), speed, alpha, Color.HotPink, 0.75f);
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
					// Find the players in that list with the lowest HP.

					// First, check that any players were found.
					if (players.Count > 0)
					{
						// Sort the dictionary in descending order by value.
						// That makes the player with the lowest HP the last pair in the dictionary.
						Player playerWithLowestHP = players.OrderByDescending(pair => pair.Value).Last().Key;

						//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("playerWithLowestHP is " + playerWithLowestHP.whoAmI + " Name " + playerWithLowestHP.name), Color.Gold);
						targetPlayer = playerWithLowestHP.whoAmI; // Set the target player as the player with the lowest HP.

						players.Remove(playerWithLowestHP); // Remove the player from the list

						if (players.Count > 0) // If players only had 1 player and we removed them above, it is now empty. There would be no second player to check.
						{
							// Find the player in the list with the second lowest HP.
							// Sort the dictionary in descending order by value.
							// That makes the player with the lowest HP the last pair in the dictionary.
							playerWithLowestHP = players.OrderByDescending(pair => pair.Value).Last().Key;

							//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("2 playerWithLowestHP is " + playerWithLowestHP.whoAmI + " Name " + playerWithLowestHP.name), Color.Gold);
							targetPlayer2 = playerWithLowestHP.whoAmI; // Set the target player as the player with the lowest HP.
						}
						// If there was no second player, the target will default to the projectile owner.
					}
					players.Clear(); // Clear the list just for good measure.
				}

				/*if (targetPlayer > 0)
					ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("targetPlayer is " + targetPlayer + " Name " + Main.player[targetPlayer].name), Color.Red);
				if (targetPlayer2 > 0)
					ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("targetPlayer2 is " + targetPlayer2 + " Name " + Main.player[targetPlayer2].name), Color.Red);*/

				// Spawn the Radiance projectile with the player with the lowest HP as its target.
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One, ModContent.ProjectileType<Radiance>(),
					0, 0, Projectile.owner,
					targetPlayer,
					Main.rand.NextFloat(0.8f, 0.9f), healAmount);

				// Spawn a second projectile with the owner as the target.
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One * -1, ModContent.ProjectileType<Radiance>(),
					0, 0, Projectile.owner,
					targetPlayer2,
					Main.rand.NextFloat(0.8f, 0.9f), healAmount);

				SoundEngine.PlaySound(SoundID.Item82 with { Pitch = 0.5f }, Projectile.Center);

				// Set ai[0] back to 0.
				Projectile.ai[0] = 0;
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

			Projectile.position.Y += (float)Math.Sin(Projectile.ai[0] / 10f); // Slightly move up and down.
			if (Projectile.ai[0] >= cooldownTime - (3 * 60)) // 27 seconds
			{
				Projectile.position.Y -= 5f; // Move up a lot when 3 seconds left
			}

			#endregion
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

		public override void PostDraw(Color lightColor)
		{
			Texture2D lightTexture = ModContent.Request<Texture2D>(Mod.Name + "/Projectiles/Summon/Support/RadiantLanternShine").Value;

			Player owner = Main.player[Projectile.owner];

			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = ((Projectile.spriteDirection <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			if (owner.gravDir == -1f)
			{
				spriteEffects |= SpriteEffects.FlipVertically;
			}

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = lightTexture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

			// The origin 
			Vector2 origin = new(sourceRectangle.Size().X / 2f, sourceRectangle.Size().Y / 2f + DrawOriginOffsetY);

			// The rotation of the projectile.
			float rotation = Projectile.rotation;

			// The position of the sprite. Subtracting Main.player[Projectile.owner].gfxOffY is not necessary here.
			Vector2 position = new(Projectile.Center.X, Projectile.Center.Y + 6);

			Color drawColor = Color.Lime * LerpValue();
			drawColor.A = 255;

			Main.EntitySpriteDraw(lightTexture,
				position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, rotation, origin, LerpValue(), spriteEffects, 0);

			Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * LerpValue());
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(cooldownTime);
			writer.Write(distRadius);
			writer.Write(targetPlayer);
			writer.Write(targetPlayer2);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			cooldownTime = reader.ReadInt32();
			distRadius = reader.ReadInt32();
			targetPlayer = reader.ReadInt32();
			targetPlayer2 = reader.ReadInt32();
		}
	}
}