using Microsoft.Xna.Framework;
using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class FestiveWhip : WhipItemBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Festive Whip");
			// Tooltip.SetDefault("8 summon tag damage\nCauses Ornaments to fall from the sky when\nstriking an enemy at the end of the whip\n{$CommonItemTooltip.Whips}");
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Dropped by Ice Queen]"] );
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect_FestiveWhip() { TagDamage = 8 };
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<FestiveWhipProj>(), 120, 4f, 4f, 28);

			Item.rare = ItemRarityID.Yellow;
			Item.value = 100000;
			Item.channel = false;
		}
	}

	public class WhipTagEffect_FestiveWhip : WhipTagEffect
	{
		public override bool OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, NPC.HitInfo hit)
		{
			return base.OnProcHit(owner, optionalProjectile, npcHit, hit);
		}

		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, NPC.HitInfo hit)
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
