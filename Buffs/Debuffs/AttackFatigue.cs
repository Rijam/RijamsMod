using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Debuffs
{
	public class AttackFatigue1 : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Attack Fatigue I");
			// Description.SetDefault("Your weapons are heavy!\n(-20% attack speed)");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetAttackSpeed(DamageClass.Generic) -= 0.2f;
		}
	}
	public class AttackFatigue2 : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Attack Fatigue II");
			// Description.SetDefault("Your weapons are heavy!\n(-50% attack speed)");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetAttackSpeed(DamageClass.Generic) -= 0.5f;
		}
	}
}
