using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;

namespace RijamsMod.Items
{
	public class ILItems : GlobalItem
	{
		public override void Load()
		{
			// Load the IL Patch
			IL_Player.PlayerFrame += LanternEdit;
		}

		/// <summary>
		/// <br>This IL Edit is used to correct the front arm animation when using items with ItemUseStyleID.RaiseLamp.</br>
		/// <br>See also <seealso cref="GlobalItems.fixItemUseStyleIDRaiseLampFrontArmAnimation"/></br>
		/// </summary>
		/// <param name="il"></param>
		private static void LanternEdit(ILContext il)
		{
			ILCursor c = new(il);

			// Try to find where 4952 is placed onto the stack
			// 4952 is the item ID of the Nightglow
			if (!c.TryGotoNext(i => i.MatchLdcI4(4952)))
			{
				RijamsMod.Instance.Logger.WarnFormat("LanternEdit IL Patch could not be applied! c = {0}", c);
				return; // Patch unable to be applied
			}

			// Move the cursor after 4952 and onto the ret op.
			c.Index++;
			// Push the Player instance onto the stack
			c.Emit(OpCodes.Ldarg_0);
			// Call a delegate using the int and Player from the stack.
			c.EmitDelegate<Func<int, Player, int>>((returnValue, player) => {
				// Regular c# code

				// If I understand this correctly...

				// Here I am editing the value of 4952 (I think)
				// The original code is: `bool flag4 = HeldItem.type != 4952;`
				// If the HeldItem is a lanternWeapon, then change number 4952 to the HeldItem.type
				// This will make it so it reads something like `bool flag4 = HeldItem.type != HeldItem.type;`.

				// I want the bool to be false if it is a lantern weapon because later in Player.PlayerFrame() is
				// `else if (itemAnimation > 0 && inventory[selectedItem].useStyle != 10 && flag4)`
				// Which does not run if the item is the Nightglow.

				if (GlobalItems.fixItemUseStyleIDRaiseLampFrontArmAnimation.Contains(player.HeldItem.type))
				{
					return player.HeldItem.type;
				}

				return returnValue;
			});
		}

		/* Not needed because I made the IL edit that does this for all lantern weapons.
		 * This is the code that is supposed to run to animate the front arm.
		public override void UseItemFrame(Player player)
		{
			if (player.pulley)
			{
				if (player.pulleyDir == 2)
					player.bodyFrame.Y = player.bodyFrame.Height;
				else
					player.bodyFrame.Y = player.bodyFrame.Height * 2;
			}
			//else if (player.shieldRaised)
			//{
			//	player.bodyFrame.Y = player.bodyFrame.Height * 10;
			//}
			else if (player.mount.Active)
			{
				player.bodyFrameCounter = 0.0;
				player.bodyFrame.Y = player.bodyFrame.Height * player.mount.BodyFrame;
			}
			else if (player.grappling[0] >= 0)
			{
				player.sandStorm = false;
				//player.CancelAllJumpVisualEffects();
				Vector2 vector = new(player.position.X + (float)player.width * 0.5f, player.position.Y + (float)player.height * 0.5f);
				float num21 = 0f;
				float num22 = 0f;
				for (int m = 0; m < player.grapCount; m++)
				{
					num21 += Main.projectile[player.grappling[m]].position.X + (float)(Main.projectile[player.grappling[m]].width / 2);
					num22 += Main.projectile[player.grappling[m]].position.Y + (float)(Main.projectile[player.grappling[m]].height / 2);
				}

				num21 /= (float)player.grapCount;
				num22 /= (float)player.grapCount;
				num21 -= vector.X;
				num22 -= vector.Y;
				if (num22 < 0f && Math.Abs(num22) > Math.Abs(num21))
				{
					player.bodyFrame.Y = player.bodyFrame.Height * 2;
					if (player.gravDir == -1f)
						player.bodyFrame.Y = player.bodyFrame.Height * 4;
				}
				else if (num22 > 0f && Math.Abs(num22) > Math.Abs(num21))
				{
					player.bodyFrame.Y = player.bodyFrame.Height * 4;
					if (player.gravDir == -1f)
						player.bodyFrame.Y = player.bodyFrame.Height * 2;
				}
				else
				{
					player.bodyFrame.Y = player.bodyFrame.Height * 3;
				}
			}
			else if (player.wet && player.ShouldFloatInWater)
			{
				player.bodyFrame.Y = player.bodyFrame.Height * 10;
			}
			else if (player.swimTime > 0)
			{
				if (player.swimTime > 20)
				{
					player.bodyFrame.Y = 0;
				}
				else if (player.swimTime > 10)
				{
					player.bodyFrame.Y = player.bodyFrame.Height * 5;
				}
				else
				{
					player.bodyFrame.Y = 0;
				}
			}
			else if (player.velocity.Y != 0f)
			{
				if (player.sliding)
				{
					player.bodyFrame.Y = player.bodyFrame.Height * 3;
				}
				else if (player.sandStorm || player.carpetFrame >= 0)
				{
					player.bodyFrame.Y = player.bodyFrame.Height * 6;
				}
				else if (player.eocDash > 0)
				{
					player.bodyFrame.Y = player.bodyFrame.Height * 6;
				}
				else if (player.wings > 0)
				{
					if (player.wings == 22 || player.wings == 28 || player.wings == 45)
					{
						player.bodyFrame.Y = 0;
					}
					else if (player.velocity.Y > 0f)
					{
						if (player.controlJump)
							player.bodyFrame.Y = player.bodyFrame.Height * 6;
						else
							player.bodyFrame.Y = player.bodyFrame.Height * 5;
					}
					else
					{
						player.bodyFrame.Y = player.bodyFrame.Height * 6;
					}
				}
				else
				{
					player.bodyFrame.Y = player.bodyFrame.Height * 5;
				}

				player.bodyFrameCounter = 0.0;
			}
			else if (player.velocity.X != 0f)
			{
				//if (player.legs == 140)
				//{
				//	player.bodyFrameCounter += Math.Abs(player.velocity.X) * 0.5f;
				//	while (player.bodyFrameCounter > 8.0)
				//	{
				//		player.bodyFrameCounter -= 8.0;
				//		player.bodyFrame.Y += player.bodyFrame.Height;
				//	}

				//	if (player.bodyFrame.Y < player.bodyFrame.Height * 7)
				//		player.bodyFrame.Y = player.bodyFrame.Height * 19;
				//	else if (player.bodyFrame.Y > player.bodyFrame.Height * 19)
				//		player.bodyFrame.Y = player.bodyFrame.Height * 7;
				//}
				//else
				//{
					player.bodyFrameCounter += (double)Math.Abs(player.velocity.X) * 1.5;
					player.bodyFrame.Y = player.legFrame.Y;
				//}
			}
			else
			{
				player.bodyFrameCounter = 0.0;
				player.bodyFrame.Y = 0;
			}
		}
		*/
	}
}