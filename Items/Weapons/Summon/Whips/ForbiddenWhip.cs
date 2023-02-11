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
	public class ForbiddenWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Forbidden Whip");
			// Tooltip.SetDefault("Reduces enemy velocity by 20% when struck by a minion\n  Scales with Summon Knockback\n{$CommonItemTooltip.Whips}");
			GlobalItems.isWhip.Add(Item.type);
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<ForbiddenWhipProj>(), 50, 6f, 6f, 40);

			Item.width = 42;
			Item.height = 36;
			Item.rare = ItemRarityID.Pink;
			Item.value = 60000;
			Item.channel = false;
			Item.UseSound = SoundID.Item152 with { Pitch = -0.2f };
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("RijamsMod:MythrilBars", 5)
				.AddIngredient(ItemID.AncientCloth, 5)
				.AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
