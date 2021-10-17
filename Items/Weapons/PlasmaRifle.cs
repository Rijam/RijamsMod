using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RijamsMod.Items.Weapons
{
	public class PlasmaRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Rifle");
			Tooltip.SetDefault("[c/403638:Sold by Interstellar Traveler]");
		}

		public override void SetDefaults()
		{
			item.damage = 60;
			item.noMelee = true;
			item.magic = true;
			item.channel = true; //Channel so that you can hold the weapon [Important]
			item.mana = 5;
			item.rare = ItemRarityID.Yellow; //8
			item.width = 60;
			item.height = 18;
			item.useTime = 7;
			item.knockBack = 2;
			if (!Main.dedServ) //Need to check if a server is running, otherwise it will break multiplayer
            {
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PlasmaRifleAlt").WithVolume(.8f).WithPitchVariance(.05f);
			}
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shootSpeed = 16f;
			item.useAnimation = 7;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<PlasmaBolt>();
			item.value = Item.sellPrice(gold: 8);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
		// How can I make the shots appear out of the muzzle exactly?
		// Also, when I do this, how do I prevent shooting through tiles?
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}
	}
}
