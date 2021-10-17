using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//using Terraria.GameContent.Creative;

namespace RijamsMod.Items.Placeable
{
	public class RecyclingMachine : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Recycling Machine");
			Tooltip.SetDefault("'It's unlike anything you've ever seen.'");
		}
		public override void SetDefaults()
		{

			item.maxStack = 99;
			item.consumable = true;
			item.width = 28;
			item.height = 40;
			item.value = 5000;
			item.rare = ItemRarityID.Blue;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.createTile = ModContent.TileType<Tiles.RecyclingMachine>();
		}

		/*public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}*/
	}
}