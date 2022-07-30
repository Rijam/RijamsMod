using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
	public class VileWhipDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vile Whipped");
			Description.SetDefault("+8 summon tag damage");
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
			DisplayName.SetDefault("Vicious Whipped");
			Description.SetDefault("+8 summon tag damage");
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
			DisplayName.SetDefault("Sulfuric Whipped");
			Description.SetDefault("+5 summon tag damage");
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<WhipDebuffNPC>().markedBySulfuricWhip = true;
		}
	}
	public class FestiveWhipDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Festive Whipped");
			Description.SetDefault("+8 summon tag damage");
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
			DisplayName.SetDefault("Supernova Whipped");
			Description.SetDefault("+20 summon tag damage");
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
		public bool markedByFestiveWhip;
		public bool markedBySupernovaWhip;

		public override void ResetEffects(NPC npc)
		{
			markedByVileWhip = false;
			markedByViciousWhip = false;
			markedBySulfuricWhip = false;
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
	}
}