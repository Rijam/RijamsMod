using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Pets
{
	public class DwarfStarBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dwarf Star");
			// Description.SetDefault("Enemies are lit ablaze upon contact");
			Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<RijamsModPlayer>().dwarfStarPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.DwarfStar>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Entity.GetSource_None(), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<Projectiles.Pets.DwarfStar>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class LumothBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lumoth");
			// Description.SetDefault("The bulb only stays lit while the Lumoth flaps its wings.");
			Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<RijamsModPlayer>().lumothPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.Lumoth>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Entity.GetSource_None(), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<Projectiles.Pets.Lumoth>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class LEDLumothBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("LED Lumoth");
			// Description.SetDefault("This Lumoth is much more energy efficient!");
			Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<RijamsModPlayer>().lEDLumothPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.LEDLumoth>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Entity.GetSource_None(), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<Projectiles.Pets.LEDLumoth>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class SnuggetBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Snugget");
			// Description.SetDefault("A fluff ball with legs");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<RijamsModPlayer>().snuggetPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.Snugget>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Entity.GetSource_None(), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<Projectiles.Pets.Snugget>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class FluffaloBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fluffalo");
			// Description.SetDefault("A farm animal and steed");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<RijamsModPlayer>().fluffaloPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.Fluffalo>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Entity.GetSource_None(), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<Projectiles.Pets.Fluffalo>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class BabyStardustDragonBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<RijamsModPlayer>().babyStardustDragonPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.BabyStardustDragon>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Entity.GetSource_None(), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<Projectiles.Pets.BabyStardustDragon>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}