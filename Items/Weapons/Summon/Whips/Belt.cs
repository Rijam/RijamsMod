using Microsoft.Xna.Framework;
using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.DataStructures;
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
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Found in Wooden Chests or Wooden Crates]"] );
			CustomItemIDSets.IsWhip[Type] = true;
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

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// This gives some visual variance on how fast the whip swinging animation plays out.
			// This has no effect on the actual collision.
			float swingDirection = 0.6f + (0.4f * Main.rand.NextFloat());
			// 1/3 of the time, swing the whip from the bottom to top instead of from top to bottom.
			// The Dark Harvest is the only whip that doesn't have the chance of swinging from the button up.
			if (Main.rand.NextBool(3))
			{
				swingDirection *= -2.5f;
			}
			// Set swingDirection to 1f for the pre-1.4.5 behavior.

			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, swingDirection);
			return false; // Return false because we've already spawned the projectile.
		}

		public override bool MeleePrefix() => true;
	}
}