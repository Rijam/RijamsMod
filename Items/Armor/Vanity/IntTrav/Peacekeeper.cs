using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;

namespace RijamsMod.Items.Armor.Vanity.IntTrav
{
	[AutoloadEquip(EquipType.Head)]
	public class PeacekeeperHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Peacekeeper Hat");
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<IntTrav_Helmet>(); // Shimmer transforms the item.
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
	public class PeacekeeperShirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Peacekeeper Shirt");
			ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<IntTrav_Chestplate>(); // Shimmer transforms the item.
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
	public class PeacekeeperTrousers : ModItem
	{
		public int LegEquipTextureFemale;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTextureFemale = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_FemaleLegs", EquipType.Legs, this);
			}
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Peacekeeper Trousers");
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<IntTrav_Leggings>(); // Shimmer transforms the item.
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
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			if (!male) equipSlot = LegEquipTextureFemale;
		}
	}
}