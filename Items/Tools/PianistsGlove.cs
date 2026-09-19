using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;

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

		public int HandsOnEquipTexture;
		public int HandsOffEquipTexture;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				HandsOnEquipTexture = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace).Replace('.', '/') + "/PianistsGlove_HandsOn", EquipType.HandsOn, this, "PianistsGlove_HandsOn");
				HandsOffEquipTexture = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace).Replace('.', '/') + "/PianistsGlove_HandsOff", EquipType.HandsOff, this, "PianistsGlove_HandsOff");
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
				// Setting the cached slot doesn't seem to work anymore?
				// player.handon = HandsOnEquipTexture;
				// player.handoff = HandsOffEquipTexture;
				player.handon = EquipLoader.GetEquipSlot(Mod, "PianistsGlove_HandsOn", EquipType.HandsOn);
				player.handoff = EquipLoader.GetEquipSlot(Mod, "PianistsGlove_HandsOff", EquipType.HandsOff);
				// Apply the body dye to the gloves.
				player.cHandOn = player.cBody;
				player.cHandOff = player.cBody;
			}
			int note = CalcNote(player);
			if (note > 0)
			{
				CursorNotes(note, player, octave);
			}
		}

		public bool? UseItemInner(Player player, int octave, int itemToTransformTo)
		{
			int note = CalcNote(player);
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
					// Recipe.FindRecipes()

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
			// Recipe.FindRecipes();
		}

		public override bool ConsumeItem(Player player)
		{
			return false;
		}

		public static int CalcNote(Player player)
		{
			int playerPosX = (int)player.Center.X / 16;
			int playerPosY = (int)player.Center.Y / 16;
			Tile tile = Main.tile[playerPosX, playerPosY];
			ModTile modTile = TileLoader.GetTile(tile.TileType);
			if (WorldGen.InWorld(playerPosX, playerPosY) && tile != null && (Tiles.CustomTileIDSets.IsPiano[tile.TileType] || (modTile != null && modTile.AdjTiles.Contains(TileID.Pianos))))
			{
				float OneSixth = 1f / 6f;
				// This is different to how vanilla calculates the note based on the distance.
				// Vanilla also only allows 6 notes per octave instead of all 12.

				// Get the mouse position.
				float mousePosX = (float)Main.mouseX + Main.screenPosition.X - player.Center.X;
				float mousePosY = (float)Main.mouseY + Main.screenPosition.Y - player.Center.Y;

				// Main.NewText($"mousePosX {mousePosX} mousePosY {mousePosY} Main.Camera.ScaledSize.X {Main.Camera.ScaledSize.X}");
				
				// Calculate X and Y separately.
				// This means the distance becomes a rectangular area with diagonal steps instead of a perfect circle.
				// Normally that would be a problem for distance calculations, but in this application it actually makes it easier to get the correct note.
				//		___________
				//	   |--__   __--|
				//	   |  __-P-__  |
				//	   |--_______--|

				// At 100% zoom and a 1920px wide screen, this gives -960 to 959.
				float pitchX = (float)Math.Abs(mousePosX);
				// At 100% zoom and a 1080px height screen, this gives -540 to 549.
				float pitchY = (float)Math.Abs(mousePosY);

				// At 100% zoom and a 1920px wide screen, this gives a range from the player's center to 80% of the screen width which is 48 tiles (2 tiles per note).
				// At 200% zoom and a 1920px wide screen, it is 24 tiles (1 tile per note).
				pitchX /= (Main.Camera.ScaledSize.X / 2f) * 0.8f; // 80% of the screen is 1f.
				pitchY /= (Main.Camera.ScaledSize.Y / 2f) * 0.8f; // 80% of the screen is 1f.

				pitchX *= 4f;
				pitchX = Math.Clamp(pitchX, 0f, 4f); // Multiply and clamp to 4f so 80% of the screen is 4f.
				pitchY *= 4f;
				pitchY = Math.Clamp(pitchY, 0f, 4f); // Multiply and clamp to 4f so 80% of the screen is 4f.

				// Main.NewText($"pitchX {pitchX} pitchX/OneSixth {pitchX / OneSixth} {(int)(pitchX / OneSixth) + 1}");
				// Main.NewText($"pitchY {pitchY} pitchX/OneSixth {pitchY / OneSixth} {(int)(pitchY / OneSixth) + 1}");

				float combinedPitch = MathHelper.Max(pitchX, pitchY); // Take the one furthest from the player.

				// Pitch ranges from about -0.01 to 4f
				// Divide by 1/6 and cast to int to get 0 to 24. Add one for 1 to 25.
				// 1 -> C low
				// 13 -> C middle
				// 25 -> C high
				return ((int)(combinedPitch / OneSixth)) + 1;
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

			float pitchToPlay = noteValue switch
			{
				1 => -0.5f,			// C low
				2 => -0.45833f,		// C#
				3 => -0.41667f,		// D
				4 => -0.375f,		// D#
				5 => -0.33333f,		// E
				6 => -0.29167f,		// F
				7 => -0.25f,		// F#
				8 => -0.20833f,		// G
				9 => -0.16667f,		// G#
				10 => -0.125f,		// A
				11 => -0.08333f,	// A#
				12 => -0.04167f,	// B
				13 => 0,			// C middle
				14 => 0.08333f,		// C#
				15 => 0.16667f,		// D
				16 => 0.25f,		// D#
				17 => 0.33333f,		// E
				18 => 0.41667f,		// F
				19 => 0.5f,			// F#
				20 => 0.58333f,		// G
				21 => 0.66667f,		// G#
				22 => 0.75f,		// A
				23 => 0.83333f,		// A#
				24 => 0.91667f,		// B
				25 => 1,			// C middle
				_ => 0
			};

			modInstance.PlayNetworkSound(note with { Pitch = pitchToPlay }, player.position, player);
		}
		public static void CursorNotes(int noteValue, Player player, int octave)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				Main.mouseText = true;

				string noteName = noteValue switch
				{
					1 => "C",	// C low
					2 => "C#",
					3 => "D",
					4 => "D#",
					5 => "E",
					6 => "F",
					7 => "F#",
					8 => "G",
					9 => "G#",
					10 => "A",
					11 => "A#",
					12 => "B",
					13 => "C",	// C middle
					14 => "C#",
					15 => "D",
					16 => "D#",
					17 => "E",
					18 => "F",
					19 => "F#",
					20 => "G",
					21 => "G#",
					22 => "A",
					23 => "A#",
					24 => "B",
					25 => "C",	// C high
					_ => "C"
				};

				octave += noteValue >= 25 ? 3 : noteValue >= 13 ? 2 : 1;

				Main.instance.MouseText($"{noteName} {octave}");
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
		public override void Load()
		{
			// Empty so that it doesn't load the equip textures from the parent class
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
		public override void Load()
		{
			// Empty so that it doesn't load the equip textures from the parent class
		}
	}
}