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
	public class SirSlushsTopHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Sir Slush's Top Hat");
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
    }
}