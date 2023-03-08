using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Potions
{
	public static class FlaskIDs
	{
		public const int SulfuricAcid = 1;
		public const int Oiled = 2;
	}
	public class ImbueSulfuricAcid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Weapon Imbue: Sulfuric Acid");
			// Description.SetDefault("Melee attacks inflict Sulfuric Acid");
			Main.meleeBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RijamsModPlayer>().flaskBuff = FlaskIDs.SulfuricAcid;
		}
	}
	public class ImbueOiled : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Weapon Imbue: Oiled");
			// Description.SetDefault("Melee attacks inflict Oiled");
			Main.meleeBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RijamsModPlayer>().flaskBuff = FlaskIDs.Oiled;
		}
	}
}
