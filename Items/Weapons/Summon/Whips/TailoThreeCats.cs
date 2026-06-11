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
	public class TailoThreeCats : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Dropped by Deerclops]"]);
			CustomItemIDSets.IsWhip[Type] = true;
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect_TailoThreeCats() { TagDamage = 5 };
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

	public class WhipTagEffect_TailoThreeCats : WhipTagEffect
	{
		public override void OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, float calcDamage)
		{
			//base.OnProcHit(owner, optionalProjectile, npcHit, calcDamage);
			npcHit.AddBuff(BuffID.Confused, 420);
			CombatText.NewText(optionalProjectile.Hitbox, Color.Magenta, "❓");
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.DeadCellsMushroomBoiTargetFound, new ParticleOrchestraSettings
			{
				UniqueInfoPiece = optionalProjectile.whoAmI
			});
		}

		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, float calcDamage)
		{
			//npcHit.AddBuff(BuffID.Confused, 240);
			//CombatText.NewText(optionalProjectile.Hitbox, Color.Purple, "BANG!");
		}
	}
}
