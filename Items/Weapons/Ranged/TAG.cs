using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Ranged;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class TAG : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 32;
			Item.damage = 2;
			Item.knockBack = 1;
			Item.shoot = ModContent.ProjectileType<TAGProj>();
			Item.maxStack = Item.CommonMaxStack;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.shootSpeed = 8;
			Item.rare = ItemRarityID.Blue;
			Item.value = 100;
			Item.consumable = true;
			Item.DamageType = DamageClass.Ranged;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
	}
}