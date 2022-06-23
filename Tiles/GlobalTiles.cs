using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;

namespace RijamsMod.Tiles
{
    public class GlobalTiles : GlobalTile
    {
        public override bool Drop(int i, int j, int type)
        {
            if (type == TileID.Pots)
            {
                if (Main.hardMode && Main.rand.NextBool(100))
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Weapons.Ammo.BloodyArrow>(), Main.rand.Next(20, 41));
                }
            }
            return base.Drop(i, j, type);
        }
    }
}