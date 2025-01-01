using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	public class RingGlobalNPCs : GlobalNPC
	{
		/*public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (npc.life <= 0)
			{
				RunRingLogic(npc, player);
			}
		}
		public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (npc.life < 0)
			{
				Player owner = Main.player[projectile.owner];
				if (owner.active && projectile.owner == Main.myPlayer)
				{
					RunRingLogic(npc, owner);
				}
			}
		}*/
		public override bool SpecialOnKill(NPC npc)
		{
			if (npc.lastInteraction != 255)
			{
				foreach (Player player in Main.ActivePlayers)
				{
					if (player.dead)
					{
						continue;
					}
					// ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"player.whoAmI {player.whoAmI} npc.lastInteraction {npc.lastInteraction}"), Color.White);
					if (player.whoAmI == npc.lastInteraction)
					{
						// ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"Running Ring logic with player.whoAmI {player.whoAmI} npc.lastInteraction {npc.lastInteraction}"), Color.LightBlue);
						RunRingLogic(npc, player);
						break;
					}
				}
			}
			return base.SpecialOnKill(npc);
		}

		public void RunRingLogic(NPC npc, Player player)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				RijamsModPlayer moddedplayer = player.GetModPlayer<RijamsModPlayer>();
				if (moddedplayer.burglarsRing || moddedplayer.warriorRing || moddedplayer.lifeSapperRing || moddedplayer.manaSapperRing)
				{
					// Mod.Logger.Debug($"Player {player.whoAmI} has Burglar's Ring");
					// Mod.Logger.Debug($"NPC is: {npc.FullName}");
					// Mod.Logger.Debug($"npc.lastInteraction is {npc.lastInteraction}");
					// Mod.Logger.Debug($"player.lastCreatureHit is {player.lastCreatureHit}");

					// ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"1"), Color.Yellow);

					if (npc.immortal || npc.dontCountMe || !npc.active || npc.CountsAsACritter)
					{
						return;
					}

					// ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"2"), Color.Yellow);

					// If the player has the Burglar's Ring equipped, the NPC is not a boss, the NPC is not immortal, the NPC is counted, the NPC is alive, and the NPC was hit by a player
					if (moddedplayer.burglarsRing && !npc.boss)
					{
						// Mod.Logger.Debug($"NPCLoot called for NPC Id: {npc.type}. Who Am I: {npc.whoAmI}. To banner: {Item.NPCtoBanner(npc.type)}");
						npc.NPCLoot();
						// Mod.Logger.Debug(" burglarsRing Success?");
						float volume = ModContent.GetInstance<RijamsModConfigClient>().BurglarsRingSound / 100f;
						if (volume > 0)
						{
							SoundEngine.PlaySound(SoundID.Item35 with { Volume = volume, Pitch = 0.75f }, player.Center);
							// ModContent.GetInstance<RijamsMod>().PlayNetworkSound(SoundID.Item35 with { Volume = volume, Pitch = 0.75f }, player.Center, player);
						}
					}

					// If the player has the Warrior Ring equipped, the NPC is not immortal, the NPC is counted, the NPC is alive, and the NPC was hit by a player
					if (moddedplayer.warriorRing)
					{
						int chance = (int)Math.Round(10 - (player.luck * 10)); //1 in 10 chance (10%) but is affected by luck.
						if (Main.rand.NextBool(chance))
						{
							player.AddBuff(ModContent.BuffType<Buffs.Other.WarriorEnergy>(), 600);
							ModContent.GetInstance<RijamsMod>().PlayNetworkSound(new($"{nameof(RijamsMod)}/Sounds/Custom/RingPowerup") { Volume = 0.75f }, player.Center, player);
							// Mod.Logger.Debug(" warriorRing Success?");
							ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.TrueNightsEdge, new ParticleOrchestraSettings
							{
								PositionInWorld = player.Center,
								MovementVector = Vector2.Zero
							});
						}
					}

					// If the player has the Life Sapper Ring equipped, the NPC is not immortal, the NPC is counted, the NPC is alive, and the NPC was hit by a player
					if (moddedplayer.lifeSapperRing && !player.moonLeech)
					{
						int healAmount = 1 + (Math.Max(player.statLifeMax, player.statLifeMax2) / 100); // Base of 1 + 1 for every 100 max HP. Example: 100 HP = healed 2; 400 HP = healed 5; 
						if (Main.netMode == NetmodeID.SinglePlayer)
						{
							player.Heal(healAmount);
						}
						else
						{
							player.statLife += healAmount;
							if (player.statLife > player.statLifeMax2)
							{
								player.statLife = player.statLifeMax2;
							}

							// ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"lifeSapperRing Success?"), Color.OrangeRed);

							NetMessage.SendData(MessageID.SpiritHeal, number: player.whoAmI, number2: healAmount);
						}
						// Mod.Logger.Debug(" lifeSapperRing Success?");
					}

					// If the player has the Life Sapper Ring equipped, the NPC is not immortal, the NPC is counted, the NPC is alive, and the NPC was hit by a player
					if (moddedplayer.manaSapperRing && (player.HeldItem.DamageType.CountsAsClass(DamageClass.Magic) || player.HeldItem.ModItem is MagicMeleeGlow))
					{
						int manaAmount = 2 + (Math.Max(player.statManaMax, player.statManaMax2) / 100); // Base of 2 + 1 for every 100 max mana. Example: 20 MP = healed 2; 200 MP = healed 4; 

						ModContent.GetInstance<RijamsMod>().HealMana(player.whoAmI, manaAmount);

						// Mod.Logger.Debug(" manaSapperRing Success?");
						// ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"manaSapperRing Success?"), Color.OrangeRed);
					}
				}
			}
		}
	}
}