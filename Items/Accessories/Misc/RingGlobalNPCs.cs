using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories.Misc
{
	public class RingGlobalNPCs : GlobalNPC
	{
		public override bool SpecialOnKill(NPC npc)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Player player = Main.player[i];
					if (!player.active)
					{
						continue;
					}
					RijamsModPlayer moddedplayer = player.GetModPlayer<RijamsModPlayer>();
					if (moddedplayer.burglarsRing || moddedplayer.warriorRing || moddedplayer.lifeSapperRing || moddedplayer.manaSapperRing)
					{
						// Mod.Logger.Debug($"Player {player.whoAmI} has Burglar's Ring");
						// Mod.Logger.Debug($"NPC is: {npc.FullName}");
						// Mod.Logger.Debug($"npc.lastInteraction is {npc.lastInteraction}");
						// Mod.Logger.Debug($"player.lastCreatureHit is {player.lastCreatureHit}");

						if (npc.immortal || npc.dontCountMe || !npc.active || npc.CountsAsACritter)
						{
							continue;
						}

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
							int chance = (int)Math.Round(20 - (player.luck * 10)); //1 in 20 chance (5%) but is affected by luck.
							if (Main.rand.NextBool(chance))
							{
								player.AddBuff(ModContent.BuffType<Buffs.Other.WarriorEnergy>(), 300);
								ModContent.GetInstance<RijamsMod>().PlayNetworkSound(new($"{nameof(RijamsMod)}/Sounds/Custom/RingPowerup") { Volume = 0.75f }, player.Center, player);
								Mod.Logger.Debug(" warriorRing Success?");
							}
						}

						// If the player has the Life Sapper Ring equipped, the NPC is not immortal, the NPC is counted, the NPC is alive, and the NPC was hit by a player
						if (moddedplayer.lifeSapperRing)
						{
							int healAmount = 1 + (Math.Max(player.statLifeMax, player.statLifeMax2) / 100); // Base of 1 + 1 for every 100 max HP. Example: 100 HP = healed 2; 400 HP = healed 5; 
							player.Heal(healAmount);
							// Mod.Logger.Debug(" lifeSapperRing Success?");
						}

						// If the player has the Life Sapper Ring equipped, the NPC is not immortal, the NPC is counted, the NPC is alive, and the NPC was hit by a player
						if (moddedplayer.manaSapperRing)
						{
							int manaAmount = 2 + (Math.Max(player.statManaMax, player.statManaMax2) / 100); // Base of 2 + 1 for every 100 max mana. Example: 20 MP = healed 2; 200 MP = healed 4; 
							player.statMana += manaAmount;
							player.ManaEffect(manaAmount);
							// Mod.Logger.Debug(" manaSapperRing Success?");
						}
					}
				}
			}
			return base.SpecialOnKill(npc);
		}
	}
}