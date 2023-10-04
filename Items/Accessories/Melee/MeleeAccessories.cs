using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Melee
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class DaybreakGauntlet : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Daybreak Gauntlet");
			// Tooltip.SetDefault("+12% Melee damage\n+12% Melee attack speed\n+100% Knockback\n+8 Defense\nEnables auto swing for melee weapons\nIncreases the size of melee weapons\nMelee attacks inflict Daybroken\nGives the user master yoyo skills\nYoyos have infinite duration\nCounterweights are 1.5x bigger\nEnemies are more likely to target you");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Cyan; //9
			Item.value = Item.sellPrice(0, 25, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Melee) += 0.12f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
			player.kbGlove = true;
			player.statDefense += 8;
			player.GetModPlayer<RijamsModPlayer>().daybreakStone = true;
			player.GetModPlayer<RijamsModPlayer>().yoyoBackpack = true;
			player.aggro += 25 * 16; //25 tiles
			player.autoReuseGlove = true;
			player.meleeScaleGlove = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FireGauntlet, 1)
				.AddIngredient(ModContent.ItemType<YoyoBackpack>(), 1)
				.AddIngredient(ItemID.FleshKnuckles, 1)
				.AddIngredient(ModContent.ItemType<DaybreakStone>(), 1)
				.AddIngredient(ItemID.LunarBar, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Back)]
	public class YoyoBackpack : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Yoyo Backpack");
			// Tooltip.SetDefault("Gives the user master yoyo skills\nYoyos have infinite duration\nCounterweights are 1.5x bigger\nRainbow colored string!");
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 46;
			Item.rare = ItemRarityID.Yellow; //8
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().yoyoBackpack = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.YoyoBag)
				.AddIngredient(ItemID.RainbowString)
				.AddRecipeGroup(RijamsModRecipes.Counterweights)
				.AddIngredient(ModContent.ItemType<LoopingOil>())
				.AddIngredient(ModContent.ItemType<Materials.InfernicFabric>(), 10)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

	public class LoopingOil : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Type, new System.Collections.Generic.List<string> { "[c/474747:Sold by Traveling Merchant]", "[c/474747:after defeating any early game boss]" });
		}
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 32;
			Item.rare = ItemRarityID.Green; // 2
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().loopingOil = true;
		}
	}

	public class SideEffects : ModItem
	{
		public override void SetStaticDefaults()
		{

		}
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 32;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().sideEffects = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.ShroomiteBar, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		// Credits to stormytuna for the One Drop drawing code.
		public override void PostDrawTooltip(ReadOnlyCollection<DrawableTooltipLine> lines)
		{
			int verticalOffset = 0;
			for (int i = 0; i < lines.Count; i++)
			{
				if (lines[i].Name == "Tooltip1")
				{
					int flickerAmount = (int)((float)(int)Main.mouseTextColor);
					Color drawColor = Color.Black;
					for (int l = 0; l < 5; l++)
					{
						int xCoord = lines[i].X;
						int yCoord = lines[i].Y;// + verticalOffset; IDK WHY THIS CHANGED BUT IT DID
						if (l == 4)
						{
							drawColor = new Color(flickerAmount, flickerAmount, flickerAmount, flickerAmount);
						}
						switch (l)
						{
							case 0:
								xCoord--;
								break;
							case 1:
								xCoord++;
								break;
							case 2:
								yCoord--;
								break;
							case 3:
								yCoord++;
								break;
						}
						Texture2D oneDropLogo = ((Texture2D)TextureAssets.OneDropLogo);
						Main.spriteBatch.Draw(oneDropLogo, new Vector2(xCoord, yCoord), null, drawColor, 0f, default, 1f, SpriteEffects.None, 0f);
					}
				}
				verticalOffset += (int)FontAssets.MouseText.Value.MeasureString(lines[i].Text).Y;
			}
		}
	}
}