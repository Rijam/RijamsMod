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
			item.defense = 0;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = mod.GetEquipSlot("Harpy_Vanity_Shorts", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("Harpy_Vanity_Shorts_Female", EquipType.Legs);
        }
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

	[AutoloadEquip(EquipType.Head)]
	public class SirSlushsTopHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Sir Slush's Top Hat");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 20;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
			drawAltHair = true;
        }
    }
	[AutoloadEquip(EquipType.Head)]
	public class HellTrader_Vanity_Hood : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Hell Trader's Hood");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 24;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class HellTrader_Vanity_Robes : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Hell Trader's Robes");
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
	public class HellTrader_Vanity_Trousers : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hell Trader's Trousers");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
}