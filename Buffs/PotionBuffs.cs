using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
	public class Fury : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fury");
			Description.SetDefault("+10% attack speed");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
		}
	}
	public class Support : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Support");
			Description.SetDefault("+1 sentry capacity");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxTurrets++;
		}
	}

	public class Soaring : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soaring");
			Description.SetDefault("+0.5 seconds wing flight time");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RijamsModPlayer>().soaringPotion = true;
		}
	}
}