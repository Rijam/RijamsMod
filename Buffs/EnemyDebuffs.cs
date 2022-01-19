using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
	/*public class EnemySlow : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			// NPC only buff so we'll just assign it a useless buff icon.
			texture = "Terraria/Buff_" + BuffID.Slow;
			return base.Autoload(ref name, ref texture);
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Enemy Slow");
			Description.SetDefault("Reduces horizontal movement speed by 25%");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<RijamsModNPCs>().enemySlow = true;
		}
	}*/
}