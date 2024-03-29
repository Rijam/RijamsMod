using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Support
{
	public class HarpyIdol : ModProjectile
	{
		public int additionalDefense = 0;
		public float additionalDR = 0;
		public int distRadius = 0;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Harpy Idol");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[Projectile.type] = 6;
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
			Projectile.width = 30;
			Projectile.height = 28;
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

			DrawOffsetX = -10;
			DrawOriginOffsetY -= 10;
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
				player.ClearBuff(ModContent.BuffType<Buffs.Minions.HarpyIdolBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.Minions.HarpyIdolBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			#endregion

			#region AI

			Vector2 yOffset = new(0, -45f);
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

			int radius = (distRadius + player.GetModPlayer<RijamsModPlayer>().supportMinionRadiusIncrease) * 16; // 5 tiles
			if (Main.netMode != NetmodeID.Server)
			{
				RijamsModConfigClient configClient = ModContent.GetInstance<RijamsModConfigClient>();
				if (configClient.DisplayDefenseSupportSummonsAura != RijamsModConfigClient.SupportSummonsAura.Off)
				{
					for (int i = 0; i < 20; i++)
					{
						Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
						int alpha = configClient.DisplayDefenseSupportSummonsAura switch
						{
							RijamsModConfigClient.SupportSummonsAura.Opaque => 0,
							RijamsModConfigClient.SupportSummonsAura.Normal => 150,
							RijamsModConfigClient.SupportSummonsAura.Faded => 240,
							RijamsModConfigClient.SupportSummonsAura.Off => 255,
							_ => 150,
						};
						Dust d = Dust.NewDustPerfect(Projectile.Center + speed * radius, ModContent.DustType<Dusts.AuraDust>(), speed, alpha, Color.Gold, 0.75f);
						d.noGravity = true;
						d.noLightEmittence = true;
					}
				}
			}

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

			#endregion

			#region Animation and visuals
			// So it will lean slightly towards the direction it's moving
			Projectile.rotation = Projectile.velocity.X * 0.01f;

			Projectile.spriteDirection = 1;

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 5;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed) {
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type]) {
					Projectile.frame = 0;
				}
			}

			// Some visuals here
			Lighting.AddLight(Projectile.Center, Color.Gold.ToVector3() * 0.05f);
			#endregion
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

		public static bool SearchPlayers(Player player1, Player searchPlayer2)
		{
			if (searchPlayer2.active && !searchPlayer2.dead && !searchPlayer2.hostile && ((searchPlayer2.team == player1.team && searchPlayer2.team != 0 && Main.netMode != NetmodeID.SinglePlayer) || Main.netMode == NetmodeID.SinglePlayer))
			{
				return true;
			}
			return false;
		}
	}
}