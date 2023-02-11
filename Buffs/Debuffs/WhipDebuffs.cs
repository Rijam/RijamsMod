using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Debuffs
{
	public class VileWhipDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vile Whipped");
			// Description.SetDefault("+8 summon tag damage");
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
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vicious Whipped");
			// Description.SetDefault("+8 summon tag damage");
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
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sulfuric Whipped");
			// Description.SetDefault("+5 summon tag damage");
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
			// DisplayName.SetDefault("Forbidden Whipped");
			// Description.SetDefault("");
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
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Festive Whipped");
			// Description.SetDefault("+8 summon tag damage");
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
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Supernova Whipped");
			// Description.SetDefault("+20 summon tag damage");
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

		// TODO: Inconsistent with vanilla, increasing damage AFTER it is randomised, not before. Change to a different hook in the future.
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			// Only player attacks should benefit from this buff, hence the NPC and trap checks.
			if (!projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
			{
				if (markedByVileWhip)
				{
					damage += 8;
				}
				if (markedByViciousWhip)
				{
					damage += 8;
				}
				if (markedBySulfuricWhip)
				{
					damage += 10;
					Dust.NewDust(npc.Center, npc.width, npc.height, ModContent.DustType<Dusts.SulfurDust>(), 1f * hitDirection, 1f * hitDirection, 150, default, 1f);
				}
				if (markedByForbiddenWhip)
				{
					// Player owner = Main.player[projectile.owner];
					// float ownerSummonKB = owner.GetTotalKnockback(DamageClass.Summon).Additive * owner.GetTotalKnockback(DamageClass.Summon).Multiplicative;
					// float ownerSummonKB2 = owner.GetTotalKnockback(DamageClass.Summon).Base; // Vanilla uses .Base instead of the others

					// Oddly, the projectile doesn't update it's knockback dynamically like it does with damage.

					float multiplier = 0.8f;
					if (knockback != 0)
					{
						multiplier -= (knockback / 50f); // multiplier becomes less with more knockback
					}
					npc.velocity *= multiplier;
					// Main.NewText("ownerSummonKB " + ownerSummonKB + " ownerSummonKB2 " + ownerSummonKB2 + " knockback " + knockback + " multiplier " + multiplier);
					// Main.NewText("damage " + damage + " knockback " + knockback + " crit " + crit + " multiplier " + multiplier);
					Dust.NewDust(npc.Center, npc.width, npc.height, DustID.GemAmber, 1f * hitDirection, 1f * hitDirection, 150, default, 1f);
				}
				if (markedByFestiveWhip)
				{
					damage += 8;
					for (int i = 0; i < 5; i++)
					{
						int selectRand = Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.RedTorch);
						Dust.NewDust(npc.position, npc.width, npc.height, selectRand, 1f * hitDirection, 1f * hitDirection, 0, Color.White, 1f);
					}
				}
				if (markedBySupernovaWhip)
				{
					damage += 20;
					if (Main.rand.NextBool(10))
					{
						crit = true;
					}
					for (int i = 0; i < 10; i++)
					{
						int selectRand = Utils.SelectRandom(Main.rand, DustID.YellowTorch, DustID.BlueTorch);
						int dust = Dust.NewDust(npc.position, npc.width, npc.height, selectRand, 1f * hitDirection, 1f * hitDirection, 0, Color.White, 2.0f);
						Main.dust[dust].noGravity = true;
					}
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