using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.UI;
using Terraria.DataStructures;
using RijamsMod.Projectiles.Tools;
using Terraria.Audio;
using System.Collections.Generic;
using System;

namespace RijamsMod.Items.Tools
{
	public class MatterManipulator : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}
		public override string Texture => "RijamsMod/Items/Tools/MMPickaxe";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Right click to bring up the tool UI\n  The UI can be dragged around on the screen\n  Pickaxe takes priority over hammer");
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
			Item.UseSound = null;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MMProj>(); // ProjectileID.LaserDrill;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
		{
			bool spawnProj = true;
			if (player.altFunctionUse == 2)
			{
				if (MatterManipulatorUI.Visible == false)
				{
					MatterManipulatorUI.Visible = true;
					SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.5f });
				}
				spawnProj = false;
			}

			bool[] toolTypes = MatterManipulatorUI.buttonsActive;
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
				Item.axe = 40; // 40 * 5 = 200
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

			if (spawnProj)
			{
				Projectile projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockBack, player.whoAmI, 0, 0);
				if (projectile.ModProjectile is MMProj modProjectile)
				{
					modProjectile.pick = Item.pick > 0;
					modProjectile.axe = Item.axe > 0;
					modProjectile.hammer = Item.hammer > 0;
				}
				SoundEngine.PlaySound(SoundID.Item68 with { Pitch = 0.5f, Volume = 0.5f }, projectile.position);
			}

			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.Mod == "Terraria" && line.Name == "ItemName")
				{
					line.OverrideColor = Color.Lerp(Color.OrangeRed, Color.Yellow, 0.5f + ((float)Math.Sin(Main.GlobalTimeWrappedHourly * 5)) / 2f);
				}
			}
		}
	}
}