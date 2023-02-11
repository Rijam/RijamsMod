using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Debuffs
{
	// Ethereal Flames is an example of a buff that causes constant loss of life.
	// See ExamplePlayer.UpdateBadLifeRegen and ExampleGlobalNPC.UpdateLifeRegen for more information.
	public class BleedingOut : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bleeding Out");
			// Description.SetDefault("You are bleeding out!\n(-10 HP/Sec)\n(-10% movement speed)\n(-10% attack speed)");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RijamsModPlayer>().bleedingOut = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<RijamsModNPCs>().bleedingOut = true;
		}
	}
}
