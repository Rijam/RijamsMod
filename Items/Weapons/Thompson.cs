using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons
{
	public class Thompson : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Snowman Gansta]", null, null });
		}

		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 66;
			Item.height = 22;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 4;
			Item.value = 50000;
			Item.rare = ItemRarityID.LightRed;//4
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = AmmoID.Bullet; //idk why but all the guns in the vanilla source have this
			Item.shootSpeed = 16f;
			Item.scale = 0.875f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
		}
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 2);
		}
	}
}
