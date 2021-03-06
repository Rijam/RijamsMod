using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Pets
{
	public class LumothBulb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lumoth Bulb");
			Tooltip.SetDefault("Summons a light pet Lumoth");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 16;
			Item.height = 28;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.Lumoth>();
			Item.buffType = ModContent.BuffType<Buffs.LumothBuff>();
			Item.rare = ItemRarityID.LightRed;
			Item.value = 40000;
			Item.UseSound = SoundID.Item25;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Accessories.Misc.LargeGlowRing>(), 1)
				.AddIngredient(ItemID.PixieDust, 5)
				.AddIngredient(ItemID.SoulofFlight, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
	public class LEDLumothBulb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("LED Lumoth Bulb");
			Tooltip.SetDefault("Summons a light pet LED Lumoth");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 16;
			Item.height = 28;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.LEDLumoth>();
			Item.buffType = ModContent.BuffType<Buffs.LEDLumothBuff>();
			Item.rare = ItemRarityID.Red;
			Item.value = 80000;
			Item.UseSound = SoundID.Item25;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<LumothBulb>(), 1)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 1)
				.AddIngredient(ItemID.LunarOre, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	public class InterestingSphere : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Interesting Sphere");
			Tooltip.SetDefault("Summons a pet Snugget");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 22;
			Item.height = 22;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.Snugget>();
			Item.buffType = ModContent.BuffType<Buffs.SnuggetBuff>();
			Item.rare = ItemRarityID.Orange;
			Item.value = 50000;
			Item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class FluffaloEgg : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fluffalo Egg");
			Tooltip.SetDefault("Summons a pet Fluffalo");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 26;
			Item.height = 32;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.Fluffalo>();
			Item.buffType = ModContent.BuffType<Buffs.FluffaloBuff>();
			Item.rare = ItemRarityID.LightPurple;
			Item.value = 70000;
			Item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
}