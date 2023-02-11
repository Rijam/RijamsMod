using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ShroomiteVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shroomite Visor");
			// Tooltip.SetDefault("10% increased all ranged damage\n5% increased ranged critical strike chance");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.value = 375000;
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 11;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.ShroomiteBreastplate && legs.type == ItemID.ShroomiteLeggings;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("ArmorSetBonus.Shroomite");
			player.shroomiteStealth = true;
			player.GetCritChance(DamageClass.Ranged) += 5;
			player.GetDamage(DamageClass.Ranged) += 0.1f;
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.ShroomiteBar, 12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}