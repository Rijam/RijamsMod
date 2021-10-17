using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace RijamsMod.Items.Armor.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class IntTrav_Vanity_Helmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Interstellar Helmet");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class IntTrav_Vanity_Chestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Interstellar Chestplate");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class IntTrav_Vanity_Leggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Interstellar Leggings");
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 20;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense=0;
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class Harpy_Vanity_Shirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Harpy's Shirt");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 20;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = true;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class Harpy_Vanity_Shorts : ModItem
	{
		public override bool Autoload(ref string name)
        {
            return true;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy's Shorts");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 24;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense=0;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = mod.GetEquipSlot("Harpy_Vanity_Shorts", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("Harpy_Vanity_Shorts_Female", EquipType.Legs);
        }
		/*public override string Texture
		{
			if (Main.player[Main.myPlayer].Male)
			{
				AddEquipTexture(GetItem("??"), EquipType.Legs, "Harpy_Vanity_Shorts", "RijamsMod/Items/Armor/Vanity/Harpy_Vanity_Shorts_Legs");
			}
			else
			{
				AddEquipTexture(GetItem("??"), EquipType.Legs, "Harpy_Vanity_Shorts_Female", "RijamsMod/Items/Armor/Vanity/Harpy_Vanity_Shorts_Legs_Female");
			}
		}*/
	}
	[AutoloadEquip(EquipType.Body)]
	public class Fisherman_Vanity_Shirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Fisherman's Shirt");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = false;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class Fisherman_Vanity_Pants : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fisherman's Pants");
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 20;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense=0;
		}
	}
}