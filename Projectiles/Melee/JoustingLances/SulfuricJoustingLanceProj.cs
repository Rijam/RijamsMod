using Microsoft.Xna.Framework;
using RijamsMod.Buffs.Debuffs;
using RijamsMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	public class SulfuricJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = ModContent.DustType<SulfurDust>();
			dustTypeRare = DustID.YellowTorch;
			offset = 2;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 90f;
			widthMultiplier = 22f;
			lanceHitboxBounds = new(0, 0, 250, 250);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.ownerHitCheck = false; // No line of sight check, which means the projectile can hit through walls.
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<SulfuricAcid>(), (int)(damageDone * 1.5f)); // The buff lasts longer the more damage we do.
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<SulfuricAcid>(), (int)(info.Damage * 1.5f));
		}
	}
}