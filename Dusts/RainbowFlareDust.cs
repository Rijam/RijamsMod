using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Dusts
{
	public class RainbowFlareDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.scale *= 0.5f;
		}
		public override bool MidUpdate(Dust dust)
		{
			dust.velocity *= 0.95f;
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.05f;
			}
			/* OG RainbowTorch dust
			if (dust.velocity.X < 0f)
				dust.rotation -= 1f;
			else
				dust.rotation += 1f;

			dust.velocity.Y *= 0.98f;
			dust.velocity.X *= 0.98f;
			dust.scale += 0.02f;
			*/
			float num90 = dust.scale * 0.8f;
			if (num90 > 1f)
				num90 = 1f;
			
			if (!dust.noLight && !dust.noLightEmittance)
			{
				Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num90 * ((float)(int)dust.color.R / 255f), num90 * ((float)(int)dust.color.G / 255f), num90 * ((float)(int)dust.color.B / 255f));
			}
			dust.color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.5f);
			return base.MidUpdate(dust);
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color(lightColor.R, lightColor.G, lightColor.B, 0);
		}
	}
}