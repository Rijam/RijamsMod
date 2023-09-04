using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class FestiveWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Festive Whip");
			// Tooltip.SetDefault("8 summon tag damage\nCauses Ornaments to fall from the sky when\nstriking an enemy at the end of the whip\n{$CommonItemTooltip.Whips}");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by Ice Queen]" } );
			GlobalItems.isWhip.Add(Item.type);
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<FestiveWhipProj>(), 120, 4f, 4f, 28);

			Item.width = 36;
			Item.height = 36;
			Item.rare = ItemRarityID.Yellow;
			Item.value = 100000;
			Item.channel = false;
		}

		public override bool MeleePrefix() => true;
	}
}
