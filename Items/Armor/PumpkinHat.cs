using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class PumpkinHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Pumpkin Hat");
			// Tooltip.SetDefault("+1 Minion Capacity");
			AddArmorSet(ModContent.ItemType<PumpkinHat>(), ItemID.PumpkinBreastplate, ItemID.PumpkinLeggings,
				LocalizedText: Language.GetText($"ArmorSetBonus.Pumpkin"),
				PrimaryPart: ArmorSetBonus.PartType.Head,
				Identifier: "Pumpkin",
				Effect: ArmorSetBonuses.Benefits.Pumpkin);
			ArmorIDs.Head.Sets.CanDrawOnVelociraptorMount[Item.headSlot] = true;
			ArmorIDs.Head.Sets.VelociraptorMountHeadOffset[Item.headSlot] = new Vector2(0, -2);
		}

		public override void SetDefaults()
		{
			Item.value = 10;
			Item.rare = ItemRarityID.White;
			Item.defense = 1;
		}
		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
		}

		/*
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.PumpkinBreastplate && legs.type == ItemID.PumpkinLeggings;
		}

		public override void UpdateArmorSet(Player player, ArmorSetBonus armorSetBonus)
		{
			// TODO player.setBonus = Language.GetTextValue("ArmorSetBonus.Pumpkin");
			player.GetDamage(DamageClass.Generic) += 0.1f;
		}
		*/

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Pumpkin, 20)
				.AddIngredient(ItemID.FallenStar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}