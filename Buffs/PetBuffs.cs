using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
	public class LumothBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Lumoth");
			Description.SetDefault("The bulb only stays lit while the Lumoth flaps its wings.");
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
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<Projectiles.Pets.Lumoth>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}