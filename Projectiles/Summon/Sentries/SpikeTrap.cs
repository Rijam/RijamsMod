using Microsoft.Xna.Framework;
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

			AI_CheckForHeavySling(Projectile);

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
								// NetMessage.SendData(MessageID.DamageNPC, number: npc.whoAmI, number2: damage);
								npc.StrikeNPC(hitInfo, false, true);
								NetMessage.SendStrikeNPC(npc, hitInfo);
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

		internal static void AI_CheckForHeavySling(Projectile projectile)
		{
			if (projectile.ai[2] == 0f && projectile.CanGetPickedUpBySentryBackpack())
			{
				projectile.ai[2] = 1f;
				projectile.netUpdate = true;
			}

			if (projectile.ai[2] == 1f)
			{
				projectile.SetAsSelectedSentryBackpackTarget();
				projectile.velocity = Vector2.Zero;
				projectile.tileCollide = false;
				AI_TrapSentries_HeavySlingReposition(projectile);
			}
		}

		internal static void AI_TrapSentries_HeavySlingReposition(Projectile projectile)
		{
			Player owner = Main.player[projectile.owner];
			if (!owner.accSentryBackpack)
			{
				projectile.Kill();
				return;
			}

			bool playerTransformationMount = owner.mount.Active && owner.mount.Type >= MountID.Rudolph && MountID.Sets.IsTransformationMount[owner.mount.Type];
			/*
			bool playerRidingMinecart = player.mount.Active && player.mount.AnyTrackRider;
			int minimumYOffset = Math.Min(21, player.height / 2);
			projectile.Center = player.MountedCenter + new Vector2(0f, player.gravDir * (0f - (minimumYOffset + 0 + (projectile.height / 2))));
			projectile.position.Y += player.gfxOffY;

			if (playerRidingMinecart)
			{
				projectile.Bottom = player.RotatedRelativePoint(projectile.Bottom);
			}

			projectile.position = projectile.position.Floor();
			*/

			int pushFromOriginHack = 0;
			Vector2 halfProjSize = projectile.Size / 2f;
			Vector2 sentryOffset = new(0, playerTransformationMount ? 0 : -27);

			if (owner.mount.Active && owner.mount.Type == MountID.PogoStick)
			{
				pushFromOriginHack = 12;
			}

			switch (owner.mount.Type)
			{
				default:
					{
						// The default location of the sentry will be near the player's head.
						projectile.Center = owner.MountedCenter + sentryOffset;

						// Add the additional height to the sentry to match the player's bobbing while walking.
						Vector2 playerWalkingBobbing = Main.OffsetsPlayerHeadgear[owner.bodyFrame.Y / owner.bodyFrame.Height] + new Vector2(0f, -2f);
						projectile.position += playerWalkingBobbing * new Vector2(0f, owner.gravDir);

						// Set the final position taking into account the player's rotation.
						projectile.position = owner.RotatedRelativePoint(projectile.position + halfProjSize, reverseRotation: false, addGfxOffY: true, pushFromOriginHack) - halfProjSize;

						// If desired, rotate the sentry to match the player's rotation.
						projectile.rotation = owner.fullRotation;
						break;
					}
				// For the transformation mounts, set the position of the sentry to be on the mount's head or back.
				case MountID.Rat:
					{
						projectile.Bottom = owner.Top;
						int ratYOffset = 10;
						int ratWalkingBobbing = 0;
						int mountFrame = owner.mount.Frame;
						if (mountFrame == 0 || (uint)(mountFrame - 11) <= 1u || (uint)(mountFrame - 15) <= 1u)
							ratWalkingBobbing = 2;

						projectile.position.Y += ratYOffset + ratWalkingBobbing + owner.gfxOffY;
						projectile.Bottom += sentryOffset;
						break;
					}
				case MountID.Pixie:
					{
						projectile.Bottom = owner.Top;
						int pixieYOffset = 10;
						projectile.position.Y += pixieYOffset;
						projectile.Bottom += sentryOffset;
						break;
					}
				case MountID.Bat:
					{
						if (owner.dash > 0 && owner.dashDelay < 0)
						{
							projectile.Bottom = owner.Top + new Vector2(4f * owner.Directions.X, 3f);
							projectile.Bottom += new Vector2(0f, -2f);
						}
						else
						{
							projectile.Bottom = (owner.Directions.X > 0f) ? (owner.Left + new Vector2(9f, 0f)) : (owner.Right + new Vector2(-10f, 0f));
						}

						int batYOffset = 2;
						int mountFrame = owner.mount.Frame;
						if ((uint)(mountFrame - 3) <= 2u)
							batYOffset = 0;

						projectile.Bottom += sentryOffset + new Vector2(0f, batYOffset);
						projectile.Bottom = owner.RotatedRelativePoint(projectile.Bottom);
						break;
					}
				case MountID.Velociraptor:
					{
						projectile.Bottom = owner.MountedTop + new Vector2(-12f * owner.Directions.X, 16f);
						projectile.Bottom += sentryOffset;
						projectile.Bottom = owner.RotatedRelativePoint(projectile.Bottom);
						break;
					}
				case MountID.Wolf:
					{
						projectile.Bottom = owner.MountedTop + new Vector2(-4f * owner.Directions.X, 10f);
						projectile.Bottom += sentryOffset;
						projectile.Bottom = owner.RotatedRelativePoint(projectile.Bottom);
						break;
					}
			}

			// Floor the position to snap it to tile coordinates.
			projectile.position = projectile.position.Floor();
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

			SpikeTrap.AI_CheckForHeavySling(Projectile);

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
								// NetMessage.SendData(MessageID.DamageNPC, number: npc.whoAmI, number2: damage);
								npc.StrikeNPC(hitInfo, false, true);
								NetMessage.SendStrikeNPC(npc, hitInfo);
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
			Projectile.idStaticNPCHitCooldown = 30; // 2;

			//projectile.usesLocalNPCImmunity = true;
			//projectile.localNPCHitCooldown = 30;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public override void AI()
		{
			Projectile.velocity.Y = Projectile.velocity.Y + 0.4f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
			Projectile.ai[0] += 1;

			SpikeTrap.AI_CheckForHeavySling(Projectile);

			if (Projectile.ai[0] % 2 == 0)
			{
				int damage = Main.DamageVar(Projectile.damage, Main.player[Projectile.owner].luck);
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
								if (Projectile.ai[0] % 30 == 0)
								{
									npc.StrikeNPC(hitInfo, false, true);
									Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
								}
								//npc.AddBuff(BuffID.Slow, 2);
								npc.velocity.X *= 0.75f;
								npc.AddBuff(BuffID.Slimed, 30);
								npc.netUpdate = true;
							}
							else
							{
								if (Projectile.ai[0] % 30 == 0)
								{
									// NetMessage.SendData(MessageID.DamageNPC, number: npc.whoAmI, number2: damage);
									npc.StrikeNPC(hitInfo, false, true);
									NetMessage.SendStrikeNPC(npc, hitInfo);
									npc.netUpdate = true;
									Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
								}
								//NetMessage.SendData(MessageID.AddNPCBuff, number: BuffID.Slow, number2: 2);
								npc.velocity.X *= 0.75f;
								npc.AddBuff(BuffID.Slimed, 30);
								NetMessage.SendData(MessageID.NPCBuffs, number: BuffID.Slimed, number2: 30);
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