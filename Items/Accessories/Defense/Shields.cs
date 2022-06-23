using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Defense
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
			Item.width = 34;
			Item.height = 34;
			Item.rare = ItemRarityID.Pink; //5
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.accessory = true;
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
				player.starCloakItem = Item;
				player.honeyCombItem = Item;
				//player.GetModPlayer<RijamsModPlayer>().honeyComb = true;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.ObsidianShield, 1)
				.AddIngredient(ModContent.ItemType<RoseOfFireAndIce>(), 1)
				.AddIngredient(ItemID.StarVeil, 1)
				.AddIngredient(ItemID.SweetheartNecklace, 1)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
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
			Item.width = 38;
			Item.height = 36;
			Item.rare = ItemRarityID.Red; //10
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.accessory = true;
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
				player.starCloakItem = Item;
				player.honeyCombItem = Item;
				//player.GetModPlayer<RijamsModPlayer>().honeyComb = true;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<StellarShield>(), 1)
				.AddIngredient(ItemID.AnkhCharm, 1)
				.AddIngredient(ItemID.PocketMirror, 1)
				.AddIngredient(ItemID.LunarBar, 2)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<StellarShield>(), 1)
				.AddIngredient(ItemID.AnkhShield, 1)
				.AddIngredient(ItemID.PocketMirror, 1)
				.AddIngredient(ItemID.LunarBar, 2)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.AnkhShield, 1)
				.AddIngredient(ModContent.ItemType<RoseOfFireAndIce>(), 1)
				.AddIngredient(ItemID.StarVeil, 1)
				.AddIngredient(ItemID.SweetheartNecklace, 1)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddIngredient(ItemID.PocketMirror, 1)
				.AddIngredient(ItemID.LunarBar, 2)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}