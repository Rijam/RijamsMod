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

namespace RijamsMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class DilapidatedCrimsonHelmet : ModItem
	{
		
		public override bool Autoload(ref string name)
		{
			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dilapidated Crimson Helmet");
			Tooltip.SetDefault("2% increased damage\nInterchangeable with Crimson Armor");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.CrimsonHelmet);
			item.headSlot = mod.GetEquipSlot("DilapidatedCrimsonHelmet", EquipType.Head); //Will draw the correct equip sprite, but then it's not interchangable with Crimson Armor and doesn't have the effects
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			//The Crimson Helmet doesn't seem to work.
			return (head.type == ItemID.CrimsonHelmet || head.type == ModContent.ItemType<DilapidatedCrimsonHelmet>()) && (body.type == ItemID.CrimsonScalemail || body.type == ModContent.ItemType<DilapidatedCrimsonScalemail>()) && (legs.type == ItemID.CrimsonGreaves || legs.type == ModContent.ItemType<DilapidatedCrimsonGreaves>());
		}

		public override void UpdateEquip(Player player)
		{
			player.allDamage += 0.02f;
			int maxValue = 10;
			if (Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y) > 1f)
			{
				maxValue = 2;
			}
			if (Main.rand.Next(maxValue) == 0)
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 15);
			recipe.AddIngredient(ItemID.TissueSample, 10);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class DrawDilapidatedCrimsonHelmet : EquipTexture
    {
		public override bool DrawHead()
		{
			return true;
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class DilapidatedCrimsonScalemail : ModItem
	{
		public override void SetStaticDefaults()
		{
			//base.SetStaticDefaults();
			DisplayName.SetDefault("Dilapidated Crimson Scalemail");
			Tooltip.SetDefault("2% increased damage\nInterchangeable with Crimson Armor");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.CrimsonScalemail);
			item.bodySlot = mod.GetEquipSlot("DilapidatedCrimsonScalemail", EquipType.Body);
		}

		public override void UpdateEquip(Player player)
		{
			player.allDamage += 0.02f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 20);
			recipe.AddIngredient(ItemID.TissueSample, 15);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class DrawDilapidatedCrimsonScalemail : EquipTexture
	{
		public override bool DrawBody()
		{
			return true;
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class DilapidatedCrimsonGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dilapidated Crimson Greaves");
			Tooltip.SetDefault("2% increased damage\nInterchangeable with Crimson Armor");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.CrimsonGreaves);
			item.legSlot = mod.GetEquipSlot("DilapidatedCrimsonGreaves", EquipType.Legs);
		}

		public override void UpdateEquip(Player player)
		{
			player.allDamage += 0.02f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 25);
			recipe.AddIngredient(ItemID.TissueSample, 20);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class DrawDilapidatedCrimsonGreaves : EquipTexture
	{
		public override bool DrawLegs()
		{
			return false;
		}
	}
}