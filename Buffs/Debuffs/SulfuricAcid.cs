using System;
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
			// It works but it is a little janky if the AI sets the npc.damage to something else.
			/*if (!npc.boss || NPCID.Sets.ShouldBeCountedAsBoss[npc.type])
			{
				Main.NewText("npc.damage = " + npc.damage + " npc.defDamage = " + npc.defDamage);
				if (npc.buffTime[buffIndex] > 2)
				{
					npc.damage = (int)Math.Ceiling(npc.defDamage * 0.9f);
				}
				if (npc.buffTime[buffIndex] <= 2)
				{
					npc.damage = npc.defDamage;
				}
			}*/
		}
	}
}
