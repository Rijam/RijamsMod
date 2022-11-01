using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class SmallGlowRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Small Glow Ring");
			Tooltip.SetDefault("Emits a small amount of light");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Lighting.AddLight(player.Center, Color.Yellow.ToVector3() * 0.35f);
		}
		public override Color? GetAlpha(Color newColor)
		{
			return new Color(255, 255, 255, 255);
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FallenStar, 1)
				.AddRecipeGroup("RijamsMod:CopperBars", 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.HandsOn)]
	public class LargeGlowRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Large Glow Ring");
			Tooltip.SetDefault("Emits a large amount of light");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.rare = ItemRarityID.Orange; //3
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Lighting.AddLight(player.Center, 1.0f, 0.90f, 0.40f);
		}
		public override Color? GetAlpha(Color newColor)
		{
			return new Color(255, 255, 255, 255);
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SmallGlowRing>(), 1)
				.AddIngredient(ItemID.Meteorite, 1)
				.AddIngredient(ItemID.Hellstone, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.HandsOn)]
	public class BurglarsRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Burglar's Ring");
			Tooltip.SetDefault("Enemies drop double loot\n'Purple burglar alarm'");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.rare = ItemRarityID.LightRed; //4
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			bool isLeftShiftHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
			if (isLeftShiftHeld)
			{
				tooltips.Add(new TooltipLine(Mod, "Info", "Enemies will call their loot function"));
				tooltips.Add(new TooltipLine(Mod, "Info", "twice upon death. Does not work on"));
				tooltips.Add(new TooltipLine(Mod, "Info", "enemies that are considered bosses"));
				tooltips.Add(new TooltipLine(Mod, "Info", "and the player must have attacked"));
				tooltips.Add(new TooltipLine(Mod, "Info", "the enemy at least once."));
			}
			else
			{
				tooltips.Add(new TooltipLine(Mod, "Info", "Hold Left Shift for more info"));
			}
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().burglarsRing = true;
			//See RijamsModNPC PreNPCLoot() for the effects of the accessory
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("RijamsMod:EvilBars", 1)
				.AddIngredient(ItemID.AncientCloth, 1)
				.AddIngredient(ItemID.SpiderFang, 1)
				.AddIngredient(ItemID.GoldDust, 1)
				.AddIngredient(ItemID.UnicornHorn, 1)
				.AddIngredient(ItemID.Vine, 1)
				.AddIngredient(ItemID.FrostCore, 1)
				.AddIngredient(ItemID.AncientBattleArmorMaterial, 1) //Forbidden Fragment
				.AddIngredient(ItemID.GreenThread, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}