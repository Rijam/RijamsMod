using RijamsMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace RijamsMod.Items.Weapons.Magic
{
	public class PlasmaRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Rifle");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating Plantera]", null });
		}

		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true; //Channel so that you can hold the weapon [Important]
			Item.mana = 5;
			Item.rare = ItemRarityID.Yellow; //8
			Item.width = 60;
			Item.height = 18;
			Item.useTime = 7;
			Item.knockBack = 2;
			Item.UseSound = new(Mod.Name + "/Sounds/Item/PlasmaRifleAlt") { Volume = 0.8f, PitchVariance = 0.05f, MaxInstances = 10 };
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 16f;
			Item.useAnimation = 7;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PlasmaBolt>();
			Item.value = Item.sellPrice(gold: 8);
			if (!Main.dedServ)
			{
				var flash = Item.GetGlobalItem<WeaponAttackFlash>();
				flash.flashTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_MuzzleFlash").Value;
				flash.posOffsetXLeft = 6;
				flash.posOffsetXRight = 2;
				flash.posOffsetY = 0;
				flash.frameCount = 2;
				flash.frameRate = 7;
				flash.useRandomFrame = true;
			}
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
		// How can I make the shots appear out of the muzzle exactly?
		// Also, when I do this, how do I prevent shooting through tiles?
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 50f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
				position.Y -= 4 * player.gravDir;
			}
		}
	}
}
