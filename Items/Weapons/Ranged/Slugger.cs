using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Ranged
{
	public class Slugger : ModItem
	{
		/// <summary>
		/// The number of shots loaded in the item.
		/// </summary>
		internal byte numberOfShots = 1;
		/// <summary>
		/// 
		/// </summary>
		internal bool rightClicking = false;

		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:???]"]);
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true; // Alows holding right click
		}

		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 146;
			Item.height = 28;
			Item.useTime = 6;
			Item.useAnimation = 6;
			Item.reuseDelay = 6;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 12;
			Item.value = 50000;
			Item.rare = ItemRarityID.LightRed;//4
			Item.UseSound = null;
			Item.autoReuse = true;
			Item.shoot = AmmoID.Bullet;
			Item.shootSpeed = 16f;
			Item.scale = 0.75f;
			Item.useAmmo = AmmoID.Bullet;

			if (!Main.dedServ) //Need to check if a server is running, otherwise it will break multiplayer
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Flash");
				flash.posOffsetXLeft = 46;
				flash.posOffsetXRight = -72;
				flash.posOffsetY = -8;
				flash.posOffsetYGravity = 4;
				flash.frameCount = 5;
				flash.frameRate = 2;
				flash.animationLoop = false;
				flash.forceFirstFrame = false;
				flash.onlyUseOnPrimaryFire = true;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			//var flash = ModContent.GetInstance<WeaponAttackFlashLayer>();
			//if (player.ItemAnimationJustStarted)
			//{
				//Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				//flash.frame = 2;
			//}
			// Main.NewText($"{player.itemTimeMax} {player.itemTime} {player.itemTimeMax - player.itemTime}");
			/*if (player.ItemAnimationActive && (player.itemTimeMax - player.itemTime == 30))
			{
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				flash.frame = 2;
			}*/
			//Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			return false; // Don't spawn the projectile.
		}

		public override bool AltFunctionUse(Player player)
		{
			return true; // Allows right click
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			var flash = ModContent.GetInstance<WeaponAttackFlashLayer>();
			RijamsMod mod = ModContent.GetInstance<RijamsMod>();
			//Main.NewText($"{flash.frame} {flash.Timer}");

			//Main.NewText($"reuseDelay {player.reuseDelay} itemTime {player.itemTime} itemTimeMax {player.itemTimeMax} frame {flash.frame} shots {numberOfShots}");

			// Shoot the projectiles if:
			// Left click
			// Use just started
			// Before the reuse delay
			// At least 1 shot is loaded
			if ((player.altFunctionUse != 2 || !rightClicking) && player.itemTime == 0  && player.reuseDelay > 0 && numberOfShots > 0)
			{
				ShootProjectile(player, mod, flash);
			}
			/*else if (player.altFunctionUse == 2)
			{
				//flash.frame = 5;
				if (player.itemTime == 5 && numberOfShots < 10)
				{
					mod.PlayNetworkSound(SoundID.Item143 with { Pitch = -0.25f, Volume = 0.5f }, player.position, player);
				}
			}*/
			/*else if (player.reuseDelay == 0)
			{
				flash.frame = 5;
				if (player.itemTime == 5)
				{
					if (numberOfShots < 2)
					{
						numberOfShots = 2;
						Item.NetStateChanged();
						mod.PlayNetworkSound(SoundID.Item143 with { Pitch = -0.25f, Volume = 0.5f }, player.position, player);
					}
				}
			}*/
			/*
			if (numberOfShots <= 0)
			{
				//flash.frame = 6;
				//player.ApplyItemTime(Item, 5);
				//player.reuseDelay = 6;
				//numberOfShots = 2;
				//Item.NetStateChanged();
				//mod.PlayNetworkSound(SoundID.Item143 with { Pitch = -0.25f, Volume = 0.5f }, player.position, player);
				//Main.NewText($"{numberOfShots}");
			}
			*/
		}

		public override bool? UseItem(Player player)
		{
			var flash = ModContent.GetInstance<WeaponAttackFlashLayer>();
			RijamsMod mod = ModContent.GetInstance<RijamsMod>();

			//Main.NewText($"{numberOfShots}");
			ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"numberOfShots {numberOfShots}; rightClicking {rightClicking}; player.altFunctionUse {player.altFunctionUse}"), Color.White);
			
			/*
			//if (player.whoAmI != 255)
			if (player.whoAmI != Main.myPlayer)
			{
				return !(rightClicking || player.altFunctionUse == 2);
			}
			*/
			
			// Out of ammo!
			// Play click sound and emote the prohibition sign (circle with slash)
			if (numberOfShots <= 0 && (player.altFunctionUse != 2 || !rightClicking))
			{
				flash.frame = 6; // Set the flash frame to 6 which is invisible.
				mod.PlayNetworkSound(SoundID.Item143 with { Pitch = 1f, Volume = 0.5f }, player.position, player);
				EmoteBubble.NewBubble(EmoteID.DebuffCurse, new WorldUIAnchor(player), 45);
				// Main.NewText($"Out of ammo! Load more by right clicking.");
				/*player.ApplyItemTime(Item, 5);
				player.reuseDelay = 30;
				numberOfShots = 2;
				Item.NetStateChanged();
				mod.PlayNetworkSound(SoundID.Item143 with { Pitch = -0.25f, Volume = 0.5f }, player.position, player);
				Main.NewText($"{numberOfShots}");
				*/
			}
			// Pressed Right click
			if (player.altFunctionUse == 2)
			{
				numberOfShots++; // Increase the number of shots loaded
				// Play a click sound each time a shot is loaded.
				if (numberOfShots <= 10)
				{
					mod.PlayNetworkSound(SoundID.Item143 with { Pitch = -0.25f, Volume = 0.5f }, player.position, player);
				}
				// Max of 10 shots can be loaded. 
				if (numberOfShots > 10)
				{
					numberOfShots = 10;
				}
				// Main.NewText($"Loading ammo... {numberOfShots}");
				rightClicking = true;
				// This line will trigger NetSend to be called at the end of this game update, allowing the changes to useStyle to be in sync. 
				Item.NetStateChanged(); // Sync the numberOfShots var.
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"Loading ammo... numberOfShots {numberOfShots}; rightClicking {rightClicking}; player.altFunctionUse {player.altFunctionUse}"), Color.Gold);
				return true;
			}
			// Not pressed right click and there are shots loaded.
			else if (numberOfShots > 0)
			{
				numberOfShots--; // Decrease the number of shots loaded.
				rightClicking = false;
				Item.NetStateChanged(); // Sync the numberOfShots var.
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"Shot decreased? numberOfShots {numberOfShots}; rightClicking {rightClicking}; player.altFunctionUse {player.altFunctionUse}"), Color.Gray);
			}

			return base.UseItem(player);
		}

		/// <summary>
		/// Shoots two projectiles with randomness.
		/// </summary>
		/// <param name="player">Player instance</param>
		/// <param name="mod">Mod instance</param>
		/// <param name="flash">WeaponAttackFlashLayer instance</param>
		public void ShootProjectile(Player player, RijamsMod mod, WeaponAttackFlashLayer flash)
		{
			if (player.whoAmI != Main.myPlayer)
			{
				return;
			}
			flash.frame = 0; // Set the flash frame and flash time to the beginning.
			flash.Timer = 0;
			mod.PlayNetworkSound(SoundID.Item38 with { Pitch = -0.5f }, player.position, player); // Play the shooting sound

			Vector2 pointPoisition = player.RotatedRelativePoint(player.MountedCenter); // Position of the shots

			// Velocity of the shots (includes direction)
			float velX = (float)Main.mouseX + Main.screenPosition.X - pointPoisition.X;
			float velY = (float)Main.mouseY + Main.screenPosition.Y - pointPoisition.Y;
			Vector2 velocity = new(velX, velY);

			// Adjust the projectile type, damage, knockback based on the ammo used.
			player.PickAmmo(Item, out int projToShoot, out float _, out int projDamage, out float projKnockback, out int _); 
			int damage = projDamage;
			float knockback = player.GetWeaponKnockback(Item) + projKnockback;

			// Run ModifyShootStats in case other global modifies are applied.
			ModifyShootStats(player, ref pointPoisition, ref velocity, ref projToShoot, ref damage, ref knockback);

			// Get source
			IEntitySource projectileSource_Item_WithPotentialAmmo = player.GetSource_ItemUse_WithPotentialAmmo(Item, Item.useAmmo);

			// Rotate the velocity by 5 degrees
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));

			player.direction = velocity.X > 0 ? 1 : -1; // Set the player direction based on the direct they shot

			// Supposed to rotate the item in the player's hand, but it doesn't seem to do anything. 
			player.itemRotation = (float)Math.Atan2(velocity.Y * (float)player.direction, velocity.X * (float)player.direction) - player.fullRotation;
			NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
			NetMessage.SendData(MessageID.ShotAnimationAndSound, -1, -1, null, player.whoAmI);

			// Spawn the first projectile
			Projectile.NewProjectile(projectileSource_Item_WithPotentialAmmo, pointPoisition, velocity, projToShoot, damage, knockback, player.whoAmI);
			
			// Rotate the velocity and spawn the second projectile.
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
			Projectile.NewProjectile(projectileSource_Item_WithPotentialAmmo, pointPoisition, velocity, projToShoot, damage, knockback, player.whoAmI);
			
			//Shoot(player, (EntitySource_ItemUse_WithAmmo)projectileSource_Item_WithPotentialAmmo, pointPoisition, velocity, projToShoot, damage, knockback);
			//numberOfShots--;
			//Main.NewText($"{numberOfShots}");
			//Item.NetStateChanged();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse != 2 && numberOfShots > 0)
			{
				// Rotate the item in the player's hand when shooting only.
				// Since this randomness is independent from the bullet's randomness, the gun's rotation might not match the direction of the bullets.
				velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
			}
		}
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-14, 2); // Hold by the handle
		}

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			// Only consume ammo when firing.
			if (numberOfShots <= 0 || player.altFunctionUse == 2)
			{
				return false;
			}
			return base.CanConsumeAmmo(ammo, player);
		}

		// Sync the numberOfShots var
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(numberOfShots);
			writer.Write(rightClicking);
		}

		public override void NetReceive(BinaryReader reader)
		{
			numberOfShots = reader.ReadByte();
			rightClicking = reader.ReadBoolean();
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
		}
	}

	// Draw the ammo counter
	public class SluggerPlayerDrawLayer : PlayerDrawLayer
	{
		public Asset<Texture2D> AmmoCounter;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			// Only draw when holding the item and using the item
			return drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<Slugger>() && (drawInfo.drawPlayer.controlUseItem || drawInfo.drawPlayer.altFunctionUse == 2);
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head); // Render after almost everything.
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			AmmoCounter ??= ModContent.Request<Texture2D>("RijamsMod/Items/Weapons/Ranged/SluggerAmmoCounter"); // Load the texture.
			Player drawPlayer = drawInfo.drawPlayer;
			Item item = drawPlayer.HeldItem;
			int frame = 0;
			if (item.ModItem is Slugger slugger)
			{
				frame = slugger.numberOfShots; // Set the frame to the numberOfShots
			}
			Rectangle sourceRect = AmmoCounter.Frame(1, 11, frameY: frame); // Set the source rectangle
			Vector2 pos = drawInfo.Center - Main.screenPosition; // Position
			pos.X -= AmmoCounter.Width() / 2f; // Center with the player
			pos.Y += drawPlayer.height; // Move down

			// Draw the ammo counter
			DrawData drawData = new(AmmoCounter.Value, pos, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
}
