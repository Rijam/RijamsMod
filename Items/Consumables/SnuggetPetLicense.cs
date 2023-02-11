using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RijamsMod.NPCs.TownNPCs;
using RijamsMod.NPCs.TownNPCs.SnuggetPet;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.Enums;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RijamsMod.Items.Consumables
{
	public class SnuggetPetLicense : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Use to adopt a Snugget for your town\nAlready have a Snugget?\nUse additional licenses to activate the Pet Exchange Program!\nFind the perfect fit for you and your Snugget!");
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

		/* Player
		private void ItemCheck_UsePetLicenses(Item sItem)
		{
			if (sItem.type == 4829 && itemAnimation > 0)
				LicenseOrExchangePet(sItem, ref NPC.boughtCat, 637, "Misc.LicenseCatUsed", -12);

			if (sItem.type == 4830 && itemAnimation > 0)
				LicenseOrExchangePet(sItem, ref NPC.boughtDog, 638, "Misc.LicenseDogUsed", -13);

			if (sItem.type == 4910 && itemAnimation > 0)
				LicenseOrExchangePet(sItem, ref NPC.boughtBunny, 656, "Misc.LicenseBunnyUsed", -14);
		}
		private void LicenseOrExchangePet(Item sItem, ref bool petBoughtFlag, int npcType, string textKeyForLicense, int netMessageData)
		{
			if (ItemTimeIsZero && (!petBoughtFlag || NPC.AnyNPCs(npcType))) {
				ApplyItemTime(sItem);
				NPC.UnlockOrExchangePet(ref petBoughtFlag, npcType, textKeyForLicense, netMessageData);
			}
		}
		*/
		/* NPC
		public static void UnlockOrExchangePet(ref bool petBoughtFlag, int npcType, string textKeyForLicense, int netMessageData)
		{
			Color color = new Color(50, 255, 130);
			if (Main.netMode == 1) {
				if (!petBoughtFlag || AnyNPCs(npcType))
					NetMessage.SendData(61, -1, -1, null, Main.myPlayer, netMessageData);
			}
			else if (!petBoughtFlag) {
				petBoughtFlag = true;
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey(textKeyForLicense), color);
				NetMessage.TrySendData(7);
			}
			else if (RerollVariationForNPCType(npcType)) {
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PetExchangeSuccess"), color);
			}
			else {
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PetExchangeFail"), color);
			}
		}
		public static bool RerollVariationForNPCType(int npcType)
		{
			for (int i = 0; i < 200; i++) {
				NPC nPC = Main.npc[i];
				if (nPC.active && nPC.type == npcType)
					return nPC.RerollVariation();
			}

			return false;
		}

		public bool RerollVariation()
		{
			if (!TownNPCProfiles.Instance.GetProfile(this, out var profile))
				return false;

			int num = townNpcVariationIndex;
			int num2 = 0;
			while (num2++ < 100 && townNpcVariationIndex == num) {
				townNpcVariationIndex = profile.RollVariation();
			}

			if (num == townNpcVariationIndex)
				return false;

			GivenName = profile.GetNameForVariant(this);
			life = lifeMax;
			if (Main.netMode != 1) {
				ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
				particleOrchestraSettings.PositionInWorld = base.Center;
				particleOrchestraSettings.MovementVector = velocity;
				ParticleOrchestraSettings settings = particleOrchestraSettings;
				ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PetExchange, settings);
			}

			NetMessage.TrySendData(56, -1, -1, null, whoAmI);
			return true;
		}
		*/

		public override bool ConsumeItem(Player player)
		{
			return true;
		}
		public override void OnConsumeItem(Player player)
		{
			int npcType = ModContent.NPCType<SnuggetPet>();
			if (player.whoAmI == Main.myPlayer && player.itemAnimation >= 0)
			{
				if (!RijamsModWorld.boughtSnuggetPet || NPC.AnyNPCs(npcType)) // && player.ItemTimeIsZero
				{
					player.ApplyItemTime(Item);
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						NPC.UnlockOrExchangePet(ref RijamsModWorld.boughtSnuggetPet, npcType, "Mods.RijamsMod.UI.LicenseSnuggetUse", -15);
					}
					if (Main.netMode == NetmodeID.MultiplayerClient)
					{
						string chatMessage = Language.GetTextValue("Mods.RijamsMod.UI.LicenseSnuggetUse");
						ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(chatMessage), new Color(50, 255, 130));
						RijamsModWorld.boughtSnuggetPet = true;
						NetMessage.SendData(MessageID.WorldData);
						ModPacket packet = Mod.GetPacket();
						packet.Write((byte)RijamsModMessageType.SetSnuggetTownPetArrivable);
						packet.Send();
					}
					//if (Main.netMode == NetmodeID.MultiplayerClient)
					//{
					//	NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, Main.myPlayer, -15);
					//}
					//NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, Main.myPlayer, -14);
				}
			}
		}
		public override bool? UseItem(Player player)
		{
			return true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				tooltips.Add(new TooltipLine(Mod, "SnuggetPetLicense", "[c/ff8300:Exchanging doesn't work in Multiplayer]"));
			}
		}
	}
}