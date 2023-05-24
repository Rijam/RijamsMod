using RijamsMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class SemiAutoDartSniper : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("50% chance not to consume ammo\nRight Click to zoom out");
			ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 125;
			Item.crit = 16;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 92;
			Item.height = 28;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Shoot; //5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 6;
			Item.value = 120000;
			Item.rare = ItemRarityID.Cyan; //9
			Item.UseSound = SoundID.Item98;
			Item.autoReuse = true;
			Item.useTurn = false;
			Item.shoot = AmmoID.Dart; //idk why but all the guns in the vanilla source have this
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.ShroomiteBar, 15)
				.AddIngredient(ItemID.FragmentVortex, 10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}

		//What if I wanted this gun to have a 38% chance not to consume ammo?
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= 0.50f;
		}
		public override void HoldItem(Player player)
		{
			player.scope = true;
		}
		
		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, 0);
		}

		// How can I shoot 2 different projectiles at the same time?
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Main.rand.NextBool(4)) //1 in 4 chance
			{
				// Here we manually spawn the 2nd projectile, manually specifying the projectile type that we wish to shoot.
				Projectile.NewProjectile(source, position, velocity, ProjectileID.VortexBeaterRocket, damage, knockback, Main.myPlayer); //or could try PhantasmArrow
				 // By returning true, the vanilla behavior will take place, which will shoot the 1st projectile, the one determined by the ammo.
				return true;
			}
			return true;
		}
	}
}
