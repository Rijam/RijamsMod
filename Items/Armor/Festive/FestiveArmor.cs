using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.Festive
{
	[AutoloadEquip(EquipType.Head)]
	public class FestiveMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Festive Mask");
			Tooltip.SetDefault("+1 Minion capacity\n+15% Summon damage");
			if (!Main.dedServ)
			{
				ArmorUseGlowHead.RegisterData(Item.headSlot, new string[] { Texture + "_Head_Glowmask", "255", "255", "255", "lerpOnOff" });
			}
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;//8
			Item.defense = 10;
		}
		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
			player.GetDamage(DamageClass.Summon) += 0.15f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<FestiveChestplate>() && legs.type == ModContent.ItemType<FestiveLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "\n+35% Whip speed\n+50% Whip range\n+5% whip damage\n+2% whip critical strike chance";
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.35f;
			player.whipRangeMultiplier += 0.5f;
			player.GetDamage(DamageClass.SummonMeleeSpeed) += 0.05f;
			player.GetCritChance(DamageClass.SummonMeleeSpeed) += 0.02f; // Don't even know if this works lol. It's so low that hopefully nobody will notice if it doesn't work.
			if (Math.Abs(player.velocity.X) > 2 || Math.Abs(player.velocity.Y) > 2)
            {
				float x2 = player.position.X - player.velocity.X / 10f;
				float y2 = player.position.Y - player.velocity.Y / 10f;
				int selectRand = Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.RedTorch);
				Dust.NewDust(new Vector2(x2,y2), player.width / 2, player.height, selectRand, 0f, 0f, 100, default, 0.5f);
			}
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.FestivePlating>(), 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class FestiveChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Festive Chestplate");
			Tooltip.SetDefault("+1 Minion capacity\n+15% Summon damage");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 20;
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
			player.GetDamage(DamageClass.Summon) += 0.15f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.FestivePlating>(), 22)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class FestiveLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Festive Leggings");
			Tooltip.SetDefault("+1 Minion capacity\n+20% Movement speed");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 20000;
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 11;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
			player.moveSpeed += 0.2f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.FestivePlating>(), 18)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}