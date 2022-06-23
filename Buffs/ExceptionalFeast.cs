using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
    public class ExceptionalFeast : ModBuff
    {
        public override void SetStaticDefaults()
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
            BuffID.Sets.IsWellFed[Type] = true;
            BuffID.Sets.IsFedState[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.wellFed = true;
            player.statDefense += 5;
            player.GetCritChance(DamageClass.Melee) += 5;
            player.GetDamage(DamageClass.Generic) += 0.125f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.125f;
            player.GetCritChance(DamageClass.Magic) += 5;
            player.GetCritChance(DamageClass.Ranged) += 5;
            player.GetCritChance(DamageClass.Throwing) += 5;
            player.GetKnockback(DamageClass.Summon).Base += 1.25f;
            player.moveSpeed += 0.5f;
            player.pickSpeed -= 0.2f;
            /*if (Main.expertMode) //player.wellFed = true; already does this
            {
                player.lifeRegen += 2;
            }*/
        }
    }
    public class Satiated : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Satiated");
            Description.SetDefault("Minuscule improvements to all stats");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            BuffID.Sets.IsWellFed[Type] = true;
            BuffID.Sets.IsFedState[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.wellFed = true;
            player.statDefense += 1;
            player.GetCritChance(DamageClass.Melee) += 1;
            player.GetDamage(DamageClass.Generic) += 0.025f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.025f;
            player.GetCritChance(DamageClass.Magic) += 1;
            player.GetCritChance(DamageClass.Ranged) += 1;
            player.GetCritChance(DamageClass.Throwing) += 1;
            player.GetKnockback(DamageClass.Summon).Base += 0.25f;
            player.moveSpeed += 0.1f;
            /*if (Main.expertMode) //player.wellFed = true; already does this
            {
                player.lifeRegen += 2;
            }*/
        }
    }
}
