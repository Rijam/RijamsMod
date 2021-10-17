using Microsoft.Xna.Framework;
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
				if (projectile.melee)
				{
					RijamsModPlayer moddedplayer = owner.GetModPlayer<RijamsModPlayer>();

					if (moddedplayer.daybreakStone)
					{
						if (projectile.friendly && !projectile.hostile && !projectile.noEnchantments && Main.rand.Next(2 * (1 + projectile.extraUpdates)) == 0)
						{
							int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 259, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 0.7f;
							Main.dust[dust].velocity.Y -= 0.5f;
							Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.875f);
						}
					}
				}
			}
		}
	}
}