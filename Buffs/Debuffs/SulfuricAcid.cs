using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Debuffs
{
	// Ethereal Flames is an example of a buff that causes constant loss of life.
	// See ExamplePlayer.UpdateBadLifeRegen and ExampleGlobalNPC.UpdateLifeRegen for more information.
	public class SulfuricAcid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sulfuric Acid");
			// Description.SetDefault("Your skin is burning!\n(-8 HP/Sec)\n(-10% damage)");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RijamsModPlayer>().sulfuricAcid = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<RijamsModNPCs>().sulfuricAcid = true;
		}
	}
}
