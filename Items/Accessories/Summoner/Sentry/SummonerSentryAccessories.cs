using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Items;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Summoner.Sentry
{
	public class TaggingSpike : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Dropped by Hoplites & Medusas]"]);
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().Accessory_TaggingSpike = true;
		}
	}

	public class TaggingSpikeProjectile : GlobalProjectile
	{
		public override bool PreAI(Projectile projectile)
		{
			/*
			Player owner = Main.player[projectile.owner];
			if (owner.active && owner.GetModPlayer<RijamsModPlayer>().Accessory_MarbleSentryAccessory && (projectile.sentry || ProjectileID.Sets.SentryShot[projectile.type]))
			{
				UniqueTagEffect whipTag = ItemID.Sets.UniqueTagEffects[owner.HeldItem.type];
				if (whipTag != null)
				{
					projectile.tagEffectType = owner.HeldItem.type;
					Main.NewText($"Applied {projectile.Name} {projectile.tagEffectType} {ItemID.Sets.UniqueTagEffects[owner.HeldItem.type]}");
				}
			}
			*/
			return base.PreAI(projectile);
		}

		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			Player owner = Main.player[projectile.owner];
			if (owner.active && owner.GetModPlayer<RijamsModPlayer>().Accessory_TaggingSpike && (projectile.sentry || ProjectileID.Sets.SentryShot[projectile.type]))
			{
				UniqueTagEffect whipTag = ItemID.Sets.UniqueTagEffects[owner.HeldItem.type];
				if (whipTag != null)
				{
					projectile.tagEffectType = owner.HeldItem.type;
					// Main.NewText($"Applied {projectile.Name} {projectile.tagEffectType} {ItemID.Sets.UniqueTagEffects[owner.HeldItem.type]}");
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI);
				}
			}
		}
	}

	[AutoloadEquip(EquipType.HandsOn)]
	public class FoamFinger : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Sold by Party Girl]"]);
			ItemID.Sets.NoMeleeSpeedBonus[Type] = true;
			ItemID.Sets.BonusMeleeSpeedMultiplier[Type] = 0f;
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;

			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;

			Item.shoot = ModContent.ProjectileType<FoamFingerProjectile>(); // The projectile is what makes a shortsword work
			Item.shootSpeed = 0.05f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxTurrets++;
		}
	}

	public class FoamFingerProjectile : ModProjectile
	{
		// Based on shortswords

		public int Timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(14); // This sets width and height to the same value (important when projectiles can rotate)
			Projectile.aiStyle = -1; // Use our own AI to customize how it behaves, if you don't want that, keep this at ProjAIStyleID.ShortSword. You would still need to use the code in SetVisualOffsets() though
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.ownerHitCheck = true; // Prevents hits through tiles. Most melee weapons that use projectiles have this
			Projectile.extraUpdates = 1; // Update 1+extraUpdates times per tick
			Projectile.timeLeft = 60; // This value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
			Projectile.drawLayer = ProjectileDrawLayerID.HeldProjOverHand; // Draws over the player's body and under the player's hands
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (Projectile.timeLeft > 30)
			{
				Timer += 1;
			}
			else
			{
				Timer -= 1;
			}

			player.heldProj = Projectile.whoAmI;

			// Fade in and out
			// GetLerpValue returns a value between 0f and 1f - if clamped is true - representing how far Timer got along the "distance" defined by the first two parameters
			// The first call handles the fade in, the second one the fade out.
			// Notice the second call's parameters are swapped, this means the result will be reverted
			//Projectile.Opacity = Utils.GetLerpValue(0f, FadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(TotalDuration, TotalDuration - FadeOutDuration, Timer, clamped: true);

			Projectile.Opacity = 1f;

			// Keep locked onto the player, but extend further based on the given velocity (Requires ShouldUpdatePosition returning false to work)
			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
			Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);

			// Set spriteDirection based on moving left or right. Left -1, right 1
			Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt();

			// Point towards where it is moving, applied offset for top right of the sprite respecting spriteDirection
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			SetVisualOffsets(); // The code in this method is important to align the sprite with the hitbox how we want it to
		}

		private void SetVisualOffsets()
		{
			// 14 is the sprite size (here both width and height equal)
			int HalfSpriteWidth = 26 / 2;
			int HalfSpriteHeight = 26 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			// Vanilla configuration for "hitbox in middle of sprite"
			DrawOriginOffsetX = 0;
			DrawOffsetX = (int)(-(HalfSpriteWidth - HalfProjWidth));
			DrawOriginOffsetY = (int)(-(HalfSpriteHeight + HalfProjHeight));

			// Vanilla configuration for "hitbox towards the end"
			//if (Projectile.spriteDirection == 1) {
			//	DrawOriginOffsetX = -(HalfProjWidth - HalfSpriteWidth);
			//	DrawOffsetX = (int)-DrawOriginOffsetX * 2;
			//	DrawOriginOffsetY = 0;
			//}
			//else {
			//	DrawOriginOffsetX = (HalfProjWidth - HalfSpriteWidth);
			//	DrawOffsetX = 0;
			//	DrawOriginOffsetY = 0;
			//}
		}

		public override bool ShouldUpdatePosition()
		{
			// Update Projectile.Center manually
			return false;
		}

		// This hook lets us change how the held projectile looks while a mannequin is holding it.
		// The following code is adapted from vanilla's Projectile.AI_DisplayDoll for aiStyle 161 (Shortsword)
		public override bool DisplayDollSettings(Player doll, TEDisplayDoll.DisplayDollPose pose, ref int aiStyle, ref int aiType)
		{
			SetVisualOffsets(); // The code in this method is important to align the sprite with the hitbox how we want it to

			Projectile.spriteDirection = Projectile.direction; // Set the direction of the sprite to the direction the projectile is facing.
			Vector2 projectileRotation = Vector2.UnitX * 6f; // This will move the projectile forward in the mannequin's hand because the hitbox of the projectile is the center of the sprite.
			float armRotation = 0f;
			if (pose.ItemAimRadians.HasValue)
				armRotation = pose.ItemAimRadians.Value; // The rotation of the mannequin's hand.

			projectileRotation = projectileRotation.RotatedBy(armRotation); // Rotate the projectile based on the rotation of the mannequin's hand.
			if (Projectile.direction == -1)
				projectileRotation.X *= -1f;

			Projectile.velocity = projectileRotation;
			Projectile.position += projectileRotation; // Move the projectile's location while being held by the mannequin.

			Projectile.rotation = (float)Math.Atan2(projectileRotation.Y, projectileRotation.X) + MathHelper.PiOver2; // Set the projectile's rotation based on the mannequin's hand.

			return false;
		}
	}
}
