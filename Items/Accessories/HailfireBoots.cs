using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	[AutoloadEquip(EquipType.Shoes)]
	public class HailfireBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows flight, super fast running, and extra mobility on ice\n" +
				"+10% increased movement speed and +0.5 seconds of flight time\n" +
				"Provides the ability to walk on water, honey, & lava\n" +
				"Grants immunity to fire blocks and 7 seconds of immunity to lava\n" +			
				"Grants immunity to On Fire!, Frostburn, Frozen, and Chilled\n" +
				"Reduces damage from touching lava\n" +
				"Increases jump speed and allows auto-jump\n" +
				"Increases fall resistance");
		}

		public override void SetDefaults()
		{
			item.width = 42; 
			item.height = 44;
			item.accessory = true;
			item.rare = ItemRarityID.Red;
			item.value = 200000;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RijamsModPlayer>().hailfireBootsBoost = true;
			player.rocketBoots = 2;
			player.accRunSpeed = 8f; // The player's maximum run speed with accessories
			player.moveSpeed += 0.1f; // The acceleration multiplier of the player's movement speed
			player.iceSkate = true;
			player.lavaMax += 420;
			player.waterWalk = true;
			player.fireWalk = true;
			player.jumpBoost = true;
			player.autoJump = true;
			player.jumpSpeedBoost += 1f;
			player.extraFall += 15;
			player.buffImmune[BuffID.OnFire] = true;
			player.GetModPlayer<RijamsModPlayer>().frostyRose = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FrostsparkBoots, 1);
			recipe.AddIngredient(ItemID.LavaWaders, 1);
			recipe.AddIngredient(ModContent.ItemType<FrostyRose>(), 1);
			recipe.AddIngredient(ItemID.ObsidianRose, 1);
			recipe.AddIngredient(ItemID.IceFeather, 1);
			recipe.AddIngredient(ItemID.FireFeather, 1);
			recipe.AddIngredient(ItemID.FrogLeg, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
