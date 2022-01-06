using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RijamsMod.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class RedHarpyWings : ModItem
	{
		/*public override bool Autoload(ref string name)
		{
			return;
		}*/

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows flight and slow fall\nBetter than average horizontal acceleration");
		}

		public override void SetDefaults() {
			item.width = 22;
			item.height = 20;
			item.value = 80000;
			item.rare = ItemRarityID.Red;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 170; //2.83 second
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2.5f;
			constantAscend = 0.135f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 6f;
			acceleration *= 12f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 1);
			recipe.AddIngredient(ItemID.SoulofFlight, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			bool isLeftShiftHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
			if (isLeftShiftHeld)
			{
				tooltips.Add(new TooltipLine(mod, "Stats", "Flight Time: 170 (2.83 seconds)"));
				tooltips.Add(new TooltipLine(mod, "Stats", "Max Height: 136 tiles"));
				tooltips.Add(new TooltipLine(mod, "Stats", "Max Horizontal Speed: 32"));
				tooltips.Add(new TooltipLine(mod, "Stats", "Vertical Speed Multiplier: 250%"));
			}
		}
	}
}