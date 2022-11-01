using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace RijamsMod
{
	/// <summary>
	/// This config operates on a server basis. 
	/// These parameters are synced from the server.
	/// </summary>
	[Label("Server Options")]
	public class RijamsModConfigServer : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("[c/00FF00:Server Options]")]
		
		[Label("[i:848]   Vanilla Vanity to Armor Changes")]
		[Tooltip("This option toggles if certain vanilla vanity sets are changed into armor sets.\n" +
			"  When On: The follow vanity sets will be changed to armor.\n" +
			"  When Off: The follow vanity sets WILL NOT be changed to armor.\n" +
			"    Turn Off to keep them as vanity or for cross mod compatibility.\n" +
			"* Pharaoh's Set\n" +
			"* Ancient Set\n" +
			"* Buffs to Stardust set bonus\n" +
			"Default value: On\n" +
			"Reload required")]
		[DefaultValue(true)]
		[ReloadRequired]
		public bool VanillaVanityToArmor { get; set; }

		[Label("[i:3121]   Town NPCs Cross Mod Support")]
		[Tooltip("This option toggles if the Town NPCs will sell items from other mods,\n" +
			"if they will have cross mod dialog, and cross mod happiness.\n" +
			"  When On: The Town NPCs will sell cross mod items.\n" +
			"  When Off: The Town NPCs WILL NOT sell cross mod items.\n" +
			"    Turn Off to remove cross mod items from the shops and dialog.\n" +
			"Default value: On\n" +
			"Reload not required for dialog and shop items.\n" +
			"Reload required for happiness.")]
		[DefaultValue(true)]
		public bool TownNPCsCrossModSupport { get; set; }

		[Label("[i:RijamsMod/BurglarsRing]   Burglar's Ring Notification Sound")]
		[Tooltip("This option controls the volume of the sound the " +
			"Burglar's Ring will make upon successfully working\n" +
			"   Set to 0 if you don't want to hear the sound\n" +
			"Default value: 100\n" +
			"Reload not required.")]
		[Range(0, 200)]
		[DefaultValue(100)]
		[Slider]
		public int BurglarsRingSound { get; set; }

		[Label("[i:784]   Load Debug Items")]
		[Tooltip("This option toggles if the debug items will be loaded.\n" +
			"  When On: The debug items WILL be loaded.\n" +
			"  When Off: The debug items WILL NOT be loaded.\n" +
			"    Turn On to load the debug items.\n" +
			"Default value: Off\n" +
			"Requires a Reload.")]
		[ReloadRequired]
		[DefaultValue(false)]
		public bool LoadDebugItems { get; set; }

		[Label("[i:1991]   Catch Town NPCs")]
		[Tooltip("This option toggles if the Town NPCs added by this mod can be\n" +
			"caught with a Bug Net (Fargo's Mutant Mod style)\n" +
			"  When On: The Town NPCs CAN be caught.\n" +
			"  When Off: The Town NPCs CAN NOT be caught.\n" +
			"    Turn On to catch the Town NPCs.\n" +
			"Default value: Off\n" +
			"Requires a Reload.")]
		[ReloadRequired]
		[DefaultValue(false)]
		public bool CatchNPCs { get; set; }

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
	[Label("Client Options")]
	public class RijamsModConfigClient : ModConfig
	{
		public static RijamsModConfigClient Instance;
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("[c/00FF00:Client Option]")]
		[Label("[i:RijamsMod/IntTrav_Helmet]   Ornithophobia")]
		[Tooltip("This option toggles if the Interstellar Traveler will wear a helmet or not\n" +
			"  When On: the Interstellar Traveler will wear a helmet.\n" +
			"  When Off: the Interstellar Traveler will not wear a helmet.\n" +
			"   Turn On if you don't want to see her head\n" +
			"Default value: Off\n" +
			"Requires Reload.")]
		[ReloadRequired]
		[DefaultValue(false)]
		public bool Ornithophobia { get; set; }
	}
}