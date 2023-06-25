using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Melee
{
	public class StabbyShiv : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stabby Shiv");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Mister Stabby]", null, null });
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Melee;
			Item.width = 18;
			Item.height = 18;
			Item.useTime = 7; 
			Item.useAnimation = 7;
			Item.knockBack = 8;
			Item.value = Item.buyPrice(gold: 2);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = false;
			Item.crit = 10;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
			Item.noMelee = true; // The projectile will do the damage and not the item

			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.StabbyShivProjectile>(); // The projectile is what makes a shortsword work
			Item.shootSpeed = 2.1f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
		}

		public override void HoldItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime != 0)
			{
				player.GetModPlayer<RijamsModPlayer>().criticalHitAdditionalDamage += 0.25f;
			}
		}
	}
	
	public class FrostyShiv : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Frosty Shiv");
			// Tooltip.SetDefault("Inflicts Frostburn"); 
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Melee;
			Item.width = 20;
			Item.height = 20;
			Item.useTime = 7; 
			Item.useAnimation = 7;
			Item.knockBack = 8;
			Item.value = 22000;
			Item.rare = ItemRarityID.Pink; // 5
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = false; //Looks weird if true
			Item.crit = 10;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
			Item.noMelee = true; // The projectile will do the damage and not the item
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
			}

			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.FrostyShivProjectile>(); // The projectile is what makes a shortsword work
			Item.shootSpeed = 2.1f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
		}

		public override void HoldItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime != 0)
			{
				player.GetModPlayer<RijamsModPlayer>().criticalHitAdditionalDamage += 0.25f;
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<StabbyShiv>())
				.AddIngredient(ItemID.IceTorch, 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
