using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Tiles
{
    public class GlobalTiles : GlobalTile
    {
		public override void Drop(int i, int j, int type)
		{
            if (type == TileID.Pots)
            {
				if (Main.hardMode && Main.rand.NextBool(100))
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Weapons.Ranged.Ammo.BloodyArrow>(), Main.rand.Next(20, 41));
                }
				// Height is in between Caverns layer and Underworld. Increased chance in The Constant worlds. Decreased chance in Hardmode.
				if (j > Main.rockLayer && j < Main.UnderworldLayer)
				{
					if (!Main.hardMode && Main.rand.NextBool(WorldGen.dontStarveWorldGen || Main.dontStarveWorld ? 50 : 75))
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Consumables.CaveCarrot>());
					}
					if (Main.hardMode && Main.rand.NextBool(WorldGen.dontStarveWorldGen || Main.dontStarveWorld ? 100 : 150))
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Consumables.CaveCarrot>());
					}
				}
			}
        }

		public override void NearbyEffects(int i, int j, int type, bool closer)
		{
			if (type == TileID.Candles)
			{
				if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 22 * 22) // Lit Honey Candle
				{
					Main.LocalPlayer.AddBuff(BuffID.Honey, 5);
				}
			}
		}
	}
}