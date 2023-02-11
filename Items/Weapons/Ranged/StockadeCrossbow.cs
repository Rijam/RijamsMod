using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Items.Armor.Vanity.IntTrav;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class StockadeCrossbow : ModItem
	{
		public uint numTimesShot = 0;

		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Shoots a Water Stream every 10 shots");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Dropped by Skeleton Crossbower]", null, null });
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Handgun; // Shimmer transforms the item.
		}

		public override void SetDefaults()
		{
			Item.damage = 21;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 56;
			Item.height = 20;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot; //5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 1;
			Item.value = 25000;
			Item.rare = ItemRarityID.Green;//2
			Item.UseSound = SoundID.Item5 with { Pitch = -0.2f };
			Item.autoReuse = true;
			Item.shoot = AmmoID.Arrow;
			Item.shootSpeed = 9f;
			Item.scale = 1f;
			Item.useAmmo = AmmoID.Arrow;

			if (!Main.dedServ) //Need to check if a server is running, otherwise it will break multiplayer
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_MuzzleFlash").Value;
				flash.posOffsetXLeft = 8;
				flash.posOffsetXRight = -4;
				flash.posOffsetY = 2;
				flash.frameCount = 2;
				flash.frameRate = 5;
				flash.colorNoAlpha = new(255, 255, 255);
				flash.alpha = 0;
				//flash.flashCondition = () => TimeForAltShot() == true;
				flash.forceFirstFrame = true;
				flash.animationLoop = false;
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			numTimesShot++;
			// Main.NewText(numTimesShot);
			var flash = Item.GetGlobalItem<WeaponAttackFlash>();
			flash.flashCondition = () => false;
			if (TimeForAltShot()) // Every 10th shot.
			{
				flash.flashCondition = () => true;
				Projectile aqua = Projectile.NewProjectileDirect(source, position, velocity * 1.4f, ProjectileID.WaterStream, damage, knockback, Main.myPlayer);
				aqua.DamageType = DamageClass.Ranged;
				SoundEngine.PlaySound(SoundID.Item21 with { Pitch = 0.5f }, aqua.position);

				return false;
			}
			return true;
		}

		// Don't consume ammo on the alt shots
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return (numTimesShot + 1) % 10 != 0;
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, -4);
		}

		public bool TimeForAltShot()
		{
			return numTimesShot % 10 == 0; // Every 10th shot.
		}
	}
}
