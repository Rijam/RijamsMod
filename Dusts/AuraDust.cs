using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Dusts
{
	public class AuraDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.frame = new Rectangle(0, 0, 10, 10);
		}

		public override bool MidUpdate(Dust dust)
		{
			if (dust.noLight || dust.noLightEmittence)
			{
				return false;
			}

			float strength = dust.scale * (1 - (dust.alpha / 255f));
			if (strength > 1f)
			{
				strength = 1f;
			}

			Vector3 newColor = dust.color.ToVector3();

			Lighting.AddLight(dust.position, newColor.X * strength, newColor.Y * strength, newColor.Z * strength);
			return false;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			if (!dust.noLight)
			{
				return dust.color * (1 - (dust.alpha / 255f));
			}
			return base.GetAlpha(dust, lightColor);
		}
	}
}