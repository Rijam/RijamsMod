using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.UI;

namespace RijamsMod.Items.Tools
{
	public class TestMM : ModItem
	{
		public override bool Autoload(ref string name)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override string Texture => "Terraria/UI/Reforge_1";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Right click to bring up the tool UI (UI has been disabled)");
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.channel = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 5;
			item.useAnimation = 25;
			item.pick = 0;
			item.axe = 0;
			item.hammer = 0;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 0;
			item.value = 1000000;
			item.tileBoost += 20;
			item.rare = ItemRarityID.Purple;
			item.UseSound = SoundID.Item23;
			item.autoReuse = true;
			item.shoot = ProjectileID.LaserDrill;
		}

		public override bool AltFunctionUse(Player player) => true;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			bool[] toolTypes = TheUI.buttonsActive;
			if (toolTypes[0])
            {
				item.pick = 300;
			}
			else
            {
				item.pick = 0;
			}
			if (toolTypes[1])
			{
				item.axe = 40;//40 * 5 = 200
			}
			else
			{
				item.axe = 0;
			}
			if (toolTypes[2])
			{
				item.hammer = 200;
			}
			else
			{
				item.hammer = 0;
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