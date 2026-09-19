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
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref modPlayer.Pet_DwarfStarPet, ModContent.ProjectileType<Projectiles.Pets.DwarfStar>());
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
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref modPlayer.Pet_LumothPet, ModContent.ProjectileType<Projectiles.Pets.Lumoth>());
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
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref modPlayer.Pet_LEDLumothPet, ModContent.ProjectileType<Projectiles.Pets.LEDLumoth>());
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
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref modPlayer.Pet_SnuggetPet, ModContent.ProjectileType<Projectiles.Pets.Snugget>());
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
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref modPlayer.Pet_FluffaloPet, ModContent.ProjectileType<Projectiles.Pets.Fluffalo>());
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
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref modPlayer.Pet_BabyStardustDragonPet, ModContent.ProjectileType<Projectiles.Pets.BabyStardustDragon>());
		}
	}
}