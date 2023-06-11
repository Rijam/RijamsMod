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
			//IL_Player.PlayerFrame += LanternEdit; // No longer needed. Fix implemented into tModLoader directly. PR #3530
		}

		public override void Unload()
		{
			// tModLoader SHOULD automatically unload IL edits, but I'm putting this here just in case.
			//IL_Player.PlayerFrame -= LanternEdit;
		}

		/*
		/// <summary>
		/// <br>This IL Edit is used to correct the front arm animation when using items with ItemUseStyleID.RaiseLamp.</br>
		/// <br>See also <seealso cref="GlobalItems.fixItemUseStyleIDRaiseLampFrontArmAnimation"/></br>
		/// </summary>
		/// <param name="il"></param>
		private static void LanternEdit(ILContext il) // No longer needed. Fix implemented into tModLoader directly. PR #3530
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
		*/
	}
}