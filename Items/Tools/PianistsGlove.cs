using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ReLogic.Utilities;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace RijamsMod.Items.Tools
{
	public class PianistsGlove : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = false;
			ItemID.Sets.ShimmerCountsAsItem[Type] = ModContent.ItemType<PianistsGlove>();
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Blue;
			Item.consumable = false;
		}
		public override bool AltFunctionUse(Player player) => true;

		public override bool? UseItem(Player player)
		{
			UseItemInner(player, 2, ModContent.ItemType<PianistsGloveHigh>());
			return null;
		}
		public override void HoldItem(Player player)
		{
			HoldItemInner(player, 2);
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void RightClick(Player player)
		{
			RickClickInner(player, ModContent.ItemType<PianistsGloveHigh>());
		}

		public override void Load()
		{
			if (!Main.dedServ)
			{
				EquipLoader.AddEquipTexture(Mod, (GetType().Namespace).Replace('.', '/') + "/PianistsGlove_HandsOn", EquipType.HandsOn, this, "PianistsGloveHandsOn");
				EquipLoader.AddEquipTexture(Mod, (GetType().Namespace).Replace('.', '/') + "/PianistsGlove_HandsOff", EquipType.HandsOff, this, "PianistsGloveHandsOff");
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Silk, 10)
				.AddIngredient(ItemID.BlackThread, 5)
				.AddTile(TileID.Loom)
				.Register();
		}

		public void HoldItemInner(Player player, int octave)
		{
			if (!Main.dedServ)
			{
				player.handon = EquipLoader.GetEquipTexture(Mod, "PianistsGloveHandsOn", EquipType.HandsOn).Slot;
				player.handoff = EquipLoader.GetEquipTexture(Mod, "PianistsGloveHandsOff", EquipType.HandsOff).Slot;
			}
			int note = CalcNote(player, out float _);
			if (note > 0)
			{
				CursorNotes(note, player, octave);
			}
		}

		public bool? UseItemInner(Player player, int octave, int itemToTransformTo)
		{
			int note = CalcNote(player, out	float range);
			if (Main.mouseLeft && Main.mouseLeftRelease && note > 0)
			{
				PlayPiano(note, player, octave);
				// Main.musicPitch = range;
				// NetMessage.SendData(MessageID.InstrumentSound, -1, -1, null, player.whoAmI, range);

				return false;
			}
			if (player.controlUseTile && Main.myPlayer == player.whoAmI && !player.tileInteractionHappened && player.releaseUseItem && !player.controlUseItem && !player.mouseInterface && !CaptureManager.Instance.Active && !Main.HoveringOverAnNPC && !Main.SmartInteractShowingGenuine)
			{
				if (player.altFunctionUse == 2 && player.itemAnimation == player.itemAnimationMax - 1)
				{
					player.releaseUseTile = false;
					Main.mouseRightRelease = false;
					SoundEngine.PlaySound(SoundID.Unlock with { Volume = (ModContent.GetInstance<RijamsModConfigClient>().BurglarsRingSound / 100f) });
					player.inventory[player.selectedItem].ChangeItemType(itemToTransformTo);
					Recipe.FindRecipes();
					
					return true;
				}
			}
			return null;
		}

		public void RickClickInner(Player player, int itemToTransformTo)
		{
			// Sort of works as long as there aren't any other Pianist's Gloves in their inventory.
			SoundEngine.PlaySound(SoundID.Unlock with { Volume = (ModContent.GetInstance<RijamsModConfigClient>().BurglarsRingSound / 100f) });
			int index = player.FindItemInInventoryOrOpenVoidBag(Type, out _);
			player.inventory[index].ChangeItemType(itemToTransformTo);
			Recipe.FindRecipes();
		}

		public override bool ConsumeItem(Player player)
		{
			return false;
		}

		public static int CalcNote(Player player, out float range)
		{
			range = 0;
			float OneSixth = 1f / 6f;
			int playerPosX = (int)player.Center.X / 16;
			int playerPosY = (int)player.Center.Y / 16;
			Tile tile = Main.tile[playerPosX, playerPosY];
			ModTile modTile = TileLoader.GetTile(tile.TileType);
			if (WorldGen.InWorld(playerPosX, playerPosY) && tile != null && (Tiles.GlobalTiles.isPiano.Contains(tile.TileType) || (modTile != null && modTile.AdjTiles.Contains(TileID.Pianos))))
			{
				Vector2 vector6 = new(player.position.X + (float)player.width * 0.5f, player.position.Y + (float)player.height * 0.5f);
				float mousePosX = (float)Main.mouseX + Main.screenPosition.X - vector6.X;
				float mousePosY = (float)Main.mouseY + Main.screenPosition.Y - vector6.Y;
				float pitch = (float)Math.Sqrt(mousePosX * mousePosX + mousePosY * mousePosY) * 0.675f; // 48 tile reach... maybe
				float adjustedScreenHeight = (float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y;
				pitch /= adjustedScreenHeight / 2f;
				if (pitch > 1f)
				{
					pitch = 1f;
				}

				pitch = pitch * 4f - 2f;
				pitch = Math.Clamp(pitch, -2f, 2f);

				range = pitch;

				if (pitch <= OneSixth * -11f)
				{
					return 1; // C low
				}
				else if (pitch > OneSixth * -11f && pitch <= OneSixth * -10f)
				{
					return 2; // C#
				}
				else if (pitch > OneSixth * -10f && pitch <= OneSixth * -9f)
				{
					return 3; // D
				}
				else if (pitch > OneSixth * -9f && pitch <= OneSixth * -8f)
				{
					return 4; // D#
				}
				else if (pitch > OneSixth * -8f && pitch <= OneSixth * -7f)
				{
					return 5; // E
				}
				else if (pitch > OneSixth * -7f && pitch <= OneSixth * -6f)
				{
					return 6; // F
				}
				else if (pitch > OneSixth * -6f && pitch <= OneSixth * -5f)
				{
					return 7; // F#
				}
				else if (pitch > OneSixth * -5f && pitch <= OneSixth * -4f)
				{
					return 8; // G
				}
				else if (pitch > OneSixth * -4f && pitch <= OneSixth * -3f)
				{
					return 9; // G#
				}
				else if (pitch > OneSixth * -3f && pitch <= OneSixth * -2f)
				{
					return 10; // A
				}
				else if (pitch > OneSixth * -2f && pitch <= OneSixth * -1f)
				{
					return 11; // A#
				}
				else if (pitch > OneSixth * -1f && pitch < 0f)
				{
					return 12; // B
				}
				else if (pitch >= 0f && pitch < OneSixth)
				{
					return 13; // C middle
				}
				else if (pitch >= OneSixth && pitch < OneSixth * 2f)
				{
					return 14; // C#
				}
				else if (pitch >= OneSixth * 2f && pitch < OneSixth * 3f)
				{
					return 15; // D
				}
				else if (pitch >= OneSixth * 3f && pitch < OneSixth * 4f)
				{
					return 16; // D#
				}
				else if (pitch >= OneSixth * 4f && pitch < OneSixth * 5f)
				{
					return 17; // E
				}
				else if (pitch >= OneSixth * 5f && pitch < OneSixth * 6f)
				{
					return 18; // F
				}
				else if (pitch >= OneSixth * 6f && pitch < OneSixth * 7f)
				{
					return 19; // F#
				}
				else if (pitch >= OneSixth * 7f && pitch < OneSixth * 8f)
				{
					return 20; // G
				}
				else if (pitch >= OneSixth * 8f && pitch < OneSixth * 9f)
				{
					return 21; // G#
				}
				else if (pitch >= OneSixth * 9f && pitch < OneSixth * 10f)
				{
					return 22; // A
				}
				else if (pitch >= OneSixth * 10f && pitch < OneSixth * 11f)
				{
					return 23; // A#
				}
				else if (pitch >= OneSixth * 11f && pitch < OneSixth * 12f)
				{
					return 24; // B
				}
				else if (pitch >= OneSixth * 12f)
				{
					return 25; // C high
				}
			}
			return 0;
		}

		public void PlayPiano(int noteValue, Player player, int octave)
		{
			SoundStyle PianoC3 = new(Mod.Name + "/Sounds/Custom/PianoC3")
			{
				MaxInstances = 3
			};
			SoundStyle PianoC4 = new(Mod.Name + "/Sounds/Custom/PianoC4")
			{
				MaxInstances = 3
			};
			SoundStyle PianoC5 = new(Mod.Name + "/Sounds/Custom/PianoC5")
			{
				MaxInstances = 3
			};
			SoundStyle note = octave switch
			{
				1 => PianoC3,
				2 => PianoC4,
				3 => PianoC5,
				_ => PianoC3
			};

			RijamsMod modInstance = ModContent.GetInstance<RijamsMod>();

			switch (noteValue)
			{
				case 1:
					modInstance.PlayNetworkSound(note with { Pitch = -0.5f }, player.position, player); // C low
					break;
				case 2:
					modInstance.PlayNetworkSound(note with { Pitch = -0.4703f }, player.position, player); // C#
					break;
				case 3:
					modInstance.PlayNetworkSound(note with { Pitch = -0.4388f }, player.position, player); // D
					break;
				case 4:
					modInstance.PlayNetworkSound(note with { Pitch = -0.4054f }, player.position, player); // D#
					break;
				case 5:
					modInstance.PlayNetworkSound(note with { Pitch = -0.3700f }, player.position, player); // E
					break;
				case 6:
					modInstance.PlayNetworkSound(note with { Pitch = -0.3326f }, player.position, player); // F
					break;
				case 7:
					modInstance.PlayNetworkSound(note with { Pitch = -0.2929f }, player.position, player); // F#
					break;
				case 8:
					modInstance.PlayNetworkSound(note with { Pitch = -0.2508f }, player.position, player); // G
					break;
				case 9:
					modInstance.PlayNetworkSound(note with { Pitch = -0.2063f }, player.position, player); // G#
					break;
				case 10:
					modInstance.PlayNetworkSound(note with { Pitch = -0.1591f }, player.position, player); // A
					break;
				case 11:
					modInstance.PlayNetworkSound(note with { Pitch = -0.1091f }, player.position, player); // A#
					break;
				case 12:
					modInstance.PlayNetworkSound(note with { Pitch = -0.0561f }, player.position, player); // B
					break;
				case 13:
					modInstance.PlayNetworkSound(note with { Pitch = 0f }, player.position, player); // C middle
					break;
				case 14:
					modInstance.PlayNetworkSound(note with { Pitch = 0.0595f }, player.position, player); // C#
					break;
				case 15:
					modInstance.PlayNetworkSound(note with { Pitch = 0.1225f }, player.position, player); // D
					break;
				case 16:
					modInstance.PlayNetworkSound(note with { Pitch = 0.1892f }, player.position, player); // D#
					break;
				case 17:
					modInstance.PlayNetworkSound(note with { Pitch = 0.2599f }, player.position, player); // E
					break;
				case 18:
					modInstance.PlayNetworkSound(note with { Pitch = 0.3348f }, player.position, player); // F
					break;
				case 19:
					modInstance.PlayNetworkSound(note with { Pitch = 0.4142f }, player.position, player); // F#
					break;
				case 20:
					modInstance.PlayNetworkSound(note with { Pitch = 0.4983f }, player.position, player); // G
					break;
				case 21:
					modInstance.PlayNetworkSound(note with { Pitch = 0.5874f }, player.position, player); // G#
					break;
				case 22:
					modInstance.PlayNetworkSound(note with { Pitch = 0.6818f }, player.position, player); // A
					break;
				case 23:
					modInstance.PlayNetworkSound(note with { Pitch = 0.7818f }, player.position, player); // A#
					break;
				case 24:
					modInstance.PlayNetworkSound(note with { Pitch = 0.8877f }, player.position, player); // B
					break;
				case 25:
					modInstance.PlayNetworkSound(note with { Pitch = 2f }, player.position, player); // C high
					break;
				default:
					modInstance.PlayNetworkSound(note with { Pitch = 0f }, player.position, player); // C middle
					break;
			};
		}
		public static void CursorNotes(int noteValue, Player player, int octave)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				Main.mouseText = true;
				switch (noteValue)
				{
					case 1:
						Main.instance.MouseText("C " + (octave + 1)); // C low
						break;
					case 2:
						Main.instance.MouseText("C# " + (octave + 1)); // C#
						break;
					case 3:
						Main.instance.MouseText("D " + (octave + 1)); // D
						break;
					case 4:
						Main.instance.MouseText("D# " + (octave + 1)); // D#
						break;
					case 5:
						Main.instance.MouseText("E " + (octave + 1)); // E
						break;
					case 6:
						Main.instance.MouseText("F " + (octave + 1)); // F
						break;
					case 7:
						Main.instance.MouseText("F# " + (octave + 1)); // F#
						break;
					case 8:
						Main.instance.MouseText("G " + (octave + 1)); // G
						break;
					case 9:
						Main.instance.MouseText("G# " + (octave + 1));// G#
						break;
					case 10:
						Main.instance.MouseText("A " + (octave + 1)); // A
						break;
					case 11:
						Main.instance.MouseText("A# " + (octave + 1)); // A#
						break;
					case 12:
						Main.instance.MouseText("B " + (octave + 1)); // B
						break;
					case 13:
						Main.instance.MouseText("C " + (octave + 2)); // C middle
						break;
					case 14:
						Main.instance.MouseText("C# " + (octave + 2)); // C#
						break;
					case 15:
						Main.instance.MouseText("D " + (octave + 2)); // D
						break;
					case 16:
						Main.instance.MouseText("D# " + (octave + 2)); // D#
						break;
					case 17:
						Main.instance.MouseText("E " + (octave + 2)); // E
						break;
					case 18:
						Main.instance.MouseText("F " + (octave + 2)); // F
						break;
					case 19:
						Main.instance.MouseText("F# " + (octave + 2)); // F#
						break;
					case 20:
						Main.instance.MouseText("G " + (octave + 2)); // G
						break;
					case 21:
						Main.instance.MouseText("G# " + (octave + 2)); // G#
						break;
					case 22:
						Main.instance.MouseText("A " + (octave + 2)); // A
						break;
					case 23:
						Main.instance.MouseText("A# " + (octave + 2)); // A#
						break;
					case 24:
						Main.instance.MouseText("B " + (octave + 2)); // B
						break;
					case 25:
						Main.instance.MouseText("C " + (octave + 3)); // C high
						break;
					default:
						Main.instance.MouseText("C " + (octave + 2)); // C middle
						break;
				};
			}
		}
	}
	public class PianistsGloveLow : PianistsGlove
	{
		public override void RightClick(Player player)
		{
			RickClickInner(player, ModContent.ItemType<PianistsGlove>());
		}
		public override void HoldItem(Player player)
		{
			HoldItemInner(player, 1);
		}
		public override bool? UseItem(Player player)
		{
			UseItemInner(player, 1, ModContent.ItemType<PianistsGlove>());
			return null;
		}
		public override void AddRecipes()
		{
			// Empty so that it doesn't inherit the recipe from the parent class
		}
	}
	public class PianistsGloveHigh : PianistsGlove
	{
		public override void RightClick(Player player)
		{
			RickClickInner(player, ModContent.ItemType<PianistsGloveLow>());
		}
		public override void HoldItem(Player player)
		{
			HoldItemInner(player, 3);
		}
		public override bool? UseItem(Player player)
		{
			UseItemInner(player, 3, ModContent.ItemType<PianistsGloveLow>());
			return null;
		}
		public override void AddRecipes()
		{
			// Empty so that it doesn't inherit the recipe from the parent class
		}
	}
}