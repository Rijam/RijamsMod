using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

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
			Item.width = 20;
			Item.height = 20;
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
			Item.width = 28;
			Item.height = 20;
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
			Item.width = 26;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
	}
}