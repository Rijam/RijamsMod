using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace RijamsMod
{
	public class RijamsModNPCs : GlobalNPC
	{
		/*public override bool InstancePerEntity => true;
		public bool enemySlow;

		public override void ResetEffects(NPC npc)
		{
			enemySlow = false;
		}
		public override void SetDefaults(NPC npc)
		{
			// We want our EnemySlow buff to follow the same immunities as normal Slow
			npc.buffImmune[ModContent.BuffType<Buffs.EnemySlow>()] = npc.buffImmune[BuffID.Slow];
		}*/
		public override void NPCLoot(NPC npc)
		{
			if (npc.type == NPCID.SnowmanGangsta)
			{
				if (Main.rand.Next(15) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Thompson"));
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CarrotNose"));
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrostyRose"));
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
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LegionScarf"));
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CarrotNose"));
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrostyRose"));
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
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LegionScarf"));
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CarrotNose"));
				}
				if (Main.rand.Next(15) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StabbyShiv"));
				}
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrostyRose"));
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
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowflameStaff"));
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
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NaughtyList"));
					}
				}
				if (!Main.expertMode)
				{
					if (Main.rand.Next(20) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NaughtyList"));
					}
				}
			}
			if (npc.type == NPCID.ChaosElemental || npc.type == NPCID.EnchantedSword)
			{
				if (Main.rand.Next(20) == 0) //5% chance, compared with the RoD's 0.2%
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TeleportationCore"));
				}
			}
			Mod consolaria = ModLoader.GetMod("Consolaria"); //Consolaria's Specral Elemental can also drop it
			if (consolaria != null)
            {
				if (npc.type == mod.NPCType("SpectralElemental"))
				{
					if (Main.rand.Next(20) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TeleportationCore"));
					}
				}
			}
			if (npc.type == NPCID.Crimera)
            {
				if (Main.rand.Next(525) == 0) //0.19% chance
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DilapidatedCrimsonHelmet"));
				}
				if (Main.rand.Next(525) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DilapidatedCrimsonScalemail"));
				}
				if (Main.rand.Next(525) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DilapidatedCrimsonGreaves"));
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
		}
		public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
		{
			RijamsModPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<RijamsModPlayer>();
			if (projectile != null)
			{
				if (moddedplayer.daybreakStone && projectile.owner == Main.LocalPlayer.whoAmI && projectile.melee)
				{
					if (projectile.melee == true)
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
				}
			}
		}
		public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
		{
			RijamsModPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<RijamsModPlayer>();
			if (moddedplayer.daybreakStone && item.owner == Main.LocalPlayer.whoAmI && item.melee)
			{
				if (item.melee == true)
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
			}
		}
	}
}