using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Minions
{
	public class BabyBloodEelBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.BuffTextHandlers.Add(Type, new WormCachedProjectileCounterBuffTextHandler(ModContent.ProjectileType<Projectiles.Summon.Minions.BabyBloodEel>()));
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Minions.BabyBloodEel>()] > 0)
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

	public class BabyBoneSerpentBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.BuffTextHandlers.Add(Type, new WormCachedProjectileCounterBuffTextHandler(ModContent.ProjectileType<Projectiles.Summon.Minions.BabyBoneSerpent>()));
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Minions.BabyBoneSerpent>()] > 0)
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

	public class GiantWormBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.BuffTextHandlers.Add(Type, new WormCachedProjectileCounterBuffTextHandler(ModContent.ProjectileType<Projectiles.Summon.Minions.GiantWorm>()));
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Minions.GiantWorm>()] > 0)
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

	public class WormCachedProjectileCounterBuffTextHandler(params int[] projectileTypesToLookFor) : IBuffTextHandler
	{
		public string HandleBuffText()
		{
			if (projectileTypesToLookFor == null)
				return null;

			int[] ownedProjectileCounts = Main.LocalPlayer.ownedProjectileCounts;
			float count = 0f;
			int[] array = projectileTypesToLookFor;
			foreach (int type in array)
			{
				count += (float)ownedProjectileCounts[type];
				foreach (Projectile projectile in Main.ActiveProjectiles)
				{
					if (projectile.type != type)
					{
						continue;
					}
					if (projectile.owner != Main.LocalPlayer.whoAmI)
					{
						continue;
					}
					count += (projectile.minionSlots - 1);
				}
			}

			if (count > 0f)
				return "x" + count;

			return null;
		}
	}
}