using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Mounts
{
	public class DistortionSphere : ModMount
	{
		public override void SetStaticDefaults()
		{
			MountID.Sets.CanDash[Type] = true; // This mount can dash with normal dashes.
			MountID.Sets.IsTransformationMount[Type] = true;
			MountID.Sets.PlayerIsHidden[Type] = true;
			MountID.Sets.DontHoldItems[Type] = true;
			MountID.Sets.DontDismountWhenCCed[Type] = true; // Being "crowd controlled" (frozen, stoned, webbed, etc.) does not dismount the player.
			MountData.dismountsOnItemUse = true; // The player will be dismounted if they use items.

			// Movement
			MountData.jumpHeight = 7; // How high the mount can jump.
			MountData.jumpSpeed = 4f; // The rate at which the player and mount ascend towards (negative y velocity) the jump height when the jump button is pressed.
			MountData.blockExtraJumps = false; // Determines whether or not you can use a double jump (like cloud in a bottle) while in the mount.
			MountData.constantJump = false; // Allows you to hold the jump button down.
			MountData.heightBoost = -22; // Height between the mount and the ground
			MountData.fallDamage = 0.5f; // Fall damage multiplier.
			MountData.dashSpeed = 1f; // The speed the mount moves when in the state of dashing.
			MountData.flightTimeMax = 0; // The amount of time in frames a mount can be in the state of flying.

#pragma warning disable IDE0150 // Prefer 'null' check over type check
			if (this is DistortionSphere) // For some reason this was overriding the SwiftSphere's speed.
			{
				MountData.acceleration = 0.2f; // The rate at which the mount speeds up.
				MountData.runSpeed = 9f; // The speed of the mount
			}
#pragma warning restore IDE0150 // Prefer 'null' check over type check

			// Misc
			MountData.fatigueMax = 0;
			MountData.buff = ModContent.BuffType<Buffs.Mounts.DistortionSphereBuff>(); // The ID number of the buff assigned to the mount.

			// Effects
			MountData.spawnDust = DustID.MagicMirror; // The ID of the dust spawned when mounted or dismounted.

			// Frame data and player offsets
			MountData.totalFrames = 1; // Amount of animation frames for the mount
			MountData.playerYOffsets = Enumerable.Repeat(0, MountData.totalFrames).ToArray(); // Fills an array with values for less repeating code
			MountData.xOffset = 0;
			MountData.yOffset = 0;
			MountData.playerHeadOffset = -16;
			MountData.bodyFrame = 3;
			// Standing
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 1;
			MountData.standingFrameStart = 0;
			// Running
			MountData.runningFrameCount = MountData.standingFrameCount;
			MountData.runningFrameDelay = MountData.standingFrameDelay;
			MountData.runningFrameStart = MountData.standingFrameStart;
			// Flying
			MountData.flyingFrameCount = MountData.standingFrameCount;
			MountData.flyingFrameDelay = MountData.standingFrameDelay;
			MountData.flyingFrameStart = MountData.standingFrameStart;
			// In-air
			MountData.inAirFrameCount = MountData.standingFrameCount;
			MountData.inAirFrameDelay = MountData.standingFrameDelay;
			MountData.inAirFrameStart = MountData.standingFrameStart;
			// Idle
			MountData.idleFrameCount = MountData.standingFrameCount;
			MountData.idleFrameDelay = MountData.standingFrameDelay;
			MountData.idleFrameStart = MountData.standingFrameStart;
			// Swim
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;

			if (!Main.dedServ)
			{
				MountData.textureWidth = MountData.frontTexture.Width();
				MountData.textureHeight = MountData.frontTexture.Height();
			}
		}

		public override void SetMount(Player player, ref bool skipDust)
		{
			// player.slippy2 = true;
			base.SetMount(player, ref skipDust);
		}

		public override void ModifyPlayerDrawInfo(ref PlayerDrawSet drawInfo)
		{
			drawInfo.hideEntirePlayer = true;
		}

		public override void UpdateEffects(Player player)
		{
			//player.portalPhysicsFlag = true;
			// player.slippy = true; // Make the player slide on the ground.
			// player.velocity.X *= 0.98f; // Apply extra friction

			// Prevent the transformed player from holding items
			player.IsAllowedToHoldItems = false;
			player.noItems = true;

			player.fullRotationOrigin = new Vector2(10, 10);
			float playerVelX = Math.Abs(player.velocity.X) / 18f; // 18 is just an arbitrary number. I chose it because it's double the mount runSpeed number.
			
			// So the sphere doesn't still slowly rotate when running into a wall.
			float minSpeed = player.mount.Type == ModContent.MountType<SwiftSphere>() ? 0.02f : 0.01f;
			if (playerVelX < minSpeed)
			{
				playerVelX = 0;
			}
			player.fullRotation += playerVelX * player.direction;
		}

		public override void JumpSpeed(Player mountedPlayer, ref float jumpSeed, float xVelocity)
		{
			jumpSeed += (mountedPlayer.jumpSpeedBoost / 2f);
		}
		public override void JumpHeight(Player mountedPlayer, ref int jumpHeight, float xVelocity)
		{
			jumpHeight += (mountedPlayer.jumpBoost ? 8 : 0);
		}

		public override void UpdateAfterEquips(Player player)
		{
			if (player.autoJump)
			{
				MountData.constantJump = true;
			}
			else
			{
				MountData.constantJump = false;
			}

			if (player.iceSkate)
			{
				player.slippy = false; // Make the player slide on the ground.
				if (!player.controlLeft && !player.controlRight && player.velocity.Y == 0)
				{
					player.velocity.X *= 0.95f; // Apply extra friction if the player is on the ground and not pressing left or right.
				}
			}
			else
			{
				player.slippy = true; // Make the player slide on the ground.
			}
		}

		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
		{
			spriteEffects = SpriteEffects.None; // So the sprite doesn't flip when facing left.

			float velocity = Math.Abs(drawPlayer.velocity.Length());

			Vector2 magicVector = new(10 * Main.GameZoomTarget, 11 - Main.GameZoomTarget);

			if (velocity > 8f)
			{
				Main.EntitySpriteDraw(texture, drawPlayer.shadowPos[2].ToScreenPosition() + magicVector, new Rectangle(0, 0, 24, 24), drawColor * 0.125f, drawPlayer.shadowRotation[2], drawOrigin, drawScale, spriteEffects);
			}

			if (velocity > 4f)
			{
				Main.EntitySpriteDraw(texture, drawPlayer.shadowPos[1].ToScreenPosition() + magicVector, new Rectangle(0, 0, 24, 24), drawColor * 0.25f, drawPlayer.shadowRotation[1], drawOrigin, drawScale, spriteEffects);
			}

			if (velocity > 2f)
			{
				Main.EntitySpriteDraw(texture, drawPlayer.shadowPos[0].ToScreenPosition() + magicVector, new Rectangle(0, 0, 24, 24), drawColor * 0.5f, drawPlayer.shadowRotation[0], drawOrigin, drawScale, spriteEffects);
			}

			//Main.NewText("drawPlayer.shadowPos[0] " + (drawPlayer.shadowPos[0].ToScreenPosition() + magicVector));
			//Main.NewText("drawPosition " + drawPosition);

			playerDrawData.Add(new DrawData(texture, drawPosition, new Rectangle(0, 0, 24, 24), drawColor, rotation, drawOrigin, drawScale, spriteEffects));

			return false;
		}
	}

	public class DistortionSphereItem : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}

		public override string Texture => GetType().Namespace.Replace('.', '/') + "/DistortionSphere";

		public override void SetDefaults()
		{
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item79; // What sound should play when using the item
			Item.noMelee = true; // this item doesn't do any melee damage
			Item.mountType = ModContent.MountType<DistortionSphere>();
		}

		public override bool? UseItem(Player player)
		{
			return base.UseItem(player);
		}

		public override void AddRecipes()
		{

		}
	}
}
