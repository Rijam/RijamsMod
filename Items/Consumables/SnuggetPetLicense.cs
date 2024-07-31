using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RijamsMod.NPCs.TownNPCs;
using RijamsMod.NPCs.TownNPCs.SnuggetPet;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Consumables
{
	public class SnuggetPetLicense : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemOriginDesc.itemList.Add(Item.type, new List<string> { "[c/474747:Sold by Zoologist]", "[c/474747:after 60% Bestiary completion]" });
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.UseSound = SoundID.Item92;
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = Item.CommonMaxStack;
			Item.SetShopValues(ItemRarityColor.Green2, Item.buyPrice(0, 5));
		}

		public override void OnConsumeItem(Player player)
		{
			int npcType = ModContent.NPCType<SnuggetPet>(); // The NPC Type for the Town Pet.
			if (player.whoAmI == Main.myPlayer && player.itemAnimation > 0)
			{
				// Only do something if the License hasn't been used before or the Town Pet exists in the world.
				if (!RijamsModWorld.boughtSnuggetPet || NPC.AnyNPCs(npcType))
				{
					player.ApplyItemTime(Item); // Make it so the player uses the item for the useAnimation.
					SnuggetUnlockOrExchangePet(ref RijamsModWorld.boughtSnuggetPet, npcType, "Mods.RijamsMod.UI.LicenseSnuggetUse"); // Modified NPC.UnlockOrExchangePet method.
				}
			}
		}
		public override bool? UseItem(Player player)
		{
			// Only do something if the License hasn't been used before or the Town Pet exists in the world.
			int npcType = ModContent.NPCType<SnuggetPet>(); // The NPC Type for the Town Pet.
			if (player.ItemAnimationJustStarted && (!RijamsModWorld.boughtSnuggetPet || NPC.AnyNPCs(npcType)))
			{
				if (player.whoAmI == Main.myPlayer)
				{
					player.ApplyItemTime(Item); // Make it so the player uses the item for the useAnimation.
					SnuggetUnlockOrExchangePet(ref RijamsModWorld.boughtSnuggetPet, npcType, "Mods.RijamsMod.UI.LicenseSnuggetUse"); // Modified NPC.UnlockOrExchangePet method.
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// <br>The vanilla method NPC.UnlockOrExchangePet will not work for our modded Town Pets because the NetMessage only works with vanilla NPCs.</br>
		/// <br>This version uses a ModPacket for that instead.</br>
		/// </summary>
		/// <param name="petBoughtFlag">The bool that determines if the License has been used once. Doesn't really have anything to do with buying.</param>
		/// <param name="npcType">The NPC Type for the Town Pet.</param>
		/// <param name="textKeyForLicense">The localization path for when the License has been used for the first time.</param>
		public static void SnuggetUnlockOrExchangePet(ref bool petBoughtFlag, int npcType, string textKeyForLicense)
		{
			Color color = new(50, 255, 130);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				if (!petBoughtFlag || NPC.AnyNPCs(npcType))
				{
					ModPacket packet = ModContent.GetInstance<RijamsMod>().GetPacket();
					packet.Write((byte)RijamsModMessageType.SnuggetUnlockOrExchange);
					packet.Send();
				}
			}
			else if (!petBoughtFlag)
			{
				petBoughtFlag = true;
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey(textKeyForLicense), color);
				NetMessage.TrySendData(MessageID.WorldData);
			}
			else if (NPC.RerollVariationForNPCType(npcType))
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PetExchangeSuccess"), color);
			}
			else
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PetExchangeFail"), color);
			}
		}
	}
}