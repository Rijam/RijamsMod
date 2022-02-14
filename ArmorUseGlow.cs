using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace RijamsMod
{
	/// <summary>
	/// Adapted from SGAmod by IDGCaptainRussia94, SGAPlayer_Layers.cs
	/// Adds Glowmasks to armor pieces
	/// Modified to include FemaleBody
	/// </summary>
    public class ArmorUseGlow : ModPlayer
    {
		public string[] armorglowmasks = new string[5];
		public Func<Player, int, Color>[] armorglowcolor = {delegate (Player player,int index)
		{
			return Color.White;
		},
			delegate (Player player,int index)
		{
			return Color.White;
		},
			delegate (Player player,int index)
		{
			return Color.White;
		},
			delegate (Player player,int index)
		{
			return Color.White;
		},
			delegate (Player player,int index)
		{
			return Color.White;
		} };

        public override void ResetEffects()
        {
			for (int i = 0; i < armorglowmasks.Length; i += 1)
			{
				armorglowmasks[i] = null;
				armorglowcolor[i] = delegate (Player player, int index)
				{
					return Color.White;
				};
			}
		}

        public static void DrawGlowmasks(PlayerDrawInfo drawInfo, int index)
		{

			Player drawPlayer = drawInfo.drawPlayer;
			RijamsMod mod = RijamsMod.Instance;
			ArmorUseGlow modply = drawPlayer.GetModPlayer<ArmorUseGlow>();
			Color GlowColor = modply.armorglowcolor[index](drawPlayer, index);

			Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

			if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
				color = drawInfo.bodyColor * drawInfo.bodyColor.A;

			if (modply.armorglowmasks[index] != null && !drawPlayer.mount.Active)
			{
				Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[index]);
				if (index == 1 && !drawPlayer.Male)
                {
					texture = ModContent.GetTexture(modply.armorglowmasks[4]);
				}

				int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
				int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) + drawPlayer.gfxOffY - Main.screenPosition.Y);//gravDir 
				DrawData data;
				if (index == 3)
					data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.legFrame.Y, drawPlayer.legFrame.Width, drawPlayer.legFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.legFrame.Width / 2, drawPlayer.legFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
				else
					data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
				data.shader = (int)drawPlayer.dye[index > 1 ? index - 1 : index].dye;

				Main.playerDrawData.Add(data);
			}
		}
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			ArmorUseGlow modply = player.GetModPlayer<ArmorUseGlow>();
			string[] stringsz = { "Head", "Body", "Arms", "Legs" };
			PlayerLayer[] thelayer = { PlayerLayer.Head, PlayerLayer.Body, PlayerLayer.Arms, PlayerLayer.Legs };

			for (int intc = 0; intc < 4; intc += 1)
			{

				if (modply.armorglowmasks[intc] != null)
				{
					Action<PlayerDrawInfo> glowtarget;
					switch (intc)//donno why but passing the value here from the for loop causes a crash, boo
					{
						case 1://Body
							glowtarget = s => DrawGlowmasks(s, 1);
							break;
						case 2://Arms
							glowtarget = s => DrawGlowmasks(s, 2);
							break;
						case 3://Legs
							glowtarget = s => DrawGlowmasks(s, 3);
							break;
						case 4://FemaleBody
							glowtarget = s => DrawGlowmasks(s, 3);
							break;
						default://Head
							glowtarget = s => DrawGlowmasks(s, 0);
							break;
					}
					PlayerLayer glowlayer = new PlayerLayer("RijamsMod", "Armor Glowmask", thelayer[intc], glowtarget);
					int layer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals(stringsz[intc])) + 1;
					glowlayer.visible = true;
					layers.Insert(layer, glowlayer);
				}
			}
		}
	}
}