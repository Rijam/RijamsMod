using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	[ReinitializeDuringResizeArrays]
	public class CustomProjectileIDSets
	{
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2211 // Non-constant fields should not be visible
		private const string RocketsAffectedByRocketBoosterExtraUpdatesKey = "RocketsAffectedByRocketBoosterExtraUpdates";

		/// <summary>
		/// <br>Rocket projectiles that are affected by the Rocket Booster accessory to add 1 extraUpdates to it.</br>
		/// <br>Not including Grenades, Proximity Mines, or the Celebration Rockets because extraUpdates causes them to:</br>
		/// <br>Grenades and Proximity Mines fall way faster which makes them have even less range.</br>
		/// <br>Celebration Rockets explode twice as soon (also they are shared with the placed colored firework Rockets)</br>
		/// <br>Exctrosphere Missile moves slow enough that it doesn't need extraUpdates.</br>
		/// </summary>
		public static bool[] RocketsAffectedByRocketBoosterExtraUpdates = ProjectileID.Sets.Factory.CreateNamedSet(RocketsAffectedByRocketBoosterExtraUpdatesKey)
			.Description("Rocket projectiles that are affected by the Rocket Booster accessory to add 1 extraUpdates to it.")
			.RegisterBoolSet(false,
				ProjectileID.RocketI, ProjectileID.RocketII, ProjectileID.RocketIII, ProjectileID.RocketIV,
				ProjectileID.RocketSnowmanI, ProjectileID.RocketSnowmanII, ProjectileID.RocketSnowmanIII, ProjectileID.RocketSnowmanIV,
				ProjectileID.ClusterRocketI, ProjectileID.ClusterRocketII, ProjectileID.DryRocket, ProjectileID.WetRocket,
				ProjectileID.LavaRocket, ProjectileID.HoneyRocket, ProjectileID.MiniNukeRocketI, ProjectileID.MiniNukeRocketII);

		private const string RocketBoosterExtraUpdatesBlackListKey = "RocketBoosterExtraUpdatesBlackList";
		/// <summary>
		/// Projectiles in this set will not be affected by the Rocket Booster accessory adding 1 extra update.
		/// </summary>
		public static bool[] RocketBoosterExtraUpdatesBlackList = ProjectileID.Sets.Factory.CreateNamedSet(RocketBoosterExtraUpdatesBlackListKey)
			.Description("Projectiles in this set will not be affected by the Rocket Booster accessory adding 1 extra update.")
			.RegisterBoolSet(false);

		private const string IsJoustingLanceProjectileKey = "IsJoustingLanceProjectile";
		/// <summary> This set is a set of all jousting lance projectiles. </summary>
		public static bool[] IsJoustingLanceProjectile = ProjectileID.Sets.Factory.CreateNamedSet(IsJoustingLanceProjectileKey)
			.Description("Denotes which projectiles are jousting lances.")
			.RegisterBoolSet(false,
				ProjectileID.JoustingLance,
				ProjectileID.HallowJoustingLance,
				ProjectileID.ShadowJoustingLance
			);

		private const string IsFireBasedAttackForFrostyRoseKey = "IsFireBasedAttackForFrostyRose";
		/// <summary> Denotes this projectile is based on fire so that the Frosty Rose can apply damage reduction. It is only for "hot" fire, so not frostburn related things. </summary>
		public static bool[] IsFireBasedAttackForFrostyRose = ProjectileID.Sets.Factory.CreateNamedSet(IsFireBasedAttackForFrostyRoseKey)
			.Description("Denotes this projectile is based on fire so that the Frosty Rose can apply damage reduction. It is only for \"hot\" fire, so not frostburn related things.")
			.RegisterBoolSet(false,
				ProjectileID.FireArrow,
				ProjectileID.BallofFire,
				ProjectileID.Flamelash,
				ProjectileID.Flamarang,
				ProjectileID.Sunfury,
				ProjectileID.HellfireArrow,
				ProjectileID.FlamingArrow,
				ProjectileID.CursedFlameFriendly,
				ProjectileID.CursedFlameHostile,
				ProjectileID.EyeFire,
				ProjectileID.CursedArrow,
				ProjectileID.CursedBullet,
				ProjectileID.FlamethrowerTrap,
				ProjectileID.FlamesTrap,
				ProjectileID.Fireball,
				ProjectileID.InfernoFriendlyBolt,
				ProjectileID.InfernoFriendlyBlast,
				ProjectileID.InfernoHostileBolt,
				ProjectileID.InfernoHostileBlast,
				ProjectileID.JackOLantern,
				ProjectileID.FlamingJack,
				ProjectileID.FlamingWood,
				ProjectileID.GreekFire1,
				ProjectileID.GreekFire2,
				ProjectileID.GreekFire3,
				ProjectileID.FlamingScythe,
				ProjectileID.ImpFireball,
				ProjectileID.MolotovCocktail,
				ProjectileID.MolotovFire,
				ProjectileID.MolotovFire2,
				ProjectileID.MolotovFire3,
				ProjectileID.CultistBossFireBall,
				ProjectileID.CursedDart,
				ProjectileID.CursedDartFlame,
				ProjectileID.ClingerStaff,
				ProjectileID.Hellwing,
				ProjectileID.ShadowFlameArrow,
				ProjectileID.ShadowFlame,
				ProjectileID.ShadowFlameKnife,
				ProjectileID.Spark,
				ProjectileID.HelFire,
				ProjectileID.DesertDjinnCurse,
				ProjectileID.SolarFlareRay,
				ProjectileID.SolarCounter,
				ProjectileID.SolarWhipSword,
				ProjectileID.SolarWhipSwordExplosion,
				ProjectileID.Daybreak,
				ProjectileID.GeyserTrap,
				ProjectileID.SpiritFlame,
				ProjectileID.DD2FlameBurstTowerT1Shot,
				ProjectileID.DD2FlameBurstTowerT2Shot,
				ProjectileID.DD2FlameBurstTowerT3Shot,
				ProjectileID.DD2DrakinShot,
				ProjectileID.DD2BetsyFireball,
				ProjectileID.DD2BetsyFlameBreath,
				ProjectileID.MonkStaffT2,
				ProjectileID.MonkStaffT2Ghast,
				ProjectileID.DD2PhoenixBow,
				ProjectileID.DD2PhoenixBowShot,
				ProjectileID.DD2BetsyArrow,
				ProjectileID.ApprenticeStaffT3Shot,
				ProjectileID.ShadowJoustingLance,
				ProjectileID.LavaBomb,
				ProjectileID.FireWhip,
				ProjectileID.FireWhipProj,
				ProjectileID.FlamingMace,
				ProjectileID.TorchGod,
				ProjectileID.DaybreakExplosion,
				ProjectileID.Volcano,
				ProjectileID.HorsemanPumpkin,
				ProjectileID.PalworldMinionFoxsparks,
				ProjectileID.PalworldMinionFoxsparksFireball,
				ProjectileID.ObsidianFire,
				ProjectileID.ObsidianFire2,
				ProjectileID.ObsidianFire3,
				ProjectileID.PalworldMinionFoxsparksFlames,
				ProjectileID.PalworldMinionTrustyFoxsparks,
				ProjectileID.SnappingStoneMeleeEffect
			);
#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning restore IDE0079 // Remove unnecessary suppression
	}
}