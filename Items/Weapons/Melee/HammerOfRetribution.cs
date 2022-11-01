using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Melee
{
	public class HammerOfRetribution : MagicMeleeGlow
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Hammer of Retribution");
			Tooltip.SetDefault("Much more powerful when combined with mana:\n  Throws a long range projectile\n  Weapon does double damage\n  Uses 20 mana");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Hell Trader]", "[c/474747:after defeating any Mechanical Boss]", null });
		}

		public override void SetDefaults() 
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5;
			Item.value = 200000;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.HammerOfRetributionProj>();
			Item.shootSpeed = 16f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
			}
			Item.hammer = 0;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.CheckMana(20, true)) //Checks if the player has 20 mana and then consumes it. Benefits from decreased mana cost.
			{
				position += Vector2.Normalize(velocity);

				Projectile.NewProjectile(source, position, velocity, type, damage * 2, knockback * 2, player.whoAmI);
				player.manaRegenDelay = (int)player.maxRegenDelay;
			}
			return false;
		}
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (player.CheckMana(20, false))
			{
				damage *= 2;
			}
		}
	}
}