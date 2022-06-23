using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
	public class ShadowflamePhantomBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Phantom");
			Description.SetDefault("The Shadowflame Phantom will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		
		/*public override bool Autoload(ref string name, ref string texture)
		{
			texture = "RijamsMod/Buffs/ShadowflamePhantomBuff";
			return true;
		}*/

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("ShadowflamePhantom").Type] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}