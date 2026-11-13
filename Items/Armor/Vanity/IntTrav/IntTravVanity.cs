using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.Vanity.IntTrav
{
	[AutoloadEquip(EquipType.Head)]
	public class IntTrav_Helmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Interstellar Helmet");
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<PeacekeeperHat>(); // Shimmer transforms the item.
		}
		public override void SetDefaults()
		{
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class IntTrav_Chestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Interstellar Chestplate");
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<PeacekeeperShirt>(); // Shimmer transforms the item.
		}
		public override void SetDefaults()
		{
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class IntTrav_Leggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Interstellar Leggings");
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<PeacekeeperTrousers>(); // Shimmer transforms the item.
		}
		public override void SetDefaults()
		{
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
	}
}