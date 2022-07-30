using RijamsMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace RijamsMod.Items.Weapons.Magic
{
	public class Cresceptre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cresceptre");
			Tooltip.SetDefault("'Just going through a purple patch, don't mind me.'");
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.crit = 4;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true; //Channel so that you can hold the weapon [Important]
			Item.mana = 15;
			Item.rare = ItemRarityID.Pink; //5
			Item.width = 64;
			Item.height = 64;
			Item.useTime = 13;
			Item.knockBack = 1;
			Item.UseSound = new(Mod.Name + "/Sounds/Item/Cresceptre") { PitchVariance = 0.05f, MaxInstances = 5 };
			if (!Main.dedServ) //Need to check if a server is running, otherwise it will break multiplayer
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/Weapons/Magic/" + Name).Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -16;
			}
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 12f;
			Item.useAnimation = 13;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CresceptreSphere>();
			Item.value = Item.sellPrice(gold: 6);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, 0);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			float numberProjectiles = 2;// 2 shots
			float rotation = MathHelper.ToRadians(20);
			position += Vector2.Normalize(velocity) * 45f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				{
					position += muzzleOffset;
				}
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("RijamsMod:GoldBars", 10)
				.AddIngredient(ItemID.CrystalShard, 5)
				.AddIngredient(ItemID.GoldDust, 5)
				.AddIngredient(ItemID.SoulofNight, 3)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
