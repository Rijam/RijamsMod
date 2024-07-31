using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using RijamsMod.Items.Accessories.Misc;

namespace RijamsMod.Items.Fishing
{
	public class FishingPlayer : ModPlayer
	{
		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			if (attempt.inHoney && Player.ShoppingZone_BelowSurface && attempt.common && Main.rand.NextBool())
			{
				itemDrop = ModContent.ItemType<HornetTail>();
				return;
			}
			if (Player.ZoneGlowshroom && Player.ShoppingZone_BelowSurface && attempt.common && Main.rand.NextBool())
			{
				itemDrop = ModContent.ItemType<FungiEel>();
				return;
			}
		}
		public override void ModifyCaughtFish(Item fish)
		{
			if (Player.GetFishingConditions().BaitItemType == ModContent.ItemType<WildBait>() && fish.rare != ItemRarityID.Quest && !ItemID.Sets.IsFishingCrate[fish.type])
			{
				int rand = Main.rand.Next(0, 100);
				// "Best" odds: 75 - 5 - (100 / 2) = 20 aka 80% chance
				// "Worst" odds: 75 + 5 + 0 = 80 aka 20% chance
				// Main.NewText("rand " + rand);
				// Main.NewText("Player.fishingSkill " + Player.fishingSkill + " Player.GetFishingConditions().FinalFishingLevel " + Player.GetFishingConditions().FinalFishingLevel);
				if (rand >= 75 - (int)(Player.luck * 5) - ((MathHelper.Clamp(Player.GetFishingConditions().FinalFishingLevel, 0, 125) - 25) / 2))
				{
					if (fish.maxStack == 1)
					{
						Item.NewItem(new EntitySource_OverfullInventory(Player), Player.Center, fish.Clone(), noGrabDelay: true);
					}
					else
					{
						fish.stack++;
					}
				}
			}
		}
		public override void ModifyFishingAttempt(ref FishingAttempt attempt)
		{
			if (Player.GetModPlayer<RijamsModPlayer>().curiosityLure)
			{
				// Main.NewText($"Pre {attempt.common} {attempt.uncommon} {attempt.rare} {attempt.veryrare} {attempt.legendary}");

				// Values taken from Projectile.FishingCheck_RollDropLevels
				// Rolls again, doubling the chances.
				int commonRarity = Math.Clamp(150 / attempt.fishingLevel, 2, int.MaxValue);
				int uncommonRarity = Math.Clamp(150 * 2 / attempt.fishingLevel, 3, int.MaxValue);
				int rareRarity = Math.Clamp(150 * 7 / attempt.fishingLevel, 4, int.MaxValue);
				int veryRareRarity = Math.Clamp(150 * 15 / attempt.fishingLevel, 5, int.MaxValue);
				int legendaryRarity = Math.Clamp(150 * 30 / attempt.fishingLevel, 6, int.MaxValue);
				if (Main.rand.NextBool(commonRarity))
				{
					attempt.common = true;
				}
				if (Main.rand.NextBool(uncommonRarity))
				{
					attempt.uncommon = true;
				}
				if (Main.rand.NextBool(rareRarity))
				{
					attempt.rare = true;
				}
				if (Main.rand.NextBool(veryRareRarity))
				{
					attempt.veryrare = true;
				}
				if (Main.rand.NextBool(legendaryRarity))
				{
					attempt.legendary = true;
				}

				// Main.NewText($"Post {attempt.common} {attempt.uncommon} {attempt.rare} {attempt.veryrare} {attempt.legendary}");
			}
		}

		public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
		{
			// Becomes more common the more quests you complete.
			// 0 quests is 1/9 chance (because rareMultiplier gets multiplied by 0.9f).
			// 50 quests is 1/4 chance. (1f - (50 * 0.01f)) * 0.9f * 10 = 4.5
			int rand = (int)(10 * rareMultiplier);
			if (Main.rand.NextBool(rand))
			{
				rewardItems.Add(new Item(ModContent.ItemType<CuriosityLure>()));
			}
			else if (Main.rand.NextBool(rand))
			{
				rewardItems.Add(new Item(ModContent.ItemType<TrapBobber>()));
			}
			else if (Main.rand.NextBool(rand))
			{
				rewardItems.Add(new Item(ModContent.ItemType<SpinnerBobber>()));
			}
		}
	}

	public class FishingGlobalProjectile : GlobalProjectile
	{
		public override bool PreAI(Projectile projectile)
		{

			if (projectile.aiStyle == ProjAIStyleID.Bobber)
			{
				// ai[0] == 1 when reeling in
				// ai[1] is negative when a fish can be caught. After caught, becomes the item ID of the catch.
				// localAI[0] timer after catching a fish
				// localAI[1] timer until you can catch a fish. Counts up to 660. Becomes the item ID when the fish is on the line.
				// Main.NewText($" {projectile.ai[0]} {projectile.ai[1]} {projectile.localAI[0]} {projectile.localAI[1]} {projectile.localAI[2]}");

				if (Main.player[projectile.owner].GetModPlayer<RijamsModPlayer>().spinnerBobber && projectile.ai[1] == 0f && Main.myPlayer == projectile.owner)
				{
					projectile.localAI[1] += 4; // Timer until you can catch a fish. Increasing it makes you catch it faster.
				}
				if (Main.player[projectile.owner].GetModPlayer<RijamsModPlayer>().trapBobber && projectile.ai[1] < 0f)
				{
					// ai[1] gets set to Main.rand.Next(-180, -60) - 100 when a fish can be caught.
					// Increases by Main.rand.Next(1, 5) every tick. (1 to 4)
					projectile.ai[1] -= Main.rand.Next(1, 3); // 1 or 2

					// Small safety check if for some reason the number is shrinking faster than it can grow.
					// Shouldn't happen in vanilla.
					if (projectile.ai[1] < -400)
					{
						projectile.ai[1] -= projectile.ai[1] + 400; // Add the amount to get it back to -400.
					}
				}
			}

			return base.PreAI(projectile);
		}
	}
}