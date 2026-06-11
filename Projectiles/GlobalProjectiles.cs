using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class RijamsModProjectile : GlobalProjectile
	{
		// public static List<int> RocketsAffectedByRocketBoosterExtraUpdates = new()
		// 	{ ProjectileID.RocketI, ProjectileID.RocketII, ProjectileID.RocketIII, ProjectileID.RocketIV,
		// 	ProjectileID.RocketSnowmanI, ProjectileID.RocketSnowmanII, ProjectileID.RocketSnowmanIII, ProjectileID.RocketSnowmanIV,
		// 	ProjectileID.ClusterRocketI, ProjectileID.ClusterRocketII, ProjectileID.DryRocket, ProjectileID.WetRocket,
		// 	ProjectileID.LavaRocket, ProjectileID.HoneyRocket, ProjectileID.MiniNukeRocketI, ProjectileID.MiniNukeRocketII };
		//Not including Grenades, Proximity Mines, or the Celebration Rockets because extraUpdates causes them to:
		//	Grenades and Proximity Mines fall way faster which makes them have even less range.
		//	Celebration Rockets explode twice as soon (also they are shared with the placed colored firework Rockets)
		//  Exctrosphere Missile moves slow enough that it doesn't need extraUpdates.
		// public static List<int> RocketBoosterExtraUpdatesBlackList = new() {  };
		

		public override void SetDefaults(Projectile projectile)
		{
			if (ModContent.GetInstance<RijamsModConfigServer>().JoustingLanceStaticInvincibility)
			{
				/*
				foreach (int itemType in CustomItemIDSets.IsJoustingLance)
				{
					if (itemType > 0 && itemType < ItemLoader.ItemCount)
					{
						if (ContentSamples.ItemsByType[itemType].shoot == projectile.type)
						{
							projectile.usesIDStaticNPCImmunity = true;
							projectile.idStaticNPCHitCooldown = 10;
						}
					}
					else
					{
						Mod.Logger.WarnFormat("Warning: Item Type {0} exceeded ItemLoader.ItemCount", itemType);
					}
				}
				*/
				try
				{
					if (CustomProjectileIDSets.IsJoustingLanceProjectile[projectile.type])
					{
						projectile.usesIDStaticNPCImmunity = true;
						projectile.idStaticNPCHitCooldown = 10;
					}
				}
				catch
				{
					Mod.Logger.WarnFormat("Warning: OOB error for {0} when trying to set the static immunity for CustomProjectileIDSets.IsJoustingLanceProjectile", projectile.type);
				}
				
				/*
				if (GlobalItems.isJoustingLance.ContainsValue(projectile.type))
				{
					projectile.usesIDStaticNPCImmunity = true;
					projectile.idStaticNPCHitCooldown = 10;
				}
				*/
			}
			if (ModContent.GetInstance<RijamsModConfigServer>().YoyoStaticInvincibility)
			{
				try
				{
					if (ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] != -1 || projectile.aiStyle == ProjAIStyleID.Yoyo)
					{
						projectile.usesIDStaticNPCImmunity = true;
						projectile.idStaticNPCHitCooldown = 10;
					}
				}
				catch
				{
					Mod.Logger.WarnFormat("Warning: Error for {0} when trying to set the static immunity for Yoyo AI projectile", projectile.type);
				}
			}
		}

		public override void PostAI(Projectile projectile)
		{
			Player owner = Main.player[projectile.owner];
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
						(CustomProjectileIDSets.RocketsAffectedByRocketBoosterExtraUpdates[projectile.type] || ProjectileID.Sets.IsARocketThatDealsDoubleDamageToPrimaryEnemy[projectile.type])
						&& !CustomProjectileIDSets.RocketBoosterExtraUpdatesBlackList[projectile.type] && !ProjectileID.Sets.IsAMineThatDealsTripleDamageWhenStationary[projectile.type])
					{
						if (projectile.extraUpdates == 0)
						{
							//Main.NewText("rocketBooster GlobalProjectile");
							projectile.velocity *= 0.5f; // Doesn't really do anything because the velocity is continuously multiplied by 1.1f.
							projectile.extraUpdates++;
						}
					}
				}
				// Yoyo related things
				if (owner.active && projectile.aiStyle == ProjAIStyleID.Yoyo)
				{
					if (owner.GetModPlayer<RijamsModPlayer>().yoyoBackpack && projectile.counterweight)
					{
						projectile.scale *= 1.5f;
						projectile.Resize((int)(projectile.width * 1.5f), (int)(projectile.height * 1.5f));
					}
					if (owner.GetModPlayer<RijamsModPlayer>().loopingOil)
					{
						projectile.localAI[0] = -1;
					}
					if (owner.GetModPlayer<RijamsModPlayer>().sideEffects && projectile.extraUpdates == 0 && !projectile.counterweight)
					{
						projectile.velocity *= 0.5f; // Doesn't really do anything because the Yoyo follows the mouse and this is only applied once.
						projectile.extraUpdates++;
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

		public static void DrawLineForWhips(List<Vector2> controlPoints, Color lineColor, bool decrementCount = false)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new(frame.Width / 2, 2);

			Vector2 pos = controlPoints[0];
			int maxCount = controlPoints.Count - 1;
			if (decrementCount)
			{
				maxCount--;
			}
			// If you whip has a long range and this line is poking out of the front, use list.Count - 2 instead of list.Count - 1.
			for (int i = 0; i < maxCount; i++)
			{
				Vector2 element = controlPoints[i];
				Vector2 diff = controlPoints[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates(), lineColor);
				Vector2 scale = new(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}
	}
}