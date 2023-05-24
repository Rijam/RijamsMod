using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Minions
{
	public class BabyBloodEelBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Minions.BabyBloodEel>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}