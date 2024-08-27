using Microsoft.Xna.Framework;
using RijamsMod.Buffs.Debuffs;
using RijamsMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	public class DarkJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.Corruption;
			dustTypeRare = DustID.Shadowflame;
			offset = 3;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 84f;
			widthMultiplier = 22f;
			lanceHitboxBounds = new(0, 0, 250, 250);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.ShadowFlame, (int)(damageDone * 1.5f)); // The buff lasts longer the more damage we do.
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.ShadowFlame, (int)(info.Damage * 1.5f));
		}
	}
}