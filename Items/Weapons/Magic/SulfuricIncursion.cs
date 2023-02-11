using RijamsMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons.Magic
{
	public class SulfuricIncursion : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Summons balls of sulfur that travel through tiles\nInflicts Sulfuric Acid");
		}

		public override void SetDefaults()
		{
			Item.damage = 33;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true; //Channel so that you can hold the weapon [Important]
			Item.mana = 8;
			Item.rare = ItemRarityID.LightRed;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 14;
			Item.knockBack = 2;
			Item.UseSound = SoundID.Item8;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 6f;
			Item.useAnimation = 14;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SulfurSphere>();
			Item.value = Item.sellPrice(gold: 2);
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SpellTome, 1)
				.AddIngredient(ModContent.ItemType<Items.Materials.Sulfur>(), 20)
				.AddIngredient(ItemID.SoulofLight, 15)
				.AddIngredient(ModContent.ItemType<Items.Materials.InfernicFabric>(), 3)
				.AddTile(TileID.Bookcases)
				.Register();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
	}
}
