/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RijamsMod
{
	public partial class RijamsModNPCs : GlobalNPC
	{
		public void DoModifies(NPC npc, Player player, Projectile projectile, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			RijamsModPlayer moddedplayer = player.GetModPlayer<RijamsModPlayer>();
			//damage = (int)(damage * allDamage);

			Projectile held = null;
			if (projectile != null)
			{
				if (player != null && player.heldProj >= 0)
					held = Main.projectile[player.heldProj];

				if (projectile.dart)
					damage = (int)(damage * player.GetModPlayer<RijamsModPlayer>().dartDamage);
			}
		}
	}
}*/