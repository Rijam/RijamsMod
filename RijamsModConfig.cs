using System;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

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
		
		[DefaultValue(ArmorOptions.All)]
		[DrawTicks]
		[ReloadRequired]
		public ArmorOptions VanillaVanityToArmor { get; set; }

		[DefaultValue(true)]
		public bool TownNPCsCrossModSupport { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		public bool LoadDebugItems { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		public bool CatchNPCs { get; set; }

		public enum ArmorOptions
		{
			All,
			VanityOnly,
			ArmorOnly,
			Off
		}

		/* Not written by Rijam*/
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

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				return true;
			}

			if (!IsPlayerLocalServerOwner(whoAmI))
			{
				message = "You are not the server owner so you can not change this config!";
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