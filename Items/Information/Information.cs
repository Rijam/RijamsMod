using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Information
{
	public class LifeDisplay : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Life Display");
			Tooltip.SetDefault("Displays your current life bonuses");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(60, 2));
		}

		public override void SetDefaults()
		{
			//item.color = Color.Red; //colors the inventory sprite
			item.width = 32;
			item.height = 28;
			item.maxStack = 1;
			item.rare = ItemRarityID.Blue;
			item.value = 100;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int statLifeMax = Main.player[Main.myPlayer].statLifeMax;
			string statLifeMaxString = statLifeMax.ToString();
			int statLifeMax2 = Main.player[Main.myPlayer].statLifeMax2;
			string statLifeMax2String = statLifeMax2.ToString();
			int lifeRegen = Main.player[Main.myPlayer].lifeRegen;
			string lifeRegenString = lifeRegen.ToString();
			int lifeRegenTime = Main.player[Main.myPlayer].lifeRegenTime;
			string lifeRegenTimeString = lifeRegenTime.ToString();

			tooltips.Add(new TooltipLine(mod, "MaxLife", "Maximum life: " + statLifeMaxString));
			tooltips.Add(new TooltipLine(mod, "MaxTempLife", "Maximum temporary life: " + statLifeMax2String));
			tooltips.Add(new TooltipLine(mod, "lifeRegen", "Life regeneration: " + lifeRegenString));
			tooltips.Add(new TooltipLine(mod, "lifeRegenTime", "Life regeneration time: " + lifeRegenTimeString));
		}
	}
	public class ManaDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Display");
			Tooltip.SetDefault("Displays your current mana bonuses");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(60, 2));
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int statManaMax = Main.player[Main.myPlayer].statManaMax;
			string statManaMaxString = statManaMax.ToString();
			int statManaMax2 = Main.player[Main.myPlayer].statManaMax2;
			string statManaMax2String = statManaMax2.ToString();
			float manaCost = Main.player[Main.myPlayer].manaCost;
			string manaCostString = manaCost.ToString();
			int manaRegen = Main.player[Main.myPlayer].manaRegen;
			string manaRegenString = manaRegen.ToString();
			int manaRegenBonus = Main.player[Main.myPlayer].manaRegenBonus;
			string manaRegenBonusString = manaRegenBonus.ToString();

			tooltips.Add(new TooltipLine(mod, "statManaMax", "Maximum mana: " + statManaMaxString));
			tooltips.Add(new TooltipLine(mod, "statManaMax2", "Maximum temporary mana: " + statManaMax2String));
			tooltips.Add(new TooltipLine(mod, "manaCost", "Mana cost multiplier: " + manaCostString));
			tooltips.Add(new TooltipLine(mod, "manaRegen", "Mana regeneration: " + manaRegenString));
			tooltips.Add(new TooltipLine(mod, "manaRegenBonus", "Mana regeneration bonus: " + manaRegenBonusString));
		}
	}
	public class DefenseDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Defense Display");
			Tooltip.SetDefault("Displays your current defense bonuses");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(60, 2));
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			float statDefense = Main.player[Main.myPlayer].statDefense;
			string statDefenseString = statDefense.ToString();
			float endurance = Main.player[Main.myPlayer].endurance;
			string enduranceString = endurance.ToString();

			tooltips.Add(new TooltipLine(mod, "StatDefense", "Defense: " + statDefenseString));
			tooltips.Add(new TooltipLine(mod, "Endurance", "Damage Reduction: " + enduranceString + "%"));
		}
	}
	public class MovementDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Movement Display");
			Tooltip.SetDefault("Displays your current movement bonuses");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(60, 2));
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			float moveSpeed = Main.player[Main.myPlayer].moveSpeed;
			string moveSpeedString = moveSpeed.ToString();
			float maxRunSpeed = Main.player[Main.myPlayer].maxRunSpeed;
			string maxRunSpeedString = maxRunSpeed.ToString();
			float runAcceleration = Main.player[Main.myPlayer].runAcceleration;
			string runAccelerationString = runAcceleration.ToString();
			float runSlowdown = Main.player[Main.myPlayer].runSlowdown;
			string runSlowdownString = runSlowdown.ToString();
			float wingTimeMax = Main.player[Main.myPlayer].wingTimeMax;
			string wingTimeMaxString = wingTimeMax.ToString();
			bool noKnockback = Main.player[Main.myPlayer].noKnockback;
			string noKnockbackString = noKnockback.ToString();
			bool noFallDmg = Main.player[Main.myPlayer].noFallDmg;
			int wings = Main.player[Main.myPlayer].wings;
			string realFallDmgString = "False";
			if (noFallDmg || wings > 0)
            {
				realFallDmgString = "True";
			}

			tooltips.Add(new TooltipLine(mod, "moveSpeed", "Movement speed multiplier: " + moveSpeedString));
			tooltips.Add(new TooltipLine(mod, "moveSpeed", "Maxium Running speed: " + maxRunSpeedString));
			tooltips.Add(new TooltipLine(mod, "runAcceleration", "Running acceleration speed: " + runAccelerationString));
			tooltips.Add(new TooltipLine(mod, "runSlowdown", "Running deceleration speed: " + runSlowdownString));
			tooltips.Add(new TooltipLine(mod, "wingTimeMax", "Wing flight time: " + wingTimeMaxString));
			tooltips.Add(new TooltipLine(mod, "noKnockback", "Knockback immunity: " + noKnockbackString));
			tooltips.Add(new TooltipLine(mod, "noFallDmg", "Fall damage immunity: " + realFallDmgString));
		}
	}
	public class DamageDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Damage Display");
			Tooltip.SetDefault("Displays your current damage bonuses\nValues greater than 1 means increased damage");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(60, 2));
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			float meleeDamage = Main.player[Main.myPlayer].meleeDamage;
			string meleeDamageString = meleeDamage.ToString();
			float rangedDamage = Main.player[Main.myPlayer].rangedDamage;
			string rangedDamageString = rangedDamage.ToString();
			float magicDamage = Main.player[Main.myPlayer].magicDamage;
			string magicDamageString = magicDamage.ToString();
			float minionDamage = Main.player[Main.myPlayer].minionDamage;
			string minionDamageString = minionDamage.ToString();
			float thrownDamage = Main.player[Main.myPlayer].thrownDamage;
			string thrownDamageString = thrownDamage.ToString();
			float allDamage = Main.player[Main.myPlayer].allDamage;
			string allDamageString = allDamage.ToString();

			tooltips.Add(new TooltipLine(mod, "Melee", "Melee damage multipler: " + meleeDamageString));
			tooltips.Add(new TooltipLine(mod, "Ranged", "Ranged damage multipler: " + rangedDamageString));
			tooltips.Add(new TooltipLine(mod, "Magic", "Magic damage multipler: " + magicDamageString));
			tooltips.Add(new TooltipLine(mod, "Summon", "Summon damage multipler: " + minionDamageString));
			tooltips.Add(new TooltipLine(mod, "Throwing", "Throwing damage multipler: " + thrownDamageString));
			tooltips.Add(new TooltipLine(mod, "All", "All damage multipler: " + allDamageString));
		}
	}
	public class CritDisplay : LifeDisplay
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Critical Hit Display");
			Tooltip.SetDefault("Displays your current critical hit bonuses");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(60, 2));
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int meleeCrit = Main.player[Main.myPlayer].meleeCrit;
			string meleeCritString = meleeCrit.ToString();
			int rangedCrit = Main.player[Main.myPlayer].rangedCrit;
			string rangedCritString = rangedCrit.ToString();
			int magicCrit = Main.player[Main.myPlayer].magicCrit;
			string magicCritString = magicCrit.ToString();
			int thrownCrit = Main.player[Main.myPlayer].thrownCrit;
			string thrownCritString = thrownCrit.ToString();

			tooltips.Add(new TooltipLine(mod, "Melee", "Melee critical hit: " + meleeCritString));
			tooltips.Add(new TooltipLine(mod, "Ranged", "Ranged critical hit: " + rangedCritString));
			tooltips.Add(new TooltipLine(mod, "Magic", "Magic critical hit: " + magicCritString));
			tooltips.Add(new TooltipLine(mod, "Throwing", "Throwing critical hit: " + thrownCritString));
		}
	}
	public class SummonsDisplay : LifeDisplay
	{
		//public override string Texture => "Terraria/Item_" + ItemID.REK;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Summons Display");
			Tooltip.SetDefault("Displays your current summons capacity and bonus");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(60, 2));
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int summonTotal = Main.player[Main.myPlayer].maxMinions;
			string summonTotalString = summonTotal.ToString();
			int sentryTotal = Main.player[Main.myPlayer].maxTurrets;
			string sentryTotalString = sentryTotal.ToString();
			float minionKB = Main.player[Main.myPlayer].minionKB;
			string minionKBString = minionKB.ToString();
			int numMinions = Main.player[Main.myPlayer].numMinions;
			string numMinionsString = numMinions.ToString();

			tooltips.Add(new TooltipLine(mod, "MinionCount", "Maximum minions: " + summonTotalString));
			tooltips.Add(new TooltipLine(mod, "SentryCount", "Maximum sentries: " + sentryTotalString));
			tooltips.Add(new TooltipLine(mod, "SummonKB", "Minion knockback: " + minionKBString));
			tooltips.Add(new TooltipLine(mod, "SummonCountCurrent", "Current minion count: " + numMinionsString));
		}
	}
	public class InformationInterface : ModItem
	{
		//public static string ShowAll => Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) ? "Rustburn lowers defense by 25 and is effective against inorganic enemies\nInorganic enemies with Rustburn take even more damage from Acid Burn\nOrganic enemies only take a bit of damage over time" : "(Hold LEFT CONTROL for more info on Rust Burn)";
		//public override string Texture => "Terraria/Item_" + ItemID.CellPhone;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Information Interface");
			Tooltip.SetDefault("Displays your stats and bonuses\nHold Left Shift to see all player stats\nHold Left Control to see all damage stats");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(30, 12));
			/*if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/InformationInterface_Glow");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = 0;
				item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 0;
			}*/
		}

		public override void SetDefaults()
		{
			//item.color = Color.Gold; //colors the inventory sprite
			item.width = 48;
			item.height = 48;
			item.maxStack = 1;
			item.rare = ItemRarityID.Orange;
			item.value = 1000;
		}
		/*public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/GlowMasks/InformationInterface_Glow");
			spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}*/
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			bool isLeftCtrlHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl);
			bool isLeftShiftHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
			bool isRightShiftHeld = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift);

			//Life
			int statLifeMax = Main.player[Main.myPlayer].statLifeMax;
			string statLifeMaxString = statLifeMax.ToString();
			int statLifeMax2 = Main.player[Main.myPlayer].statLifeMax2;
			string statLifeMax2String = statLifeMax2.ToString();
			int lifeRegen = Main.player[Main.myPlayer].lifeRegen;
			string lifeRegenString = lifeRegen.ToString();
			int lifeRegenTime = Main.player[Main.myPlayer].lifeRegenTime;
			string lifeRegenTimeString = lifeRegenTime.ToString();

			//Mana
			int statManaMax = Main.player[Main.myPlayer].statManaMax;
			string statManaMaxString = statManaMax.ToString();
			int statManaMax2 = Main.player[Main.myPlayer].statManaMax2;
			string statManaMax2String = statManaMax2.ToString();
			float manaCost = Main.player[Main.myPlayer].manaCost;
			string manaCostString = manaCost.ToString();
			int manaRegen = Main.player[Main.myPlayer].manaRegen;
			string manaRegenString = manaRegen.ToString();
			int manaRegenBonus = Main.player[Main.myPlayer].manaRegenBonus;
			string manaRegenBonusString = manaRegenBonus.ToString();

			//Defense
			float statDefense = Main.player[Main.myPlayer].statDefense;
			string statDefenseString = statDefense.ToString();
			float endurance = Main.player[Main.myPlayer].endurance;
			string enduranceString = endurance.ToString();

			//Movement
			float moveSpeed = Main.player[Main.myPlayer].moveSpeed;
			string moveSpeedString = moveSpeed.ToString();
			float maxRunSpeed = Main.player[Main.myPlayer].maxRunSpeed;
			string maxRunSpeedString = maxRunSpeed.ToString();
			float runAcceleration = Main.player[Main.myPlayer].runAcceleration;
			string runAccelerationString = runAcceleration.ToString();
			float runSlowdown = Main.player[Main.myPlayer].runSlowdown;
			string runSlowdownString = runSlowdown.ToString();
			float wingTimeMax = Main.player[Main.myPlayer].wingTimeMax;
			string wingTimeMaxString = wingTimeMax.ToString();
			bool noKnockback = Main.player[Main.myPlayer].noKnockback;
			string noKnockbackString = noKnockback.ToString();
			bool noFallDmg = Main.player[Main.myPlayer].noFallDmg;
			int wings = Main.player[Main.myPlayer].wings;
			string realFallDmgString = "False";
			if (noFallDmg || wings > 0) //Having wings prevents fall damage but doesn't change the noFallDmg bool
			{
				realFallDmgString = "True";
			}

			//Damage
			float meleeDamage = Main.player[Main.myPlayer].meleeDamage;
			string meleeDamageString = meleeDamage.ToString();
			float rangedDamage = Main.player[Main.myPlayer].rangedDamage;
			string rangedDamageString = rangedDamage.ToString();
			float magicDamage = Main.player[Main.myPlayer].magicDamage;
			string magicDamageString = magicDamage.ToString();
			float minionDamage = Main.player[Main.myPlayer].minionDamage;
			string minionDamageString = minionDamage.ToString();
			float thrownDamage = Main.player[Main.myPlayer].thrownDamage;
			string thrownDamageString = thrownDamage.ToString();
			float allDamage = Main.player[Main.myPlayer].allDamage;
			string allDamageString = allDamage.ToString();

			//Crit
			int meleeCrit = Main.player[Main.myPlayer].meleeCrit;
			string meleeCritString = meleeCrit.ToString();
			int rangedCrit = Main.player[Main.myPlayer].rangedCrit;
			string rangedCritString = rangedCrit.ToString();
			int magicCrit = Main.player[Main.myPlayer].magicCrit;
			string magicCritString = magicCrit.ToString();
			int thrownCrit = Main.player[Main.myPlayer].thrownCrit;
			string thrownCritString = thrownCrit.ToString();

			//Summmons
			int summonTotal = Main.player[Main.myPlayer].maxMinions;
			string summonTotalString = summonTotal.ToString();
			int sentryTotal = Main.player[Main.myPlayer].maxTurrets;
			string sentryTotalString = sentryTotal.ToString();
			float minionKB = Main.player[Main.myPlayer].minionKB;
			string minionKBString = minionKB.ToString();
			int numMinions = Main.player[Main.myPlayer].numMinions;
			string numMinionsString = numMinions.ToString();

			//Other
			int taxMoney = Main.player[Main.myPlayer].taxMoney;
			string taxMoneyString = taxMoney.ToString();
			int taxTimer = Main.player[Main.myPlayer].taxTimer;
			string taxTimerString = taxTimer.ToString();
			//int taxRate = Player.taxRate;
			//string taxRateString = taxRate.ToString();
			int anglerQuestsFinished = Main.player[Main.myPlayer].anglerQuestsFinished;
			string anglerQuestsFinishedString = anglerQuestsFinished.ToString();
			int breath = Main.player[Main.myPlayer].breath;
			string breathString = breath.ToString();
			int breathCD = Main.player[Main.myPlayer].breathCD;
			string breathCDString = breathCD.ToString();
			int breathMax = Main.player[Main.myPlayer].breathMax;
			string breathMaxString = breathMax.ToString();
			bool lavaImmune = Main.player[Main.myPlayer].lavaImmune;
			string lavaImmuneString = lavaImmune.ToString();
			float pickSpeed = Main.player[Main.myPlayer].pickSpeed;
			string pickSpeedString = pickSpeed.ToString();
			bool ZoneWaterCandle = Main.player[Main.myPlayer].ZoneWaterCandle;
			string ZoneWaterCandleString = ZoneWaterCandle.ToString();
			bool ZonePeaceCandle = Main.player[Main.myPlayer].ZonePeaceCandle;
			string ZonePeaceCandleString = ZonePeaceCandle.ToString();

			if (isLeftShiftHeld)
            {
				//Life
				tooltips.Add(new TooltipLine(mod, "MaxLife", "Maximum life: " + statLifeMaxString));
				tooltips.Add(new TooltipLine(mod, "MaxTempLife", "Maximum temporary life: " + statLifeMax2String));
				tooltips.Add(new TooltipLine(mod, "lifeRegen", "Life regeneration: " + lifeRegenString));
				tooltips.Add(new TooltipLine(mod, "lifeRegenTime", "Life regeneration time: " + lifeRegenTimeString));

				//Mana
				tooltips.Add(new TooltipLine(mod, "statManaMax", "Maximum mana: " + statManaMaxString));
				tooltips.Add(new TooltipLine(mod, "statManaMax2", "Maximum temporary mana: " + statManaMax2String));
				tooltips.Add(new TooltipLine(mod, "manaCost", "Mana cost multiplier: " + manaCostString));
				tooltips.Add(new TooltipLine(mod, "manaRegen", "Mana regeneration: " + manaRegenString));
				tooltips.Add(new TooltipLine(mod, "manaRegenBonus", "Mana regeneration bonus: " + manaRegenBonusString));

				//Defense
				tooltips.Add(new TooltipLine(mod, "StatDefense", "Defense: " + statDefenseString));
				tooltips.Add(new TooltipLine(mod, "Endurance", "Damage Reduction: " + enduranceString + "%"));

				//Movement
				tooltips.Add(new TooltipLine(mod, "moveSpeed", "Movement speed multiplier: " + moveSpeedString));
				tooltips.Add(new TooltipLine(mod, "moveSpeed", "Maxium Running speed: " + maxRunSpeedString));
				tooltips.Add(new TooltipLine(mod, "runAcceleration", "Running acceleration speed: " + runAccelerationString));
				tooltips.Add(new TooltipLine(mod, "runSlowdown", "Running deceleration speed: " + runSlowdownString));
				tooltips.Add(new TooltipLine(mod, "wingTimeMax", "Wing flight time: " + wingTimeMaxString));
				tooltips.Add(new TooltipLine(mod, "noKnockback", "Knockback immunity: " + noKnockbackString));
				tooltips.Add(new TooltipLine(mod, "noFallDmg", "Fall damage immunity: " + realFallDmgString));
			}

			if (isLeftCtrlHeld)
			{
				//Damage
				tooltips.Add(new TooltipLine(mod, "Melee", "Melee damage multipler: " + meleeDamageString));
				tooltips.Add(new TooltipLine(mod, "Ranged", "Ranged damage multipler: " + rangedDamageString));
				tooltips.Add(new TooltipLine(mod, "Magic", "Magic damage multipler: " + magicDamageString));
				tooltips.Add(new TooltipLine(mod, "Summon", "Summon damage multipler: " + minionDamageString));
				tooltips.Add(new TooltipLine(mod, "Throwing", "Throwing damage multipler: " + thrownDamageString));
				tooltips.Add(new TooltipLine(mod, "All", "All damage multipler: " + allDamageString));

				//Crit
				tooltips.Add(new TooltipLine(mod, "Melee", "Melee critical hit: " + meleeCritString));
				tooltips.Add(new TooltipLine(mod, "Ranged", "Ranged critical hit: " + rangedCritString));
				tooltips.Add(new TooltipLine(mod, "Magic", "Magic critical hit: " + magicCritString));
				tooltips.Add(new TooltipLine(mod, "Throwing", "Throwing critical hit: " + thrownCritString));

				//Summons
				tooltips.Add(new TooltipLine(mod, "MinionCount", "Maximum minions: " + summonTotalString));
				tooltips.Add(new TooltipLine(mod, "SentryCount", "Maximum sentries: " + sentryTotalString));
				tooltips.Add(new TooltipLine(mod, "SummonKB", "Minion knockback: " + minionKBString));
				tooltips.Add(new TooltipLine(mod, "SummonCountCurrent", "Current minion count: " + numMinionsString));
			}
			if (isRightShiftHeld)
			{
				//Other
				tooltips.Add(new TooltipLine(mod, "taxMoney", "Tax money: " + taxMoneyString));
				tooltips.Add(new TooltipLine(mod, "taxTimer", "Tax timer: " + taxTimerString));
				//tooltips.Add(new TooltipLine(mod, "taxRate", "Tax rate: " + taxRateString));
				tooltips.Add(new TooltipLine(mod, "anglerQuestsFinished", "Angler quests finished: " + anglerQuestsFinishedString));
				tooltips.Add(new TooltipLine(mod, "breath", "Current breath: " + breathString));
				tooltips.Add(new TooltipLine(mod, "breathCD", "Drowning damage: " + breathCDString));
				tooltips.Add(new TooltipLine(mod, "breathMax", "Max breath: " + breathMaxString));
				tooltips.Add(new TooltipLine(mod, "lavaImmune", "Lava immunity: " + lavaImmuneString));
				tooltips.Add(new TooltipLine(mod, "pickSpeed", "Mining speed: " + pickSpeedString));
				tooltips.Add(new TooltipLine(mod, "ZoneWaterCandle", "Near Water Candle: " + ZoneWaterCandleString));
				tooltips.Add(new TooltipLine(mod, "ZonePeaceCandle", "Near Peace Candle: " + ZonePeaceCandleString));
			}
			if (!isLeftShiftHeld && !isLeftCtrlHeld && !isRightShiftHeld)
			{
				tooltips.Add(new TooltipLine(mod, "lifeRegen", "Life regeneration: " + lifeRegenString));
				tooltips.Add(new TooltipLine(mod, "manaRegen", "Mana regeneration: " + manaRegenString));
				tooltips.Add(new TooltipLine(mod, "Endurance", "Damage Reduction: " + enduranceString + "%"));
				tooltips.Add(new TooltipLine(mod, "moveSpeed", "Movement speed multipler: " + moveSpeedString));
				tooltips.Add(new TooltipLine(mod, "wingTimeMax", "Wing flight time: " + wingTimeMaxString));
				tooltips.Add(new TooltipLine(mod, "All", "All damage multipler: " + allDamageString));
				tooltips.Add(new TooltipLine(mod, "MinionCount", "Maximum minions: " + summonTotalString));
				tooltips.Add(new TooltipLine(mod, "SentryCount", "Maximum sentries: " + sentryTotalString));
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "LifeDisplay", 1);
			recipe.AddIngredient(mod, "ManaDisplay", 1);
			recipe.AddIngredient(mod, "DefenseDisplay", 1);
			recipe.AddIngredient(mod, "MovementDisplay", 1);
			recipe.AddIngredient(mod, "DamageDisplay", 1);
			recipe.AddIngredient(mod, "CritDisplay", 1);
			recipe.AddIngredient(mod, "SummonsDisplay", 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}