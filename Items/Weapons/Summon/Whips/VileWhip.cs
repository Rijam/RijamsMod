using Microsoft.Xna.Framework;
using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class VileWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vile Whip");
			// Tooltip.SetDefault("8 summon tag damage\n{$CommonItemTooltip.Whips}");
			CustomItemIDSets.IsWhip[Type] = true;
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect() { TagDamage = 8 };
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<VileWhipProj>(), 22, 2f, 4f, 33);

			Item.width = 44;
			Item.height = 36;
			Item.rare = ItemRarityID.Blue;
			Item.value = 20000;
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

		/*public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			int index = 2;
			if (Item.favorited)
			{
				index += 2;
			}
			tooltips.Insert(index, new TooltipLine(Mod, "Speed", Math.Round(Item.shootSpeed * 2f / player.GetAttackSpeed(DamageClass.SummonMeleeSpeed), 3) + " firing speed"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "Time", Math.Round(Item.useTime * player.GetAttackSpeed(DamageClass.SummonMeleeSpeed), 3) + " firing time"));
		}*/
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DemoniteBar, 10)
				.AddIngredient(ItemID.ShadowScale, 5)
				.AddIngredient(ItemID.WormTooth, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override bool MeleePrefix() => true;
	}
}
