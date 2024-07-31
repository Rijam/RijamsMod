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
		Vector3 LightColor = new(0.35f, 0.35f, 0f);
		
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
			Lighting.AddLight(player.Center, LightColor);
		}
		public override void UpdateVanity(Player player)
		{
			Lighting.AddLight(player.Center, LightColor * 0.25f);
		}

		public override Color? GetAlpha(Color newColor)
		{
			return Color.White;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FallenStar, 1)
				.AddRecipeGroup(RijamsModRecipes.CopperBars, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.HandsOn)]
	public class MediumGlowRing : ModItem
	{
		Vector3 LightColor = new(0.90f, 0.80f, 0.40f);

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.rare = ItemRarityID.Green; // 2
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Lighting.AddLight(player.Center, LightColor);
		}
		public override void UpdateVanity(Player player)
		{
			Lighting.AddLight(player.Center, LightColor * 0.25f);
		}

		public override Color? GetAlpha(Color newColor)
		{
			return Color.White;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SmallGlowRing>(), 1)
				.AddIngredient(ItemID.Meteorite, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.HandsOn)]
	public class LargeGlowRing : ModItem
	{
		Vector3 LightColor = new(1.2f, 1.2f, 0.60f);

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.rare = ItemRarityID.Orange; // 3
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Lighting.AddLight(player.Center, LightColor);
		}
		public override void UpdateVanity(Player player)
		{
			Lighting.AddLight(player.Center, LightColor * 0.25f);
		}
		public override Color? GetAlpha(Color newColor)
		{
			return Color.White;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<MediumGlowRing>(), 1)
				.AddIngredient(ItemID.Hellstone, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.HandsOn)]
	public class BurglarsRing : ModItem
	{
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
				.AddRecipeGroup(RijamsModRecipes.EvilBars, 1)
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