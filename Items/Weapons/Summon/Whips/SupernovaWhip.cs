using Microsoft.Xna.Framework;
using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class SupernovaWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Supernova Whip");
			// Tooltip.SetDefault("20 summon tag damage\n10% summon tag critical strike chance\nCauses Stardust Explosions on enemies\nCan hit enemies through tiles\n{$CommonItemTooltip.Whips}");
			CustomItemIDSets.IsWhip[Type] = true;
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect_SupernovaWhip()
			{
				TagDamage = 20,
				CritChance = 10
			};
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<SupernovaWhipProj>(), 200, 5f, 5f, 24);

			Item.width = 44;
			Item.height = 42;
			Item.rare = ItemRarityID.Red;
			Item.value = 150000;
			Item.channel = false;
			Item.UseSound = SoundID.Item152 with { Pitch = 0.05f };
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

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FragmentStardust, 10)
				.AddIngredient(ItemID.LunarBar, 10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}

		public override bool MeleePrefix() => true;
	}

	public class WhipTagEffect_SupernovaWhip : WhipTagEffect
	{
		public override void OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, float calcDamage)
		{
			base.OnProcHit(owner, optionalProjectile, npcHit, calcDamage);
			
		}

		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, float calcDamage)
		{
			for (int i = 0; i < 10; i++)
			{
				int selectRand = Utils.SelectRandom(Main.rand, DustID.YellowTorch, DustID.BlueTorch);
				int dust = Dust.NewDust(npcHit.position, npcHit.width, npcHit.height, selectRand, npcHit.direction, npcHit.direction, 0, Color.White, 2.0f);
				Main.dust[dust].noGravity = true;
			}
			/*
			//Solar Eruption source
			if (optionalProjectile.owner == Main.myPlayer)
			{
				if (optionalProjectile.damage > optionalProjectile.originalDamage * 0.5f)
				{
					// Main.NewText("Timer: " + optionalProjectile.ai[0]);
					// Main.NewText("swingTime: " + swingTime);
					// Main.NewText("Timer2: " + optionalProjectile.ai[1]);
					float swingTime = owner.itemAnimationMax * optionalProjectile.MaxUpdates;
					if (optionalProjectile.ai[2] > 0 && optionalProjectile.ai[0] < swingTime)
					{
						// Main.NewText("Spawn StardustExplosion");
						Projectile.NewProjectile(
							optionalProjectile.GetSource_FromThis(),
							npcHit.Center.X,
							npcHit.Center.Y,
							0f,
							0f,
							ModContent.ProjectileType<StardustExplosion>(),
							optionalProjectile.damage,
							0,
							optionalProjectile.owner);
						optionalProjectile.ai[1] -= 1;
					}
				}
			}
			*/
		}
	}
}
