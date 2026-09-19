using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;

namespace RijamsMod
{
	/// <summary>
	/// This config operates on a server basis. 
	/// These parameters are synced from the server.
	/// </summary>
	public class RijamsModConfigServer : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("ServerOptions")]

		// [ReloadRequired] Don't do this on class elements! This require the config is reloaded every time the config is opened.
		public VanillaArmorList VanillaArmorChanges { get; set; }

		/// <summary>
		/// A class to house the armor changes so that it shows up as a "sub-config" in the config menu.
		/// </summary>
		[BackgroundColor(51, 72, 149)] // Default UI color: (73, 94, 171)
		public class VanillaArmorList
		{
			[ReloadRequired] // This will add the tooltip saying a reload is required, but it doesn't actually trigger a reload because the default logic doesn't check for nested classes.
			[DefaultValue(true)] // Doesn't actually set the default value when the mod config is opened. The default is set in constructor for the mod config class.
			public bool PharaohsSet { get; set; }

			[ReloadRequired]
			[DefaultValue(true)]
			public bool AncientSet { get; set; }

			[ReloadRequired]
			[DefaultValue(true)]
			public bool BuffStardustSetBonus { get; set; }

			[ReloadRequired]
			[DefaultValue(true)]
			public bool BuffBeeGreaves { get; set; }

			[ReloadRequired]
			[DefaultValue(true)]
			public bool BuffFlinxFurCoat { get; set; }

			// Need for the custom NeedsReload logic
			public override bool Equals(object obj)
			{
				if (obj is VanillaArmorList other)
					return PharaohsSet == other.PharaohsSet 
						&& AncientSet == other.AncientSet
						&& BuffStardustSetBonus == other.BuffStardustSetBonus
						&& BuffBeeGreaves == other.BuffBeeGreaves
						&& BuffFlinxFurCoat == other.BuffFlinxFurCoat;
				return base.Equals(obj);
			}

			public override int GetHashCode()
			{
				return new { PharaohsSet, AncientSet, BuffStardustSetBonus, BuffBeeGreaves, BuffFlinxFurCoat }.GetHashCode();
			}
			
			/* Shows up in the mod config
			public override string ToString()
			{
				return $"(PharaohsSet: {PharaohsSet}), (AncientSet: {AncientSet}), (BuffStardustSetBonus: {BuffStardustSetBonus}), (BuffBeeGreaves: {BuffBeeGreaves}), (BuffFlinxFurCoat: {BuffFlinxFurCoat})";
			}
			*/
		}

		[DefaultValue(true)]
		public bool JoustingLanceStaticInvincibility { get; set; }

		[DefaultValue(true)]
		public bool YoyoStaticInvincibility { get; set; }

		[DefaultValue(true)]
		public bool TownNPCsCrossModSupport { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		public bool LoadDebugItems { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		public bool CatchNPCs { get; set; }

		[DefaultValue(SnowBallaGriefingOptions.DropAsItem)]
		[DrawTicks]
		public SnowBallaGriefingOptions SnowBallaGriefing { get; set; }

		public enum SnowBallaGriefingOptions
		{
			On,
			DropAsItem,
			Off
		}

		public RijamsModConfigServer()
		{
			VanillaArmorChanges = new()
			{
				PharaohsSet = true,
				AncientSet = true,
				BuffStardustSetBonus = true,
				BuffBeeGreaves = true,
				BuffFlinxFurCoat = true
			};
		}

		public override bool NeedsReload(ModConfig pendingConfig)
		{
			// The default logic doesn't look for nested classes, so check for that here.
			foreach (PropertyFieldWrapper variable in ConfigManager.GetFieldsAndProperties(this))
			{
				if (variable.Name == "VanillaArmorChanges")
				{
					// if (variable.GetValue(this) is VanillaArmorList thisConfig)
					// {
					// 	Main.NewText($"variable.GetValue(this) {thisConfig.PharaohsSet}");
					// }
					// if (variable.GetValue(pendingConfig) is VanillaArmorList pending)
					// {
					// 	Main.NewText($"variable.GetValue(this) {pending.PharaohsSet}");
					// }
					if (!ConfigManager.ObjectEquals(variable.GetValue(this), variable.GetValue(pendingConfig)))
					{
						return true;
					}
				}
			}
			// Base covers the ReloadRequired attributes.
			return base.NeedsReload(pendingConfig);
		}

		/* Not written by Rijam */
		public static bool IsPlayerLocalServerOwner(int whoAmI)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
			}

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				RemoteClient client = Netplay.Clients[i];
				if (client.State == 10 && i == whoAmI && client.Socket.GetRemoteAddress().IsLocalHost())
				{
					return true;
				}
			}
			return false;
		}

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				return true;
			}

			if (!IsPlayerLocalServerOwner(whoAmI))
			{
				message = NetworkText.FromLiteral("You are not the server owner so you can not change this config!");
				// message = "You are not the server owner so you can not change this config!";
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
		/* */
	}
	/// <summary>
	/// This config operates on a per-client basis. 
	/// These parameters are local to this computer and are NOT synced from the server.
	/// </summary>
	public class RijamsModConfigClient : ModConfig
	{
		public static RijamsModConfigClient Instance;
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("ClientOption")]

		[ReloadRequired]
		[DefaultValue(false)]
		public bool Ornithophobia { get; set; }

		[Range(0, 200)]
		[DefaultValue(100)]
		[Slider]
		public int BurglarsRingSound { get; set; }

		[DefaultValue(WhipMultihitPenalty.On)]
		[DrawTicks]
		public WhipMultihitPenalty DisplayWhipMultihitPenalty { get; set; }

		[DefaultValue(SupportSummonsAura.Normal)]
		[DrawTicks]
		public SupportSummonsAura DisplayDefenseSupportSummonsAura { get; set; }

		[DefaultValue(SupportSummonsAura.Normal)]
		[DrawTicks]
		public SupportSummonsAura DisplayHealingSupportSummonsAura { get; set; }

		[DefaultValue(true)]
		public bool SnowDuringFrostLegion { get; set; }

		public enum WhipMultihitPenalty
		{
			On,
			HoldShift,
			Off
		}
		public enum SupportSummonsAura
		{
			Opaque,
			Normal,
			Faded,
			Off
		}
	}
}