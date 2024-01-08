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
			// DisplayName.SetDefault("Pumpkin Hat");
			// Tooltip.SetDefault("+1 Minion Capacity");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 10;
			Item.rare = ItemRarityID.White;
			Item.defense = 1;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.PumpkinBreastplate && legs.type == ItemID.PumpkinLeggings;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("ArmorSetBonus.Pumpkin");
			player.GetDamage(DamageClass.Generic) += 0.1f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Pumpkin, 20)
				.AddIngredient(ItemID.FallenStar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}