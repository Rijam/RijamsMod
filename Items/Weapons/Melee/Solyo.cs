using RijamsMod.Projectiles.Melee;
using RijamsMod.Items.Materials;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Melee
{
	public class Solyo : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Inflicts Daybroken\nCauses Solar Flares to fall from the sky");

			// These are all related to gamepad controls and don't seem to affect anything else
			ItemID.Sets.Yoyo[Item.type] = true;
			ItemID.Sets.GamepadExtraRange[Item.type] = 15;
			ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 24;
			Item.height = 24;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 16f;
			Item.knockBack = 4.5f;
			Item.damage = 135;
			Item.rare = ItemRarityID.Cyan;

			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.channel = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(gold: 10);
			Item.shoot = ModContent.ProjectileType<SolyoProjectile>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SunEssence>(), 10)
				.AddIngredient(ItemID.FragmentSolar, 5)
				.AddIngredient(ItemID.LunarTabletFragment, 5) //Solar Tablet Fragment
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
