using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace RijamsMod.Items.Weapons
{
	public class InterstellarPistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating Skeletron]", "[c/474747:and Arms Dealer is present]" } );
		}

		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 52;
			Item.height = 32;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Shoot; //5
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 4;
			Item.value = 100000;
			Item.rare = ItemRarityID.Green;//2
			Item.UseSound = SoundID.Item41;
			Item.autoReuse = true;
			Item.shoot = AmmoID.Bullet;
			Item.shootSpeed = 16f;
			Item.scale = 0.75f;
			Item.useAmmo = AmmoID.Bullet;
			if (!Main.dedServ)
            {
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = 1;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 2;
            }
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 2);
		}
	}
}
