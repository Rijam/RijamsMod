using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Summon.Whips;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class SupernovaWhip : WhipItemBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Supernova Whip");
			// Tooltip.SetDefault("20 summon tag damage\n10% summon tag critical strike chance\nCauses Stardust Explosions on enemies\nCan hit enemies through tiles\n{$CommonItemTooltip.Whips}");
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect_SupernovaWhip()
			{
				TagDamage = 20,
				CritChance = 10
			};
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<SupernovaWhipProj>(), 200, 5f, 5f, 24);

			Item.rare = ItemRarityID.Red;
			Item.value = 150000;
			Item.channel = false;
			Item.UseSound = SoundID.Item152 with { Pitch = 0.05f };
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FragmentStardust, 10)
				.AddIngredient(ItemID.LunarBar, 10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}

	public class WhipTagEffect_SupernovaWhip : WhipTagEffect
	{
		public override bool OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, NPC.HitInfo hit)
		{
			return base.OnProcHit(owner, optionalProjectile, npcHit, hit);
		}

		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, NPC.HitInfo hit)
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
