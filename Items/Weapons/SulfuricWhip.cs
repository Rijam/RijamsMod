using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons
{
	public class SulfuricWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sulfuric Whip");
			Tooltip.SetDefault("5 summon tag damage\nInflicts Sulfuric Acid\n{$CommonItemTooltip.Whips}");
			GlobalItems.isWhip.Add(Item.type);
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<SulfuricWhipProj>(), 45, 2f, 4f, 30);

			Item.width = 44;
			Item.height = 36;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 40000;
			Item.channel = false;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 10)
				.AddIngredient(ModContent.ItemType<Items.Materials.Sulfur>(), 20)
				.AddIngredient(ModContent.ItemType<Items.Materials.InfernicFabric>(), 3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
