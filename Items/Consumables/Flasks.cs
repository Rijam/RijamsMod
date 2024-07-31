using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Consumables
{
	public class FlaskOfSulfuricAcid : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.DrinkParticleColors[Item.type] = new Color[3]
			{
				new(255, 255, 0),
				new(193, 43, 43),
				new(181, 115, 20)
			};
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(silver: 10);
			Item.buffType = ModContent.BuffType<Buffs.Potions.ImbueSulfuricAcid>();
			Item.buffTime = Item.flaskTime; //20 minutes
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater, 1)
				.AddIngredient(ModContent.ItemType<Materials.Sulfur>(), 5)
				.AddTile(TileID.ImbuingStation)
				.Register();
		}
	}
	public class FlaskOfOil : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Flask of Oil");
			// Tooltip.SetDefault("Melee attacks inflict enemies with Oiled");
			ItemID.Sets.DrinkParticleColors[Item.type] = new Color[3]
			{
				new(176, 177, 57),
				new(59, 48, 32),
				new(10, 9, 9)
			};
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(silver: 5);
			Item.buffType = ModContent.BuffType<Buffs.Potions.ImbueOiled>();
			Item.buffTime = Item.flaskTime; //20 minutes
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater, 1)
				.AddIngredient(ItemID.Sunflower, 5)
				.AddTile(TileID.ImbuingStation)
				.Register();
		}
	}
}
