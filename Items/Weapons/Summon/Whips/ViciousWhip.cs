using Terraria;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Summon.Whips;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class ViciousWhip : WhipItemBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Vicious Whip");
			// Tooltip.SetDefault("8 summon tag damage\n{$CommonItemTooltip.Whips}");
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect() { TagDamage = 8 };
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<ViciousWhipProj>(), 25, 2f, 4f, 36);

			Item.rare = ItemRarityID.Blue;
			Item.value = 20000;
			Item.channel = false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CrimtaneBar, 10)
				.AddIngredient(ItemID.TissueSample, 5)
				.AddIngredient(ModContent.ItemType<Materials.CrawlerChelicera>(), 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
