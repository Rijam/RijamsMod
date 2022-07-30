using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Items;
using RijamsMod.Items.Weapons;
using RijamsMod.Items.Weapons.Melee;
using RijamsMod.Items.Weapons.Ranged;
using RijamsMod.Items.Weapons.Summon.Whips;
using RijamsMod.Items.Weapons.Summon.Minions;
using RijamsMod.Items.Weapons.Summon.Cudgels;
using RijamsMod.Items.Accessories;
using RijamsMod.Items.Accessories.Defense;
using RijamsMod.Items.Accessories.Misc;
using RijamsMod.Items.Accessories.Vanity;
using RijamsMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using Terraria.Audio;

namespace RijamsMod
{
	public class RijamsModNPCs : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool sulfuricAcid;

		public override void ResetEffects(NPC npc)
		{
			sulfuricAcid = false;
		}
		public override bool SpecialOnKill(NPC npc)
        {
			RijamsModPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<RijamsModPlayer>();
			//Player player = Main.LocalPlayer;
			//If the player has the Burglar's Ring equipped, the NPC is not a boss, the NPC is not immortal, the NPC is counted, the NPC is alive, and the NPC was hit by a player
			if (moddedplayer.burglarsRing && (!npc.boss || !npc.immortal || !npc.dontCountMe) && npc.active && npc.lastInteraction != 255)
            {
				//Main.NewText($"NPCLoot called for NPC Id: {npc.type}");
				npc.NPCLoot();
				//Main.NewText("Success?");
				if (ModContent.GetInstance<RijamsModConfigClient>().BurglarsRingSound)
                {
					/*if (Main.netMode != NetmodeID.Server && Main.myPlayer == player.whoAmI)
                    {
						SoundEngine.PlaySound(SoundID.Item, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, 35, 0.75f, 0.5f);
					}
					else
                    {
						SoundEngine.PlaySound(SoundID.Item, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, 35, 0.75f, 0.5f);
					}*/
					SoundEngine.PlaySound(new("Terraria/Sounds/Item_35", 0, 1) { Volume = 0.5f, PitchRange = (0.75f, 0.75f)} );

					//SoundEngine.PlaySound(SoundLoader.customSoundType, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/beep"), 0.25f, 2f);
				}
			}
			return base.SpecialOnKill(npc);
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
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Quest.TeleportationCore>(), 20));
			}
			if (ModLoader.TryGetMod("Consolaria", out Mod consolaria)) //Consolaria's Spectral Elemental can also drop it
			{
				if (npc.type == consolaria.Find<ModNPC>("SpectralElemental").Type)
				{
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Quest.TeleportationCore>(), 20));
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
			}
			if (npc.type == NPCID.GraniteGolem)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Granitization>(), 20));
			}
			if (npc.type == NPCID.AngryBones || npc.type == NPCID.AngryBonesBig || npc.type == NPCID.AngryBonesBigHelmet || npc.type == NPCID.AngryBonesBigMuscle)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneHeadJoustingLance>(), 30));
			}
			if (npc.type == NPCID.Paladin)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FallenPaladinCudgel>(), 10));
			}
			if (npc.type == NPCID.QueenSlimeBoss && !Main.expertMode)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrystalClusterCudgel>(), 4));
			}
		}
		public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
		{
			RijamsModPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<RijamsModPlayer>();
			if (projectile != null)
			{
				if (moddedplayer.daybreakStone && projectile.owner == Main.LocalPlayer.whoAmI && projectile.CountsAsClass(DamageClass.Melee))
				{
					//Same chances as Magma Stone, but half duration
					if (Main.rand.Next(8) <= 2)
					{
						npc.AddBuff(BuffID.Daybreak, 180);
					}
					else if (Main.rand.Next(8) <= 3)
                    {
						npc.AddBuff(BuffID.Daybreak, 120);
					}
					else if (Main.rand.Next(8) <= 3)
					{
						npc.AddBuff(BuffID.Daybreak, 60);
					}
				}
				if (moddedplayer.frostburnStone && projectile.owner == Main.LocalPlayer.whoAmI && projectile.CountsAsClass(DamageClass.Melee))
				{
					//Same chances as Magma Stone, but half duration
					if (Main.rand.Next(8) <= 2)
					{
						npc.AddBuff(BuffID.Frostburn, 360);
					}
					else if (Main.rand.Next(8) <= 3)
					{
						npc.AddBuff(BuffID.Frostburn, 240);
					}
					else if (Main.rand.Next(8) <= 3)
					{
						npc.AddBuff(BuffID.Frostburn, 120);
					}
				}
				if (moddedplayer.flaskBuff == 1 && projectile.owner == Main.LocalPlayer.whoAmI && projectile.CountsAsClass(DamageClass.Melee))
				{
					npc.AddBuff(ModContent.BuffType<Buffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
				}
			}
		}
		public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
		{
			RijamsModPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<RijamsModPlayer>();
			if (moddedplayer.daybreakStone && item.playerIndexTheItemIsReservedFor == player.whoAmI && item.CountsAsClass(DamageClass.Melee))
			{
				//Same chances as Magma Stone, but half duration
				int dayBreakStoneRand = Main.rand.Next(8);//random number from 0 to 7
				if (dayBreakStoneRand <= 1)//0 or 1
				{
					npc.AddBuff(BuffID.Daybreak, 180);
				}
				else if (dayBreakStoneRand > 1 && dayBreakStoneRand <= 4)//2, 3, or 4
				{
					npc.AddBuff(BuffID.Daybreak, 120);
				}
				else if (dayBreakStoneRand > 4 && dayBreakStoneRand <= 7)//5, 6, or 7
				{
					npc.AddBuff(BuffID.Daybreak, 60);
				}
			}
			if (moddedplayer.frostburnStone && item.playerIndexTheItemIsReservedFor == Main.LocalPlayer.whoAmI && item.DamageType == DamageClass.Melee)
			{
				//Same chances as Magma Stone
				int dayBreakStoneRand = Main.rand.Next(8);//random number from 0 to 7
				if (dayBreakStoneRand <= 1)//0 or 1
				{
					npc.AddBuff(BuffID.Frostburn, 360);
				}
				else if (dayBreakStoneRand > 1 && dayBreakStoneRand <= 4)//2, 3, or 4
				{
					npc.AddBuff(BuffID.Frostburn, 240);
				}
				else if (dayBreakStoneRand > 4 && dayBreakStoneRand <= 7)//5, 6, or 7
				{
					npc.AddBuff(BuffID.Frostburn, 120);
				}
			}
			if (moddedplayer.flaskBuff == 1 && item.playerIndexTheItemIsReservedFor == Main.LocalPlayer.whoAmI && item.DamageType == DamageClass.Melee)
			{
				npc.AddBuff(ModContent.BuffType<Buffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
			}
		}
		/*public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			/*int shopPriceScaling = ModContent.GetInstance<RijamsModConfigServer>().ShopPriceScaling;
			float shopMulti = (shopPriceScaling / 100f);
			if (type == ModContent.NPCType<NPCs.TownNPCs.Fisherman>())
			{
				foreach (Item item in shop.item)
				{
					int shopPrice = item.shopCustomPrice ?? 0; //Some hackery with changing the int? type into int
					item.shopCustomPrice = (int?)Math.Round(shopPrice * shopMulti);
				}
			}
		}*/
		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (sulfuricAcid)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 16;
				if (damage < 2)
				{
					damage = 2;
				}
				/*int index = npc.FindBuffIndex(ModContent.BuffType<Buffs.SulfuricAcid>());
				if (npc.buffTime[index] > 2)
				{
					npc.damage = (int)(npc.defDamage * 0.9f);
				}
				if (npc.buffTime[index] <= 2)
				{
					npc.damage = npc.defDamage;
				}*/
			}
		}
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (sulfuricAcid)
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