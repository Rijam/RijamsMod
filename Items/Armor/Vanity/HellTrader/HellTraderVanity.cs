using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace RijamsMod.Items.Armor.Vanity.HellTrader
{
	[AutoloadEquip(EquipType.Head)]
	public class HellTrader_Hood : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Hell Trader's Hood");
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
	}
	[AutoloadEquip(EquipType.Body)]
	public class HellTrader_Robes : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Hell Trader's Robes");
			ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
			ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
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
	public class HellTrader_Trousers : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hell Trader's Trousers");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
	}
}