using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
	public class ImbueSulfuricAcid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Weapon Imbue: Sulfuric Acid");
			Description.SetDefault("Melee attacks inflict Sulfuric Acid");
			Main.meleeBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RijamsModPlayer>().flaskBuff = 1;
		}
	}
	public class ImbueOiled : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Weapon Imbue: Oiled");
			Description.SetDefault("Melee attacks inflict Oiled");
			Main.meleeBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RijamsModPlayer>().flaskBuff = 2;
		}
	}
}
