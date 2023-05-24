using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Pets
{
	public class StarCallerStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Star Caller Staff");
			// Tooltip.SetDefault("Summons a light pet Dwarf Star\nEnemies are lit ablaze upon contact");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Deerclops]", null, null });
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 40;
			Item.height = 42;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.DwarfStar>();
			Item.buffType = ModContent.BuffType<Buffs.Pets.DwarfStarBuff>();
			Item.rare = ItemRarityID.Green;
			Item.value = 15000;
			Item.UseSound = SoundID.Item25;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class LumothBulb : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lumoth Bulb");
			// Tooltip.SetDefault("Summons a light pet Lumoth");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 16;
			Item.height = 28;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.Lumoth>();
			Item.buffType = ModContent.BuffType<Buffs.Pets.LumothBuff>();
			Item.rare = ItemRarityID.LightRed;
			Item.value = 50000;
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
			// DisplayName.SetDefault("LED Lumoth Bulb");
			// Tooltip.SetDefault("Summons a light pet LED Lumoth");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 16;
			Item.height = 28;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.LEDLumoth>();
			Item.buffType = ModContent.BuffType<Buffs.Pets.LEDLumothBuff>();
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
			// DisplayName.SetDefault("Interesting Sphere");
			// Tooltip.SetDefault("Summons a pet Snugget");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 22;
			Item.height = 22;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.Snugget>();
			Item.buffType = ModContent.BuffType<Buffs.Pets.SnuggetBuff>();
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
			// DisplayName.SetDefault("Fluffalo Egg");
			// Tooltip.SetDefault("Summons a pet Fluffalo");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 26;
			Item.height = 32;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.Fluffalo>();
			Item.buffType = ModContent.BuffType<Buffs.Pets.FluffaloBuff>();
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

	public class StardustDragonCrest : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Zoologist]", "[c/474747:After defeating Stardust Pillar]", null });
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 26;
			Item.height = 24;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.BabyStardustDragon>();
			Item.buffType = ModContent.BuffType<Buffs.Pets.BabyStardustDragonBuff>();
			Item.rare = ItemRarityID.Red;
			Item.value = 80000;
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