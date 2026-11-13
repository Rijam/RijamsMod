using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RijamsMod.Items.Materials;
using RijamsMod.Items.Weapons.Melee.JoustingLances;
using RijamsMod.Items.Weapons.Ranged;
using RijamsMod.Items.Weapons.Ranged.Ammo;
using RijamsMod.Items.Weapons.Summon.Whips;

namespace RijamsMod.Items
{
	public class GlobalItems : GlobalItem
	{
		/// <summary> This set is a set of all whips. </summary>
		// public static List<int> isWhip = new() { ItemID.BlandWhip, ItemID.ThornWhip, ItemID.BoneWhip, ItemID.FireWhip,
		// 	ItemID.CoolWhip, ItemID.SwordWhip, ItemID.MaceWhip, ItemID.ScytheWhip, ItemID.RainbowWhip };
		/// <summary> This set is a set of all jousting lances and their projectiles. </summary>
		// public static Dictionary<int, int> isJoustingLance = new()
		// {
		//		{ ItemID.JoustingLance, ProjectileID.JoustingLance},
		//		{ ItemID.HallowJoustingLance, ProjectileID.HallowJoustingLance },
		// 		{ ItemID.ShadowJoustingLance, ProjectileID.ShadowJoustingLance}
		// };

		/// <summary>
		/// This set is a set of all lantern weapons (like the Nightglow, but does not include the Nightglow).
		/// Items in this set will automatically be drawn behind the player's back hand.
		/// </summary>
		// public static List<int> isLanternWeapon = new(); // Nightglow not included.

		/// <summary>
		/// The set for the Combat Flare Pistol and Triple Barrel Flare Pistol
		/// </summary>
		// public static List<int> isCombatFlareGun = new();

		/*/// <summary>
		/// The front arm of the player will not animate correctly when the useStyle is set to RaiseLamp (14). Items in this set will be corrected with an IL Edit.
		/// </summary>
		// public static List<int> fixItemUseStyleIDRaiseLampFrontArmAnimation = new(); // No longer needed. Fix implemented into tModLoader directly. PR #3530*/

		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[ItemID.WormTooth] = ModContent.ItemType<CrawlerChelicera>(); // Shimmer transforms the item.
			ItemID.Sets.ShimmerTransformToItem[ItemID.Handgun] = ModContent.ItemType<StockadeCrossbow>(); // Shimmer transforms the item.
			ItemID.Sets.ShimmerTransformToItem[ItemID.FleshKnuckles] = ItemID.PutridScent; // Shimmer transforms the item.
			ItemID.Sets.ShimmerTransformToItem[ItemID.PutridScent] = ItemID.FleshKnuckles; // Shimmer transforms the item.
			ItemID.Sets.ShimmerTransformToItem[ItemID.DarkLance] = ModContent.ItemType<DarkJoustingLance>(); // Shimmer transforms the item.
		}

		public override void SetDefaults(Item item)
		{
			if (item.type == ItemID.Coal)
			{
				item.maxStack = Item.CommonMaxStack;
			}
			if (item.type == ItemID.PinkPricklyPear)
			{
				item.consumable = true;
				item.useStyle = ItemUseStyleID.EatFood;
				item.useAnimation = 15;
				item.useTime = 15;
				item.useTurn = true;
				item.UseSound = SoundID.Item2;
				item.buffType = ModContent.BuffType<Buffs.Potions.Satiated>(); //Specify an existing buff to be applied when used.
				item.buffTime = 3600; //1 minute
			}
			if (item.ModItem != null && item.ModItem.Mod == Mod) //Hacky solution because I'm lazy lol
			{
				if (item.ResearchUnlockCount != 0)
				{
					return;
				}
				if (item.maxStack == 1)
				{
					item.ResearchUnlockCount = 1;
				}
				else if (item.consumable)
				{
					if (item.buffType > BuffID.ObsidianSkin)
					{
						item.ResearchUnlockCount = 30;
					}
					else if (item.createTile > TileID.Stone)
					{
						item.ResearchUnlockCount = 100;
					}
					else if (item.createWall > WallID.Stone)
					{
						item.ResearchUnlockCount = 400;
					}
					else if (item.ammo > 1)
					{
						item.ResearchUnlockCount = 99;
					}
					else if (item.makeNPC > 1)
					{
						item.ResearchUnlockCount = 3;
					}
					else if (ItemID.Sets.BossBag[item.type])
					{
						item.ResearchUnlockCount = 3;
					}
					else
					{
						item.ResearchUnlockCount = 10;
					}
				}
				else if (item.ModItem.GetType().Namespace.ToString() == "RijamsMod.Items.Materials")
				{
					item.ResearchUnlockCount = 25;
				}
				else if (item.dye > 1)
				{
					item.ResearchUnlockCount = 3;
				}
				else
				{
					item.ResearchUnlockCount = 2;
				}
			}
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			RijamsModConfigServer.VanillaArmorList vanillaArmorChanges = ModContent.GetInstance<RijamsModConfigServer>().VanillaArmorChanges;
			bool isLeftShiftHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);

