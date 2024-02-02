﻿using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace RijamsMod.EmoteBubbles
{
	// This abstract class is used for town NPC emotes quick setup.
	public abstract class ModTownEmote : ModEmoteBubble
	{
		// Redirecting texture path.
		public override string Texture => "RijamsMod/EmoteBubbles/TownNPCEmotes";

		public override void SetStaticDefaults()
		{
			// Add NPC emotes to "Town" category.
			AddToCategory(EmoteID.Category.Town);
		}

		/// <summary>
		/// Which row of the sprite sheet is this NPC emote in?
		/// This is used to help get the correct frame rectangle for different emotes.
		/// </summary>
		public virtual int Row => 0;

		// You should decide the frame rectangle yourself by these two methods.
		public override Rectangle? GetFrame()
		{
			return new Rectangle(EmoteBubble.frame * 34, 28 * Row, 34, 28);
		}

		// Do note that you should never use EmoteBubble instance as the GetFrame() method above
		// in "Emote Menu Methods" (methods with -InEmoteMenu suffix).
		// Because in that case the value of EmoteBubble is always null.
		public override Rectangle? GetFrameInEmoteMenu(int frame, int frameCounter)
		{
			return new Rectangle(frame * 34, 28 * Row, 34, 28);
		}
	}

	// This is a showcase of using the same texture for different emotes.
	// Command names of these classes are defined using .hjson files in the Localization/ folder.
	public class InterstellarTravelerEmote : ModTownEmote
	{
		public override int Row => 0;
	}

	public class HarpyEmote : ModTownEmote
	{
		public override int Row => 1;
	}

	public class HellTraderEmote : ModTownEmote
	{
		public override int Row => 2;
	}

	public class InterstellarTravelerShimmeredEmote : ModTownEmote
	{
		public override int Row => 3;
	}

	public class HarpyShimmeredEmote : ModTownEmote
	{
		public override int Row => 4;
	}

	public class HellTraderShimmeredEmote : ModTownEmote
	{
		public override int Row => 5;
	}

	public class SnuggetEmote : ModTownEmote
	{
		public override int Row => 6;
	}
}
