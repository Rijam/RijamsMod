using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor.MechaArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class MechaArmor_Helmet : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mecha Helmet");
			Tooltip.SetDefault("This is a modded helmet.");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;//2
			Item.defense = 30;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("MechaArmor_Chestplate").Type && legs.type == Mod.Find<ModItem>("MechaArmor_Leggings").Type;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "10% increased damage";
			player.GetDamage(DamageClass.Melee) *= 1.1f;
			player.GetDamage(DamageClass.Ranged) *= 1.1f;
			player.GetDamage(DamageClass.Magic) *= 1.1f;
			player.GetDamage(DamageClass.Summon) *= 1.1f;
			player.GetDamage(DamageClass.Throwing) *= 1.1f;
		}

		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "EquipMaterial", 30);
			recipe.AddTile(null, "ExampleWorkbench");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
	[AutoloadEquip(EquipType.Body, EquipType.Back)]
	public class MechaArmor_Chestplate : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Mecha Chestplate");
			Tooltip.SetDefault("This is a modded body armor."
				+ "\nImmunity to 'On Fire!'"
				+ "\n+20 max mana and +1 max minions");
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;//2
			Item.defense = 60;
		}

		public override void UpdateEquip(Player player)
		{
			player.buffImmune[BuffID.OnFire] = true;
			player.statManaMax2 += 20;
			player.maxMinions++;
		}
		/*public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = false;
			equipSlot = mod.GetEquipSlot("MechaArmor_Chestplate_Back", EquipType.Back);
		}*/

		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "EquipMaterial", 60);
			recipe.AddTile(null, "ExampleWorkbench");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
	[AutoloadEquip(EquipType.Legs)]
	public class MechaArmor_Leggings : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mecha Leggings");
			Tooltip.SetDefault("This is a modded leg armor."
				+ "\n5% increased movement speed");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;//2
			Item.defense = 45;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.05f;
		}

		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "EquipMaterial", 45);
			recipe.AddTile(null, "ExampleWorkbench");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
}