			if (item.buffType == ModContent.BuffType<Buffs.Potions.ExceptionalFeast>())
			{
				if (isLeftShiftHeld)
				{
					tooltips.Add(new TooltipLine(Mod, "ExceptionalFeast", "Exceptional Feast provides:"));
					tooltips.Add(new TooltipLine(Mod, "EFStats1", "+5 Defense"));
					tooltips.Add(new TooltipLine(Mod, "EFStats2", "+5% Critical Hit Chance"));
					tooltips.Add(new TooltipLine(Mod, "EFStats3", "+12.5% Melee Speed"));
					tooltips.Add(new TooltipLine(Mod, "EFStats4", "+12.5% Damage"));
					tooltips.Add(new TooltipLine(Mod, "EFStats5", "+1.25 Minion Knockback"));
					tooltips.Add(new TooltipLine(Mod, "EFStats6", "+50% Movement Speed"));
					tooltips.Add(new TooltipLine(Mod, "EFStats7", "+20% Mining Speed"));
					tooltips.Add(new TooltipLine(Mod, "EFOverride", "Counts as a Well Fed buff"));
				}
				else
				{
					tooltips.Add(new TooltipLine(Mod, "EFInfo", "Hold Left Shift for more information"));
				}
			}
			if (item.buffType == ModContent.BuffType<Buffs.Potions.Satiated>())
			{
				if (item.type == ItemID.PinkPricklyPear)
				{
					if (FindTooltipIndex(tooltips, "Tooltip0", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "SItemInfo", "Minuscule improvements to all stats"));
					}
				}
			   
