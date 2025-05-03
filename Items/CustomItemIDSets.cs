using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items
{
	[ReinitializeDuringResizeArrays]
	public class CustomItemIDSets
	{
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public const string IsWhipKey = "IsWhip";
		/// <summary> This set is a set of all whips. </summary>
		public static bool[] IsWhip = ItemID.Sets.Factory.CreateNamedSet(IsWhipKey)

			.Description("Denotes items that are whip weapons.")
			.RegisterBoolSet(false, ItemID.BlandWhip, ItemID.ThornWhip, ItemID.BoneWhip, ItemID.FireWhip,
			ItemID.CoolWhip, ItemID.SwordWhip, ItemID.MaceWhip, ItemID.ScytheWhip, ItemID.RainbowWhip);

		public const string IsJoustingLanceKey = "IsJoustingLance";
		/*
		public static Dictionary<int, int>[] IsJoustingLance = ItemID.Sets.Factory.CreateNamedSet(IsJoustingLanceKey)
			.Description("")
			.RegisterCustomSet<Dictionary<int, int>>(null,
				ItemID.JoustingLance, ProjectileID.JoustingLance,
				ItemID.HallowJoustingLance, ProjectileID.HallowJoustingLance,
				ItemID.ShadowJoustingLance, ProjectileID.ShadowJoustingLance
			);
		*/

		/*
		public static int[] IsJoustingLance = ItemID.Sets.Factory.CreateNamedSet(IsJoustingLanceKey)
			.Description("Denotes which items are jousting lances and their associated projectiles.")
			.RegisterIntSet(-1,
				ItemID.JoustingLance, ProjectileID.JoustingLance,
				ItemID.HallowJoustingLance, ProjectileID.HallowJoustingLance,
				ItemID.ShadowJoustingLance, ProjectileID.ShadowJoustingLance
			);
		*/
		/// <summary> This set is a set of all jousting lances items. </summary>
		public static bool[] IsJoustingLance = ItemID.Sets.Factory.CreateNamedSet(IsJoustingLanceKey)
			.Description("Denotes which items are jousting lances.")
			.RegisterBoolSet(false,
				ItemID.JoustingLance,
				ItemID.HallowJoustingLance,
				ItemID.ShadowJoustingLance
			);

		/// <summary>
		/// This set is a set of all lantern weapons like the Nightglow.
		/// Items in this set will automatically be drawn behind the player's back hand.
		/// </summary>
		public const string IsLanternWeaponKey = "IsLanternWeapon";
		public static bool[] IsLanternWeapon = ItemID.Sets.Factory.CreateNamedSet(IsLanternWeaponKey)
			.Description("This set is a set of all lantern weapons like the Nightglow. Items in this set will automatically be drawn behind the player's back hand.")
			.RegisterBoolSet(false, ItemID.FairyQueenMagicItem);

		public const string IsCombatFlareGunKey = "IsCombatFlareGun";
		public static bool[] IsCombatFlareGun = ItemID.Sets.Factory.CreateNamedSet(IsCombatFlareGunKey)
			.Description("The set for the Combat Flare Pistol and Triple Barrel Flare Pistol")
			.RegisterBoolSet(false);
#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning restore IDE0079 // Remove unnecessary suppression
	}
}