using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class FlinxFurHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			AddArmorSet(ModContent.ItemType<FlinxFurHat>(), ItemID.FlinxFurCoat, ModContent.ItemType<FlinxFurBoots>(),
				LocalizedText: Language.GetText($"Mods.{Mod.Name}.ArmorSetBonus.FlinxFur"),
				PrimaryPart: ArmorSetBonus.PartType.None,
				Identifier: "RijamsMod/FlinxFur",
				Effect: SetBonus_FlinxFur);
		}
		public override void SetDefaults()
		{
			Item.value = 25000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.1f;
		}

		/*
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.FlinxFurCoat && legs.type == ModContent.ItemType<FlinxFurBoots>();
		}

		public override void UpdateArmorSet(Player player, ArmorSetBonus armorSetBonus)
		{
			// TODO player.setBonus = Language.GetTextValue("Mods." + Mod.Name + ".ArmorSetBonus.FlinxFur");
			player.maxMinions++;
			player.statDefense++;
		}
		*/

		public static void SetBonus_FlinxFur(Player player)
		{
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
			Item.value = 25000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionRadiusIncrease += 3;
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

		public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
		{
			// Draw the pants
			DrawData pantsDrawData = new(
				TextureAssets.Players[drawInfo.skinVar, 11].Value,
				drawData.position,
				//new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.legFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.legFrame.Height + 4f)) + drawInfo.drawPlayer.legPosition + drawInfo.legVect,
				drawInfo.drawPlayer.legFrame,
				drawInfo.colorPants,
				drawInfo.drawPlayer.legRotation,
				drawInfo.legVect,
				1f,
				drawInfo.playerEffect);
			drawInfo.DrawDataCache.Add(pantsDrawData);
			return true;
		}
	}

	// Superseded by ModItem.ModifyEquipTextureDraw
	/*
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

				// Draw the shoes
				// drawData = new DrawData(
				//	TextureAssets.Players[drawInfo.skinVar, 12].Value,
				//	new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.legFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.legFrame.Height + 4f)) + drawInfo.drawPlayer.legPosition + drawInfo.legVect,
				//	drawInfo.drawPlayer.legFrame,
				//	drawInfo.colorShoes,
				//	drawInfo.drawPlayer.legRotation,
				//	drawInfo.legVect,
				//	1f,
				//	drawInfo.playerEffect);
				// drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	*/
}