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
using Terraria.Audio;

namespace RijamsMod
{
	public class RijamsModPlayer : ModPlayer
	{
		public bool guideToProperFlightTechniques;
		public bool controlGlove;
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
		public bool yoyoBackpack;
		public bool snuggetPet;
		public bool fluffaloPet;
		public bool hailfireBootsBoost;
		public int flaskBuff = 0;
		public int skywareArmorSetBonus = 0;
		public int skywareArmorSetBonusTimer = 0;
		public bool bleedingOut = false;
		public bool soaringPotion;
		public bool warriorRing;
		public bool lifeSapperRing;
		public bool manaSapperRing;

		public override void ResetEffects()
		{
			guideToProperFlightTechniques = false;
			controlGlove = false;
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
			yoyoBackpack = false;
			snuggetPet = false;
			fluffaloPet = false;
			hailfireBootsBoost = false;
			flaskBuff = 0;
			skywareArmorSetBonus = 0;
			//skywareArmorSetBonusTimer = 0;
			bleedingOut = false;
			soaringPotion = false;
			warriorRing = false;
			lifeSapperRing = false;
			manaSapperRing = false;
		}
		public override void UpdateDead()
		{
			sulfuricAcid = false;
		}
		public override void clientClone(ModPlayer clientClone)
		{
			RijamsModPlayer _ = clientClone as RijamsModPlayer;
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)Player.whoAmI);
			packet.Send(toWho, fromWho);
		}
		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			RijamsModPlayer _ = clientPlayer as RijamsModPlayer;
		}

		public static readonly SoundStyle BreathingPackBeep = new($"{nameof(RijamsMod)}/Sounds/Custom/beep")
		{
			Volume = 0.5f,
		};
		public override void PostUpdateEquips()
		{
			if (frostyRose)
			{
				Player.buffImmune[BuffID.Frostburn] = true;
				Player.buffImmune[BuffID.Frozen] = true;
				Player.buffImmune[BuffID.Chilled] = true;
			}
			if (guideToProperFlightTechniques)
			{
				if (Player.wingTimeMax > 0)
				{
					Player.wingTimeMax += 60;
					Player.jumpSpeedBoost += 2f;
					Player.moveSpeed += 2;
				}
			}
			if (soaringPotion)
			{
				Player.wingTimeMax += 30;
			}
			if (hailfireBootsBoost)
			{
				if (Player.wingTimeMax > 0)
				{
					Player.wingTimeMax += 30;
				}
				if (Player.rocketTimeMax > 0)
				{
					Player.rocketTimeMax = 8;
				}
			}
			if (breathingPack)
			{
				//Main.NewText("breathingPackUsed " + breathingPackUsed);
				//Main.NewText("breathingPackTimer " + breathingPackTimer);
				//Main.NewText("player.wet " + player.wet);
				if (Player.breath <= 0)//&& breathingPackUsed == false)
				{
					//breathingPackUsed = true;
					//player.GetModPlayer<RijamsModPlayer>().breathingPackUsed = true;
					breathingPackTimer++;
					if (breathingPackTimer == 1 && !Main.dedServ)
					{
						SoundEngine.PlaySound(new($"{nameof(RijamsMod)}/Sounds/Custom/beep") { Volume = 0.5f, Pitch = 1.5f } );
					}
					if (breathingPackTimer == 60 && !Main.dedServ)
					{
						SoundEngine.PlaySound(new($"{nameof(RijamsMod)}/Sounds/Custom/beep") { Volume = 0.5f, Pitch = 2f });
					}
					if (breathingPackTimer >= 120)
					{
						if (!Main.dedServ)
						{
							SoundEngine.PlaySound(new($"{nameof(RijamsMod)}/Sounds/Custom/beep") { Volume = 0.5f, Pitch = 0.5f });
						}
						breathingPackTimer = 0;
						//breathingPackUsed = true;
						//player.GetModPlayer<RijamsModPlayer>().breathingPackUsed = true;
						Player.breath += Player.breathMax;
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
				Player.counterWeight = ProjectileID.BlackCounterweight + Main.rand.Next(6);
				Player.yoyoGlove = true;
				Player.yoyoString = true;
				Player.stringColor = 27;
			}
		}
		public override void UpdateBadLifeRegen()
		{
			if (sulfuricAcid)
			{
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
				}
				Player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second.
				Player.lifeRegen -= 16;
				Player.GetDamage(DamageClass.Generic) *= 0.9f;
			}
			if (bleedingOut)
			{
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
				}
				Player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 10 life lost per second.
				Player.lifeRegen -= 20;
				Player.moveSpeed *= 0.9f;
				Player.GetAttackSpeed(DamageClass.Generic) *= 0.9f;
			}
		}
		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (sulfuricAcid)
			{
				if (drawInfo.drawPlayer.active && !drawInfo.drawPlayer.dead)
				{
					if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
					{
						int dust = Dust.NewDust(drawInfo.drawPlayer.position - new Vector2(2f, 2f), Player.width + 4, Player.height + 4, ModContent.DustType<Dusts.SulfurDust>(), Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 2f);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 1.8f;
						Main.dust[dust].velocity.Y -= 0.5f;
						if (Main.rand.NextBool(4))
						{
							Main.dust[dust].noGravity = false;
							Main.dust[dust].scale *= 0.5f;
						}
					}
					r *= 1.0f;
					g *= 1.0f;
					b *= 0.0f;
					fullBright = true;
				}
			}
			if (bleedingOut)
			{
				if (drawInfo.drawPlayer.active && !drawInfo.drawPlayer.dead)
				{
					for (int i = 0; i < 5; i++)
					{
						Dust.NewDust(drawInfo.drawPlayer.position, drawInfo.drawPlayer.width, drawInfo.drawPlayer.height, DustID.Blood, 0, 0, 50, default, 1f);
					}
					r *= 1.0f;
					g *= 0.4f;
					b *= 0.4f;
				}
			}
		}
		public override void PostUpdateRunSpeeds()
		{
			if (ancientSet)
			{
				Player.runAcceleration += 0.1f;
				Player.maxRunSpeed += 2;
			}
		}
	}
}