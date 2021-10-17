using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class PumpkinHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpkin Hat");
			Tooltip.SetDefault("+1 Minion Capacity");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = 10;
			item.rare = ItemRarityID.White;
			item.defense = 1;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.PumpkinBreastplate && legs.type == ItemID.PumpkinLeggings;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.maxMinions += 1;
			player.setBonus = Language.GetTextValue("ArmorSetBonus.Pumpkin");
			player.allDamage += 0.1f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Pumpkin, 20);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}