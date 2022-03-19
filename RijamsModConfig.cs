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
		[Label("[i:3122]   Sell Fisherman NPC Modded Items")]
		[Tooltip("This option toggles if the Fisherman will sell the custom items added by this mod.\n" +
			"  When On: The Fisherman WILL sell the custom items added by this mod.\n" +
			"  When Off: The Fisherman WILL NOT sell the custom items added by this mod.\n" +
			"    Turn Off to remove the modded items.\n" +
			"Default value: On\n" +
			"Reload not required.")]
		[DefaultValue(true)]
		public bool SellModdedItems { get; set; }

		[Label("[i:2002]   Sell Bait")]
		[Tooltip("This option toggles if the Fisherman will sell bait.\n" +
			"  When On: The Fisherman WILL sell bait.\n" +
			"  When Off: The Fisherman WILL NOT sell bait.\n" +
			"    Turn Off to remove bait from the shop.\n" +
			"Default value: On\n" +
			"Reload not required.")]
		[DefaultValue(true)]
		public bool SellBait { get; set; }

		[Label("[i:2290]   Sell Fish")]
		[Tooltip("This option toggles if the Fisherman will sell fish.\n" +
			"  When On: The Fisherman WILL sell fish.\n" +
			"  When Off: The Fisherman WILL NOT sell fish.\n" +
			"    Turn Off to remove fish from the shop.\n" +
			"Default value: On\n" +
			"Reload not required.")]
		[DefaultValue(true)]
		public bool SellFish { get; set; }

		[Label("[i:2291]   Sell Fishing Rods")]
		[Tooltip("This option toggles if the Fisherman will sell fishing rods.\n" +
			"  When On: The Fisherman WILL sell fishing rods.\n" +
			"  When Off: The Fisherman WILL NOT sell fishing rods.\n" +
			"    Turn Off to remove fishing rods from the shop.\n" +
			"Default value: On\n" +
			"Reload not required.")]
		[DefaultValue(true)]
		public bool SellFishingRods { get; set; }

		[Label("[i:3120]   Sell Extra Items")]
		[Tooltip("This option toggles if the Fisherman will sell extra items such as:\n" +
			"fishing accessories, Fish Finder parts, and vanity items.\n" +
			"  When On: The Fisherman will sell extra items.\n" +
			"  When Off: The Fisherman WILL NOT sell extra items.\n" +
			"    Turn Off to remove extra items from the shop.\n" +
			"Default value: On\n" +
			"Reload not required.")]
		[DefaultValue(true)]
		public bool SellExtraItems { get; set; }

		[Label("[i:87]   Shop Price Scaling")]
		[Tooltip("This option sets the scaling for the prices in the Fisherman's shop.\n" +
			"  50 means half the normal price\n" +
			"  200 means double the normal price.\n" +
			"    Change this value if you want the shop to be cheaper or more expensive.\n" +
			"Default value: 100\n" +
			"Reload not required.")]
		[Increment(1)]
		[Range(50, 200)]
		[DefaultValue(100)]
		[Slider]
		public int ShopPriceScaling { get; set; }

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
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("[c/00FF00:Client Option]")]
		[Label("$Mods.RijamsMod.Config.Ornithophobia")]
		[Tooltip("This option toggles if the Interstellar Traveler will wear a helmet or not\n" +
			"  When On: the Interstellar Traveler will wear a helmet.\n" +
			"  When Off: the Interstellar Traveler will not wear a helmet.\n" +
			"   Turn On if you don't want to see her head\n" +
			"Default value: Off\n" +
			"Requires Reload.")]
		[ReloadRequired]
		[DefaultValue(false)]
		public bool Ornithophobia { get; set; }

		[Label("$Mods.RijamsMod.Config.BurglarsRingSound")]
		[Tooltip("This option toggles if the Burglar's Ring will make a sound upon successfully working\n" +
			"  When On: the Burglar's Ring will make a sound.\n" +
			"  When Off: the Burglar's Ring will not make a sound.\n" +
			"   Turn Off if you don't want to hear the sound\n" +
			"Default value: On\n" +
			"Reload not required.")]
		[DefaultValue(true)]
		public bool BurglarsRingSound { get; set; }

		/*[Label("[i:1516]   Vanilla Harpy Enemy Resprite")]
		[Tooltip("This option toggles if the vanilla Harpy enemy will use an updated sprite\n" +
			"  When On: Harpies will be changed.\n" +
			"  When Off: Harpies will not be changed.\n" +
			"   Turn Off if you don't the new sprite or have a resource pack.\n" +
			"Default value: On\n" +
			"Requires Reload.")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool HarpySpriteChange { get; set; }*/
	}
}