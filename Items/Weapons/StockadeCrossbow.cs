using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons
{
	public class StockadeCrossbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("[c/403638:Dropped by Skeleton Crossbower]");
		}

		public override void SetDefaults()
		{
			item.damage = 21;
			item.ranged = true;
			item.width = 56;
			item.height = 20;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut; //5
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 1;
			item.value = 25000;
			item.rare = ItemRarityID.Green;//2
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shoot = AmmoID.Arrow; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 9f;
			item.scale = 1f;
			item.useAmmo = AmmoID.Arrow;
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, -4);
		}
	}
}
