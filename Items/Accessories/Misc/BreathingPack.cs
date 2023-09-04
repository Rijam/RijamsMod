using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RijamsMod.Items.Accessories.Misc
{
	[AutoloadEquip(EquipType.Back)]
	public class BreathingPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Refills the player's air supply two seconds after it reaches zero");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After completing quest]" });
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 34;
			Item.value = 50000;
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().breathingPack = true;
		}
	}
	[AutoloadEquip(EquipType.Back)]
	public class FireBreathingPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Grants immunity to On Fire!, Sulfuric Acid, Ichor, Cursed Inferno, and Frostburn\nRefills the player's air supply two seconds after it reaches zero");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 34;
			Item.value = 60000;
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().breathingPack = true;
			player.buffImmune[BuffID.OnFire] = true;
			player.buffImmune[ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>()] = true;
			player.buffImmune[BuffID.Ichor] = true;
			player.buffImmune[BuffID.CursedInferno] = true;
			player.buffImmune[BuffID.Frostburn] = true;
			player.buffImmune[BuffID.ShadowFlame] = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<BreathingPack>(), 1)
				.AddIngredient(ItemID.LivingFireBlock, 20)
				.AddIngredient(ModContent.ItemType<Placeable.LivingSulfurFireBlock>(), 20)
				.AddIngredient(ItemID.LivingIchorBlock, 20)
				.AddIngredient(ItemID.LivingCursedFireBlock, 20)
				.AddIngredient(ItemID.LivingFrostFireBlock, 20)
				.AddIngredient(ItemID.LivingDemonFireBlock, 20)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}