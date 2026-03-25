using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	[ReinitializeDuringResizeArrays]
	public class CustomProjectileIDSets
	{
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public const string RocketsAffectedByRocketBoosterExtraUpdatesKey = "RocketsAffectedByRocketBoosterExtraUpdates";

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

		public const string RocketBoosterExtraUpdatesBlackListKey = "RocketBoosterExtraUpdatesBlackList";
		/// <summary>
		/// Projectiles in this set will not be affected by the Rocket Booster accessory adding 1 extra update.
		/// </summary>
		public static bool[] RocketBoosterExtraUpdatesBlackList = ProjectileID.Sets.Factory.CreateNamedSet(RocketBoosterExtraUpdatesBlackListKey)
			.Description("Projectiles in this set will not be affected by the Rocket Booster accessory adding 1 extra update.")
			.RegisterBoolSet(false);

		public const string IsJoustingLanceProjectileKey = "IsJoustingLanceProjectile";
		/// <summary> This set is a set of all jousting lance projectiles. </summary>
		public static bool[] IsJoustingLanceProjectile = ProjectileID.Sets.Factory.CreateNamedSet(IsJoustingLanceProjectileKey)
			.Description("Denotes which projectiles are jousting lances.")
			.RegisterBoolSet(false,
				ProjectileID.JoustingLance,
				ProjectileID.HallowJoustingLance,
				ProjectileID.ShadowJoustingLance
			);
#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning restore IDE0079 // Remove unnecessary suppression
	}
}