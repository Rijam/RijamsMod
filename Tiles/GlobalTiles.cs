using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace RijamsMod.Tiles
{
    public class GlobalTiles : GlobalTile
    {
        public override bool Drop(int i, int j, int type)
        {
            if (type == TileID.Pots)
            {
                if (Main.hardMode && Main.rand.Next(100) == 0)
                {
                    Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Weapons.Ammo.BloodyArrow>(), Main.rand.Next(20, 40));
                }
            }
            return base.Drop(i, j, type);
        }
    }
}