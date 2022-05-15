using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RijamsMod.Items.Accessories
{
	[AutoloadEquip(EquipType.Back)]
	public class BreathingPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Refills the player's air supply two seconds after it reaches zero");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Sold by Interstellar Traveler]");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 34;
			item.value = 50000;
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
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
			Tooltip.SetDefault("Grants immunity to On Fire!, Sulfuric Acid, Ichor, Cursed Inferno, and Frostburn\nRefills the player's air supply two seconds after it reaches zero");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 34;
			item.value = 60000;
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().breathingPack = true;
			player.buffImmune[BuffID.OnFire] = true;
			player.buffImmune[ModContent.BuffType<Buffs.SulfuricAcid>()] = true;
			player.buffImmune[BuffID.Ichor] = true;
			player.buffImmune[BuffID.CursedInferno] = true;
			player.buffImmune[BuffID.Frostburn] = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BreathingPack>());
			recipe.AddIngredient(ItemID.LivingFireBlock, 20);
			recipe.AddIngredient(ModContent.ItemType<Placeable.LivingSulfurFireBlock>(), 20);
			recipe.AddIngredient(ItemID.LivingIchorBlock, 20);
			recipe.AddIngredient(ItemID.LivingCursedFireBlock, 20);
			recipe.AddIngredient(ItemID.LivingFrostFireBlock, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}