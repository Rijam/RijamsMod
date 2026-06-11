using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Magic
{
	public class MiniGuster : ModItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(15, 4));
			ItemID.Sets.AnimatesAsSoul[Type] = true;
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by Angry Gusters]" });
		}
		public override void SetDefaults()
		{
			Item.height = 24;
			Item.width = 26;
			Item.damage = 20;
			Item.knockBack = 14f;
			Item.mana = 10;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.reuseDelay = 10;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 10);
			Item.UseSound = SoundID.Item104 with { Pitch = 0.75f };
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.MiniGusterProj>();
			Item.channel = true;
		}
		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			// Makes it so the item doesn't cost any mana to use if it didn't target anything.
			// Has the side effect of stating that the item uses 0 mana in the tooltip. That is corrected below.
			
			// Main.NewText($"{player.itemAnimation} {player.itemAnimationMax}");
			if (player.itemAnimation == player.itemAnimationMax)
			{
				// Main.NewText("true");
				// mult = 0;
				reduce -= Item.mana;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GlobalItems.FindTooltipIndex(tooltips, "UseMana", "Terraria", out int index))
			{
				tooltips[index].Text = Language.GetTextValue("CommonItemTooltip.UsesMana", (int)(Item.mana * Main.LocalPlayer.manaCost));
			}
		}

		public override void Load()
		{
			On_Player.ItemCheck_PayMana_ShouldSkipManaUse += Detour_Player_ItemCheck_PayMana_ShouldSkipManaUse;
		}

		private bool Detour_Player_ItemCheck_PayMana_ShouldSkipManaUse(On_Player.orig_ItemCheck_PayMana_ShouldSkipManaUse orig, Player self, Item sItem, bool altFire)
		{
			if (sItem.type == ModContent.ItemType<MiniGuster>() || sItem.type == ModContent.ItemType<FrostyGuster>())
			{
				return true;
			}
			return orig(self, sItem, altFire);
		}
	}
}