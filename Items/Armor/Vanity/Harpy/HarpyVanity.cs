using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.Vanity.Harpy
{
	[AutoloadEquip(EquipType.Body)]
	public class Harpy_Shirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Harpy's Shirt");
			ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = false;
			ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
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
	public class Harpy_Shorts : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Harpy's Shorts");
		}
		public override void SetDefaults()
		{
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}

		public Asset<Texture2D> LegEquipTextureFemale;

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
