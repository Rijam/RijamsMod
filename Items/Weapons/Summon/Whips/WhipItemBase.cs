using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class WhipItemBase : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(WhipItemBase);
		public override string Texture => Type == ModContent.ItemType<WhipItemBase>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');

		public override void SetStaticDefaults()
		{
			CustomItemIDSets.IsWhip[Type] = true;
		}

		/// <summary>
		/// Allows to override the swing direction float.
		/// </summary>
		/// <returns>Standard swing direction by default.</returns>
		public virtual float SwingDirection()
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
			return swingDirection;
		}

		/// <summary>
		/// Allows to change the damage and knockback penalty if the whip is a secondary whip.
		/// </summary>
		public virtual void SecondaryWhipMultipliers(ref float damageMultiplier, ref float knockbackMultiplier) { }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float swingDirection = SwingDirection();

			// If the Snake Band spawns this whip, no item time is applied.
			// However, the whip projectile AI applies the life time from the item time.
			// So, spawn the secondary whip with a value set in Projectile.ai[2] with its life time.
			float secondaryWhipTime = 0f;

			if (/*Item.IsPerformingFreeUse*/ false)
			{
				// Set the velocity of the secondary whip to be a little different from the primary whip.
				velocity = velocity.RotatedByRandom(MathHelper.Pi / 10f);

				// Secondary whips are set to double this item's animation time.
				// The 1000f is important for designating that the whip is a secondary whip and is corrected in Projectile.GetWhipSettings().
				secondaryWhipTime = 1000f + player.itemAnimationMax * 2f;

				// Reduce the damage and knockback of the secondary whip.
				float secondaryWhipDamageMultiplier = 0.3f; // -70% damage
				float secondaryWhipKnockbackMultiplier = 0.25f; // -75% knockback
				SecondaryWhipMultipliers(ref secondaryWhipDamageMultiplier, ref secondaryWhipKnockbackMultiplier);
				damage = (int)MathHelper.Max(damage * secondaryWhipDamageMultiplier, 3); // Make the secondary whip deal at least 3 damage.
				knockback *= secondaryWhipKnockbackMultiplier;
			}

			// Spawn the whip projectile.
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai0: 0f, ai1: swingDirection, ai2: secondaryWhipTime);

			if (/*!Item.IsPerformingFreeUse*/ true) // If the whip is a primary whip (not spawned by the Snake Band), keep track of how many swings we've done.
			{
				player.repeatWhipSwings++;
				player.repeatWhipsResetCooldown = 120;
			}

			return false; // Return false because we've already spawned the projectile.
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			/*
			if (player.accWickedArmlet)
			{
				knockback += 1;
			}
			*/
		}

		public override bool MeleePrefix() => true;

		/*
		public override void Load()
		{
			Terraria.On_Player.ItemCheck_Shoot += On_Player_ItemCheck_Shoot;
		}

		private void On_Player_ItemCheck_Shoot(On_Player.orig_ItemCheck_Shoot orig, Player self, int i, Item sItem, int weaponDamage, bool withAudioVisualFeedback)
		{
			orig(self, i, sItem, weaponDamage, withAudioVisualFeedback);

			if (sItem.ModItem is not WhipItemBase)
			{
				return;
			}

			IEntitySource source = self.GetSource_ItemUse_WithPotentialAmmo(sItem, 0);
			Vector2 position = self.RotatedRelativePoint(self.MountedCenter);
			Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition - position;
			float speed = sItem.shootSpeed;
			float mouseDistance = (float)Math.Sqrt(Main.mouseX * Main.mouseX + Main.mouseY * Main.mouseY);
			if ((float.IsNaN(mouse.X) && float.IsNaN(mouse.Y)) || (mouse.X == 0f && mouse.Y == 0f))
			{
				mouse.X = self.direction;
				mouse.Y = 0f;
				mouseDistance = speed;
			}
			else
			{
				mouseDistance = speed / mouseDistance;
			}
			Vector2 velocity = mouse * mouseDistance;
			int type = sItem.shoot;
			int damage = weaponDamage;
			float knockback = sItem.knockBack;

			float swingDirection = SwingDirection();

			// If the Snake Band spawns this whip, no item time is applied.
			// However, the whip projectile AI applies the life time from the item time.
			// So, spawn the secondary whip with a value set in Projectile.ai[2] with its life time.
			float secondaryWhipTime = 0f;

			Main.NewText($"Item: wAVF {withAudioVisualFeedback} {sItem.Name}");

			if (!withAudioVisualFeedback)
			{
				// Set the velocity of the secondary whip.
				Vector2 mousePos = new(Main.mouseX, Main.mouseY);
				Vector2 secondaryWhipVelocity = mousePos + Main.screenPosition - self.RotatedRelativePoint(self.MountedCenter);
				Vector2 secondaryWhipVelocityWithRandomness = secondaryWhipVelocity.RotatedByRandom(MathHelper.Pi / 10f);
				velocity = secondaryWhipVelocityWithRandomness;

				// Secondary whips are set to double this item's animation time.
				// The 1000f is important for designating that the whip is a secondary whip and is corrected in Projectile.GetWhipSettings().
				secondaryWhipTime = 1000f + self.itemAnimationMax * 2f;

				// Reduce the damage and knockback of the secondary whip.
				float secondaryWhipDamageMultiplier = 0.3f; // -70% damage
				float secondaryWhipKnockbackMultiplier = 0.25f; // -75% knockback
				SecondaryWhipMultipliers(ref secondaryWhipDamageMultiplier, ref secondaryWhipKnockbackMultiplier);
				damage = (int)MathHelper.Max(damage * secondaryWhipDamageMultiplier, 3); // Make the secondary whip deal at least 3 damage.
				knockback *= secondaryWhipKnockbackMultiplier;
			}

			Main.NewText($"Item: {secondaryWhipTime} {swingDirection} {sItem.Name}");

			// Spawn the whip projectile.
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, self.whoAmI, ai0: 0f, ai1: swingDirection, ai2: secondaryWhipTime);

			if (withAudioVisualFeedback) // If the whip is a primary whip (not spawned by the Snake Band), keep track of how many swings we've done.
			{
				self.repeatWhipSwings++;
				self.repeatWhipsResetCooldown = 120;
			}
		}
		*/
	}
}
