using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Potions
{
	public struct FlaskIDs
	{
		public const int None = 0;
		public const int SulfuricAcid = 1;
		public const int Oiled = 2;
	}
	public class ImbueSulfuricAcid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Weapon Imbue: Sulfuric Acid");
			// Description.SetDefault("Melee attacks inflict Sulfuric Acid");
			BuffID.Sets.IsAFlaskBuff[Type] = true;
			Main.meleeBuff[Type] = true;
			Main.persistentBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RijamsModPlayer>().flaskBuff = FlaskIDs.SulfuricAcid;
			// We set this to a value not used by existing imbues simply to indicate to other mods that a weapon imbue is active. The real flag is exampleWeaponImbue above.
			player.meleeEnchant = 255;
		}
	}
	public class ImbueOiled : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Weapon Imbue: Oiled");
			// Description.SetDefault("Melee attacks inflict Oiled");
			BuffID.Sets.IsAFlaskBuff[Type] = true;
			Main.meleeBuff[Type] = true;
			Main.persistentBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RijamsModPlayer>().flaskBuff = FlaskIDs.Oiled;
			// We set this to a value not used by existing imbues simply to indicate to other mods that a weapon imbue is active. The real flag is exampleWeaponImbue above.
			player.meleeEnchant = 255;
		}
	}
}
