using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using RijamsMod.NPCs.TownNPCs;

namespace RijamsMod.Items
{
	//Code adapted from Fargo's Mutant Mod (CaughtNPCItem.cs)
	//and code adapted from Alchemist NPC (BrewerHorcrux.cs and similar)
	public class CaughtFisherman : ModItem
	{
		public override string Texture => "RijamsMod/NPCs/TownNPCs/Fisherman";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fisherman");
			Tooltip.SetDefault("'Greetings. Care to do some fishin'?'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 10;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noUseGraphic = true;
			item.consumable = true;
			item.UseSound = SoundID.Item44;
			item.makeNPC = (short)ModContent.NPCType<Fisherman>();
		}

		public override void HoldItem(Player player)
		{
			Player.tileRangeX += 600;
			Player.tileRangeY += 600;
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Fisherman>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			NPC.NewNPC((int)mousePos.X, (int)mousePos.Y, ModContent.NPCType<Fisherman>());
			Main.NewText(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Fisherman>())].GivenName + " the Fisherman has been spawned!", 50, 125, 255);
		}
	}
	public class CaughtHarpy : ModItem
	{
		public override string Texture => "RijamsMod/NPCs/TownNPCs/Harpy";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy");
			Tooltip.SetDefault("'Friends? I am friendly.'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 10;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noUseGraphic = true;
			item.consumable = true;
			item.UseSound = SoundID.Item44;
			item.makeNPC = (short)ModContent.NPCType<Harpy>();
		}
		public override void HoldItem(Player player)
		{
			Player.tileRangeX += 600;
			Player.tileRangeY += 600;
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Harpy>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}
		public override void OnConsumeItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			NPC.NewNPC((int)mousePos.X, (int)mousePos.Y, ModContent.NPCType<Harpy>());
			Main.NewText(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Harpy>())].GivenName + " the Harpy has been spawned!", 50, 125, 255);
		}
	}
	public class CaughtIntTrav : ModItem
	{
		public override string Texture => "RijamsMod/NPCs/TownNPCs/InterstellarTraveler";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Interstellar Traveler");
			Tooltip.SetDefault("'I'm pretty far from home, but this place is pretty cool.'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 26));
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 10;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noUseGraphic = true;
			item.consumable = true;
			item.UseSound = SoundID.Item44;
			item.makeNPC = (short)ModContent.NPCType<InterstellarTraveler>();
		}
		public override void HoldItem(Player player)
		{
			Player.tileRangeX += 600;
			Player.tileRangeY += 600;
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<InterstellarTraveler>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}
		public override void OnConsumeItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			NPC.NewNPC((int)mousePos.X, (int)mousePos.Y, ModContent.NPCType<InterstellarTraveler>());
			Main.NewText(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<InterstellarTraveler>())].GivenName + " the InterstellarTraveler has been spawned!", 50, 125, 255);
		}
	}
}