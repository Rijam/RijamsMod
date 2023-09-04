using RijamsMod.Projectiles.Summon.Whips;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class Belt : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Belt");
			// Tooltip.SetDefault("{$CommonItemTooltip.Whips}\n'Not responsible for triggering traumatic memories'");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Found in Wooden Chests or Wooden Crates]" } );
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

		public override bool MeleePrefix() => true;
	}
}