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
	public class VileWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vile Whip");
			Tooltip.SetDefault("8 summon tag damage\n{$CommonItemTooltip.Whips}");
			GlobalItems.isWhip.Add(Item.type);
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<VileWhipProj>(), 22, 2f, 4f, 33);

			Item.width = 44;
			Item.height = 36;
			Item.rare = ItemRarityID.Blue;
			Item.value = 20000;
			Item.channel = false;
		}
		/*public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			int index = 2;
			if (Item.favorited)
			{
				index += 2;
			}
			tooltips.Insert(index, new TooltipLine(Mod, "Speed", Math.Round(Item.shootSpeed * 2f / player.GetAttackSpeed(DamageClass.SummonMeleeSpeed), 3) + " firing speed"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "Time", Math.Round(Item.useTime * player.GetAttackSpeed(DamageClass.SummonMeleeSpeed), 3) + " firing time"));
		}*/
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DemoniteBar, 10)
				.AddIngredient(ItemID.ShadowScale, 5)
				.AddIngredient(ItemID.WormTooth, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
