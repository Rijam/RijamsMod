using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class SirSlushsTopHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Sir Slush's Top Hat");
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;

			ArmorIDs.Head.Sets.CanDrawOnVelociraptorMount[Item.headSlot] = true;
			ArmorIDs.Head.Sets.VelociraptorMountHeadOffset[Item.headSlot] = new Vector2(0, -2);
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