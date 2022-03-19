using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons
{
	public class FlowerOfHailfire : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flower of Hailfire");
			Tooltip.SetDefault("Throws balls of fire and frost");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Crafted in the Snow biome near lava]");
		}

		public override void SetDefaults()
		{
			item.damage = 60;
			item.noMelee = true;
			item.magic = true;
			item.channel = false;
			item.mana = 15;
			item.rare = ItemRarityID.Pink; //5
			item.width = 32;
			item.height = 34;
			item.useTime = 15;
			item.knockBack = 7f;
			item.UseSound = SoundID.Item20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shootSpeed = 8f;
			item.useAnimation = 15;
			item.autoReuse = true;
			item.shoot = ProjectileID.BallofFire;
			item.value = Item.sellPrice(gold: 1);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY) * 10f);
			float numberProjectiles = 2;// 2 shots
			float rotation = MathHelper.ToRadians(10);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				{
					position += muzzleOffset;
				}
				int projType = ProjectileID.BallofFire;
				if (i % 2 == 0)
                {
					projType = ProjectileID.BallofFrost;
				}
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, projType, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FlowerofFire);
			recipe.AddIngredient(ItemID.FlowerofFrost);
			recipe.AddIngredient(ItemID.Fireblossom, 3);
			recipe.AddIngredient(ItemID.Shiverthorn, 3);
			recipe.AddIngredient(ItemID.LivingFireBlock, 20);
			recipe.AddIngredient(ItemID.LivingFrostFireBlock, 20);
			recipe.needSnowBiome = true;
			recipe.needLava = true;
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
