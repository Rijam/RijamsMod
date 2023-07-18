using RijamsMod.Projectiles.Summon.Sentries;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace RijamsMod.Items.Weapons.Summon.Sentries
{
	public class SpikeTrapStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spike Trap Staff");
			// Tooltip.SetDefault("Summons a sentry Spike Trap\nDeals damage every 0.5 seconds");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.knockBack = 0f;
			Item.mana = 10;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item44;
			Item.autoReuse = false;

			// These below are needed for a sentry weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.shoot = ModContent.ProjectileType<SpikeTrap>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position = Main.MouseWorld;
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			projectile.originalDamage = Item.damage;
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
			CreateRecipe()
				.AddIngredient(ItemID.Spike, 9)
				.AddIngredient(ItemID.Bone, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	public class SuperSpikeTrapStaff : SpikeTrapStaff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Super Spike Trap Staff");
			// Tooltip.SetDefault("Summons a sentry Super Spike Trap\nDeals damage every 0.25 seconds");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 35;
			Item.width = 48;
			Item.height = 48;
			Item.value = Item.sellPrice(gold: 1, silver: 50);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item44;
			Item.autoReuse = true;

			// These below are needed for a sentry weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.shoot = ModContent.ProjectileType<SuperSpikeTrap>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SpikeTrapStaff>(), 1)
				.AddIngredient(ItemID.LihzahrdBrick, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	public class SlimeTrapStaff : SpikeTrapStaff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Slime Trap Staff");
			// Tooltip.SetDefault("Summons a sentry Slime Trap\nReduces enemy movement speed by 25%");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 1; // Token amount of damage so it can be reforged and so it shows up in summon weapon filters.
			Item.width = 28;
			Item.height = 30;
			Item.value = Item.sellPrice(silver: 50);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item44;
			Item.autoReuse = false;

			// These below are needed for a sentry weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.shoot = ModContent.ProjectileType<SlimeTrap>();
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Gel, 20)
				.AddIngredient(ItemID.RichMahogany, 10)
				.AddIngredient(ItemID.FallenStar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			damage -= 1;
		}
	}
}