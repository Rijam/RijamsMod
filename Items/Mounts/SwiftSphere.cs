using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Mounts
{
	public class SwiftSphere : DistortionSphere
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// Movement
			MountData.acceleration = 0.4f; // The rate at which the mount speeds up.
			MountData.constantJump = true; // Allows you to hold the jump button down.
			MountData.runSpeed = 24f; // The speed of the mount

			MountData.buff = ModContent.BuffType<Buffs.Mounts.SwiftSphereBuff>(); // The ID number of the buff assigned to the mount.

			// Effects
			MountData.spawnDustNoGravity = true;
			MountData.spawnDust = DustID.GreenTorch; // The ID of the dust spawned when mounted or dismounted.
		}
	}


	public class SwiftSphereItem : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<RijamsModConfigServer>().LoadDebugItems;
		}

		public override string Texture => GetType().Namespace.Replace('.', '/') + "/SwiftSphere";

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item79; // What sound should play when using the item
			Item.noMelee = true; // this item doesn't do any melee damage
			Item.mountType = ModContent.MountType<SwiftSphere>();
		}

		public override bool? UseItem(Player player)
		{
			return base.UseItem(player);
		}

		public override void AddRecipes()
		{

		}
	}
}