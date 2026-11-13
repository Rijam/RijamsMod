using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

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

			ArmorIDs.Head.Sets.CanDrawOnVelociraptorMount[Item.headSlot] = true;
			ArmorIDs.Head.Sets.VelociraptorMountHeadOffset[Item.headSlot] = new Vector2(2, -2);
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
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PeacekeeperTrousers : ModItem
	{
		public Asset<Texture2D> LegEquipTextureFemale;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Peacekeeper Trousers");
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<IntTrav_Leggings>(); // Shimmer transforms the item.
		}
		public override void SetDefaults()
		{
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}

		public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
		{
			if (!drawInfo.drawPlayer.Male)
			{
				LegEquipTextureFemale ??= ModContent.Request<Texture2D>((GetType().Namespace + "." + Name).Replace('.', '/') + "_FemaleLegs");

				drawData.texture = LegEquipTextureFemale.Value;
			}

			return base.ModifyEquipTextureDraw(ref drawInfo, ref drawData, equipTexture, methodName);
		}
	}
}
