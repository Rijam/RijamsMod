using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class TailoThreeCats : ModItem
	{
		public override void SetStaticDefaults()
		{
			CustomItemIDSets.IsWhip[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<TailoThreeCatsProj>(), 30, 2f, 4.25f, 35);

			Item.width = 40;
			Item.height = 36;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(gold: 1, silver: 50);
			Item.channel = false;
		}

		public override bool MeleePrefix() => true;
	}
}
