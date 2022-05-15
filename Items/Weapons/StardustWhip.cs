using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace RijamsMod.Items.Weapons
{
	public class StardustWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Whip");
			Tooltip.SetDefault("Benefits from Flasks and melee effects\nCauses Stardust Explosions on enemies\nCan hit enemies through tiles\nYour summons will focus struck enemies");
		}
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 24;
			item.useTime = 24;
			item.shootSpeed = 35f; //range
			item.knockBack = 4.5f;
			item.width = 44;
			item.height = 42;
			item.UseSound = SoundID.Item77;
			item.shoot = ModContent.ProjectileType<StardustWhipProj>();
			item.rare = ItemRarityID.Red;
			item.value = 150000;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.channel = true;
			item.autoReuse = true;
			item.summon = true;
			item.damage = 70;
			item.crit = 0;
		}
		//From Whips & More
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// How far out the inaccuracy of the shot chain can be.
			float radius = 30f;
			// Sets ai[1] to the following value to determine the firing direction.
			// The smaller the value of NextFloat(), the more accurate the shot will be. The larger, the less accurate. This changes depending on your radius.
			// NextBool().ToDirectionInt() will have a 50% chance to make it negative instead of positive.
			// The Solar Eruption uses this calculation: Main.rand.NextFloat(0f, 0.5f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(45f);
			float direction = Main.rand.NextFloat(0.1f, 0.75f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(radius);
			Projectile projectile = Projectile.NewProjectileDirect(player.RotatedRelativePoint(player.MountedCenter), new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, 0f, direction);
			// Extra logic for the chain to adjust to item stats, unlike the Solar Eruption.
			if (projectile.modProjectile is StardustWhipProj modItem)
			{
				modItem.firingSpeed = item.shootSpeed * 2f / player.meleeSpeed;
				modItem.firingAnimation = item.useAnimation * player.meleeSpeed;
				modItem.firingTime = item.useTime * player.meleeSpeed;
			}
			return false;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			int index = 2;
			if (item.favorited)
			{
				index += 2;
			}
			tooltips.Insert(index, new TooltipLine(mod, "Speed", Math.Round(item.shootSpeed * 2f / player.meleeSpeed, 3) + " firing speed"));
			tooltips.Insert(index + 1, new TooltipLine(mod, "Time", Math.Round(item.useTime * player.meleeSpeed, 3) + " firing time"));
			tooltips.Insert(index + 2, new TooltipLine(mod, "Crit", item.crit + 4 + "% critical strike chance"));
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentStardust, 10);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
