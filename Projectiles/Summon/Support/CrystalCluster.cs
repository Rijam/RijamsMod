using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Support
{
	public class CrystalCluster : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Cluster");
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
			Projectile.width = 34;
			Projectile.height = 44;
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
				player.ClearBuff(ModContent.BuffType<Buffs.CrystalClusterBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.CrystalClusterBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			#endregion

			#region AI

			Vector2 yOffset = new(0, -60f);
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

			int radius = 15 * 16; // 15 tiles
			for (int i = 0; i < 60; i++)
			{
				Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
				Dust d = Dust.NewDustPerfect(Projectile.Center + speed * radius, ModContent.DustType<Dusts.AuraDust>(), speed, 150, Color.Magenta, 0.75f);
				d.noGravity = true;
				d.noLightEmittence = true;
			}

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player searchPlayer = Main.player[i];
				if (searchPlayer.active || !searchPlayer.dead || !searchPlayer.hostile || (searchPlayer.team == player.team && searchPlayer.team != 0))
				{
					double distance = Vector2.Distance(searchPlayer.Center, Projectile.Center);
					if (distance <= radius)
					{
						searchPlayer.statDefense += 7;
						searchPlayer.endurance += 0.04f;
					}
				}
			}

			#endregion

			#region Animation and visuals
			// So it will lean slightly towards the direction it's moving
			Projectile.rotation = Projectile.velocity.X * 0.01f;

			Projectile.spriteDirection = 1;

			// Some visuals here
			Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() * 0.25f);
			#endregion
		}

		private int fadeInOrOut = 0;
		private Color lerpColor = Color.White;
		public override void PostDraw(Color lightColor)
		{
			Projectile.ai[1] += 1;

			// Get texture of projectile
			Texture2D texture = ModContent.Request<Texture2D>((GetType().Namespace + "." + Name + "Bright").Replace('.', '/')).Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

			if (fadeInOrOut == 0)
			{
				lerpColor = Color.Lerp(Color.White, new(0, 0, 0, 0), Projectile.ai[1] / 100f);
			}
			if (fadeInOrOut == 1)
			{
				lerpColor = Color.Lerp(new(0, 0, 0, 0), Color.White, Projectile.ai[1] / 100f);
			}

			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, lerpColor, Projectile.rotation, sourceRectangle.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

			if (Projectile.ai[1] > 100)
			{
				fadeInOrOut++;
				Projectile.ai[1] = 0;
				if (fadeInOrOut > 1)
				{
					fadeInOrOut = 0;
				}
			}
		}
	}
}