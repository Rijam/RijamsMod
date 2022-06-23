using RijamsMod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace RijamsMod.Items.Weapons
{
	public class Belt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Belt");
			Tooltip.SetDefault("{$CommonItemTooltip.Whips}\n'Not responsible for triggering traumatic memories'");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Found in chests or Wooden Crates]", null, null } );
			GlobalItems.isWhip.Add(Item.type);
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<BeltProj>(), 6, 0.5f, 3f, 45);

			Item.width = 36;
			Item.height = 32;
			Item.rare = ItemRarityID.White;
			Item.value = 500;
			Item.channel = false;
		}
	}
}