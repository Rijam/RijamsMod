using Microsoft.Xna.Framework;
using RijamsMod.Buffs.Potions;
using RijamsMod.Items;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

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
		public int flaskBuff = FlaskIDs.None;
		public int skywareArmorSetBonus = 0;
		public int skywareArmorSetBonusTimer = 0;
		public bool bleedingOut = false;
		public bool soaringPotion;
		public bool warriorRing;
		public bool lifeSapperRing;
		public bool manaSapperRing;
		public bool terraStepStool;
		public bool dwarfStarPet;
		public bool redSkywareLeggings;
		public bool babyStardustDragonPet;
		public bool loopingOil;
		public bool sideEffects;
		public bool curiosityLure;
		public bool trapBobber;
		public bool spinnerBobber;
		public bool oiled;
		public bool onShadowflame;
		public bool betsysCurse;
		public bool dryadsBane;
		public bool onDaybroken;

		public int supportMinionRadiusIncrease = 0;
		public float criticalHitAdditionalDamage = 0f;
		public float knockbackSusceptibility = 1f;
		public int bonusYoyosAndCounterweights = 0;
		public int bonusCounterweights = 0;

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
			flaskBuff = FlaskIDs.None;
			skywareArmorSetBonus = 0;
			bleedingOut = false;
			soaringPotion = false;
			warriorRing = false;
			lifeSapperRing = false;
			manaSapperRing = false;
			terraStepStool = false;
			dwarfStarPet = false;
			redSkywareLeggings = false;
			babyStardustDragonPet = false;
			loopingOil = false;
			sideEffects = false;
			curiosityLure = false;
			trapBobber = false;
			spinnerBobber = false;
			oiled = false;
			onShadowflame = false;
			betsysCurse = false;
			dryadsBane = false;
			onDaybroken = false;

			supportMinionRadiusIncrease = 0;
			criticalHitAdditionalDamage = 0f;
			knockbackSusceptibility = 1f;
			bonusYoyosAndCounterweights = 0;
			bonusCounterweights = 0;
			if (!hailfireBootsBoost)
			{
				Player.rocketTimeMax = 7;
			}
		}

		public override void UpdateDead()
		{
			sulfuricAcid = false;
			oiled = false;
			onShadowflame = false;
			betsysCurse = false;
			dryadsBane = false;
			onDaybroken = false;
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)Player.whoAmI);
			packet.Send(toWho, fromWho);
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
					Player.moveSpeed += 1f;
				}
			}
			if (soaringPotion)
			{
				Player.wingTimeMax += 30;
			}
			if (redSkywareLeggings)
			{
				Player.wingTimeMax += 60;
				Player.moveSpeed += 0.2f;
				Player.maxRunSpeed += 0.2f;
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
						SoundEngine.PlaySound(BreathingPackBeep with { Pitch = 1.5f } );
					}
					if (breathingPackTimer == 60 && !Main.dedServ)
					{
						SoundEngine.PlaySound(BreathingPackBeep with { Pitch = 2f });
					}
					if (breathingPackTimer >= 120)
					{
						if (!Main.dedServ)
						{
							SoundEngine.PlaySound(BreathingPackBeep with { Pitch = 0.5f });
						}
						breathingPackTimer = 0;
						//breathingPackUsed = true;
						//player.GetModPlayer<RijamsModPlayer>().breathingPackUsed = true;
						Player.breath += Player.breathMax;
						if (Main.netMode == NetmodeID.Server)
						{
							NetMessage.SendData(MessageID.SyncPlayer);
						}
					}
				}
				//if (player.wet == false || player.honeyWet == false || player.lavaWet == false)
				//{
					//breathingPackTimer = 0;
					//breathingPackUsed = false;
					//player.GetModPlayer<RijamsModPlayer>().breathingPackUsed = false;
				//}
			}
			if (yoyoBackpack)
			{
				Player.counterWeight = ProjectileID.BlackCounterweight + Main.rand.Next(6);
				Player.yoyoGlove = true;
				Player.yoyoString = true;
				Player.stringColor = 27;
				bonusCounterweights += 2;
				loopingOil = true;
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
			if (drawInfo.drawPlayer.active && !drawInfo.drawPlayer.dead)
			{
				if (sulfuricAcid)
				{
					if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
					{
						Dust dust = Dust.NewDustDirect(drawInfo.drawPlayer.position - new Vector2(2f, 2f), Player.width + 4, Player.height + 4, ModContent.DustType<Dusts.SulfurDust>(), Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 2f);
						dust.noGravity = true;
						dust.velocity *= 1.8f;
						dust.velocity.Y -= 0.5f;
						if (Main.rand.NextBool(4))
						{
							dust.noGravity = false;
							dust.scale *= 0.5f;
						}
					}
					r *= 1.0f;
					g *= 1.0f;
					b *= 0.0f;
					fullBright = true;
				}
				if (bleedingOut)
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
			if (Player.armor[2].type == ModContent.ItemType<Items.Armor.GodsentKing.GodsentKingPants>())
			{
				Player.runAcceleration += 0.1f;
				Player.maxRunSpeed += 1;
				Player.accRunSpeed += 1;
			}
		}
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
		{
			// Move the lantern weapons to draw behind the back arm.
			if (GlobalItems.isLanternWeapon.Contains(drawInfo.drawPlayer.HeldItem.type))
			{
				drawInfo.weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.CritDamage += criticalHitAdditionalDamage;
		}
		public override void ModifyHurt(ref Player.HurtModifiers modifiers)
		{
			MathHelper.Clamp(knockbackSusceptibility, 0, 10);
			modifiers.Knockback *= knockbackSusceptibility;
		}

		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (daybreakStone && item.DamageType.CountsAsClass<MeleeDamageClass>())
			{
				//Same chances as Magma Stone, but half duration
				int dayBreakStoneRand = Main.rand.Next(8);//random number from 0 to 7
				if (dayBreakStoneRand <= 1)//0 or 1
				{
					target.AddBuff(BuffID.Daybreak, 180);
				}
				else if (dayBreakStoneRand > 1 && dayBreakStoneRand <= 4)//2, 3, or 4
				{
					target.AddBuff(BuffID.Daybreak, 120);
				}
				else if (dayBreakStoneRand > 4 && dayBreakStoneRand <= 7)//5, 6, or 7
				{
					target.AddBuff(BuffID.Daybreak, 60);
				}
			}
			if (frostburnStone && item.DamageType.CountsAsClass<MeleeDamageClass>())
			{
				//Same chances as Magma Stone
				int dayBreakStoneRand = Main.rand.Next(8);//random number from 0 to 7
				if (dayBreakStoneRand <= 1)//0 or 1
				{
					target.AddBuff(BuffID.Frostburn2, 360);
				}
				else if (dayBreakStoneRand > 1 && dayBreakStoneRand <= 4)//2, 3, or 4
				{
					target.AddBuff(BuffID.Frostburn2, 240);
				}
				else if (dayBreakStoneRand > 4 && dayBreakStoneRand <= 7)//5, 6, or 7
				{
					target.AddBuff(BuffID.Frostburn2, 120);
				}
			}
			if (flaskBuff == FlaskIDs.SulfuricAcid)
			{
				target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
			}
			if (flaskBuff == FlaskIDs.Oiled)
			{
				target.AddBuff(BuffID.Oiled, 150 + Main.rand.Next(0, 120));
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if ((proj.DamageType.CountsAsClass<MeleeDamageClass>() || ProjectileID.Sets.IsAWhip[proj.type]) && !proj.noEnchantments)
			{
				if (flaskBuff == FlaskIDs.SulfuricAcid)
				{
					target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
				}
				if (flaskBuff == FlaskIDs.Oiled)
				{
					target.AddBuff(BuffID.Oiled, 150 + Main.rand.Next(0, 120));
				}
				if (daybreakStone)
				{
					//Same chances as Magma Stone, but half duration
					if (Main.rand.Next(8) <= 2)
					{
						target.AddBuff(BuffID.Daybreak, 180);
					}
					else if (Main.rand.Next(8) <= 3)
					{
						target.AddBuff(BuffID.Daybreak, 120);
					}
					else if (Main.rand.Next(8) <= 3)
					{
						target.AddBuff(BuffID.Daybreak, 60);
					}
				}
				if (frostburnStone)
				{
					//Same chances as Magma Stone
					if (Main.rand.Next(8) <= 2)
					{
						target.AddBuff(BuffID.Frostburn2, 360);
					}
					else if (Main.rand.Next(8) <= 3)
					{
						target.AddBuff(BuffID.Frostburn2, 240);
					}
					else if (Main.rand.Next(8) <= 3)
					{
						target.AddBuff(BuffID.Frostburn2, 120);
					}
				}
			}
		}

		public override void MeleeEffects(Item item, Rectangle hitbox)
		{
			if (item.DamageType.CountsAsClass<MeleeDamageClass>() && !item.noMelee && !item.noUseGraphic && Main.rand.NextBool(2))
			{
				if (daybreakStone)
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SolarFlare, Player.velocity.X * 0.2f + (Player.direction * 3), Player.velocity.Y * 0.2f, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.7f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.Yellow.ToVector3() * 0.875f);
				}
				if (frostburnStone)
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Frost, Player.velocity.X * 0.2f + (Player.direction * 3), Player.velocity.Y * 0.2f, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.7f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.LightBlue.ToVector3() * 0.875f);
				}
				if (flaskBuff >= 1)
				{
					int dustType = DustID.Dirt;
					if (flaskBuff == FlaskIDs.SulfuricAcid)
					{
						dustType = ModContent.DustType<Dusts.SulfurDust>();
						Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.Yellow.ToVector3() * 0.1f);
					}
					if (flaskBuff == FlaskIDs.Oiled)
					{
						dustType = DustID.Asphalt;
					}
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustType, Player.velocity.X * 0.2f + (Player.direction * 3), Player.velocity.Y * 0.2f, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.7f;
					Main.dust[dust].velocity.Y -= 0.5f;
				}
			}
		}

		public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
		{
			if ((projectile.DamageType.CountsAsClass<MeleeDamageClass>() || ProjectileID.Sets.IsAWhip[projectile.type]) && !projectile.noEnchantments && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
			{
				if (daybreakStone)
				{
					if (projectile.friendly && !projectile.hostile && !projectile.noEnchantmentVisuals && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
					{
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SolarFlare, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default, 1f);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0.7f;
						Main.dust[dust].velocity.Y -= 0.5f;
						Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.875f);
					}
				}
				if (frostburnStone)
				{
					if (projectile.friendly && !projectile.hostile && !projectile.noEnchantmentVisuals && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)))
					{
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Frost, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default, 1f);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0.7f;
						Main.dust[dust].velocity.Y -= 0.5f;
						Lighting.AddLight(projectile.Center, Color.LightBlue.ToVector3() * 0.875f);
					}
				}
				if (flaskBuff >= 1)
				{
					int dustType = DustID.Dirt;
					if (flaskBuff == FlaskIDs.SulfuricAcid)
					{
						dustType = ModContent.DustType<Dusts.SulfurDust>();
						Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.1f);
					}
					if (flaskBuff == FlaskIDs.Oiled)
					{
						dustType = DustID.Asphalt;
					}
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.7f;
					Main.dust[dust].velocity.Y -= 0.5f;
				}
			}
		}
	}
}