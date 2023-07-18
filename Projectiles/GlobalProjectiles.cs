using Microsoft.Xna.Framework;
using RijamsMod.Buffs.Potions;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class RijamsModProjectile : GlobalProjectile
	{
		public static List<int> RocketsAffectedByRocketBoosterExtraUpdates = new()
			{ ProjectileID.RocketI, ProjectileID.RocketII, ProjectileID.RocketIII, ProjectileID.RocketIV,
			ProjectileID.RocketSnowmanI, ProjectileID.RocketSnowmanII, ProjectileID.RocketSnowmanIII, ProjectileID.RocketSnowmanIV,
			ProjectileID.ClusterRocketI, ProjectileID.ClusterRocketII, ProjectileID.DryRocket, ProjectileID.WetRocket,
			ProjectileID.LavaRocket, ProjectileID.HoneyRocket, ProjectileID.MiniNukeRocketI, ProjectileID.MiniNukeRocketII };
		//Not including Grenades, Proximity Mines, or the Celebration Rockets because extraUpdates causes them to:
		//	Grenades and Proximity Mines fall way faster which makes them have even less range.
		//	Celebration Rockets explode twice as soon (also they are shared with the placed colored firework Rockets)
		//  Exctrosphere Missile moves slow enough that it doesn't need extraUpdates.
		public static List<int> RocketBoosterExtraUpdatesBlackList = new() {  };

		public override void PostAI(Projectile projectile)
		{
			Player owner = Main.player[projectile.owner];
			if (owner != null)
			{
				RijamsModPlayer moddedplayer = owner.GetModPlayer<RijamsModPlayer>();
				if (projectile.CountsAsClass(DamageClass.Melee))
				{
					if (moddedplayer.daybreakStone)
					{
						if (projectile.friendly && !projectile.hostile && !projectile.noEnchantmentVisuals && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
						{
							int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SolarFlare, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default, 1f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 0.7f;
							Main.dust[dust].velocity.Y -= 0.5f;
							Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.875f);
						}
					}
					if (moddedplayer.frostburnStone)
					{
						if (projectile.friendly && !projectile.hostile && !projectile.noEnchantmentVisuals && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
						{
							int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Frost, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default, 1f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 0.7f;
							Main.dust[dust].velocity.Y -= 0.5f;
							Lighting.AddLight(projectile.Center, Color.LightBlue.ToVector3() * 0.875f);
						}
					}
					if (moddedplayer.flaskBuff >= 1 && projectile.friendly && !projectile.hostile && !projectile.noEnchantmentVisuals && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
					{
						int dustType = DustID.Dirt;
						if (moddedplayer.flaskBuff == FlaskIDs.SulfuricAcid)
						{
							dustType = ModContent.DustType<Dusts.SulfurDust>();
						}
						if (moddedplayer.flaskBuff == FlaskIDs.Oiled)
						{
							dustType = DustID.Asphalt;
						}
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default, 1f);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0.7f;
						Main.dust[dust].velocity.Y -= 0.5f;
						Lighting.AddLight(projectile.Center, Color.LightBlue.ToVector3() * 0.1f);
					}
					if (moddedplayer.flaskBuff == FlaskIDs.Oiled)
					{
						if (projectile.friendly && !projectile.hostile && !projectile.noEnchantmentVisuals && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
						{
							int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Asphalt, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default, 1f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 0.7f;
							Main.dust[dust].velocity.Y -= 0.5f;
							Lighting.AddLight(projectile.Center, Color.LightBlue.ToVector3() * 0.1f);
						}
					}
				}
			}
			if (projectile.type == ProjectileID.JoustingLance)
			{
				// The Hallowed and Shadow Jousting Lance spawn dusts when the player is moving at a certain speed.
				float minimumPlayerVelocity = 6f;
				float minimumSpeedX = 0.8f;
				float speedX = Vector2.Dot(projectile.velocity.SafeNormalize(Vector2.UnitX * owner.direction), owner.velocity.SafeNormalize(Vector2.UnitX * owner.direction));
				float playerVelocity = owner.velocity.Length();
				if (playerVelocity > minimumPlayerVelocity && speedX > minimumSpeedX)
				{
					// The chance for the dust to spawn. The actual chance (see below) is 1/dustChance. We make the chance higher the faster the player is moving by making the denominator smaller.
					int dustChance = 8;
					if (playerVelocity > minimumPlayerVelocity + 1f)
					{
						dustChance = 5;
					}
					if (playerVelocity > minimumPlayerVelocity + 2f)
					{
						dustChance = 2;
					}

					// Set your dust types here.
					int dustTypeCommon = DustID.Iron;
					int dustTypeRare = DustID.Lead;

					int offset = 2; // This offset will affect how much the dust spreads out.

					// Spawn the dusts based on the dustChance. The dusts are spawned at the tip of the Jousting Lance.
					if (Main.rand.NextBool(dustChance))
					{
						int newDust = Dust.NewDust(projectile.Center - new Vector2(offset, offset), offset * 2, offset * 2, dustTypeCommon, projectile.velocity.X * 0.2f + (projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default, 0.5f);
						Main.dust[newDust].noGravity = true;
						Main.dust[newDust].velocity *= 0.25f;
						newDust = Dust.NewDust(projectile.Center - new Vector2(offset, offset), offset * 2, offset * 2, dustTypeCommon, 0f, 0f, 150, default, 0.75f);
						Main.dust[newDust].velocity *= 0.25f;
					}

					if (Main.rand.NextBool(dustChance + 3))
					{
						Dust.NewDust(projectile.Center - new Vector2(offset, offset), offset * 2, offset * 2, dustTypeRare, 0f, 0f, 150, default, 1.0f);
					}
				}
			}
		}
		public override bool PreAI(Projectile projectile)
		{
			Player owner = Main.player[projectile.owner];
			if (owner != null && owner.whoAmI != Main.maxPlayers)
			{
				if (owner.active && owner.GetModPlayer<RijamsModPlayer>().rocketBooster)
				{
					if (owner.HeldItem.useAmmo == AmmoID.Rocket &&
						(RocketsAffectedByRocketBoosterExtraUpdates.Contains(projectile.type) || ProjectileID.Sets.IsARocketThatDealsDoubleDamageToPrimaryEnemy[projectile.type])
						&& !RocketBoosterExtraUpdatesBlackList.Contains(projectile.type) && !ProjectileID.Sets.IsAMineThatDealsTripleDamageWhenStationary[projectile.type])
					{
						if (projectile.extraUpdates == 0)
						{
							//Main.NewText("rocketBooster GlobalProjectile");
							projectile.velocity *= 0.5f; // Doesn't really do anything because the velocity is continuously multiplied by 1.1f.
							projectile.extraUpdates++;
						}
					}
				}
				if (owner.active && owner.GetModPlayer<RijamsModPlayer>().yoyoBackpack && projectile.aiStyle == 99)
				{
					//Main.NewText("yoyoBackpack GlobalProjectile");
					projectile.localAI[0] = -1;
					if (projectile.type == ProjectileID.BlackCounterweight + Main.rand.Next(6))
					{
						projectile.scale = 1.5f;
						projectile.width = 15;
						projectile.height = 15;
					}
				}
			}
			return base.PreAI(projectile);
		}
		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (projectile.type == ProjectileID.ShadowJoustingLance)
			{
				target.AddBuff(BuffID.ShadowFlame, damageDone * 2);
			}
		}
		public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
		{
			if (projectile.type == ProjectileID.ShadowJoustingLance)
			{
				target.AddBuff(BuffID.ShadowFlame, info.Damage * 2);
			}
		}
	}
}