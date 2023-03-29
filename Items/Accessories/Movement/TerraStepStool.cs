using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria.Localization;

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
		}

		// Detour the original method to add my drawing.

		private static readonly MethodInfo DrawPlayer_03_PortableStool = typeof(PlayerDrawLayers).GetMethod(nameof(PlayerDrawLayers.DrawPlayer_03_PortableStool), BindingFlags.Public | BindingFlags.Static);
		private delegate void orig_DrawPlayer_03_PortableStool(ref PlayerDrawSet drawinfo);
		private delegate void hook_DrawPlayer_03_PortableStool(orig_DrawPlayer_03_PortableStool orig, ref PlayerDrawSet drawinfo);

		/*private static event hook_DrawPlayer_03_PortableStool On_DrawPlayer_03_PortableStool
		{
			add => HookEndpointManager.Add<hook_DrawPlayer_03_PortableStool>(DrawPlayer_03_PortableStool, value);
			remove => HookEndpointManager.Remove<hook_DrawPlayer_03_PortableStool>(DrawPlayer_03_PortableStool, value);
		}

		public override void Load()
		{
			On_DrawPlayer_03_PortableStool += Hook_DrawPlayer_03_PortableStool;
		}

		public override void Unload()
		{
			On_DrawPlayer_03_PortableStool -= Hook_DrawPlayer_03_PortableStool;
		}*/

		private void Hook_DrawPlayer_03_PortableStool(orig_DrawPlayer_03_PortableStool orig, ref PlayerDrawSet drawinfo)
		{
			// Small bug, shaders (dyes) don't get applied unless the player also has the vanilla Step Stool (in vanity)
			// Probably not the best way to do this since it is just drawing the new texture on top of the vanilla one.
			// The check for the HeightBoost is so it doesn't draw when the vanilla Step Stool takes priority.
			orig(ref drawinfo);
			Player stoolPlayer = drawinfo.drawPlayer;
			if (stoolPlayer.portableStoolInfo.IsInUse && stoolPlayer.portableStoolInfo.HeightBoost != 26 && stoolPlayer.GetModPlayer<RijamsModPlayer>().terraStepStool)
			{
				Texture2D value = ModContent.Request<Texture2D>("RijamsMod/Items/Accessories/Movement/TerraStepStoolWorld").Value;
				Vector2 position = new ((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(stoolPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)stoolPlayer.height + 112f));
				Rectangle rectangle = value.Frame();
				Vector2 origin = rectangle.Size() * new Vector2(0.5f, 1f);
				DrawData item = new(value, position, rectangle, drawinfo.colorArmorLegs, stoolPlayer.bodyRotation, origin, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cPortableStool;
				drawinfo.DrawDataCache.Add(item);
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