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
		// Accessories
		public bool Accessory_GuideToProperFlightTechniques;
		public bool Accessory_ControlGlove;
		public bool Accessory_SummonersGlove;
		public bool Accessory_DaybreakStone;
		public bool Accessory_BreathingPack;
		public int Accessory_BreathingPackTimer;
		public bool Accessory_BurglarsRing;
		public bool Accessory_RocketBooster;
		public bool Accessory_GamutApparatus;
		public bool Accessory_FrostburnStone;
		public bool Accessory_FrostyRose;
		public bool Accessory_YoyoBackpack;
		public bool Accessory_HailfireBootsBoost;
		public bool Accessory_WarriorRing;
		public bool Accessory_LifeSapperRing;
		public bool Accessory_ManaSapperRing;
		public bool Accessory_TerraStepStool;
		public bool Accessory_LoopingOil;
		public bool Accessory_SideEffects;
		public bool Accessory_CuriosityLure;
		public bool Accessory_TrapBobber;
		public bool Accessory_SpinnerBobber;
		public bool Accessory_PeakPerformanceRing;

		// Pets
		public bool Pet_LumothPet;
		public bool Pet_LEDLumothPet;
		public bool Pet_SnuggetPet;
		public bool Pet_FluffaloPet;
		public bool Pet_DwarfStarPet;
		public bool Pet_BabyStardustDragonPet;

		// Buffs
		public bool Buff_SulfuricAcid;
		public int Buff_ModFlaskBuff = FlaskIDs.None;
		public bool Buff_BleedingOut = false;
		public bool Buff_SoaringPotion;
		public bool Buff_Oiled;
		public bool Buff_OnShadowflame;
		public bool Buff_BetsysCurse;
		public bool Buff_DryadsBane;
		public bool Buff_OnDaybroken;

		// Stats
		/// <summary> Increases the radius for support minions. Number is in tiles. </summary>
		public int Stat_SupportMinionRadiusIncrease = 0;
		/// <summary> Increases damage for critical hits. Crit damage. </summary>
		public float Stat_CriticalHitAdditionalDamage = 0f;
		/// <summary> Changes how resistant to knockback the player is. 1f is 0% resistance, 0f is 100% resistance. </summary>
		public float Stat_KnockbackSusceptibility = 1f;
		/// <summary> Grants additional yoyos and counterweights when the player has the Yoyo Glove. </summary>
		public int Stat_BonusYoyosAndCounterweights = 0;
		/// <summary> Grants additional counterweights when the player has counterweights. </summary>
		public int Stat_BonusCounterweights = 0;
		/// <summary> The maximum amount of damage reduction to grant based on the player's movement speed. </summary>
		public float Stat_MoveSpeedDamageReductionMax = 0f;

		// Armor
		public bool Armor_AncientSet;
		public int Armor_SkywareArmorSetBonus = 0;
		public int Armor_SkywareArmorSetBonusTimer = 0;
		public bool Armor_RedSkywareLeggings;

		public override void ResetEffects()
		{
			Accessory_GuideToProperFlightTechniques = false;
			Accessory_ControlGlove = false;
			Accessory_SummonersGlove = false;
			Accessory_DaybreakStone = false;
			Accessory_BreathingPack = false;
			Accessory_BurglarsRing = false;
			Accessory_RocketBooster = false;
			Accessory_GamutApparatus = false;
			Accessory_FrostburnStone = false;
			Accessory_FrostyRose = false;
			Accessory_YoyoBackpack = false;
			Accessory_HailfireBootsBoost = false;
			Accessory_WarriorRing = false;
			Accessory_LifeSapperRing = false;
			Accessory_ManaSapperRing = false;
			Accessory_TerraStepStool = false;
			Accessory_LoopingOil = false;
			Accessory_SideEffects = false;
			Accessory_CuriosityLure = false;
			Accessory_TrapBobber = false;
			Accessory_SpinnerBobber = false;
			Accessory_PeakPerformanceRing = false;

			Pet_LumothPet = false;
			Pet_LEDLumothPet = false;
			Pet_SnuggetPet = false;
			Pet_FluffaloPet = false;
			Pet_DwarfStarPet = false;
			Pet_BabyStardustDragonPet = false;

			Buff_SulfuricAcid = false;
			Buff_ModFlaskBuff = FlaskIDs.None;
			Buff_BleedingOut = false;
			Buff_SoaringPotion = false;
			Buff_Oiled = false;
			Buff_OnShadowflame = false;
			Buff_BetsysCurse = false;
			Buff_DryadsBane = false;
			Buff_OnDaybroken = false;

			Armor_AncientSet = false;
			Armor_SkywareArmorSetBonus = 0;
			Armor_RedSkywareLeggings = false;

			Stat_SupportMinionRadiusIncrease = 0;
			Stat_CriticalHitAdditionalDamage = 0f;
			Stat_KnockbackSusceptibility = 1f;
			Stat_BonusYoyosAndCounterweights = 0;
			Stat_BonusCounterweights = 0;
			Stat_MoveSpeedDamageReductionMax = 0f;
			if (!Accessory_HailfireBootsBoost)
			{
				Player.rocketTimeMax = 7;
			}
		}

		public override void UpdateDead()
		{
			Buff_SulfuricAcid = false;
			Buff_Oiled = false;
			Buff_OnShadowflame = false;
			Buff_BetsysCurse = false;
			Buff_DryadsBane = false;
			Buff_OnDaybroken = false;
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
			if (Accessory_FrostyRose)
			{
				Player.buffImmune[BuffID.Frostburn] = true;
				Player.buffImmune[BuffID.Frozen] = true;
				Player.buffImmune[BuffID.Chilled] = true;
			}
			if (Accessory_GuideToProperFlightTechniques)
			{
				if (Player.wingTimeMax > 0)
				{
					Player.wingTimeMax += 60;
					Player.jumpSpeedBoost += 2f;
					Player.moveSpeed += 1f;
				}
			}
			if (Buff_SoaringPotion)
			{
				Player.wingTimeMax += 30;
			}
			if (Armor_RedSkywareLeggings)
			{
				Player.wingTimeMax += 60;
				Player.moveSpeed += 0.2f;
				Player.maxRunSpeed += 0.2f;
			}
			if (Accessory_HailfireBootsBoost)
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
			if (Accessory_BreathingPack)
			{
				//Main.NewText("breathingPackUsed " + breathingPackUsed);
				//Main.NewText("breathingPackTimer " + breathingPackTimer);
				//Main.NewText("player.wet " + player.wet);
				if (Player.breath <= 0)//&& breathingPackUsed == false)
				{
					//breathingPackUsed = true;
					//player.GetModPlayer<RijamsModPlayer>().breathingPackUsed = true;
					Accessory_BreathingPackTimer++;
					if (Accessory_BreathingPackTimer == 1 && !Main.dedServ)
					{
						SoundEngine.PlaySound(BreathingPackBeep with { Pitch = 1.5f } );
					}
					if (Accessory_BreathingPackTimer == 60 && !Main.dedServ)
					{
						SoundEngine.PlaySound(BreathingPackBeep with { Pitch = 2f });
					}
					if (Accessory_BreathingPackTimer >= 120)
					{
						if (!Main.dedServ)
						{
							SoundEngine.PlaySound(BreathingPackBeep with { Pitch = 0.5f });
						}
						Accessory_BreathingPackTimer = 0;
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
			if (Accessory_YoyoBackpack)
			{
				if (Player.counterWeight == 0)
				{
					if (Main.rand.NextBool(7))
						Player.counterWeight = ProjectileID.PinkCounterweight;
					else
						Player.counterWeight = ProjectileID.BlackCounterweight + Main.rand.Next(6);
				}
				Player.yoyoGlove = true;
				Player.yoyoString = true;
				Player.stringColor = 27;
				Stat_BonusCounterweights += 2;
				Accessory_LoopingOil = true;
			}

			if (Stat_MoveSpeedDamageReductionMax > 0f)
			{
				// Main.NewText(Player.velocity.Length());
				double potentialDR = Player.velocity.Length() / 50.0;
				// Main.NewText(potentialDR);
				Player.endurance += (float)Math.Round(Math.Clamp(potentialDR, 0.0, Stat_MoveSpeedDamageReductionMax), 3);
				// Main.NewText(Player.endurance);
			}
		}
		public override void UpdateBadLifeRegen()
		{
			if (Buff_SulfuricAcid)
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
			if (Buff_BleedingOut)
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
				if (Buff_SulfuricAcid)
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
				if (Buff_BleedingOut)
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
			if (Armor_AncientSet)
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
			// if (GlobalItems.isLanternWeapon.Contains(drawInfo.drawPlayer.HeldItem.type))
			if (CustomItemIDSets.IsLanternWeapon[drawInfo.drawPlayer.HeldItem.type])
			{
				drawInfo.weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.CritDamage += Stat_CriticalHitAdditionalDamage;
		}
		public override void ModifyHurt(ref Player.HurtModifiers modifiers)
		{
			float clampedKnockbackSusceptibility = MathHelper.Clamp(Stat_KnockbackSusceptibility, 0, 10);
			modifiers.Knockback *= clampedKnockbackSusceptibility;
		}

		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
		{
			//modifiers.FinalDamage *= moveSpeedDamageReductionCurrent;
		}

		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Accessory_DaybreakStone && item.DamageType.CountsAsClass<MeleeDamageClass>())
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
			if (Accessory_FrostburnStone && item.DamageType.CountsAsClass<MeleeDamageClass>())
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
			if (Buff_ModFlaskBuff == FlaskIDs.SulfuricAcid)
			{
				target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
			}
			if (Buff_ModFlaskBuff == FlaskIDs.Oiled)
			{
				target.AddBuff(BuffID.Oiled, 150 + Main.rand.Next(0, 120));
			}
			if (Buff_ModFlaskBuff == FlaskIDs.Daybroken)
			{
				target.AddBuff(BuffID.Daybreak, 150 + Main.rand.Next(0, 120));
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if ((proj.DamageType.CountsAsClass<MeleeDamageClass>() || ProjectileID.Sets.IsAWhip[proj.type]) && !proj.noEnchantments)
			{
				if (Buff_ModFlaskBuff == FlaskIDs.SulfuricAcid)
				{
					target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
				}
				if (Buff_ModFlaskBuff == FlaskIDs.Oiled)
				{
					target.AddBuff(BuffID.Oiled, 150 + Main.rand.Next(0, 120));
				}
				if (Buff_ModFlaskBuff == FlaskIDs.Daybroken)
				{
					target.AddBuff(BuffID.Daybreak, 150 + Main.rand.Next(0, 120));
				}
				if (Accessory_DaybreakStone)
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
				if (Accessory_FrostburnStone)
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
				if (Accessory_DaybreakStone)
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SolarFlare, Player.velocity.X * 0.2f + (Player.direction * 3), Player.velocity.Y * 0.2f, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.7f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.Yellow.ToVector3() * 0.875f);
				}
				if (Accessory_FrostburnStone)
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Frost, Player.velocity.X * 0.2f + (Player.direction * 3), Player.velocity.Y * 0.2f, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.7f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.LightBlue.ToVector3() * 0.875f);
				}
				if (Buff_ModFlaskBuff >= 1)
				{
					int dustType = DustID.Dirt;
					if (Buff_ModFlaskBuff == FlaskIDs.SulfuricAcid)
					{
						dustType = ModContent.DustType<Dusts.SulfurDust>();
						Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.Yellow.ToVector3() * 0.1f);
					}
					if (Buff_ModFlaskBuff == FlaskIDs.Oiled)
					{
						dustType = DustID.Asphalt;
					}
					if (Buff_ModFlaskBuff == FlaskIDs.Daybroken)
					{
						dustType = DustID.SolarFlare;
						Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.LightGoldenrodYellow.ToVector3() * 0.1f);
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
				if (Accessory_DaybreakStone)
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
				if (Accessory_FrostburnStone)
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
				if (Buff_ModFlaskBuff >= 1)
				{
					int dustType = DustID.Dirt;
					if (Buff_ModFlaskBuff == FlaskIDs.SulfuricAcid)
					{
						dustType = ModContent.DustType<Dusts.SulfurDust>();
						Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.1f);
					}
					if (Buff_ModFlaskBuff == FlaskIDs.Oiled)
					{
						dustType = DustID.Asphalt;
					}
					if (Buff_ModFlaskBuff == FlaskIDs.Daybroken)
					{
						dustType = DustID.SolarFlare;
						Lighting.AddLight(projectile.Center, Color.LightGoldenrodYellow.ToVector3() * 0.1f);
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