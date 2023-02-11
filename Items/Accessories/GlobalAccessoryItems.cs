using Microsoft.Xna.Framework;
using RijamsMod.Items.Armor.RedSkyware;
using Terraria;
using Terraria.DataStructures;
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
				knockback *= 1.1f;
				speed *= 1.2f;
			}
			if (player.GetModPlayer<RijamsModPlayer>().gamutApparatus)
			{
				knockback *= 1.2f;
				speed *= 1.2f;
				if (type == ProjectileID.WoodenArrowFriendly)
				{
					type = ProjectileID.FireArrow;
					damage += 2;
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
}