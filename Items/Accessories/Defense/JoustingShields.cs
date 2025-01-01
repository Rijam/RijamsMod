using RijamsMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Defense
{
	[AutoloadEquip(EquipType.Shield)]
	public class EcrancheShield : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 1);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.moveSpeedDamageReductionMax = 0.1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DynastyWood, 10)
				.AddIngredient(ItemID.Leather, 1)
				.AddTile(TileID.Sawmill)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Shield)]
	public class GrandGuardShield : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 75);
			Item.rare = ItemRarityID.Green;
			Item.defense = 1;
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.moveSpeedDamageReductionMax = 0.2f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 10)
				.AddIngredient(ModContent.ItemType<InfernicFabric>(), 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Shield)]
	public class HeaterShield : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 1, 50);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 1;
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.moveSpeedDamageReductionMax = 0.3f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<FestivePlating>(), 15)
				.AddIngredient(ItemID.Silk, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
