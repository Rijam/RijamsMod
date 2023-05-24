using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using RijamsMod.Items.Armor;
using RijamsMod.Items.Weapons.Ranged.Ammo;
using RijamsMod.Items.Weapons.Summon.Whips;
using RijamsMod.Buffs.Minions;
using System.Linq;
using Terraria.GameContent.Creative;
using Terraria.Utilities;
using Terraria.GameContent.ItemDropRules;
using RijamsMod.Items.Accessories.Misc;
using RijamsMod.Items.Materials;
using RijamsMod.Items.Weapons.Ranged;
using RijamsMod.Buffs.Potions;
using static RijamsMod.RijamsModConfigServer;

namespace RijamsMod.Items
{
	public class GlobalItems : GlobalItem
	{
		/// <summary> This set is a set of all whips. </summary>
		public static List<int> isWhip = new() { ItemID.BlandWhip, ItemID.ThornWhip, ItemID.BoneWhip, ItemID.FireWhip,
			ItemID.CoolWhip, ItemID.SwordWhip, ItemID.MaceWhip, ItemID.ScytheWhip, ItemID.RainbowWhip };
		/// <summary> This set is a set of all jousting lances. </summary>
		public static List<int> isJoustingLance = new() { ItemID.JoustingLance, ItemID.HallowJoustingLance, ItemID.ShadowJoustingLance };
		/// <summary>
		/// This set is a set of all lantern weapons (like the Nightglow, but does not include the Nightglow).
		/// Items in this set will automatically be drawn behind the player's back hand.
		/// </summary>
		public static List<int> isLanternWeapon = new(); // Nightglow not included.
		/// <summary>
		/// The front arm of the player will not animate correctly when the useStyle is set to RaiseLamp (14). Items in this set will be corrected with an IL Edit.
		/// </summary>
		public static List<int> fixItemUseStyleIDRaiseLampFrontArmAnimation = new();

