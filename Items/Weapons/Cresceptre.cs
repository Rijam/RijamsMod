using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons
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
			item.damage = 45;
			item.crit = 4;
			item.noMelee = true;
			item.magic = true;
			item.channel = true; //Channel so that you can hold the weapon [Important]
			item.mana = 15;
			item.rare = ItemRarityID.Pink; //5
			item.width = 64;
			item.height = 64;
			item.useTime = 13;
			item.knockBack = 1;
			if (!Main.dedServ) //Need to check if a server is running, otherwise it will break multiplayer
			{
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Cresceptre").WithPitchVariance(.05f);
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/Cresceptre");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -16;
			}
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shootSpeed = 12f;
			item.useAnimation = 13;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<CresceptreSphere>();
			item.value = Item.sellPrice(gold: 6);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			float numberProjectiles = 2;// 2 shots
			float rotation = MathHelper.ToRadians(20);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				{
					position += muzzleOffset;
				}
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("RijamsMod:GoldBars", 10);
			recipe.AddIngredient(ItemID.CrystalShard, 5);
			recipe.AddIngredient(ItemID.GoldDust, 5);
			recipe.AddIngredient(ItemID.SoulofNight, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
