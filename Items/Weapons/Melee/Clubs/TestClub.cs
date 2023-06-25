using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Projectiles.Melee.Clubs;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace RijamsMod.Items.Weapons.Melee.Clubs
{
	public class TestClub : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Test Club");
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Melee;
			Item.width = 18;
			Item.height = 18;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 8;
			Item.value = Item.buyPrice(gold: 2);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = new(Mod.Name + "/Sounds/Item/ClubSwing") { Volume = 1f, PitchVariance = 0.05f, MaxInstances = 10 };
			Item.autoReuse = true;
			Item.useTurn = false;
			Item.crit = 10;
			Item.useStyle = -1;
			Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
			Item.noMelee = true; // The projectile will do the damage and not the item

			Item.shoot = ModContent.ProjectileType<TestClubSwingProj>(); // The projectile is what makes a shortsword work
			Item.shootSpeed = 1f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
		}

		private float shootRotation = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			shootRotation = velocity.ToRotation();
			Projectile projectile = Projectile.NewProjectileDirect(source, player.RotatedRelativePoint(player.MountedCenter), velocity, type, damage, knockback, player.whoAmI, 0, 0);
			// Extra logic for the chain to adjust to item stats, unlike the Solar Eruption.
			if (projectile.ModProjectile is TestClubSwingProj modProjectile)
			{
				modProjectile.useTurn = Item.useTurn;
				modProjectile.firingAnimation = (int)Math.Round(Item.useAnimation * 2f / player.GetTotalAttackSpeed(DamageClass.Melee));
				modProjectile.firingTime = (int)Math.Round(Item.useTime * 2f / player.GetTotalAttackSpeed(DamageClass.Melee));
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI);
			}
			return false;
		}

		public override bool MeleePrefix() => true;

		public override void UseItemFrame(Player player)
		{
			// Adapted from useStyle 9 (DrinkLiquid)
			// The itemLocation stuff doesn't really apply here because the item is hidden (the projectile is the thing that shows)

			// Neutral stance
			player.bodyFrame.Y = 0;

			float newShootRotation = shootRotation;

			float rotateMin = -45f;
			float rotateMax = 45f;

			// Right is 0
			// Top is -Pi/2			-90
			// Left is -Pi or Pi	-180 or 180
			// Bottom is Pi/2		90

			if (player.direction < 0) // Shot left
			{
				if (shootRotation < 0) // Up and left
				{
					newShootRotation = Math.Clamp(newShootRotation, MathHelper.ToRadians(-180), MathHelper.ToRadians(-180 + rotateMax)); //-180 to -150
					newShootRotation += MathHelper.ToRadians(180);
				}
				else // Down and left
				{
					newShootRotation = Math.Clamp(newShootRotation, MathHelper.ToRadians(180 + rotateMin), MathHelper.ToRadians(180)); // 150 to 180
					newShootRotation += MathHelper.ToRadians(-180);
				}
			}
			else
			{
				newShootRotation = Math.Clamp(newShootRotation, MathHelper.ToRadians(rotateMin), MathHelper.ToRadians(rotateMax));
			}
			newShootRotation *= player.direction;

			float currentAnimationTime = (player.itemAnimation * 2f) / (float)(player.itemAnimationMax * 2f);

			float time = 1f - currentAnimationTime;
			time = Utils.GetLerpValue(0f, 1.0f, time, clamped: true);
			player.itemRotation = time * -player.direction * 2f + 0.7f * player.direction;
			player.itemLocation = player.MountedCenter + new Vector2(player.direction * 10 * currentAnimationTime, 0f);
			float rotationAmount;
			if (time <= 0.5f)
			{
				rotationAmount = (1f - time) * 5f;
			}
			else
			{
				rotationAmount = time * 5f;
			}
			float rotationDirection = rotationAmount + (float)Math.PI * 2f / 5f + newShootRotation;
			rotationDirection *= player.direction;

			player.SetCompositeArmFront(enabled: true, CompositeArmStretchAmount.Full, rotationDirection);

			
			player.itemLocation = player.GetFrontHandPosition(CompositeArmStretchAmount.Full, rotationDirection);
			// player.itemLocation -= player.MountedCenter; // Part of the original useStyle
			player.itemLocation *= MathHelper.Lerp(1.5f, 1.2f, time);
			// player.itemLocation += player.MountedCenter; // Part of the original useStyle
			player.itemLocation.X += player.direction * MathHelper.Lerp(8f, 2f, time);
			if (player.gravDir == -1f)
			{
				player.itemRotation *= player.gravDir;
				player.itemLocation.Y = player.position.Y + player.height + (player.position.Y - player.itemLocation.Y);
			}
		}
	}
}