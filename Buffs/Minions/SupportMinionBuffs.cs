using Microsoft.Xna.Framework;
using RijamsMod.Items;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Minions
{
	public class HarpyIdolBuff : ModBuff
	{
		public override LocalizedText Description => base.Description.WithFormatArgs(3, 1, 5);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Harpy Idol");
			// Description.SetDefault("The Harpy Idol will defend you\nPlayers within its aura receive:\n+3 defense\n+1% damage reduction\n5 tile radius");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			SupportMinionCanUseCheck.SupportMinionsDefenseBuffs.Add(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Support.HarpyIdol>()] > 0)
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
	public class CobaltProtectorBuff : ModBuff
	{
		public override LocalizedText Description => base.Description.WithFormatArgs(5, 2, 10);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cobalt Protector");
			// Description.SetDefault("The Cobalt Protector will defend you\nPlayers within its aura receive:\n+5 defense\n+2% damage reduction\n10 tile radius");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			SupportMinionCanUseCheck.SupportMinionsDefenseBuffs.Add(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Support.CobaltProtector>()] > 0)
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
	public class CrystalClusterBuff : ModBuff
	{
		public override LocalizedText Description => base.Description.WithFormatArgs(7, 4, 15);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crystal Cluster");
			// Description.SetDefault("The Crystal Cluster will defend you\nPlayers within its aura receive:\n+7 defense\n+4% damage reduction\n15 tile radius");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			SupportMinionCanUseCheck.SupportMinionsDefenseBuffs.Add(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Support.CrystalCluster>()] > 0)
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
	public class FallenPaladinBuff : ModBuff
	{
		public override LocalizedText Description => base.Description.WithFormatArgs(10, 5, 20);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fallen Paladin");
			// Description.SetDefault("The Fallen Paladin will defend you\nPlayers within its aura receive:\n+10 defense\n+5% damage reduction\n20 tile radius");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			SupportMinionCanUseCheck.SupportMinionsDefenseBuffs.Add(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Support.FallenPaladin>()] > 0)
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
	public class StardustProtectorBuff : ModBuff
	{
		public override LocalizedText Description => base.Description.WithFormatArgs(13, 8, 30);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Protector");
			// Description.SetDefault("The Stardust Protector will defend you\nPlayers within its aura receive:\n+13 defense\n+8% damage reduction\n30 tile radius");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			SupportMinionCanUseCheck.SupportMinionsDefenseBuffs.Add(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Support.StardustProtector>()] > 0)
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
	public class GraniteElementalBuff : ModBuff
	{
		public override LocalizedText Description => base.Description.WithFormatArgs(20, 40, 10);
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			SupportMinionCanUseCheck.SupportMinionsHealingBuffs.Add(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Support.GraniteElemental>()] > 0)
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
	public class SanityFlowerBuff : ModBuff
	{
		public override LocalizedText Description => base.Description.WithFormatArgs(20, 30, 15);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sanity Flower");
			// Description.SetDefault("The Sanity Flower will heal you\nPlayers within its aura will be healed\n20 HP every 30 seconds\n20 tile radius");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			SupportMinionCanUseCheck.SupportMinionsHealingBuffs.Add(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Support.SanityFlower>()] > 0)
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

	public class RadiantLanternBuff : ModBuff
	{
		public override LocalizedText Description => base.Description.WithFormatArgs(20, 30, 30);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Radiant Lantern");
			// Description.SetDefault("The Radiant Lantern will heal you\nPlayers within its aura will be healed\n20 HP every 30 seconds\n30 tile radius");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			SupportMinionCanUseCheck.SupportMinionsHealingBuffs.Add(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Support.RadiantLantern>()] > 0)
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