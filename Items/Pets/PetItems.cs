using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Pets
{
	public class LumothBulb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lumoth Bulb");
			Tooltip.SetDefault("Summons a Lumoth");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 16;
			item.height = 28;
			item.shoot = ModContent.ProjectileType<Projectiles.Pets.Lumoth>();
			item.buffType = ModContent.BuffType<Buffs.LumothBuff>();
			item.rare = ItemRarityID.LightRed;
			item.value = 15000;
			item.UseSound = SoundID.Item25;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "LargeGlowRing", 1);
			recipe.AddIngredient(ItemID.PixieDust, 1);
			recipe.AddIngredient(ItemID.SoulofFlight, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}