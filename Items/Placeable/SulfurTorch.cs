using RijamsMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Placeable
{
	public class SulfurTorch : ModItem
	{
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Can be placed in water");
		}
        public override void SetDefaults()
		{
			item.width = 10;
			item.height = 12;
			item.maxStack = 999;
			item.holdStyle = 1;
			item.noWet = false;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.SulfurTorch>();
			item.flame = true;
			item.value = 31;
		}

		public override void HoldItem(Player player)
		{
			if (Main.rand.Next(player.itemAnimation > 0 ? 40 : 80) == 0)
			{
				Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, ModContent.DustType<SulfurDust>());
			}
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, Color.Yellow.ToVector3() * 0.75f);
		}

		public override void PostUpdate()
		{
			Lighting.AddLight((int)((item.position.X + item.width / 2) / 16f), (int)((item.position.Y + item.height / 2) / 16f), 0.5f, 0.5f, 0f);
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			dryTorch = false;
			wetTorch = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Torch, 33);
			recipe.AddIngredient(ModContent.ItemType<Materials.Sulfur>());
			recipe.SetResult(this, 33);
			recipe.AddRecipe();
		}
	}
}