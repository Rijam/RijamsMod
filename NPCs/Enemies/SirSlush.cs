using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace RijamsMod.NPCs.Enemies
{
    public class SirSlush : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sir Slush");
			Main.npcFrameCount[npc.type] = 12;
		}

		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 52;
			npc.damage = 1;
			npc.defense = 0;
			npc.lifeMax = 600;
			npc.buffImmune[BuffID.Confused] = false;
			npc.HitSound = SoundID.NPCHit11; //16 for hat
			npc.DeathSound = SoundID.NPCDeath15;
			npc.value = 10000f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 0; //0 will face the player
			//npc.dontTakeDamage = true;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Placeable.SirSlushBanner>();
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(25) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessories.FrostyRose>());
			}
			if (Main.rand.Next(50) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Present);
			}
			if (Main.rand.Next(50) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.HandWarmer);
			}
			if (Main.rand.Next(3) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Armor.Vanity.SirSlushsTopHat>());
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SlushBlock, Main.rand.Next(10, 20));

			//From Spirit Mod FrostSaucer.cs
			if (Main.invasionType == 2)
			{
				Main.invasionSize -= 1;
				if (Main.invasionSize < 0)
				{
					Main.invasionSize = 0;
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, 1, 0);
				}
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.InvasionProgressReport, -1, -1, null, Main.invasionProgress, (float)Main.invasionProgressMax, (float)Main.invasionProgressIcon, 0f, 0, 0, 0);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (SpawnCondition.FrostLegion.Active)
			{
				return SpawnCondition.FrostLegion.Chance * 0.1f;
			}
			else
			{
				return 0;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SirSlush_Gore_Hat"), 1f);
				for (int k = 0; k < 5; k++)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SirSlush_Gore"), 1f);
				}
			}
		}

		public int AIState = 0;
		//0 == idle
		//1 == alert
		//2 == attack

		
        public override void AI()
        {
			npc.ai[0]++;
			//Main.NewText(npc.ai[0]);
			//Main.NewText(AIState);
			if (AIState == 0) //idle
			{
				npc.TargetClosest();
				npc.FaceTarget();
				bool lineOfSight = Collision.CanHitLine(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height);
				float distance = Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y);
				if (npc.ai[0] >= 30 && npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient && distance <= 1000f && lineOfSight)
                {
					npc.ai[0] = 0;
					Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/SirSlushAlert"));
					AIState = 1;
				}
			}
			else if (AIState == 1) //alert
			{
				npc.FaceTarget();
				float distance = Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y);
				if (npc.ai[0] == 80 && npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient && distance <= 1000f)
                {
					npc.ai[0] = 0;
					npc.frameCounter = 0;
					AIState = 2;
                }
				else if (npc.ai[0] > 80 || distance > 1000f)
                {
					npc.ai[0] = 0;
					AIState = 0;
				}
			}
			else if (AIState == 2) //attack
			{
				npc.FaceTarget();
				if (npc.ai[0] == 20)
                {
					Vector2 vectoryForProj = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
					float projSpeedX = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vectoryForProj.X;
					float projSpeedXAbs; //= Math.Abs(projSpeedX) * 0.1f;
					projSpeedXAbs = Math.Abs(projSpeedX) * Main.rand.Next(10, 20) * 0.01f;
					float projSpeedY = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vectoryForProj.Y - projSpeedXAbs;
					float sqrtXto2PlusYto2 = (float)Math.Sqrt(projSpeedX * projSpeedX + projSpeedY * projSpeedY);
					npc.netUpdate = true;
					sqrtXto2PlusYto2 = 10f / sqrtXto2PlusYto2;
					projSpeedX *= sqrtXto2PlusYto2;
					projSpeedY *= sqrtXto2PlusYto2;
					int projDamage = 30;
					int projType = ModContent.ProjectileType<Projectiles.SirSlushSnowball>();
					vectoryForProj.X += projSpeedX;
					vectoryForProj.Y += projSpeedY;
					if (!Main.dedServ)
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/SirSlushThrow"));
						Projectile.NewProjectile(vectoryForProj.X, vectoryForProj.Y, projSpeedX, projSpeedY, projType, projDamage, 4f, Main.myPlayer);
					}
				}
				if (npc.ai[0] == 40)
                {
					npc.ai[0] = 0;
					AIState = 0;
				}
			}
			else
			{
				AIState = 0;
			}
		}


		//Animations
		//0-3 idle
		//4-7 alert
		//8-11 attack
		private const int Frame_Idle1 = 0;
		private const int Frame_Idle2 = 1;
		private const int Frame_Idle3 = 2;
		private const int Frame_Idle4 = 3;
		private const int Frame_Alert1 = 4;
		private const int Frame_Alert2 = 5;
		private const int Frame_Alert3 = 6;
		private const int Frame_Alert4 = 7;
		private const int Frame_Attack1 = 8;
		private const int Frame_Attack2 = 9;
		private const int Frame_Attack3 = 10;
		private const int Frame_Attack4 = 11;

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (AIState == 0) //idle
			{
				if (npc.frameCounter < 10)
                {
					npc.frame.Y = Frame_Idle1 * frameHeight;
				}
				else if (npc.frameCounter < 20)
				{
					npc.frame.Y = Frame_Idle2 * frameHeight;
				}
				else if (npc.frameCounter < 30)
				{
					npc.frame.Y = Frame_Idle3 * frameHeight;
				}
				else if (npc.frameCounter < 40)
				{
					npc.frame.Y = Frame_Idle4 * frameHeight;
				}
				else
                {
					npc.frameCounter = 0;
				}
			}
			else if (AIState == 1) //alert
			{
				if (npc.frameCounter < 10)
				{
					npc.frame.Y = Frame_Alert1 * frameHeight;
				}
				else if (npc.frameCounter < 20)
				{
					npc.frame.Y = Frame_Alert2 * frameHeight;
				}
				else if (npc.frameCounter < 30)
				{
					npc.frame.Y = Frame_Alert3 * frameHeight;
				}
				else if (npc.frameCounter <= 40)
				{
					npc.frame.Y = Frame_Alert4 * frameHeight;
				}
				else
				{
					npc.frameCounter = 0;
				}
			}
			else if (AIState == 2) //attack
			{
				if (npc.frameCounter < 10)
				{
					npc.frame.Y = Frame_Attack1 * frameHeight;
				}
				else if (npc.frameCounter < 20)
				{
					npc.frame.Y = Frame_Attack2 * frameHeight;
				}
				else if (npc.frameCounter < 30)
				{
					npc.frame.Y = Frame_Attack3 * frameHeight;
				}
				else if (npc.frameCounter < 40)
				{
					npc.frame.Y = Frame_Attack4 * frameHeight;
				}
				else
				{
					npc.frameCounter = 0;
				}
			}
		}
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return true;
		}
	}
	public class SirSlushHat : ModNPC
    {
        public override bool Autoload(ref string name)
        {
            return false;
        }
        public override string Texture => "Terraria/NPC_0";

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.width = 20;
			npc.height = 30;
		}
        public override void AI()
        {
            
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}
	}
}