using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Support
{
	public class StardustProtector : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Protector");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[Projectile.type] = 8;
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[Projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 52;
			Projectile.height = 48;
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

			Projectile.alpha = 200;
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

		private bool attacking = false;
		public int baseDamage = 20;
		public int baseAttackSpeed = 420; // 7 seconds
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			#region Active check
			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<Buffs.StardustProtectorBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.StardustProtectorBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			#endregion

			#region AI

			Vector2 yOffset = new(0, -60f)
			{
				X = player.direction == 1 ? -30f : 30f
			};
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

			int radius = 30 * 16; // 30 tiles
			for (int i = 0; i < 70; i++)
			{
				Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
				Dust d = Dust.NewDustPerfect(Projectile.Center + speed * radius, ModContent.DustType<Dusts.AuraDust>(), speed, 150, Color.LightBlue, 0.75f);
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
						searchPlayer.statDefense += 15;
						searchPlayer.endurance += 0.1f;
					}
				}
			}

			if (Projectile.ai[0] > 0)
			{
				Projectile.ai[0]--;
			}

			#endregion

			#region Find target
			// Starting search distance

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float distance = Vector2.Distance(npc.Center, Projectile.Center);
				if (distance <= radius && Projectile.ai[0] <= 0)
				{
					Projectile.frame = 4;
					attacking = true;
					Projectile.ai[0] = (int)(baseAttackSpeed * (1f / player.GetAttackSpeed(DamageClass.Summon)));
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						npc.StrikeNPCNoInteraction((int)(baseDamage * Math.Round(player.GetTotalDamage(DamageClass.Summon).Additive * player.GetTotalDamage(DamageClass.Summon).Multiplicative, 2)),
							0f, npc.direction, false, true, true);
					}
					else
					{
						NetMessage.SendData(MessageID.DamageNPC, number: npc.whoAmI,
							number2: (int)(baseDamage * Math.Round(player.GetTotalDamage(DamageClass.Summon).Additive * player.GetTotalDamage(DamageClass.Summon).Multiplicative, 2)),
							number3: 0f, number4: npc.direction, number5: 0);
						npc.netUpdate = true;
					}
					for (int j = 0; j < 20; j++)
					{
						Dust.NewDust(npc.Center, npc.width / 2, npc.height / 2, DustID.YellowStarDust, 0f, 0f, 200, Color.White, 1f);
					}
				}
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

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 8;
			Projectile.frameCounter++;
			if (!attacking)
			{
				if (Projectile.frameCounter >= frameSpeed)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;
					if (Projectile.frame >= 4)
					{
						Projectile.frame = 0;
					}
				}
			}
			if (attacking)
			{
				if (Projectile.frameCounter >= frameSpeed)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;
					if (Projectile.frame >= Main.projFrames[Projectile.type])
					{
						Projectile.frame = 0;
						attacking = false;
					}
				}
			}
			// Some visuals here
			Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.5f);
			#endregion
		}

		public override Color? GetAlpha(Color lightColor) => new(255, 255, 255 , 255 - Projectile.alpha);

		public override bool PreDraw(ref Color lightColor)
		{
			// Get texture of projectile
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

			//Redraw the projectile with the color not influenced by light

			Vector2 drawOrigin = new(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				//Main.EntitySpriteDraw(texture, drawPos, sourceRectangle, color, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0);
			}
			return true;
		}
	}
}