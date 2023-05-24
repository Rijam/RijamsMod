using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Magic
{
	public class LanternLightShining : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 500;
		}

		public override bool BounceOnTiles() => false;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 5f;
			homingRange = 20 * 16; // 20 tiles
			timeBeforeItCanStartHoming = 380;
			timeLeftBeforeItStopsHoming = 60;
		}

		public override Color OverrideColor()
		{
			TorchID.TorchColor(TorchID.Torch, out float r, out float g, out float b);
			return new Color(r, g, b, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 7;
			shineScale = 0.5f;
		}
	}

	public class LanternLightFrostburn : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 500;
			Projectile.coldDamage = true;
		}

		public override bool BounceOnTiles() => false;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 6f;
			homingRange = 25 * 16; // 25 tiles
			timeBeforeItCanStartHoming = 420;
			timeLeftBeforeItStopsHoming = 60;
		}

		public override Color OverrideColor()
		{
			TorchID.TorchColor(TorchID.Ice, out float r, out float g, out float b);
			return new Color(r, g, b, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 8;
			shineScale = 0.6f;
		}

		public override bool Buffs(ref int buffType, ref int buffChance, ref int buffTime)
		{
			buffType = BuffID.Frostburn;
			buffChance = 2;
			buffTime = 240;
			return true;
		}
	}

	public class LanternLightUltrabright : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 500;
		}

		public override bool BounceOnTiles() => true;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 7f;
			homingRange = 25 * 16; // 25 tiles
			timeBeforeItCanStartHoming = 420;
			timeLeftBeforeItStopsHoming = 30;
		}

		public override Color OverrideColor()
		{
			TorchID.TorchColor(TorchID.UltraBright, out float r, out float g, out float b);
			return new Color(r, g, b, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 9;
			shineScale = 0.6f;
		}
	}

	public class LanternLightPeace : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 500;
		}

		public override bool BounceOnTiles() => true;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 7f;
			homingRange = 30 * 16; // 30 tiles
			timeBeforeItCanStartHoming = 440;
			timeLeftBeforeItStopsHoming = 30;
		}

		public override Color OverrideColor()
		{
			return new Color(0.9f, 0.1f, 0.75f, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 9;
			shineScale = 0.7f;
		}
	}

	public class LanternLightWater : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 440;
		}

		public override bool BounceOnTiles() => true;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 8f;
			homingRange = 25 * 16; // 25 tiles
			timeBeforeItCanStartHoming = 380;
			timeLeftBeforeItStopsHoming = 60;
		}

		public override Color OverrideColor()
		{
			return new Color(0f, 0.35f, 0.8f, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 10;
			shineScale = 0.7f;
		}
	}

	public class LanternLightShadow : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 440;
		}

		public override bool BounceOnTiles() => true;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 6f;
			homingRange = 35 * 16; // 35 tiles
			timeBeforeItCanStartHoming = 420;
			timeLeftBeforeItStopsHoming = 20;
		}

		public override Color OverrideColor()
		{
			return new Color(0.2f, 0.3f, 0.32f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 8;
			shineScale = 0.7f;
		}
	}
}