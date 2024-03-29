using RijamsMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Items.Information;

namespace RijamsMod.Items.Placeable
{
	public class SulfurTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Can be placed in water");
			ItemID.Sets.Torches[Type] = true;
			ItemID.Sets.WaterTorches[Type] = true;
			ItemID.Sets.SingleUseInGamepad[Type] = true;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.ShimmerTorch; // Shimmer transforms the item.
		}
		public override void SetDefaults()
		{
			Item.DefaultToTorch(ModContent.TileType<Tiles.SulfurTorch>(), 0, true);
			Item.value = 31;
		}

		public override void HoldItem(Player player)
		{
			if (Main.rand.NextBool(player.itemAnimation > 0 ? 40 : 80))
			{
				Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, ModContent.DustType<SulfurDust>());
			}
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, Color.Yellow.ToVector3() * 0.75f);
		}

		public override void PostUpdate()
		{
			Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 0.5f, 0.5f, 0f);
		}

		public override void AddRecipes()
		{
			CreateRecipe(33)
				.AddIngredient(ItemID.Torch, 33)
				.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 1)
				.Register();
		}
	}
}