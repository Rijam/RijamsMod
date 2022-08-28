using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.Skyware
{
	[AutoloadEquip(EquipType.Head)]
	public class SkywareMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyware Mask");
			Tooltip.SetDefault("+5% Melee damage\n+5% Melee critical strike chance");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = 2000;
			Item.rare = ItemRarityID.Blue;//1
			Item.defense = 8;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.05f;
			player.GetCritChance(DamageClass.Melee) += 5f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.GiantHarpyFeather, 1)
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 20)
				.AddRecipeGroup("RijamsMod:GoldBars", 5)
				.AddRecipeGroup("RijamsMod:EvilBars", 2)
				.AddTile(TileID.SkyMill)
				.Register();
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<SkywareChestplate>() && legs.type == ModContent.ItemType<SkywareLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods." + Mod.Name + ".ArmorSetBonus.Skyware");
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.skywareArmorSetBonus = 1;
			if (modPlayer.skywareArmorSetBonusTimer > 0)
			{
				modPlayer.skywareArmorSetBonusTimer--;
			}
		}

		// Changing the equipSlot in the leggings breaks the shadows
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class SkywareHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyware Helmet");
			Tooltip.SetDefault("+5% Ranged damage\n+5% Ranged critical strike chance");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = 2000;
			Item.rare = ItemRarityID.Blue;//1
			Item.defense = 6;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Ranged) += 0.05f;
			player.GetCritChance(DamageClass.Ranged) += 5f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.GiantHarpyFeather, 1)
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 20)
				.AddRecipeGroup("RijamsMod:GoldBars", 5)
				.AddRecipeGroup("RijamsMod:EvilBars", 2)
				.AddTile(TileID.SkyMill)
				.Register();
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == Type && body.type == ModContent.ItemType<SkywareChestplate>() && legs.type == ModContent.ItemType<SkywareLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.skywareArmorSetBonus = 2;
			player.setBonus = Language.GetTextValue("Mods." + Mod.Name + ".ArmorSetBonus.Skyware");
			if (modPlayer.skywareArmorSetBonusTimer > 0)
			{
				modPlayer.skywareArmorSetBonusTimer--;
			}
			ArmorSetShadows(player);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class SkywareHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyware Hood");
			Tooltip.SetDefault("+5% Magic damage\n+5% Magic critical strike chance");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = 2000;
			Item.rare = ItemRarityID.Blue;//1
			Item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.05f;
			player.GetCritChance(DamageClass.Magic) += 5f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.GiantHarpyFeather, 1)
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 20)
				.AddRecipeGroup("RijamsMod:GoldBars", 5)
				.AddRecipeGroup("RijamsMod:EvilBars", 2)
				.AddTile(TileID.SkyMill)
				.Register();
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == Type && body.type == ModContent.ItemType<SkywareChestplate>() && legs.type == ModContent.ItemType<SkywareLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.skywareArmorSetBonus = 3;
			player.setBonus = Language.GetTextValue("Mods." + Mod.Name + ".ArmorSetBonus.Skyware");
			if (modPlayer.skywareArmorSetBonusTimer > 0)
			{
				modPlayer.skywareArmorSetBonusTimer--;
			}
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class SkywareHeadgear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyware Headgear");
			Tooltip.SetDefault("+5% Summon damage\n+1 summon capacity");
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 14;
			Item.value = 2000;
			Item.rare = ItemRarityID.Blue;//1
			Item.defense = 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.05f;
			player.maxMinions++;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.GiantHarpyFeather, 1)
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 20)
				.AddRecipeGroup("RijamsMod:GoldBars", 5)
				.AddRecipeGroup("RijamsMod:EvilBars", 2)
				.AddTile(TileID.SkyMill)
				.Register();
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == Type && body.type == ModContent.ItemType<SkywareChestplate>() && legs.type == ModContent.ItemType<SkywareLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.skywareArmorSetBonus = 4;
			player.setBonus = Language.GetTextValue("Mods." + Mod.Name + ".ArmorSetBonus.Skyware");
			if (modPlayer.skywareArmorSetBonusTimer > 0)
			{
				modPlayer.skywareArmorSetBonusTimer--;
			}
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class SkywareChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyware Chestplate");
			Tooltip.SetDefault("+10% all attack speed");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 20;
			Item.value = 2000;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Feather, 5)
				.AddIngredient(ItemID.SunplateBlock, 20)
				.AddRecipeGroup("RijamsMod:GoldBars", 5)
				.AddRecipeGroup("RijamsMod:EvilBars", 2)
				.AddTile(TileID.SkyMill)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class SkywareLeggings : ModItem
	{
		// Thanks Exterminator for the help
		public int LegEquipTextureFemale;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTextureFemale = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_FemaleLegs", EquipType.Legs, this);
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyware Leggings");
			Tooltip.SetDefault("Increased fall resistance");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 20000;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.extraFall += 15;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Cloud, 10)
				.AddIngredient(ItemID.SunplateBlock, 20)
				.AddRecipeGroup("RijamsMod:GoldBars", 5)
				.AddRecipeGroup("RijamsMod:EvilBars", 2)
				.AddTile(TileID.SkyMill)
				.Register();
		}

		// Changing the equipSlot in the leggings breaks the shadows. Sorry ladies!
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			if (!male) equipSlot = LegEquipTextureFemale;
		}
	}
	/*public class SkywareArmorSetItem : GlobalItem
	{
		public override string IsArmorSet(Item head, Item body, Item legs)
		{
			if ((head.type == ModContent.ItemType<SkywareMask>() || head.type == ModContent.ItemType<SkywareHelmet>() ||
				head.type == ModContent.ItemType<SkywareHood>() || head.type == ModContent.ItemType<SkywareHeadgear>()) 
				&& body.type == ModContent.ItemType<SkywareChestplate>() && legs.type == ModContent.ItemType<SkywareLeggings>())
			{
				return "SkywareArmor";
			}
			return base.IsArmorSet(head, body, legs);
		}
		public override void UpdateArmorSet(Player player, string set)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			if (set == "SkywareMask")
			{
				modPlayer.skywareArmorSetBonus = 1;
				player.setBonus = "\nShoot feathers at your enemies";
				if (modPlayer.skywareArmorSetBonusTimer > 0)
				{
					modPlayer.skywareArmorSetBonusTimer--;
				}
				ArmorSetShadows(player, "SkywareMask");
			}
			if (set == "SkywareHelmet")
			{
				modPlayer.skywareArmorSetBonus = 2;
				player.setBonus = "\nShoot feathers at your enemies";
				if (modPlayer.skywareArmorSetBonusTimer > 0)
				{
					modPlayer.skywareArmorSetBonusTimer--;
				}
				ArmorSetShadows(player, "SkywareHelmet");
			}
			if (set == "SkywareHood")
			{
				modPlayer.skywareArmorSetBonus = 3;
				player.setBonus = "\nShoot feathers at your enemies";
				if (modPlayer.skywareArmorSetBonusTimer > 0)
				{
					modPlayer.skywareArmorSetBonusTimer--;
				}
				ArmorSetShadows(player, "SkywareHood");
			}
			if (set == "SkywareHeadgear")
			{
				modPlayer.skywareArmorSetBonus = 4;
				player.setBonus = "\nShoot feathers at your enemies";
				if (modPlayer.skywareArmorSetBonusTimer > 0)
				{
					modPlayer.skywareArmorSetBonusTimer--;
				}
				ArmorSetShadows(player, "SkywareHeadgear");
			}
		}
		public override void ArmorSetShadows(Player player, string set)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();

			Main.NewText("modPlayer.skywareArmorSetBonus " + modPlayer.skywareArmorSetBonus);
			Main.NewText("set " + set);
			if (set == "SkywareMask" || modPlayer.skywareArmorSetBonus == 1)
			{
				player.armorEffectDrawOutlines = true;
			}
			if (set == "SkywareHelmet" || modPlayer.skywareArmorSetBonus == 2)
			{
				player.armorEffectDrawOutlinesForbidden = true;
			}
			if (set == "SkywareHood" || modPlayer.skywareArmorSetBonus == 3)
			{
				player.armorEffectDrawShadowBasilisk = true;
			}
			if (set == "SkywareHeadgear" || modPlayer.skywareArmorSetBonus == 4)
			{
				player.armorEffectDrawShadowEOCShield = true;
			}
		}
	}*/
	public class SkywareArmorSetPlayer : ModPlayer
	{
		public override void OnHitAnything(float x, float y, Entity victim)
		{
			if (Main.myPlayer != Player.whoAmI)
			{
				return;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				RijamsModPlayer modPlayer = Player.GetModPlayer<RijamsModPlayer>();
				if (modPlayer.skywareArmorSetBonus > 0 && modPlayer.skywareArmorSetBonusTimer == 0)
				{
					modPlayer.skywareArmorSetBonusTimer = 60;

					Vector2 direction = Utils.DirectionTo(Player.Center, new Vector2(x, y));
					Vector2 velocity = direction * 20f;
					velocity *= Main.rand.NextFloat(0.7f, 1.6f);

					switch (modPlayer.skywareArmorSetBonus)
					{
						case 1:
							int damageMelee = (int)Math.Round(20 * Player.GetTotalDamage(DamageClass.Melee).Additive * Player.GetTotalDamage(DamageClass.Melee).Multiplicative);
							int projMelee = Projectile.NewProjectile(
								Entity.GetSource_OnHit(victim),
								Player.Center,
								velocity,
								ModContent.ProjectileType<Projectiles.Misc.SkywareArmorHarpyFeather>(),
								damageMelee,
								Player.HeldItem != null ? Player.GetWeaponKnockback(Player.HeldItem) : 0f,
								Player.whoAmI);
							Main.projectile[projMelee].DamageType = DamageClass.Melee;
							break;
						case 2:
							int damageRanged = (int)Math.Round(20 * Player.GetTotalDamage(DamageClass.Ranged).Additive * Player.GetTotalDamage(DamageClass.Ranged).Multiplicative);
							int projRanged = Projectile.NewProjectile(
								Entity.GetSource_OnHit(victim),
								Player.Center,
								velocity,
								ModContent.ProjectileType<Projectiles.Misc.SkywareArmorHarpyFeather>(),
								damageRanged,
								Player.HeldItem != null ? Player.GetWeaponKnockback(Player.HeldItem) : 0f,
								Player.whoAmI);
							Main.projectile[projRanged].DamageType = DamageClass.Ranged;
							break;
						case 3:
							int damageMagic = (int)Math.Round(20 * Player.GetTotalDamage(DamageClass.Magic).Additive * Player.GetTotalDamage(DamageClass.Magic).Multiplicative);
							int projMagic = Projectile.NewProjectile(
								Entity.GetSource_OnHit(victim),
								Player.Center,
								velocity,
								ModContent.ProjectileType<Projectiles.Misc.SkywareArmorHarpyFeather>(),
								damageMagic,
								Player.HeldItem != null ? Player.GetWeaponKnockback(Player.HeldItem) : 0f,
								Player.whoAmI);
							Main.projectile[projMagic].DamageType = DamageClass.Magic;
							break;
						case 4:
							int damageSummon = (int)Math.Round(20 * Player.GetTotalDamage(DamageClass.Summon).Additive * Player.GetTotalDamage(DamageClass.Summon).Multiplicative);
							int projSummon = Projectile.NewProjectile(
								Entity.GetSource_OnHit(victim),
								Player.Center,
								velocity,
								ModContent.ProjectileType<Projectiles.Misc.SkywareArmorHarpyFeather>(),
								damageSummon,
								Player.HeldItem != null ? Player.GetWeaponKnockback(Player.HeldItem) : 0f,
								Player.whoAmI);
							Main.projectile[projSummon].DamageType = DamageClass.Summon;
							break;
						default:
							break;
					}
				}
			}
		}
	}
}