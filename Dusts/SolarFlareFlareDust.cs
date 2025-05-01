using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Dusts
{
	public class SolarFlareFlareDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.scale *= 0.5f;
		}
		public override bool MidUpdate(Dust dust)
		{
			dust.velocity *= 0.97f;
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.05f;
			}

			if (!dust.noLight && !dust.noLightEmittence)
			{
				Lighting.AddLight(dust.position, new Color(255, 230, 150).ToVector3());
			}

			/* OG SolarFlare Dust
			if ((int)dust?.customData == 0)
			{
				if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
				{
					dust.scale *= 0.9f;
					dust.velocity *= 0.25f;
				}
			}
			else if ((int)dust?.customData == 1)
			{
				dust.scale *= 0.98f;
				dust.velocity.Y *= 0.98f;
				if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
				{
					dust.scale *= 0.9f;
					dust.velocity *= 0.25f;
				}
			}
			*/

			return base.MidUpdate(dust);
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color(230, 230, 230, 230);
		}
	}
}