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
    /// This config operates on a per-client basis. 
    /// These parameters are local to this computer and are NOT synced from the server.
    /// </summary>
    [Label("Rijam's Mod Client Options")]
    public class RijamsModConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("[c/00FF00:Client Option]")]
        [Label("$Mods.RijamsMod.Config.Ornithophobia")]
        [Tooltip("This option toggles if the Interstellar Traveler will wear a helmet or not\n" +
			"  When Enabled: the Interstellar Traveler will wear a helmet.\n" +
			"  When Disabled: the Interstellar Traveler will not wear a helmet.\n" +
			"   Enable if you don't want to see her head\n" +
			"Default value: Disabled\n" +
			"Requires Reload")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool Ornithophobia { get; set; }

		[Label("$Mods.RijamsMod.Config.BurglarsRingSound")]
		[Tooltip("This option toggles if the Burglar's Ring will make a sound upon successfully working\n" +
			"  When Enabled: the Burglar's Ring will make a sound.\n" +
			"  When Disabled: the Burglar's Ring will not make a sound.\n" +
			"   Disable if you don't want to hear the sound\n" +
			"Default value: Enabled\n" +
			"Reload not required")]
		[DefaultValue(true)]
		public bool BurglarsRingSound { get; set; }
	}
	[Label("Rijam's Mod Server Options")]
	public class RijamsModConfigServer : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("[c/00FF00:Server Options]")]
		[Label("[i:3122]   Sell Fisherman NPC Modded Items")]
		[Tooltip("This option toggles if the Fisherman will sell the custom items added by this mod.\n" +
			"  When Enabled: The Fisherman WILL sell the custom items added by this mod.\n" +
			"  When Disabled: The Fisherman WILL NOT sell the custom items added by this mod.\n" +
			"    Disable to remove the modded items.\n" +
			"Default value: Enabled\n" +
			"Requires a Reload.")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool LoadModdedItems { get; set; }

		[Label("[i:2002]   Sell Bait")]
		[Tooltip("This option toggles if the Fisherman will sell bait.\n" +
			"  When Enabled: The Fisherman WILL sell bait.\n" +
			"  When Disabled: The Fisherman WILL NOT sell bait.\n" +
			"    Disable to remove bait from the shop.\n" +
			"Default value: Enabled\n" +
			"Requires a Reload.")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool SellBait { get; set; }

		[Label("[i:2290]   Sell Fish")]
		[Tooltip("This option toggles if the Fisherman will sell fish.\n" +
			"  When Enabled: The Fisherman WILL sell fish.\n" +
			"  When Disabled: The Fisherman WILL NOT sell fish.\n" +
			"    Disable to remove fish from the shop.\n" +
			"Default value: Enabled\n" +
			"Requires a Reload.")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool SellFish { get; set; }

		[Label("[i:2291]   Sell Fishing Rods")]
		[Tooltip("This option toggles if the Fisherman will sell fishing rods.\n" +
			"  When Enabled: The Fisherman WILL sell fishing rods.\n" +
			"  When Disabled: The Fisherman WILL NOT sell fishing rods.\n" +
			"    Disable to remove fishing rods from the shop.\n" +
			"Default value: Enabled\n" +
			"Requires a Reload.")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool SellFishingRods { get; set; }

		[Label("[i:3120]   Sell Extra Items")]
		[Tooltip("This option toggles if the Fisherman will sell extra items such as:\n" +
			"fishing accessories, Fish Finder parts, and vanity items.\n" +
			"  When Enabled: The Fisherman will sell extra items.\n" +
			"  When Disabled: The Fisherman WILL NOT sell extra items.\n" +
			"    Disable to remove extra items from the shop.\n" +
			"Default value: Enabled\n" +
			"Requires a Reload.")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool SellExtraItems { get; set; }

		[Label("[i:87]   Shop Price Scaling")]
		[Tooltip("This option sets the scaling for the prices in the Fisherman's shop.\n" +
			"  50 means half the normal price\n" +
			"  200 means double the normal price.\n" +
			"    Change this value if you want the shop to be cheaper or more expensive.\n" +
			"Default value: 100\n" +
			"Requires a Reload.")]
		[Increment(1)]
		[Range(50, 200)]
		[DefaultValue(100)]
		[Slider]
		[ReloadRequired]
		public int ShopPriceScaling { get; set; }

		[Label("[i:784]   Load Debug Items")]
		[Tooltip("This option toggles if the debug items will be loaded.\n" +
			"  When Enabled: The debug items WILL be loaded.\n" +
			"  When Disabled: The debug items WILL NOT be loaded.\n" +
			"    Enable to load the debug items.\n" +
			"Default value: Disabled\n" +
			"Requires a Reload.")]
		[ReloadRequired]
		[DefaultValue(false)]
		public bool LoadDebugItems { get; set; }
	}
}