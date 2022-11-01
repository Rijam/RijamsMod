using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
	public class WarriorEnergy : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Warrior Energy");
			Description.SetDefault("(+10% damage)\n(+5% attack speed)");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
			player.GetDamage(DamageClass.Generic) += 0.05f;
		}
	}
}