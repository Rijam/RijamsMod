using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.UI;
using Terraria.DataStructures;

namespace RijamsMod.Items.Tools
{
	public class TestMM : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override string Texture => "RijamsMod/Items/Tools/MMPickaxe";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Right click to bring up the tool UI (UI has been disabled)");
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 5;
			Item.useAnimation = 25;
			Item.pick = 0;
			Item.axe = 0;
			Item.hammer = 0;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0;
			Item.value = 1000000;
			Item.tileBoost += 20;
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item23;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.LaserDrill;
		}

		public override bool AltFunctionUse(Player player) => true;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
		{
			bool[] toolTypes = TheUI.buttonsActive;
			if (toolTypes[0])
            {
				Item.pick = 300;
			}
			else
            {
				Item.pick = 0;
			}
			if (toolTypes[1])
			{
				Item.axe = 40;//40 * 5 = 200
			}
			else
			{
				Item.axe = 0;
			}
			if (toolTypes[2])
			{
				Item.hammer = 200;
			}
			else
			{
				Item.hammer = 0;
			}

			if (player.altFunctionUse == 2)
			{
				if (TheUI.Visible == false)
                {
					TheUI.Visible = true;
				}
				return false;
			}

			return true;
		}
	}
}