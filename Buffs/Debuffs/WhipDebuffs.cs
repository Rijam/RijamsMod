using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Items.Weapons.Summon.Whips;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace RijamsMod.Buffs.Debuffs
{
	public class VileWhipDebuff : ModBuff
	{
		public static readonly int TagDamage = 8;

		public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<WhipDebuffNPC>().markedByVileWhip = true;
		}
	}
	public class ViciousWhipDebuff : ModBuff
	{
		public static readonly int TagDamage = 8;

		public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<WhipDebuffNPC>().markedByViciousWhip = true;
		}
	}
	public class SulfuricWhipDebuff : ModBuff
	{
		public static readonly int TagDamage = 10;

		public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<WhipDebuffNPC>().markedBySulfuricWhip = true;
		}
	}
	public class ForbiddenWhipDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<WhipDebuffNPC>().markedByForbiddenWhip = true;
		}
	}
	public class FestiveWhipDebuff : ModBuff
	{
		public static readonly int TagDamage = 8;

		public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<WhipDebuffNPC>().markedByFestiveWhip = true;
		}
	}
	public class SupernovaWhipDebuff : ModBuff
	{
		public static readonly int TagDamage = 20;

		public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<WhipDebuffNPC>().markedBySupernovaWhip = true;
		}
	}

	public class WhipDebuffNPC : GlobalNPC
	{
		// This is required to store information on entities that isn't shared between them.
		public override bool InstancePerEntity => true;

		public bool markedByVileWhip;
		public bool markedByViciousWhip;
		public bool markedBySulfuricWhip;
		public bool markedByForbiddenWhip;
		public bool markedByFestiveWhip;
		public bool markedBySupernovaWhip;

		public override void ResetEffects(NPC npc)
		{
			markedByVileWhip = false;
			markedByViciousWhip = false;
			markedBySulfuricWhip = false;
			markedByForbiddenWhip = false;
			markedByFestiveWhip = false;
			markedBySupernovaWhip = false;
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			// Only player attacks should benefit from this buff, hence the NPC and trap checks.
			if (projectile.npcProj || projectile.trap || !projectile.IsMinionOrSentryRelated)
			{
				return;
			}

			// SummonTagDamageMultiplier scales down tag damage for some specific minion and sentry projectiles for balance purposes.
			float projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];

			if (markedByVileWhip)
			{
				// Apply a flat bonus to every hit
				modifiers.FlatBonusDamage += VileWhipDebuff.TagDamage * projTagMultiplier;
			}
			if (markedByViciousWhip)
			{
				modifiers.FlatBonusDamage += ViciousWhipDebuff.TagDamage * projTagMultiplier;
			}
			if (markedBySulfuricWhip)
			{
				modifiers.FlatBonusDamage += SulfuricWhipDebuff.TagDamage * projTagMultiplier;
				Dust.NewDust(npc.Center, npc.width, npc.height, ModContent.DustType<Dusts.SulfurDust>(), modifiers.HitDirection, modifiers.HitDirection, 150, default, 1f);
			}
			if (markedByForbiddenWhip)
			{
				// Player owner = Main.player[projectile.owner];
				// float ownerSummonKB = owner.GetTotalKnockback(DamageClass.Summon).Additive * owner.GetTotalKnockback(DamageClass.Summon).Multiplicative;
				// float ownerSummonKB2 = owner.GetTotalKnockback(DamageClass.Summon).Base; // Vanilla uses .Base instead of the others

				// Oddly, the projectile doesn't update it's knockback dynamically like it does with damage.

				float multiplier = 0.8f;
				if (projectile.knockBack != 0)
				{
					multiplier -= (projectile.knockBack / 50f); // multiplier becomes less with more knockback
				}
				npc.velocity *= multiplier;
				npc.netUpdate = true;
				// Main.NewText("ownerSummonKB " + ownerSummonKB + " ownerSummonKB2 " + ownerSummonKB2 + " knockback " + knockback + " multiplier " + multiplier);
				// Main.NewText("damage " + damage + " knockback " + knockback + " crit " + crit + " multiplier " + multiplier);
				Dust.NewDust(npc.Center, npc.width, npc.height, DustID.GemAmber, modifiers.HitDirection, modifiers.HitDirection, 150, default, 1f);
			}
			if (markedByFestiveWhip)
			{
				modifiers.FlatBonusDamage += FestiveWhipDebuff.TagDamage * projTagMultiplier;
				for (int i = 0; i < 5; i++)
				{
					int selectRand = Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.RedTorch);
					Dust.NewDust(npc.position, npc.width, npc.height, selectRand, modifiers.HitDirection, modifiers.HitDirection, 0, Color.White, 1f);
				}
			}
			if (markedBySupernovaWhip)
			{
				modifiers.FlatBonusDamage += SupernovaWhipDebuff.TagDamage * projTagMultiplier;
				if (Main.rand.NextBool(10))
				{
					modifiers.SetCrit();
				}
				for (int i = 0; i < 10; i++)
				{
					int selectRand = Utils.SelectRandom(Main.rand, DustID.YellowTorch, DustID.BlueTorch);
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, selectRand, modifiers.HitDirection, modifiers.HitDirection, 0, Color.White, 2.0f);
					Main.dust[dust].noGravity = true;
				}
			}
		}
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (markedByForbiddenWhip && npc.active)
			{
				//npc.color = Color.Lerp(new Color(240, 156, 64), drawColor, 0.5f);
				drawColor = Color.Lerp(new Color(240, 156, 64), drawColor, 0.5f);
				Dust.NewDust(new(npc.Center.X, npc.Center.Y - npc.height / 2), npc.width / 4, npc.height / 4, DustID.GemAmber, 0, 0, 150, default, 0.5f);
			}
		}
	}
}