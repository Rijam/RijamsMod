using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons
{
	public class SulfuricIncursion : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons balls of sulfur that travel through tiles\nInflicts Sulfuric Acid");
		}

		public override void SetDefaults()
		{
			item.damage = 33;
			item.noMelee = true;
			item.magic = true;
			item.channel = true; //Channel so that you can hold the weapon [Important]
			item.mana = 8;
			item.rare = ItemRarityID.LightRed;
			item.width = 28;
			item.height = 30;
			item.useTime = 14;
			item.knockBack = 2;
			item.UseSound = SoundID.Item8;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shootSpeed = 6f;
			item.useAnimation = 14;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<SulfurSphere>();
			item.value = Item.sellPrice(gold: 2);
		}
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.Sulfur>(), 20);
			recipe.AddIngredient(ItemID.SoulofLight, 15);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.InfernicFabric>(), 3);
			recipe.AddTile(TileID.Bookcases);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
	}
}
