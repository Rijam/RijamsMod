using RijamsMod.NPCs.TownNPCs;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace RijamsMod.NPCs;

public class ChatWithPortrait()
{
	public readonly struct DialogWithEmotion(string chat, PortraitEmotion emotion)
	{
		public readonly string Chat => chat;
		public readonly PortraitEmotion Emotion => emotion;
	}

	private readonly WeightedRandom<DialogWithEmotion> _chatDB = new();
	public WeightedRandom<DialogWithEmotion> ChatDB => _chatDB;

	public void Clear()
	{
		ChatDB.Clear();
	}

	public void Add(string dialog, double weight = 1.0, PortraitEmotion emotion = PortraitEmotion.Neutral)
	{
		ChatDB.Add(new DialogWithEmotion(dialog, emotion), weight);
	}

	public void Add(LocalizedText key, double weight = 1.0, PortraitEmotion emotion = PortraitEmotion.Neutral)
	{
		ChatDB.Add(new DialogWithEmotion(key.ToString(), emotion), weight);
	}
	public string GetRandomChat()
	{
		return ChatDB.Get().Chat;
	}

	public PortraitEmotion GetEmotion(string dialog)
	{
		return ChatDB.elements.FirstOrDefault(c => c.Item1.Chat == dialog)?.Item1.Emotion ?? PortraitEmotion.Neutral;
	}

	public static PortraitEmotion GetEmotion()
	{
		int talkNPC = Main.LocalPlayer.talkNPC;
		if (talkNPC < 0 || talkNPC >= Main.maxNPCs)
		{
			return PortraitEmotion.Neutral;
		}

		ModNPC modNPC = Main.npc[talkNPC]?.ModNPC;
		ChatWithPortrait chatEmotion = null;
		if (modNPC is Harpy harpy)
		{
			chatEmotion = harpy.chatEmotion;
		}
		if (modNPC is HellTrader hellTrader)
		{
			chatEmotion = hellTrader.chatEmotion;
		}
		if (modNPC is InterstellarTraveler interstellarTraveler)
		{
			chatEmotion = interstellarTraveler.chatEmotion;
		}

		if (chatEmotion is null)
		{
			return PortraitEmotion.Neutral;
		}

		return chatEmotion.ChatDB.elements.FirstOrDefault(c => c.Item1.Chat == Main.npcChatText)?.Item1.Emotion ?? PortraitEmotion.Neutral;
	}

	public static bool PortraitEmotionCondtion(PortraitEmotion emotion)
	{
		if (GetEmotion() == emotion)
		{
			return true;
		}
		return false;
	}
	public static bool ShimmerPortraitEmotionCondtion(PortraitEmotion emotion)
	{
		if (NPCID.Sets.ShimmeredPortraitCondition() && GetEmotion() == emotion)
		{
			return true;
		}
		return false;
	}
	public static bool PartyPortraitEmotionCondtion(PortraitEmotion emotion)
	{
		if (NPCHelper.PartyPortraitCondition() && GetEmotion() == emotion)
		{
			return true;
		}
		return false;
	}

	public static bool ShimmerPartyPortraitEmotionCondtion(PortraitEmotion emotion)
	{
		if (NPCHelper.ShimmerPartyPortraitCondition() && GetEmotion() == emotion)
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// Stages are 0-82%, 82%-100%, 100%-110%, 110%-inf
	/// </summary>
	/// <param name="minHappiness"></param>
	/// <param name="maxHappiness"></param>
	/// <returns></returns>
	public static bool ShowingHappinessText(float minHappiness, float maxHappiness)
	{
		// Main.NewText($"{Main.LocalPlayer.currentShoppingSettings.PriceAdjustment} minHappiness {minHappiness} maxHappiness {maxHappiness}");
		return Main.LocalPlayer.currentShoppingSettings.HappinessReport == Main.npcChatText &&
			minHappiness <= Main.LocalPlayer.currentShoppingSettings.PriceAdjustment
			&& Main.LocalPlayer.currentShoppingSettings.PriceAdjustment < maxHappiness;
	}

	public static bool ShimmerShowingHappinessText(float minHappiness, float maxHappiness)
	{
		return NPCID.Sets.ShimmeredPortraitCondition() && ShowingHappinessText(minHappiness, maxHappiness);
	}

	public static bool PartyShowingHappinessText(float minHappiness, float maxHappiness)
	{
		return NPCHelper.PartyPortraitCondition() && ShowingHappinessText(minHappiness, maxHappiness);
	}

	public static bool ShimmerPartyShowingHappinessText(float minHappiness, float maxHappiness)
	{
		return NPCHelper.ShimmerPartyPortraitCondition() && ShowingHappinessText(minHappiness, maxHappiness);
	}

	public static bool ShowingHousingText()
	{
		int talkNPC = Main.LocalPlayer.talkNPC;
		if (talkNPC < 0 || talkNPC >= Main.maxNPCs)
		{
			return false;
		}

		NPC npc = Main.npc[talkNPC];
		// Same thing that NPCInteractions.Actions.RequestHome does to construct the chat.
		string housingChat;
		if (npc.ModNPC is ModNPC modNPC)
		{
			housingChat = Language.GetTextValue(modNPC.GetLocalizationKey("TownNPCMood") + ".NoHome");
		}
		else
		{
			housingChat = Language.GetTextValue("TownNPCMood_" + NPCID.Search.GetName(talkNPC) + ".NoHome");
		}
		housingChat += "\n\n";
		if (npc.type == NPCID.Truffle)
		{
			housingChat += Language.GetTextValueWith("HousingText.HousingRequirements_Truffle", new
			{
				NPCName = npc.FullName
			});
		}
		else if (npc.ModNPC is ModNPC modNPC2 && Language.Exists($"{modNPC2.GetLocalizationKey("HousingText")}.HousingRequirements"))
		{
			// If Mods.ModName.NPCs.NPCName.HousingText.HousingRequirements exists, use that as the text.
			housingChat += Language.GetTextValueWith($"{modNPC2.GetLocalizationKey("HousingText")}.HousingRequirements", new
			{
				NPCName = npc.FullName
			});
		}
		else
		{
			housingChat += Language.GetTextValue("HousingText.HousingRequirements");
		}

		return Main.npcChatText == housingChat;
	}

	public static bool ShimmerShowingHousingText()
	{
		return NPCID.Sets.ShimmeredPortraitCondition() && ShowingHousingText();
	}
	public static bool PartyShowingHousingText()
	{
		return NPCHelper.PartyPortraitCondition() && ShowingHousingText();
	}
	public static bool ShimmerPartyShowingHousingText()
	{
		return NPCHelper.ShimmerPartyPortraitCondition() && ShowingHousingText();
	}

	public static string PortraitPath(object classname, string prefix, string mood)
	{
		return $"{classname.GetType().Namespace.Replace('.', '/')}/Portraits/{classname.GetType().Name}/{prefix}_{mood}";
	}

	public static NPCID.Sets.BasicNPCPortrait BasicPortrait(object classname, string prefix, string mood)
	{
		return NPCID.Sets.BasicPortrait(PortraitPath(classname, prefix, mood));
	}

	public static string PortraitPath(string nameSpace, string classname, string prefix, string mood)
	{
		return $"{nameSpace}/Portraits/{classname}/{prefix}_{mood}";
	}
}

public enum PortraitEmotion
{
	Neutral, // 😐
	Happy, // 🙂
	Sad, // 😢
	Angry, // 😠
	Thinking, // 🤔
	VeryHappy, // 😀
	Shocked, // 😲 
	Smirk, // 😏
	Blushing, // 😳 
	Worried // 😟
}
