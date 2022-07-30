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
	public class SupernovaWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Whip");
			Tooltip.SetDefault("20 summon tag damage\n10% summon tag critical strike chance\nCauses Stardust Explosions on enemies\nCan hit enemies through tiles\n{$CommonItemTooltip.Whips}");
			GlobalItems.isWhip.Add(Item.type);
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<SupernovaWhipProj>(), 200, 5f, 5f, 24);

			Item.width = 44;
			Item.height = 42;
			Item.rare = ItemRarityID.Red;
			Item.value = 150000;
			Item.channel = false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FragmentStardust, 10)
				.AddIngredient(ItemID.LunarBar, 10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
