using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Magic
{
	public class FrostyGuster : ModItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(15, 4));
			ItemID.Sets.AnimatesAsSoul[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.height = 24;
			Item.width = 26;
			Item.damage = 50;
			Item.knockBack = 16f;
			Item.mana = 10;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.reuseDelay = 10;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(silver: 75);
			Item.UseSound = SoundID.Item104 with { Pitch = 0.6f };
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.FrostyGusterProj>();
			Item.channel = true;
		}
		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			// Makes it so the item doesn't cost any mana to use if it didn't target anything.
			// Has the side effect of stating that the item uses 0 mana in the tooltip. That is corrected below.
			if (player.itemAnimation == 0)
			{
				reduce -= Item.mana;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<MiniGuster>())
				.AddIngredient(ModContent.ItemType<Accessories.Defense.FrostyRose>())
				.AddTile(TileID.CrystalBall)
				.Register();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GlobalItems.FindTooltipIndex(tooltips, "UseMana", "Terraria", out int index))
			{
				tooltips[index].Text = Language.GetTextValue("CommonItemTooltip.UsesMana", (int)(Item.mana * Main.LocalPlayer.manaCost));
			}
		}
	}
}