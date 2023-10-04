using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RijamsMod.Items.Armor.RedSkyware;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	// This file shows a very simple example of a GlobalItem class. GlobalItem hooks are called on all items in the game and are suitable for sweeping changes like 
	// adding additional data to all items in the game. Here we simply adjust the damage of the Copper Shortsword item, as it is simple to understand. 
	// See other GlobalItem classes in ExampleMod to see other ways that GlobalItem can be used.
	public class GlobalAccessoryItems : GlobalItem
	{
		public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
		{
			if (player.GetModPlayer<RijamsModPlayer>().daybreakStone && item.CountsAsClass(DamageClass.Melee) && !item.noMelee && !item.noUseGraphic && Main.rand.NextBool(2))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SolarFlare, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default, 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.7f;
				Main.dust[dust].velocity.Y -= 0.5f;
				Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.Yellow.ToVector3() * 0.875f);
			}
			if (player.GetModPlayer<RijamsModPlayer>().frostburnStone && item.CountsAsClass(DamageClass.Melee) && !item.noMelee && !item.noUseGraphic && Main.rand.NextBool(2))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Frost, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default, 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.7f;
				Main.dust[dust].velocity.Y -= 0.5f;
				Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.LightBlue.ToVector3() * 0.875f);
			}
		}
		public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
		{
			if (player.GetModPlayer<RijamsModPlayer>().rocketBooster && (ammo.type == AmmoID.Rocket || weapon.useAmmo == AmmoID.Rocket || 
					ammo.type == ItemID.ExplosiveJackOLantern || weapon.useAmmo == ItemID.ExplosiveJackOLantern || ammo.type == ItemID.Nail ||
					weapon.useAmmo == ItemID.Nail || ammo.type == ItemID.StyngerBolt || weapon.useAmmo == ItemID.StyngerBolt) && !player.GetModPlayer<RijamsModPlayer>().gamutApparatus)
			{
				damage += 0.1f;
				knockback *= 1.1f;
				speed *= 1.5f;
			}
			if (player.GetModPlayer<RijamsModPlayer>().gamutApparatus)
			{
				knockback *= 1.2f;
				if (ammo.type == AmmoID.Rocket || weapon.useAmmo == AmmoID.Rocket ||
					ammo.type == ItemID.ExplosiveJackOLantern || weapon.useAmmo == ItemID.ExplosiveJackOLantern || ammo.type == ItemID.Nail ||
					weapon.useAmmo == ItemID.Nail || ammo.type == ItemID.StyngerBolt || weapon.useAmmo == ItemID.StyngerBolt)
				{
					speed *= 1.5f;
				}
				else
				{
					speed *= 1.2f;
				}
				if (type == ProjectileID.WoodenArrowFriendly)
				{
					type = ProjectileID.FireArrow;
					damage.Flat += 2;
				}
			}
			if (player.armor[0].type == ModContent.ItemType<RedSkywareHelmet>()) // If the Red Skyware Helmet is equipped in the helmet slot
			{
				speed *= 1.1f;
				knockback *= 1.1f;
			}
		}
		public override void ModifyItemScale(Item item, Player player, ref float scale)
		{
			if (player.armor[0].type == ModContent.ItemType<RedSkywareMask>() && (player.HeldItem.DamageType.CountsAsClass(DamageClass.Melee) || Main.mouseItem.DamageType.CountsAsClass(DamageClass.Melee)))
			{
				scale *= 1.1f;
			}
		}
	}

	public class SummonersGloveUpdate : GlobalItem
	{
		public override bool? CanAutoReuseItem(Item item, Player player)
		{
			if (player.GetModPlayer<RijamsModPlayer>().controlGlove)
			{
				if ((item.sentry || ProjectileID.Sets.MinionShot[item.shoot] || ProjectileID.Sets.MinionSacrificable[item.shoot]) && item.CountsAsClass(DamageClass.Summon))
				{
					return true;
				}
			}
			if (player.GetModPlayer<RijamsModPlayer>().summonersGlove)
			{
				if (item.CountsAsClass(DamageClass.Summon))
				{
					return true;
				}
			}
			return base.CanAutoReuseItem(item, player);
		}
	}
	public class GuideToProperFlightTechniquesUpdate : GlobalItem
	{
		public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
		{
			if (player.GetModPlayer<RijamsModPlayer>().guideToProperFlightTechniques)
			{
				if (player.wingTimeMax > 0)
				{
					speed += 1f;
					acceleration *= 2f;
				}
			}
		}
		public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			if (player.GetModPlayer<RijamsModPlayer>().guideToProperFlightTechniques)
			{
				if (player.wingTimeMax > 0)
				{
					maxCanAscendMultiplier *= 1.1f;
					maxAscentMultiplier *= 1.1f;
				}
			}
		}
	}

	public class YoyoBackpackILEdit : GlobalItem
	{
		public override void Load()
		{
			Terraria.IL_Player.Counterweight += Player_Counterweight_YoyoBackpackEdit;
		}

		private static void Player_Counterweight_YoyoBackpackEdit(ILContext il)
		{
			ILCursor c = new(il);

			// Try to find where 2 is placed onto the stack
			// This 2 is the start of the crowding 
			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(2)))
			{
				ModContent.GetInstance<RijamsMod>().Logger.Debug("Patch 1 of Player_Counterweight_YoyoBackpackEdit unable to be applied! ");
				return; // Patch unable to be applied
			}

			// Push the Player instance onto the stack
			c.Emit(OpCodes.Ldarg_0);
			// Call a delegate using the int and Player from the stack.
			c.EmitDelegate<Func<int, Player, int>>((returnValue, player) =>
			{
				// Regular c# code
				// Original code:

				// num is the Main.projectile index
				// num2 is the number of aiStyle 99 projectiles the player has (both yoyos and counterweights).
				// num3 is the number of counterweights the player has.

				// if (yoyoGlove && num2 < 2) {		<---- Editing this 2
				//	if (num >= 0)
				//	{
				//		Vector2 vector = hitPos - base.Center;
				//		vector.Normalize();
				//		vector *= 16f;
				//		Projectile.NewProjectile(Projectile.InheritSource(Main.projectile[num]), base.Center.X, base.Center.Y, vector.X, vector.Y, Main.projectile[num].type, Main.projectile[num].damage, Main.projectile[num].knockBack, whoAmI, 1f);
				//	}
				// With num2 being the number of counterweights that are alive.

				// Change the 2 to a 3 if the player has the Yoyo Backpack.
				// This will give the player an extra yoyo and extra counterweight.
				return returnValue + player.GetModPlayer<RijamsModPlayer>().bonusYoyosAndCounterweights;
			});

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdloc1()))
			{
				ModContent.GetInstance<RijamsMod>().Logger.Debug("Patch 2 of Player_Counterweight_YoyoBackpackEdit unable to be applied! ");
				return; // Patch unable to be applied
			}

			c.Emit(OpCodes.Ldarg_0);

			c.EmitDelegate<Func<int, Player, int>>((returnValue, player) =>
			{
				// Regular c# code
				// Original code:

				// else if (num3 < num2)		<---- Editing this num2
				// {
				//	Vector2 vector2 = hitPos - base.Center;
				//	vector2.Normalize();
				//	vector2 *= 16f;
				//	float knockBack = (kb + 6f) / 2f;
				//	IEntitySource spawnSource = Projectile.InheritSource(Main.projectile[num]);
				//	if (num3 > 0)
				// 		Projectile.NewProjectile(spawnSource, base.Center.X, base.Center.Y, vector2.X, vector2.Y, counterWeight, (int)((double)dmg * 0.8), knockBack, whoAmI, 1f);
				//	else
				// 		Projectile.NewProjectile(spawnSource, base.Center.X, base.Center.Y, vector2.X, vector2.Y, counterWeight, (int)((double)dmg * 0.8), knockBack, whoAmI);
				// }

				// This will give the player an extra counterweight (no extra yoyo, though).
				return returnValue + player.GetModPlayer<RijamsModPlayer>().bonusCounterweights;
			});
		}
	}
}