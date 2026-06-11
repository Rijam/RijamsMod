using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Dusts
{
	// Unused currently
	public class ClankDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, 0, 24, 24);
			dust.scale = 1f;
		}

		public override bool Update(Dust dust)
		{
			dust.position.Y += 0.5f;
			dust.rotation = 0f;
			dust.scale *= 0.97f;
			dust.fadeIn++;

			if (5 < dust.fadeIn && dust.fadeIn <= 10)
			{
				dust.frame = new Rectangle(0, 24, 24, 24);
			}
			if (dust.fadeIn > 10)
			{
				dust.frame = new Rectangle(0, 48, 24, 24);
			}

			if (dust.noLight || dust.noLightEmittance)
			{
				return true;
			}

			float strength = dust.scale * (1 - (dust.alpha / 255f));
			if (strength > 1f)
			{
				strength = 1f;
			}

			Lighting.AddLight(dust.position, 0.06f * strength, 0.13f * strength, 0.98f * strength);

			if (dust.scale <= 0f || dust.color == Color.Black || dust.fadeIn >= 15)
			{
				dust.active = false;
			}
			return false;
		}

		public override bool MidUpdate(Dust dust)
		{

			return true;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return base.GetAlpha(dust, lightColor);
		}
	}
}