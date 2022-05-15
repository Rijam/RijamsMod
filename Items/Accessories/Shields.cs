using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{

	[AutoloadEquip(EquipType.Shield)]
	public class StellarShield : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Shield");
			Tooltip.SetDefault("+2 Defense\nGrants immunity to knockback\nGrants immunity to fire blocks\nGrants immunity to Frostburn, Frozen, and Chilled" +
				"\nReduce damage from touching lava\nCauses stars to fall after taking damage\nIncreases length of invincibility after taking damage" +
				"\nReleases bees and douses the user in honey after taking damage\nIncreases movement after taking damage\n(Hide to disable Star Cloak and Honey Comb)");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.rare = ItemRarityID.Pink; //5
			item.value = Item.sellPrice(0, 4, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statDefense += 2;
			player.noKnockback = true;
			player.fireWalk = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
			player.lavaRose = true;
			player.longInvince = true;
			player.panic = true;
			if (!hideVisual)
            {
				player.starCloak = true;
				player.GetModPlayer<RijamsModPlayer>().honeyComb = true;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ObsidianShield, 1);
			recipe.AddIngredient(ModContent.ItemType<RoseOfFireAndIce>(), 1);
			recipe.AddIngredient(ItemID.StarVeil, 1);
			recipe.AddIngredient(ItemID.SweetheartNecklace, 1);
			recipe.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Shield)]
	public class Citadel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Citadel");
			Tooltip.SetDefault("+4 Defense\nGrants immunity to knockback\nGrants immunity to fire blocks\nGrants immunity to most debuffs" +
				"\nReduce damage from touching lava\nCauses stars to fall after taking damage\nIncreases length of invincibility after taking damage" +
				"\nReleases bees and douses the user in honey after taking damage\nIncreases movement after taking damage\n(Hide to disable Star Cloak and Honey Comb)" +
				"\n'I've never see it lit up like this!'");
		}

		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 36;
			item.rare = ItemRarityID.Red; //10
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statDefense += 4;
			player.noKnockback = true;
			player.fireWalk = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
			player.buffImmune[BuffID.Bleeding] = true;
			player.buffImmune[BuffID.BrokenArmor] = true;
			player.buffImmune[BuffID.Confused] = true;
			player.buffImmune[BuffID.Cursed] = true;
			player.buffImmune[BuffID.Darkness] = true;
			player.buffImmune[BuffID.Poisoned] = true;
			player.buffImmune[BuffID.Silenced] = true;
			player.buffImmune[BuffID.Darkness] = true;
			player.buffImmune[BuffID.Slow] = true;
			player.buffImmune[BuffID.Weak] = true;
			player.buffImmune[BuffID.OnFire] = true;
			player.buffImmune[BuffID.Stoned] = true;
			player.lavaRose = true;
			player.longInvince = true;
			player.panic = true;
			if (!hideVisual)
			{
				player.starCloak = true;
				player.GetModPlayer<RijamsModPlayer>().honeyComb = true;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StellarShield>(), 1);
			recipe.AddIngredient(ItemID.AnkhCharm, 1);
			recipe.AddIngredient(ItemID.PocketMirror, 1);
			recipe.AddIngredient(ItemID.LunarBar, 2);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StellarShield>(), 1);
			recipe.AddIngredient(ItemID.AnkhShield, 1);
			recipe.AddIngredient(ItemID.PocketMirror, 1);
			recipe.AddIngredient(ItemID.LunarBar, 2);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AnkhShield, 1);
			recipe.AddIngredient(ModContent.ItemType<RoseOfFireAndIce>(), 1);
			recipe.AddIngredient(ItemID.StarVeil, 1);
			recipe.AddIngredient(ItemID.SweetheartNecklace, 1);
			recipe.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10);
			recipe.AddIngredient(ItemID.PocketMirror, 1);
			recipe.AddIngredient(ItemID.LunarBar, 2);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}