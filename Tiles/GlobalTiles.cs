using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace RijamsMod.Tiles
{
    public class GlobalTiles : GlobalTile
    {
		/// <summary> This set is a set of all jousting lances. </summary>
		public static List<int> isPiano = new() { TileID.Pianos };

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
					if (Main.hardMode && Main.rand.NextBool(WorldGen.dontStarveWorldGen | Main.dontStarveWorld ? 100 : 150))
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Consumables.CaveCarrot>());
					}
				}
			}
        }
	}
}