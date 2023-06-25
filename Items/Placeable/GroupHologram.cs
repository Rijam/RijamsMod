using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.NPCs.TownNPCs;
using RijamsMod.NPCs;

namespace RijamsMod.Items.Placeable
{
	public class GroupHologramItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}
		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 32;
			Item.value = 1000000;
			Item.rare = ItemRarityID.Purple;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 20;
			Item.consumable = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.createTile = ModContent.TileType<Tiles.GroupHologramTile>();
			if (!Main.dedServ)
			{
				var glow = Item.GetGlobalItem<ItemUseGlow>();
				glow.glowTexture = ModContent.Request<Texture2D>(GetType().FullName.Replace('.', '/') + "_Glow").Value;
				//glow.blendAlpha = true;
				glow.drawColor = new Color(255, 255, 255, 0);
			}
		}

		public override void AddRecipes()
		{
			if (ModLoader.TryGetMod("CraftablePaintings", out Mod craftablePaintings) && (bool)craftablePaintings.Call("CraftModdedPaintings"))
			{
				CreateRecipe()
					.AddIngredient(ItemID.GlowPaint)
					.AddIngredient((int)craftablePaintings.Call("CanvasItemType"))
					.AddTile((int)craftablePaintings.Call("EaselTileType"))
					.AddCondition(
						Condition.NpcIsPresent(ModContent.NPCType<Harpy>()), 
						Condition.NpcIsPresent(ModContent.NPCType<InterstellarTraveler>()),
						Condition.NpcIsPresent(ModContent.NPCType<HellTrader>()),
						ShopConditions.HellTraderMovedIn)
					.Register();
			}
		}
	}
}