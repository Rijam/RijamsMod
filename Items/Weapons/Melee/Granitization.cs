using RijamsMod.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons.Melee
{
	public class Granitization : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Granitization");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Granite Golems]", "[c/474747:Or crafted]", null });
			// Tooltip.SetDefault("Cannot hit enemies through tiles\nOnly the blade can hit enemies");
		}
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 48;
			Item.useTime = 48;
			Item.shootSpeed = 6f; //range
			Item.knockBack = 0.25f;
			Item.width = 44;
			Item.height = 36;
			Item.UseSound = SoundID.Item77;
			Item.shoot = ModContent.ProjectileType<GranitizationProj>();
			Item.rare = ItemRarityID.Blue;
			Item.value = 5000;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.autoReuse = false;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 15;
		}
		//From Whips & More
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// How far out the inaccuracy of the shot chain can be.
			float radius = 30f;
			// Sets ai[1] to the following value to determine the firing direction.
			// The smaller the value of NextFloat(), the more accurate the shot will be. The larger, the less accurate. This changes depending on your radius.
			// NextBool().ToDirectionInt() will have a 50% chance to make it negative instead of positive.
			// The Solar Eruption uses this calculation: Main.rand.NextFloat(0f, 0.5f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(45f);
			float direction = Main.rand.NextFloat(0.25f, 1f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(radius);
			Projectile projectile = Projectile.NewProjectileDirect(source, player.RotatedRelativePoint(player.MountedCenter), velocity, type, damage, knockback, player.whoAmI, 0, direction);
			// Extra logic for the chain to adjust to item stats, unlike the Solar Eruption.
			if (projectile.ModProjectile is GranitizationProj modProjectile)
			{
				modProjectile.firingSpeed = Item.shootSpeed * 2f * player.GetTotalAttackSpeed(DamageClass.Melee);
				modProjectile.firingAnimation = Item.useAnimation / player.GetTotalAttackSpeed(DamageClass.Melee);
				modProjectile.firingTime = Item.useTime / player.GetTotalAttackSpeed(DamageClass.Melee);
			}
			return false;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			int index = 3;
			if (Item.favorited)
			{
				index += 2;
			}
			tooltips.Insert(index, new TooltipLine(Mod, "Speed", Math.Round(Item.shootSpeed * 2f * player.GetTotalAttackSpeed(DamageClass.Melee), 3) + " firing speed"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "Time", Math.Round(Item.useTime / player.GetTotalAttackSpeed(DamageClass.Melee), 3) + " firing time"));
		}

		public override bool MeleePrefix() => true;

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.GraniteBlock, 20) // Smooth Granite
				.AddIngredient(ItemID.PlatinumBar, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
