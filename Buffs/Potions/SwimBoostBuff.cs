using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Potions
{
	public class SwimBoostBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Swim Boost");
			// Description.SetDefault("Move freely in liquids");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.ignoreWater = true;
		}
	}
}
