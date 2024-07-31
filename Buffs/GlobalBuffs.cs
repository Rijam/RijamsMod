using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs
{
    public class GlobalBuffs : GlobalBuff
    {
		/*
        public override void Update(int type, Player player, ref int buffIndex)
        {
            // Not needed anymore. The last Well Fed buff overrides any previous Well Fed buff.
            // Precedence: ExceptionalFeast > WellFed3 > WellFed2 > WellFed > Satiated
            if (player.HasBuff(ModContent.BuffType<ExceptionalFeast>()) && player.HasBuff(BuffID.WellFed))
            {
                player.ClearBuff(BuffID.WellFed);
                buffIndex--;
            }
            if (player.HasBuff(ModContent.BuffType<ExceptionalFeast>()) && player.HasBuff(ModContent.BuffType<Satiated>()))
            {
                player.ClearBuff(ModContent.BuffType<Satiated>());
                buffIndex--;
            }
            if (player.HasBuff(ModContent.BuffType<ExceptionalFeast>()) && player.HasBuff(BuffID.WellFed) && player.HasBuff(ModContent.BuffType<Satiated>()))
            {
                player.ClearBuff(ModContent.BuffType<Satiated>());
                player.ClearBuff(BuffID.WellFed);
                buffIndex -= 2;
            }
            if (player.HasBuff(ModContent.BuffType<Satiated>()) && player.HasBuff(BuffID.WellFed))
            {
                player.ClearBuff(ModContent.BuffType<Satiated>());
                buffIndex--;
            }
        }
		*/
		public override void Update(int type, Player player, ref int buffIndex)
		{
			if (type == BuffID.Oiled)
			{
				player.GetModPlayer<GlobalBuffsPlayer>().Oiled = true;
			}
			if (type == BuffID.ShadowFlame)
			{
				player.GetModPlayer<GlobalBuffsPlayer>().ShadowFlame = true;
			}
			if (type == BuffID.BetsysCurse)
			{
				player.GetModPlayer<GlobalBuffsPlayer>().BetsysCurse = true;
				player.statDefense -= 40;
			}
			if (type == BuffID.DryadsWardDebuff)
			{
				player.GetModPlayer<GlobalBuffsPlayer>().DryadsBane = true;
			}
			if (type == BuffID.Daybreak)
			{
				player.GetModPlayer<GlobalBuffsPlayer>().Daybroken = true;
			}
		}
		public override void SetStaticDefaults()
		{
			Main.debuff[BuffID.Oiled] = true;
			Main.pvpBuff[BuffID.Oiled] = true;
			Main.buffNoSave[BuffID.Oiled] = true;
			BuffID.Sets.LongerExpertDebuff[BuffID.Oiled] = true;

			Main.debuff[BuffID.ShadowFlame] = true;
			Main.pvpBuff[BuffID.ShadowFlame] = true;
			Main.buffNoSave[BuffID.ShadowFlame] = true;
			BuffID.Sets.LongerExpertDebuff[BuffID.ShadowFlame] = true;

			Main.debuff[BuffID.BetsysCurse] = true;
			Main.pvpBuff[BuffID.BetsysCurse] = true;
			Main.buffNoSave[BuffID.BetsysCurse] = true;
			BuffID.Sets.LongerExpertDebuff[BuffID.BetsysCurse] = true;
			
			Main.debuff[BuffID.DryadsWardDebuff] = true;
			Main.pvpBuff[BuffID.DryadsWardDebuff] = true;
			Main.buffNoSave[BuffID.DryadsWardDebuff] = true;
			
			Main.debuff[BuffID.Daybreak] = true;
			Main.pvpBuff[BuffID.Daybreak] = true;
			Main.buffNoSave[BuffID.Daybreak] = true;
		}
	}

	public class GlobalBuffsPlayer : RijamsModPlayer
	{
		public bool Oiled { get => oiled; set => oiled = value; }
		public bool ShadowFlame { get => onShadowflame; set => onShadowflame = value; }
		public bool BetsysCurse { get => betsysCurse; set => betsysCurse = value; }
		public bool DryadsBane { get => dryadsBane; set => dryadsBane = value; }
		public bool Daybroken { get => onDaybroken; set => onDaybroken = value; }

		public override void UpdateBadLifeRegen()
		{
			base.UpdateBadLifeRegen();
			if (Oiled && (Player.onFire || Player.onFire2 || Player.onFire3 || Player.onFrostBurn || Player.onFrostBurn2 || ShadowFlame))
			{
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
				}
				Player.lifeRegenTime = 0;
				Player.lifeRegen -= 50;
			}
			if (ShadowFlame)
			{
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
				}
				Player.lifeRegenTime = 0;

				Player.lifeRegen -= 30;
			}
			if (Daybroken)
			{
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
				}

				/*int numberOfDaybreaksInPlayer = 0;
				//int num9 = 4;
				for (int l = 0; l < 1000; l++)
				{
					if (Main.projectile[l].active && Main.projectile[l].type == ProjectileID.Daybreak && Main.projectile[l].ai[0] == 1f && Main.projectile[l].ai[1] == (float)Player.whoAmI)
						numberOfDaybreaksInPlayer++;
				}

				if (numberOfDaybreaksInPlayer == 0)
				{
					numberOfDaybreaksInPlayer = 1;
				}
				*/

				Player.lifeRegen -= 200;
				// Player.lifeRegen -= numberOfDaybreaksInPlayer * 2 * 100;
				// if (num < num8 * 100 / num9)
				//	 num = num8 * 100 / num9;
			}
			if (DryadsBane)
			{
				int baseDamage = 4;
				float additiveDamage = 1f;
				if (Player.lifeRegen > 0)
					Player.lifeRegen = 0;

				if (NPC.downedBoss1)
					additiveDamage += 0.1f;

				if (NPC.downedBoss2)
					additiveDamage += 0.1f;

				if (NPC.downedBoss3)
					additiveDamage += 0.1f;

				if (NPC.downedQueenBee)
					additiveDamage += 0.1f;

				if (Main.hardMode)
					additiveDamage += 0.4f;

				if (NPC.downedMechBoss1)
					additiveDamage += 0.15f;

				if (NPC.downedMechBoss2)
					additiveDamage += 0.15f;

				if (NPC.downedMechBoss3)
					additiveDamage += 0.15f;

				if (NPC.downedPlantBoss)
					additiveDamage += 0.15f;

				if (NPC.downedGolemBoss)
					additiveDamage += 0.15f;

				if (NPC.downedAncientCultist)
					additiveDamage += 0.15f;

				if (Main.expertMode)
					additiveDamage *= Main.GameModeInfo.TownNPCDamageMultiplier;

				baseDamage = (int)(baseDamage * additiveDamage);
				Player.lifeRegen -= 2 * baseDamage;
			}
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
			if (drawInfo.drawPlayer.active && !drawInfo.drawPlayer.dead)
			{
				if (Oiled)
				{
					if (!Main.rand.NextBool(3))
					{
						if (Main.rand.NextBool(2))
						{
							Dust oilDust = Dust.NewDustDirect(Player.position - new Vector2(-2f, -2f), Player.width + 4, Player.height + 2, DustID.TintableDust, 0f, 0f, 175, new(0, 0, 0, 250), 1.4f);
							if (Main.rand.NextBool(2))
							{
								oilDust.alpha += 25;
							}

							if (Main.rand.NextBool(2))
							{
								oilDust.alpha += 25;
							}

							oilDust.noLight = true;
							oilDust.velocity *= 0.2f;
							oilDust.velocity.Y += 0.2f;
							oilDust.velocity += Player.velocity;
						}
					}
					r *= 0.4f;
					g *= 0.4f;
					b *= 0.4f;
				}
				if (ShadowFlame && Main.rand.Next(5) < 4)
				{
					Dust shadowflameDust = Dust.NewDustDirect(new Vector2(Player.position.X - 2f, Player.position.Y - 2f), Player.width + 4, Player.height + 4, DustID.Shadowflame, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 180, default, 1.95f);
					shadowflameDust.noGravity = true;
					shadowflameDust.velocity *= 0.75f;
					shadowflameDust.velocity.X *= 0.75f;
					shadowflameDust.velocity.Y -= 1f;
					if (Main.rand.NextBool(4))
					{
						shadowflameDust.noGravity = false;
						shadowflameDust.scale *= 0.5f;
					}
				}
				if (DryadsBane && Main.rand.NextBool(4))
				{
					Dust draydsBaneDust = Dust.NewDustDirect(new Vector2(Player.position.X - 2f, Player.position.Y), Player.width + 4, Player.height, DustID.PoisonStaff, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 1.5f);
					draydsBaneDust.noGravity = true;
					draydsBaneDust.velocity *= new Vector2(Main.rand.NextFloat() * 4f - 2f, 0f);
					draydsBaneDust.noLight = true;
				}
				if (BetsysCurse)
				{
					if (Main.rand.Next(4) < 3)
					{
						Dust betsysCurseDust = Dust.NewDustDirect(new Vector2(Player.position.X - 2f, Player.position.Y - 2f), Player.width + 4, Player.height + 4, DustID.Pixie, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 3.5f);
						betsysCurseDust.noGravity = true;
						betsysCurseDust.velocity *= 2.8f;
						betsysCurseDust.velocity.Y -= 1.5f;
						betsysCurseDust.noGravity = false;
						betsysCurseDust.scale = 0.9f;
						betsysCurseDust.color = new Color(0, 0, 180, 255);
						betsysCurseDust.velocity *= 0.2f;
					}

					Lighting.AddLight((int)(Player.position.X / 16f), (int)(Player.position.Y / 16f + 1f), 0.6f, 0.1f, 0.9f);
				}
				if (Daybroken)
				{
					if (Main.rand.Next(4) < 3)
					{
						Dust daybrokenDust = Dust.NewDustDirect(new Vector2(Player.position.X - 2f, Player.position.Y - 2f), Player.width + 4, Player.height + 4, DustID.OrangeTorch, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 3.5f);
						daybrokenDust.noGravity = true;
						daybrokenDust.velocity *= 2.8f;
						daybrokenDust.velocity.Y -= 0.5f;
						if (Main.rand.NextBool(4))
						{
							daybrokenDust.noGravity = false;
							daybrokenDust.scale *= 0.5f;
						}
					}

					Lighting.AddLight((int)(Player.position.X / 16f), (int)(Player.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
				}
			}
		}
	}
}
