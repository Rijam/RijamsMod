using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Magic
{
	public class LanternLightSulfuric : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 500;
		}

		public override bool BounceOnTiles() => false;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 10f;
			homingRange = 40 * 16; // 40 tiles
			timeBeforeItCanStartHoming = 440;
			timeLeftBeforeItStopsHoming = 60;
			homingNeedsLineOfSight = false;
		}

		public override Color OverrideColor()
		{
			return new Color(1f, 1f, 0.1f, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 11;
			shineScale = 0.75f;
		}

		public override bool Buffs(ref int buffType, ref int buffChance, ref int buffTime)
		{
			buffType = ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>();
			buffChance = 2;
			buffTime = 180;
			return true;
		}
	}

	public class LanternLightVile : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 300;
		}

		public override bool BounceOnTiles() => true;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 14f;
			homingRange = 30 * 16; // 30 tiles
			timeBeforeItCanStartHoming = 260;
			timeLeftBeforeItStopsHoming = 60;
			homingNeedsLineOfSight = true;
		}

		public override Color OverrideColor()
		{
			TorchID.TorchColor(TorchID.Cursed, out float r, out float g, out float b);
			return new Color(r, g, b, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 12;
			shineScale = 0.8f;
		}

		public override bool Buffs(ref int buffType, ref int buffChance, ref int buffTime)
		{
			buffType = BuffID.CursedInferno;
			buffChance = 4;
			buffTime = 240;
			return true;
		}
	}

	public class LanternLightVicious : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 300;
		}

		public override bool BounceOnTiles() => true;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 13f;
			homingRange = 30 * 16; // 30 tiles
			timeBeforeItCanStartHoming = 240;
			timeLeftBeforeItStopsHoming = 40;
			homingNeedsLineOfSight = true;
		}

		public override Color OverrideColor()
		{
			TorchID.TorchColor(TorchID.Ichor, out float r, out float g, out float b);
			return new Color(r, g, b, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 12;
			shineScale = 0.8f;
		}

		public override bool Buffs(ref int buffType, ref int buffChance, ref int buffTime)
		{
			buffType = BuffID.Ichor;
			buffChance = 1;
			buffTime = 240;
			return true;
		}
	}

	public class LanternLightRainbow : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 500;
		}

		public override bool BounceOnTiles() => true;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 15f;
			homingRange = 35 * 16; // 35 tiles
			timeBeforeItCanStartHoming = 440;
			timeLeftBeforeItStopsHoming = 60;
			homingNeedsLineOfSight = true;
		}

		public override Color OverrideColor()
		{
			return Main.DiscoColor * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 20;
			shineScale = 1f;
		}
	}

	public class LanternLightSpectre : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 500;
		}

		public override bool BounceOnTiles() => false;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 45f;
			homingRange = 40 * 16; // 40 tiles
			timeBeforeItCanStartHoming = 380;
			timeLeftBeforeItStopsHoming = 60;
			homingNeedsLineOfSight = false;
		}

		public override Color OverrideColor()
		{
			return new Color(0.5f, 0.9f, 1f, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 17;
			shineScale = 1f;
		}
	}

	public class LanternLightJackO : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 300;
		}

		public override bool BounceOnTiles() => true;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 20f;
			homingRange = 45 * 16; // 45 tiles
			timeBeforeItCanStartHoming = 240;
			timeLeftBeforeItStopsHoming = 30;
			homingNeedsLineOfSight = true;
		}

		public override Color OverrideColor()
		{
			return new Color(1f, 0.72f, 0.47f, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 15;
			shineScale = 0.9f;
		}
	}

	public class LanternLightAvoliteRed : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 500;
		}

		public override bool BounceOnTiles() => true;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 25f;
			homingRange = 50 * 16; // 50 tiles
			timeBeforeItCanStartHoming = 450;
			timeLeftBeforeItStopsHoming = 50;
			homingNeedsLineOfSight = true;
		}

		public override Color OverrideColor()
		{
			return new Color(1f, 0.47f, 0.59f, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 18;
			shineScale = 1.2f;
		}
	}

	public class LanternLightAvoliteYellow : LanternLightAvoliteRed
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 500;
		}

		public override bool BounceOnTiles() => false;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 10f;
			homingRange = 50 * 16; // 50 tiles
			timeBeforeItCanStartHoming = 400;
			timeLeftBeforeItStopsHoming = 70;
			homingNeedsLineOfSight = false;
		}

		public override Color OverrideColor()
		{
			return new Color(1f, 0.99f, 0.47f, 1f) * 0.5f;
		}
	}

	public class LanternLightAvoliteBlue : LanternLightAvoliteRed
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 500;
		}

		public override bool BounceOnTiles() => false;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 45f;
			homingRange = 50 * 16; // 50 tiles
			timeBeforeItCanStartHoming = 490;
			timeLeftBeforeItStopsHoming = 10;
			homingNeedsLineOfSight = true;
		}

		public override Color OverrideColor()
		{
			return new Color(0.47f, 0.82f, 1f, 1f) * 0.5f;
		}
	}

	public class LanternLightNebula : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 500;
		}

		public override bool BounceOnTiles() => true;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 30f;
			homingRange = 55 * 16; // 55 tiles
			timeBeforeItCanStartHoming = 470;
			timeLeftBeforeItStopsHoming = 30;
			homingNeedsLineOfSight = true;
		}

		public override Color OverrideColor()
		{
			return new Color(1f, 0.5f, 0.9f, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 19;
			shineScale = 1.5f;
		}
	}

	public class LanternLightAether : LanternLightBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 600;
		}

		public override bool BounceOnTiles() => false;

		public override void Movement(ref float vecolityMultiplier, ref int homingRange, ref float timeBeforeItCanStartHoming, ref float timeLeftBeforeItStopsHoming, ref bool homingNeedsLineOfSight)
		{
			vecolityMultiplier = 35f;
			homingRange = 60 * 16; // 60 tiles
			timeBeforeItCanStartHoming = 540f;
			timeLeftBeforeItStopsHoming = 30f;
			homingNeedsLineOfSight = false;
		}

		public override Color OverrideColor()
		{
			TorchID.TorchColor(TorchID.Shimmer, out float r, out float g, out float b);
			return new Color(r, g, b, 1f) * 0.5f;
		}

		public override void Trail(ref int trailLength, ref float shineScale)
		{
			trailLength = 20;
			shineScale = 1.75f;
		}
	}
}