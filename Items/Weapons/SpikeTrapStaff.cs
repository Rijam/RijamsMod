using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons
{
	public class SpikeTrapStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spike Trap Staff");
			Tooltip.SetDefault("Summons a sentry Spike Trap\nDeals damage every 0.5 seconds");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.knockBack = 0f;
			item.mana = 10;
			item.width = 32;
			item.height = 32;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item44;
			item.autoReuse = false;

			// These below are needed for a sentry weapon
			item.noMelee = true;
			item.summon = true;
			item.sentry = true;
			item.shoot = ModContent.ProjectileType<SpikeTrap>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
			player.UpdateMaxTurrets();
			return false;
		}
		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return !Collision.SolidCollision(mousePos, player.width / 4, player.height / 4);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Spike, 9);
			recipe.AddIngredient(ItemID.Bone, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class SuperSpikeTrapStaff : SpikeTrapStaff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Spike Trap Staff");
			Tooltip.SetDefault("Summons a sentry Super Spike Trap\nDeals damage every 0.25 seconds");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.damage = 35;
			item.width = 48;
			item.height = 48;
			item.value = Item.sellPrice(gold: 1, silver: 50);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item44;
			item.autoReuse = true;

			// These below are needed for a sentry weapon
			item.noMelee = true;
			item.summon = true;
			item.sentry = true;
			item.shoot = ModContent.ProjectileType<SuperSpikeTrap>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "SpikeTrapStaff", 1);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class SlimeTrapStaff : SpikeTrapStaff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slime Trap Staff");
			Tooltip.SetDefault("Summons a sentry Slime Trap\nReduces enemy movement speed by 25%");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.damage = 0;
			item.width = 28;
			item.height = 30;
			item.value = Item.sellPrice(silver: 50);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item44;
			item.autoReuse = false;

			// These below are needed for a sentry weapon
			item.noMelee = true;
			item.summon = true;
			item.sentry = true;
			item.shoot = ModContent.ProjectileType<SlimeTrap>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Gel, 20);
			recipe.AddIngredient(ItemID.RichMahogany, 10);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}