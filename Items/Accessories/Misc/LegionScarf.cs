using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	[AutoloadEquip(EquipType.Neck)]
	public class LegionScarf : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Legion Scarf");
			// Tooltip.SetDefault("+5% increased damage\nWhile in the Snow biome or Space:\n +9% increased damage");
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Dropped by Mister Stabby & Snow Balla]"]);
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.LightRed; //4
			Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			/*
			Old functionality
			if (player.ZoneSnow || player.ZoneSkyHeight) //if Snow biome or Space Layer
			{
				player.GetDamage(DamageClass.Generic) += 0.09f;
			}
			else
			{
				player.GetDamage(DamageClass.Generic) += 0.05f;
			}
			*/
			player.GetModPlayer<RijamsModPlayer>().Accessory_LegionScarf = Item;
		}
	}

	[AutoloadEquip(EquipType.Neck)]
	public class PanicScarf : ModItem
	{
		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.LightRed; //4
			Item.value = Item.sellPrice(0, 2, 75, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().Accessory_LegionScarf = Item;
			player.panic = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PanicNecklace)
				.AddIngredient(ModContent.ItemType<LegionScarf>())
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Back)]
	public class LegionCloak : ModItem
	{
		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.LightRed; //4
			Item.value = Item.sellPrice(0, 2, 75, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().Accessory_LegionScarf = Item;
			player.starCloakItem = Item;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.StarCloak)
				.AddIngredient(ModContent.ItemType<LegionScarf>())
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Back, EquipType.Front)]
	public class LegionSweetheartCloak : ModItem
	{
		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Pink; //5
			Item.value = Item.sellPrice(0, 3, 25, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().Accessory_LegionScarf = Item;
			player.starCloakItem = Item;
			player.panic = true;
			player.honeyCombItem = Item;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SweetheartNecklace)
				.AddIngredient(ModContent.ItemType<LegionCloak>())
				.AddIngredient(ItemID.SoulofMight)
				.AddIngredient(ItemID.SoulofSight)
				.AddIngredient(ItemID.SoulofFright)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.BeeCloak)
				.AddIngredient(ModContent.ItemType<PanicScarf>())
				.AddIngredient(ItemID.SoulofMight)
				.AddIngredient(ItemID.SoulofSight)
				.AddIngredient(ItemID.SoulofFright)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.HoneyComb)
				.AddIngredient(ItemID.StarCloak)
				.AddIngredient(ItemID.PanicNecklace)
				.AddIngredient(ModContent.ItemType<LegionScarf>())
				.AddIngredient(ItemID.SoulofMight)
				.AddIngredient(ItemID.SoulofSight)
				.AddIngredient(ItemID.SoulofFright)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
