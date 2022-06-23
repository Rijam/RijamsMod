using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
    public class RijamsModProjectile : GlobalProjectile
    {
		public override void PostAI(Projectile projectile)
		{
			Player owner = Main.player[projectile.owner];
			if (owner != null)
			{
				if (projectile.CountsAsClass(DamageClass.Melee))
				{
					RijamsModPlayer moddedplayer = owner.GetModPlayer<RijamsModPlayer>();

					if (moddedplayer.daybreakStone)
					{
						if (projectile.friendly && !projectile.hostile && !projectile.noEnchantments && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
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
						if (projectile.friendly && !projectile.hostile && !projectile.noEnchantments && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
						{
							int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Frost, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default, 1f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 0.7f;
							Main.dust[dust].velocity.Y -= 0.5f;
							Lighting.AddLight(projectile.Center, Color.LightBlue.ToVector3() * 0.875f);
						}
					}
					if (moddedplayer.flaskBuff == 1)
					{
						if (projectile.friendly && !projectile.hostile && !projectile.noEnchantments && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
						{
							int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.SulfurDust>(), projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default, 1f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 0.7f;
							Main.dust[dust].velocity.Y -= 0.5f;
							Lighting.AddLight(projectile.Center, Color.LightBlue.ToVector3() * 0.1f);
						}
					}
				}
			}
		}
        public override bool PreAI(Projectile projectile)
        {
			Player owner = Main.player[projectile.owner];
			if (owner != null && owner.GetModPlayer<RijamsModPlayer>().rocketBooster)
			{
				short[] types = {ProjectileID.RocketI, ProjectileID.RocketII, ProjectileID.RocketIII, ProjectileID.RocketIV,
							ProjectileID.RocketSnowmanI, ProjectileID.RocketSnowmanII, ProjectileID.RocketSnowmanIII, ProjectileID.RocketSnowmanIV};
							//Not including Grenades, Proximity Mines, or the Celebration Rockets because extraUpdates causes them to:
							//	Grenades and Proximity Mines fall way faster which makes them have even less range.
							//	Celebration Rockets explode twice as soon (also they are shared with the placed colored firework Rockets)
							//  Exctrosphere Missile moves slow enough that it doesn't need extraUpdates.
							//Need to add all of the new 1.4 rocket types (liquid rockets & Celebration MkII)
				foreach (short element in types)
				{
					if (owner.HeldItem.useAmmo == AmmoID.Rocket && projectile.type == element)
					{
						if (projectile.extraUpdates == 0)
						{
							//Main.NewText("rocketBooster GlobalProjectile");
							projectile.extraUpdates++;
							projectile.velocity *= 0.5f;
						}
					}
				}
			}
			if (owner != null && owner.GetModPlayer<RijamsModPlayer>().yoyoBackpack && projectile.aiStyle == 99)
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
			return base.PreAI(projectile);
        }
    }
}