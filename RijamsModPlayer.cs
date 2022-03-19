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
		public bool sulfuricAcid;
		public bool ancientSet;
		public bool frostyRose;
		public bool honeyComb;
		public bool yoyoBackpack;
		public bool snuggetPet;
		public bool fluffaloPet;
		public int flaskBuff = 0;

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
			sulfuricAcid = false;
			ancientSet = false;
			frostyRose = false;
			honeyComb = false;
			yoyoBackpack = false;
			snuggetPet = false;
			fluffaloPet = false;
			flaskBuff = 0;
		}
		public override void UpdateDead()
		{
			sulfuricAcid = false;
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
			if (frostyRose)
			{
				player.buffImmune[BuffID.Frostburn] = true;
				player.buffImmune[BuffID.Frozen] = true;
				player.buffImmune[BuffID.Chilled] = true;
			}
			if (honeyComb)
            {
				player.bee = true;
            }
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
				if (player.breath <= 0)//&& breathingPackUsed == false)
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
			if (yoyoBackpack)
            {
				player.counterWeight = ProjectileID.BlackCounterweight + Main.rand.Next(6);
				player.yoyoGlove = true;
				player.yoyoString = true;
				player.stringColor = 27;
			}
		}
		public override void UpdateBadLifeRegen()
		{
			if (sulfuricAcid)
			{
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second.
				player.lifeRegen -= 16;
				player.allDamageMult *= 0.9f;
			}
		}
		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (sulfuricAcid)
			{
				if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
				{
					int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, ModContent.DustType<Dusts.SulfurDust>(), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default, 2f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}
				r *= 1.0f;
				g *= 1.0f;
				b *= 0.0f;
				fullBright = true;
			}
		}
        public override void PostUpdateRunSpeeds()
        {
            if (ancientSet)
            {
				player.runAcceleration += 0.1f;
				player.maxRunSpeed += 2;
			}
		}
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (honeyComb)
            {
				player.AddBuff(BuffID.Honey, 300);
			}			
        }
    }
}