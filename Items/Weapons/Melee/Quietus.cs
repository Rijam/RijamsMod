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
	public class Quietus : MagicMeleeGlow
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Quietus");
			Tooltip.SetDefault("Much more powerful when combined with mana:\n  Throws multiple long range projectiles\n  Weapon does double damage\n  Uses 20 mana\n'Not to be confused with the Whisper's Edge'");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Hell Trader]", "[c/474747:After defeating Golem]", null });
		}

		public override void SetDefaults() 
		{
			Item.damage = 60;
			Item.DamageType = DamageClass.Melee;
			Item.width = 72;
			Item.height = 72;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 500000;
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.QuietusProj>();
			Item.shootSpeed = 16f;
            if (!Main.dedServ)
            {
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
			}
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.CheckMana(20, true)) //Checks if the player has 20 mana and then consumes it. Benefits from decreased mana cost.
			{
				float numberProjectiles = 6;// 6 shots
				float rotation = MathHelper.ToRadians(30); //spread across 30 degrees
				position += Vector2.Normalize(velocity);

				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI); //Already gets the double damage from below
				}
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