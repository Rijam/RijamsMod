using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using RijamsMod.Items.Weapons.Melee;
using RijamsMod.Items.Weapons.Melee.JoustingLances;
using RijamsMod.Items.Weapons.Ranged;
using RijamsMod.Items.Weapons.Summon.Whips;
using RijamsMod.Items.Weapons.Summon.Minions;
using RijamsMod.Items.Weapons.Summon.Cudgels;
using RijamsMod.Items.Accessories.Defense;
using RijamsMod.Items.Accessories.Misc;
using RijamsMod.Items.Accessories.Vanity;
using RijamsMod.Items.Materials;
using RijamsMod.Items.Pets;
using RijamsMod.Buffs.Potions;
using RijamsMod.NPCs;
using RijamsMod.Items.Quest;
using RijamsMod.Items.Placeable;
using RijamsMod.Items;

namespace RijamsMod
{
	public class RijamsModNPCs : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool sulfuricAcid;
		public bool bleedingOut;

		public override void ResetEffects(NPC npc)
		{
			sulfuricAcid = false;
			bleedingOut = false;
		}

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.SnowmanGangsta)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Thompson>(), 15));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CarrotNose>(), 25));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrostyRose>(), 25));
				npcLoot.Add(ItemDropRule.Common(ItemID.GangstaHat, 50));
				npcLoot.Add(ItemDropRule.Common(ItemID.Present, 150));
			}
			if (npc.type == NPCID.SnowBalla)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LegionScarf>(), 15));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CarrotNose>(), 25));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrostyRose>(), 25));
				npcLoot.Add(ItemDropRule.Common(ItemID.BallaHat, 50));
				npcLoot.Add(ItemDropRule.Common(ItemID.Present, 150));
			}
			if (npc.type == NPCID.MisterStabby)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LegionScarf>(), 15));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CarrotNose>(), 25));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StabbyShiv>(), 15));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrostyRose>(), 25));
				npcLoot.Add(ItemDropRule.Common(ItemID.Present, 150));
			}
			if (npc.type == NPCID.GoblinSummoner)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShadowflameStaff>(), 3));
			}
			if (npc.type == NPCID.ArmoredViking)
			{
				npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.ArmorPolish, 100, 50));
			}
			if (npc.type == NPCID.SantaNK1)
			{
				npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Accessories.Summoner.NaughtyList>(), 20, 10));
				npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<FestivePlating>(), 1, 1, 5));
				npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<FestivePlating>(), 1, 2, 8));
			}
			if (npc.type == NPCID.ChaosElemental || npc.type == NPCID.EnchantedSword)
			{
				// Decrease the drop chance after the quest has been completed.
				LeadingConditionRule questComplete = new(ShopConditions.IntTravQuestTPCore.ToDropCondition(ShowItemDropInUI.Always));
				questComplete.OnFailedConditions(ItemDropRule.Common(ModContent.ItemType<TeleportationCore>(), 20));
				questComplete.OnSuccess(ItemDropRule.Common(ModContent.ItemType<TeleportationCore>(), 40));
				npcLoot.Add(questComplete);
			}
			if (ModLoader.TryGetMod("Consolaria", out Mod consolaria)) // Consolaria's Spectral Elemental can also drop it
			{
				if (consolaria.TryFind<ModNPC>("SpectralElemental", out ModNPC spectralElemental) && npc.type == spectralElemental.Type)
				{
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TeleportationCore>(), 20));
				}
			}
			if (npc.type == NPCID.Crimera)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.DilapidatedCrimson.DilapidatedCrimsonHelmet>(), 525)); //0.19% chance
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.DilapidatedCrimson.DilapidatedCrimsonScalemail>(), 525));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.DilapidatedCrimson.DilapidatedCrimsonGreaves>(), 525));
			}
			if (npc.type == NPCID.PresentMimic)
			{
				npcLoot.Add(ItemDropRule.Common(ItemID.Present, 3, 1, 6));
			}
			if (npc.type == NPCID.Nutcracker)
			{
				npcLoot.Add(ItemDropRule.Common(ItemID.Present, 5, 1, 4));
			}
			if (npc.type == NPCID.Everscream)
			{
				npcLoot.Add(ItemDropRule.Common(ItemID.Present, 2, 1, 4));
			}
			if (npc.type == NPCID.IceQueen)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FestiveWhip>(), 4));
			}
			if (npc.type == NPCID.Poltergeist)
			{
				npcLoot.Add(ItemDropRule.Common(ItemID.GoodieBag, 5, 1, 4));
			}
			if (npc.type == NPCID.HeadlessHorseman)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HorsemansJoustingLance>(), 10));
			}
			if (npc.type == NPCID.MourningWood)
			{
				npcLoot.Add(ItemDropRule.Common(ItemID.GoodieBag, 2, 1, 4));
			}
			if (npc.type == NPCID.Pumpking)
			{
				npcLoot.Add(ItemDropRule.Common(ItemID.GoodieBag, 1, 1, 3));
			}
			if (npc.type == NPCID.BloodCrawler || npc.type == NPCID.BloodCrawlerWall)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrawlerChelicera>(), 1, 1, 2));
			}
			if (npc.type == NPCID.EyeofCthulhu && !Main.expertMode && WorldGen.crimson)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.Ammo.BloodyArrow>(), 1, 20, 51));
			}
			if (npc.type == NPCID.DesertGhoul || npc.type == NPCID.DesertGhoulCorruption || npc.type == NPCID.DesertGhoulCrimson || npc.type == NPCID.DesertGhoulHallow)
			{
				npcLoot.Add(ItemDropRule.Common(ItemID.AncientCloth, 10, 1, 3));
			}
			if (npc.type == NPCID.GiantTortoise || npc.type == NPCID.IceTortoise)
			{
				npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.TurtleShell, 34, 17));
			}
			if (npc.type == NPCID.GreekSkeleton)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LonkheJoustingLance>(), 20));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Aspis>(), 10));
			}
			if (npc.type == NPCID.GraniteGolem)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Granitization>(), 20));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GraniteElementalCudgel>(), 20));
			}
			if (npc.type == NPCID.GraniteFlyer)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GraniteElementalCudgel>(), 4));
			}
			if (npc.type == NPCID.AngryBones || npc.type == NPCID.AngryBonesBig || npc.type == NPCID.AngryBonesBigHelmet || npc.type == NPCID.AngryBonesBigMuscle)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneHeadJoustingLance>(), 30));
			}
			if (npc.type == NPCID.Paladin)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FallenPaladinCudgel>(), 10));
			}
			if (npc.type == NPCID.KingSlime)
			{
				LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MorphasRing>(), 7));
				npcLoot.Add(notExpertRule);
			}
			if (npc.type == NPCID.QueenSlimeBoss)
			{
				LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CrystalClusterCudgel>(), 4));
				npcLoot.Add(notExpertRule);
			}
			if (npc.type == NPCID.Deerclops)
			{
				LeadingConditionRule notExpertRule = new (new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<StarCallerStaff>(), 4));
				notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SanityFlowerCudgel>(), 4));
				notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<TailoThreeCats>(), 4));
				npcLoot.Add(notExpertRule);
			}
			if (npc.type == NPCID.HallowBoss)
			{
				LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<RadiantLanternCudgel>(), 4));
				notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EtherealJoustingLance>(), 4));
				npcLoot.Add(notExpertRule);
			}
			// EoW and BoC will drop the Odd Device if the Interstellar Traveling has not moved in ever. But, don't show it in the Bestiary.
			if (Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1)
			{
				LeadingConditionRule leadingConditionRule = new(new Conditions.LegacyHack_IsABoss());
				leadingConditionRule.OnSuccess(ItemDropRule.ByCondition(ShopConditions.NotIntTravMovedIn.ToDropCondition(ShowItemDropInUI.Never), ModContent.ItemType<OddDevice>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.BrainofCthulhu)
			{
				npcLoot.Add(ItemDropRule.ByCondition(ShopConditions.NotIntTravMovedIn.ToDropCondition(ShowItemDropInUI.Never), ModContent.ItemType<OddDevice>()));
			}
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (sulfuricAcid)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 16;
				if (damage < 4)
				{
					damage = 4;
				}
			}
			if (bleedingOut)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 20;
			}
		}
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (sulfuricAcid && npc.active)
			{
				if (Main.rand.Next(4) < 3)
				{
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<Dusts.SulfurDust>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 2f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4))
					{
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.5f;
					}
				}
				Lighting.AddLight(npc.position, 1.0f, 1.0f, 0.0f);
			}
			if (bleedingOut && npc.active)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 0, 0, 50, default, 1f);
				drawColor.G *= (byte)0.4f;
				drawColor.B *= (byte)0.4f;
			}
		}
		/*public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (sulfuricAcid)
			{
				damage = (int)(npc.defDamage * 0.5f);
			}
		}
		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			if (sulfuricAcid)
			{
				damage = (int)(npc.defDamage * 0.5f);
			}
		}
		public override void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			if (sulfuricAcid)
			{
				damage = (int)(npc.defDamage * 0.5f);
			}
		}*/
	}
}