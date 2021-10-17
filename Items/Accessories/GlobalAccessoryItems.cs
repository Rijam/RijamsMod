using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Items.Accessories
{
	// This file shows a very simple example of a GlobalItem class. GlobalItem hooks are called on all items in the game and are suitable for sweeping changes like 
	// adding additional data to all items in the game. Here we simply adjust the damage of the Copper Shortsword item, as it is simple to understand. 
	// See other GlobalItem classes in ExampleMod to see other ways that GlobalItem can be used.
	public class GlobalAccessoryItems : GlobalItem
	{
		public override void SetDefaults(Item item)
		{
			if (item.type == ItemID.CelestialCuffs)
			{ // Here we make sure to only change Copper Shortsword by checking item.type in an if statement
				//item.damage = 50;       // Changed original CopperShortsword's damage to 50!
			}
		}
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
			if (item.type == ItemID.CelestialCuffs)
            {
				player.statManaMax2 += 20;
			}
		}
		public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
		{
			if (player.GetModPlayer<RijamsModPlayer>().daybreakStone && item.melee && !item.noMelee && !item.noUseGraphic && Main.rand.Next(2) == 0)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SolarFlare, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default, 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.7f;
				Main.dust[dust].velocity.Y -= 0.5f;
				Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), Color.Yellow.ToVector3() * 0.875f);
			}
		}
	}
	public class SummonersGloveUpdate : GlobalItem
	{
		public override bool CanUseItem(Item item, Player player)
        {
			//Works, but makes all summon items and sentries still auto swing after the gloves have been unequipped.
			if (player.GetModPlayer<RijamsModPlayer>().summonersGlove)
			{
				if (item.summon || item.sentry)
				{
					item.autoReuse = true;
				}
            }
			//Works, but makes all summon items and sentries still auto swing after the gloves have been unequipped.
			/*bool flag = false;
			if (player.GetModPlayer<RijamsModPlayer>().summonersGlove)
			{
				flag |= item.summon;
				flag |= item.sentry;
			}
			if (flag)
			{
				item.autoReuse = true;
			}*/
			return base.CanUseItem(item, player);//taken from Omniswing
		}
	}
	public class GuideToProperFlightTechniquesUpdate : GlobalItem
	{
		public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
			if (player.GetModPlayer<RijamsModPlayer>().guideToProperFlightTechniques)
            {
				if (player.wingTimeMax > 0)
				{
					speed += 1f;
					acceleration *= 2f;
				}
			}
		}
		public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			if (player.GetModPlayer<RijamsModPlayer>().guideToProperFlightTechniques)
			{
				if (player.wingTimeMax > 0)
				{
					maxCanAscendMultiplier *= 1.1f;
					maxAscentMultiplier *= 1.1f;
				}
			}
		}
	}
}