				if (isLeftShiftHeld)
				{
					tooltips.Add(new TooltipLine(Mod, "Satiated", "Satiated provides:"));
					tooltips.Add(new TooltipLine(Mod, "SStats1", "+1 Defense"));
					tooltips.Add(new TooltipLine(Mod, "SStats2", "+1% Critical Hit Chance"));
					tooltips.Add(new TooltipLine(Mod, "SStats3", "+2.5% Melee Speed"));
					tooltips.Add(new TooltipLine(Mod, "SStats4", "+2.5% Damage"));
					tooltips.Add(new TooltipLine(Mod, "SStats5", "+0.25 Minion Knockback"));
					tooltips.Add(new TooltipLine(Mod, "SStats6", "+10% Movement Speed"));
					tooltips.Add(new TooltipLine(Mod, "SStats7", "Does not provide increased life regeneration in Expert Mode"));
					tooltips.Add(new TooltipLine(Mod, "SOverride", "Counts as a Well Fed buff"));
				}
				else
				{
					tooltips.Add(new TooltipLine(Mod, "SInfo", "Hold Left Shift for more information"));
				}
			}
			if (item.type == ItemID.HoneyCandle)
			{
				if (FindTooltipIndex(tooltips, "Placeable", "Terraria", out int index))
				{
					tooltips.Insert(index + 1, new TooltipLine(Mod, "Buff", "Provides the Honey buff when placed nearby"));
				}
			}
			if (item.type == ItemID.JoustingLance)
			{
				TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.Mod == "Terraria");
				line?.Text = Language.GetTextValue("ItemTooltip.JoustingLance").Split("\n")[0] + "\n7.65 tile reach";
				TooltipLine line2 = tooltips.FirstOrDefault(x => x.Name == "Tooltip3" && x.Mod == "Terraria");
			}
			if (item.type == ItemID.HallowJoustingLance)
			{
				TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.Mod == "Terraria");
				line?.Text = Language.GetTextValue("ItemTooltip.HallowJoustingLance") + "\n7.65 tile reach";
			}
			if (item.type == ItemID.ShadowJoustingLance)
			{
				TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.Mod == "Terraria");
				line?.Text = Language.GetTextValue("ItemTooltip.ShadowJoustingLance") + "\n7.65 tile reach\nInflicts Shadowflame";
			}
			if (item.type == ItemID.FairyQueenMagicItem) // Nightglow
			{
				if (FindTooltipIndex(tooltips, "UseMana", "Terraria", out int index))
				{
					tooltips.Insert(index + 1, new TooltipLine(Mod, "LanternStats", "3 Very Fast projectiles\n50 tile homing radius\nBounces off tiles\nPierces 2 times"));
				}
			}
			RijamsModConfigClient configClient = ModContent.GetInstance<RijamsModConfigClient>();
			if (CustomItemIDSets.IsWhip[item.type] && (isLeftShiftHeld && configClient.DisplayWhipMultihitPenalty == RijamsModConfigClient.WhipMultihitPenalty.HoldShift || configClient.DisplayWhipMultihitPenalty == RijamsModConfigClient.WhipMultihitPenalty.On))
			{
				if (item.type == ModContent.ItemType<Belt>())
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "ModWhipDamageReduction", "50% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.CobWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "60% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.SlimeWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "60% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.BlandWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "50% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.ThornWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "40% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.CorruptWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "40% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.CrimsonWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "40% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ModContent.ItemType<VileWhip>())
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "ModWhipDamageReduction", "30% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ModContent.ItemType<ViciousWhip>())
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "ModWhipDamageReduction", "30% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.MeteorWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "40% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.BoneWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "10% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ModContent.ItemType<TailoThreeCats>())
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "25% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.FireWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "33% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ModContent.ItemType<SulfuricWhip>())
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "ModWhipDamageReduction", "40% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.CoolWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "30% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ModContent.ItemType<ForbiddenWhip>())
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "ModWhipDamageReduction", "10% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.SwordWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "20% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.FlowerWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "15% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.MaceWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "5% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.ScytheWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "10% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ModContent.ItemType<FestiveWhip>())
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "ModWhipDamageReduction", "20% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.EelWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "5% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.RainbowWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "20% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.ConstellationWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "10% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ItemID.MoonLordWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "5% damage penalty per enemy pierced"));
					}
				}
				if (item.type == ModContent.ItemType<SupernovaWhip>())
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "ModWhipDamageReduction", "5% damage penalty per enemy pierced"));
					}
				}
			}
		}

		/// <summary>
		/// Finds the index of a certain tooltip line.
		/// </summary>
		/// <param name="tooltips">The list of tooltips.</param>
		/// <param name="name">The name of the tooltip to find. See the docs for all of the names.</param>
		/// <param name="mod">Which mod the tooltip line is from. "Terraria" for vanilla.</param>
		/// <param name="index">Out: the index of the tooltip line. 0 if not found.</param>
		/// <param name="silenceWarning">The function will print a warning if the tooltip was not found. Pass true to silence.</param>
		/// <returns>True if found.</returns>
		public static bool FindTooltipIndex(List<TooltipLine> tooltips, string name, string mod, out int index, bool silenceWarning = false)
		{
			TooltipLine tooltipLine = tooltips.FirstOrDefault(x => x.Name == name && x.Mod == mod);
			if (tooltipLine != null)
			{
				index = tooltips.IndexOf(tooltipLine);
				return true;
			}
			index = 0;
			if (!silenceWarning)
			{
				ModContent.GetInstance<RijamsMod>().Logger.WarnFormat("Tooltip line {0} from mod {1} not found!", name, mod);
			}
			return false;
		}

		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (item.type == ItemID.KingSlimeBossBag)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Accessories.Misc.MorphasRing>(), 7));
			}
			if (item.type == ItemID.EyeOfCthulhuBossBag)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodyArrow>(), 1, 20, 50));
			}
			if (item.type == ItemID.WoodenCrate || item.type == ItemID.WoodenCrateHard)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Belt>(), 10));
			}
			if (item.type == ItemID.LockBox)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Summon.Cudgels.CobaltProtectorCudgel>(), 7));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Magic.StumbleCard>(), 10));
			}
			if (item.type == ItemID.ObsidianLockbox)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Magic.StumbleCard>(), 7));
			}
			if (item.type == ItemID.QueenSlimeBossBag)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Summon.Cudgels.CrystalClusterCudgel>(), 4));
			}
			if (item.type == ItemID.DeerclopsBossBag)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Pets.StarCallerStaff>(), 4));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Summon.Cudgels.SanityFlowerCudgel> (), 4));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Summon.Whips.TailoThreeCats> (), 4));
			}
			if (item.type == ItemID.FairyQueenBossBag)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Summon.Cudgels.RadiantLanternCudgel>(), 4));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Melee.JoustingLances.EtherealJoustingLance>(), 4));
			}
		}
	}
}
