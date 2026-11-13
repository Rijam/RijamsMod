using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RijamsMod.Items.Armor.Skyware;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.RedSkyware
{
	[AutoloadEquip(EquipType.Head)]
	public class RedSkywareMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Skyware Mask");
			// Tooltip.SetDefault("+10% Melee damage\n+10% Melee critical strike chance\n+5 Melee armor penetration\n+10% Melee weapon scale");
			AddArmorSet<RedSkywareMask, RedSkywareChestplate, RedSkywareLeggings>
			(
				LocalizedText: Language.GetText($"Mods.{Mod.Name}.ArmorSetBonus.RedSkyware"),
				PrimaryPart: ArmorSetBonus.PartType.None,
				Identifier: "RijamsMod/RedSkyware_Melee",
				Effect: SetBonus_RedSkyware_Melee
			);
		}

		public override void SetDefaults()
		{
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;//8
			Item.defense = 30;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.1f;
			player.GetCritChance(DamageClass.Melee) += 10f;
			player.GetArmorPenetration(DamageClass.Melee) += 5f;
			// Scale set int GlobalAccessoryItems.cs
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 1)
				.AddIngredient(ModContent.ItemType<SkywareMask>())
				.AddIngredient(ItemID.ChlorophyteBar, 2)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.SkyMill)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 1)
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 20)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 20)
				.AddTile(TileID.SkyMill)
				.Register();
		}

		public static void SetBonus_RedSkyware_Melee(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.Armor_SkywareArmorSetBonus = SkywareArmorSetPlayer.SetBonusType.RedSkywareMelee;
			if (modPlayer.Armor_SkywareArmorSetBonusTimer > 0)
			{
				modPlayer.Armor_SkywareArmorSetBonusTimer--;
			}
		}

		// Changing the equipSlot in the leggings breaks the shadows
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class RedSkywareHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Skyware Helmet");
			// Tooltip.SetDefault("+10% Ranged damage\n+10% Ranged critical strike chance\n+5 Ranged armor penetration\n+10% projectile speed and knockback");
			AddArmorSet<RedSkywareHelmet, RedSkywareChestplate, RedSkywareLeggings>
			(
				LocalizedText: Language.GetText($"Mods.{Mod.Name}.ArmorSetBonus.RedSkyware"),
				PrimaryPart: ArmorSetBonus.PartType.None,
				Identifier: "RijamsMod/RedSkyware_Ranged",
				Effect: SetBonus_RedSkyware_Ranged
			);
		}

		public override void SetDefaults()
		{
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;//8
			Item.defense = 20;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Ranged) += 0.1f;
			player.GetCritChance(DamageClass.Ranged) += 10f;
			player.GetArmorPenetration(DamageClass.Ranged) += 5f;
			// Projectile speed and knockback in GlobalAccessoryItems.cs
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 1)
				.AddIngredient(ModContent.ItemType<SkywareHelmet>())
				.AddIngredient(ItemID.ChlorophyteBar, 2)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.SkyMill)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 1)
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 20)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 20)
				.AddTile(TileID.SkyMill)
				.Register();
		}

		public static void SetBonus_RedSkyware_Ranged(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.Armor_SkywareArmorSetBonus = SkywareArmorSetPlayer.SetBonusType.RedSkywareRanged;
			if (modPlayer.Armor_SkywareArmorSetBonusTimer > 0)
			{
				modPlayer.Armor_SkywareArmorSetBonusTimer--;
			}
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class RedSkywareHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Skyware Hood");
			// Tooltip.SetDefault("+10% Magic damage\n+10% Magic critical strike chance\n+40 Mana capacity\n-20% Mana cost");
			AddArmorSet<RedSkywareHood, RedSkywareChestplate, RedSkywareLeggings>
			(
				LocalizedText: Language.GetText($"Mods.{Mod.Name}.ArmorSetBonus.RedSkyware"),
				PrimaryPart: ArmorSetBonus.PartType.None,
				Identifier: "RijamsMod/RedSkyware_Magic",
				Effect: SetBonus_RedSkyware_Magic
			);
		}

		public override void SetDefaults()
		{
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;//8
			Item.defense = 14;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.1f;
			player.GetCritChance(DamageClass.Magic) += 10f;
			player.statManaMax2 += 40;
			player.manaCost -= 0.2f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 1)
				.AddIngredient(ModContent.ItemType<SkywareHood>())
				.AddIngredient(ItemID.ChlorophyteBar, 2)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.SkyMill)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 1)
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 20)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 20)
				.AddTile(TileID.SkyMill)
				.Register();
		}

		public static void SetBonus_RedSkyware_Magic(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.Armor_SkywareArmorSetBonus = SkywareArmorSetPlayer.SetBonusType.RedSkywareMagic;
			if (modPlayer.Armor_SkywareArmorSetBonusTimer > 0)
			{
				modPlayer.Armor_SkywareArmorSetBonusTimer--;
			}
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class RedSkywareHeadgear : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Skyware Headgear");
			// Tooltip.SetDefault("+10% Summon damage\n+3 minion capacity\n+1 sentry capacity\n+7 Support minion radius");
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
			AddArmorSet<RedSkywareHeadgear, RedSkywareChestplate, RedSkywareLeggings>
			(
				LocalizedText: Language.GetText($"Mods.{Mod.Name}.ArmorSetBonus.RedSkyware"),
				PrimaryPart: ArmorSetBonus.PartType.None,
				Identifier: "RijamsMod/RedSkyware_Summon",
				Effect: SetBonus_RedSkyware_Summon
			);
			ArmorIDs.Head.Sets.CanDrawOnVelociraptorMount[Item.headSlot] = true;
			ArmorIDs.Head.Sets.VelociraptorMountHeadOffset[Item.headSlot] = new Vector2(0, -2);
		}

		public override void SetDefaults()
		{
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;//8
			Item.defense = 10;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.1f;
			player.maxMinions += 3;
			player.maxTurrets++;
			player.GetModPlayer<RijamsModPlayer>().Stat_SupportMinionRadiusIncrease += 7;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 1)
				.AddIngredient(ModContent.ItemType<SkywareHeadgear>())
				.AddIngredient(ItemID.ChlorophyteBar, 2)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.SkyMill)
				.Register();

			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.GiantRedHarpyFeather>(), 1)
				.AddIngredient(ModContent.ItemType<Placeable.SunplatePillarBlock>(), 20)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 20)
				.AddTile(TileID.SkyMill)
				.Register();
		}

		public static void SetBonus_RedSkyware_Summon(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.Armor_SkywareArmorSetBonus = SkywareArmorSetPlayer.SetBonusType.RedSkywareSummon;
			if (modPlayer.Armor_SkywareArmorSetBonusTimer > 0)
			{
				modPlayer.Armor_SkywareArmorSetBonusTimer--;
			}
		}


		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class RedSkywareChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Skyware Chestplate");
			// Tooltip.SetDefault("+15% all attack speed\n+10% all knockback\n+20 max life");
		}

		public override void SetDefaults()
		{
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 16;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
			player.GetKnockback(DamageClass.Generic) += 0.1f;
			player.statLifeMax2 += 20;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SkywareChestplate>())
				.AddIngredient(ItemID.ChlorophyteBar, 2)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.SkyMill)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.Feather, 5)
				.AddIngredient(ItemID.SunplateBlock, 20)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 20)
				.AddTile(TileID.SkyMill)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class RedSkywareLeggings : ModItem
	{
		public Asset<Texture2D> LegEquipTextureFemale;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Skyware Leggings");
			// Tooltip.SetDefault("+1 Second Flight Time\n+20% movement speed\nNegates fall damage");
		}

		public override void SetDefaults()
		{
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 14;
		}

		public override void UpdateEquip(Player player)
		{
			player.noFallDmg = true;
			player.GetModPlayer<RijamsModPlayer>().Armor_RedSkywareLeggings = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SkywareLeggings>())
				.AddIngredient(ItemID.ChlorophyteBar, 2)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 10)
				.AddTile(TileID.SkyMill)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.Cloud, 10)
				.AddIngredient(ItemID.SunplateBlock, 20)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ModContent.ItemType<Materials.SunEssence>(), 20)
				.AddTile(TileID.SkyMill)
				.Register();
		}

		public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
		{
			if (!drawInfo.drawPlayer.Male)
			{
				LegEquipTextureFemale ??= ModContent.Request<Texture2D>((GetType().Namespace + "." + Name).Replace('.', '/') + "_FemaleLegs");

				drawData.texture = LegEquipTextureFemale.Value;
			}

			return base.ModifyEquipTextureDraw(ref drawInfo, ref drawData, equipTexture, methodName);
		}
	}
}