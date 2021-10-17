using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons
{
	public class Thompson : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("[c/403638:Dropped by Snowman Gansta]");
		}

		public override void SetDefaults()
		{
			item.damage = 23;
			item.ranged = true;
			item.width = 66;
			item.height = 22;
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4;
			item.value = 50000;
			item.rare = ItemRarityID.LightRed;//4
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = AmmoID.Bullet; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 16f;
			item.scale = 0.875f;
			item.useAmmo = AmmoID.Bullet;
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 2);
		}
	}
}
