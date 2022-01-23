using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RijamsMod
{
	public class RijamsModPlayer : ModPlayer
	{
		public bool guideToProperFlightTechniques;
		public bool summonersGlove;
		public bool daybreakStone;
		public bool breathingPack;
		public int breathingPackTimer;
		public bool lumothPet;
		public bool lEDLumothPet;
		public bool burglarsRing;
		public bool rocketBooster;
		public bool gamutApparatus;
		public bool frostburnStone;
		//public bool delapHelmet;
		//public bool delapScalemail;
		//public bool delapGreaves;
		//public bool breathingPackUsed = true;

		public override void ResetEffects()
		{
			guideToProperFlightTechniques = false;
			summonersGlove = false;
			daybreakStone = false;
			breathingPack = false;
			lumothPet = false;
			lEDLumothPet = false;
			burglarsRing = false;
            rocketBooster = false;
			gamutApparatus = false;
			frostburnStone = false;
			//breathingPackUsed = true;
			//breathingPackTimer = 0;
		}
        public override void clientClone(ModPlayer clientClone)
		{
			RijamsModPlayer clone = clientClone as RijamsModPlayer;
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)player.whoAmI);
			packet.Send(toWho, fromWho);
		}
		public override void SendClientChanges(ModPlayer clientPlayer)
        {
			RijamsModPlayer clone = clientPlayer as RijamsModPlayer;
		}
		public override void PostUpdateEquips()
        {
			if (guideToProperFlightTechniques)
            {
				if (player.wingTimeMax > 0)
				{
					player.wingTimeMax += 60;
					player.jumpSpeedBoost += 2f;
					player.moveSpeed += 2;
				}
			}
			if (breathingPack)
            {
				//Main.NewText("breathingPackUsed " + breathingPackUsed);
				//Main.NewText("breathingPackTimer " + breathingPackTimer);
				//Main.NewText("player.wet " + player.wet);
				if (player.breath <= 0 )//&& breathingPackUsed == false)
				{
					//breathingPackUsed = true;
					//player.GetModPlayer<RijamsModPlayer>().breathingPackUsed = true;
					breathingPackTimer++;
					if (breathingPackTimer == 1 && !Main.dedServ)
                    {
						Main.PlaySound(SoundLoader.customSoundType, (int)player.Center.X, (int)player.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/beep"), 0.5f, 1.5f);
					}
					if (breathingPackTimer == 60 && !Main.dedServ)
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)player.Center.X, (int)player.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/beep"), 0.5f, 2f);
					}
					if (breathingPackTimer >= 120)
					{
						if (!Main.dedServ)
                        {
							Main.PlaySound(SoundLoader.customSoundType, (int)player.Center.X, (int)player.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/beep"), 0.5f, 0.5f);
						}
						breathingPackTimer = 0;
						//breathingPackUsed = true;
						//player.GetModPlayer<RijamsModPlayer>().breathingPackUsed = true;
						player.breath += player.breathMax;
					}
				}
				/*if (player.wet == false || player.honeyWet == false || player.lavaWet == false)
                {
					breathingPackTimer = 0;
					//breathingPackUsed = false;
					//player.GetModPlayer<RijamsModPlayer>().breathingPackUsed = false;
				}*/
            }
        }
    }
}