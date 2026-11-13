using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class Belt : WhipItemBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Belt");
			// Tooltip.SetDefault("{$CommonItemTooltip.Whips}\n'Not responsible for triggering traumatic memories'");
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Found in Wooden Chests or Wooden Crates]"] );
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<BeltProj>(), 6, 0.5f, 3f, 45);

			Item.rare = ItemRarityID.White;
			Item.value = 500;
			Item.channel = false;
		}
	}
}
