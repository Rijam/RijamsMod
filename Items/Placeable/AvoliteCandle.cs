using RijamsMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Placeable
{
	public class AvoliteCandle : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("'The crystal never runs out of light'");
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.holdStyle = ItemHoldStyleID.HoldFront;
			Item.noWet = false;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.AvoliteCandle>();
			Item.value = 5000;
		}
		public override void HoldItem(Player player)
		{
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, 1f, 0.02f, 0.24f);
		}

		public override void PostUpdate()
		{
			Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 1f, 0.02f, 0.24f);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DD2ElderCrystal)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>())
				.AddTile(TileID.CrystalBall)
				.AddCondition(Condition.InSkyHeight)
				.Register();
		}
	}
}