using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Summoner.Support
{
	[AutoloadEquip(EquipType.Neck)]
	public class ImpChoker : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Sold by Hell Trader]"]);
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionRadiusIncrease += 8;
			player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionPotency += 0.5f;
		}
	}

	[AutoloadEquip(EquipType.Face)]
	public class HarpyHeadband : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Sold by Harpy]"]);
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().Stat_FreeSupportMinion = true;
		}
	}

	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class InterstellarCuffs : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Sold by Interstellar Traveler]"]);
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().Accessory_InterstellarCuffs = true;
		}
	}

	[AutoloadEquip(EquipType.Neck)]
	public class DreamCatcher : ModItem
	{
		public override void SetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 2, 20, 0);
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().Stat_FreeSupportMinion = true;
			player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionRadiusIncrease += 8;
			player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionPotency += 0.5f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<HarpyHeadband>())
				.AddIngredient(ModContent.ItemType<ImpChoker>())
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class FringeCuffs : ModItem
	{
		public override void SetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().Accessory_InterstellarCuffs = true;
			player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionRadiusIncrease += 8;
			player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionPotency += 0.5f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<InterstellarCuffs>())
				.AddIngredient(ModContent.ItemType<HarpyHeadband>())
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class SpikedBracelets : ModItem
	{
		public override void SetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().Accessory_InterstellarCuffs = true;
			player.GetModPlayer<RijamsModPlayer>().Stat_FreeSupportMinion = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<InterstellarCuffs>())
				.AddIngredient(ModContent.ItemType<ImpChoker>())
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	public class UltimateSupportAccessory : ModItem
	{
		public override string Texture => "Terraria/Images/Item_1";

		public override void SetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().Accessory_InterstellarCuffs = true;
			player.GetModPlayer<RijamsModPlayer>().Stat_FreeSupportMinion = true;
			player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionRadiusIncrease += 8;
			player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionPotency += 0.5f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<InterstellarCuffs>())
				.AddIngredient(ModContent.ItemType<HarpyHeadband>())
				.AddIngredient(ModContent.ItemType<ImpChoker>())
				.AddIngredient(ItemID.SoulofLight)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<DreamCatcher>())
				.AddIngredient(ModContent.ItemType<InterstellarCuffs>())
				.AddIngredient(ItemID.SoulofLight)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<FringeCuffs>())
				.AddIngredient(ModContent.ItemType<ImpChoker>())
				.AddIngredient(ItemID.SoulofLight)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SpikedBracelets>())
				.AddIngredient(ModContent.ItemType<HarpyHeadband>())
				.AddIngredient(ItemID.SoulofLight)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
