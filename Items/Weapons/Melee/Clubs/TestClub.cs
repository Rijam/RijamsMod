using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Projectiles.Melee.Clubs;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Melee.Clubs
{
	public class TestClub : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Test Club");
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Melee;
			Item.width = 18;
			Item.height = 18;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.knockBack = 8;
			Item.value = Item.buyPrice(gold: 2);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = false;
			Item.crit = 10;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
			Item.noMelee = true; // The projectile will do the damage and not the item

			Item.shoot = ModContent.ProjectileType<TestClubSwingProj>(); // The projectile is what makes a shortsword work
			Item.shootSpeed = 1f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile projectile = Projectile.NewProjectileDirect(source, player.RotatedRelativePoint(player.MountedCenter), velocity, type, damage, knockback, player.whoAmI, 0, 0);
			// Extra logic for the chain to adjust to item stats, unlike the Solar Eruption.
			if (projectile.ModProjectile is TestClubSwingProj modProjectile)
			{
				modProjectile.useTurn = Item.useTurn;
				modProjectile.firingAnimation = (int)Math.Round(Item.useAnimation / (player.GetAttackSpeed(DamageClass.Melee) * player.GetAttackSpeed(DamageClass.Generic)));
				modProjectile.firingTime = (int)Math.Round(Item.useTime / (player.GetAttackSpeed(DamageClass.Melee) * player.GetAttackSpeed(DamageClass.Generic)));
			}
			return false;
		}
	}
}