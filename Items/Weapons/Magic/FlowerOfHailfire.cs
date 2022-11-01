using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons.Magic
{
	public class FlowerOfHailfire : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flower of Hailfire");
			Tooltip.SetDefault("Throws balls of fire and frost");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Crafted in the Snow biome near lava]", null, null } );
		}

		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.channel = false;
			Item.mana = 15;
			Item.rare = ItemRarityID.Pink; //5
			Item.width = 32;
			Item.height = 34;
			Item.useTime = 15;
			Item.knockBack = 7f;
			Item.UseSound = SoundID.Item20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 8f;
			Item.useAnimation = 15;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.BallofFire;
			Item.value = Item.sellPrice(gold: 1);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity * 10f);
			float numberProjectiles = 2;// 2 shots
			float rotation = MathHelper.ToRadians(10);
			position += Vector2.Normalize(velocity) * 10f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				{
					position += muzzleOffset;
				}
				int projType = ProjectileID.BallofFire;
				if (i % 2 == 0)
				{
					projType = ProjectileID.BallofFrost;
				}
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, projType, damage, knockback, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FlowerofFire)
				.AddIngredient(ItemID.FlowerofFrost)
				.AddIngredient(ItemID.Fireblossom, 3)
				.AddIngredient(ItemID.Shiverthorn, 3)
				.AddIngredient(ItemID.LivingFireBlock, 20)
				.AddIngredient(ItemID.LivingFrostFireBlock, 20)
				.AddCondition(Recipe.Condition.InSnow)
				.AddCondition(Recipe.Condition.NearLava)
				.Register();
		}
	}
}
