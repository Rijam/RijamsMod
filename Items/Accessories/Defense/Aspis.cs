using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Defense
{
	[AutoloadEquip(EquipType.Shield)]
	public class Aspis : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Dropped by Hoplites]"]);
		}
		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 20);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statDefense++;
			player.GetModPlayer<RijamsModPlayer>().Stat_KnockbackSusceptibility -= 0.5f;
		}
	}
}