using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RijamsMod.Projectiles.Other;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Other
{
	public class StumbleCard : ModItem
	{
		public Asset<Texture2D> StumbleCardSingle;

		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 8));
			ItemID.Sets.AnimatesAsSoul[Type] = true;
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Found in Dungeon chests or Golden Lock Boxes]", "[c/474747:Found in Shadow chests or Obsidian Lock Boxes]" });
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 32;
			Item.DamageType = DamageClass.MagicSummonHybrid;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.mana = 5;
			Item.damage = 40;
			Item.knockBack = 10f;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item71 with { Pitch = -1f };
			Item.shoot = ModContent.ProjectileType<ClankBeam>();
			Item.shootSpeed = 8f;
			if (!Main.dedServ) //Need to check if a server is running, otherwise it will break multiplayer
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/Weapons/Other/Clank");
				flash.posOffsetXLeft = 20;
				flash.posOffsetXRight = -60;
				flash.posOffsetY = 204;
				flash.alpha = 127;
				flash.frameCount = 1;
				flash.frameRate = 10;
				flash.animationLoop = false;
				flash.forceFirstFrame = true;
			}
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			StumbleCardSingle ??= Mod.Assets.Request<Texture2D>("Items/Weapons/Other/" + Name + "_Single");
			spriteBatch.Draw(StumbleCardSingle.Value, position, null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);

			return false;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float numberProjectiles = 2; // 2 shots
			float rotation = MathHelper.ToRadians(5);
			position += Vector2.Normalize(velocity) * 2f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false;
		}

		public override void HoldItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime != 0)
			{
				player.GetModPlayer<RijamsModPlayer>().criticalHitAdditionalDamage += 0.25f;
			}
		}
	}
}