using Microsoft.Xna.Framework;
using RijamsMod.Items.Armor.Skyware;
using System;
using Terraria;
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
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
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

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<RedSkywareChestplate>() && legs.type == ModContent.ItemType<RedSkywareLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods." + Mod.Name + ".ArmorSetBonus.RedSkyware");
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.skywareArmorSetBonus = 5;
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
	public class RedSkywareHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Skyware Helmet");
			// Tooltip.SetDefault("+10% Ranged damage\n+10% Ranged critical strike chance\n+5 Ranged armor penetration\n+10% projectile speed and knockback");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
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
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == Type && body.type == ModContent.ItemType<RedSkywareChestplate>() && legs.type == ModContent.ItemType<RedSkywareLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.skywareArmorSetBonus = 6;
			player.setBonus = Language.GetTextValue("Mods." + Mod.Name + ".ArmorSetBonus.RedSkyware");
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
	public class RedSkywareHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Skyware Hood");
			// Tooltip.SetDefault("+10% Magic damage\n+10% Magic critical strike chance\n+40 Mana capacity\n-20% Mana cost");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
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
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == Type && body.type == ModContent.ItemType<RedSkywareChestplate>() && legs.type == ModContent.ItemType<RedSkywareLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.skywareArmorSetBonus = 7;
			player.setBonus = Language.GetTextValue("Mods." + Mod.Name + ".ArmorSetBonus.RedSkyware");
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
	public class RedSkywareHeadgear : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Skyware Headgear");
			// Tooltip.SetDefault("+10% Summon damage\n+3 minion capacity\n+1 sentry capacity\n+7 Support minion radius");
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 14;
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;//8
			Item.defense = 10;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.1f;
			player.maxMinions += 3;
			player.maxTurrets++;
			player.GetModPlayer<RijamsModPlayer>().supportMinionRadiusIncrease += 7;
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
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == Type && body.type == ModContent.ItemType<RedSkywareChestplate>() && legs.type == ModContent.ItemType<RedSkywareLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			RijamsModPlayer modPlayer = player.GetModPlayer<RijamsModPlayer>();
			modPlayer.skywareArmorSetBonus = 8;
			player.setBonus = Language.GetTextValue("Mods." + Mod.Name + ".ArmorSetBonus.RedSkyware");
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
	public class RedSkywareChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Skyware Chestplate");
			// Tooltip.SetDefault("+15% all attack speed\n+10% all knockback\n+20 max life");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 20;
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
			// DisplayName.SetDefault("Red Skyware Leggings");
			// Tooltip.SetDefault("+1 Second Flight Time\n+20% movement speed\nNegates fall damage");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 14;
		}

		public override void UpdateEquip(Player player)
		{
			player.noFallDmg = true;
			player.GetModPlayer<RijamsModPlayer>().redSkywareLeggings = true;
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

		// Changing the equipSlot in the leggings breaks the shadows. Sorry ladies!
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			if (!male) equipSlot = LegEquipTextureFemale;
		}
	}
}