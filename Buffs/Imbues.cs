using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
	public class ImbueSulfuricAcid : ModBuff
	{
		public override void SetDefaults()
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
}
