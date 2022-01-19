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
			ItemOriginDesc.itemList.Add(item.type, "[c/474747:Sold by Interstellar Traveler]");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.RocketLauncher);
			item.damage = 58;
			//item.ranged = true;
			item.width = 70;
			item.height = 28;
			item.useTime = 25;
			item.useAnimation = 25;
			//item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 300000;
			item.rare = ItemRarityID.Yellow; //8
			if (!Main.dedServ) //Need to check if a server is running, otherwise it will break multiplayer
			{
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/AGMMissileLauncher").WithVolume(.7f);
			}
			//item.autoReuse = true;
			//item.shootSpeed = 16f;
			//item.shoot = AmmoID.Rocket;
			//item.useAmmo = AmmoID.Rocket;
		}

		//What if I wanted this gun to have a 10% chance not to consume ammo?
		public override bool ConsumeAmmo(Player player)
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
