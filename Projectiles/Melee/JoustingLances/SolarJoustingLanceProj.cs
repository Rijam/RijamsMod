using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Buffs.Debuffs;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	// I made Example Jousting Lance so I'm going to use it!
	public class SolarJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.SolarFlare;
			dustTypeRare = DustID.OrangeTorch;
			offset = 4;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 160f;
			widthMultiplier = 25f;
			lanceHitboxBounds = new(0, 0, 400, 400);
		}

		public override void ModifyDrawing(ref Color drawColor)
		{
			drawColor = Color.White;
		}

		public override void AI()
		{
			base.AI();

			// Use one of the projectile's localAI slot as a cooldown timer for spawning explosions. When an explosion is spawned, this gets set to 20, so it takes 20 ticks to reach 0 again.
			if (Projectile.localAI[1] > 0f)
			{
				Projectile.localAI[1] -= 1f;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Daybreak, damageDone); // The buff lasts longer the more damage we do.

			if (Projectile.owner == Main.myPlayer && Projectile.damage > 1 && Projectile.localAI[1] <= 0f)
			{
				//foreach (NPC npc in Main.ActiveNPCs)
				//{
					//Solar Explosion
					Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_ItemUse(Main.player[Projectile.owner].HeldItem), target.Center.X, target.Center.Y, 0, 0, ProjectileID.SolarWhipSwordExplosion, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, 0f, 0.5f + Main.rand.NextFloat());
					Projectile.localAI[1] = 20f;
					//Projectile.localNPCImmunity[i] = 6;
					//Main.npc[i].immune[Projectile.owner] = 4;
				//}
			}
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<SulfuricAcid>(), info.Damage);

			if (Projectile.owner == Main.myPlayer && Projectile.damage > 1 && Projectile.localAI[1] <= 0f)
			{
				Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_ItemUse(Main.player[Projectile.owner].HeldItem), target.Center.X, target.Center.Y, 0, 0, ProjectileID.SolarWhipSwordExplosion, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, 0f, 0.5f + Main.rand.NextFloat());
				Projectile.localAI[1] = 20f;
			}
		}
	}
}