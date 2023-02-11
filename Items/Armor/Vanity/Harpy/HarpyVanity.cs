using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

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
			Item.width = 18;
			Item.height = 20;
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
			Item.width = 22;
			Item.height = 24;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}

		//Thanks Exterminator for the help
		public int LegEquipTextureMale;
		public int LegEquipTextureFemale;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTextureMale = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_Legs", EquipType.Legs, this);
				LegEquipTextureFemale = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_FemaleLegs", EquipType.Legs, this);
			}
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			if (male) equipSlot = LegEquipTextureMale;
			if (!male) equipSlot = LegEquipTextureFemale;
		}
	}
}