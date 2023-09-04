using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Mounts
{
	public class SwiftSphere : ModMount
	{
		public override void SetStaticDefaults()
		{
			// Movement
			MountData.jumpHeight = 7; // How high the mount can jump.
			MountData.acceleration = 0.4f; // The rate at which the mount speeds up.
			MountData.jumpSpeed = 4f; // The rate at which the player and mount ascend towards (negative y velocity) the jump height when the jump button is presssed.
			MountData.blockExtraJumps = false; // Determines whether or not you can use a double jump (like cloud in a bottle) while in the mount.
			MountData.constantJump = true; // Allows you to hold the jump button down.
			MountData.heightBoost = -22; // Height between the mount and the ground
			MountData.fallDamage = 0.5f; // Fall damage multiplier.
			MountData.runSpeed = 24f; // The speed of the mount
			MountData.dashSpeed = 0f; // The speed the mount moves when in the state of dashing.
			MountData.flightTimeMax = 0; // The amount of time in frames a mount can be in the state of flying.
			MountData.spawnDustNoGravity = true;
			MountData.spawnDust = DustID.GreenTorch;

			// Misc
			MountData.fatigueMax = 0;
			MountData.buff = ModContent.BuffType<Buffs.Mounts.SwiftSphereBuff>(); // The ID number of the buff assigned to the mount.

			// Effects
			MountData.spawnDust = DustID.MagicMirror; // The ID of the dust spawned when mounted or dismounted.

			// Frame data and player offsets
			MountData.totalFrames = 1; // Amount of animation frames for the mount
			MountData.playerYOffsets = Enumerable.Repeat(0, MountData.totalFrames).ToArray(); // Fills an array with values for less repeating code
			MountData.xOffset = 0;
			MountData.yOffset = 0;
			MountData.playerHeadOffset = 0;
			MountData.bodyFrame = 3;
			// Standing
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;
			// Running
			MountData.runningFrameCount = 1;
			MountData.runningFrameDelay = 12;
			MountData.runningFrameStart = 0;
			// Flying
			MountData.flyingFrameCount = 0;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			// In-air
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 12;
			MountData.inAirFrameStart = 0;
			// Idle
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 12;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;
			// Swim
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;

			if (!Main.dedServ)
			{
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

		public override void SetMount(Player player, ref bool skipDust)
		{
			//player.slippy2 = true;
			base.SetMount(player, ref skipDust);
		}

		public override void UpdateEffects(Player player)
		{
			//player.autoJump = player.GetModPlayer<DistortionSpherePlayer>().HasAutoJump;
			//player.portalPhysicsFlag = true;
			player.slippy = true; // Make the player slide on the ground.
			player.velocity.X *= 0.98f; // Apply extra friction
			base.UpdateEffects(player);
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

			if (velocity > 0f)
			{
				Main.EntitySpriteDraw(texture, drawPlayer.shadowPos[0].ToScreenPosition() + magicVector, new Rectangle(0, 0, 24, 24), drawColor * 0.5f, drawPlayer.shadowRotation[0], drawOrigin, drawScale, spriteEffects);
			}

			//Main.NewText("drawPlayer.shadowPos[0] " + (drawPlayer.shadowPos[0].ToScreenPosition() + magicVector));
			//Main.NewText("drawPosition " + drawPosition);

			playerDrawData.Add(new DrawData(texture, drawPosition, new Rectangle(0, 0, 24, 24), drawColor, rotation, drawOrigin, drawScale, spriteEffects));

			return false;
		}

		public override void JumpSpeed(Player mountedPlayer, ref float jumpSeed, float xVelocity)
		{
			jumpSeed += (mountedPlayer.jumpSpeedBoost / 2f);
		}
		public override void JumpHeight(Player mountedPlayer, ref int jumpHeight, float xVelocity)
		{
			jumpHeight += (mountedPlayer.jumpBoost ? 8 : 0);
		}
	}

	public class SwiftSpherePlayer : ModPlayer
	{
		public override void HideDrawLayers(PlayerDrawSet drawInfo)
		{
			// Hiding the head layer causes the head icon to disappear on the map. So, don't hide the layers if the fullscreen map is open. 
			if (Player.HasBuff(ModContent.BuffType<Buffs.Mounts.SwiftSphereBuff>()) && !Main.mapFullscreen)
			{
				drawInfo.hideEntirePlayer = true;
				foreach (var layer in PlayerDrawLayerLoader.Layers)
				{
					if (layer.ToString() == "MountFront")
					{
						continue;
					}
					layer.Hide();
				}
			}
		}

		public override void PreUpdateMovement()
		{
			if (Player.HasBuff(ModContent.BuffType<Buffs.Mounts.SwiftSphereBuff>()))
			{
				// Thing to increase speed while on a slope.
				/*
				int tileX = (int)((Player.position.X + (float)(Player.width / 2)) / 16f);
				int tileY = (int)((Player.position.Y + (float)Player.height) / 16f);

				Tile? floorTile = Player.GetFloorTile(tileX, tileY);
				int slopeDirection = 0;
				if (floorTile != null)
				{
					if (floorTile.Value.LeftSlope)
					{
						slopeDirection = -1;
					}
					if (floorTile.Value.RightSlope)
					{
						slopeDirection = 1;
					}
				}
				Main.NewText(Player.velocity.X);
				if (slopeDirection != 0 && Math.Abs(Player.velocity.X) < 0.1f)
				{
					Main.NewText("+");
					Player.velocity.X += slopeDirection;
				}
				if (slopeDirection != 0 && Math.Sign(Player.velocity.X) == slopeDirection)
				{
					Main.NewText("*");
					Player.velocity.X *= 1.1f;
				}
				*/

				Player.fullRotationOrigin = new Vector2(10, 10);
				float playerVelX = Math.Abs(Player.velocity.X) / 24f; // 24 is just an arbitrary number. I chose it because it's double the mount runSpeed number.
				// So the sphere doesn't still slowly rotate when running into a wall.
				if (playerVelX < 0.02f)
				{
					playerVelX = 0;
				}
				Player.fullRotation += playerVelX * Player.direction;
				//Player.slippy2 = true;

				Player.manaRegenDelay = Player.maxRegenDelay;
			}
		}

		public override void ResetEffects()
		{
			//Player.fullRotationOrigin = default; // Vector2.Zero
			//HasAutoJump = false;
		}

		public override bool CanUseItem(Item item)
		{
			if (Player.HasBuff(ModContent.BuffType<Buffs.Mounts.SwiftSphereBuff>()))
			{
				return false;
			}
			return base.CanUseItem(item);
		}
	}


	public class SwiftSphereItem : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}

		public override string Texture => GetType().Namespace.Replace('.', '/') + "/SwiftSphere";

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item79; // What sound should play when using the item
			Item.noMelee = true; // this item doesn't do any melee damage
			Item.mountType = ModContent.MountType<SwiftSphere>();
			Item.mana = 20;
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