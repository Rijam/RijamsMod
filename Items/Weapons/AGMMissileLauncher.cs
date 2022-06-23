using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Weapons
{
	public class AGMMissileLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("AGM Missile Launcher");
			Tooltip.SetDefault("10% chance not to consume ammo");
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Sold by Interstellar Traveler]", "[c/474747:After defeating Golem]", "[c/474747:and Cyborg is present]" } );
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.RocketLauncher);
			Item.damage = 58;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 70;
			Item.height = 28;
			Item.useTime = 25;
			Item.useAnimation = 25;
			//item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = 300000;
			Item.rare = ItemRarityID.Yellow; //8
			Item.UseSound = new(Mod.Name + "/Sounds/Item/AGMMissileLauncher") { Volume = 0.7f, MaxInstances = 5 };
			//item.autoReuse = true;
			//item.shootSpeed = 16f;
			//item.shoot = AmmoID.Rocket;
			//item.useAmmo = AmmoID.Rocket;
		}

		//What if I wanted this gun to have a 10% chance not to consume ammo?
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= 0.10f;
		}

		// Help, my gun isn't being held at the handle! Adjust these 2 numbers until it looks right.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-15, 0);
		}
	}
}
