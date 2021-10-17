using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons
{
	public class StabbyShiv : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stabby Shiv");
			Tooltip.SetDefault("[c/403638:Dropped by Mister Stabby]");
		}

		public override void SetDefaults() {
			item.damage = 45;
			item.melee = true;
			item.width = 18;
			item.height = 18;
			item.useTime = 7; 
			item.useAnimation = 7;
			item.knockBack = 8;
			item.value = Item.buyPrice(gold: 2);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = false;
			item.crit = 10;
			item.useStyle = ItemUseStyleID.Stabbing; // 3, shortsword style
		}
	}
	
	public class FrostyShiv : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frosty Shiv");
			Tooltip.SetDefault("Inflicts Frostburn"); 
		}

		public override void SetDefaults() {
			item.damage = 45;
			item.melee = true;
			item.width = 20;
			item.height = 20;
			item.useTime = 7; 
			item.useAnimation = 7;
			item.knockBack = 8;
			item.value = 22000;
			item.rare = ItemRarityID.Pink; // 5
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.crit = 10;
			item.useStyle = ItemUseStyleID.Stabbing; // 3, shortsword style
			if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/FrostyShiv_Glow");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = 0;
				item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 0;
            }
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StabbyShiv>());
			recipe.AddIngredient(ItemID.IceTorch, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			// 60 frames = 1 second
			target.AddBuff(BuffID.Frostburn, 120);
		}
	}
}
