using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MechaArmor_Helmet : ModItem
	{
		public override bool Autoload(ref string name)
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
			item.width = 20;
			item.height = 20;
			item.value = 10000;
			item.rare = ItemRarityID.Green;//2
			item.defense = 30;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("MechaArmor_Chestplate") && legs.type == mod.ItemType("MechaArmor_Leggings");
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "10% increased damage";
			player.meleeDamage *= 1.1f;
			player.thrownDamage *= 1.1f;
			player.rangedDamage *= 1.1f;
			player.magicDamage *= 1.1f;
			player.minionDamage *= 1.1f;
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
		public override bool Autoload(ref string name)
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
			item.width = 38;
			item.height = 20;
			item.value = 10000;
			item.rare = ItemRarityID.Green;//2
			item.defense = 60;
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
		public override bool Autoload(ref string name)
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
			item.width = 22;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Green;//2
			item.defense = 45;
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