using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Concurrent;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace RijamsMod.UI
{
	public class RijamsModIconChatTag : ITagHandler
	{
		// This chat tag is registered in RijamsMod.cs

		// Tags can only be 10 characters long
		public static readonly string RijamsModIcon = "rjmi";
		public static readonly string IconShadow = $"[{RijamsModIcon}:icon]";
		public static readonly string IconNoShadow = $"[{RijamsModIcon}:icns]";
		public static readonly string IconShadowGray = $"[{RijamsModIcon}:icog]";
		public static readonly string IconNoShadowGray = $"[{RijamsModIcon}:insg]";

		// Tags are set up as prefix/options:text
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			int frameIndex = 0;

			if (text == "icon")
			{
				frameIndex = 0;
			}
			else if (text == "icns")
			{
				frameIndex = 1;
			}
			else if (text == "icog")
			{
				frameIndex = 2;
			}
			else if (text == "insg")
			{
				frameIndex = 3;
			}

			if (int.TryParse(options, out int index))
			{
				frameIndex = index;
			}

			return new RijamsModIconSnippet(frameIndex, baseColor)
			{
				DeleteWhole = true,
				// Text = $"[rjmi:{text}]"
				Text = $"    " // Can't be an empty string or it doesn't show up at all.
				// Four spaces == 24px
			};
		}

		private static readonly FieldInfo Field__handlers = typeof(Terraria.UI.Chat.ChatManager).GetField("_handlers", BindingFlags.NonPublic | BindingFlags.Static);

		/// <summary> Gets ChatManager._handlers </summary>
		public static ConcurrentDictionary<string, ITagHandler> Get_ChatManager__handlers()
		{
			return (ConcurrentDictionary<string, ITagHandler>)Field__handlers.GetValue(null);
		}

		public class RijamsModIconSnippet : TextSnippet
		{
			private int iconIndex;

			private Asset<Texture2D> iconTexture;

			public RijamsModIconSnippet(int frameIndex, Color baseColor)
			{
				iconIndex = frameIndex;
				Color = baseColor;
			}

			public override bool UniqueDraw(bool justCheckingSize, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default, Color color = default, float scale = 1)
			{
				if (spriteBatch is null || justCheckingSize)
				{
					size = default;
					return false;
				}

				int frameY = iconIndex;

				iconTexture ??= ModContent.Request<Texture2D>("RijamsMod/UI/RijamsModIcons", AssetRequestMode.AsyncLoad);

				spriteBatch.Draw(iconTexture.Value, position, iconTexture.Frame(1, 4, 0, frameY), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

				size = new Vector2(24f) * scale; // I don't understand what size does.
				// size = new Vector2(2f) * scale;
				//size = Vector2.One;
				return true;
			}
		}
	}
}
