using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons.Magic
{
	public class MiniGuster : ModItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(15, 4));
			ItemID.Sets.AnimatesAsSoul[Type] = true;
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Dropped by Angry Gusters]" });
		}
		public override void SetDefaults()
		{
			Item.height = 24;
			Item.width = 26;
			Item.damage = 20;
			Item.knockBack = 14f;
			Item.mana = 10;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.reuseDelay = 10;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 10);
			Item.UseSound = SoundID.Item104 with { Pitch = 0.75f };
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.MiniGusterProj>();
			Item.channel = true;
		}
		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.itemAnimation == 0)
			{
				reduce -= Item.mana;
			}
		}
	}
}