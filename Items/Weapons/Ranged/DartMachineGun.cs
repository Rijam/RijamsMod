using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class DartMachineGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("40% chance not to consume ammo");
		}

		public override void SetDefaults()
		{
			Item.damage = 65;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 68;
			Item.height = 30;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = ItemUseStyleID.Shoot; //5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 4;
			Item.value = 100000;
			Item.rare = ItemRarityID.Yellow; //8
			Item.UseSound = SoundID.Item99;
			Item.autoReuse = true;
			Item.shoot = AmmoID.Dart;
			Item.shootSpeed = 16f;
			//player.scope = true;
			Item.useAmmo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.ChlorophyteBar, 15)
				.AddIngredient(ItemID.Ectoplasm, 5)
				.AddIngredient(ItemID.IllegalGunParts, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		//What if I wanted this gun to have a 38% chance not to consume ammo?
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= 0.40f;
		}

		// What if I wanted an inaccurate gun? (Chain Gun)
		// Inaccurate Gun style: Single Projectile, Random spread 
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 0);
		}
	}
}
