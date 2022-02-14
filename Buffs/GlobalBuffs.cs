using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
    public class GlobalBuffs : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            if (player.HasBuff(ModContent.BuffType<ExceptionalFeast>()) && player.HasBuff(BuffID.WellFed))
            {
                player.ClearBuff(BuffID.WellFed);
                buffIndex--;
            }
            if (player.HasBuff(ModContent.BuffType<ExceptionalFeast>()) && player.HasBuff(ModContent.BuffType<Satiated>()))
            {
                player.ClearBuff(ModContent.BuffType<Satiated>());
                buffIndex--;
            }
            if (player.HasBuff(ModContent.BuffType<ExceptionalFeast>()) && player.HasBuff(BuffID.WellFed) && player.HasBuff(ModContent.BuffType<Satiated>()))
            {
                player.ClearBuff(ModContent.BuffType<Satiated>());
                player.ClearBuff(BuffID.WellFed);
                buffIndex -= 2;
            }
            if (player.HasBuff(ModContent.BuffType<Satiated>()) && player.HasBuff(BuffID.WellFed))
            {
                player.ClearBuff(ModContent.BuffType<Satiated>());
                buffIndex--;
            }
            base.Update(type, player, ref buffIndex);
        }
    }
}