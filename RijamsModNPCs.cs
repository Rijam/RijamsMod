using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Items;
using RijamsMod.Items.Weapons;
using RijamsMod.Items.Accessories;
using RijamsMod.Items.Accessories.Vanity;
using RijamsMod.Items.Materials;

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

		public override bool SpecialNPCLoot(NPC npc)
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
						Main.PlaySound(SoundID.Item, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, 35, 0.75f, 0.5f);
					}
					else
                    {
						Main.PlaySound(SoundID.Item, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, 35, 0.75f, 0.5f);
					}*/
					Main.PlaySound(SoundID.Item, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, 35, 0.75f, 0.5f);

					//Main.PlaySound(SoundLoader.customSoundType, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/beep"), 0.25f, 2f);
				}
			}
			return base.SpecialNPCLoot(npc);
        }
		public override void NPCLoot(NPC npc)
		{
			if (npc.type == NPCID.SnowmanGangsta)
			{
				if (Main.rand.Next(15) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Thompson>());
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CarrotNose>());
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FrostyRose>());
				}
				if (Main.rand.Next(50) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GangstaHat);
				}
				if (Main.rand.Next(150) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Present);
				}
			}
			if (npc.type == NPCID.SnowBalla)
			{
				if (Main.rand.Next(15) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<LegionScarf>());
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CarrotNose>());
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FrostyRose>());
				}
				if (Main.rand.Next(50) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BallaHat);
				}
				if (Main.rand.Next(150) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Present);
				}
			}
			if (npc.type == NPCID.MisterStabby)
			{
				if (Main.rand.Next(15) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<LegionScarf>());
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CarrotNose>());
				}
				if (Main.rand.Next(15) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<StabbyShiv>());
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FrostyRose>());
				}
				if (Main.rand.Next(150) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Present);
				}
			}
			if (npc.type == NPCID.GoblinSummoner)
			{
				if (Main.rand.Next(3) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ShadowflameStaff>());
				}
			}
			if (npc.type == NPCID.ArmoredViking)
			{
				if (Main.expertMode)
				{
					if (Main.rand.Next(50) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ArmorPolish);
					}
				}
				if (!Main.expertMode)
                {
					if (Main.rand.Next(100) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ArmorPolish);
					}
				}
			}
			if (npc.type == NPCID.SantaNK1)
			{
				if (Main.expertMode)
				{
					if (Main.rand.Next(10) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<NaughtyList>());
					}
				}
				if (!Main.expertMode)
				{
					if (Main.rand.Next(20) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<NaughtyList>());
					}
				}
			}
			if (npc.type == NPCID.ChaosElemental || npc.type == NPCID.EnchantedSword)
			{
				if (Main.rand.Next(20) == 0) //5% chance, compared with the RoD's 0.2%
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Quest.TeleportationCore>());
				}
			}
			Mod consolaria = ModLoader.GetMod("Consolaria"); //Consolaria's Spectral Elemental can also drop it
			if (consolaria != null)
            {
				if (npc.type == mod.NPCType("SpectralElemental"))
				{
					if (Main.rand.Next(20) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Quest.TeleportationCore>());
					}
				}
			}
			if (npc.type == NPCID.Crimera)
            {
				if (Main.rand.Next(525) == 0) //0.19% chance
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Armor.DilapidatedCrimsonHelmet>());
				}
				if (Main.rand.Next(525) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Armor.DilapidatedCrimsonScalemail>());
				}
				if (Main.rand.Next(525) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Armor.DilapidatedCrimsonGreaves>());
				}
			}
			if (npc.type == NPCID.PresentMimic)
            {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Present, Main.rand.Next(1, 5));
			}
			if (npc.type == NPCID.Nutcracker)
			{
				if (Main.rand.Next(5) == 0)
                {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Present, Main.rand.Next(1, 3));
				}
			}
			if (npc.type == NPCID.Everscream)
			{
				if (Main.rand.Next(2) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Present, Main.rand.Next(1, 3));
				}
			}
			if (npc.type == NPCID.Poltergeist)
			{
				if (Main.rand.Next(5) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoodieBag, Main.rand.Next(1, 3));
				}
			}
			if (npc.type == NPCID.MourningWood)
			{
				if (Main.rand.Next(2) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoodieBag, Main.rand.Next(1, 3));
				}
			}
			if (npc.type == NPCID.Pumpking)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoodieBag, Main.rand.Next(1, 5));
			}
			if (npc.type == NPCID.BloodCrawler || npc.type == NPCID.BloodCrawlerWall)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CrawlerChelicera>(), Main.rand.Next(1, 2));
			}
			if (npc.type == NPCID.EyeofCthulhu && !Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.Ammo.BloodyArrow>(), Main.rand.Next(20, 50));
			}
		}
		public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
		{
			RijamsModPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<RijamsModPlayer>();
			if (projectile != null)
			{
				if (moddedplayer.daybreakStone && projectile.owner == Main.LocalPlayer.whoAmI && projectile.melee)
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
				if (moddedplayer.frostburnStone && projectile.owner == Main.LocalPlayer.whoAmI && projectile.melee)
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
				if (moddedplayer.flaskBuff == 1 && projectile.owner == Main.LocalPlayer.whoAmI && projectile.melee)
				{
					npc.AddBuff(ModContent.BuffType<Buffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
				}
			}
		}
		public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
		{
			RijamsModPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<RijamsModPlayer>();
			if (moddedplayer.daybreakStone && item.owner == Main.LocalPlayer.whoAmI && item.melee)
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
			if (moddedplayer.frostburnStone && item.owner == Main.LocalPlayer.whoAmI && item.melee)
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
			if (moddedplayer.flaskBuff == 1 && item.owner == Main.LocalPlayer.whoAmI && item.melee)
			{
				npc.AddBuff(ModContent.BuffType<Buffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
			}
		}
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			int shopPriceScaling = ModContent.GetInstance<RijamsModConfigServer>().ShopPriceScaling;
			float shopMulti = (shopPriceScaling / 100f);
			if (type == ModContent.NPCType<NPCs.TownNPCs.Fisherman>())
			{
				foreach (Item item in shop.item)
				{
					int shopPrice = item.shopCustomPrice ?? 0; //Some hackery with changing the int? type into int
					item.shopCustomPrice = (int?)Math.Round(shopPrice * shopMulti);
				}
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