using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class VileWhip : WhipItemBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Vile Whip");
			// Tooltip.SetDefault("8 summon tag damage\n{$CommonItemTooltip.Whips}");
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect() { TagDamage = 8 };
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<VileWhipProj>(), 22, 2f, 4f, 33);

			Item.rare = ItemRarityID.Blue;
			Item.value = 20000;
			Item.channel = false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DemoniteBar, 10)
				.AddIngredient(ItemID.ShadowScale, 5)
				.AddIngredient(ItemID.WormTooth, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
