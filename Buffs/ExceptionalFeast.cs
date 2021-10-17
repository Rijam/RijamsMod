using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
    public class ExceptionalFeast : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Exceptional Feast");
            if (Main.expertMode)
            {
                Description.SetDefault("Extreme improvements to all stats and increased life regeneration");
            }
            else
            {
                Description.SetDefault("Extreme improvements to all stats");
            }
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.wellFed = true;
            player.statDefense += 5;
            player.meleeCrit += 5;
            player.allDamage += 0.125f;
            player.meleeSpeed += 0.125f;
            player.magicCrit += 5;
            player.rangedCrit += 5;
            player.thrownCrit += 5;
            player.minionKB += 1.25f;
            player.moveSpeed += 0.5f;
            player.pickSpeed += 0.2f;
            /*if (Main.expertMode) //player.wellFed = true; already does this
            {
                player.lifeRegen += 2;
            }*/
        }
    }
}
