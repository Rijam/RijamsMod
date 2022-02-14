using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class InfernicHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernic Hood");
			Tooltip.SetDefault("+60 Max mana\n+1 Minion count");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 24;
			item.value = 20000;
			item.rare = ItemRarityID.Orange;//3
			item.defense = 7;
		}
		public override void UpdateEquip(Player player)
		{
			player.statManaMax2 += 60;
			player.maxMinions++;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<InfernicRobes>() && legs.type == ModContent.ItemType<InfernicTrousers>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "\nGrants immunity to fire blocks\nGrants immunity to On Fire!\nProvides 2 seconds of immunity to lava";
			player.buffImmune[BuffID.OnFire] = true;
			player.fireWalk = true;
			player.lavaMax += 120;
			if (Math.Abs(player.velocity.X) > 2 || Math.Abs(player.velocity.Y) > 2)
            {
				float x2 = player.position.X - player.velocity.X / 10f;
				float y2 = player.position.Y - player.velocity.Y / 10f;
				Dust.NewDust(new Vector2(x2,y2), player.width / 2, player.height, DustID.Ash, 0f, 0f, 100, default, 0.5f);
			}
			
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}
		//Glowmasks adapted from SGAmod by IDGCaptainRussia94; See ArmorUseGlow
		public Color ArmorGlow(Player player, int index) { return Color.White; }
		public override void UpdateVanity(Player player, EquipType type)
		{
			ArmorUseGlow modplayer = player.GetModPlayer(mod, typeof(ArmorUseGlow).Name) as ArmorUseGlow;
			if (!Main.dedServ)
			{
				modplayer.armorglowmasks[0] = "RijamsMod/Items/GlowMasks/" + Name + "_Head" + "_Glow";
				modplayer.armorglowcolor[0] = ArmorGlow;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 8);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.InfernicFabric>(), 7);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class InfernicRobes : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernic Robes");
			Tooltip.SetDefault("+10% Magic critical strike chance\n+12% Magic and summon damage");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 20;
			item.value = 20000;
			item.rare = ItemRarityID.Orange;
			item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicCrit += 10;
			player.magicDamage += 0.12f;
			player.minionDamage += 0.12f;
		}
		//Glowmasks adapted from SGAmod by IDGCaptainRussia94; See ArmorUseGlow
		public Color ArmorGlow(Player player, int index) { return Color.White; }
		public override void UpdateVanity(Player player, EquipType type)
		{
			ArmorUseGlow modplayer = player.GetModPlayer(mod, typeof(ArmorUseGlow).Name) as ArmorUseGlow;
			if (!Main.dedServ)
			{
				modplayer.armorglowmasks[1] = "RijamsMod/Items/GlowMasks/" + Name + "_Body" + "_Glow";
				modplayer.armorglowmasks[2] = "RijamsMod/Items/GlowMasks/" + Name + "_Arms" + "_Glow";
				modplayer.armorglowmasks[4] = "RijamsMod/Items/GlowMasks/" + Name + "_FemaleBody" + "_Glow";
				modplayer.armorglowcolor[1] = ArmorGlow;
				modplayer.armorglowcolor[2] = ArmorGlow;
				modplayer.armorglowcolor[4] = ArmorGlow;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 16);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.InfernicFabric>(), 10);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class InfernicTrousers : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernic Trousers");
			Tooltip.SetDefault("+5% Movement speed\n10% reduced mana usage\n+1 mana regeneration bonus");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = 20000;
			item.rare = ItemRarityID.Orange;
			item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.05f;
			player.manaCost -= 0.1f;
			player.manaRegenBonus++;
		}
		//Glowmasks adapted from SGAmod by IDGCaptainRussia94; See ArmorUseGlow
		public Color ArmorGlow(Player player, int index) { return Color.White; }
		public override void UpdateVanity(Player player, EquipType type)
		{
			ArmorUseGlow modplayer = player.GetModPlayer(mod, typeof(ArmorUseGlow).Name) as ArmorUseGlow;
			if (!Main.dedServ)
            {
				modplayer.armorglowmasks[3] = "RijamsMod/Items/GlowMasks/" + Name + "_Legs" + "_Glow";
				modplayer.armorglowcolor[3] = ArmorGlow;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.InfernicFabric>(), 8);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}