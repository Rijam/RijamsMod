using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using RijamsMod.Projectiles.Summon.Whips;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class ForbiddenWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Forbidden Whip");
			// Tooltip.SetDefault("Reduces enemy velocity by 20% when struck by a minion\n  Scales with Summon Knockback\n{$CommonItemTooltip.Whips}");
			CustomItemIDSets.IsWhip[Type] = true;
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect_ForbiddenWhip() { VelocityReductionPercent = 20, TagDuration = 180 };
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<ForbiddenWhipProj>(), 50, 6f, 6f, 40);

			Item.width = 42;
			Item.height = 36;
			Item.rare = ItemRarityID.Pink;
			Item.value = 60000;
			Item.channel = false;
			Item.UseSound = SoundID.Item152 with { Pitch = -0.2f };
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
				.AddRecipeGroup(RecipeGroups.MythrilBar, 5)
				.AddIngredient(ItemID.AncientCloth, 5)
				.AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override bool MeleePrefix() => true;
	}

	public class WhipTagEffect_ForbiddenWhip : WhipTagEffect
	{
		public int VelocityReductionPercent;
		public float VelocityReductionMultiplier => 1f - (VelocityReductionPercent / 100f);

		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, float calcDamage)
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
				if (npc.active && player.TagEffectState.Type == ModContent.ItemType<ForbiddenWhip>() && player.TagEffectState.IsNPCTagged(npc.whoAmI))
				{
					//npc.color = Color.Lerp(new Color(240, 156, 64), drawColor, 0.5f);
					drawColor = Color.Lerp(new Color(240, 156, 64), drawColor, 0.5f);
					Dust.NewDust(new(npc.Center.X, npc.Center.Y - npc.height / 2), npc.width / 4, npc.height / 4, DustID.GemAmber, 0, 0, 150, default, 0.5f);
				}
			}
		}
	}
}
