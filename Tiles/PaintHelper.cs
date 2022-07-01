using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using ReLogic.Content;

namespace RijamsMod.Tiles
{
	/// <summary>
	/// A small class to convert paint into Colors.
	/// </summary>
	public class PaintHelper
	{
		/// <summary>
		/// Pass tile.TileColor to return its corresponding color.
		/// </summary>
		/// <param name="paintID"></param>
		/// <returns>null if no paint is applied. Color if one is.</returns>
		public static Color? GetColorFromPaintForLighting(byte paintID)
		{
			return paintID switch
			{
				PaintID.None => null,
				PaintID.RedPaint => new Color(127, 0, 0),
				PaintID.OrangePaint => new Color(127, 64, 0),
				PaintID.YellowPaint => new Color(127, 127, 0),
				PaintID.LimePaint => new Color(64, 127, 0),
				PaintID.GreenPaint => new Color(0, 127, 0),
				PaintID.TealPaint => new Color(0, 127, 64),
				PaintID.CyanPaint => new Color(0, 127, 127),
				PaintID.SkyBluePaint => new Color(0, 64, 127),
				PaintID.BluePaint => new Color(0, 0, 127),
				PaintID.PurplePaint => new Color(64, 0, 127),
				PaintID.VioletPaint => new Color(127, 0, 127),
				PaintID.PinkPaint => new Color(127, 0, 64),
				PaintID.DeepRedPaint => new Color(255, 0, 0),
				PaintID.DeepOrangePaint => new Color(255, 127, 0),
				PaintID.DeepYellowPaint => new Color(255, 255, 0),
				PaintID.DeepLimePaint => new Color(127, 255, 0),
				PaintID.DeepGreenPaint => new Color(0, 255, 0),
				PaintID.DeepTealPaint => new Color(0, 255, 127),
				PaintID.DeepCyanPaint => new Color(0, 255, 255),
				PaintID.DeepSkyBluePaint => new Color(0, 127, 255),
				PaintID.DeepBluePaint => new Color(0, 0, 255),
				PaintID.DeepPurplePaint => new Color(127, 0, 255),
				PaintID.DeepVioletPaint => new Color(255, 0, 255),
				PaintID.DeepPinkPaint => new Color(255, 0, 127),
				PaintID.BlackPaint => new Color(72, 72, 72),
				PaintID.GrayPaint => new Color(160, 160, 160),
				PaintID.WhitePaint => new Color(255, 255, 255),
				PaintID.BrownPaint => new Color(255, 191, 127),
				PaintID.ShadowPaint => new Color(1, 1, 1),
				PaintID.NegativePaint => new Color(200, 200, 255),
				PaintID.IlluminantPaint => new Color(306, 306, 306),
				_ => null,
			};
		}

		/// <summary>
		/// Normalizes the Colors from 0-255 to 0-1f
		/// </summary>
		/// <param name="color"></param>
		/// <returns>null if color is null, Vector3 otherwise.</returns>
		public static Vector3? NormalizeColors(Color? color)
		{
			if (color == null)
			{
				return null;
			}
			else
			{
				Color newColor = (Color)color;
				float r = newColor.R / 255f;
				float g = newColor.G / 255f;
				float b = newColor.B / 255f;

				return new Vector3(r, g, b);
			}
		}
	}
}