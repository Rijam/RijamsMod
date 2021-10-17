using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	//ModPlayer class handles the bools (RijamsModPlayer.cs)
	//GlobalItem class handles spawning the dust and light for swinging the sword (GlobalAccessoryItems.cs)
	//GlobalProjectile class handles spawning the dust and light for melee projectile (GlobalProjectiles.cs)
	//GlobalNPC class handles applying the debuff to enemies (RijamsModNPCs.cs)
	public class DaybreakStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daybreak Stone");
			Tooltip.SetDefault("Melee attacks inflict Daybroken");
		}

		public override void SetDefaults()
		{
			//item.color = Color.Yellow; //colors the inventory sprite
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Cyan; //9
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().daybreakStone = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MagmaStone, 1);
			recipe.AddIngredient(ItemID.FragmentSolar, 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}