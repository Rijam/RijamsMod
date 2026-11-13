using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Summon.Whips;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class ForbiddenWhip : WhipItemBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Forbidden Whip");
			// Tooltip.SetDefault("Reduces enemy velocity by 20% when struck by a minion\n  Scales with Summon Knockback\n{$CommonItemTooltip.Whips}");
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect_ForbiddenWhip() { VelocityReductionPercent = 20, TagDuration = 180 };
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<ForbiddenWhipProj>(), 50, 6f, 6f, 40);

			Item.rare = ItemRarityID.Pink;
			Item.value = 60000;
			Item.channel = false;
			Item.UseSound = SoundID.Item152 with { Pitch = -0.2f };
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(RecipeGroups.MythrilBar, 5)
				.AddIngredient(ItemID.AncientCloth, 5)
				.AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class WhipTagEffect_ForbiddenWhip : WhipTagEffect
	{
		public int VelocityReductionPercent;
		public float VelocityReductionMultiplier => 1f - (VelocityReductionPercent / 100f);

		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, NPC.HitInfo hit)
		{
			CombatText.NewText(optionalProjectile.Hitbox, Color.OrangeRed, "⏪");
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.DeadCellsMushroomBoiTargetFound, new ParticleOrchestraSettings
			{
				UniqueInfoPiece = optionalProjectile.whoAmI
			});
			// Player owner = Main.player[projectile.owner];
			// float ownerSummonKB = owner.GetTotalKnockback(DamageClass.Summon).Additive * owner.GetTotalKnockback(DamageClass.Summon).Multiplicative;
			// float ownerSummonKB2 = owner.GetTotalKnockback(DamageClass.Summon).Base; // Vanilla uses .Base instead of the others

			// Oddly, the projectile doesn't update it's knockback dynamically like it does with damage.

			float multiplier = VelocityReductionMultiplier;
			if (optionalProjectile.knockBack != 0)
			{
				multiplier -= (optionalProjectile.knockBack / 50f); // multiplier becomes less with more knockback
			}
			npcHit.velocity *= multiplier;
			npcHit.netUpdate = true;
			// Main.NewText("ownerSummonKB " + ownerSummonKB + " ownerSummonKB2 " + ownerSummonKB2 + " knockback " + knockback + " multiplier " + multiplier);
			// Main.NewText("damage " + damage + " knockback " + knockback + " crit " + crit + " multiplier " + multiplier);
			Dust.NewDust(npcHit.Center, npcHit.width, npcHit.height, DustID.GemAmber, npcHit.direction, npcHit.direction, 150, default, 1f);
		}
	}
	public class ForbiddenWhipGlobalNPC : GlobalNPC
	{
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			// if (markedByForbiddenWhip && npc.active)
			foreach (Player player in Main.ActivePlayers)
			{
				if (npc.active && player.TagEffectStack.IsNPCTagged(ModContent.ItemType<ForbiddenWhip>(), npc.whoAmI))
				{
					//npc.color = Color.Lerp(new Color(240, 156, 64), drawColor, 0.5f);
					drawColor = Color.Lerp(new Color(240, 156, 64), drawColor, 0.5f);
					Dust.NewDust(new(npc.Center.X, npc.Center.Y - npc.height / 2), npc.width / 4, npc.height / 4, DustID.GemAmber, 0, 0, 150, default, 0.5f);
				}
			}
		}
	}
}
