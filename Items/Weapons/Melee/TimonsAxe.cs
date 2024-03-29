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
	public class TimonsAxe : MagicMeleeGlow
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Timon's Axe");
			// Tooltip.SetDefault("Much more powerful when combined with mana:\n  Throws short range projectiles\n  Weapon does double damage\n  Uses 20 mana");
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Hell Trader]", "[c/474747:after defeating Skeletron]" });
		}

		public override void SetDefaults() 
		{
			Item.damage = 30;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 44;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 100000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.DD2_MonkStaffSwing with { Pitch = -0.3f };
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.TimonsAxeProj2>();
			Item.shootSpeed = 16f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
			}
			Item.axe = 0;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.CheckMana(20, true)) //Checks if the player has 20 mana and then consumes it. Benefits from decreased mana cost.
			{
				//float numberProjectiles = 20;// 20 shots
				//float rotation = MathHelper.ToRadians(60);
				//position += Vector2.Normalize(velocity);

				/*for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
					Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI); //Already gets the double damage from below
				}*/

				float adjustedItemScale = player.GetAdjustedItemScale(Item);
				Projectile.NewProjectile(source, player.MountedCenter, new(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
				//Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, (float)player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
				//Projectile.NewProjectile(source, player.MountedCenter, velocity, type, damage, knockback, player.whoAmI, (float)player.direction * player.gravDir * 0.1f, 30f, adjustedItemScale);
				NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
				player.manaRegenDelay = player.maxRegenDelay * 2f;
			}

			return true;
		}
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (player.CheckMana(20, false))
			{
				damage *= 2 + (player.GetTotalDamage(DamageClass.Magic).Additive - 1);
			}
		}
	}
}