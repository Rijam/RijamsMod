using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Melee.JoustingLances
{
	// I made Example Jousting Lance so I'm going to use it!
	public class LonkheJoustingLanceProj : JoustingLanceProjBase
	{
		public override void DustTypes(ref int dustTypeCommon, ref int dustTypeRare, ref int offset)
		{
			dustTypeCommon = DustID.Marble;
			dustTypeRare = DustID.Enchanted_Gold;
			offset = 2;
		}

		public override void CollidingPoints(ref float scaleFactor, ref float widthMultiplier, ref Rectangle lanceHitboxBounds)
		{
			scaleFactor = 64f;
			widthMultiplier = 21f;
			lanceHitboxBounds = new(0, 0, 150, 150);
		}
	}
}