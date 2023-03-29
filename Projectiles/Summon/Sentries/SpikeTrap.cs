using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Summon.Sentries
{
	public class SpikeTrap : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spike Trap");
		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 16;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.sentry = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 30;
			
			//projectile.usesLocalNPCImmunity = true;
			//projectile.localNPCHitCooldown = 30;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		
		public override void AI()
        {
			
			int damage = Main.DamageVar(Projectile.damage, Main.player[Projectile.owner].luck);
			//Player player = Main.player[projectile.owner];
			Projectile.velocity.Y = Projectile.velocity.Y + 0.4f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
			Projectile.ai[0] += 1;
			if (Projectile.ai[0] % 30 == 0)
			{
				foreach (NPC npc in Main.npc)
				{
					if (!npc.dontTakeDamage && !npc.friendly && !npc.townNPC && npc.active)
					{
						Rectangle rec1 = new((int)Projectile.position.X, (int)Projectile.Center.Y, Projectile.width, (int)Projectile.height);
						Rectangle rec2 = new((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
						if (rec1.Intersects(rec2))
						{
							NPC.HitInfo hitInfo = new()
							{
								Damage = damage,
								HitDirection = npc.direction,
								Knockback = 0f
							};
							if (Main.netMode == NetmodeID.SinglePlayer)
							{
								npc.StrikeNPC(hitInfo, false, true);
							}
							else
							{
								NetMessage.SendData(MessageID.DamageNPC, number: npc.whoAmI, number2: damage);
								npc.netUpdate = true;
							}
							Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
						}
					}
				}
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = Vector2.Zero;
			return false;
		}

		public override bool? CanDamage()
		{
			return false;
		}
	}
	public class SuperSpikeTrap : SpikeTrap
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Super Spike Trap");
		}
		public override void SetDefaults()
        {
			base.SetDefaults();
			Projectile.idStaticNPCHitCooldown = 15;
		}
		public override void AI()
		{
			int damage = Main.DamageVar(Projectile.damage, Main.player[Projectile.owner].luck);
			Projectile.velocity.Y = Projectile.velocity.Y + 0.4f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
			Projectile.ai[0] += 1;
			if (Projectile.ai[0] % 15 == 0)
			{
				foreach (NPC npc in Main.npc)
				{
					if (!npc.dontTakeDamage && !npc.friendly && !npc.townNPC && npc.active)
					{
						Rectangle rec1 = new((int)Projectile.position.X, (int)Projectile.Center.Y, Projectile.width, (int)Projectile.height);
						Rectangle rec2 = new((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
						if (rec1.Intersects(rec2))
						{
							NPC.HitInfo hitInfo = new()
							{
								Damage = damage,
								HitDirection = npc.direction,
								Knockback = 0f
							};
							if (Main.netMode == NetmodeID.SinglePlayer)
							{
								npc.StrikeNPC(hitInfo, false, true);
							}
							else
							{
								NetMessage.SendData(MessageID.DamageNPC, number: npc.whoAmI, number2: damage);
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
			// DisplayName.SetDefault("Slime Trap");
		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 8;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.sentry = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 2;

			//projectile.usesLocalNPCImmunity = true;
			//projectile.localNPCHitCooldown = 30;
		}
		public override bool MinionContactDamage()
		{
			return false;
		}
		public override void AI()
		{
			Projectile.velocity.Y = Projectile.velocity.Y + 0.4f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
			Projectile.ai[0] += 1;
			if (Projectile.ai[0] % 2 == 0)
			{
				foreach (NPC npc in Main.npc)
				{
					if (!npc.dontTakeDamage && !npc.friendly && !npc.townNPC && npc.active)
					{
						Rectangle rec1 = new((int)Projectile.position.X, (int)Projectile.Center.Y, Projectile.width, (int)Projectile.height);
						Rectangle rec2 = new((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
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
								NetMessage.SendData(MessageID.NPCBuffs, number: BuffID.Slimed, number2: 2);
								npc.netUpdate = true;
							}
							if (Projectile.ai[0] % 4 == 0)
							{
								Dust.NewDust(npc.position, npc.width, npc.height, DustID.t_Slime, npc.velocity.X, default, 200, Color.Blue);
							}
						}
					}
				}
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = Vector2.Zero;
			return false;
		}

		public override bool? CanDamage()
		{
			return false;
		}
	}
}