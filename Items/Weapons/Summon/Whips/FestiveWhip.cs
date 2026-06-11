using Microsoft.Xna.Framework;
using RijamsMod.Buffs.Debuffs;
using RijamsMod.Projectiles.Summon.Whips;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class FestiveWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Festive Whip");
			// Tooltip.SetDefault("8 summon tag damage\nCauses Ornaments to fall from the sky when\nstriking an enemy at the end of the whip\n{$CommonItemTooltip.Whips}");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by Ice Queen]" } );
			CustomItemIDSets.IsWhip[Type] = true;
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect_FestiveWhip() { TagDamage = 8 };
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<FestiveWhipProj>(), 120, 4f, 4f, 28);

			Item.width = 36;
			Item.height = 36;
			Item.rare = ItemRarityID.Yellow;
			Item.value = 100000;
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

	public class WhipTagEffect_FestiveWhip : WhipTagEffect
	{
		public override void OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, float calcDamage)
		{
			base.OnProcHit(owner, optionalProjectile, npcHit, calcDamage);
			
		}

		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, float calcDamage)
		{
			for (int i = 0; i < 5; i++)
			{
				int selectRand = Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.RedTorch);
				Dust.NewDust(npcHit.position, npcHit.width, npcHit.height, selectRand, npcHit.direction, npcHit.direction, 0, Color.White, 1f);
			}

			/*
			//Solar Eruption source
			if (optionalProjectile.owner == Main.myPlayer)
			{
				if (optionalProjectile.damage > optionalProjectile.originalDamage * 0.5f)
				{
					//Main.NewText("Timer: " + optionalProjectile.ai[0]);
					//Main.NewText("swingTime: " + swingTime);
					float swingTime = owner.itemAnimationMax * optionalProjectile.MaxUpdates;
					if (optionalProjectile.ai[0] > (int)(swingTime * 0.55f) && optionalProjectile.ai[0] < (int)(swingTime * 0.75f) && owner.itemAnimation > 0)
					{
						//Main.NewText("Spawn Ornament");
						Projectile.NewProjectile(
							optionalProjectile.GetSource_FromThis(),
							new Vector2(npcHit.position.X, npcHit.position.Y - Main.screenHeight - 100 - (npcHit.position.Y - owner.position.Y)),
							new Vector2(Main.rand.NextFloat(-2, 2f),
							Main.rand.Next(10, 14)),
							ModContent.ProjectileType<FestiveOrnament>(),
							optionalProjectile.damage / 2,
							optionalProjectile.knockBack / 2,
							optionalProjectile.owner, -1,
							Main.rand.Next(0, 4));
					}
				}
			}
			*/
		}
	}
}
