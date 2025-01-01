using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace RijamsMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class FlinxFurHat : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.value = 25000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.FlinxFurCoat && legs.type == ModContent.ItemType<FlinxFurBoots>();
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.1f;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods." + Mod.Name + ".ArmorSetBonus.FlinxFur");
			player.maxMinions++;
			player.statDefense++;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Silk, 5)
				.AddIngredient(ItemID.FlinxFur, 4)
				.AddRecipeGroup(RijamsModRecipes.GoldBars, 2)
				.AddTile(TileID.Loom)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class FlinxFurBoots : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.value = 25000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().supportMinionRadiusIncrease += 3;
			player.whipRangeMultiplier += 0.05f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Leather, 3)
				.AddIngredient(ItemID.FlinxFur, 2)
				.AddRecipeGroup(RijamsModRecipes.GoldBars, 2)
				.AddTile(TileID.Loom)
				.Register();
		}
	}

	public class FlinxFurBootsPlayerDrawLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new BeforeParent(PlayerDrawLayers.Leggings);
		}
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, "FlinxFurBoots", EquipType.Legs);
		}
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (!drawInfo.drawPlayer.invis && !(drawInfo.drawPlayer.shoe > 0 && ArmorIDs.Shoe.Sets.OverridesLegs[drawInfo.drawPlayer.shoe]))
			{
				// Draw the pants
				DrawData drawData = new(
					TextureAssets.Players[drawInfo.skinVar, 11].Value,
					new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.legFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.legFrame.Height + 4f)) + drawInfo.drawPlayer.legPosition + drawInfo.legVect,
					drawInfo.drawPlayer.legFrame,
					drawInfo.colorPants,
					drawInfo.drawPlayer.legRotation,
					drawInfo.legVect,
					1f,
					drawInfo.playerEffect);
				drawInfo.DrawDataCache.Add(drawData);
				/*
				// Draw the shoes
				drawData = new DrawData(
					TextureAssets.Players[drawInfo.skinVar, 12].Value,
					new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.legFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.legFrame.Height + 4f)) + drawInfo.drawPlayer.legPosition + drawInfo.legVect,
					drawInfo.drawPlayer.legFrame,
					drawInfo.colorShoes,
					drawInfo.drawPlayer.legRotation,
					drawInfo.legVect,
					1f,
					drawInfo.playerEffect);
				drawInfo.DrawDataCache.Add(drawData);
				*/
			}
		}
	}
}