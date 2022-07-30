using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class StockadeCrossbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Skeleton Crossbower]", null, null });
		}

		public override void SetDefaults()
		{
			Item.damage = 21;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 56;
			Item.height = 20;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot; //5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 1;
			Item.value = 25000;
			Item.rare = ItemRarityID.Green;//2
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = AmmoID.Arrow; //idk why but all the guns in the vanilla source have this
			Item.shootSpeed = 9f;
			Item.scale = 1f;
			Item.useAmmo = AmmoID.Arrow;
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, -4);
		}
	}
}
