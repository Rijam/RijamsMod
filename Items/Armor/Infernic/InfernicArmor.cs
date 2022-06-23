using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.Infernic
{
	[AutoloadEquip(EquipType.Head)]
	public class InfernicHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernic Hood");
			Tooltip.SetDefault("+60 Max mana\n+1 Minion count");
			if (!Main.dedServ)
			{
				ArmorUseGlowHead.RegisterData(Item.headSlot, new string[] { Texture + "_Head_Glowmask", "255", "255", "255", "none" });
			}
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = 20000;
			Item.rare = ItemRarityID.Orange;//3
			Item.defense = 7;
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
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 8)
				.AddIngredient(ModContent.ItemType<Materials.InfernicFabric>(), 7)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class InfernicRobes : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernic Robes");
			Tooltip.SetDefault("+10% Magic critical strike chance\n+12% Magic and summon damage");
			if (!Main.dedServ)
			{
				ArmorUseGlowBody.RegisterData(Item.bodySlot, Color.White);
			}
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 20;
			Item.value = 20000;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Magic) += 10;
			player.GetDamage(DamageClass.Magic) += 0.12f;
			player.GetDamage(DamageClass.Summon) += 0.12f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 16)
				.AddIngredient(ModContent.ItemType<Materials.InfernicFabric>(), 10)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class InfernicTrousers : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernic Trousers");
			Tooltip.SetDefault("+5% Movement speed\n10% reduced mana usage\n+1 mana regeneration bonus");
			if (!Main.dedServ)
			{
				ArmorUseGlowLegs.RegisterData(Item.legSlot, new string[] { Texture + "_Legs_Glowmask", "255", "255", "255", "none" });
			}
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 20000;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.05f;
			player.manaCost -= 0.1f;
			player.manaRegenBonus++;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 10)
				.AddIngredient(ModContent.ItemType<Materials.InfernicFabric>(), 8)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}
}