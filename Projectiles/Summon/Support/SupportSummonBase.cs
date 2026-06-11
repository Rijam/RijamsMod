using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Support
{
	public class SupportSummonBase : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => false;
		public override string Texture => null;

		/// <summary>
		/// The radius of the support summon. Defaults to 0;
		/// </summary>
		public int distRadius = 0;

		public override void SetStaticDefaults()
		{
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[Projectile.type] = 1;
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargetingFeature[Projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[Projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
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

		/// <summary>
		/// Set the associated buff for this summon.
		/// </summary>
		/// <param name="buffType">Use <code>ModContent.BuffType&lt;T&gt;()</code></param>
		public virtual void BuffType(ref int buffType)
		{

		}

		/// <summary>
		/// <br>Change the color of the light that the summon emits.</br>
		/// <br>Return false to stop the base class' light code.</br>
		/// </summary>
		/// <param name="lightColor">The color of the light.</param>
		/// <param name="multiplier">A multiplier on the color to dim the light.</param>
		/// <returns>Return true by default to emit light.</returns>
		public virtual bool LightingColor(ref Color lightColor, ref float multiplier)
		{
			return true;
		}

		/// <summary>
		/// Customize the dust for the aura.
		/// </summary>
		/// <param name="color">The color of the aura dust.</param>
		/// <param name="numberOfDusts">The number of dusts to emit. Large radii needs more dusts.</param>
		public virtual void DustCustomization(ref Color color, ref int numberOfDusts)
		{

		}

		/// <summary>
		/// Adjusts the position offset relative to the player.
		/// </summary>
		/// <param name="offset">Defaults to (0, -60f)</param>
		public virtual void PositionOffset(ref Vector2 offset)
		{

		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			#region Active check
			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not

			int buffType = 0;
			BuffType(ref buffType);

			if (player.dead || !player.active)
			{
				player.ClearBuff(buffType);
			}
			if (player.HasBuff(buffType))
			{
				Projectile.timeLeft = 2;
			}
			#endregion

			#region AI

			Vector2 yOffset = new(0, -60f);
			PositionOffset(ref yOffset);
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

			if (Main.netMode != NetmodeID.Server)
			{
				int radius = GetRadius(player);
				RijamsModConfigClient configClient = ModContent.GetInstance<RijamsModConfigClient>();
				Color dustColor = Color.Blue;
				int numberOfDusts = 50;
				DustCustomization(ref dustColor, ref numberOfDusts);
				if (this is DefenseSupportSummonBase && configClient.DisplayDefenseSupportSummonsAura != RijamsModConfigClient.SupportSummonsAura.Off)
				{
					int alpha = configClient.DisplayDefenseSupportSummonsAura switch
					{
						RijamsModConfigClient.SupportSummonsAura.Opaque => 0,
						RijamsModConfigClient.SupportSummonsAura.Normal => 150,
						RijamsModConfigClient.SupportSummonsAura.Faded => 240,
						RijamsModConfigClient.SupportSummonsAura.Off => 255,
						_ => 150,
					};
					for (int i = 0; i < numberOfDusts; i++)
					{
						Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
						Dust d = Dust.NewDustPerfect(Projectile.Center + speed * radius, ModContent.DustType<Dusts.AuraDust>(), speed, alpha, dustColor, 0.75f);
						d.noGravity = true;
						d.noLightEmittance = true;
						// Messing around with mixing the tile light with the color.
						/*Color lightingColor = Lighting.GetColor(d.position.ToTileCoordinates());
						d.color = Color.Lerp(dustColor, lightingColor, 0.75f);
						if (d.color.R < 100)
						{
							d.alpha += 50;
						}
						if (d.color.G < 100)
						{
							d.alpha += 50;
						}
						if (d.color.B < 100)
						{
							d.alpha += 50;
						}*/
					}
				}
				if (this is HealingSupportSummonBase && configClient.DisplayHealingSupportSummonsAura != RijamsModConfigClient.SupportSummonsAura.Off)
				{
					int alpha = configClient.DisplayHealingSupportSummonsAura switch
					{
						RijamsModConfigClient.SupportSummonsAura.Opaque => 0,
						RijamsModConfigClient.SupportSummonsAura.Normal => 150,
						RijamsModConfigClient.SupportSummonsAura.Faded => 240,
						RijamsModConfigClient.SupportSummonsAura.Off => 255,
						_ => 150,
					};
					for (int i = 0; i < numberOfDusts; i++)
					{
						Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
						Dust d = Dust.NewDustPerfect(Projectile.Center + speed * radius, ModContent.DustType<Dusts.AuraDust>(), speed, alpha, dustColor, 0.75f);
						d.noGravity = true;
						d.noLightEmittance = true;
					}
				}
			}
			#endregion

			// So it will lean slightly towards the direction it's moving
			Projectile.rotation = Projectile.velocity.X * 0.01f;

			Color lightingColor = Color.White;
			float multiplier = 1f;
			if (LightingColor(ref lightingColor, ref multiplier))
			{
				Lighting.AddLight(Projectile.Center, lightingColor.ToVector3() * multiplier);
			}
		}

		public int GetRadius(Player player)
		{
			return (distRadius + player.GetModPlayer<RijamsModPlayer>().supportMinionRadiusIncrease) * 16;
		}

		public static bool SearchPlayers(Player player1, Player searchPlayer2)
		{
			if (searchPlayer2.active && !searchPlayer2.dead && !searchPlayer2.hostile && ((searchPlayer2.team == player1.team && searchPlayer2.team != 0 && Main.netMode != NetmodeID.SinglePlayer) || Main.netMode == NetmodeID.SinglePlayer))
			{
				return true;
			}
			return false;
		}
	}

	public class DefenseSupportSummonBase : SupportSummonBase
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(DefenseSupportSummonBase);
		public override string Texture => Projectile.type == ModContent.ProjectileType<DefenseSupportSummonBase>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');

		/// <summary>
		/// The defense that the summon provides to players.
		/// </summary>
		public int additionalDefense = 0;
		/// <summary>
		/// The damage reduction that the summon provides to players.
		/// </summary>
		public float additionalDR = 0;

		public override void AI()
		{
			base.AI();

			Player player = Main.player[Projectile.owner];
			int radius = GetRadius(player);
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player searchPlayer = Main.player[i];
				if (SearchPlayers(player, searchPlayer))
				{
					double distance = Vector2.Distance(searchPlayer.Center, Projectile.Center);
					if (distance <= radius)
					{
						searchPlayer.statDefense += additionalDefense;
						searchPlayer.endurance += additionalDR;
					}
				}
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(additionalDefense);
			writer.Write(additionalDR);
			writer.Write(distRadius);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			additionalDefense = reader.ReadInt32();
			additionalDR = reader.ReadSingle();
			distRadius = reader.ReadInt32();
		}
	}

	public class HealingSupportSummonBase : SupportSummonBase
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(HealingSupportSummonBase);
		public override string Texture => Projectile.type == ModContent.ProjectileType<HealingSupportSummonBase>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');

		/// <summary>
		/// The amount that is healed after period. It also affects regen.
		/// </summary>
		public int healAmount = 0;
		/// <summary>
		/// The cooldown between the big heals.
		/// </summary>
		public int cooldownTime = 0;
		/// <summary>
		/// <br>The player that targeted to be healed.</br>
		/// <br>If there was no player found, it will be -1.</br>
		/// </summary>
		internal int targetPlayer = -1;
		/// <summary>
		/// <br>The second player that targeted to be healed.</br>
		/// <br>If there was no player found, it will be -1.</br>
		/// </summary>
		internal int targetPlayer2 = -1;

		/// <summary>
		/// The projectile that is used to do the healing.
		/// </summary>
		/// <returns>Returns ProjectileID.None by default.</returns>
		public virtual int HealingProjectile()
		{
			return ProjectileID.None;
		}

		/// <summary>
		/// The sound to use when the healing projectile is shot.
		/// </summary>
		/// <returns>Returns <code>SoundID.Item8 with { Pitch = 0.5f }</code> by default</returns>
		public virtual SoundStyle LaunchSound()
		{
			return SoundID.Item8 with { Pitch = 0.5f };
		}

		/// <summary>
		/// Target two players instead of one for healing.
		/// </summary>
		/// <returns></returns>
		public virtual bool TargetTwoPlayers()
		{
			return false;
		}

		public override void PositionOffset(ref Vector2 offset)
		{
			offset = new(-60 * Main.player[Projectile.owner].direction, -60f);
		}

		public override bool LightingColor(ref Color lightColor, ref float multiplier)
		{
			return false;
		}

		public override void AI()
		{
			base.AI();

			Player player = Main.player[Projectile.owner];
			int radius = GetRadius(player);
			// Give passive regen for all players while within the radius.
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player searchPlayer = Main.player[i];
				if (SearchPlayers(player, searchPlayer))
				{
					double distance = Vector2.Distance(searchPlayer.Center, Projectile.Center);
					if (distance <= radius)
					{
						searchPlayer.lifeRegen += (healAmount / 10); // 1 = +0.5 HP per second
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
				if (!TargetTwoPlayers())
				{
					// Search for each player within the radius and add them to the list.
					if (Main.netMode != NetmodeID.Server)
					{
						Dictionary<Player, int> players = new();
						for (int i = 0; i < Main.maxPlayers; i++)
						{
							Player searchPlayer = Main.player[i];
							if (SearchPlayers(player, searchPlayer))
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
						int projectileType = HealingProjectile();

						Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, new(0, -1), projectileType,
							0, 0, Projectile.owner,
							targetPlayer, healAmount);
					}
				}
				else // TargetTwoPlayers
				{
					// Search for each player within the radius and add them to the list.
					if (Main.netMode != NetmodeID.Server)
					{
						Dictionary<Player, int> players = new();
						for (int i = 0; i < Main.maxPlayers; i++)
						{
							Player searchPlayer = Main.player[i];
							if (SearchPlayers(player, searchPlayer))
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
				}

				SoundEngine.PlaySound(LaunchSound(), Projectile.Center);

				// Set ai[0] back to 0.
				Projectile.ai[0] = 0;
				Projectile.frame = 0;
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
			if (TargetTwoPlayers())
			{
				writer.Write(targetPlayer2);
			}
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			cooldownTime = reader.ReadInt32();
			distRadius = reader.ReadInt32();
			targetPlayer = reader.ReadInt32();
			if (TargetTwoPlayers())
			{
				targetPlayer2 = reader.ReadInt32();
			}
		}
	}
}