using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons
{
	public class HotStick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hot Stick");
		}

		public override void SetDefaults()
		{
			item.damage = 5;
			item.noMelee = false;
			item.magic = true;
			item.channel = true; //Channel so that you can hold the weapon [Important]
			item.mana = 1;
			item.rare = ItemRarityID.White; //1
			item.width = 22;
			item.height = 24;
			item.useAnimation = 12;
			item.useTime = 12;
			item.knockBack = 0.1f;
			item.UseSound = SoundID.Item1;
			item.useStyle = ItemUseStyleID.Stabbing;
			//item.shootSpeed = 16f;
			item.autoReuse = true;
			//item.noUseGraphic = true;
			//item.shoot = ModContent.ProjectileType<HotStickProjectile>();
			//item.shootSpeed = 1f;
			item.value = Item.sellPrice(copper: 5);
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			// 60 frames = 1 second
			if (Main.rand.Next(10) == 0)
			{
				target.AddBuff(BuffID.OnFire, 60);
			}
			else if (Main.rand.Next(10) == 0)
			{
				target.AddBuff(BuffID.OnFire, 30);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("Wood", 3);
			recipe.AddIngredient(ItemID.Torch, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
	}
}
