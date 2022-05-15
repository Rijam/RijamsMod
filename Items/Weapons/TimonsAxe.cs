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
	public class TimonsAxe : MagicMeleeGlow
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Timon's Axe");
			Tooltip.SetDefault("Much more powerful when combined with mana:\n  Throws short range projectiles\n  Weapon does double damage\n  Uses 20 mana");
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Sold by Hell Trader]");
		}

		public override void SetDefaults() 
		{
			item.damage = 30;
			item.melee = true;
			item.width = 54;
			item.height = 44;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3;
			item.value = 100000;
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<Projectiles.TimonsAxeProj>();
			item.shootSpeed = 16f;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/TimonsAxe_Glow");
			}
			item.axe = 0;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (player.CheckMana(20, true)) //Checks if the player has 20 mana and then consumes it. Benefits from decreased mana cost.
			{
				float numberProjectiles = 20;// 20 shots
				float rotation = MathHelper.ToRadians(60);
				position += Vector2.Normalize(new Vector2(speedX, speedY));

				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI); //Already gets the double damage from below
				}
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