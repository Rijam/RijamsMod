using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Fishing
{
	public class HornetTail : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanBePlacedOnWeaponRacks[Type] = true;
			ItemOriginDesc.itemList.Add(Item.type, new string[] { "[c/474747:Fished in Honey underground]", null, null });
			Item.ResearchUnlockCount = 3;
		}
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.rare = ItemRarityID.White;
			Item.value = 1000;
			Item.maxStack = Item.CommonMaxStack;
		}
	}
	public class FishingPlayer : ModPlayer
	{
		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			if (attempt.inHoney && Player.ShoppingZone_BelowSurface && attempt.common && Main.rand.NextBool())
			{
				itemDrop = ModContent.ItemType<HornetTail>();
				return;
			}
		}
	}
}