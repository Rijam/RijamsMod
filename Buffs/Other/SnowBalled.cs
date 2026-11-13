using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Other
{
	public class SnowBalled : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RijamsModPlayer>().Stat_CriticalHitAdditionalDamage += 0.1f;
			player.GetModPlayer<RijamsModPlayer>().Stat_BonusWhipTagCritChance += 10;
		}
	}
}
