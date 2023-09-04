using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Mounts
{
	public class DistortionSphereBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<Items.Mounts.DistortionSphere>(), player);
			player.buffTime[buffIndex] = 10; // reset buff time
		}
	}
	public class SwiftSphereBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<Items.Mounts.SwiftSphere>(), player);
			player.buffTime[buffIndex] = 10; // reset buff time
		}
	}
}