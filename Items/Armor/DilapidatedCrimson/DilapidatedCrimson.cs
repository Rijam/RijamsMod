using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.DilapidatedCrimson
{
	[AutoloadEquip(EquipType.Head)]
	public class DilapidatedCrimsonHelmet : ModItem
	{
		public override void Load()
		{
			if (!Main.dedServ)
			{
				EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_Head", EquipType.Head, this, "DilapidatedCrimsonHelmet");
			}
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dilapidated Crimson Helmet");
			// Tooltip.SetDefault("2% increased damage\nInterchangeable with Crimson Armor");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Crimeras]", null, null });
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.CrimsonHelmet; // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.CrimsonHelmet);
			Item.headSlot = EquipLoader.GetEquipSlot(Mod, "DilapidatedCrimsonHelmet", EquipType.Head); //Will draw the correct equip sprite, but then it's not interchangable with Crimson Armor and doesn't have the effects
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			//The Crimson Helmet doesn't seem to work.
			return (head.type == ItemID.CrimsonHelmet || head.type == ModContent.ItemType<DilapidatedCrimsonHelmet>()) && (body.type == ItemID.CrimsonScalemail || body.type == ModContent.ItemType<DilapidatedCrimsonScalemail>()) && (legs.type == ItemID.CrimsonGreaves || legs.type == ModContent.ItemType<DilapidatedCrimsonGreaves>());
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.02f;
			int maxValue = 10;
			if (Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y) > 1f)
			{
				maxValue = 2;
			}
			if (Main.rand.NextBool(maxValue))
			{
				int num6 = Dust.NewDust(player.position, player.width, player.height, DustID.CrimtaneWeapons, 0f, 0f, 140, default, 0.75f);
				Main.dust[num6].noGravity = true;
				Main.dust[num6].fadeIn = 1.5f;
				Main.dust[num6].velocity *= 0.3f;
				Main.dust[num6].velocity += player.velocity * 0.2f;
				Main.dust[num6].shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
			}
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("ArmorSetBonus.Crimson");
			player.crimsonRegen = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CrimtaneBar, 15)
				.AddIngredient(ItemID.TissueSample, 10)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class DilapidatedCrimsonScalemail : ModItem
	{
		public override void Load()
		{
			if (!Main.dedServ)
			{
				EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_Body", EquipType.Body, this, "DilapidatedCrimsonScalemail");
			}
		}
		public override void SetStaticDefaults()
		{
			//base.SetStaticDefaults();
			// DisplayName.SetDefault("Dilapidated Crimson Scalemail");
			// Tooltip.SetDefault("2% increased damage\nInterchangeable with Crimson Armor");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Crimeras]", null, null });
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.CrimsonScalemail; // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.CrimsonScalemail);
			Item.bodySlot = EquipLoader.GetEquipSlot(Mod, "DilapidatedCrimsonScalemail", EquipType.Body);
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.02f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CrimtaneBar, 25)
				.AddIngredient(ItemID.TissueSample, 20)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class DilapidatedCrimsonGreaves : ModItem
	{
		public override void Load()
		{
			if (!Main.dedServ)
			{
				EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_Legs", EquipType.Legs, this, "DilapidatedCrimsonGreaves");
			}
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dilapidated Crimson Greaves");
			// Tooltip.SetDefault("2% increased damage\nInterchangeable with Crimson Armor");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Crimeras]", null, null });
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.CrimsonGreaves; // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.CrimsonGreaves);
			Item.legSlot = EquipLoader.GetEquipSlot(Mod, "DilapidatedCrimsonGreaves", EquipType.Legs);
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.02f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CrimtaneBar, 20)
				.AddIngredient(ItemID.TissueSample, 15)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}