using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class SpikeTrap : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spike Trap");
		}
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 16;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.sentry = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.penetrate = -1;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 30;
			
			//projectile.usesLocalNPCImmunity = true;
			//projectile.localNPCHitCooldown = 30;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			/*target.StrikeNPC(projectile.damage, 0f, projectile.spriteDirection, false, true, true);
			target.netUpdate = true;*/
			//Dust.NewDust(target.position, target.width, target.height, DustID.Blood);
		}
		public override void AI()
        {
			//Player player = Main.player[projectile.owner];
			projectile.velocity.Y = projectile.velocity.Y + 0.4f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
			projectile.ai[0] += 1;
			if (projectile.ai[0] % 30 == 0)
			{
				foreach (NPC npc in Main.npc)
				{
					if (!npc.dontTakeDamage && !npc.friendly && !npc.townNPC)
					{
						Rectangle rec1 = new Rectangle((int)projectile.position.X, (int)projectile.Center.Y, projectile.width, (int)projectile.height);
						Rectangle rec2 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
						if (rec1.Intersects(rec2))
						{
							if (Main.netMode == NetmodeID.SinglePlayer)
							{
								npc.StrikeNPC(projectile.damage, 0f, npc.direction, false, true, true);
								npc.netUpdate = true;
							}
							else
                            {
								NetMessage.SendData(MessageID.StrikeNPC, number: npc.whoAmI, number2: projectile.damage, number3: 0f, number4: npc.direction, number5: 0);
								npc.netUpdate = true;
							}
							Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
						}
					}
				}
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity = Vector2.Zero;
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}
	}
	public class SuperSpikeTrap : SpikeTrap
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Spike Trap");
		}
		public override void SetDefaults()
        {
			base.SetDefaults();
			projectile.idStaticNPCHitCooldown = 15;
		}
		public override void AI()
		{
			projectile.velocity.Y = projectile.velocity.Y + 0.4f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
			projectile.ai[0] += 1;
			if (projectile.ai[0] % 15 == 0)
			{
				foreach (NPC npc in Main.npc)
				{
					if (!npc.dontTakeDamage && !npc.friendly && !npc.townNPC)
					{
						Rectangle rec1 = new Rectangle((int)projectile.position.X, (int)projectile.Center.Y, projectile.width, (int)projectile.height);
						Rectangle rec2 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
						if (rec1.Intersects(rec2))
						{
							if (Main.netMode == NetmodeID.SinglePlayer)
							{
								npc.StrikeNPC(projectile.damage, 0f, npc.direction, false, true, true);
								npc.netUpdate = true;
							}
							else
							{
								NetMessage.SendData(MessageID.StrikeNPC, number: npc.whoAmI, number2: projectile.damage, number3: 0f, number4: npc.direction, number5: 0);
								npc.netUpdate = true;
							}
							Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
						}
					}
				}
			}
		}
	}
	public class SlimeTrap : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slime Trap");
		}
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 8;
			projectile.ignoreWater = false;
			projectile.tileCollide = true;
			projectile.sentry = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.penetrate = -1;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 2;

			//projectile.usesLocalNPCImmunity = true;
			//projectile.localNPCHitCooldown = 30;
		}
		public override bool MinionContactDamage()
		{
			return false;
		}
		public override void AI()
		{
			projectile.velocity.Y = projectile.velocity.Y + 0.4f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
			projectile.ai[0] += 1;
			if (projectile.ai[0] % 2 == 0)
			{
				foreach (NPC npc in Main.npc)
				{
					if (!npc.dontTakeDamage && !npc.friendly && !npc.townNPC)
					{
						Rectangle rec1 = new Rectangle((int)projectile.position.X, (int)projectile.Center.Y, projectile.width, (int)projectile.height);
						Rectangle rec2 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
						if (rec1.Intersects(rec2))
						{
							if (Main.netMode == NetmodeID.SinglePlayer)
							{
								//npc.AddBuff(BuffID.Slow, 2);
								npc.velocity.X *= 0.75f;
								npc.AddBuff(BuffID.Slimed, 2);
								npc.netUpdate = true;
							}
							else
							{
								//NetMessage.SendData(MessageID.AddNPCBuff, number: BuffID.Slow, number2: 2);
								npc.velocity.X *= 0.75f;
								npc.AddBuff(BuffID.Slimed, 2);
								NetMessage.SendData(MessageID.SendNPCBuffs, number: BuffID.Slimed, number2: 2);
								npc.netUpdate = true;
							}
							if (projectile.ai[0] % 4 == 0)
							{
								Dust.NewDust(npc.position, npc.width, npc.height, DustID.t_Slime, npc.velocity.X, default, 200, Color.Blue);
							}
						}
					}
				}
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity = Vector2.Zero;
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}
	}
}