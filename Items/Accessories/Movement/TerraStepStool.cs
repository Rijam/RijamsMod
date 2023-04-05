using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace RijamsMod.Items.Accessories.Movement
{
	public class TerraStepStool : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.PortableStool"));
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 56;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(gold: 5);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.portableStoolInfo.SetStats(112, 112, 112);
			player.GetModPlayer<RijamsModPlayer>().terraStepStool = true;
	
			// Add the dye for the slot the step stool is in.
			// Still a little weirdness when equipping the normal step stool (or Hand Of Creation for some reason) in different slots.
			for (int i = 0; i < player.armor.Length; i++)
			{
				if (player.IsItemSlotUnlockedAndUsable(i))
				{
					int num = i % 10;
					if (player.armor[i].type == Type)
					{
						player.cPortableStool = player.dye[num].dye;
						break;
					}
				}
			}
			
		}

		// Detour the original method to add my drawing.

		public override void Load()
		{
			Terraria.DataStructures.On_PlayerDrawLayers.DrawPlayer_03_PortableStool += Player_Hook_DrawPlayer_03_PortableStool;
		}

		private static void Player_Hook_DrawPlayer_03_PortableStool(Terraria.DataStructures.On_PlayerDrawLayers.orig_DrawPlayer_03_PortableStool orig, ref PlayerDrawSet drawinfo)
		{
			// Probably not the best way to do this since it is just drawing the new texture on top of the vanilla one.
			// The check for the HeightBoost is so it doesn't draw when the vanilla Step Stool takes priority.
			Player stoolPlayer = drawinfo.drawPlayer;
			if (stoolPlayer.portableStoolInfo.IsInUse && stoolPlayer.portableStoolInfo.HeightBoost != 26 && stoolPlayer.GetModPlayer<RijamsModPlayer>().terraStepStool)
			{
				Texture2D value = ModContent.Request<Texture2D>("RijamsMod/Items/Accessories/Movement/TerraStepStoolWorld").Value;
				Vector2 position = new((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(stoolPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)stoolPlayer.height + 112f));
				Rectangle rectangle = value.Frame();
				Vector2 origin = rectangle.Size() * new Vector2(0.5f, 1f);
				DrawData item = new(value, position, rectangle, drawinfo.colorArmorLegs, stoolPlayer.bodyRotation, origin, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cPortableStool
				};
				drawinfo.DrawDataCache.Add(item);
			}
			else
			{
				// Draw the original step stool if the Terra Step Stool doesn't take priority.
				orig(ref drawinfo);
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.PortableStool)
				.AddIngredient(ItemID.BrokenHeroSword)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}