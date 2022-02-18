using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons
{
	public class HammerOfRetribution : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Hammer of Retribution");
			Tooltip.SetDefault("Much more powerful when combined with mana:\nThrows a long range projectile\nWeapon does double damage\nUses 20 mana");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Sold by Hell Trader]");
		}

		public override void SetDefaults() 
		{
			item.damage = 50;
			item.melee = true;
			item.width = 60;
			item.height = 60;
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5;
			item.value = 200000;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<Projectiles.HammerOfRetributionProj>();
			item.shootSpeed = 16f;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/HammerOfRetribution_Glow");
			}
			item.hammer = 0;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (player.CheckMana(20, true)) //Checks if the player has 20 mana and then consumes it. Benefits from decreased mana cost.
			{
				position += Vector2.Normalize(new Vector2(speedX, speedY));

				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage * 2, knockBack * 2, player.whoAmI);
				player.manaRegenDelay = (int)player.maxRegenDelay;
			}
			return false;
        }
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			if (player.CheckMana(20, false))
			{
				mult *= 2;
			}
		}
    }
}