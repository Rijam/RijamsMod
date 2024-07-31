using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using System.Linq;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Audio;

namespace RijamsMod.Items.Accessories.Movement
{
	[AutoloadEquip(EquipType.Wings)]
	public class RedHarpyWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Allows flight and slow fall\nBetter than average horizontal acceleration");
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(170, 6f, 12f);
			// 170 = 2.83 seconds flight time
			// 6 = fly speed
			// 12 = acceleration multiplier

			// Celestial Starboard:
			// 180
			// 8f
			// 4.5f
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 80000;
			Item.rare = ItemRarityID.Red;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//player.wingTimeMax = 170; //2.83 second
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2.5f;
			constantAscend = 0.135f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			// speed = 6f;
			// acceleration *= 12f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 1)
				.AddIngredient(ItemID.SoulofFlight, 20)
				.AddTile(TileID.MythrilAnvil)
				.SortAfter(Main.recipe.First(recipe => recipe.createItem.wingSlot != -1)) // Places this recipe after any wing so every wing stays together in the crafting menu.
				.Register();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			bool isLeftShiftHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
			if (isLeftShiftHeld)
			{
				tooltips.Add(new TooltipLine(Mod, "Stats", "Flight Time: 170 (2.83 seconds)"));
				tooltips.Add(new TooltipLine(Mod, "Stats", "Max Height: 136 tiles"));
				tooltips.Add(new TooltipLine(Mod, "Stats", "Max Horizontal Speed: 32"));
				tooltips.Add(new TooltipLine(Mod, "Stats", "Vertical Speed Multiplier: 250%"));
			}
		}
		public override bool WingUpdate(Player player, bool inUse)
		{
			if (player.pulley || player.velocity.Y == 0f || (player.wet && player.velocity.Y > -0.02 && player.velocity.Y < 0.02)
				/*|| !player.ShouldDrawWingsThatAreAlwaysAnimated()*/)
			{
				player.wingFrame = 0;
				player.wingFrameCounter = 0;
				return true;
			}
			// Copied and modified from Empress Wings Player.WingFrame()
			// search if (wings == 44 && ShouldDrawWingsThatAreAlwaysAnimated()
			int time = 5;

			if (inUse || player.jump > 0)
			{
				player.wingFrameCounter++;
				if (player.wingFrameCounter >= time * 6)
				{
					player.wingFrameCounter = 0;
					if (player.flapSound)
					{
						SoundEngine.PlaySound(SoundID.Item32, player.position);
					}
				}

				player.wingFrame = 1 + player.wingFrameCounter / time;
			}
			else if (player.velocity.Y != 0f)
			{
				if (player.controlJump) // Gliding down
				{
					player.wingFrame = 3;
				}
				else if (player.ShouldFloatInWater && player.wet) // Inner Tube
				{
					player.wingFrame = 1;
				}
				else // In the air but not flying nor gliding.
				{
					player.wingFrame = 2;
				}
			}
			else
			{
				player.wingFrameCounter++;
				if (player.wingFrameCounter >= time * 6)
				{
					player.wingFrameCounter = 0;
					if (player.flapSound)
					{
						SoundEngine.PlaySound(SoundID.Item32, player.position);
					}
				}

				player.wingFrame = 1 + player.wingFrameCounter / time;
			}
			return true;
		}
	}

	public class RedHarpyWingsDrawLayer : PlayerDrawLayer
	{
		public Asset<Texture2D> wingTexture;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawInfo.hideEntirePlayer)
			{
				return false;
			}
			if (drawPlayer.wings == EquipLoader.GetEquipSlot(Mod, "RedHarpyWings", EquipType.Wings))
			{
				return true;
			}

			return false;
		}
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Wings);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			wingTexture ??= ModContent.Request<Texture2D>("RijamsMod/Items/Accessories/Movement/RedHarpyWings_DrawLayer");

			DrawData? wingData = null;
			foreach (DrawData data in drawInfo.DrawDataCache)
			{
				if (data.texture == TextureAssets.Wings[EquipLoader.GetEquipSlot(Mod, "RedHarpyWings", EquipType.Wings)].Value)
				{
					wingData = data;
					break;
				}
			}
			if (wingData.HasValue)
			{
				DrawData glow = new(
					texture: wingTexture.Value,
					color: drawInfo.colorArmorBody, // For glow masks, multiply the color by `drawInfo.stealth * (1f - drawInfo.shadow))`
					position: wingData.Value.position,
					sourceRect: wingData.Value.sourceRect,
					rotation: wingData.Value.rotation,
					origin: wingData.Value.origin,
					scale: wingData.Value.scale,
					effect: wingData.Value.effect,
					inactiveLayerDepth: 0
				)
				{
					shader = wingData.Value.shader
				};
				drawInfo.DrawDataCache.Add(glow);
			}
		}
	}
}