		public override void SetDefaults(Item item)
		{
			RijamsModConfigServer.ArmorOptions vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;

			if (item.type == ItemID.WormTooth)
			{
				ItemID.Sets.ShimmerTransformToItem[item.type] = ModContent.ItemType<CrawlerChelicera>(); // Shimmer transforms the item.
			}
			if (item.type == ItemID.Handgun)
			{
				ItemID.Sets.ShimmerTransformToItem[item.type] = ModContent.ItemType<StockadeCrossbow>(); // Shimmer transforms the item.
			}

			if (item.type == ItemID.Coal)
			{
				item.maxStack = Item.CommonMaxStack;
			}
			if (item.type == ItemID.PharaohsMask && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				item.vanity = false;
				item.defense = 2;
				item.value = 5000;
			}
			if (item.type == ItemID.PharaohsRobe && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				item.vanity = false;
				item.defense = 3;
				item.value = 5000;
			}
			if (item.type == ItemID.AncientArmorHat && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				item.vanity = false;
				item.defense = 10;
			}
			if (item.type == ItemID.AncientArmorShirt && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				item.vanity = false;
				item.defense = 14;
			}
			if (item.type == ItemID.AncientArmorPants && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				item.vanity = false;
				item.defense = 9;
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
		public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
		{
			RijamsModPlayer moddedplayer = player.GetModPlayer<RijamsModPlayer>();
			if (moddedplayer.flaskBuff >= 1 && item.CountsAsClass(DamageClass.Melee) && !item.noMelee && !item.noUseGraphic && Main.rand.NextBool(2))
			{
				int dustType = DustID.Dirt;
				if (moddedplayer.flaskBuff == FlaskIDs.SulfuricAcid)
				{
					dustType = ModContent.DustType<Dusts.SulfurDust>();
				}
				if (moddedplayer.flaskBuff == FlaskIDs.Oiled)
				{
					dustType = DustID.Asphalt;
				}
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustType, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default, 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.7f;
				Main.dust[dust].velocity.Y -= 0.5f;
				Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.Yellow.ToVector3() * 0.1f);
			}
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			ArmorOptions vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;
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
					tooltips.Add(new TooltipLine(Mod, "SStats7", "Does not provide increase life regeneration in Expert Mode"));
					tooltips.Add(new TooltipLine(Mod, "SOverride", "Counts as a Well Fed buff"));
				}
				else
				{
					tooltips.Add(new TooltipLine(Mod, "SInfo", "Hold Left Shift for more information"));
				}
			}
			if (item.type == ItemID.AncientArmorHat && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				if (FindTooltipIndex(tooltips, "Defense", "Terraria", out int index))
				{
					tooltips.Insert(index + 1, new TooltipLine(Mod, "AncientHeaddress", "+5 critical strike chance"));
					tooltips.Insert(index + 2, new TooltipLine(Mod, "AncientHeaddress", "20% chance to not consume ammo"));
				}
			}
			if (item.type == ItemID.AncientArmorShirt && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				if (FindTooltipIndex(tooltips, "Defense", "Terraria", out int index))
				{
					tooltips.Insert(index + 1, new TooltipLine(Mod, "AncientGarments", "20% increased damage"));
					tooltips.Insert(index + 2, new TooltipLine(Mod, "AncientGarments", "+1 Minion capacity"));
					tooltips.Insert(index + 3, new TooltipLine(Mod, "AncientGarments", "+5 Support minion radius"));
				}
			}
			if (item.type == ItemID.AncientArmorPants && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				if (FindTooltipIndex(tooltips, "Defense", "Terraria", out int index))
				{
					tooltips.Insert(index + 1, new TooltipLine(Mod, "AncientSlacks", "10% increased melee speed"));
					tooltips.Insert(index + 2, new TooltipLine(Mod, "AncientSlacks", "10% reduced mana usage"));
					tooltips.Insert(index + 3, new TooltipLine(Mod, "AncientSlacks", "+2 life regeneration"));
				}
			}
			if (item.type == ItemID.ShadowJoustingLance)
			{
				TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.Mod == "Terraria");
				if (line != null)
				{
					line.Text = Language.GetTextValue("ItemTooltip.ShadowJoustingLance") + "\nInflicts Shadowflame";
				}
			}
			RijamsModConfigClient configClient = ModContent.GetInstance<RijamsModConfigClient>();
			if (isWhip.Contains(item.type) && (isLeftShiftHeld && configClient.DisplayWhipMultihitPenalty == RijamsModConfigClient.WhipMultihitPenalty.HoldShift || configClient.DisplayWhipMultihitPenalty == RijamsModConfigClient.WhipMultihitPenalty.On))
			{
				if (item.type == ModContent.ItemType<Belt>())
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "ModWhipDamageReduction", "50% damage penalty per enemy pierced"));
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
				if (item.type == ItemID.BoneWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "10% damage penalty per enemy pierced"));
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
				if (item.type == ItemID.RainbowWhip)
				{
					if (FindTooltipIndex(tooltips, "Knockback", "Terraria", out int index))
					{
						tooltips.Insert(index + 1, new TooltipLine(Mod, "VanillaWhipDamageReduction", "10% damage penalty per enemy pierced"));
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

		public static bool FindTooltipIndex(List<TooltipLine> tooltips, string name, string mod, out int index)
		{
			TooltipLine tooltipLine = tooltips.FirstOrDefault(x => x.Name == name && x.Mod == mod);
			if (tooltipLine != null)
			{
				index = tooltips.IndexOf(tooltipLine);
				return true;
			}
			index = 0;
			ModLoader.GetMod("RijamsMod").Logger.WarnFormat("Tooltip line {0} from mod {1} not found!", name, mod);
			return false;
		}

		public override string IsArmorSet(Item head, Item body, Item legs)
		{
			ArmorOptions vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;

			if (head.type == ItemID.CrimsonHelmet && (body.type == ItemID.CrimsonScalemail || body.type == ModContent.ItemType<Armor.DilapidatedCrimson.DilapidatedCrimsonScalemail>()) && (legs.type == ItemID.CrimsonGreaves || legs.type == ModContent.ItemType<Armor.DilapidatedCrimson.DilapidatedCrimsonGreaves>()))
			{
				return Language.GetTextValue("ArmorSetBonus.Crimson");
			}
			if (head.type == ItemID.PharaohsMask && body.type == ItemID.PharaohsRobe && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				return "Pharaoh";
			}
			if (head.type == ItemID.AncientArmorHat && body.type == ItemID.AncientArmorShirt && legs.type == ItemID.AncientArmorPants && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				return "Ancient";
			}
			if (head.type == ItemID.StardustHelmet && body.type == ItemID.StardustBreastplate && legs.type == ItemID.StardustLeggings && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.StardustOnly))
			{
				return "Stardust";
			}
			return "";
		}
		public override void UpdateEquip(Item item, Player player)
		{
			ArmorOptions vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;

			if (item.type == ItemID.AncientArmorHat && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				player.GetCritChance(DamageClass.Melee) += 5;
				player.GetCritChance(DamageClass.Ranged) += 5;
				player.GetCritChance(DamageClass.Magic) += 5;
				player.GetCritChance(DamageClass.Throwing) += 5;
				player.ammoCost80 = true;
			}
			if (item.type == ItemID.AncientArmorShirt && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				player.maxMinions += 1;
				player.GetDamage(DamageClass.Generic) *= 1.2f;
				player.GetModPlayer<RijamsModPlayer>().supportMinionRadiusIncrease += 5;
			}
			if (item.type == ItemID.AncientArmorPants && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				player.GetAttackSpeed(DamageClass.Melee) *= 1.1f;
				player.manaCost *= 0.9f;
				player.lifeRegen += 2;
			}
		}
		public override void UpdateArmorSet(Player player, string set)
		{
			ArmorOptions vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;

			if (set == "Pharaoh" && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				player.setBonus = "\n10% reduced mana usage\n+1 Minion capacity\n+10% Whip range\n5% increased movement speed\nGrants Immunity to Mighty Wind";
				player.manaCost -= 0.1f;
				player.maxMinions++;
				player.whipRangeMultiplier += 0.1f;
				player.moveSpeed += 0.05f;
				player.buffImmune[BuffID.WindPushed] = true;
			}
			if (set == "Ancient" && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				player.GetModPlayer<RijamsModPlayer>().ancientSet = true;
				player.setBonus = "\nIncreased maximum running speed\nIncreased running acceleration\n+0.5 seconds flight time\n+15% Whip speed\n+20% Whip range\nAllows Shield of Cthulhu style dashing";
				player.dashType = 2; //Shield of Cthulhu dash
				//Set in PostUpdateRunSpeeds() in RijamsModPlayer.cs
				//player.runAcceleration += 0.1f;
				//player.maxRunSpeed += 2;
				player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
				player.whipRangeMultiplier += 0.2f;
				player.wingTimeMax += 30;
			}
			if ((player.setStardust || set == "Stardust") && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.StardustOnly))
			{
				player.setBonus = Language.GetTextValue("ArmorSetBonus.Stardust") + "\n+15% Whip speed";
				player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
			}
		}
		public override void ArmorSetShadows(Player player, string set)
		{
			ArmorOptions vanillaVanityToArmor = ModContent.GetInstance<RijamsModConfigServer>().VanillaVanityToArmor;

			if (set == "Ancient" && (vanillaVanityToArmor == ArmorOptions.All || vanillaVanityToArmor == ArmorOptions.VanityOnly))
			{
				player.armorEffectDrawShadow = true;
			}
		}
		public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
		{
			if (weapon.type == ItemID.SnowmanCannon)
			{
				if (ammo.type == ModContent.ItemType<EndlessRocketBox>())
				{
					type = ProjectileID.RocketSnowmanI;
				}
			}
			if (weapon.type == ItemID.Celeb2)
			{
				if (ammo.type == ModContent.ItemType<EndlessRocketBox>())
				{
					type = ProjectileID.Celeb2Rocket;
				}
			}
		}
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
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
			}
			if (item.type == ItemID.QueenSlimeBossBag)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Summon.Cudgels.CrystalClusterCudgel>(), 4));
			}
			if (item.type == ItemID.DeerclopsBossBag)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Pets.StarCallerStaff>(), 4));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Summon.Cudgels.SanityFlowerCudgel> (), 4));
			}
			if (item.type == ItemID.FairyQueenBossBag)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Summon.Cudgels.RadiantLanternCudgel>(), 4));
			}
		}

		// The player can't use the support minion weapons if they already have one active.
		private static readonly List<int> SupportMinionsDefenseBuffs = new() { ModContent.BuffType<HarpyIdolBuff>(), 
			ModContent.BuffType<CobaltProtectorBuff>(), ModContent.BuffType<CrystalClusterBuff>(), ModContent.BuffType<FallenPaladinBuff>(),
			ModContent.BuffType<StardustProtectorBuff>() };
		private static readonly List<int> SupportMinionsHealingBuffs = new() { ModContent.BuffType<SanityFlowerBuff>(),
			ModContent.BuffType<RadiantLanternBuff>() };

		public override bool CanUseItem(Item item, Player player)
		{
			if (item.ModItem is CudgelDefenseItem)
			{
				for (int i = 0; i < SupportMinionsDefenseBuffs.Count; i++)
				{
					if (player.HasBuff(SupportMinionsDefenseBuffs[i]))
					{
						return false;
					}
				}
			}
			if (item.ModItem is CudgelHealingItem)
			{
				for (int i = 0; i < SupportMinionsHealingBuffs.Count; i++)
				{
					if (player.HasBuff(SupportMinionsHealingBuffs[i]))
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	// This is used for the Timon's Axe, Hammer of Retribution, and Quietus for checking if the glow mask should be drawn in ItemUseGlow
	public class MagicMeleeGlow : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(MagicMeleeGlow);
		public override string Texture => Item.type == ModContent.ItemType<MagicMeleeGlow>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');
	}
	// This is used for the Cudgel support minion items.
	public class CudgelDefenseItem : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(CudgelDefenseItem);
		public override string Texture => Item.type == ModContent.ItemType<CudgelDefenseItem>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GlobalItems.FindTooltipIndex(tooltips, "Tooltip0", "Terraria", out int index))
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "SupportMinionMessage1", "[c/00af00:- Defense Support Minion -]"));
				tooltips.Insert(index + 2, new TooltipLine(Mod, "SupportMinionMessage2", "[c/00af00:Maximum of one defense support minion per player]"));
			}
		}
	}
	public class CudgelHealingItem : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(CudgelHealingItem);
		public override string Texture => Item.type == ModContent.ItemType<CudgelHealingItem>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GlobalItems.FindTooltipIndex(tooltips, "Tooltip0", "Terraria", out int index))
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "SupportMinionMessage1", "[c/00af00:- Healing Support Minion -]"));
				tooltips.Insert(index + 2, new TooltipLine(Mod, "SupportMinionMessage2", "[c/00af00:Maximum of one healing support minion per player]"));
			}
		}
	}
	public class CudgelBuffItem : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(CudgelBuffItem);
		public override string Texture => Item.type == ModContent.ItemType<CudgelBuffItem>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GlobalItems.FindTooltipIndex(tooltips, "Tooltip0", "Terraria", out int index))
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "SupportMinionMessage1", "[c/00af00:- Buff Support Minion -]"));
				tooltips.Insert(index + 2, new TooltipLine(Mod, "SupportMinionMessage2", "[c/00af00:Maximum of one buff support minion per player]"));
			}
		}
	}
}