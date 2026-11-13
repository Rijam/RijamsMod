using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.NPCs
{
	[ReinitializeDuringResizeArrays]
	public class CustomNPCIDSets
	{
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public const string IsFireBasedAttackForFrostyRoseKey = "IsFireBasedAttackForFrostyRose";
		/// <summary> Denotes this NPC is based on fire so that the Frosty Rose can apply damage reduction. It is only for "hot" fire, so not frostburn related things. </summary>
		public static bool[] IsFireBasedAttackForFrostyRose = NPCID.Sets.Factory.CreateNamedSet(IsFireBasedAttackForFrostyRoseKey)
			.Description("Denotes this NPC is based on fire so that the Frosty Rose can apply damage reduction. It is only for \"hot\" fire, so not frostburn related things.")
			.RegisterBoolSet(false,
				NPCID.MeteorHead,
				NPCID.FireImp,
				NPCID.BurningSphere,
				NPCID.LavaSlime,
				NPCID.BlazingWheel,
				NPCID.Lavabat,
				NPCID.HellArmoredBones,
				NPCID.HellArmoredBonesSpikeShield,
				NPCID.HellArmoredBonesMace,
				NPCID.HellArmoredBonesSword,
				NPCID.DiabolistRed,
				NPCID.DiabolistWhite,
				NPCID.MourningWood,
				NPCID.Splinterling,
				NPCID.SolarCrawltipedeHead,
				NPCID.SolarCrawltipedeBody,
				NPCID.SolarCrawltipedeTail,
				NPCID.SolarDrakomire,
				NPCID.SolarDrakomireRider,
				NPCID.SolarSroller,
				NPCID.SolarCorite,
				NPCID.SolarSolenian,
				NPCID.SolarFlare,
				NPCID.LunarTowerSolar,
				NPCID.SolarSpearman,
				NPCID.SolarGoop,
				NPCID.TorchZombie,
				NPCID.ArmedTorchZombie,
				NPCID.TorchGod
			);
#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning restore IDE0079 // Remove unnecessary suppression
	}
}